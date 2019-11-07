using MatrixEngine.Cocos2d;
using MatrixEngine.CocoStudio.GUI;
using MatrixEngine.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thunder.Common;
using Thunder.GameLogic.Common;

namespace Thunder.GameLogic.UI.Widgets
{
    public class BossWaring : UIWidget
    {
        private readonly float outTime = 0.5f;
        private readonly int playTimes = 5;

        private CCSprite waringFrameLeft;
        private CCSprite waringFrameRight;
        private CCSprite waringBottom;

        private CCSprite arrowLeft;
        private CCSprite arrowRight;

        private CCNode waringNode = new CCNode();
        private CCSprite waringText;

        private CCSprite waringText1;
        private CCSprite waringText2;

        private bool isPlaying;

        public delegate void CallFunc();

        private CallFunc callFuc;
        public BossWaring()
        {
            waringFrameLeft = new CCSprite("effects_warning01.png", true);
            waringFrameRight = new CCSprite("effects_warning01.png", true);
            waringFrameRight.ScaleX = -1;

            arrowLeft = new CCSprite("effects_warning03.png", true);
            arrowRight = new CCSprite("effects_warning03.png", true);
            arrowRight.ScaleX = -1;
            arrowLeft.Color = Color32.Red;
            arrowRight.Color = Color32.Red;
            arrowLeft.Alpha = 150;
            arrowRight.Alpha = 150;
            arrowLeft.PostionX = -170;
            arrowRight.PostionX = 170;
            arrowLeft.BlendFunc = BlendFunc.Additive;
            arrowRight.BlendFunc = BlendFunc.Additive;

            waringBottom = new CCSprite("effects_warning02.png", true);
            waringBottom.Color = Color32.Red;

            CCSprite waringUp1 = new CCSprite("effects_warning06.png",true);
            CCSprite waringUp2 = new CCSprite("effects_warning06.png", true);
            waringUp1.Postion = new Vector2(-90, 30);
            waringUp2.Postion = new Vector2(90, 30);
            waringUp2.ScaleX = -1;

            CCSprite waringDown1 = new CCSprite("effects_warning06.png", true);
            CCSprite waringDown2 = new CCSprite("effects_warning06.png", true);
            waringDown1.Postion = new Vector2(-90, -30);
            waringDown2.Postion = new Vector2(90, -30);
            waringDown1.ScaleY = -1;
            waringDown2.Scale = -1;

            waringUp1.Color = Color32.Yellow;
            waringUp2.Color = Color32.Yellow;
            waringDown1.Color = Color32.Yellow;
            waringDown2.Color = Color32.Yellow;
            waringUp1.BlendFunc = BlendFunc.Additive;
            waringUp2.BlendFunc = BlendFunc.Additive;
            waringDown1.BlendFunc = BlendFunc.Additive;
            waringDown2.BlendFunc = BlendFunc.Additive;
            waringNode.AddChild(waringUp1);
            waringNode.AddChild(waringUp2);
            waringNode.AddChild(waringDown1);
            waringNode.AddChild(waringDown2);

            waringText = new CCSprite("effects_warning07.png",true);
            waringText.Color = Color32.Yellow;
            waringText.BlendFunc = BlendFunc.Additive;

            waringText1 = new CCSprite("effects_warning04.png", true);
            waringText1.Color = Color32.Red;
            waringText1.BlendFunc = BlendFunc.Additive;
            waringText1.PostionX = -120;

            waringText2 = new CCSprite("effects_warning05.png", true);
            waringText2.Color = Color32.Red;
            waringText2.BlendFunc = BlendFunc.Additive;
            waringText2.PostionX = 120;

            this.AddNode(waringBottom);
            this.AddNode(waringNode);
            this.AddNode(arrowLeft);
            this.AddNode(arrowRight);

            this.AddNode(waringFrameLeft);
            this.AddNode(waringFrameRight);

            this.AddNode(waringText);
            this.AddNode(waringText1);
            this.AddNode(waringText2);

            this.Reset();
        }

        private void Reset()
        {
            waringBottom.Postion = 0;
            waringFrameLeft.PostionX = -15;
            waringFrameRight.PostionX = 15;

            arrowLeft.IsVisible = true;
            arrowRight.IsVisible = true;
            arrowLeft.Alpha = 0;
            arrowRight.Alpha = 0;

            waringText.Alpha = 0;
            waringText1.Alpha = 0;
            waringText2.Alpha = 0;

            this.waringBottom.ScaleX = 0.1f;
            this.waringBottom.ScaleY = 1.0f;

            this.IsVisible = false;
            this.isPlaying = false;
        }

        public virtual void Play(CallFunc call)
        {
            if (this.isPlaying) return;
            callFuc = call;
            this.isPlaying = true;
            this.IsVisible = true;

            this.waringBottom.ScaleY = 1.0f;

            CCAction left = new CCActionSpawn(new CCActionFadeIn(outTime), new CCActionMoveBy(outTime, new Vector2(-180, waringFrameLeft.PostionY)));
            CCAction right = new CCActionSpawn(new CCActionFadeIn(outTime), new CCActionMoveBy(outTime, new Vector2(180, waringFrameRight.PostionY)));

            CCAction scale = new CCActionScaleTo(outTime, 1.0f);
            CCAction fadeIn = new CCActionFadeIn(1);

//             CCAction fadeIn1 = new CCActionFadeIn(outTime);
//             CCAction fadeIn2 = new CCActionFadeIn(outTime);

//             waringFrameLeft.Alpha = 255;
//             waringFrameRight.Alpha = 255;
            waringFrameLeft.RunAction(left);
            waringFrameRight.RunAction(right);
            waringBottom.RunAction(new CCActionSequence(new CCActionSpawn(scale, fadeIn), new CCActionCallFunc(this.Waring)));

//             arrowLeft.RunAction(fadeIn1);
//             arrowRight.RunAction(fadeIn2);
            waringNode.IsVisible = false;
            GameAudio.PlayEffect(GameAudio.Effect.boss_await);
        }

        private void Waring()
        {
            arrowLeft.IsVisible = true;
            arrowRight.IsVisible = true;
            waringNode.IsVisible = true;
            waringText.IsVisible = true;
            waringText1.IsVisible = true;
            waringText2.IsVisible = true;

            var fade1 = new CCActionSequence(new CCActionFadeOut(0.3f), new CCActionFadeIn(0.3f));
            var fade2 = new CCActionSequence(new CCActionFadeOut(0.3f), new CCActionFadeIn(0.3f));
            var fade3 = new CCActionSequence(new CCActionFadeOut(0.3f), new CCActionFadeIn(0.3f));
            var fade4 = new CCActionSequence(new CCActionFadeOut(0.3f), new CCActionFadeIn(0.3f));
            var fade5 = new CCActionSequence(new CCActionFadeOut(0.3f), new CCActionFadeIn(0.3f));
            arrowLeft.RunAction(new CCActionSequence(new CCActionFadeIn(0.3f), new CCActionRepeat(fade1, playTimes)));
            arrowRight.RunAction(new CCActionSequence(new CCActionFadeIn(0.3f), new CCActionRepeat(fade2, playTimes), new CCActionCallFunc(this.WaringOver)));
            waringText.RunAction(new CCActionRepeat(fade3, playTimes));
            waringText1.RunAction(new CCActionRepeat(fade4, playTimes));
            waringText2.RunAction(new CCActionRepeat(fade5, playTimes));
        }

        private void WaringOver()
        {
            var fade1 = new CCActionFadeOut(0.5f);
            var fade2 = new CCActionFadeOut(0.5f);
            var fade3 = new CCActionFadeOut(0.2f);
            var fade4 = new CCActionFadeOut(0.2f);

            var scale1 = new CCActionScaleTo(0.5f, 1, 0);

            arrowLeft.IsVisible = false;
            arrowRight.IsVisible = false;
            waringText.IsVisible = false;
            waringNode.IsVisible = false;
            waringText1.IsVisible = false;
            waringText2.IsVisible = false;

            waringFrameLeft.RunAction(fade3);
            waringFrameRight.RunAction(fade4);

            waringBottom.RunAction(new CCActionSequence(scale1, new CCActionCallFunc(this.Reset)));
            this.callFuc();
        }

    }
}
