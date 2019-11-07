using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MatrixEngine.Cocos2d;
using MatrixEngine.CocoStudio.GUI;
using MatrixEngine.Math;
using MatrixEngine.Engine;
using Thunder.Common;
using Thunder.GameLogic.Common;
using Thunder.Game;

namespace Thunder.GameLogic.GameSystem
{
    public class InfoShow
    {
        public  static Size offsetSize = new Size(100, 0);
        private static List<Window> oldWindows = new List<Window>(3);

        //isHaveD是否有底框；true有，默认false
        private static CCAction defaultAction(float time)
        {
            var fadeIn = new MCActionFadeIn(0);
            var scaleByStart = new CCActionScaleBy(0, 0);
            var spawn0 = new CCActionSpawn(fadeIn, scaleByStart);
            var scaleTo = new CCActionScaleTo(0.1f, 1);
            var delayTime = new CCActionDelayTime(time);
            var moveBy = new CCActionMoveBy(0.2f, new Vector2(0, 50));
            var fadeOut = new MCActionFadeOut(0.2f);
            var spawn = new CCActionSpawn(moveBy, fadeOut);
            var sequence = new CCActionSequence(spawn0, scaleTo, delayTime, spawn);
            return sequence;
        }

        private static CCAction MyActionZoom(float time)
        {
            var fadeIn = new MCActionFadeIn(0);
            var scaleByStart = new CCActionScaleBy(0, 0);
            var spawn0 = new CCActionSpawn(fadeIn, scaleByStart);
            var scaleTo = new CCActionScaleTo(0.1f, 1);
            var delayTime = new CCActionDelayTime(time);
            var fadeOut = new MCActionFadeOut(0.2f);
            var spawn = new CCActionSpawn(fadeOut);
            var sequence = new CCActionSequence(spawn0, scaleTo, delayTime, spawn);
            return sequence;
        }

        public static void AddInfo(object info)
        {
            AddInfo(info == null ? "null" : info.ToString());
        }
        public static void AddInfo(object info, bool isHaveD)
        {
            AddInfo(info == null ? "null" : info.ToString(), isHaveD);
        }

        public static void AddInfo(object info, Vector2 pos)
        {
            AddInfo(info == null ? "null" : info.ToString(), pos);
        }
        public static void AddInfo(object info, Vector2 pos, bool isHaveD)
        {
            AddInfo(info == null ? "null" : info.ToString(), pos, isHaveD);
        }

        public static void AddInfo(string info)
        {
            AddInfo(info, 1);
        }
        public static void AddInfo(string info, bool isHaveD)
        {
            AddInfo(info, 1, isHaveD);
        }

        public static void AddInfo(string info, Vector2 pos)
        {
            AddInfo(info, 1, pos);
        }

        public static void AddInfo(string info, Vector2 pos, bool isHaveD)
        {
            AddInfo(info, 1, pos, isHaveD);
        }

        public static void AddInfo(string info, float time)
        {
            AddInfo(info, time, Config.ScreenCenter);
        }
        public static void AddInfo(string info, float time, bool isHaveD)
        {
            AddInfo(info, time, Config.ScreenCenter, isHaveD);
        }

        public static void AddInfo(string info, float time, Vector2 pos)
        {
            AddInfo(info, pos, defaultAction(time));
        }
        public static void AddInfo(string info, float time, Vector2 pos, bool isHaveD)
        {
            AddInfo(info, pos, defaultAction(time), isHaveD);
        }
        public static UIImageView image = UIReader.GetWidget(ResID.UI_UI_Templates).GetWidget("msg_background") as UIImageView;
        public static void AddInfo(string info, Vector2 pos, CCAction action, bool isHaveD)
        {
            if (isHaveD)
            {
                action = MyActionZoom(1);
            }

            if (info == null || info.Length == 0)
            {
                return;
            }
            Console.WriteLine(info);

            Window infoWindow = new Window(Window.Priority.PRIORITY_INFO, AppMain.NotificationLayer.UILayout);
            if (isHaveD)
            {

                image.IsVisible = true;
                image.Postion = new Vector2(-3, 0);
            }


            infoWindow.TouchEnabled = false;

            if (info[0] == '<')
            {
                UIDocText doc = new InfoUIDocText();
                doc.SetDocText(info);
                infoWindow.AddChild(doc, 1);

                infoWindow.Size = doc.Size + offsetSize;
                if (isHaveD)
                {

                    image.SetSize(doc.Size + offsetSize);
                    image.Size += 12;
                }
            }
            else
            {
                //描边
                /*
                Color32 lineColor = new Color32(0, 0, 0);
                UILabel up = new UILabel();
                up.SetFontSize(25);
                up.SetText(info);
                up.PostionY += 2;
                up.Color = lineColor;
                infoWindow.AddChild(up, 1);

                UILabel down = new UILabel();
                down.SetFontSize(25);
                down.SetText(info);
                down.PostionY -= 2;
                down.Color = lineColor;
                infoWindow.AddChild(down, 1);

                UILabel left = new UILabel();
                left.SetFontSize(25);
                left.SetText(info);
                left.PostionX -= 2;
                left.Color = lineColor;
                infoWindow.AddChild(left, 1);

                UILabel right = new UILabel();
                right.SetFontSize(25);
                right.SetText(info);
                right.PostionX += 2;
                right.Color = lineColor;
                infoWindow.AddChild(right, 1);
                 */
                //描边

                UILabel label_str = new UILabel();
                label_str.SetFontSize(40);
                label_str.SetText(info);
                label_str.Color = new Color32(0, 218, 226);
                infoWindow.AddChild(label_str, 1);

                infoWindow.Size = label_str.ContextSize + offsetSize;
                if (isHaveD)
                {
                    image.SetSize(label_str.ContextSize + offsetSize);
                    image.Size += 12;

                }
            }

            infoWindow.Postion = pos;
            if (isHaveD)
            {
                infoWindow.AddChild(image, 0);
            }

            infoWindow.Show(true);

            //Console.WriteLine(label_str.ContextSize);

            //if (oldWindow != null && Utils.CollisionRect(oldWindow.Postion, oldWindow.ContextSize, infoWindow.Postion, infoWindow.ContextSize))
            //{
            //    ActionMoveBy moveBy = new ActionMoveBy(0.1f, new Vector2(0, infoWindow.GetSize().height));
            //    oldWindow.RunAction(moveBy);
            //}

            CCActionSequence sequence;
            CallFunc callBack = delegate(object[] args)
            {
                Window closeWin = (Window)args[0];
                closeWin.Show(false);
                oldWindows.Remove(closeWin);
            };
            CCActionCallFunc callFunc = new CCActionCallFunc(callBack, infoWindow);
            if (action != null)
            {
                sequence = new CCActionSequence(action, callFunc);
            }
            else
            {
                sequence = new CCActionSequence(defaultAction(1), callFunc);
            }
            infoWindow.RunAction(sequence);


            //判断是否超出范围了
            oldWindows.Insert(0, infoWindow);
            for (int i = oldWindows.Count - 1; i >= 3; i--)
            {
                Window window = oldWindows[i];
                window.StopAllAction();
                CCAction moveBy;
                if (isHaveD)
                {
                    moveBy = new CCActionMoveBy(0.2f, new Vector2(0, 0));
                }
                else
                {
                    moveBy = new CCActionMoveBy(0.2f, new Vector2(0, 50));
                }

                var fadeOut = new MCActionFadeOut(0.2f);
                var spawn = new CCActionSpawn(moveBy, fadeOut);
                callFunc = new CCActionCallFunc(callBack, window);
                var seq = new CCActionSequence(spawn, callFunc);
                window.RunAction(seq);
                oldWindows.Remove(window);
            }
            if (!isHaveD)
            {
                for (int i = 1; i < 3 && i < oldWindows.Count; i++)
                {
                    Window window = oldWindows[i];
                    if (Utils.CollisionRect(window.Postion, window.Size, infoWindow.Postion, infoWindow.Size))
                    {
                        CCActionMoveBy moveBy = new CCActionMoveBy(0.2f, new Vector2(0, window.GetSize().height));
                        window.RunAction(moveBy);
                    }
                }
            }
            else
            {
                for (int i = 1; i < 3 && i < oldWindows.Count; i++)
                {
                    Window window = oldWindows[i];
                    window.IsVisible = false;
                }
            }



            //oldWindow = infoWindow;
        }
        public static void AddInfo(string info, Vector2 pos, CCAction action)
        {
            bool isHaveD = true;
            if (isHaveD)
            {
                action = MyActionZoom(1);
            }

            if (info == null || info.Length == 0)
            {
                return;
            }
            Console.WriteLine(info);

            Window infoWindow = new Window(Window.Priority.PRIORITY_INFO, AppMain.NotificationLayer.UILayout);
            if (isHaveD)
            {

                image.IsVisible = true;
                image.Postion = new Vector2(-3, 0);
            }


            infoWindow.TouchEnabled = false;

            if (info[0] == '<')
            {
                UIDocText doc = new InfoUIDocText();
                doc.SetDocText(info);
                infoWindow.AddChild(doc, 1);

                infoWindow.Size = doc.Size + offsetSize;
                if (isHaveD)
                {

                    image.SetSize(doc.Size + offsetSize);
                    image.Size += 12;
                }
            }
            else
            {
                //描边
                /*
                Color32 lineColor = new Color32(0, 0, 0);
                UILabel up = new UILabel();
                up.SetFontSize(25);
                up.SetText(info);
                up.PostionY += 2;
                up.Color = lineColor;
                infoWindow.AddChild(up, 1);

                UILabel down = new UILabel();
                down.SetFontSize(25);
                down.SetText(info);
                down.PostionY -= 2;
                down.Color = lineColor;
                infoWindow.AddChild(down, 1);

                UILabel left = new UILabel();
                left.SetFontSize(25);
                left.SetText(info);
                left.PostionX -= 2;
                left.Color = lineColor;
                infoWindow.AddChild(left, 1);

                UILabel right = new UILabel();
                right.SetFontSize(25);
                right.SetText(info);
                right.PostionX += 2;
                right.Color = lineColor;
                infoWindow.AddChild(right, 1);
                */ 
                //描边

                UILabel label_str = new UILabel();
                label_str.SetFontSize(40);
                label_str.SetText(info);
                label_str.Color = new Color32(0, 218, 226);
                infoWindow.AddChild(label_str, 1);
                infoWindow.Size = label_str.ContextSize + 100;
                if (isHaveD)
                {
                    image.SetSize(label_str.ContextSize + 100);
                    image.Size += 12;

                }
            }

            infoWindow.Postion = pos;
            if (isHaveD)
            {
                infoWindow.AddChild(image, 0);
            }

            infoWindow.Show(true);

            //Console.WriteLine(label_str.ContextSize);

            //if (oldWindow != null && Utils.CollisionRect(oldWindow.Postion, oldWindow.ContextSize, infoWindow.Postion, infoWindow.ContextSize))
            //{
            //    ActionMoveBy moveBy = new ActionMoveBy(0.1f, new Vector2(0, infoWindow.GetSize().height));
            //    oldWindow.RunAction(moveBy);
            //}

            CCActionSequence sequence;
            CallFunc callBack = delegate(object[] args)
            {
                Window closeWin = (Window)args[0];
                closeWin.Show(false);
                oldWindows.Remove(closeWin);
            };
            CCActionCallFunc callFunc = new CCActionCallFunc(callBack, infoWindow);
            if (action != null)
            {
                sequence = new CCActionSequence(action, callFunc);
            }
            else
            {
                sequence = new CCActionSequence(defaultAction(1), callFunc);
            }
            infoWindow.RunAction(sequence);


            //判断是否超出范围了
            oldWindows.Insert(0, infoWindow);
            for (int i = oldWindows.Count - 1; i >= 3; i--)
            {
                Window window = oldWindows[i];
                window.StopAllAction();
                CCAction moveBy;
                if (isHaveD)
                {
                    moveBy = new CCActionMoveBy(0.2f, new Vector2(0, 0));
                }
                else
                {
                    moveBy = new CCActionMoveBy(0.2f, new Vector2(0, 50));
                }

                var fadeOut = new MCActionFadeOut(0.2f);
                var spawn = new CCActionSpawn(moveBy, fadeOut);
                callFunc = new CCActionCallFunc(callBack, window);
                var seq = new CCActionSequence(spawn, callFunc);
                window.RunAction(seq);
                oldWindows.Remove(window);
            }
            if (!isHaveD)
            {
                for (int i = 1; i < 3 && i < oldWindows.Count; i++)
                {
                    Window window = oldWindows[i];
                    if (Utils.CollisionRect(window.Postion, window.Size, infoWindow.Postion, infoWindow.Size))
                    {
                        CCActionMoveBy moveBy = new CCActionMoveBy(0.2f, new Vector2(0, window.GetSize().height));
                        window.RunAction(moveBy);
                    }
                }
            }
            else
            {
                for (int i = 1; i < 3 && i < oldWindows.Count; i++)
                {
                    Window window = oldWindows[i];
                    window.IsVisible = false;
                }
            }



            //oldWindow = infoWindow;
        }


    }

    class InfoUIDocText : UIDocText
    {

        public InfoUIDocText()
        {
            DefalutFontSize = 25;
        }

        protected override UILabel InitUILabel(XmlNode xmlNode)
        {
            UILabel label_str = new UILabel();
            return label_str;
        }
    }
}
