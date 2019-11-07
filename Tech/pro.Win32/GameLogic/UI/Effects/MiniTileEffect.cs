using MatrixEngine.Cocos2d;
using MatrixEngine.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thunder.GameLogic.ExternAction;

namespace Thunder.GameLogic.UI.Effects
{
    public class MiniTileEffect : Effectable
    {

        CCAction action1;
        CCAction action2;

        CCSprite title;

        public MiniTileEffect()
        {
            title = new CCSprite("title_1_2.png", true);
            title.BlendFunc = BlendFunc.Additive;
            title.Postion = 0;
            title.Alpha = 0;

            action1 = new CCActionRepeatForever(new CCActionSequence(new CCActionDelayTime(1f), new ActionSetAlpha(255),
                new CCActionFadeOut(2f)));

            action2 = new CCActionSequence(new ActionSetAlpha(255),
               new CCActionFadeOut(2f));

            this.AddChild(title);
        }

        public override void Play(bool loop)
        {
            if (loop)
            {
                title.RunAction(action1);
            }
            else
            {
                title.RunAction(action2);
            }
        }

        public override void Stop()
        {
            title.StopAllAction();
        }
    }
}
