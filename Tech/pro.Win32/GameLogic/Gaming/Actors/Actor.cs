using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Thunder.Common;
using Thunder.GameLogic.Gaming.ScripSystem;
using Thunder.GameLogic.Gaming.ScripSystem.Events;
using Thunder.Game;
using MatrixEngine.Cocos2d;
using MatrixEngine.CocoStudio.Armature;
using Thunder.GameLogic.Gaming.BulletSystems;
using MatrixEngine.Math;
using Thunder.GameLogic.Gaming.Actors.Drops;

namespace Thunder.GameLogic.Gaming.Actors
{
    public abstract class Actor : Spawn, IBulletEvent
    {
        /// <summary>
        /// 角色的所有行为状态
        /// </summary>
        public enum State
        {
            /// <summary>
            /// 其它分类的状态
            /// </summary>
            Null = 0,
            /// <summary>
            /// 待机1
            /// </summary>
            Idle,
            /// <summary>
            /// 移动
            /// </summary>
            Walk,
            /// <summary>
            /// 后退
            /// </summary>
            Retreat,
            /// <summary>
            /// 准备攻击状态
            /// </summary>
            Ready,
            /// <summary>
            /// 被攻击
            /// </summary>
            ByAttack,
            /// <summary>
            /// 狂暴
            /// </summary>
            Frenzy,
            /// <summary>
            /// 攻击
            /// </summary>
            Attack,
            /// <summary>
            /// 技能
            /// </summary>
            Skill,
            /// <summary>
            /// 逃跑
            /// </summary>
            Escape,
            /// <summary>
            /// 死亡
            /// </summary>
            Dead,
            /// <summary>
            /// 事件
            /// </summary>
            Event,
            /// <summary>
            /// 状态计数
            /// </summary>
            Count,
        }

        /// <summary>
        /// 动画种类名字
        /// </summary>
        public enum AnimName
        {
            Null,
            待机1,
            待机2,
            行走1,
            行走2,
            后退,
            被打1,
            被打2,
            被打3,
            死亡1,
            死亡2,
            攻击1,
            攻击2,
            攻击3,
            攻击4,
            变形1,
            变形2,
            Count,
        };
        /// <summary>
        /// 发射点
        /// 要和动画编辑器上的发射点一一对应
        /// </summary>
        public enum EmitPoint : int
        {
            Emit1,
            Emit2,
            Emit3,
            Emit4,
            Emit5,
            Emit6,
            Emit7,
            Emit8,
            Emit9,
            Emit10,
            Count,
        }

        //状态表
        private HashMap<State, AnimName[]> map_StateTable = new HashMap<State, AnimName[]>();
        //动作名字
        public HashMap<AnimName, string> animActions = new HashMap<AnimName, string>();

        /// <summary>
        /// 前一个状态
        /// </summary>
        public State PreviousState
        {
            get;
            protected set;
        }

        /// <summary>
        /// 当前状态
        /// </summary>
        public State CurrentState
        {
            get;
            protected set;
        }

        public CCColliderFilter Filter = FilterType.AllFilter;

        /// <summary>
        /// 是否有掉落
        /// </summary>
        private bool isDrap;
        /// <summary>
        /// 子弹产生器
        /// </summary>
        protected BulletSpawner bulletSpawner;

        /// <summary>
        /// 事件产生器
        /// </summary>
        public EventSpawner EventSpawner
        {
            get;
            set;
        }

        private List<CCAction> actionsPool = new List<CCAction>();
        public void AddAutonReleaseAction(CCAction action)
        {
            if (EDebug.swActorActionPool)
                actionsPool.Add(action);
        }

        /********脚本相关************/
        protected ScriptHelper scriptHelper;
        protected EventManager eventManager;

        protected List<ScriptBinding> bindingList = new List<ScriptBinding>();

        public static int EVENT_ID_BEGIN = 0;
        public static int EVENT_ID_OVER = 1000;

        protected bool isScriptBinding;
        protected static readonly string apiFile = "api.lua";
        public static string apiScript;
        /*********************/

        /********目标效果************/
        private CCSprite targetFlagImg1;
        private CCSprite targetFlagImg2;
        private CCSprite targetFlagImg3;
        private bool targetEffectFlag;
        /****************************/

        public Actor(SpawnInfo spawnInfo)
            : base(spawnInfo.resPath, spawnInfo.armaName)
        {
            this.Info = spawnInfo;
            this.Filter = this.Info.filter;
            this.Postion = this.Info.position;

            BindAnimition(State.Idle, AnimName.待机1);
            BindAnimition(State.Walk, AnimName.行走1, AnimName.行走2);
            BindAnimition(State.Retreat, AnimName.后退);
            BindAnimition(State.Ready, AnimName.待机1);
            BindAnimition(State.Escape, AnimName.行走1);
            BindAnimition(State.Dead, AnimName.死亡1);
            BindAnimition(State.ByAttack, AnimName.被打1);
            BindAnimition(State.Attack, AnimName.攻击1, AnimName.攻击2, AnimName.攻击3, AnimName.攻击4);

            if (EDebug.swBullet)
            {
                bulletSpawner = new BulletSpawner(this);
                for (int i = 0; i < this.Info.Emitters.Count; i++)
                {
                    string bulletFile = Utils.CoverBulletPath(this.Info.Emitters[i]);
                    if (CCFileUtils.IsFileExist(bulletFile))
                    {
                        BulletSystem bullet = new BulletSystem(bulletFile);
                        this.BindEmitter((EmitPoint)i, bullet);
                    }
                    else
                    {
                        Console.WriteLine("找到不子弹配置文件：" + bulletFile);
                    }
                }
            }
        }

        protected override void Dispose(bool disposing)
        {

            //if (disposing)
            {
                BulletEmitter.UnregisterTarget(this);
                this.RemoveFromParent(true);

                if (EDebug.swBullet)
                {
                    bulletSpawner.Dispose();
                    bulletSpawner = null;
                }
                UnbindScript();

                this.StopAllAction();
                foreach (var item in actionsPool)
                {
                    item.Dispose();
                }
                actionsPool.Clear();
            }
            this.Info = null;

            this.ReleaseTargetEffect(disposing);

            this.RemoveAllChildren();
            base.Dispose(disposing);

        }

        protected override void OnEnter()
        {
            //base.OnEnter();
            GotoState(State.Null);
            BulletEmitter.RegisterTarget(this);
            this.RunAction(new CCActionSequence(new CCActionDelayTime(Info.delayTime), new CCActionCallFunc(Activate)));
        }

        protected override void OnExit()
        {
            //base.OnExit();
            BulletEmitter.UnregisterTarget(this);
        }

        public virtual void Activate()
        {
            //执行脚本
            if (isScriptBinding && eventManager != null)
            {
                eventManager.Begin();
            }
            GotoState(State.Idle);
        }

        public virtual bool IsDead
        {
            get
            {
                return (this.CurrentState == State.Dead);
            }
        }

        protected virtual void InitTargetEffect()
        {
            //
            targetFlagImg1 = new CCSprite("effects_071.png", true);
            targetFlagImg1.Postion = 0;
            targetFlagImg1.BlendFunc = BlendFunc.Additive;
            targetFlagImg1.ZOrder = 10;
            targetFlagImg1.IsVisible = false;
            this.AddChild(targetFlagImg1);

            targetFlagImg2 = new CCSprite("effects_060.png", true);
            targetFlagImg2.Postion = 0;
            targetFlagImg2.BlendFunc = BlendFunc.Additive;
            targetFlagImg2.ZOrder = 10;
            targetFlagImg2.IsVisible = false;
            this.AddChild(targetFlagImg2);

            targetFlagImg3 = new CCSprite("effects_061.png", true);
            targetFlagImg3.Postion = 0;
            targetFlagImg3.Scale = 1.5f;
            targetFlagImg3.BlendFunc = BlendFunc.Additive;
            targetFlagImg3.ZOrder = 10;
            targetFlagImg3.IsVisible = false;
            this.AddChild(targetFlagImg3);
            //
        }

        protected virtual void ReleaseTargetEffect(bool disposing)
        {
            //
            if (this.targetFlagImg1 != null)
            {
                this.targetFlagImg1.StopAllAction();
                this.RemoveChild(this.targetFlagImg1);
                if (disposing)
                {
                    this.targetFlagImg1.Dispose();
                }
                this.targetFlagImg1 = null;
            }
            if (this.targetFlagImg2 != null)
            {
                this.targetFlagImg2.StopAllAction();
                this.RemoveChild(this.targetFlagImg2);
                if (disposing)
                {
                    this.targetFlagImg2.Dispose();
                }
                this.targetFlagImg2 = null;
            }
            if (this.targetFlagImg3 != null)
            {
                this.targetFlagImg3.StopAllAction();
                this.RemoveChild(this.targetFlagImg3);
                if (disposing)
                {
                    this.targetFlagImg3.Dispose();
                }
                this.targetFlagImg3 = null;
            }
            //
        }

        protected virtual void HideTargetEffect()
        {
            this.targetEffectFlag = false;
        }
        protected virtual void ShowEndTargetEffect()
        {
            targetFlagImg2.IsVisible = true;
            targetFlagImg3.IsVisible = true;
            var ac1 = new CCActionSequence(new CCActionDelayTime(1), new CCActionHide());
            var ac2 = new CCActionSequence(new CCActionRotateBy(1, 360), new CCActionHide(), new CCActionCallFunc(this.HideTargetEffect));
            targetFlagImg2.RunSequenceActions(ac1);
            targetFlagImg3.RunSequenceActions(ac2);
        }

        protected virtual void ShowStartTargetEffect()
        {
            if (targetFlagImg1 != null && !targetFlagImg1.IsVisible && !this.targetEffectFlag)
            {
                this.targetEffectFlag = true;

                targetFlagImg1.IsVisible = true;
                targetFlagImg1.Alpha = 0;
                targetFlagImg1.Scale = 4;
                var start = new CCActionSpawn(new CCActionRotateBy(0.6f, 360), new CCActionScaleTo(0.6f, 0.3f), new CCActionFadeTo(0.5f, 255));
                targetFlagImg1.RunSequenceActions(start, new CCActionHide(), new CCActionCallFunc(this.ShowEndTargetEffect));
            }
        }

        private double perTargetEffectTime;
        private readonly double targetEffectTimeSpan = 1500;
        public virtual void PlayTargetEffect()
        {
            if(this.HpPercent <= 1f)
            {
                TimeSpan timeSpan = new TimeSpan(DateTime.Now.Ticks);
                double currentTime = timeSpan.TotalMilliseconds;
                if (currentTime - perTargetEffectTime > targetEffectTimeSpan)
                {
                    if (MathHelper.Random_minus0_1() < 0.3f)
                    {
                        this.ShowStartTargetEffect();
                    }
                    perTargetEffectTime = currentTime;
                }
            }
        }

        public virtual bool PlayAnim(AnimName action)
        {
            return PlayAnim(action, true);
        }

        protected virtual void PlayDeadEffect()
        {
            BulletEmitter.UnregisterTarget(this);
            this.CloseFire();
            this.StopAllAction();
        }
        /// <summary>
        /// 播放指定的动画
        /// </summary>
        /// <param name="action">动画类型</param>
        /// <param name="loop">是否循环</param>
        /// <returns>播放成功后返回真</returns>
        public virtual bool PlayAnim(AnimName action, bool loop)
        {
            if (IsCurrentAnim(action))
            {
                return false;
            }
            string name = GetAnimName(action); //EqualsAnimName(action);
            return PlayAnim(name, loop);
        }

        protected virtual bool IsCurrentAnim(AnimName action)
        {
            string nowAnim = spawnAnim.CurrentAnim();
            return nowAnim.Equals(GetAnimName(action));
        }

        /// <summary>
        /// 状态与动画一起邦定
        /// </summary>
        /// <param name="state"> 状态 </param>
        /// <param name="anim"> 动画 </param>
        protected virtual void BindAnimition(State state, params AnimName[] actions)
        {
            map_StateTable[state] = actions;
        }
        /// <summary>
        /// 获取状态邦定的动画表
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        protected virtual AnimName[] GetBindAnimition(State state)
        {
            return map_StateTable[state];
        }

        /// <summary>
        /// 动作（转换成对应的动画名字）
        /// </summary>
        protected virtual bool EqualsAnimName(string animName, params AnimName[] actions)
        {
            foreach (var action in actions)
            {
                if (animName == (this.GetAnimName(action)))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 所有动作名字（具体到某个角色再添加具体的动本）
        /// 如：loading 对应的是 qiangbing_loading
        /// </summary>
        public string GetAnimName(AnimName a)
        {
            if (animActions == null)
            {
                return null;
            }
            return animActions[a];
        }
        /// <summary>
        /// 获取真实的动作名
        /// </summary>
        /// <param name="anim"></param>
        /// <returns></returns>
        public AnimName GetAnimAction(string anim)
        {
            foreach (KeyValuePair<AnimName, string> a in animActions)
            {
                if (a.Value == anim)
                {
                    return a.Key;
                }
            }
            return AnimName.Null;
        }

        /// <summary>
        /// 跳转到指定的状态
        /// </summary>
        /// <param name="state"></param>
        public virtual bool GotoState(State nextState)
        {
            //如果接受改变状态则改当于状态与动画
            if (OnChangeState(CurrentState, nextState))
            {
                PreviousState = CurrentState;
                CurrentState = nextState;

                return true;
            }
            return false;
        }

        /// <summary>
        /// 改变状态回调
        /// </summary>
        /// <param name="currentState"></param>
        /// <param name="nextState"></param>
        /// <returns></returns>
        protected virtual bool OnChangeState(State currentState, State nextState)
        {
            return true;
        }

        /// <summary>
        /// 销毁/自杀
        /// </summary>
        public virtual void Destroy()
        {
            //
            GotoState(State.Null);
            BulletEmitter.UnregisterTarget(this);
            this.CloseFire();
            this.StopAllAction();

            if (this.EventSpawner != null)
            {
                this.EventSpawner.OnRemoveActor(this);
            }

        }

        public virtual float HpPercent
        {
            get
            {
                return this.Info.HP / this.Info.totalHp;
            }
        }

        /// <summary>
        /// 把当前的子弹变成宝石
        /// </summary>
        public virtual void BulletToGem()
        {
            if (EDebug.swBullet)
            {
                this.bulletSpawner.ToGem();
            }
        }
        public virtual void BulletToGem(DropType droptype)
        {
            if (EDebug.swBullet)
            {
                this.bulletSpawner.ToGem(droptype);
            }
        }
        /// <summary>
        /// 开火
        /// </summary>
        public virtual void OpenFire()
        {
            //Console.WriteLine("OpenFire");
            if (EDebug.swBullet)
            {
                this.bulletSpawner.OpenFire();
            }
        }

        public virtual void OpenFire(EmitPoint emit)
        {
            if (EDebug.swBullet)
            {
                this.bulletSpawner.OpenFire(emit);
            }
        }
        /// <summary>
        /// 停火
        /// </summary>
        public virtual void CloseFire()
        {
            if (EDebug.swBullet)
            {
                this.bulletSpawner.CloseFire();
            }
        }
        public virtual void CloseFire(EmitPoint emit)
        {
            if (EDebug.swBullet)
            {
                this.bulletSpawner.OpenFire(emit);
            }
        }
        public virtual void BeAttacked(float damge)
        {
            if (CurrentState != State.Dead)
            {
                if (CurrentState != State.Null)
                {
                    this.Info.HP -= damge;
                }
                if (this.Info.HP <= 0)
                {
                    GotoState(State.Dead);
                }
            }

        }

        protected virtual void ResetUserDamage()
        {
            if (EDebug.swBullet)
            {
                this.bulletSpawner.UserDamage = this.Info.damage;
            }
        }
        //
        public virtual void Recycling()
        {
            if (EDebug.swBullet)
            {
                this.bulletSpawner.Recycling();
            }
        }
        /// <summary>
        /// 是否掉落物品
        /// </summary>
        public bool IsDrap
        {
            set { isDrap = value; }
            get { return isDrap; }
        }

        public bool IsAlive
        {
            get { return CurrentState == State.Dead; }
        }

        /// <summary>
        /// 飞行速度
        /// </summary>
        public virtual float Speed
        {
            set { this.Info.speed = value; }
            get { return this.Info.speed; }
        }

        /// <summary>
        /// 邦定发射点
        /// </summary>
        /// <param name="emitPoit"></param>
        /// <param name="emitr"></param>
        public virtual void BindEmitter(EmitPoint emitPoit, BulletSystem emitr)
        {
            if (EDebug.swBullet)
            {
                this.bulletSpawner.BindEmitter(emitPoit, emitr, false);
            }
        }
        /// <summary>
        /// 邦定发射点
        /// </summary>
        /// <param name="emitPoit"></param>
        /// <param name="emitr"></param>
        /// <param name="fire">是否立即开火</param>
        public virtual void BindEmitter(EmitPoint emitPoit, BulletSystem emitr, bool fire)
        {
            if (EDebug.swBullet)
            {
                this.bulletSpawner.BindEmitter(emitPoit, emitr, fire);
            }
        }

        public virtual void UnbindEmitter(EmitPoint emitPoit)
        {
            if (EDebug.swBullet)
            {
                this.bulletSpawner.UnbindEmitter(emitPoit);
            }
        }

        public virtual void UnbindEmitter()
        {
            if (EDebug.swBullet)
            {
                this.bulletSpawner.UnbindEmitter();
            }
        }

        public override void OnUpdate(float dTime)
        {
            //TODO
            if (EDebug.swBullet)
            {
                this.bulletSpawner.OnUpdate(dTime);
            }
        }

        ////////////////////////////////////////////////////////////////////
        //ScriptHelper 

        public EventManager EventManager
        {
            set { this.eventManager = value; }
            get { return eventManager; }
        }

        public ScriptHelper ScriptHelper
        {
            set { this.scriptHelper = value; }
            get { return this.scriptHelper; }
        }

        /// <summary>
        /// 注册脚本
        /// </summary>
        /// <returns></returns>
        protected virtual bool BindingScript()
        {
            if (EDebug.swScript)
            {
                scriptHelper = new ScriptHelper();
                eventManager = new EventManager();

                eventManager.SetScriptHelper(scriptHelper);
                eventManager.SetActor(this);


                this.AddChild(eventManager);

                try
                {
                    //注册脚本
                    bindingList.Add(new Thunder.GameLogic.Gaming.ScripSystem.Binding.Actor(scriptHelper));
                    bindingList.Add(new Thunder.GameLogic.Gaming.ScripSystem.Binding.Event(scriptHelper));
                    bindingList.Add(new Thunder.GameLogic.Gaming.ScripSystem.Binding.Action(scriptHelper));     
                    bindingList.Add(new Thunder.GameLogic.Gaming.ScripSystem.Binding.Node(scriptHelper));                  
                    //注册常量
                    scriptHelper.FileldValue("Self", this);
                    scriptHelper.FileldValue("EVENT_ID_BEGIN", EVENT_ID_BEGIN);
                    scriptHelper.FileldValue("EVENT_ID_OVER", EVENT_ID_OVER);
                }
                catch (Exception e)
                {
                    Utils.PrintException(e);
                }
                isScriptBinding = true;

                //加载基本api
                if (apiScript == null)
                {
                    apiScript = CCFileUtils.GetFileDataToString(Utils.CoverActionScriptPath(apiFile));
                }
                scriptHelper.DoString(apiScript);
                //
                scriptHelper.DoString(this.Info.actionScript);

                return true;
            }
            return false;
        }
        /// <summary>
        /// 解除注册脚本 
        /// TODO:释放脚本资源
        /// </summary>
        protected virtual void UnbindScript()
        {
            if (EDebug.swScript)
            {
                this.RemoveChild(eventManager);
                eventManager.Dispose();
                eventManager = null;

                bindingList.Clear();

                scriptHelper = null;
                apiScript = null;
            }
        }
        /// <summary>
        /// 脚本执行结束回调
        /// </summary>
        public virtual void OnScriptOver()
        {

        }
        ////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 被子弹打中回调
        /// </summary>
        /// <param name="bulletEmitter">发射器</param>
        /// <param name="bullet">子弹</param>
        protected virtual void OnBulletHit(BulletEmitter bulletEmitter, Bullet bullet, Vector2 hitPos)
        {

        }
        /// <summary>
        /// 吸收到落掉物
        /// </summary>
        /// <param name="drop"></param>
        public virtual void OnDropAdsorbent(Drop drop)
        {

        }

        #region IBulletEvent 成员

        public void BulletHit(BulletEmitter bulletEmitter, ref Bullet bullet, Vector2 hitPos)
        {
            this.OnBulletHit(bulletEmitter, bullet, hitPos);
        }

        #endregion
    }
}
