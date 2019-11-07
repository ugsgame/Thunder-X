using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MatrixEngine.Engine;
using MatrixEngine.Math;
using MatrixEngine.Cocos2d;

namespace Thunder.GameLogic.ExternAction
{
    public class RemoveSelf:MCActionInstant
    {
        protected override void OnUpdate(CCNode pNode)
        {
            pNode.RemoveFromParent(true);
        }

        public override CCAction Reverse()
        {
            return new RemoveSelf();
        }
    }
}
