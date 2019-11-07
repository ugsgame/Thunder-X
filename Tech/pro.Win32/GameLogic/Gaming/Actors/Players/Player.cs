
using System;

using Thunder.Game;
using MatrixEngine.CocoStudio.Armature;
using Thunder.GameLogic.Gaming.BulletSystems;
using Thunder.GameLogic.Gaming.Actors.Drops;
using Thunder.GameLogic.Common;
using MatrixEngine.Cocos2d;
using MatrixEngine.Math;
using Thunder.Common;
using Thunder.GameLogic.ExternAction;

namespace Thunder.GameLogic.Gaming.Actors.Players
{
    /// <summary>
    /// 玩家基类
    /// TODO:暂时用来测试的
    /// </summary>
    public class Player : Actor
    {
        /// <summary>
        /// 机体是否要侧身速度验证值
        /// </summary>
        private readonly float sideParalax = 30.0f;

        public enum Behavior
        {
            Null,
            FlyOut,
            FlyIn,
            Skilling,
            Playing,
        }
        protected Behavior curBehavior;

        //         protected int mSkills = 0;      //技能
        //         protected int mShields = 2;     //护盾
        //         protected int mBomb = 1;        //大招
        protected int curLevel = 1;       //级数
        protected int topLevel = 4;     //顶级
        //
        protected CCSprite shieldCover;
        protected CCParticleSystem shieldSpark;
        //
        private LevelManager levelManager;
        public LevelManager LevelManager
        {
            set { levelManager = value; }
            get { return levelManager; }
        }

        private FighterData fighterData;
        private FighterData.FighterLevelData levelData;

        public Vector2 StayPoint;       //停留点
        public Vector2 InitPoint;

        public bool WillUseSkill;
        public bool WillUseShield;

        protected readonly float timeOfLevelup = 3;
        protected float countOfLevelup = 0;

        protected readonly float timeOfInvincible = 5;
        protected bool isInvincible;

        protected CCParticleSystem attackEffect;

        private bool isShowing = true;
        public bool IsShowing
        {
            set { isShowing = value; }
            get { return isShowing; }
        }

        public Thunder.GameLogic.Gaming.PlayerSpawner.PlayerID PlayerID
        {
            get;
            set;
        }

        public Player(SpawnInfo spawnInfo)
            : base(spawnInfo)
        {
            this.Init(spawnInfo);
        }

        public Player(SpawnInfo spawnInfo, EventSpawner eventSpawner)
            : base(spawnInfo)
        {
            this.EventSpawner = eventSpawner;
            this.Init(spawnInfo);
        }

        protected virtual void Init(SpawnInfo spawnInfo)
        {
            this.ZOrder = PlayingScene.ZOrder_player;
            this.Filter = FilterType.FilterPlayer;
            Info.delayTime = 0;
            //Info.damage = 100;
            //
            shieldCover = new CCSprite("effects_038.png", true);
            shieldCover.IsVisible = false;
            shieldCover.ZOrder = 100;
            shieldCover.BlendFunc = BlendFunc.Additive;
            this.AddChild(shieldCover);
            //
            //默认动画
            try
            {
                animActions[AnimName.Null] = Info.animName + "_fly1";
                animActions[AnimName.待机1] = Info.animName + "_fly1";
                animActions[AnimName.行走1] = Info.animName + "_fly1";
                animActions[AnimName.行走2] = Info.animName + "_fly2";
                animActions[AnimName.变形1] = Info.animName + "_transform";
                animActions[AnimName.变形2] = Info.animName + "_return";
            }
            catch (Exception e)
            {
                Utils.PrintException(e);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                attackEffect.Dispose();
            }

            this.RemoveChild(this.shieldCover);
            this.shieldCover.Dispose();
            this.shieldCover = null;

            base.Dispose(disposing);
        }

        protected override void OnEnter()
        {
            //BulletEmitter.RegisterTarget(this);
            base.OnEnter();
            this.GotoBehavior(Behavior.Null);

            this.WillUseSkill = false;
            this.WillUseShield = false;

            this.shieldCover.RunAction(new CCActionRepeatForever(new CCActionSpawn(new CCActionRotateBy(1, 180), new ActionColorChange(1))));

            this.shieldSpark = new CCParticleSystem(ResID.Particles_shieldSpark);
            this.shieldSpark.PositionType = tCCPositionType.kCCPositionTypeGrouped;
            this.shieldSpark.Postion = 0;
            this.shieldSpark.ZOrder = 101;
            this.AddChild(shieldSpark);
            this.UpdateEffect(0.017f);

            attackEffect = new CCParticleSystem(ResID.Particles_hitSpark);
            attackEffect.Postion = 0;
            attackEffect.PositionType = tCCPositionType.kCCPositionTypeGrouped;
            attackEffect.ZOrder = 20;
            this.AddChild(attackEffect);

            this.OnLevelUp(1, true);
            WingmanSpawner.Instance.OnLevelUp(1);

            fighterData = PlayingScene.Instance.CurFighterData;
            levelData = PlayingScene.Instance.CurFighterLevelData;

            if (!this.isShowing)
            {
                this.Info.HP = fighterData.hp + levelData.attachHP;
                this.Info.totalHp = this.Info.HP;
                this.Info.damage = fighterData.damage + levelData.attachPlayerDamage;
                this.Info.critTime = GameData.Instance.CritTime + levelData.attachCritTime;

                this.ResetUserDamage();
            }
            this.isInvincible = false;
            this.Animable.armature.IsVisible = true;
        }

        protected override void OnExit()
        {
            base.OnExit();
            this.CloseFire();
            //BulletEmitter.UnregisterTarget(this);

            this.shieldSpark.Stop();
            this.RemoveChild(this.shieldSpark);
            this.shieldSpark.Dispose();
            this.shieldSpark = null;

            this.attackEffect.Stop();
            this.RemoveChild(this.attackEffect);
            this.attackEffect.Dispose();
            this.attackEffect = null;


            if (FrenzyOverSchedule)
            {

            }
        }

        bool FrenzyOverSchedule;
        //暴走
        protected virtual void FrenzyOver(float time)
        {
            if (this.CurrentState == State.Dead)
            {
                this.PlayAnim(AnimName.行走1);
            }
            else
            {
                this.PlayAnim(AnimName.变形2, false);
                if (GameData.Instance.PlayerData.withWingman)
                {
                    WingmanSpawner.Instance.Count = 1;
                }
                else
                {
                    WingmanSpawner.Instance.Count = 0;
                }
            }
            //
            if (this.isShowing)
            {
                this.curLevel = 1;
            }
            else
            {
                this.curLevel = topLevel;
            }
            this.OnLevelUp(curLevel, true);
            WingmanSpawner.Instance.OnLevelUp(curLevel);
            FrenzyOverSchedule = false;
            this.Unschedule("FrenzyOver");
        }

        public virtual void FlyOut()
        {
            this.GotoBehavior(Behavior.FlyOut);
        }

        public virtual void FlyIn()
        {
            this.GotoBehavior(Behavior.FlyIn);
        }

        protected override void OnAnimStart(Animable anim, string animName)
        {

        }

        protected override void OnAnimComplete(Animable anim, string animName)
        {
            if (this.EqualsAnimName(animName, AnimName.变形1))
            {
                //animActions[AnimName.行走1] = Info.animName + "_fly2";
                //this.GotoState(State.Idle);
                this.PlayAnim(AnimName.行走2);
                this.Schedule("FrenzyOver", this.Info.critTime);
                FrenzyOverSchedule = true;
            }
            else if (this.EqualsAnimName(animName, AnimName.变形2))
            {
                //animActions[AnimName.行走1] = Info.animName + "_fly1";
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

        protected virtual void FlyOutCallBack()
        {
            //Console.WriteLine("FlyOut Over");
            this.GotoBehavior(Behavior.Null);
            this.IsVisible = false;
        }

        protected virtual void FlyInCallBack()
        {
            //Console.WriteLine("FlyIn Over");
            this.GotoBehavior(Behavior.Playing);

            if (GameData.Instance.PlayerData.withWingman)
            {
                if (this.CurrentState != State.Frenzy)
                {
                    if (GameData.Instance.PlayerData.withWingman)
                        WingmanSpawner.Instance.Count = 1;
                    else
                        WingmanSpawner.Instance.Count = 0;
                }
            }
            if (!this.isShowing)
            {
                this.PlayInvincibleEffect();
                this.RunSequenceActions(new CCActionDelayTime(2), new CCActionCallFunc(this.ShowVIP));
            }
        }

        private void ShowVIP()
        {
            if (this.curBehavior == Behavior.Playing)
            {
                PlayingScene.Instance.ShowVIP();
            }
        }

        public Player.Behavior CurBehavior
        {
            get
            {
                return curBehavior;
            }
        }

        public virtual bool GotoBehavior(Behavior nextBehavior)
        {
            if (nextBehavior == curBehavior) return false;

            this.IsVisible = true;
            switch (nextBehavior)
            {
                case Behavior.Null:
                    {
                        this.StopAllAction();
                        this.IsVisible = false;
                        this.CloseFire();
                    }
                    break;
                case Behavior.FlyOut:
                    {
                        this.StopAllAction();
                        this.CloseFire();
                        var move = new CCActionEaseOut(new CCActionMoveTo(0.8f, new Vector2(this.PostionX, this.PostionY + Config.GAME_HEIGHT)), 0.5f);
                        var flyOutAction = new CCActionSequence(move, new CCActionCallFunc(this.FlyOutCallBack));
                        this.RunAction(flyOutAction);

                        this.isInvincible = false;

                        if (FrenzyOverSchedule)
                        {
                            this.FrenzyOver(0);
                        }
                        GotoState(State.Idle);

                        if (this.isShowing)
                        {
                            //
                        }
                        else
                        {
                            if (GameData.Instance.PlayerData.withWingman)
                            {
                                WingmanSpawner.Instance.Count = 0;
                            }
                            GameAudio.PlayEffect(GameAudio.Effect.play_expedite);
                        }
                    }
                    break;
                case Behavior.FlyIn:
                    {
                        this.curLevel = 1;
                        this.OnLevelUp(curLevel, true);
                        WingmanSpawner.Instance.OnLevelUp(curLevel);

                        this.StopAllAction();
                        this.CloseFire();
                        this.Postion = this.InitPoint;
                        this.IsVisible = true;
                        BulletEmitter.RegisterTarget(this);

                        GotoState(State.Idle);
                        GameAudio.PlayEffect(GameAudio.Effect.play_enter);

                        CCAction move;
                        if (this.isShowing)
                        {
                            move = new CCActionEaseIn(new CCActionMoveTo(0.5f, StayPoint), 0.7f);
                        }
                        else
                        {
                            move = new CCActionEaseIn(new CCActionMoveTo(1.0f, StayPoint), 0.5f);
                            if (GameData.Instance.PlayerData.withWingman)
                            {
                                WingmanSpawner.Instance.Count = 0;
                            }
                        }

                        var flyInAction = new CCActionSequence(move, new CCActionCallFunc(this.FlyInCallBack));
                        this.RunAction(flyInAction);
                    }
                    break;
                case Behavior.Skilling:
                    {

                    }
                    break;
                case Behavior.Playing:
                    {
                        this.OpenFire();
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
            if (currentState == nextState) return false;
            switch (nextState)
            {
                case State.Null:
                    PlayAnim(AnimName.Null);
                    break;
                case State.Idle:
                    {
                        PlayAnim(AnimName.行走1);
                    }
                    break;
                case State.Walk:
                    break;
                case State.Frenzy:
                    {
                        PlayAnim(AnimName.变形1, false);
                        WingmanSpawner.Instance.Count = 3;
                    }
                    break;
                case State.Retreat:
                    break;
                case State.Ready:
                    break;
                case State.ByAttack:
                    break;
                case State.Attack:
                    break;
                case State.Skill:
                    break;
                case State.Escape:
                    break;
                case State.Dead:
                    {
                        //
                        if (FrenzyOverSchedule)
                        {
                            this.FrenzyOver(0);
                        }
                        //
                        PlayAnim(AnimName.行走1);
                        //
                        //if (GameData.Instanse.PlayerData.withWingman)
                        {
                            WingmanSpawner.Instance.FlyOut();
                        }
                        //               
                        this.PlayDeadEffect();
                        //2秒后弹出复活对话框
                        TimeActionCall(2);
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

        protected override void OnTimeCall(float time)
        {
            base.OnTimeCall(time);
            //弹出复活框
            Function.GoTo(UIFunction.战机复活);
        }

        public virtual void TickSpeed(float tickSpeed)
        {
            //侧身
            if (Math.Abs(tickSpeed) > sideParalax)
            {
                //左侧身
                if (tickSpeed > 0)
                {

                }
                //右侧身
                else
                {

                }
            }
            else
            {

            }
        }
        //无敌结束回调
        private void InvincibleOver()
        {
            isInvincible = false;
            this.Animable.armature.IsVisible = true;
        }
        protected virtual void PlayInvincibleEffect()
        {
            
            isInvincible = true;
            this.Animable.armature.RunSequenceActions(new CCActionBlink(timeOfInvincible, 20), new CCActionCallFunc(this.InvincibleOver));
        }

        protected virtual void PlayDrop()
        {
            DropManager.Instance.DrapPlayer(this, DropType.Drop_Gem_Blue_1, 20, 5);
            DropManager.Instance.DrapPlayer(this, DropType.Drop_Gem_Blue_2, 15, 5);

            DropManager.Instance.DrapPlayer(this, DropType.Drop_Gem_Green_1, 5, 5);
            DropManager.Instance.DrapPlayer(this, DropType.Drop_Gem_Green_2, 5, 5);

            DropManager.Instance.DrapPlayer(this, DropType.Drop_Gem_Yellow_1, 3, 3);
            DropManager.Instance.DrapPlayer(this, DropType.Drop_Gem_Yellow_2, 3, 3);

            DropManager.Instance.DrapPlayer(this, DropType.Drop_Power, 4);
            DropManager.Instance.DrapPlayer(this, DropType.Drop_Shield, 3);
            DropManager.Instance.DrapPlayer(this, DropType.Drop_Skill, 1);
        }

        private CCNode effectNode;
        protected override void PlayDeadEffect()
        {
            base.PlayDeadEffect();
            GameAudio.PlayEffect(GameAudio.Effect.die_elite);
            this.IsVisible = false;
            this.PlayDrop();

            this.effectNode = new CCNode();
            this.effectNode.Postion = this.Postion;
            this.ZOrder = PlayingScene.ZOrder_effect;

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

            this.effectNode.RunSequenceActions(new CCActionDelayTime(3), new RemoveSelf());
            this.Parent.AddChild(this.effectNode);
        }

        protected virtual void UpdateEffect(float dTime)
        {
            if (this.isShowing)
            {
                this.shieldCover.IsVisible = false;
                this.shieldSpark.IsVisible = false;
                this.shieldSpark.Stop();
            }
            else
            {
                if (GameData.Instance.PlayerData.shields + PlayingScene.Instance.AttachShield > 0)
                {
                    this.shieldCover.IsVisible = true;
                    if (!this.shieldSpark.IsActive())
                        this.shieldSpark.Play();
                }
                else
                {
                    this.shieldCover.IsVisible = false;
                    this.shieldSpark.Stop();
                }
            }
        }

        public override void OnUpdate(float dTime)
        {
            base.OnUpdate(dTime);

            this.UpdateEffect(dTime);

            if (this.WillUseShield)
            {
                this.UsingShield();
                this.WillUseShield = false;
            }
            if (this.WillUseSkill)
            {
                this.UsingSkill();
                this.WillUseSkill = false;
            }
            /////////////////////////////////////////////////////////////////////
            //展示时自动升级
            if (this.isShowing && this.curBehavior == Behavior.Playing)
            {
                countOfLevelup += dTime;
                if (countOfLevelup >= timeOfLevelup)
                {
                    if (this.curLevel < this.topLevel)
                    {
                        this.curLevel++;
                        this.OnLevelUp(this.curLevel, true);

                        if (GameData.Instance.PlayerData.withWingman)
                            WingmanSpawner.Instance.OnLevelUp(curLevel);
                    }
                    else //暴走
                    {
                        if (this.CurrentState != State.Frenzy)
                        {
                            this.GotoState(State.Frenzy);
                            this.OnFrenzy();

                            if (GameData.Instance.PlayerData.withWingman)
                                WingmanSpawner.Instance.OnLevelUp(5);
                        }
                    }

                    countOfLevelup = 0;
                }
            }
            //////////////////////////////////////////////////////////////////////
        }

        public virtual void UsingShield()
        {
            //Console.WriteLine("UsingShield");
            if (GameData.Instance.PlayerData.shields + PlayingScene.Instance.AttachShield > 0)
            {
                levelManager.ScreenBulletToGems();
                GameAudio.PlayEffect(GameAudio.Effect.play_protect);

                if (PlayingScene.Instance.AttachShield > 0)
                {
                    PlayingScene.Instance.AttachShield--;
                }
                else
                {
                    GameData.Instance.PlayerData.shields--;
                }
                
                //
                CCSprite mSpriteLight = new CCSprite("effects_ui005.png", true);
                CCSprite mSpriteShield = new CCSprite("effects_040.png", true);

                mSpriteShield.Color = new Color32(23, 176, 221);
                mSpriteShield.BlendFunc = BlendFunc.Additive;

                mSpriteLight.Color = new Color32(23, 180, 200);
                mSpriteLight.BlendFunc = BlendFunc.Additive;
                mSpriteLight.Scale = 2.0f;

                mSpriteShield.Scale = 0;
                mSpriteShield.Postion = this.Postion;
                mSpriteShield.ZOrder = 1100;

                mSpriteLight.Postion = this.Postion;
                mSpriteLight.ZOrder = 1101;

                this.Parent.AddChild(mSpriteLight);
                this.Parent.AddChild(mSpriteShield);

                var action = new CCActionSequence(new CCActionEaseOut(new CCActionScaleTo(1, 20), 0.5f), new RemoveSelf());
                mSpriteShield.RunAction(action);

                var action1 = new CCActionSequence(new CCActionRotateBy(0.5f, 360), new RemoveSelf());
                mSpriteLight.RunAction(action1);
                //
            }

        }

        public virtual void UsingSkill()
        {
            //Console.WriteLine("UsingSkill");
            if (GameData.Instance.PlayerData.skills > 0)
            {
                levelManager.ScreenBulletToGems(DropType.Drop_Gem_Blue_1);
                try
                {
                    levelManager.ScreenEnemyKilled(GameData.Instance.SkillDamage + levelData.attachSkillDamage);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                GameAudio.PlayEffect(GameAudio.Effect.play_seckill);
                GameData.Instance.PlayerData.skills--;

                CCNode agentNode = new CCNode();
                agentNode.ZOrder = PlayingScene.ZOrder_effect;
                agentNode.Postion = Config.GameCenter;

                CCSprite light1 = new CCSprite("effects_040.png", true);
                CCSprite light2 = new CCSprite("effects_004.png", true);
                CCSprite light3 = new CCSprite("effects_073.png", true);
                CCSprite light4 = new CCSprite("effects_009.png", true);

                light1.BlendFunc = BlendFunc.Additive;
                light2.BlendFunc = BlendFunc.Additive;
                light3.BlendFunc = BlendFunc.Additive;
                light4.BlendFunc = BlendFunc.Additive;

                light1.Scale = 1.3f;
                light2.Scale = 2.7f;
                light2.Alpha = 0;
                light3.Scale = 3.5f;
                light3.Color = new Color32(255, 246, 100);
                light4.Scale = 2.0f;
                light4.IsVisible = false;

                light1.RunSequenceActions(new CCActionSpawn(new CCActionFadeOut(0.5f), new CCActionScaleTo(0.4f, 3.0f)), new RemoveSelf());
                light2.RunSequenceActions(new CCActionFadeIn(0.3f), new CCActionDelayTime(0.1f), new CCActionSpawn(new CCActionFadeOut(0.5f), new CCActionScaleTo(0.5f, 4.0f)), new RemoveSelf());
                light3.RunSequenceActions(new CCActionFadeIn(0.2f), new CCActionSpawn(new CCActionFadeOut(0.5f), new CCActionScaleTo(0.5f, 10.0f)), new RemoveSelf());
                light4.RunSequenceActions(new CCActionDelayTime(0.2f), new CCActionShow(), new CCActionSpawn(new CCActionFadeOut(0.5f), new CCActionScaleTo(0.5f, 8f)), new RemoveSelf());

                agentNode.AddChild(light2);
                agentNode.AddChild(light4);
                agentNode.AddChild(light1);
                agentNode.AddChild(light3);

                agentNode.RunSequenceActions(new CCActionDelayTime(3), new RemoveSelf());
                this.Parent.AddChild(agentNode);
            }
        }

        public virtual void UsingBomb()
        {

        }
        // 
        protected virtual CCNode PlayDropEffect(Color32 color, string image1, string particle)
        {
            CCNode agent = new CCNode();
            CCSprite levelImg1 = new CCSprite(image1, true);
            CCSprite levelImg2 = new CCSprite(image1, true);
            CCSprite light = new CCSprite("effects_light03.png", true);
            CCParticleSystem par = new CCParticleSystem(particle);

            levelImg1.BlendFunc = BlendFunc.Additive;
            levelImg2.BlendFunc = BlendFunc.Additive;
            light.BlendFunc = BlendFunc.Additive;
            light.Color = color;

            par.Postion = 0;
            par.Play();
            par.IsAutoRemoveOnFinish = true;

            agent.Postion = this.Postion;
            agent.ZOrder = PlayingScene.ZOrder_effect;

            levelImg1.RunAction(new CCActionSequence(new CCActionFadeIn(0.5f), new CCActionSpawn(new CCActionFadeOut(0.5f), new CCActionMoveBy(0.5f, 0, 50))));
            levelImg2.RunAction(new CCActionSpawn(new CCActionFadeOut(0.5f), new CCActionScaleTo(0.5f, 1.5f)));
            light.RunSpawnActions(new CCActionScaleTo(0.3f, 3), new CCActionFadeOut(0.5f));

            agent.RunSequenceActions(new CCActionEaseIn(new CCActionMoveBy(0.5f, 20, 0), 0.3f), new CCActionDelayTime(3), new RemoveSelf());

            agent.AddChild(levelImg1);
            agent.AddChild(levelImg2);
            agent.AddChild(light);
            agent.AddChild(par);

            this.Parent.AddChild(agent);
            return agent;
        }
        //增加等级
        protected virtual void OnLevelUp(int level, bool def = false)
        {
            if (!def)
            {
                PlayDropEffect(new Color32(243, 100, 10), "effects_zhandoutishi02.png", ResID.Particles_spark2);
            }
        }
        protected virtual void OnFrenzy()
        {

        }
        //增加技能
        protected virtual void OnSkillUp()
        {
            PlayDropEffect(new Color32(243, 40, 10), "effects_zhandoutishi03.png", ResID.Particles_spark3);
        }
        //增加护盾
        protected virtual void OnShieldUp()
        {
            PlayDropEffect(new Color32(13, 159, 243), "effects_zhandoutishi04.png", ResID.Particles_spark4);
        }
        //增加炸弹
        protected virtual void OnBombUp()
        {

        }

        protected double perHitTime;
        protected override void OnBulletHit(BulletEmitter bulletEmitter, Bullet bullet, MatrixEngine.Math.Vector2 hitPos)
        {
            TimeSpan timeSpan = new TimeSpan(DateTime.Now.Ticks);
            double currentTime = timeSpan.TotalMilliseconds;
            if (currentTime - perHitTime > Function.attackedTimeSpan)
            {
                if (this.curBehavior == Behavior.Playing && this.CurrentState!=State.Dead && !this.isInvincible)
                {
                    if (GameData.Instance.PlayerData.shields + PlayingScene.Instance.AttachShield > 0)
                    {
                        this.UsingShield();
                    }
                    else
                    {
                        this.attackEffect.Start();
                        if (this.CurrentState != State.Frenzy)
                        {
                            if (this.curLevel > 1)
                            {
                                this.curLevel--;
                                this.OnLevelUp(this.curLevel, true);
                            }
                        }
                        PlayingScene.Instance.OnPlayerHit(bulletEmitter.UserDamage + bulletEmitter.Damage);
                        this.BeAttacked(bulletEmitter.UserDamage + bulletEmitter.Damage);
                    }
                }
                perHitTime = currentTime;
            }

        }
        public override void OnDropAdsorbent(Drops.Drop drop)
        {
            GameData gameData = GameData.Instance;
            PlayerData playerData = gameData.PlayerData;

            //TODO:待完善
            switch (drop.Type)
            {
                case DropType.Drop_Gem_Blue_1:
                    {
                        PlayingScene.gameGolds += 1;
                        PlayingScene.gameScore += 5;
                    }
                    break;
                case DropType.Drop_Gem_Blue_2:
                    {
                        PlayingScene.gameGolds += 2;
                        PlayingScene.gameScore += 10;
                    }
                    break;
                case DropType.Drop_Gem_Yellow_1:
                    {
                        PlayingScene.gameGolds += 10;
                        PlayingScene.gameScore += 40;
                    }
                    break;
                case DropType.Drop_Gem_Yellow_2:
                    {
                        PlayingScene.gameGolds += 20;
                        PlayingScene.gameScore += 50;
                    }
                    break;
                case DropType.Drop_Gem_Green_1:
                    {
                        PlayingScene.gameGolds += 4;
                        PlayingScene.gameScore += 20;
                    }
                    break;
                case DropType.Drop_Gem_Green_2:
                    {
                        PlayingScene.gameGolds += 6;
                        PlayingScene.gameScore += 30;
                    }
                    break;
                case DropType.Drop_Gem_White:
                    {
                    }
                    break;
                case DropType.Drop_Power:
                    {
                        if (this.curLevel < this.topLevel)
                        {
                            this.curLevel++;
                            this.OnLevelUp(this.curLevel);
                            WingmanSpawner.Instance.OnLevelUp(curLevel);
                        }
                        else //暴走
                        {
                            if (this.CurrentState != State.Frenzy)
                            {
                                this.GotoState(State.Frenzy);
                                PlayingScene.Instance.PlayFrenzyEffect();
                                this.OnFrenzy();
                                WingmanSpawner.Instance.OnLevelUp(5);
                            }
                        }
                    }
                    break;
                case DropType.Drop_Shield:
                    {
                        playerData.shields++;

                        if (playerData.shields == 1)
                        {
                            this.shieldCover.Alpha = 0;
                            this.shieldCover.Scale = 4;
                            this.shieldCover.RunSpawnActions(new CCActionFadeIn(1.0f), new CCActionEaseExponentialOut(new CCActionScaleTo(1.0f, 1.0f)));
                            this.shieldSpark.Play();
                        }

                        this.OnShieldUp();
                    }
                    break;
                case DropType.Drop_Bomb:
                    {
                        this.OnBombUp();
                    }
                    break;
                case DropType.Drop_Skill:
                    {
                        playerData.skills++;
                        this.OnSkillUp();
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
