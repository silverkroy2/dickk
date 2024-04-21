using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dick
{
    public class User
    {
        // ID имя и никнейм в телеге
        public long ID { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        
        // размер члена, играл ли сегодня, статус, какого размера члена достигал за всю игру, шанс роста члена
        public int Dick { get; set; }
        public bool PlayToday { get; set; }
        public string Status { get; set; }
        public List<int> Max_Dicks { get; set; }
        public int Chance_Dick_Plus { get; set; }

        // размер ануса, играл ли сегодня, статус, какого размера ануса достигал за всю игру, шанс роста ануса
        public int Anus { get; set; }
        public bool Anus_Play { get; set; }
        public string Anus_Status { get; set; }
        public List<int> Max_Anus { get; set; }
        public int Chance_Anus_Plus { get; set; }

        public User()
        {
            
        }

        
        public User(long id, string name, string username)
        {
            ID = id;
            Name = name;
            Username = username;
            Dick = 0;
            Anus = 0;
            PlayToday = false;
            Anus_Play = false;
            Status = TextManager.Status(Dick);
            Status = TextManager.AnusStatus(Anus);
            Max_Dicks = new();
            Max_Anus = new();
            Chance_Dick_Plus = 70;
            Chance_Anus_Plus = 70;
        }

        // обновление имени и никнейма юзера
        public void UserUpdate(string name, string username)
        {
            Name = name;
            Username = username;
        }

        // растим член функция возвращает на сколько см вырос член
        public int GrowDick(List<int> numbers, double average_dick, int top3_dick)
        {
            try
            {
                Random random = new();
                if (PlayToday /*&& ID != 612945638*/)
                {
                    return 0;
                }

                // случайная цифра из заранее готового списка
                int val = numbers[random.Next(0, numbers.Count)];

                // если размер меньше среднего члена на весь чат - шанс роста +10
                if(Dick < average_dick)
                {
                    Chance_Dick_Plus += 10;
                }
                // если размер меньше чем топ 3 чата - шанс роста +5
                else if(Dick < top3_dick)
                {
                    Chance_Dick_Plus += 5;
                }

                // считаем шанс (от 1 до 100)
                int chance = random.Next(1, 101);

                // член растет всегда если он меньше 11 см
                if (Dick < 11)
                {
                    Dick += val;
                    Chance_Dick_Plus = 70;
                }
                // если выпавшее число больше чем шанс члена - значит число попало в промежуток уменьшения члена - уменьшаем
                else if (chance > Chance_Dick_Plus)
                {
                    Dick -= val;
                    val *= -1;
                    Chance_Dick_Plus += 5;
                }
                // противоположно прошлому коментарию - ростим член - случайное число попало в промежуток роста
                else if(chance <= Chance_Dick_Plus)
                {
                    Dick += val;
                    Chance_Dick_Plus = 70;
                }

                // играли
                PlayToday = true;

                // обновили статус
                Status = TextManager.Status(Dick);
                return val;
            }
            catch
            {
                return 99;
            }
            
        }

        // функция аналогична функции члена - читать коментарии там
        public int GrowAnus(List<int> numbers, double average_anus, int top3_anus)
        {
            try
            {
                Random random = new();
                if (Anus_Play /*&& ID != 612945638*/)
                {
                    return 0;
                }
                
                int val = numbers[random.Next(0, numbers.Count)];

                if (Dick < average_anus)
                {
                    Chance_Anus_Plus += 10;
                }
                else if (Dick < top3_anus)
                {
                    Chance_Anus_Plus += 5;
                }

                int chance = random.Next(1, 101);
                if (Anus < 6)
                {
                    Anus += val;
                    Chance_Anus_Plus = 70;
                }
                else if (chance > Chance_Anus_Plus)
                {
                    Anus -= val;
                    val *= -1;
                    Chance_Anus_Plus += 5;
                }
                else if (chance <= Chance_Anus_Plus)
                {
                    Anus += val;
                    Chance_Anus_Plus += 70;
                }

                Anus_Play = true;
                Anus_Status = TextManager.AnusStatus(Anus);
                return val;
            }
            catch
            {
                return 99;
            }

        }

        // при достижении члена определенного числа впервые - отправляем заготовленую шутку от ИИ - запоминаем это чтобы потом повторно не отправлять шутку
        public string UpdateMaxDicksAndDropJoke()
        {
            if(Max_Dicks == null)
            {
                Max_Dicks = new();
            }

            int val = 0;

            if(Dick >= 30 && Dick < 50 && !Max_Dicks.Exists(x => x == 30))
            {
                val = 30;
            }
            if(Dick >= 50 && Dick < 100 && !Max_Dicks.Exists(x=> x == 50))
            {
                val = 50;
            }
            if (Dick >= 100 && Dick < 200 && !Max_Dicks.Exists(x => x == 100))
            {
                val = 100;
            }
            if (Dick >= 200 && Dick < 300 && !Max_Dicks.Exists(x => x == 200))
            {
                val = 200;
            }
            if (Dick >= 300 && Dick < 400 && !Max_Dicks.Exists(x => x == 300))
            {
                val = 300;
            }
            if (Dick >= 400 && Dick < 500 && !Max_Dicks.Exists(x => x == 400))
            {
                val = 400;
            }
            if (Dick >= 500 && Dick < 600 && !Max_Dicks.Exists(x => x == 500))
            {
                val = 500;
            }

            if (val != 0)
            {
                Max_Dicks.Add(val);
            }
            return TextManager.Joke(val);
        }

        // аналог вышеописаной функции только для ануса
        public string UpdateMaxAnusAndDropJoke()
        {
            if (Max_Anus == null)
            {
                Max_Anus = new();
            }

            int val = 0;

            if (Anus >= 7 && Anus < 25 && !Max_Anus.Exists(x => x == 7))
            {
                val = 7;
            }
            if (Anus >= 25 && Anus < 50 && !Max_Anus.Exists(x => x == 25))
            {
                val = 25;
            }
            if (Anus >= 50 && Anus < 100 && !Max_Anus.Exists(x => x == 50))
            {
                val = 50;
            }
            if (Anus >= 100 && Anus < 150 && !Max_Anus.Exists(x => x == 100))
            {
                val = 100;
            }
            if (Anus >= 150 && Anus < 200 && !Max_Anus.Exists(x => x == 150))
            {
                val = 150;
            }
            if (Anus >= 200 && Anus < 250 && !Max_Anus.Exists(x => x == 200))
            {
                val = 200;
            }
            if (Anus >= 250 && Anus < 300 && !Max_Anus.Exists(x => x == 250))
            {
                val = 250;
            }

            if (val != 0)
            {
                Max_Anus.Add(val);
            }
            return TextManager.AnusJoke(val);
        }

    }
}
