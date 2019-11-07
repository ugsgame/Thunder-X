using MatrixEngine.Cocos2d;
using MatrixEngine.CocoStudio.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thunder.Common;
using Thunder.GameLogic.Common;
using Thunder.GameLogic.GameSystem;
using Thunder.GameLogic.Gaming;

namespace Thunder.GameLogic.UI.Widgets
{
    public class ResumeTimer : GameWindow
    {
        UIWidget mainWidget;
        UIImageView Image_bottom;

        CCSprite num_1;
        CCSprite num_2;
        CCSprite num_3;

        public delegate void CallFunc();
        private CallFunc callFuc;
        public ResumeTimer()
        {
            mainWidget = UIReader.GetWidget(ResID.UI_UI_ResumeTimer);
            UIWidget Panel_main = mainWidget.GetWidget("Panel_main");
            Image_bottom = (UIImageView)Panel_main.GetWidget("Image_bottom");

            num_1 = new CCSprite("daojishishuzi_01.png", true);
            num_2 = new CCSprite("daojishishuzi_02.png", true);
            num_3 = new CCSprite("daojishishuzi_03.png", true);

            num_1.IsVisible = false;
            num_1.BlendFunc = BlendFunc.Additive;
            num_1.Postion = Image_bottom.Postion;
            num_1.Scale = 1.3f;

            num_2.IsVisible = false;
            num_2.BlendFunc = BlendFunc.Additive;
            num_2.Postion = Image_bottom.Postion;
            num_2.Scale = 1.3f;

            num_3.IsVisible = false;
            num_3.BlendFunc = BlendFunc.Additive;
            num_3.Postion = Image_bottom.Postion;
            num_3.Scale = 1.3f;

            Panel_main.AddNode(num_1);
            Panel_main.AddNode(num_2);
            Panel_main.AddNode(num_3);

            this.AddChildCenter(mainWidget);
        }

        private void PlayNum1()
        {
            num_1.IsVisible = true;
            num_1.RunSequenceActions(new CCActionFadeOut(0.5f), new CCActionHide(), new CCActionCallFunc(this.PlayOver));
        }
        private void PlayNum2()
        {
            num_2.IsVisible = true;
            num_2.RunSequenceActions(new CCActionFadeOut(0.5f), new CCActionHide(), new CCActionCallFunc(this.PlayNum1));
        }
        private void PlayNum3()
        {
            num_3.IsVisible = true;
            num_3.RunSequenceActions(new CCActionFadeOut(0.5f), new CCActionHide(), new CCActionCallFunc(this.PlayNum2));
        }

        private void PlayOver()
        {
            this.Show(false);
            this.callFuc();
        }

        public void Play(CallFunc call)
        {
            callFuc = call;
            this.PlayNum3();
            this.Show(true);
            GameAudio.PlayEffect(GameAudio.Effect.resume);
        }

        public override void Show(bool b)
        {
            base.Show(b);
        }
    }
}
