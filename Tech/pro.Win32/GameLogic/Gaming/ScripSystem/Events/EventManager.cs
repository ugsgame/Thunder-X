using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MatrixEngine.Cocos2d;
using Thunder.GameLogic.Gaming.Actors;

namespace Thunder.GameLogic.Gaming.ScripSystem.Events
{
    public class EventManager:CCNode
    {
        private ScriptHelper scriptHelper;
        private Actor actor;
        private int runingID;

        internal EventNode curEvent;

        public EventManager()
        {

        }

        protected override void Dispose(bool disposing)
        {
            //注释这里可能有内存隐患
            if (curEvent != null)
            {
                curEvent.RemoveFromParent();
                curEvent.Dispose();
                curEvent = null;
            }
            this.RemoveAllChildren();
            base.Dispose(disposing);
        }

        public EventManager(ScriptHelper helper)
        {
            this.SetScriptHelper(helper);
        }

        public void SetScriptHelper(ScriptHelper helper)
        {
            this.scriptHelper = helper;
        }

        public void SetActor(Actor actor)
        {
            this.actor = actor;
        }

        public void OnEvent(int id,CCNode eventNode)
        {
            this.scriptHelper.CallFuction("OnEventOver", this.RuningID);
            //
            if (eventNode!=null)
            {
                this.RemoveChild(eventNode);
                eventNode.Dispose();
                eventNode = null;
            }

            //
            //回调lua OnEventCall()
            scriptHelper.CallFuction("OnEventCall", id);
            //
            if (id == Actor.EVENT_ID_OVER)
            {
                End();
            }
        }

        public int RuningID
        {
            set { runingID = value; }
            get { return runingID; }
        }

        /// <summary>
        /// 执行一个事件节点
        /// </summary>
        /// <param name="eventNode"></param>
        public void RunEvent(EventNode eventNode)
        {
            this.RemoveAllChildren(true);

            eventNode.EventManager = this;
            eventNode.Actor = actor;

            this.AddChild(eventNode);
            this.curEvent = eventNode;
            this.RuningID = eventNode.ID;

        }

        /// <summary>
        /// 开始入口
        /// </summary>
        public void Begin()
        {
            //this.OnEvent(0);
            scriptHelper.CallFuction("OnEventCall", Actor.EVENT_ID_BEGIN);
        }

        public void End()
        {
            this.actor.OnScriptOver();
        }
    }
}
