
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Thunder.Common;
using Thunder.Game;

using MatrixEngine;
using MatrixEngine.CocoStudio;
using MatrixEngine.CocoStudio.Armature;


namespace Thunder.GameLogic.Gaming.Actors
{
    public abstract class Spawn : ActorBehavior, IAnimationEvent
    {

        private SpawnInfo spawnInfo;

        private static int seqIdCount;
        /// <summary>
        /// 元素的id
        /// </summary>
        public readonly int ID = seqIdCount++;
        /// <summary>
        /// 方向
        /// true 右
        /// false 左
        /// </summary>
        private bool _direction;

        /// <summary>
        /// 动画
        /// </summary>
        protected Animable spawnAnim;

        /// <summary>
        /// 自己的传感器
        /// </summary>
        protected ArmCollider MyCollider;
        /// <summary>
        /// 对方的传感器（其它地方不要引用）
        /// </summary>
        private ArmCollider OtherCollider;

        /// <summary>
        /// 自己的传感器信息
        /// </summary>
        public ArmCollider ColliderInfo
        {
            get { return MyCollider; }
        }

        public Animable Animable
        {
            get { return spawnAnim; }
        }

        /// <summary>
        /// 自己的传感器信息
        /// </summary>
        public ArmCollider OtherColliderInfo
        {
            get { return OtherCollider; }
        }



        //动画碰撞器
        public struct ArmCollider
        {
            public Spawn Spawn;
            public Animable Animable;
            public string BoneName;
        }

        public Spawn(string resPath, string armaName)
        {
            spawnAnim = new Animable(this, resPath, armaName);
        }

        protected override void OnStart()
        {
            //base.OnStart();
        }

        protected override void OnEnable()
        {
            //注册碰撞
            //SpawnAnim.RegisterCollider();  
        }

        protected override void OnExit()
        {
            base.OnExit();
            //解除
            //SpawnAnim.UnRegisterCollider();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        protected override void Dispose(bool disposing)
        {
            if (isTimeCall)
            {
                this.Unschedule("OnTimeCall");
            }

            spawnAnim.Dispose();
            spawnAnim = null;
            this.RemoveAllChildren();

            base.Dispose(disposing);
        }

        /// <summary>
        /// 设置方向
        /// 左边是true
        /// 右边是false
        /// </summary>
        public virtual bool Direction
        {
            set
            {
                if (value)
                {
                    //this.ScaleX = 1;
                    this.spawnAnim.FilpX(false);
                }
                else
                {
                    //this.ScaleX = -1;
                    this.spawnAnim.FilpX(true);
                }
                _direction = value;
            }
            get
            {
                return _direction;
            }
        }


        public virtual bool PlayAnim(string name)
        {
            spawnAnim.PlayAnim(name, true);
            return true;
        }

        public virtual bool PlayAnim(string name, bool loop)
        {
            spawnAnim.PlayAnim(name, loop);
            return true;
        }

        public virtual void Resume()
        {
            spawnAnim.Resume();
        }

        public virtual void Pause()
        {
            spawnAnim.Pause();
        }

        public virtual void Stop()
        {
            spawnAnim.Stop();
        }

        public SpawnInfo Info
        {
            set { this.spawnInfo = value; }
            get { return spawnInfo; }
        }

        protected bool isTimeCall;

        /// <summary>
        /// 调TimeActionCall后指定时间回调此函数
        /// </summary>
        /// <param name="time"></param>
        protected virtual void OnTimeCall(float time)
        {
            isTimeCall = false;
            this.Unschedule("OnTimeCall");
        }
        /// <summary>
        /// 指定时间回调OnTimeCall函数
        /// </summary>
        /// <param name="waitingTime">等待时间</param>
        protected virtual void TimeActionCall(float waitingTime)
        {
            isTimeCall = true;
            this.Schedule("OnTimeCall", waitingTime);
        }

        /// <summary>
        /// 动画播放开始时回调
        /// </summary>
        /// <param name="anim"></param>
        /// <param name="animName"></param>
        protected abstract void OnAnimStart(Animable anim, string animName);

        /// <summary>
        /// 单次播放的动画播放回后回调
        /// </summary>
        /// <param name="anim"></param>
        protected abstract void OnAnimComplete(Animable anim, string animName);

        /// <summary>
        /// 动画每循环播放一遍回调一次
        /// </summary>
        /// <param name="anim"></param>
        /// <param name="animName"></param>
        protected abstract void OnAnimLoopComplete(Animable anim, string animName);

        /// <summary>
        /// 帧事件回调
        /// </summary>
        /// <param name="anim"></param>
        /// <param name="evt"></param>
        /// <param name="originFrameIndex"></param>
        /// <param name="currentFrameIndex"></param>
        protected abstract void OnAnimFrameEvent(Animable anim, string eventName, int originFrameIndex, int currentFrameIndex);

        /// <summary>
        /// 动画碰撞回调
        /// </summary>
        /// <param name="collider"></param>
        protected abstract void OnArmatureEnter(ArmCollider collider);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collider"></param>
        protected abstract void OnArmatureExit(ArmCollider collider);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collider"></param>
        protected abstract void OnArmatureStay(ArmCollider collider);

        ////////////////////////////////////////////////////////////////////////////////

        #region 内部调用类，禁止访问

        private void native_OnArmatureEnter(Spawn SpawnA, Spawn SpawnB, string BoneA, string BoneB)
        {
            try
            {
                //ArmCollider OtherCollider = new ArmCollider();
                this.MyCollider.Spawn = SpawnA;
                this.MyCollider.Animable = SpawnA.spawnAnim;
                this.MyCollider.BoneName = BoneA;
                OtherCollider.Spawn = SpawnB;
                OtherCollider.Animable = SpawnB.spawnAnim;
                OtherCollider.BoneName = BoneB;

                //Console.WriteLine("native_OnArmatureStay SpawnA=" + SpawnA + " SpawnB=" + SpawnB + " BoneA=" + BoneA + " BoneB=" + BoneB + " IsAttack=" + IsAttack(OtherCollider));
                OnArmatureEnter(OtherCollider);
            }
            catch (Exception e)
            {
                Utils.PrintException(e);
            }
        }

        private void native_OnArmatureExit(Spawn SpawnA, Spawn SpawnB, string BoneA, string BoneB)
        {
            try
            {
                //ArmCollider OtherCollider = new ArmCollider();
                this.MyCollider.Spawn = SpawnA;
                this.MyCollider.Animable = SpawnA.spawnAnim;
                this.MyCollider.BoneName = BoneA;

                OtherCollider.Spawn = SpawnB;
                OtherCollider.Animable = SpawnB.spawnAnim;
                OtherCollider.BoneName = BoneB;

                OnArmatureExit(OtherCollider);
            }
            catch (Exception e)
            {
                Utils.PrintException(e);
            }
        }
        private void native_OnArmatureStay(Spawn SpawnA, Spawn SpawnB, string BoneA, string BoneB)
        {
            try
            {
                //ArmCollider OtherCollider = new ArmCollider();
                this.MyCollider.Spawn = SpawnA;
                this.MyCollider.Animable = SpawnA.spawnAnim;
                this.MyCollider.BoneName = BoneA;

                OtherCollider.Spawn = SpawnB;
                OtherCollider.Animable = SpawnB.spawnAnim;
                OtherCollider.BoneName = BoneB;

                OnArmatureStay(OtherCollider);
            }
            catch (Exception e)
            {
                Utils.PrintException(e);
            }

        }

        #endregion

        #region IAnimationEvent 成员

        public void AnimationEvent(MovementEventType movementType, string movementID)
        {
            switch (movementType)
            {
                case MovementEventType.COMPLETE:
                    //Debug.Log("COMPLETE:" + movementID);
                    OnAnimComplete(this.spawnAnim, movementID);
                    break;
                case MovementEventType.LOOP_COMPLETE:
                    //Debug.Log("LOOP_COMPLETE:" + movementID);
                    OnAnimLoopComplete(this.spawnAnim, movementID);
                    break;
                case MovementEventType.START:
                    //Debug.Log("START:" + movementID);
                    OnAnimStart(this.spawnAnim, movementID);
                    break;
                default:
                    break;
            }
        }

        public void FrameEvent(string evt, int originFrameIndex, int currentFrameIndex)
        {
            this.OnAnimFrameEvent(this.spawnAnim, evt, originFrameIndex, currentFrameIndex);
        }

        #endregion
    }
}