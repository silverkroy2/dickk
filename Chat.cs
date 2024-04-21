using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dick
{
    public class Chat
    {
        // айди чата и список юзеров (он не берется сразу - только те кто вступает в игру)
        public long Id { get; set; }
        public List<User> users { get; set; }

        public Chat()
        {

        }

        public Chat(long id)
        {
            Id = id;
            users = new();
        }

        // средний член на чат
        public double ChatAverageDick()
        {
            double dick = users.Sum(x => x.Dick) / users.Count;
            return dick;
        }

        // топ 3 член на чат
        public int DickTop3()
        {
            if(users.Count > 3)
            {
                return users.OrderByDescending(x => x.Dick).ToList()[3].Dick;
            }
            return 0;
        }

        // средний анус на чат
        public double ChatAverageAnus()
        {
            double anus = users.Sum(x => x.Anus) / users.Count;
            return anus;
        }

        // топ 3 анус на чат
        public int AnusTop3()
        {
            if (users.Count > 3)
            {
                return users.OrderByDescending(x => x.Anus).ToList()[3].Anus;
            }
            return 0;
        }

    }
}
