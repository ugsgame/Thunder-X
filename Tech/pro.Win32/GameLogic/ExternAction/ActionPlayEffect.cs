using MatrixEngine.Cocos2d;
using MatrixEngine.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thunder.GameLogic.Common;

namespace Thunder.GameLogic.ExternAction
{
    public class ActionPlayEffect : MCActionInstant
    {
        GameAudio.Effect effectName;
        bool isLoop;

        public ActionPlayEffect(GameAudio.Effect effect,bool loop = false)
        {
            effectName = effect;
            isLoop = loop;
        }

        protected override void OnUpdate(CCNode pNode)
        {
            GameAudio.PlayEffect(effectName, isLoop);
        }

        public override CCAction Reverse()
        {
            return new ActionPlayEffect(effectName, isLoop);
        }
    }
}
