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

            bool initializeFlag = false;


            DataInitialization initialize = new DataInitialization();
            var logic = new CommunityLogic();

            if(initializeFlag)
            {
                logic.serializer.RestartData();
                initialize.Run();
                logic.serializer.Serialize();
                Console.WriteLine("Data Initialized");
            }
            
            
            logic.serializer.Deserialize();
            var display = new CommunityDisplay(logic);
            display.WelcomeScreen();

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
