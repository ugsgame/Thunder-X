using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MatrixEngine.Cocos2d;
using MatrixEngine.CocoStudio.GUI;
using Thunder.GameLogic.Common;
using Thunder.GameLogic.GameSystem;
using Thunder.GameLogic.Gaming;

namespace Thunder.GameLogic.UI.Dialogs
{
    public class VIPMsg:GameWindow
    {
        private UIWidget effectVIP;

        private CCSprite Image_light;
        private CCSprite Image_title;
        private CCSprite Image_vip;

        public bool IsShowing;

        public static VIPMsg Instance;

        public VIPMsg()
        {
            effectVIP = new UIWidget();
            Image_light = new CCSprite("effects_light01.png", true);
            effectVIP.AddNode(Image_light);
            Image_vip = new CCSprite("vip_btn.png", true);
            effectVIP.AddNode(Image_vip);
            Image_title = new CCSprite("vip_tip.png", true);
            Image_title.PostionY = -50;
            effectVIP.AddNode(Image_title);

            this.AddChildCenter(effectVIP);

            Instance = this;
        }

        public override void init()
        {
            base.init();

            Image_light.Scale = 1.0f;
            Image_light.RunAction(new CCActionRepeat(new CCActionRotateBy(1, 90f), 3));
            Image_light.RunAction(new CCActionEaseElasticOut(new CCActionScaleTo(1.5f, 2.5f)));

            Image_title.Scale = 0.5f;
            Image_title.RunAction(new CCActionEaseElasticOut(new CCActionScaleTo(1.8f, 1f)));

            this.RunSequenceActions(new CCActionDelayTime(2f), new CCActionCallFunc(this.AutoClose));

            //能来到这里肯定已成为vip
            GameData.Instance.IsVip = true;
        }

        private void AutoClose()
        {
            this.Show(false);

            if (GameData.Instance.GameState == GameState.Playing)
            {
                if (!GamePause.Instance.IsShow && !GameWin.Instance.IsShow)
                {
                    PlayingScene.Instance.IsPause = false;
                }
            }
        }

        public override void Show(bool b)
        {
            base.Show(b);
            if (b)
            {
                if (GameData.Instance.GameState == GameState.Playing)
                    PlayingScene.Instance.IsPause = true;
            }
            IsShowing = b;
        }
    }
}
