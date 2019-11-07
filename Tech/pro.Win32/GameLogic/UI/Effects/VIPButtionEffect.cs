using MatrixEngine.Cocos2d;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thunder.GameLogic.UI.Effects
{
    public class VIPButtionEffect:Effectable
    {
        public VIPButtionEffect()
        {
            CCSprite light1 = new CCSprite("effects_light01.png", true);
            CCSprite light2 = new CCSprite("effects_light01.png", true);

            //light1.BlendFunc = BlendFunc.Additive;
            //light2.BlendFunc = BlendFunc.Additive;

            light1.Scale = 1;
            light2.Scale = 1;

            light1.RunAction(new CCActionRepeatForever(new CCActionRotateBy(4,360)));
            light2.RunAction(new CCActionRepeatForever(new CCActionRotateBy(4, -360)));

            this.AddChild(light1);
            this.AddChild(light2);
        }
    }
}
