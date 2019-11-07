using MatrixEngine.Cocos2d;
using MatrixEngine.CocoStudio.GUI;
using MatrixEngine.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thunder.GameLogic.ExternAction
{
    public class ActionLoadingBar : MCActionInterval
    {
        int fromPro;
        int toPro;
        UILoadingBar target;

        public ActionLoadingBar(float time, int from = 0, int to = 100)
            : base(time)
        {
            fromPro = from;
            toPro = to;
        }

        protected override void StartWithTarget(CCNode node)
        {
            if (node is UILoadingBar)
            {
                target = (UILoadingBar)node;
                target.Percent = fromPro;
            }
        }

        protected override void OnUpdate(float percent)
        {
            if (target != null)
            {
                if (toPro > fromPro)
                {
                    target.Percent = fromPro + (int)(toPro * percent);
                }
                else
                {
                    target.Percent = toPro + (int)((fromPro - toPro) * (1 - percent));
                }
            }
        }

        public override CCAction Reverse()
        {
            ActionLoadingBar action = new ActionLoadingBar(GetTime(), this.toPro, this.fromPro);
            return action;
        }
    }
}
