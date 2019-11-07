
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MatrixEngine.Cocos2d;
using MatrixEngine.Engine;

namespace Thunder.GameLogic.ExternAction
{
    public class ActionSetScale : MCActionInstant
    {
        private float scale;

        public ActionSetScale(float _scale)
        {
            this.scale = _scale;
        }

        protected override void OnUpdate(CCNode pNode)
        {
            pNode.Scale = scale;
        }

        public override CCAction Reverse()
        {
            return new ActionSetScale(scale);
        }
    }
}
