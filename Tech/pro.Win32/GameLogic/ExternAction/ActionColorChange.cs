using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MatrixEngine.Cocos2d;
using MatrixEngine.CocoStudio.GUI;
using MatrixEngine.Engine;
using MatrixEngine.Math;

namespace Thunder.GameLogic.ExternAction
{
    public class ActionColorChange : MCActionInterval
    {
        Color32 fromCol;
        Color32 toCol;
        Color32 aimCol;

        bool random;

        CCNodeRGBA target;
        public ActionColorChange(float time, Color32 from, Color32 to)
            : base(time)
        {
            fromCol = from;
            toCol = to;
        }
        /// <summary>
        /// 随机改色彩
        /// </summary>
        /// <param name="time"></param>
        public ActionColorChange(float time)
            : base(time)
        {
            random = true;
        }

        protected override void StartWithTarget(CCNode node)
        {
            if (node is CCNodeRGBA)
            {
                target = (CCNodeRGBA)node;

                if (random)
                {
                    fromCol = target.Color;
                    toCol.R = (byte)MathHelper.Random_minus0_n(255);
                    toCol.G = (byte)MathHelper.Random_minus0_n(255);
                    toCol.B = (byte)MathHelper.Random_minus0_n(255);
                }
            }
        }

        protected override void OnUpdate(float percent)
        {
            if (target != null)
            {
                aimCol.R = (byte)(fromCol.R + ((toCol.R - fromCol.R) * percent));
                aimCol.G = (byte)(fromCol.G + ((toCol.G - fromCol.G) * percent));
                aimCol.B = (byte)(fromCol.B + ((toCol.B - fromCol.B) * percent));

                target.Color = aimCol;
            }
        }

        public override CCAction Reverse()
        {
            ActionColorChange action = new ActionColorChange(GetTime(), this.toCol, this.fromCol);
            return action;
        }
    }
}
