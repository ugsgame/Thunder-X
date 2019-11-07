using MatrixEngine.Cocos2d;
using MatrixEngine.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thunder.GameLogic.ExternAction;

namespace Thunder.GameLogic.UI.Effects
{
    public class SuppriseEffect:Effectable
    {
        public SuppriseEffect()
        {
            CCSprite money = new CCSprite("zhujiemian_libao1_02.png", true);
            CCSprite gift = new CCSprite("zhujiemian_libao1.png", true);

            gift.Postion = 0;
            money.Postion = new Vector2(-50,50);

            gift.BlendFunc = BlendFunc.Additive;

            var scaleRepeat = new CCActionRepeatForever(new CCActionSequence(new CCActionScaleTo(0.5f,1.3f),new CCActionScaleTo(0.5f,1.0f)));
            money.RunAction(scaleRepeat);

            var fadeScaleRepeat = new CCActionRepeatForever(new CCActionSequence(new ActionSetScale(1.0f),new ActionSetAlpha(255),new CCActionSpawn(new CCActionScaleTo(1.5f,2f),new CCActionFadeOut(1.5f))));
            gift.RunAction(fadeScaleRepeat);

            this.AddChild(gift);
            this.AddChild(money);
        }
    }
}
