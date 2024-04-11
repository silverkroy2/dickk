using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dick
{
    public class User
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public int Dick { get; set; }
        public bool PlayToday { get; set; }

        public User()
        {

        }

        public User(long id, string name, string username)
        {
            ID = id;
            Name = name;
            Username = username;
            Dick = 0;
            PlayToday = false;
        }

        public int GrowDick(List<char> plus_or_minus)
        {
            Random random = new();
            if (PlayToday/* && ID != 612945638*/)
            {
                return 0;
            }
            int val = random.Next(0, plus_or_minus.Count);
            char p_or_m = plus_or_minus[val];
            if(Dick < 11)
            {
                val = random.Next(1, 11);
                Dick += val;
                PlayToday = true;
                return val;
            }
            else if(p_or_m == '-')
            {
                val = random.Next(1, 11);
                Dick -= val;
                PlayToday = true;
                return val*(-1);
            }
            else if(p_or_m == '+')
            {
                val = random.Next(1, 11);
                Dick += val;
                PlayToday = true;
                return val;
            }

            return 99;
        }

    }
}
