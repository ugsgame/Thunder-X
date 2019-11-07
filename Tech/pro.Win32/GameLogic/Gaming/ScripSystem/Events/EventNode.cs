using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MatrixEngine.Cocos2d;
using Thunder.GameLogic.Gaming.Actors;


namespace Thunder.GameLogic.Gaming.ScripSystem.Events
{
    /// <summary>
    /// 事件节点
    /// </summary>
    public class EventNode : CCNode
    {
        private int _id;

        private int nextEventID;

        protected EventManager eventManager;
        protected Actor actor;

        public EventNode()
        {

        }

        ~EventNode()
        {
            Console.WriteLine("~EventAction");
        }

        public EventNode(int nextID)
        {
            this.NextEventID = nextID;
        }

        public Actor Actor
        {
            set
            {
                actor = value;
                EventManager = actor.EventManager;
            }
            get { return actor; }
        }

        public EventManager EventManager
        {
            set { eventManager = value; }
            get { return eventManager; }
        }

        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }

        public int NextEventID
        {
            set { nextEventID = value; }
            get { return nextEventID; }
        }

        protected virtual void EventOver()
        {
            EventManager.OnEvent(NextEventID,this);
        }
    }
}
