using CS102L_MP.Lib;
using CS102L_MP.Lib.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS102L_MP.Models
{
    class User : IComparable<User>
    {

        public User()
        {
            Posts = new LinkedStack<UserPost>();
            Communities = new AVLTree<Community>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public IStack<UserPost> Posts { get; }
        public ITree<Community> Communities { get; }
        public IEnumerable<User> FollowedUsers { get { return CommunityModel.GetInstance().Users.Neighbors(this).Select(e=>e.Item1); } }

        public int CompareTo(User other)
        {
            return this.Name.CompareTo(other.Name);
        }
    }
}
