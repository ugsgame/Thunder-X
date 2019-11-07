
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Thunder.GameLogic.Gaming.BulletSystems;
using MatrixEngine.Cocos2d;
using MatrixEngine.Math;
using Thunder.GameLogic.Common;

namespace Thunder.GameLogic.Gaming.Actors.Drops
{
    public class DropPower : DropProp
    {
        private static DropEmitter mEmitter;
        public static DropEmitter Emitter
        {
            get
            {
                if (mEmitter == null)
                {
                    mEmitter = new DropEmitter(new DropPower());
                }
                return mEmitter;
            }
        }
        public DropPower()
        {
            this.mType = DropType.Drop_Power;
            //设置diplay
            //TODO:要改为AnimationDisplay 以提高效率
            //this.Display = new ArmatureDisplay("Objects", "drop_3");
            this.Display = new AnimationDisplay(ResID.Armatures_zidan, "zidan_daoju (4).png", "zidan_daoju (5).png", "zidan_daoju (6).png");
            this.Display.AddChild(this.shieldImg);
            //this.Display = new SpriteDisplay("Data/Armatures/Particles/byhit.png");
        }
        public override CCObject Copy()
        {
            DropPower drap = new DropPower();
            this.BaseConfig(drap);
            return drap;
        }
    }
}
