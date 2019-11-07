using MatrixEngine.Cocos2d;
using MatrixEngine.CocoStudio.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thunder.GameLogic.Common
{
    public class NotificationLayer : CCLayer
    {
        public readonly UILayer UILayer = new UILayer();
        public readonly UILayout UILayout = new UILayout();




        private static uint gcCount = 1;
        /// <summary>
        /// 和类型有关的更新，只要是此类型的对象都会拥有此更新
        /// </summary>
        private static List<IRunnable> commonRuns = new List<IRunnable>();
        private static IRunnable[] updateRunnable = new IRunnable[0];




        public NotificationLayer()
        {
            UILayer.AddWidget(UILayout);
            this.AddChild(UILayer);
        }

        public override void OnUpdate(float dTime)
        {
            try
            {
                foreach (IRunnable run in updateRunnable)
                {
                    run.OnUpdate(dTime);
                }
                //                ScriptManager.GC();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
                Console.Error.WriteLine(e.StackTrace);
            }
            //10秒回收一次垃圾
            //if (gcCount++ % (60 * 5) == 0)
            //{
            //    //CCDirector.RunWithScene(scene);
            //    Console.WriteLine("gcCount=" + gcCount);
            //}
        }
        //CCScene scene = new CCScene();
        public static void AddCommonRunnable(IRunnable run)
        {
            if (!commonRuns.Contains(run))
            {
                commonRuns.Add(run);
                updateRunnable = commonRuns.ToArray();
            }
        }

        public static void RemoveCommonRunnable(IRunnable run)
        {
            Console.WriteLine("RemoveCommonRunnable=" + run);
            if (run == null)
            {
                return;
            }
            commonRuns.Remove(run);
            updateRunnable = commonRuns.ToArray();
        }
    }
}
