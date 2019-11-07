using MatrixEngine.Cocos2d;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thunder.GameLogic.ExternAction;

namespace Thunder.GameLogic.UI.Effects
{
    public class TileLight : Effectable
    {
        CCSprite light;
        CCAction action1;
        CCAction action2;

        public TileLight()
        {
            light = new CCSprite("title_guang.png", true);
            light.Postion = 0;
            light.BlendFunc = BlendFunc.Additive;

            action1 = new CCActionRepeatForever(new CCActionSequence(new ActionSetPosition(0), new ActionSetAlpha(255), new ActionSetScale(2f)
                , new CCActionSpawn(new CCActionMoveBy(3, 300, 0), new CCActionFadeOut(5), new CCActionScaleTo(3f, 1f))));
                
            action2 = new CCActionSequence(new ActionSetPosition(0), new ActionSetAlpha(255), new ActionSetScale(2f)
                , new CCActionSpawn(new CCActionMoveBy(3, 300, 0), new CCActionFadeOut(5), new CCActionScaleTo(3f, 1f)));

            this.AddChild(light);
        }

        public override void Play(bool loop)
        {
            if (loop)
            {
                light.RunAction(action1);
            }
            else
            {
                light.RunAction(action2);
            }
        }

        public override void Stop()
        {
            light.StopAllAction();
            light.Alpha = 0;
        }
    }
}
