using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace CS102L_MP.Models
{
    class UserPost
    {
        public int ID { get; set; }
        public DateTime DatePosted { get; set; }
        public string Text { get; set; }
        public User User { get; set; }
        public Community Community { get; set; }
    }
}
