
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MatrixEngine.Cocos2d;
using MatrixEngine.CocoStudio.GUI;

namespace Thunder.GameLogic.GameSystem
{
    public class GameWindow : ScreenSizeWindow
    {
        private UIWidget uiImage;
        private bool isInitDefaultConfig;


        protected float effectTime = 0.25f;

        public GameWindow()
            : this(Priority.PRIORITY_WINDOW)
        {
        }


        public GameWindow(Priority priority)
            : this(priority, null)
        {
        }


        public GameWindow(UIWidget showFrom)
            : this(Priority.PRIORITY_WINDOW, showFrom)
        {
        }

        public GameWindow(Priority priority, UIWidget showFrom)
            : base(priority, showFrom)
        {
            initEvent += initMohudiEnable;
            releaseBeforeActionEvent += releaseMohudiEnable;
        }

        protected override void InitDefaultConfig()
        {
            //if (!isInitDefaultConfig)
            //{
            //    //uiImage = UIReader.GetWidget(ResID.UI_PanelDisplay_Dialog_template).GetWidget("Image_window_bg").Copy();
            //    //uiImage.IsVisible = true;
            //    //uiImage.Size = Config.ScreenSize;
            //    //uiImage.Postion = Config.ScreenCenter;
            //    //backGround.AddChild(uiImage);

            //    isInitDefaultConfig = true;
            //}
        }

        //protected override void WindowBackGround()
        //{
        //    if (!isInitDefaultConfig)
        //    {
        //        base.WindowBackGround();
        //    }
        //}

        protected virtual void initMohudiEnable()
        {
            //if (uiImage != null)
            //{
            //    uiImage.StopAllAction();
            //    var fateIn = new MCActionFadeIn(effectTime);
            //    uiImage.RunAction(fateIn);
            //}
            //else
            //{
            //    WindowBackGround();
            //}
        }

        protected virtual void releaseMohudiEnable()
        {
            //if (uiImage != null)
            //{
            //    uiImage.StopAllAction();
            //    var fateIn = new MCActionFadeOut(effectTime);
            //    uiImage.RunAction(fateIn);
            //}
        }


        public override CCAction GetShowAction(bool isShowing)
        {
            if (isShowing)
            {
                Scale = 0;

                var scale = new CCActionScaleTo(effectTime, 1);
                var exponential = new CCActionEaseBackOut(scale);
                return exponential;
            }
            else
            {
                var scale = new CCActionScaleTo(effectTime, 0);
                var exponential = new CCActionEaseBackIn(scale);
                return exponential;
            }
        }
    }
}
