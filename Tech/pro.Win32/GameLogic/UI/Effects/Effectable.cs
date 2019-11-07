using MatrixEngine.Cocos2d;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thunder.GameLogic.UI.Effects
{
    public class Effectable : CCNode
    {
        bool isLoop;

        public virtual void Play()
        {
            Play(false);
        }

        public virtual void Play(bool loop)
        {
            isLoop = loop;
        }

        public virtual void Stop()
        {

        }
    }
}
