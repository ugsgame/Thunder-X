
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Thunder.Common;
using Thunder.Game;
using MatrixEngine.Math;
using MatrixEngine.CocoStudio.GUI;

namespace Thunder.GameLogic.GameSystem
{
    public class ScreenSizeWindow : ActionWindow
    {
        //public delegate void DefaultConfigHandle();
        //public event DefaultConfigHandle DefaultConfigEvent;
        protected UILayout backGround = new UILayout();

        public bool IsShowGameAudio;

        public ScreenSizeWindow()
            : this(Priority.PRIORITY_WINDOW)
        {
        }

        public ScreenSizeWindow(Priority priority)
            : this(priority, null)
        {
        }

        public ScreenSizeWindow(UIWidget showFrom)
            : this(Priority.PRIORITY_WINDOW, showFrom)
        {
        }


        public ScreenSizeWindow(Priority priority, UIWidget showFrom)
            : base(priority, showFrom)
        {
            WindowLayout.AddChild(backGround);
            this.ZOrder = 50;

//             initEvent += UserGuideStep.Pause;
//             initAfterActionEvent += UserGuideStep.Continue;
//             releaseBeforeActionEvent += UserGuideStep.Pause;
//             releaseEvent += UserGuideStep.Continue;

//             initEvent += () =>
//             {
//                 if (IsShowGameAudio)
//                     GameAudio.PlayEffect(GameAudio.Effect.sound_icon);
//             };
//             releaseBeforeActionEvent += () =>
//             {
//                 if (IsShowGameAudio)
//                     GameAudio.PlayEffect(GameAudio.Effect.sound_close);
//             };
            WindowDefaultSize();
            InitDefaultConfig();
            WindowBackGround();
        }


        public virtual void ConnectToLoad()
        {
        }

        protected override void initAfterAction()
        {
            ConnectToLoad();
        }

        protected virtual void WindowDefaultSize()
        {
            ContextSize = Config.ScreenSize;
            Size = Config.ScreenSize;
            AnchorPoint = Utils.AnchorPoint_Center;
            Postion = Config.ScreenCenter;
            TouchEnabled = true;
        }

        protected virtual void WindowBackGround()
        {
            backGround.ContextSize = Config.ScreenSize;
            backGround.TouchEnabled = true;
            backGround.Size = Config.ScreenSize;
            backGround.SetBackGroundColorType(LayoutBackGroundColorType.LAYOUT_COLOR_SOLID);
            backGround.SetBackGroundColorOpacity(200);
            backGround.SetBackGroundColorS(Color32.Black);
        }

        protected virtual void InitDefaultConfig()
        {
        }

    }
}
