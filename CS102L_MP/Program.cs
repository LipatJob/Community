using CS102L_MP.Lib;
using CS102L_MP.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using System.Threading.Tasks;

namespace CS102L_MP
{
    class Program
    {
        static void Main(string[] args)
        {
            DataInitialization initialize = new DataInitialization();
            //initialize.Run();
            var logic = new CommunityLogic();
            //logic.serializer.Serialize();
            logic.serializer.Deserialize();
            var display = new CommunityDisplay(logic);
            display.WelcomeScreen();

            Console.ReadLine();
        }


        static void Test()
        {
            HashMap<string, string> map = new HashMap<string, string>();
            map["job"] = "lipat1";
            map["job1"] = "lipat2";
            map["job2"] = "lipat3";
            map["job3"] = "lipat4";
            map["job4"] = "lipat5";
            map["job5"] = "lipat6";
            map["job6"] = "lipat7";
            map["job7"] = "lipat8";
            map["job8"] = "lipat9";
            map["job9"] = "lipat10";
            map["job10"] = "lipat11";
            map["job11"] = "lipat12";

            Console.WriteLine(map["job10"]);
        }
    }
}
