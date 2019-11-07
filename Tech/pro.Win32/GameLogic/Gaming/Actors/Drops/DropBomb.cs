
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MatrixEngine.Cocos2d;
using Thunder.GameLogic.Gaming.BulletSystems;

namespace Thunder.GameLogic.Gaming.Actors.Drops
{
    public class DropBomb:DropProp
    {
        private static DropEmitter mEmitter;
        public static DropEmitter Emitter
        {
            get
            {
                if (mEmitter == null)
                {
                    mEmitter = new DropEmitter(new DropBomb());
                }
                return mEmitter;
            }
        }

        public DropBomb()
        {
            this.mType = DropType.Drop_Bomb;
            //TODO:
            this.Display = new ArmatureDisplay("Objects", "drop_5");
        }

        public override CCObject Copy()
        {
            DropBomb drap = new DropBomb();
            this.BaseConfig(drap);
            return drap;
        }
    }
}
