using MatrixEngine.Cocos2d;
using MatrixEngine.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thunder.GameLogic.Common;

namespace Thunder.GameLogic.Gaming.Actors.Drops
{
    public class DropProp : Drop
    {
        protected CCSprite shieldImg;
        public DropProp()
        {
            shieldImg = new CCSprite("effects_036.png", true);
            shieldImg.Color = new Color32(243,94,13);
            shieldImg.BlendFunc = BlendFunc.Additive;
            shieldImg.RunAction(new CCActionRepeatForever(new CCActionRotateBy(1.0f,180)));
        }

        protected override void Dispose(bool disposing)
        {
            if (shieldImg != null)
            {
                shieldImg.RemoveFromParent();
                shieldImg.Dispose();
                shieldImg = null;
            }
            base.Dispose(disposing);
        }

        public override DropConfig Config
        {
            get
            {
                return base.Config;
            }
            set
            {
                value.speed = 400;
                value.speedVar = 100;

                value.speedDecay = -5;
                value.speedLimit = 170;

                value.waitingTime = -1;

                base.Config = value;
            }

        }

        protected override void Floating(float dt)
        {
            this.floatDir = this.curDir;
            base.Floating(dt);
            if (DisFromPlayer() < 200 && !this.mPlayer.IsDead)
            {
                GotoState(State.Attach);
            }
        }

        protected override void OnAdsorbent()
        {
            Function.PlayProEffect();
        }
    }
}
