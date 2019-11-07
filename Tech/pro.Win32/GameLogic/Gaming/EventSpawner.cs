
using System.Collections.Generic;

using MatrixEngine.Cocos2d;
using Thunder.GameLogic.Gaming.Actors;
using Thunder.GameLogic.Gaming.Actors.Players;
using Thunder.GameLogic.Gaming.Actors.Bosses;
using Thunder.GameLogic.Gaming.Actors.Enemys;
using System;
using Thunder.Game;
using Thunder.GameLogic.Gaming.Actors.Drops;


namespace Thunder.GameLogic.Gaming
{
    /// <summary>
    /// 事件生成器（小型关卡管理器）
    /// </summary>
    public class EventSpawner : CCNode,InterfaceGameState
    {
        LevelManager levelManager;

        EventInfo info;
        CCLayer actorLayer;

        List<Actor> actors = new List<Actor>();

        bool isDone;

        public enum State
        {
            sNull,
            sPlaying,
            sEnd,
        }

        public State CurrentState
        {
            get;
            protected set;
        }

        public string Name
        {
            get { return info.name; }
            set { info.name = value; }
        }

        protected bool isTimeToBegin;
        protected bool isTimeToEnd;

        /// <summary>
        /// Actor摆放的图层(就是GameLayer)
        /// </summary>
        public CCLayer ActorLayer
        {
            set { actorLayer = value; }
            get { return actorLayer; }
        }

        public LevelManager LevelManager
        {
            set { this.levelManager = value; }
            get { return this.levelManager; }
        }

        public EventSpawner(EventInfo eventInfo)
        {
            InitEvent(eventInfo);
        }

        public EventSpawner(EventInfo eventInfo, LevelManager levelManager)
        {
            InitEvent(eventInfo);
            this.levelManager = levelManager;
            this.actorLayer = levelManager.GameLayer;
        }

        protected override void Dispose(bool disposing)
        {
            //if (disposing)
            {
                if (isTimeToBegin) this.Unschedule("TimeToBegin");
                if (isTimeToEnd) this.Unschedule("TimeToEnd");

                foreach (var item in actors)
                {
                    this.actorLayer.RemoveChild(item, true);
                    item.Dispose();
                }
                actors.Clear();

                levelManager = null;
                actorLayer = null;
                actors = null;
            }
            base.Dispose(disposing);
        }
        /// <summary>
        /// 初始化
        /// TODO:不应该在这里加载
        /// </summary>
        /// <param name="eventInfo"></param>
        /// <returns></returns>
        protected bool InitEvent(EventInfo eventInfo)
        {
            this.info = eventInfo;
            foreach (var item in this.info.spawnInfoCache)
            {
                SpawnInfo spawnInfo = item;
                Actor actor = null;
                switch (spawnInfo.spawnType)
                {
                    case SpawnType.Actor_Player:
                        //actor = new Player(spawnInfo, this);
                        break;
                    case SpawnType.Actor_Boss:
                        actor = new Boss(spawnInfo, this);
                        break;
                    case SpawnType.Actor_Emeny:
                        {
                            if (spawnInfo.actorId >= ActorID.Enemy1 && spawnInfo.actorId < ActorID.Elite1)
                            {
                                actor = new Enemy(spawnInfo, this);
                            }
                            else if (spawnInfo.actorId >= ActorID.Elite1 && spawnInfo.actorId < ActorID.Drop1)
                            {
                                actor = new Elite(spawnInfo, this);
                            }
                        }
                        break;
                    default:
                        break;
                }
                if (actor != null)
                {
                    actors.Add(actor);
                }
            }
            CurrentState = State.sNull;
            return true;
        }
        /// <summary>
        /// 当角色死亡回调
        /// </summary>
        /// <param name="actor"></param>
        public void OnRemoveActor(Actor actor)
        {
            try
            {
                this.actorLayer.RemoveChild(actor, true);
                this.actors.Remove(actor);
                actor.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }


            //Console.WriteLine("Actors Count:" + this.actors.Count);
            //Console.WriteLine("Actors crossTime:" + this.info.crossCondition);
            //是否所有怪都over了
            if (!this.isDone)
            {
                if (this.actors.Count == 0)
                {
                    //若为时间过关，但怪又死光了，直接过关
                    if (this.info.crossCondition)
                    {
                        TimeToEnd(this.info.crossTime);
                    }
                    else
                    {
                        OnEnd();
                    }
                }
            }

        }

        public void TansToGems(DropType droptype)
        {
            foreach (var item in actors)
            {
                item.BulletToGem(droptype);
            }
        }

        public void KillAllEnemys(float damage)
        {
            for (int i = actors.Count-1; i >= 0; i--)
            {
                if (actors[i].Filter == FilterType.FilterEnemy)
                {
                    actors[i].BeAttacked(damage);
                }          
            }
        }

        public void OnPause()
        {
            foreach (var item in actors)
            {
                item.PauseSchedulerAndActions();
            }         
        }

        public void OnResume()
        {
            foreach (var item in actors)
            {
                item.ResumeSchedulerAndActions();
            }          
        }

        public void Play()
        {

            if (CurrentState != State.sPlaying)
            {
                //没有敌人就直接跳过
                if (this.actors.Count == 0)
                {
                    OnEnd();
                }
                else
                {
                    //
                    isTimeToBegin = true;
                    this.Schedule("TimeToBegin", info.waitingTime);
                    CurrentState = State.sPlaying;
                }

            }
        }

        public bool IsDone()
        {
            return isDone;
        }

        public void TimeToBegin(float time)
        {
            foreach (var item in actors)
            {
                this.actorLayer.AddChild(item);
            }
            //按时间条件过关
            if (this.info.crossCondition)
            {
                isTimeToEnd = true;
                Schedule("TimeToEnd", this.info.crossTime);
            }
            isTimeToBegin = false;
            Unschedule("TimeToBegin");
        }

        public void TimeToEnd(float time)
        {
            OnEnd();
            isTimeToEnd = false;
            Unschedule("TimeToEnd");
        }

        private void OnEnd()
        {
            Console.WriteLine("打完一波怪了！！！");

            isDone = true;
            try
            {
                this.levelManager.OnEventOver(this);
                CurrentState = State.sEnd;
            }
            catch (Exception e)
            {
                Utils.PrintException(e);
            }
        }
    }
}
