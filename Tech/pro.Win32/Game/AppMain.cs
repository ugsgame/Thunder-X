using Thunder.Common;
using MatrixEngine.Cocos2d;
using MatrixEngine.CocosDenshion;
using MatrixEngine.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thunder.GameLogic.Gaming;
using MatrixEngine.Engine;
using Thunder.GameLogic.UI;
using Thunder.GameLogic.Common;
using Thunder.GameBilling;
using MatrixEngine.Platform.Android;

namespace Thunder.Game
{
    public class AppMain
    {

        public static NotificationLayer NotificationLayer = new NotificationLayer();

        static AppMain()
        {
#if DEBUG
            Console.SetOut(new _ConsloeWrite(false));
            Console.SetError(new _ConsloeWrite(true));
#else
//             Console.SetOut(System.IO.TextWriter.Null);
//             Console.SetError(System.IO.TextWriter.Null);
            Console.SetOut(new _ConsloeWrite(false));
            Console.SetError(new _ConsloeWrite(true));
#endif
        }

        public static void OnEnterBackground()
        {
            AudioEngine.PauseBackgroundMusic();
        }

        public static void OnEnterForeground()
        {
            AudioEngine.ResumeBackgroundMusic();
        }

        public static void StartApp()
        {
            //适配屏幕
            Size frameSize = CCDirector.GetFrameSize();
            Config.ScreenFrameSize = frameSize;
            if (frameSize != Config.ScreenSize)
            {
                Size size = Config.ScreenSize;
                Size rate = Config.ScreenSize / frameSize;
                if (rate.width < rate.height)
                {
                     size.width = frameSize.width * rate.height;
                }
                else
                {
                     size.height = frameSize.height * rate.width;
                }
                CCDirector.SetResolutionSize(size, ResolutionPolicy.kResolutionExactFit);

                Config.SCREEN_WIDTH = (int)size.width;
                Config.SCREEN_HEIGHT = (int)size.height;

                Config.GAME_HEIGHT = Config.SCREEN_HEIGHT;
            }

            Config.SCREEN_RATE.X = (float)Config.SCREEN_WIDTH / Config.GAME_WIDTH;
            Config.SCREEN_RATE.Y = (float)Config.SCREEN_HEIGHT / Config.GAME_HEIGHT;

            CCDirector.SetNotificationLayer(NotificationLayer);
            //NotificationLayer.AddChild(SystemInfoLayer.Instance);
            SystemInfoLayer.Instance.PostionY = Config.SCREEN_HEIGHT - 200;
            //CCDirector.SetDisplayFPS(false);

            if (MatrixEngine.Engine.System.TARGET_PLATFORM == MatrixEngine.Engine.System.PLATFORM.WIN32)
            {
                BillingHelper.SetOperatorID(BillingHelper.Operators.Test);
            }
            else if (MatrixEngine.Engine.System.TARGET_PLATFORM == MatrixEngine.Engine.System.PLATFORM.ANDROID)
            {;
                AndroidJavaObject JavaBillingHelper = new AndroidJavaObject("org/operator/OperatorHelper");
                int id = JavaBillingHelper.CallStatic<int>("getOperator");
                Console.WriteLine("Operators:"+((BillingHelper.Operators)id));
                BillingHelper.SetOperatorID((BillingHelper.Operators)id);
            }

            
            Function.InitGameWindows();

            Function.GoTo(UIFunction.初始化);
            /*
            if (EDebug.InEditor)
            {
                Function.GoTo(UIFunction.游戏中);
            }
            else
            {
                Function.GoTo(UIFunction.主菜单);
            }
            */
        }

    }

    class _ConsloeWrite : System.IO.TextWriter
    {
        private static readonly object Lock = new object();
        private bool isError;

        public override Encoding Encoding
        {
            get
            {
                return Encoding.Unicode;
            }
        }

        public _ConsloeWrite(bool isError)
        {
            this.isError = isError;
        }

        public override void Write(string value)
        {
            lock (Lock)
            {
                MatrixEngine.Debug.Log(value, false);
            }
        }

        public override void Write(char value)
        {
            Write(new string(value, 1));
        }

        public override void Write(char[] buffer, int index, int count)
        {
            Write(new string(buffer, index, count));
        }

        public override void WriteLine()
        {
            lock (Lock)
            {
                MatrixEngine.Debug.Log("", true);
            }
        }

        public override void WriteLine(string value)
        {
            lock (Lock)
            {
                MatrixEngine.Debug.Log(value, true);
            }
        }
    }
}
