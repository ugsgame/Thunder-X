using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Thunder.Game;
using MatrixEngine.Cocos2d;
using Thunder.GameLogic.Common;
using MatrixEngine.Math;
using Thunder.GameLogic.Gaming.BulletSystems;
using MatrixEngine.Engine;
using Thunder.GameLogic.ExternAction;
using Thunder.GameLogic.Gaming.Actors.Drops;

namespace Thunder.GameLogic.Gaming.Actors.Bosses
{
    /// <summary>
    /// 所有boss基类
    /// </summary>
    public class Boss : Actor
    {
        private bool isFrenzy;
        private readonly float timeOfDeadCall = 6;
        /// <summary>
        /// 狂暴
        /// </summary>
        public bool Frenzy
        {
            set
            {
                //只会触发一次狂暴状态
                if (!this.isFrenzy)
                {
                    isFrenzy = true;
                    this.GotoState(State.Frenzy);
                }
            }
            get { return isFrenzy; }
        }

        private float frenzyHp;
        /// <summary>
        /// 触发狂暴的HP值（当HP<FrenzyHp）
        /// </summary>
        public float FrenzyHp
        {
            set { frenzyHp = value; }
            get { return frenzyHp; }
        }

        public Boss(SpawnInfo spawnInfo)
            : base(spawnInfo)
        {
            this.Init(spawnInfo);
        }

        public Boss(SpawnInfo spawnInfo, EventSpawner eventSpawner)
            : base(spawnInfo)
        {
            this.EventSpawner = eventSpawner;
            this.Init(spawnInfo);
        }

        protected virtual void Init(SpawnInfo spawnInfo)
        {
            this.Info.isEnemy = true;
            //this.frenzyHp = this.Info.frenzyHp;
            this.Info.frenzyHp = this.Info.totalHp / 2;
            this.frenzyHp = this.Info.frenzyHp;
            this.ZOrder = PlayingScene.ZOrder_boss;
            this.Filter = FilterType.FilterEnemy;
            this.Info.delayTime = 4.0f;
            this.Info.score = 500;

            //ArmatureResManager.Instance.AutoRelease(this.spawnAnim.ArmatureRes);

            if (this.Info.actionScript != null && this.Info.actionScript != "")
            {
                this.BindingScript();
            }

            this.InitTargetEffect();

            if (this.scriptHelper != null)
            {
                this.scriptHelper.CallFuction("OnInit");
            }
        }

        protected override void OnEnter()
        {
            try
            {
                animActions[AnimName.Null] = Info.animName + "_fly1";
                animActions[AnimName.待机1] = Info.animName + "_fly1";
                animActions[AnimName.变形1] = Info.animName + "_transform";
                animActions[AnimName.攻击1] = Info.animName + "_fly2_attact1";
            }
            catch (Exception e)
            {
                Utils.PrintException(e);
            }
            //BulletEmitter.RegisterTarget(this);
            //
            PlayingScene.Instance.PlayBossWaring();
            //
            base.OnEnter();
        }

        protected override void OnExit()
        {
            base.OnExit();
            //BulletEmitter.UnregisterTarget(this);
        }

        protected override void OnAnimStart(Animable anim, string animName)
        {

        }

        protected override void OnAnimComplete(Animable anim, string animName)
        {
            if (this.EqualsAnimName(animName, AnimName.变形1))
            {
                this.GotoState(State.Idle);
            }
            else if (this.EqualsAnimName(animName, AnimName.攻击1))
            {
                if (this.scriptHelper != null)
                {
                    this.scriptHelper.CallFuction("OnAttackOver");
                }
                this.GotoState(State.Idle);
            }
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

        protected override void OnTimeCall(float time)
        {
            base.OnTimeCall(time);
            //处理死亡任务
            if (CurrentState == State.Dead)
            {
                this.Destroy();
            }
        }

        protected virtual void PlayPatchs()
        {
            CCParticleSystem patch1 = new CCParticleSystem(ResID.Particles_patch3);
            patch1.Postion = 0;
            patch1.IsAutoRemoveOnFinish = true;
            patch1.Play();
            patch1.PositionType = tCCPositionType.kCCPositionTypeGrouped;
            this.AddChild(patch1);
            patch1 = null;

            CCParticleSystem patch2 = new CCParticleSystem(ResID.Particles_patch4);
            patch2.Postion = 0;
            patch2.IsAutoRemoveOnFinish = true;
            patch2.Play();
            patch2.PositionType = tCCPositionType.kCCPositionTypeGrouped;
            this.AddChild(patch2);
            patch2 = null;
        }

        protected virtual void ExplodeLight(float delyTime, float rotation)
        {
            CCSprite light = new CCSprite("effects_075.png", true);
            light.IsVisible = false;
            light.BlendFunc = BlendFunc.Additive;
            light.Color = new Color32(250, 90, 5);
            light.Rotation = rotation;
            var action = new CCActionSequence(new CCActionDelayTime(delyTime), new CCActionShow(), new CCActionEaseIn(new CCActionSpawn(new CCActionScaleTo(1.5f, 3f), new CCActionFadeOut(1.5f)), 0.5f), new RemoveSelf());
            light.RunAction(action);
            this.AddChild(light);
        }

        protected virtual void FrenzyExplodeEffect()
        {
            GameAudio.PlayEffect(GameAudio.Effect.die_elite);
            CCSprite explode = new CCSprite("effects_004.png", true);
            explode.BlendFunc = BlendFunc.Additive;
            //explode.Color = new Color32(250, 90, 5);
            var action = new CCActionSequence(new CCActionSpawn(new CCActionScaleTo(0.5f, 8.0f), new CCActionFadeOut(0.5f)), new RemoveSelf());
            explode.RunAction(action);
            this.AddChild(explode);

            this.PlayPatchs();
        }

        protected virtual void FinalExplodeEffect()
        {
            Info.drops.PlayBoss(this);
            GameAudio.PlayEffect(GameAudio.Effect.die_boss);

            CCSprite explode = new CCSprite("effects_004.png", true);
            explode.BlendFunc = BlendFunc.Additive;
            //explode.Color = new Color32(250, 90, 5);
            this.Animable.armature.IsVisible = false;
            var action = new CCActionSequence(new CCActionScaleTo(1.0f, 7.0f), new CCActionFadeOut(1.0f), new RemoveSelf());
            explode.RunAction(action);
            this.AddChild(explode);

            this.ExplodeLight(0.2f, 30);
            this.ExplodeLight(0.3f, 90);
            this.ExplodeLight(0.5f, -60);
            this.ExplodeLight(0.8f, 0);
            this.ExplodeLight(1.2f, 60);

            PlayPatchs();
        }

        protected override void PlayDeadEffect()
        {
            base.PlayDeadEffect();

            this.BulletToGem(DropType.Drop_Gem_Blue_1);
            this.PlayAnim(AnimName.待机1);

            FrenzyExplodeEffect();

            CCParticleSystem spark = new CCParticleSystem(ResID.Particles_explodeStar);
            spark.Postion = 0;
            spark.IsAutoRemoveOnFinish = true;
            spark.Play();
            spark.PositionType = tCCPositionType.kCCPositionTypeGrouped;
            this.AddChild(spark);
            spark = null;

            CCSprite pSmoke = new CCSprite("effects_010.png", true);
            pSmoke.BlendFunc = BlendFunc.Additive;
            pSmoke.Color = new Color32(250, 90, 5);
            var action = new CCActionSequence(new CCActionFadeOut(0.5f), new RemoveSelf());
            pSmoke.RunAction(action);
            this.AddChild(pSmoke);

            CCSprite light = new CCSprite("effects_073.png", true);
            light.BlendFunc = BlendFunc.Additive;
            light.Color = new Color32(250, 90, 5);
            light.Scale = 4;
            action = new CCActionSequence(new CCActionFadeOut(1.5f), new CCActionDelayTime(0.5f), new CCActionCallFunc(this.FinalExplodeEffect), new RemoveSelf());
            light.RunAction(action);
            this.AddChild(light);

            this.ExplodeLight(0.3f, 180);
            this.ExplodeLight(0.8f, 100);
            this.ExplodeLight(1.2f, 30);
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
                case State.Frenzy:
                    {
                        //变成狂暴动画
                        animActions[AnimName.待机1] = Info.name + "_fly2";
                        //
                        PlayAnim(AnimName.变形1, false);
                        GameAudio.PlayEffect(GameAudio.Effect.boss_change);
                        //
                        FrenzyExplodeEffect();
                        //回调脚本
                        if (this.scriptHelper != null)
                        {
                            this.scriptHelper.CallFuction("OnFrenzy");
                        }
                        //掉一个升级
                        DropManager.Instance.DrapPlayer(this, DropType.Drop_Power, 1);
                    }
                    break;
                case State.Attack:
                    {
                        PlayAnim(AnimName.攻击1, false);
                    }
                    break;
                case State.Dead:
                    {
                        this.PlayDeadEffect();
                        //this.Animable.armature.Color = new Color32(100, 100, 100);
                        var fallAction = new CCActionSpawn(new CCActionRotateBy(5, 60), new CCActionScaleTo(5, 0.5f));
                        var deathAction = new CCActionSequence(fallAction, new MCActionFadeOut(0.5f));
                        this.Animable.RunAction(deathAction);
                        this.Animable.RunAction(new ActionColorChange(3, Color32.White, Color32.Black));

                        PlayingScene.Instance.OnBossDead(this);
                        this.TimeActionCall(timeOfDeadCall);
                    }
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

        protected double perHitTime;
        protected override void OnBulletHit(BulletEmitter bulletEmitter, Bullet bullet, Vector2 hitPos)
        {
            TimeSpan timeSpan = new TimeSpan(DateTime.Now.Ticks);
            double currentTime = timeSpan.TotalMilliseconds;
            if (currentTime - perHitTime > Function.attackedTimeSpan && this.CurrentState != State.Dead)
            {
                this.BeAttacked(bulletEmitter.UserDamage + bulletEmitter.Damage);
                perHitTime = currentTime;
            }
        }

        public override void BeAttacked(float damge)
        {
            if (CurrentState != State.Null)
            {
                this.Info.HP -= damge;
            }
            if (this.Info.HP <= 0 && CurrentState != State.Dead)
            {
                GotoState(State.Dead);
            }
            else
            {
                if (this.Info.HP <= FrenzyHp)
                {
                    Frenzy = true;
                }
            }
        }

        public override void OnUpdate(float dTime)
        {
            base.OnUpdate(dTime);
            this.PlayTargetEffect();
            if (this.CurrentState != State.Null)
            {
                PlayingScene.Instance.OnBossHP(this);
            }
        }
    }
}
