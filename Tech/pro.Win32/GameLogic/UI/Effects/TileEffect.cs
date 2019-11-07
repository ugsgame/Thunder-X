using MatrixEngine.Cocos2d;
using MatrixEngine.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thunder.GameLogic.ExternAction;

namespace Thunder.GameLogic.UI.Effects
{
    public class TileEffect : Effectable
    {
        CCAction action1;
        CCAction action2;

        CCSprite title;

        public TileEffect()
        {
            title = new CCSprite("title_1.png", true);
            title.BlendFunc = BlendFunc.Additive;
            title.Postion = 0;
            title.Alpha = 0;

            action1 = new CCActionRepeatForever(new CCActionSequence(new CCActionDelayTime(2f), new ActionSetScale(1.0f), new ActionSetAlpha(255),
                new CCActionSpawn(new CCActionScaleTo(1.5f, 1.5f), new CCActionFadeOut(2f))));

            action2 = new CCActionSequence(new ActionSetScale(1.0f), new ActionSetAlpha(255),
                new CCActionSpawn(new CCActionScaleTo(1.5f, 1.5f), new CCActionFadeOut(2f)));

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
