using MatrixEngine.Cocos2d;
using MatrixEngine.CocoStudio;
using MatrixEngine.CocoStudio.Armature;
using MatrixEngine.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thunder.GameLogic.ExternAction
{
    public class ActionPlayAnimation : MCActionInstant
    {
        string anim;
        bool isLoop;

        public ActionPlayAnimation(string animName,bool loop = false)
        {
            anim = animName;
            isLoop = loop;
        }

        protected override void OnUpdate(CCNode pNode)
        {
            if (pNode is CCArmature)
            {
                CCArmature arma = (CCArmature)pNode;
                arma.GetAnimation().Play(anim, isLoop);
            }
        }

        public override CCAction Reverse()
        {
            return new ActionPlayAnimation(anim,!isLoop);
        }
    }
}
