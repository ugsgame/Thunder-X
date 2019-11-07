
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Thunder.GameLogic.Gaming.Actors.Players;

using MatrixEngine.Cocos2d;
using Thunder.GameLogic.Gaming.BulletSystems;
using MatrixEngine.Math;
using Thunder.Game;
using Thunder.GameLogic.Common;

namespace Thunder.GameLogic.Gaming.Actors.Drops
{
    /// <summary>
    /// 掉落物配置
    /// </summary>
    public struct DropConfig
    {
        public Vector2 startPos;    //初始坐标

        public float speed;         //初始速度
        public float speedVar;      //变化范围

        public float speedDecay;    //速度衰变值
        public float speedLimit;    //速度限值 

        public float angle;         //初始运动方向角度
        public float angleVar;      //变化范围

        public float waitingTime;
        public float waitingTimeVar;
    }
    /// <summary>
    /// 所有掉落物基类
    /// </summary>
    /// 
    public class Drop : CCNode
    {
        protected enum State
        {
            Null,
            Float,
            Attach,
            Dead
        };

        //
        protected DropConfig mConfig = new DropConfig();
        //
        protected Vector2 curDir;
        protected Vector2 floatDir;
        protected float curSpeed;
        protected float curWaitingTime;
        protected bool isStable;
        protected State curState;

        protected float timeToWaiting;
        //
        protected DropType mType;
        public virtual DropType Type
        {
            get { return mType; }
        }

        protected bool mIsLive;
        public bool IsLive
        {
            set { mIsLive = value; }
            get { return mIsLive; }
        }

        //         protected Actor mOwner;
        //         public virtual Actor Owner
        //         {
        //             set { mOwner = value; }
        //             get { return mOwner; }
        //         }

        protected Player mPlayer;
        public virtual Player AttachPlayer
        {
            set { mPlayer = value; }
            get { return mPlayer; }
        }
        //
        public virtual DropConfig Config
        {
            set { mConfig = value; }
            get { return mConfig; }
        }

        public virtual Vector2 StartPos
        {
            set { mConfig.startPos = value; }
            get { return mConfig.startPos; }
        }

        public virtual float Speed
        {
            set { mConfig.speed = value; }
            get { return mConfig.speed; }
        }
        public virtual float SpeedVar
        {
            set { mConfig.speedVar = value; }
            get { return mConfig.speedVar; }
        }

        public virtual float SpeedDecay
        {
            set { mConfig.speedDecay = value; }
            get { return mConfig.speedDecay; }
        }
        public virtual float SpeedLimit
        {
            set { mConfig.speedLimit = value; }
            get { return mConfig.speedLimit; }
        }

        public virtual float Angle
        {
            set { mConfig.angle = value; }
            get { return mConfig.angle; }
        }
        public virtual float AngleVar
        {
            set { mConfig.angleVar = value; }
            get { return mConfig.angleVar; }
        }

        public virtual float WaitingTime
        {
            set { mConfig.waitingTime = value; }
            get { return mConfig.waitingTime; }
        }
        public virtual float WaitingTimeVar
        {
            set { mConfig.waitingTimeVar = value; }
            get { return mConfig.waitingTimeVar; }
        }
        //

        protected CCNode mWorldNode;
        public CCNode WorldNode
        {
            set 
            { 
                mWorldNode = value;
                if (mDisplay != null && mDisplay.IsUsingBatch)
                {
                    SpriteBatchManager.Instance.WorldNode = value;
                }
            }
            get { return mWorldNode; }
        }

        protected BulletDisplay mDisplay;
        public virtual BulletDisplay Display
        {
            set
            {
                if (mDisplay != null)
                {
                    mDisplay.RemoveFromParent();
                    this.mDisplay.Dispose();
                }
                mDisplay = value;
                mDisplay.IsVisible = false;
                if (mDisplay.IsUsingBatch)
                {
                    CCSpriteBatchNode node = SpriteBatchManager.Instance.GetBatchNode(mDisplay.BatchImage);
                    node.AddChild(mDisplay.DisplayNode);
                }
                else
                {
                    this.AddChild(mDisplay);
                }
            }
            get { return mDisplay; }
        }

        //Override for batchNode

        protected void UpdateBatchNode()
        {
            //this.mDisplay.IsVisible = this.IsVisible;
            this.mDisplay.DisplayNode.Postion = this.Postion;
            this.mDisplay.DisplayNode.Rotation = this.Rotation;
            this.mDisplay.DisplayNode.Scale = this.Scale;
            this.mDisplay.DisplayNode.ZOrder = this.ZOrder;
        }

        public Drop()
        {
            this.ZOrder = PlayingScene.ZOrder_drop;
        }

        protected override void Dispose(bool disposing)
        {
            if (this.mDisplay != null)
            {
                mDisplay.RemoveFromParent();
                mDisplay.Dispose();
                mDisplay = null;
            }
            base.Dispose(disposing);
        }
        //         public Drap(Actor owner)
        //         {
        //             mOwner = owner;
        //         }

        public virtual void Init()
        {
            this.IsLive = true;
            this.Postion = mConfig.startPos;
            this.curSpeed = mConfig.speed - mConfig.speedVar * MathHelper.Random_minus0_1();
            this.curWaitingTime = mConfig.waitingTime - mConfig.waitingTimeVar * MathHelper.Random_minus0_1();
            this.floatDir = RandomDir();

            isStable = false;

            float angle = mConfig.angle - mConfig.angleVar * MathHelper.Random_minus0_1();
            this.curDir.X = MathHelper.Sin(MathHelper.DegreesToRadians(angle));
            this.curDir.Y = MathHelper.Cos(MathHelper.DegreesToRadians(angle));

            this.timeToWaiting = 0;
            GotoState(State.Float);
        }

        public virtual void OnEnter()
        {

        }
        public virtual void OnExit()
        {

        }

        public virtual void Reset()
        {
            GotoState(State.Null);
            this.IsLive = false;
            if (this.mDisplay != null)
            {
                this.mDisplay.IsVisible = false;
            }
        }

        protected virtual Vector2 RandomDir()
        {
            Vector2 dir;
            float angle = 360 * MathHelper.Random_minus0_1();
            dir.X = MathHelper.Sin(MathHelper.DegreesToRadians(angle));
            dir.Y = MathHelper.Cos(MathHelper.DegreesToRadians(angle));
            return dir;
        }

        /// <summary>
        /// 碰到边界反弹回来
        /// </summary>
        /// <returns></returns>
        protected Vector2 WorldAABB(Vector2 dir)
        {
            if (this.mWorldNode != null)
            {
                float angle = dir.ToDegrees();
                Rect rect = mWorldNode.BoundingBox;
                rect.origin.X = 0;
                rect.origin.Y = 0;
                if (!rect.ContainsPoint(this.Postion))
                {
                    //return Utils.AngleToVector(angle + 180);
                    if (this.PostionX < rect.GetMinX())
                    {
                        this.PostionX = rect.GetMinX();
                    }
                    else if (this.PostionX > rect.GetMaxX())
                    {
                        this.PostionX = rect.GetMaxX();
                    }

                    if (this.PostionY < rect.GetMinY())
                    {
                        this.PostionY = rect.GetMinY();
                    }
                    else if (this.PostionY > rect.GetMaxY())
                    {
                        this.PostionY = rect.GetMaxY();
                    }
                    return Utils.AngleToVector(angle + 180);
                }
                else
                {
                    return dir;
                }
            }
            else
            {
                return dir;
            }
        }

        protected float DisFromPlayer()
        {
            double a1 = (this.PostionX - this.mPlayer.PostionX) * (this.PostionX - this.mPlayer.PostionX);
            double a2 = (this.PostionY - this.mPlayer.PostionY) * (this.PostionY - this.mPlayer.PostionY);
            return (float)Math.Sqrt(a1 + a2);
        }

        protected void GotoState(Drop.State state)
        {
            curState = state;
            switch (state)
            {
                case State.Float:
                    break;
                case State.Attach:
                    break;
                case State.Dead:
                    break;
                default:
                    break;
            }
        }

        public virtual void Play()
        {
            Play(true);
        }

        public virtual void Play(bool loop)
        {
            if (mDisplay != null)
            {
                mDisplay.Play(loop);
            }
        }

        /// <summary>
        /// 漂浮
        /// </summary>
        protected virtual void Floating(float dt)
        {
            Vector2 tmp;
            if (!isStable)
            {
                if (SpeedDecay > 0)
                {
                    if (curSpeed < mConfig.speedLimit)
                    {
                        curSpeed += SpeedDecay;

                        this.curDir = this.WorldAABB(this.curDir);
                    }
                    else
                    {
                        curSpeed = mConfig.speedLimit;
                        curDir = this.floatDir;
                        isStable = true;
                    }
                }
                else
                {
                    if (curSpeed > mConfig.speedLimit)
                    {
                        curSpeed += SpeedDecay;
                        this.curDir = this.WorldAABB(this.curDir);
                    }
                    else
                    {
                        curSpeed = mConfig.speedLimit;
                        curDir = this.floatDir;
                        isStable = true;
                    }
                }
            }
            else
            {
                this.curDir = this.WorldAABB(this.curDir);
            }


            tmp = /*curDir.Normalize*/curDir * curSpeed * dt;
            this.Postion += tmp;

            this.timeToWaiting += dt;
            if (mConfig.waitingTime != -1 && this.timeToWaiting >= this.curWaitingTime)
            {
                GotoState(State.Attach);
            }

            //Console.WriteLine(this.timeToWaiting);
        }
        /// <summary>
        /// 吸附到主角
        /// </summary>
        protected virtual void Adsorbent(float dt)
        {
            if (this.mPlayer != null && !this.mPlayer.IsDead)
            {
                this.curSpeed += 80;
                float speed = this.curSpeed * dt;
                float dis = DisFromPlayer();
                float rate = speed / dis;
                Vector2 dir = (this.mPlayer.Postion - this.Postion) * rate;
                if (dis <= 50.0f)
                {
                    GotoState(State.Dead);
                }
                else
                {
                    this.Postion += dir;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        protected virtual void DeadLogic(float dt)
        {
            mIsLive = false;
            this.AttachPlayer.OnDropAdsorbent(this);
            this.OnAdsorbent();
        }

        protected virtual void UpdateDisplay()
        {
            if (this.mDisplay != null)
            {
                this.mDisplay.IsVisible = mIsLive;
                if (this.mDisplay.IsUsingBatch)
                {
                    this.UpdateBatchNode();
                }
            }
        }

        public virtual void OnUpdate(float dTime)
        {
            switch (this.curState)
            {
                case State.Float:
                    this.Floating(dTime);
                    break;
                case State.Attach:
                    this.Adsorbent(dTime);
                    break;
                case State.Dead:
                    this.DeadLogic(dTime);
                    break;
                default:
                    break;
            }

            UpdateDisplay();
        }

        protected void BaseConfig(Drop drap)
        {
            drap.Config = this.Config;
            drap.mWorldNode = this.mWorldNode;
            drap.mPlayer = this.mPlayer;
        }

        protected virtual void OnAdsorbent()
        {

        }
    }
}
