using MatrixEngine.Cocos2d;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thunder.GameLogic.ExternAction;

namespace Thunder.GameLogic.UI.Effects
{
    public class BuyButtonEffect:Effectable
    {
        public BuyButtonEffect()
        {
            CCSprite button = new CCSprite("button01.png", true);

            button.Postion = 0;
            button.BlendFunc = BlendFunc.Additive;

            var fadeScaleRepeat = new CCActionRepeatForever(new CCActionSequence(new ActionSetScale(1.0f), new ActionSetAlpha(255), new CCActionSpawn(new CCActionScaleTo(1.5f, 1.3f), new CCActionFadeOut(1.5f))));
            button.RunAction(fadeScaleRepeat);

            this.AddChild(button);
        }
    }
}
