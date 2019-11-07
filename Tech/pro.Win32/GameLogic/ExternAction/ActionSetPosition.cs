using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MatrixEngine.Engine;
using MatrixEngine.Math;
using MatrixEngine.Cocos2d;

namespace Thunder.GameLogic.ExternAction
{
    public class ActionSetPosition : MCActionInstant
    {
        private Vector2 position;

        public ActionSetPosition(Vector2 pos)
        {
            position = pos;
        }

        protected override void OnUpdate(CCNode pNode)
        {
            pNode.Postion = position;
        }

        public override CCAction Reverse()
        {
            return new ActionSetPosition(position);
        }
    }
}
