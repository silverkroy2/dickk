using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace dick
{
    public class Manage
    {
        public List<Chat> chats { get; set; }
        public List<char> plus_or_minus { get; set; }
        public bool Bot_Update { get; set; }

        public Manage()
        {
            if (System.IO.File.Exists(@"data\chats.json"))
            {
                chats = FileWorker.Deserialize<List<Chat>>(@"data\chats.json");
            }
            else
            {
                chats = new();
            }
            Bot_Update = false;
            plus_or_minus = new();
            for (int i = 0; i < 200; i++)
            {
                plus_or_minus.Add('+');
                plus_or_minus.Add('+');
                plus_or_minus.Add('+');
                plus_or_minus.Add('-');
                plus_or_minus.Add('-');
            }
        }

        public void AddChat(long id)
        {
            chats.Add(new Chat(id));
            return;
        }

        public void AddUser(long chat_id, long user_id, string name, string username)
        {
            chats.Find(x => x.Id == chat_id).users.Add(new User(user_id, name, username));
            return;
        }

        public string ReceiveMessage(Message message)
        {
            if (Bot_Update && message.From.Id != 612945638)
            {
                return "Вибачте, пісюн зараз спить! Спробуйте трошки пізніше!";
            }
            if(message.Text.Contains("/dick"))
            {
                begin:
                if(chats.Exists(x=>x.Id == message.Chat.Id))
                {
                    if(chats.Find(x=>x.Id == message.Chat.Id).users.Exists(x=>x.ID == message.From.Id))
                    {
                        var user = chats.Find(x => x.Id == message.Chat.Id).users.Find(x => x.ID == message.From.Id);
                        int answer = user.GrowDick(plus_or_minus);
                        if(answer == 0)
                        {
                            return "Ви сьогодні вже грали";
                        }
                        else if(answer == 99)
                        {
                            return "Сталася помилка((((";
                        }
                        else if(answer < 0)
                        {
                            return $"Ваш пісюн зменшився на {answer*-1} см.\nТепер його довжина: {user.Dick} см";
                        }
                        else
                        {
                            return $"Ваш пісюн збільшився на {answer} см.\nТепер його довжина: {user.Dick} см";
                        }
                    }
                    else
                    {
                        AddUser(message.Chat.Id, message.From.Id, message.From.FirstName, message.From.Username);
                        goto begin;
                    }
                }
                else
                {
                    AddChat(message.Chat.Id);
                    goto begin;
                }
            }
            else if (message.Text.Contains("/top10"))
            {
                if(chats.Exists(x => x.Id == message.Chat.Id))
                {
                    var users = chats.Find(x => x.Id == message.Chat.Id).users.OrderByDescending(x => x.Dick).ToList();
                    
                    if(users.Count > 0)
                    {
                        string answer = "Топ 10 песюнів:\n\n";
                        if(users.Count < 10)
                        {
                            for(int i = 0; i < users.Count; i++)
                            {
                                answer += $"{i + 1}. {users[i].Name} - {users[i].Dick} см\n";
                            }
                            return answer;
                        }
                        else
                        {
                            for (int i = 0; i < 10; i++)
                            {
                                answer += $"{i + 1}. {users[i].Name} - {users[i].Dick} см\n";
                            }
                            return answer;
                        }
                    }
                    else
                    {
                        return "У цьому чаті ще ніхто не грав";
                    }
                }
                else
                {
                    return "У цьому чаті ще ніхто не грав";
                }
            }
            else if (message.Text.Contains("/top"))
            {
                if (chats.Exists(x => x.Id == message.Chat.Id))
                {
                    var users = chats.Find(x => x.Id == message.Chat.Id).users.OrderByDescending(x => x.Dick).ToList();
                    if (users.Count > 0)
                    {
                        string answer = "Рейтинг гравців:\n\n";
                        for (int i = 0; i < users.Count; i++)
                        {
                            answer += $"{i + 1}. {users[i].Name} - {users[i].Dick} см\n";
                        }
                        return answer;
                    }
                    else
                    {
                        return "У цьому чаті ще ніхто не грав";
                    }
                }
                else
                {
                    return "У цьому чаті ще ніхто не грав";
                }
            }
            else if (message.Text.Contains("/rules"))
            {
                return "Гра вирости пісюн\n" +
                    "В гру можна зіграти кожного дня один раз за командою /dick\n" +
                    "Піська збільшиться/зменшиться на випадкову величину в межах 1-10\n" +
                    "Рейтинг: /top або /top10\n";
            }
            else if (message.Text.Contains("/admin"))
            {
                var text = message.Text.Replace("@grow_penis_bot", "");
                if(message.From.Id != 612945638)
                {
                    return "Нюхай бебру. Ти не адмін";
                }
                else if(text == "/admin")
                {
                    string answer = "Чати: \n\n";
                    for(int i = 0; i < chats.Count; i++)
                    {
                        answer += $"/admin_{chats[i].Id}\n";
                    }
                    return answer;
                }
                else if(text.Contains("/admin_"))
                {
                    long.TryParse(text.Replace("/admin_", ""), out long result);
                    if(chats.Exists(x => x.Id == result))
                    {
                        var users = chats.Find(x => x.Id == result).users;
                        string answer = "Юзери: \n\n";
                        for (int i = 0; i < users.Count; i++)
                        {
                            answer += $"{i + 1}. @{users[i].Username}\n";
                        }
                        return answer;
                    }
                    
                    return "Щось пішло не так";
                }
                return "Щось пішло не так";
            }
            else if (message.Text.Contains("/begin_update"))
            {
                if (message.From.Id != 612945638)
                {
                    return "Нюхай бебру. Ти не адмін";
                }
                else
                {
                    if (Bot_Update)
                    {
                        Bot_Update = false;
                        return "Оновлення бота відключено!";
                    }
                    else
                    {
                        Bot_Update = true;
                        return "Оновлення бота увімкнено!";
                    }
                }
            }
            else
            {
                return "Щось пішло не так";
            }
        }

        public Task DataUpdate()
        {
            Bot_Update = true;
            for(int i = 0; i < chats.Count; i++)
            {
                for(int j = 0; j < chats[i].users.Count; j++)
                {
                    chats[i].users[j].PlayToday = false;
                }
            }
            SaveAll();
            Bot_Update = false;
            return Task.CompletedTask;
        }

        public Task SaveAll()
        {
            FileWorker.Serialize<List<Chat>>(chats, @$"data\chats.json");
            return Task.CompletedTask;
        }
    }
}
