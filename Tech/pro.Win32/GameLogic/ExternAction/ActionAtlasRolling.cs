using MatrixEngine.Cocos2d;
using MatrixEngine.CocoStudio.GUI;
using MatrixEngine.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thunder.GameLogic.ExternAction
{
    public class ActionAtlasRolling : MCActionInterval
    {
        int fromNum;
        int toNum;

        UILabelAtlas atlas;

        public ActionAtlasRolling(float time, int from, int to)
            : base(time)
        {
            fromNum = from;
            toNum = to;
        }

        protected override void StartWithTarget(CCNode node)
        {
            if (node is UILabelAtlas)
            {
                atlas = (UILabelAtlas)node;
            }
        }

        protected override void OnUpdate(float percent)
        {
            if (atlas != null)
            {
                //TODO:让它滚得更有弹性点
                atlas.Text =Convert.ToString(fromNum + (int)((toNum - fromNum) * percent));
            }
        }
    }
}
