using CS102L_MP.Lib.Concrete;
using CS102L_MP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS102L_MP
{
    class CommunityModel
    {
        // SINGLETON
        private static CommunityModel instance = null;
        public static CommunityModel GetInstance()
        {
            if (instance == null) { instance = new CommunityModel();  }
            return instance;
        }

        private CommunityModel()
        {
            Communities = new AVLTree<Community>();
            Users = new ListDirectedGraph<User>();
        }

        public AVLTree<Community> Communities;
        public ListDirectedGraph<User> Users;
        public User LoggedinUser;
    }
}
