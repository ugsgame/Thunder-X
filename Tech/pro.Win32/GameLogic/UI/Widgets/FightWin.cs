using MatrixEngine.Cocos2d;
using MatrixEngine.CocoStudio.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thunder.GameLogic.Common;
using Thunder.GameLogic.GameSystem;
using Thunder.GameLogic.Gaming;

namespace Thunder.GameLogic.UI.Widgets
{
    public class FightWin : GameWindow
    {
        public delegate void CallFunc();
        private CallFunc callFuc;

        CCSprite light;
        CCSprite badge;
        CCSprite title;

        UILayout main;

        public FightWin()
        {
            main = new UILayout();
            light = new CCSprite("effects_light01.png", true);
            badge = new CCSprite("effects_zhandoushengli02.png", true);
            title = new CCSprite("effects_zhandoushengli01.png", true);

            light.BlendFunc = BlendFunc.Additive;
            light.PostionY = 40;
            light.Scale = 2;
            light.RunAction(new CCActionRepeatForever(new CCActionRotateBy(2f, 360)));
            badge.Postion = 0;

            title.PostionY = -50;

            main.AddNode(light);
            main.AddNode(badge);
            main.AddNode(title);

            this.AddChildCenter(main);
        }

        private void PlayOver()
        {
            this.Show(false);
            this.callFuc();
        }

        private void PlayerFlyOut()
        {
            PlayingScene.Instance.GameLayer.Player.FlyOut();
        }

        public void Play(CallFunc call)
        {
            callFuc = call;

            badge.Alpha = 0;
            badge.Scale = 0.3f;
            badge.RunSpawnActions(new CCActionFadeIn(0.5f), new CCActionEaseElasticOut(new CCActionScaleTo(1.0f, 1)));

            title.Alpha = 0;
            title.Scale = 0.3f;
            title.RunSequenceActions(new CCActionSpawn(new CCActionFadeIn(1.0f), new CCActionEaseElasticOut(new CCActionScaleTo(2.0f, 1))),
                new CCActionCallFunc(this.PlayerFlyOut), new CCActionDelayTime(1), new CCActionCallFunc(this.PlayOver));

            GameAudio.PauseMusic();
            GameAudio.PlayEffect(GameAudio.Effect.win);

            this.Show(true);
        }

        protected override void WindowBackGround()
        {
            base.WindowBackGround();
            backGround.SetBackGroundColorOpacity(0);
        }
    }
}
