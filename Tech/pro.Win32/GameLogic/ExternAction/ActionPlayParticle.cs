using MatrixEngine.Cocos2d;
using MatrixEngine.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thunder.GameLogic.ExternAction
{
    public class ActionPlayParticle : MCActionInstant
    {
        CCParticleSystem particle;

        public ActionPlayParticle()
        {

        }

        protected override void OnUpdate(CCNode pNode)
        {
            if (pNode is CCParticleSystem)
            {
                particle = (CCParticleSystem)pNode;
                particle.Start();
            }
        }

        public override CCAction Reverse()
        {
            return new ActionPlayParticle();
        }
    }
}
