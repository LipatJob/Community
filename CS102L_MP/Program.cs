using CS102L_MP.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS102L_MP
{
    class Program
    {
        static void Main(string[] args)
        {
            DataInitialization initialize = new DataInitialization();
            initialize.Run();
            var logic = new CommunityLogic();
            var display = new CommunityDisplay(logic);
            display.MainMenu();
        }
    }
}
