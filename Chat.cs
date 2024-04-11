using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dick
{
    public class Chat
    {
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

    }
}
