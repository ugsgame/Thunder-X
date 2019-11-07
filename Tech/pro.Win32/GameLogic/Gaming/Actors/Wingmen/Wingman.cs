using MatrixEngine.Cocos2d;
using MatrixEngine.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thunder.Common;
using Thunder.Game;
using Thunder.GameLogic.Common;
using Thunder.GameLogic.Gaming.Actors.Players;
using Thunder.GameLogic.Gaming.BulletSystems;

namespace Thunder.GameLogic.Gaming.Actors.Wingmen
{
    public class Wingman : Actor
    {
        protected BulletSystem bullet;
        protected Player player;
        //相对玩家的坐标偏移量
        protected Vector2 posOffset = Vector2.Zero;

        bool direction;
        float stepLen = 500f;
        //行为
        protected enum Behavior
        {
            Null,
            Opening,
            Opened,
            Closing,
            Closed,
            FlyOut,
        }

        protected Behavior curBehavior;
        public Thunder.GameLogic.Gaming.WingmanSpawner.WingmanID WingmanID
        {
            get;
            set;
        }

        FighterData.FighterLevelData levelData;
        WingmanData wingmanData;

        private bool isShowing = true;
        public bool IsShowing
        {
            set { isShowing = value; }
            get { return isShowing; }
        }

        public Player Player
        {
            set
            {
                this.player = value;
                this.Postion = player.Postion;
            }
        }

        public Wingman(SpawnInfo spawnInfo)
            : base(spawnInfo)
        {
            this.Init(this.Info);
        }

        protected virtual void Init(SpawnInfo spawnInfo)
        {
            this.ZOrder = PlayingScene.ZOrder_player;
            this.Filter = FilterType.FilterPlayer;
            Info.delayTime = 0;

            //默认动画
            try
            {
                animActions[AnimName.Null] = Info.animName + "_fly1";
                animActions[AnimName.待机1] = Info.animName + "_fly1";
                animActions[AnimName.变形1] = Info.animName + "_transform";
                animActions[AnimName.变形2] = Info.animName + "_return";
            }
            catch (Exception e)
            {
                Utils.PrintException(e);
            }
            this.GotoBehavior(Behavior.Null);
        }

        protected override void OnEnter()
        {
            base.OnEnter();
            BulletEmitter.UnregisterTarget(this);
            this.GotoBehavior(Behavior.Closed);

            levelData = PlayingScene.Instance.CurFighterLevelData;
            wingmanData = GameData.Instance.GetWingmanData(this.WingmanID);
            if (!this.isShowing)
            {
                this.Info.damage = wingmanData.damage + levelData.attachPlayerDamage;
            }
        }

        protected override void OnExit()
        {
            base.OnExit();
            this.CloseFire();
        }

        public new virtual void Reset()
        {
            this.Postion = player.Postion;
            this.GotoBehavior(Behavior.Null);
        }

        public Vector2 PosOffset
        {
            set { posOffset = value; }
            get { return posOffset; }
        }

        protected override void OnAnimStart(Animable anim, string animName)
        {
        }

        protected override void OnAnimComplete(Animable anim, string animName)
        {
        }

        protected override void OnAnimLoopComplete(Animable anim, string animName)
        {
        }

        protected override void OnAnimFrameEvent(Animable anim, string eventName, int originFrameIndex, int currentFrameIndex)
        {
        }

        protected override void OnArmatureEnter(Spawn.ArmCollider collider)
        {
        }

        protected override void OnArmatureExit(Spawn.ArmCollider collider)
        {
        }

        protected override void OnArmatureStay(Spawn.ArmCollider collider)
        {
        }

        protected virtual bool GotoBehavior(Behavior nextBehavior)
        {
            if (nextBehavior == curBehavior) return false;

            switch (nextBehavior)
            {
                case Behavior.Null:
                    {
                        //this.Postion = player.Postion;
                        this.CloseFire();
                        this.IsVisible = false;
                    }
                    break;
                case Behavior.Opening:
                    {
                        if (curBehavior == Behavior.Opened)
                            return false;

                        this.Postion = player.Postion;
                        this.IsVisible = true;
                        this.CloseFire();
                    }
                    break;
                case Behavior.Opened:
                    {
                        this.Postion = player.Postion + this.posOffset;
                        this.IsVisible = true;
                        this.OpenFire();
                    }
                    break;
                case Behavior.Closing:
                    {
                        if (curBehavior == Behavior.Closed)
                            return false;

                        this.IsVisible = true;
                        this.CloseFire();
                    }
                    break;
                case Behavior.Closed:
                    {
                        this.Postion = player.Postion;
                        this.IsVisible = false;
                        this.CloseFire();
                    }
                    break;
                case Behavior.FlyOut:
                    {
                        this.CloseFire();
                        if (this.direction)
                        {
                            this.RunSequenceActions(new CCActionEaseIn(new CCActionMoveBy(1f,Config.SCREEN_WIDTH,0),0.5f),new CCActionCallFunc(this.Reset));
                        }
                        else
                        {
                            this.RunSequenceActions(new CCActionEaseIn(new CCActionMoveBy(1f, -Config.SCREEN_WIDTH, 0), 0.5f), new CCActionCallFunc(this.Reset));
                        }
                    }
                    break;
                default:
                    break;
            }
            curBehavior = nextBehavior;
            return true;
        }

        protected override bool OnChangeState(Actor.State currentState, Actor.State nextState)
        {
            base.OnChangeState(currentState, nextState);

            switch (nextState)
            {
                case State.Null:
                    PlayAnim(AnimName.Null);
                    break;
                case State.Idle:
                    PlayAnim(AnimName.待机1);
                    break;
                case State.Walk:
                    break;
                case State.Retreat:
                    break;
                case State.Ready:
                    break;
                case State.ByAttack:
                    break;
                case State.Frenzy:
                    {
                        PlayAnim(AnimName.变形1);
                    }
                    break;
                case State.Attack:
                    break;
                case State.Skill:
                    break;
                case State.Escape:
                    break;
                case State.Dead:
                    break;
                case State.Event:
                    break;
                case State.Count:
                    break;
                default:
                    break;
            }

            return true;
        }

        public virtual void Open()
        {
            this.GotoBehavior(Behavior.Opening);
        }

        public virtual void Close()
        {
            this.GotoBehavior(Behavior.Closing);
        }

        public virtual void FlyOut(bool dir)
        {
            this.direction = dir;
            this.GotoBehavior(Behavior.FlyOut);
        }

        public override void OnUpdate(float dTime)
        {
            base.OnUpdate(dTime);

            Vector2 pos = player.Postion + posOffset;
            Vector2 newPos = this.Postion;
            switch (curBehavior)
            {
                case Behavior.Null:
                    {
                        this.Postion = player.Postion;
                    }
                    break;
                case Behavior.Opening:
                    {
                        if (Utils.Follow(dTime, this.Info.speed, this.Postion, pos, ref newPos, 0.2f,10f))
                        {
                            GotoBehavior(Behavior.Opened);
                        }
                        this.Postion = newPos;
                    }
                    break;
                case Behavior.Opened:
                    {
                        Utils.Follow(dTime, this.Info.speed, this.Postion, pos, ref newPos, 0.2f);
                        this.Postion = newPos;
                    }
                    break;
                case Behavior.Closing:
                    {
                        if (Utils.Follow(dTime, this.Info.speed, this.Postion, player.Postion, ref newPos))
                        {
                            GotoBehavior(Behavior.Closed);
                        }
                        this.Postion = newPos;
                        this.CloseFire();
                    }
                    break;
                case Behavior.Closed:
                    {
                        this.Postion = player.Postion;
                        this.CloseFire();
                    }
                    break;
                default:
                    break;
            }
        }

        public virtual void OnLevelUp(int level)
        {

        }

        public virtual void OnFrenzy()
        {

        }
    }
}
