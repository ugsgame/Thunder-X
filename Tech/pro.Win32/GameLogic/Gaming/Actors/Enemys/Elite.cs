using MatrixEngine.Cocos2d;
using MatrixEngine.CocoStudio.GUI;
using MatrixEngine.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thunder.Game;
using Thunder.GameLogic.Common;
using Thunder.GameLogic.ExternAction;
using Thunder.GameLogic.Gaming.Actors.Drops;
using Thunder.GameLogic.Gaming.BulletSystems;

namespace Thunder.GameLogic.Gaming.Actors.Enemys
{
    public class Elite : Actor
    {
        private readonly float timeOfDeadCall = 3;
        private CCNode Panel_enemyProgressBar;
        private ColorChange colorChange;

        public Elite(SpawnInfo spawnInfo)
            : base(spawnInfo)
        {
            Init(spawnInfo);
        }

        public Elite(SpawnInfo spawnInfo, EventSpawner eventSpawner)
            : base(spawnInfo)
        {
            this.EventSpawner = eventSpawner;
            Init(spawnInfo);
        }

        protected virtual void Init(SpawnInfo spawnInfo)
        {
            this.Info.isEnemy = true;
            this.ZOrder = PlayingScene.ZOrder_enemy;
            this.Filter = FilterType.FilterEnemy;
            this.Info.score = 150;

            ArmatureResManager.Instance.AutoRelease(this.spawnAnim.ArmatureRes);

            //Console.WriteLine(this.Info.actionScript);
            if (this.Info.actionScript != null && this.Info.actionScript != "")
            {
                this.BindingScript();
            }

            this.colorChange = new ColorChange(new Color32(200, 200, 200), Color32.White);

            this.InitHPBar();

            this.InitTargetEffect();
        }

        protected virtual void InitHPBar()
        {
            //UIWidget Panel_templates = UIReader.GetWidget(ResID.UI_UI_Templates);
            Panel_enemyProgressBar = new CCNode();
            CCSprite bottom = new CCSprite("xiaoguaixuetiao_bg.png", true);
            UILoadingBar bar = new UILoadingBar();
            bar.LoadTexture("xiaoguaixuetiao_xue.png", TextureResType.UI_TEX_TYPE_PLIST);
            bar.Tag = 11;
            Panel_enemyProgressBar.AddChild(bottom);
            Panel_enemyProgressBar.AddChild(bar);
            //Panel_enemyProgressBar.RemoveFromParent();

            Panel_enemyProgressBar.PostionX = 0;
            Panel_enemyProgressBar.PostionY = -100;

            this.AddChild(Panel_enemyProgressBar);           
        }

        protected override void Dispose(bool disposing)
        {
            if (Panel_enemyProgressBar != null)
            {
                this.RemoveChild(Panel_enemyProgressBar);
                Panel_enemyProgressBar.Dispose();
                Panel_enemyProgressBar = null;
            }

            base.Dispose(disposing);
        }

        protected override void OnEnter()
        {
            try
            {
                animActions[AnimName.Null] = Info.animName + "_fly1";
                animActions[AnimName.待机1] = Info.animName + "_fly1";
                animActions[AnimName.待机2] = Info.animName + "_fly2";
                animActions[AnimName.变形1] = Info.animName + "_transform";
            }
            catch (Exception e)
            {
                Utils.PrintException(e);
            }
            //BulletEmitter.RegisterTarget(this);

            if (this.Info.drops.IsWithProp)
            {
                this.Animable.armature.RunAction(new CCActionRepeatForever(
                    new CCActionSequence(
                        new ActionColorChange(0.5f, Color32.White, Color32.Red), new ActionColorChange(0.5f, Color32.Red, Color32.White))));
            }

            base.OnEnter();
        }

        protected override void OnExit()
        {
            base.OnExit();
            //BulletEmitter.UnregisterTarget(this);
        }

        private float colorPercent = 1.0f;
        public override void OnUpdate(float dTime)
        {
            base.OnUpdate(dTime);
            this.PlayTargetEffect();

            if (!this.Info.drops.IsWithProp)
            {
                colorPercent += dTime * 10f;
                this.colorChange.OnUpdate(colorPercent);
                this.Animable.armature.Color = this.colorChange.Color;
            }
        }

        protected override void OnAnimStart(Animable anim, string animName)
        {
        }

        protected override void OnAnimComplete(Animable anim, string animName)
        {
            AnimName action = GetAnimAction(animName);
            switch (action)
            {
                case AnimName.Null:
                    break;
                case AnimName.变形1:
                    {
                        animActions[AnimName.待机1] = Info.animName + "_fly2";
                        this.GotoState(Actor.State.Idle);
                    }
                    break;
                default:
                    break;
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

        private CCNode effectNode;
        protected override void PlayDeadEffect()
        {
            base.PlayDeadEffect();
            this.BulletToGem(DropType.Drop_Gem_Blue_1);
            GameAudio.PlayEffect(GameAudio.Effect.die_elite);
            Info.drops.PlayEnemy(this);
            this.Animable.armature.IsVisible = false;

            this.effectNode = new CCNode();
            this.effectNode.Postion = this.Postion;
            this.effectNode.ZOrder = PlayingScene.ZOrder_effect;

            CCParticleSystem patch1 = new CCParticleSystem(ResID.Particles_patch1);
            patch1.Postion = 0;
            patch1.IsAutoRemoveOnFinish = true;
            patch1.Play();
            patch1.PositionType = tCCPositionType.kCCPositionTypeGrouped;
            this.effectNode.AddChild(patch1);
            patch1 = null;

            CCParticleSystem patch2 = new CCParticleSystem(ResID.Particles_patch2);
            patch2.Postion = 0;
            patch2.IsAutoRemoveOnFinish = true;
            patch2.Play();
            patch2.PositionType = tCCPositionType.kCCPositionTypeGrouped;
            this.effectNode.AddChild(patch2);
            patch2 = null;

            CCParticleSystem smoke2 = new CCParticleSystem(ResID.Particles_smoke2);
            smoke2.Postion = 0;
            smoke2.IsAutoRemoveOnFinish = true;
            smoke2.Play();
            smoke2.PositionType = tCCPositionType.kCCPositionTypeGrouped;
            this.effectNode.AddChild(smoke2);
            smoke2 = null;

            CCParticleSystem spark = new CCParticleSystem(ResID.Particles_explodeStar);
            spark.Postion = 0;
            spark.IsAutoRemoveOnFinish = true;
            spark.Play();
            spark.PositionType = tCCPositionType.kCCPositionTypeGrouped;
            this.effectNode.AddChild(spark);
            spark = null;
            //
            CCSprite pSmoke = new CCSprite("effects_010.png", true);
            pSmoke.BlendFunc = BlendFunc.Additive;
            pSmoke.Color = new Color32(250, 90, 5);
            var action = new CCActionSequence(new CCActionFadeOut(0.5f), new RemoveSelf());
            pSmoke.RunAction(action);
            this.effectNode.AddChild(pSmoke);

            pSmoke = new CCSprite("effects_011.png", true);
            pSmoke.BlendFunc = BlendFunc.Additive;
            pSmoke.Color = new Color32(250, 90, 5);
            action = new CCActionSequence(new CCActionSpawn(new CCActionScaleTo(0.5f, 3f), new CCActionFadeOut(1.0f)), new RemoveSelf());
            pSmoke.RunAction(action);
            this.effectNode.AddChild(pSmoke);

            pSmoke = new CCSprite("effects_006-2.png", true);
            pSmoke.BlendFunc = BlendFunc.Additive;
            pSmoke.Color = new Color32(250, 90, 5);
            pSmoke.Scale = 2;
            action = new CCActionSequence(new CCActionFadeOut(1.5f), new RemoveSelf());
            pSmoke.RunAction(action);
            this.effectNode.AddChild(pSmoke);

            pSmoke = new CCSprite("effects_012.png", true);
            pSmoke.BlendFunc = BlendFunc.Additive;
            pSmoke.Color = new Color32(250, 90, 5);
            pSmoke.Scale = 1.5f;
            action = new CCActionSequence(new CCActionFadeOut(1.8f), new RemoveSelf());
            pSmoke.RunAction(action);
            this.effectNode.AddChild(pSmoke);

            CCSprite explode = new CCSprite("effects_004.png", true);
            explode.BlendFunc = BlendFunc.Additive;
            //explode.Color = new Color32(250, 90, 5);
            action = new CCActionSequence(new CCActionSpawn(new CCActionScaleTo(0.5f, 4.0f), new CCActionFadeOut(0.8f)), new RemoveSelf());
            explode.RunAction(action);
            this.effectNode.AddChild(explode);
            //
            //             patch1 = null;
            //             patch2 = null;
            //             smoke2 = null;
            //             pSmoke = null;
            //             explode = null;

            this.effectNode.RunSequenceActions(new CCActionDelayTime(timeOfDeadCall), new RemoveSelf());
            this.Parent.AddChild(this.effectNode);
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
                    PlayAnim(AnimName.变形1, false);
                    break;
                case State.Dead:
                    {
                        PlayingScene.gameScore += this.Info.score;
                        this.PlayDeadEffect();
                        this.Destroy();
                    }
                    break;
                default:
                    break;
            }

            return true;
        }

        public override void BeAttacked(float damge)
        {
            base.BeAttacked(damge);

            if (Panel_enemyProgressBar != null)
            {
                ((UILoadingBar)Panel_enemyProgressBar.GetChild(11)).Percent = (int)(100 * this.Info.HP / this.Info.totalHp);
            }
        }
        protected double perHitTime;
        protected override void OnBulletHit(BulletEmitter bulletEmitter, Bullet bullet, MatrixEngine.Math.Vector2 hitPos)
        {
            TimeSpan timeSpan = new TimeSpan(DateTime.Now.Ticks);
            double currentTime = timeSpan.TotalMilliseconds;
            if (currentTime - perHitTime > Function.attackedTimeSpan && this.CurrentState!=State.Dead)
            {
                this.colorPercent = 0;
                this.BeAttacked(bulletEmitter.UserDamage + bulletEmitter.Damage);
                perHitTime = currentTime;
            }
        }
    }
}
