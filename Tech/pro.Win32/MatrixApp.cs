using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MatrixEngine;
using MatrixEngine.Math;
using MatrixEngine.Cocos2d;

using Thunder.Common;
using Thunder.Game;

namespace Game
{
    public class MatrixApp
    {
        private static void OnEnterBackground()
        {
            AppMain.OnEnterBackground();
        }

        private static void OnEnterForeground()
        {
            AppMain.OnEnterForeground();
        }
        private static int StartApp(string[] args)
        {
            try
            {
                foreach (var item in args)
                {
                    Console.WriteLine(item);
                }
                if (args.Length >=2 && args[1] != null && args[1] == "debug")
                {
                    string debugConfig = CCFileUtils.GetFileDataToString("Config/debug.cf");
                    EDebug.UnserializeJson(debugConfig);
                }

                AppMain.StartApp();
                Console.WriteLine("StartApp");

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine(e.StackTrace);
                Console.ReadLine();
            }
            return 1;
        }
    }
}
