
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MatrixEngine.Cocos2d;
using MatrixEngine.Math;
using Thunder.GameLogic.Gaming.Actors.Players;

namespace Thunder.GameLogic.Gaming.Actors.Drops
{
    //TODO:改成CCNode,统一由DropMananger处理update
    public class DropEmitter:CCLayer
    {

        List<Drop> mDraps = new List<Drop>();
        Drop mDrapTemple;

        int mTotalDraps;
        int mAllocatedDraps;

        int mDrapIdx;
        int mDrapCount;

        DropConfig mDrapConfig = new DropConfig();

        protected CCNode mWorldNode = DropManager.WorldNode;
        public CCNode WorldNode
        {
            set { mWorldNode = value; }
            get { return mWorldNode; }
        }

        public DropConfig Config
        {
            set { mDrapConfig = value; }
            get { return mDrapConfig; }
        }

        protected Actor mUser;
        public virtual Actor User
        {
            set { mUser = value; }
            get { return mUser; }
        }

        protected Player mPlayer;
        public virtual Player Player
        {
            set { mPlayer = value; }
            get { return mPlayer; }
        }

        //
        public Vector2 StartPos
        {
            set { mDrapConfig.startPos = value; }
            get { return mDrapConfig.startPos; }
        }

        public float Speed
        {
            set { mDrapConfig.speed = value; }
            get { return mDrapConfig.speed; }
        }
        public float SpeedVar
        {
            set { mDrapConfig.speedVar = value; }
            get { return mDrapConfig.speedVar; }
        }

        public float SpeedDecay
        {
            set { mDrapConfig.speedDecay = value; }
            get { return mDrapConfig.speedDecay; }
        }
        public float SpeedLimit
        {
            set { mDrapConfig.speedLimit = value; }
            get { return mDrapConfig.speedLimit; }
        }

        public float Angle
        {
            set { mDrapConfig.angle = value; }
            get { return mDrapConfig.angle; }
        }
        public float AngleVar
        {
            set { mDrapConfig.angleVar = value; }
            get { return mDrapConfig.angleVar; }
        }

        public float WaitingTime
        {
            set { mDrapConfig.waitingTime = value; }
            get { return mDrapConfig.waitingTime; }
        }
        public float WaitingTimeVar
        {
            set { mDrapConfig.waitingTimeVar = value; }
            get { return mDrapConfig.waitingTimeVar; }
        }      
        //

        public DropEmitter(Drop temple)
        {
            mDrapTemple = temple;
        }

        protected override void Dispose(bool disposing)
        {
            foreach (var item in mDraps)
            {
                Drop drap = (Drop)item;
                drap.Dispose();
                drap = null;
            }
            mDraps.Clear();
            mDraps = null;
            base.Dispose(disposing);
        }

        public virtual Drop AddDrap()
        {
            if (mUser != null)
            {
                this.mDrapConfig.startPos = mUser.Postion;
            }
            return AddDrap(this.mDrapConfig);
        }

        public virtual void AddDrap(int num)
        {
            for (int i = 0; i < num; i++)
            {
                AddDrap();
            }
        }

        public virtual Drop AddDrap(Vector2 pos)
        {
            this.mDrapConfig.startPos = pos;
            return AddDrap(this.mDrapConfig);
        }
        public virtual void AddDrap(Vector2 pos,int num)
        {
            for (int i = 0; i < num; i++)
            {
                AddDrap(pos);
            }
        }

        public virtual Drop AddDrap(DropConfig config)
        {
            Drop drap;
            if (this.IsFull())
            {
                drap = (Drop)mDrapTemple.Copy();
                mDraps.Add(drap);
                this.mWorldNode.AddChild(drap);
                this.mTotalDraps = mDraps.Count;
            }
            else
            {
                drap = mDraps[mDrapCount];
            }
            this.InitDrap(ref drap,config);

            ++mDrapCount;
            ++DropManager.AllDrapCount;

            return drap;
        }
        public void AddDrap(DropConfig config,int num)
        {
            for (int i = 0; i < num; i++)
            {
                this.AddDrap(config);
            }
        }

        protected virtual void InitDrap(ref Drop drap, DropConfig config)
        {
            drap.Config = config;
            drap.WorldNode = mWorldNode;
            drap.AttachPlayer = mPlayer;
            drap.Init();
            drap.Play();
        }

        public bool InitWithTotalDraps(int num)
        {          
            mTotalDraps = num;
            mAllocatedDraps = num;
            mDraps.Clear();
            for (int i = 0; i < num; i++)
            {
                Drop drop = (Drop)mDrapTemple.Copy();
                mDraps.Add(drop);
                mWorldNode.AddChild(drop);
            }
            return true;
        }

        public bool IsFull()
        {
            return (mDrapCount == mTotalDraps);
        }

        protected virtual void Allocated(float time)
        {
             Console.WriteLine("Allocated");
//             Console.WriteLine("mDrapCount:" + mDraps.Count);
//             Console.WriteLine("mAllocatedDraps:" + mAllocatedDraps);
            if (mDraps.Count > mAllocatedDraps)
            {
                int conut = mDraps.Count - mAllocatedDraps;
                int allocate = 0;
                for (int i = mDraps.Count-1; i >=0; i--)
                {
                    Drop drap = mDraps[i];
                    if (!drap.IsLive)
                    {
                        drap.RemoveFromParent();
                        drap.Dispose();
                        mDraps.Remove(drap);
                        drap = null;
                        --conut;
                        ++allocate;
                    }
                    if (conut <= 0)
                    {
                        break;
                    }
                }
                mDrapCount = mDraps.Count;
                mTotalDraps = mDrapCount;
                Console.WriteLine("AllocateDrops:" + allocate);
            }
        }
        bool isBeginAllocated;
        bool isAllocated;
        protected virtual void BeginAllocated(float time)
        {
            isBeginAllocated = false;
            isAllocated = true;
            this.Unschedule("BeginAllocated");
            this.Schedule("Allocated", 20);
        }

        public override void OnEnter()
        {
            base.OnEnter();

            foreach (var item in mDraps)
            {
                item.OnEnter();
            }

            isBeginAllocated = true;
            this.Schedule("BeginAllocated",50*MathHelper.Random_minus0_1());
        }

        public override void OnExit()
        {
            //base.OnExit();

            foreach (var item in mDraps)
            {
                item.OnExit();
                item.Reset();
            }

            //Allocated(0);
            if (isBeginAllocated)
            {
                isBeginAllocated = false;
                this.Unschedule("BeginAllocated");
            }
            if (isAllocated)
            {
                isAllocated = false;
                this.Unschedule("Allocated");
            }
        }

        public override void OnUpdate(float dTime)
        {
            //base.OnUpdate(dTime);

            mDrapIdx = 0;
            while (mDrapIdx < mDrapCount)
            {
                Drop d = mDraps[mDrapIdx];
                if (d.IsLive)
                {
                    d.OnUpdate(dTime);
                    ++mDrapIdx;
                }
                else
                {
                    if (mDrapIdx != mDrapCount - 1)
                    {
                        var temp = mDraps[mDrapIdx];
                        mDraps[mDrapIdx] = mDraps[mDrapCount - 1];
                        mDraps[mDrapCount - 1] = temp;
                    }

                    --mDrapCount;
                    --DropManager.AllDrapCount;
                }
            }
        }
    }
}
