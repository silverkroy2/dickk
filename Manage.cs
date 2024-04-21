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
        // список чатов
        public List<Chat> chats { get; set; }

        // при true бот обрабатывает только сообщения админа
        public bool Bot_Update { get; set; }

        // числа для выборки роста писька и ануса
        public List<int> Numbers { get; set; }
        public List<int> Numbers_For_anus { get; set; }

        public Manage()
        {
            // подгрузка данніх с файла
            if (System.IO.File.Exists(@"data\chats.json"))
            {
                chats = FileWorker.Deserialize<List<Chat>>(@"data\chats.json");
            }
            else
            {
                chats = new();
            }
            Bot_Update = true;
            Numbers = new();
            Numbers_For_anus = new();

            // заполняю определенным количеством чисел для нужных вероятностей выпадения
            for(int j = 0; j < 2; j++) { Numbers.Add(1); Numbers.Add(2); Numbers.Add(9); Numbers.Add(10); }
            for (int j = 0; j < 5; j++) { Numbers.Add(3); Numbers.Add(8); }
            for (int j = 0; j < 8; j++) { Numbers.Add(4); Numbers.Add(5); Numbers.Add(6); Numbers.Add(7); }

            Shuffle(Numbers);

            Numbers_For_anus.Add(1);
            for (int j = 0; j < 6; j++) { Numbers_For_anus.Add(2); Numbers_For_anus.Add(3); Numbers_For_anus.Add(4); }
            Numbers_For_anus.Add(5);

            Shuffle(Numbers_For_anus);


        }

        // новый чат
        public void AddChat(long id)
        {
            chats.Add(new Chat(id));
            return;
        }

        // новый пользователь
        public void AddUser(long chat_id, long user_id, string name, string username)
        {
            chats.Find(x => x.Id == chat_id).users.Add(new User(user_id, name, username));
            return;
        }

        // прием сообщений
        public string ReceiveMessage(Message message)
        {
            // на апдейте
            if (Bot_Update && message.From.Id != 612945638)
            {
                return "Вибачте, пісюн зараз спить! Спробуйте трошки пізніше!";
            }
            // ростить член
            if(message.Text.Contains("/dick"))
            {
                begin:
                if(chats.Exists(x=>x.Id == message.Chat.Id))
                {
                    var chat = chats.Find(x => x.Id == message.Chat.Id);
                    if (chat.users.Exists(x=>x.ID == message.From.Id))
                    {
                        var user = chats.Find(x => x.Id == message.Chat.Id).users.Find(x => x.ID == message.From.Id);
                        user.UserUpdate(message.From.FirstName, message.From.Username);
                        int answer = user.GrowDick(Numbers, chat.ChatAverageDick(), chat.DickTop3());
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
                            return $"{user.Name}, Ваш пісюн зменшився на {answer*-1} см.\nТепер його довжина: {user.Dick} см" +
                                $"\nВаш статус: <b>{user.Status}</b>";
                        }
                        else
                        {
                            return $"{user.Name}, Ваш пісюн збільшився на {answer} см.\nТепер його довжина: {user.Dick} см" +
                                $"\nВаш статус: <b>{user.Status}</b>" +
                                $"\n\n{user.UpdateMaxDicksAndDropJoke()}";
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

            // ростить анус
            else if (message.Text.Contains("/anus"))
            {
                begin:
                if (chats.Exists(x => x.Id == message.Chat.Id))
                {
                    var chat = chats.Find(x => x.Id == message.Chat.Id);
                    if (chat.users.Exists(x => x.ID == message.From.Id))
                    {
                        var user = chats.Find(x => x.Id == message.Chat.Id).users.Find(x => x.ID == message.From.Id);
                        user.UserUpdate(message.From.FirstName, message.From.Username);
                        int answer = user.GrowAnus(Numbers_For_anus, chat.ChatAverageAnus(), chat.AnusTop3());
                        if (answer == 0)
                        {
                            return "Ви сьогодні вже грали";
                        }
                        else if (answer == 99)
                        {
                            return "Сталася помилка((((";
                        }
                        else if (answer < 0)
                        {
                            return $"{user.Name}, Ваш анус звузився на {answer * -1} см.\nТепер його діаметр: {user.Anus} см" +
                                $"\nВаш статус: <b>{user.Anus_Status}</b>";
                        }
                        else
                        {
                            return $"{user.Name}, Ваш анус розширився на {answer} см.\nТепер його діаметр: {user.Anus} см" +
                                $"\nВаш статус: <b>{user.Anus_Status}</b>" +
                                $"\n\n{user.UpdateMaxAnusAndDropJoke()}";
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

            // топ 10 жоп
            else if (message.Text.Contains("/atop10"))
            {
                if (chats.Exists(x => x.Id == message.Chat.Id))
                {
                    var users = chats.Find(x => x.Id == message.Chat.Id).users.OrderByDescending(x => x.Anus).ToList();

                    if (users.Count > 0)
                    {
                        string answer = "Топ 10 анусів:\n\n";
                        if (users.Count < 10)
                        {
                            for (int i = 0; i < users.Count; i++)
                            {
                                answer += $"{i + 1}. {users[i].Name} - {users[i].Anus} см\n";
                            }
                            return answer;
                        }
                        else
                        {
                            for (int i = 0; i < 10; i++)
                            {
                                answer += $"{i + 1}. {users[i].Name} - {users[i].Anus} см\n";
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

            // рейтинг жоп
            else if (message.Text.Contains("/atop"))
            {
                if (chats.Exists(x => x.Id == message.Chat.Id))
                {
                    var users = chats.Find(x => x.Id == message.Chat.Id).users.OrderByDescending(x => x.Anus).ToList();
                    if (users.Count > 0)
                    {
                        string answer = "Рейтинг гравців:\n\n";
                        for (int i = 0; i < users.Count; i++)
                        {
                            answer += $"{i + 1}. {users[i].Name} - {users[i].Anus} см\n";
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

            // топ 10 членов
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

            // рейтинг членов
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

            // правила игры
            else if (message.Text.Contains("/rules"))
            {
                return "Гра вирости пісюн та розширь анус\n" +
                    "В кожну гру можна зіграти кожного дня один раз за командами /dick та /anus\n" +
                    "Піська збільшиться/зменшиться на випадкову величину в межах 1-10\n" +
                    "Анус розшириться/звузиться на випадкову величину в межах 1-5\n" +
                    "Рейтинг: /top або /top10\n" +
                    "Рейтинг дуп: /atop або /atop10\n";
            }

            // хуйня посмотреть все чаты где есть бот и никнеймы юзеров
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
                        answer += $"/admin{chats[i].Id*-1}\n";
                    }
                    return answer;
                }
                else if(text.Contains("/admin"))
                {
                    long.TryParse(text.Replace("/admin", ""), out long result);
                    result *= -1;
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

            // включить/выключить режим обновы
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

            // ручная обнова разрешения играть на случай если комп в 4 утра был отключен 
            else if (message.Text.Contains("/daily_update"))
            {
                if (message.From.Id != 612945638)
                {
                    return "Нюхай бебру. Ти не адмін";
                }
                else
                {
                    DataUpdate();
                    return "Ранкове оновлення ігр виконано!";
                }
            }

            // все новое что нужно аккуратно впихнуть к старым характеристикам чтобы не было конфликтов
            else if (message.Text.Contains("/push"))
            {
                if (message.From.Id != 612945638)
                {
                    return "Нюхай бебру. Ти не адмін";
                }
                else
                {
                    Bot_Update = true;
                    for (int i = 0; i < chats.Count; i++)
                    {
                        var users = chats[i].users;
                        for (int j = 0; j < users.Count; j++)
                        {
                            users[j].Chance_Dick_Plus = 65;
                            users[j].Chance_Anus_Plus = 65;
                            if(users[j].Max_Dicks != null) { users[j].Max_Dicks.RemoveAll(x => x == 0); }
                            if (users[j].Max_Anus != null) { users[j].Max_Anus.RemoveAll(x => x == 0); }
                        }
                    }
                    SaveAll();
                    return "Нові записи успішно внесені!";
                }
            }

            // все остальные сообщения
            else
            {
                return "Щось пішло не так";
            }
        }

        // обновление возможности играть и другого
        public Task DataUpdate()
        {
            Bot_Update = true;
            for(int i = 0; i < chats.Count; i++)
            {
                for(int j = 0; j < chats[i].users.Count; j++)
                {
                    chats[i].users[j].PlayToday = false;
                    chats[i].users[j].Anus_Play = false;
                }
            }
            Shuffle(Numbers);
            Shuffle(Numbers_For_anus);
            SaveAll();
            Bot_Update = false;
            return Task.CompletedTask;
        }

        
        // сохранение данных
        public Task SaveAll()
        {
            FileWorker.Serialize<List<Chat>>(chats, @$"data\chats.json");
            return Task.CompletedTask;
        }

        // перемешивание чисел в списке
        public static void Shuffle<T>(List<T> list)
        {
            Random rand = new Random();

            for (int i = list.Count - 1; i >= 1; i--)
            {
                int j = rand.Next(i + 1);

                T tmp = list[j];
                list[j] = list[i];
                list[i] = tmp;
            }
        }
    }
}
