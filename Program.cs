using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using System.Diagnostics;
using System.IO;
using Telegram.Bot.Types.Payments;
using System.Text;
using System.Linq;
using System.Threading;


namespace dick
{
    public static class Program
    {
       
        private static ITelegramBotClient _botClient;


        private static readonly string _telegramApi = "6242343914:AAEWK67YEb7fngOTlhxs5UEh1tpgu7hdCOY";  // ГАЧА БОТ
        //private static readonly string _telegramApi = "7065692826:AAHDB7nzs5WiMxaoiRm8tFiCQc5bb1kvY48";  // БОТ ПИСЬКИ

        private static async Task Main()
        {
            // есть ли папка с данными
            if (!Directory.Exists(@"data\"))
            {
                Directory.CreateDirectory(@"data\");
            }

            // инициализация класса менеджера
            Manage manage = new();
            

            // инициализация бота
            _botClient = new TelegramBotClient(_telegramApi);//use telegram api

            CancellationTokenSource cts = new();

            ReceiverOptions receiverOptions = new()
            {
                AllowedUpdates = Array.Empty<UpdateType>() // receive all update types except ChatMember related updates
            };

            _botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token
            );
            var me = await _botClient.GetMeAsync();

            // самая конченая реализация цикла для проверки времени суток и когда обновлять бота
            while (true)
            {
                if (DateTime.Now.Hour == 4 && DateTime.Now.Minute == 0)
                {
                    await manage.DataUpdate();
                    await Task.Delay(60000);
                }
                await Task.Delay(1000);
            }

            // прием сообщений от бота
            async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
            {
                // Only process Message updates: https://core.telegram.org/bots/api#message
                if (update.Message is not { } message)
                    return;
                // Only process text messages
                if (message.Text is not { } messageText)
                    return;

                // благодаря этому бот работает только в групах а не в личных чатах
                if (message.Chat.Id < 0 && message.Text.Contains("/"))
                {
                    try
                    {
                        // Echo received message text
                        Message sentMessage = await botClient.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            text: manage.ReceiveMessage(message),
                            cancellationToken: cancellationToken,
                            replyToMessageId: message.MessageId,
                            parseMode: ParseMode.Html);
                        await manage.SaveAll();
                    }
                    catch (Exception)
                    {

                    }
                    
                }
                

                
            }

            // обработчик ошибок бота?
            Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
            {
                var ErrorMessage = exception switch
                {
                    ApiRequestException apiRequestException
                        => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                    _ => exception.ToString()
                };

                Console.WriteLine(ErrorMessage);
                return Task.CompletedTask;
            }


            //
            

        }


        





    }
}
