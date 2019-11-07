using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MatrixEngine.Engine;
using MatrixEngine.Cocos2d;

namespace Thunder.GameLogic.ExternAction
{
    public class ActionSetAlpha : MCActionInstant
    {
        private int alpha;

        public ActionSetAlpha(int _alpha)
        {
            alpha = _alpha;
        }

        protected override void OnUpdate(CCNode pNode)
        {
            if (pNode is CCNodeRGBA)
            {
                CCNodeRGBA node = (CCNodeRGBA)pNode;
                node.Alpha = alpha;
            }
        }

        public override CCAction Reverse()
        {
            return new ActionSetAlpha(alpha);
        }
    }
}
