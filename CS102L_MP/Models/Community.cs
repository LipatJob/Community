using CS102L_MP.Lib;
using CS102L_MP.Lib.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS102L_MP.Models
{
    class Community : IComparable<Community>
    {
        public Community()
        {
            Posts = new LinkedStack<UserPost>();
        }
        public int Id { get; set; }
        public string Name { get; set; }

        public IStack<UserPost> Posts { get; }

        public int CompareTo(Community other)
        {
            return this.Name.CompareTo(other.Name);
        }
    }
}
