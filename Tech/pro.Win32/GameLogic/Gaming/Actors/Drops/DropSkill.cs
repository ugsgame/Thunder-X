using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Thunder.GameLogic.Gaming.BulletSystems;
using MatrixEngine.Cocos2d;
using Thunder.GameLogic.Common;

namespace Thunder.GameLogic.Gaming.Actors.Drops
{
    public class DropSkill:DropProp
    {
        private static DropEmitter mEmitter;
        public static DropEmitter Emitter
        {
            get 
            {
                if (mEmitter == null)
                {
                    mEmitter = new DropEmitter(new DropSkill());
                }
                return mEmitter;
            }
        }


        public DropSkill()
        {
            this.mType = DropType.Drop_Skill;
            //设置diplay
            //TODO:要改为AnimationDisplay 以提高效率
            this.Display = new AnimationDisplay(ResID.Armatures_zidan, "zidan_daoju(7).png");
            this.Display.AddChild(this.shieldImg);
            //this.Display = new SpriteDisplay("Data/Armatures/Particles/byhit.png");
        }

        public override CCObject Copy()
        {
            DropSkill drap = new DropSkill();
            this.BaseConfig(drap);
            return drap;
        }
    }
}
