using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MatrixEngine.Cocos2d;

namespace Thunder.GameLogic.Gaming.ScripSystem.Events
{

    class GoTo : EventNode
    {
        public GoTo(Actors.Actor actor,int id, int nextID)
            : base(nextID)
        {
            this.Actor = actor;
            this.EventOver();
        }
    }
    class DelayTime : EventNode
    {
        CCAction action;

        protected override void Dispose(bool disposing)
        {
            this.StopAllAction();
            action.Dispose();
            action = null;

            base.Dispose(disposing);
        }

        public DelayTime(float time, int nextID)
            : base(nextID)
        {
            action = new CCActionSequence(new CCActionDelayTime(time), new CCActionCallFunc(this.EventOver));
            this.RunAction(action);
        }
    }

    class EventAction : EventNode
    {
        CCAction callfuc;

        protected override void Dispose(bool disposing)
        {
            this.StopAllAction();
            callfuc.Dispose();
            callfuc = null;
            base.Dispose(disposing);
        }

        public EventAction(int nextID)
            : base(nextID)
        {
            callfuc = new CCActionCallFunc(this.EventOver);
        }

        public CCAction Action
        {
            get { return callfuc; }
        }
    }

}
