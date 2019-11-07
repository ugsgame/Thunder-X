using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Thunder.GameLogic.Common;
using MatrixEngine;
using MatrixEngine.Cocos2d;
using MatrixEngine.CocoStudio.Armature;
using Thunder.Common;
using Thunder.GameLogic.UI;
using MatrixEngine.CocoStudio.GUI;
using Thunder.GameLogic.Gaming.Actors.Players;
using Thunder.GameLogic.UI.Widgets;
using Thunder.GameLogic.Gaming.Actors.Bosses;
using Thunder.GameLogic.ExternAction;
using MatrixEngine.Math;
using Thunder.GameLogic.UI.Dialogs;
using Thunder.GameLogic.UI.Effects;
using Thunder.GameLogic.UI.Guide;

namespace Thunder.GameLogic.Gaming
{
    public class PlayingScene : GameScene, InterfaceGameState
    {
        //游戏元素图层次序
        //TODO:还没确定
        public static readonly int ZOrder_player = 1500;
        public static readonly int ZOrder_boss = 200;
        public static readonly int ZOrder_enemy = 100;
        public static readonly int ZOrder_drop = 800;
        public static readonly int ZOrder_batch = 900;
        public static readonly int ZOrder_bullet = 1000;
        public static readonly int ZOrder_effect = 1510;
        //
        public static float gameGolds;
        public static float gameScore;

        protected float displayGolds;
        protected float displayScore;
        //
        public FighterData CurFighterData;                          //当前战机数据
        public FighterData.FighterLevelData CurFighterLevelData;    //当前战机的等级数据

        public int AttachShield;
        //
        private GameLayer gameLayer;
        public GameLayer GameLayer
        {
            get { return gameLayer; }
        }

        private GameMap gameMap;
        public GameMap GameMap
        {
            get { return gameMap; }
        }

        private UILayer gameUILayer;
        private UIWidget gameUI;

        private UIButton Button_pause;
        private UIButton Button_skill;
        private UIButton Button_shield;
        private UIButton Button_supprise;

        private UIImageView Image_golds;
        private UIImageView Image_scroe;
        private UILabelAtlas AtlasLabel_golds;
        private UILabelAtlas AtlasLabel_scroe;

        private UILabelAtlas AtlasLabel_skill;
        private UILabelAtlas AtlasLabel_shield;

        private UILayout Panel_bossHP;
        private UILayout Panel_frenzyBar;
        private UILoadingBar ProgressBar_playerHP;
        private UILoadingBar ProgressBar_bossHP1;
        private UILoadingBar ProgressBar_bossHP2;
        private UILoadingBar ProgressBar_playerFrenzy;

        private CCParticleSystem playerHPSpark;
        //每撸一次护盾/技能最少间隔时间
        private readonly double timeOfSkill = 2;
        //private readonly double timeOfShield = 100; 

        private LevelManager levelManager;
        public LevelManager LevelManager
        {
            get { return levelManager; }
        }
        //
        private BossWaring bossWaring;
        private ResumeTimer resumeTimer;
        private FightWin fightWin;

        private CCProgressTimer skillProgressTimer;
        private UIImageView redScreenLeft;
        private UIImageView redScreenRight;

        private float redAlpha = 0;
        private bool HPWaringFlag;
        //
        private bool isPause;

        // 全局句柄
        public static PlayingScene Instance;

        public PlayingScene()
        {
            //Base res
            //CCArmDataManager.AddArmatureFile(ResID.Armatures_Objects);
            //layer
            if (EDebug.swMap)
            {
                gameMap = new GameMap();
            }

            gameLayer = new GameLayer();
            gameUILayer = new UILayer();
            //ui
            gameUI = UIReader.GetWidget(ResID.UI_UI_GamePlaying);
            Button_pause = (UIButton)gameUI.GetWidget("Button_pause");
            Button_skill = (UIButton)gameUI.GetWidget("Button_skill");
            Button_shield = (UIButton)gameUI.GetWidget("Button_shield");
            Button_supprise = (UIButton)gameUI.GetWidget("Button_supprise");

            AtlasLabel_skill = (UILabelAtlas)gameUI.GetWidget("AtlasLabel_skill");
            AtlasLabel_shield = (UILabelAtlas)gameUI.GetWidget("AtlasLabel_shield");

            UILayout Panel_golds = (UILayout)gameUI.GetWidget("Panel_golds");
            UILayout Panel_score = (UILayout)gameUI.GetWidget("Panel_score");

            AtlasLabel_golds = (UILabelAtlas)Panel_golds.GetWidget("AtlasLabel_golds");
            AtlasLabel_scroe = (UILabelAtlas)Panel_score.GetWidget("AtlasLabel_scroe");

            playerHPSpark = new CCParticleSystem(ResID.Particles_playerHPSpark);
            playerHPSpark.Postion = 0;
            playerHPSpark.Play();

            Panel_bossHP = (UILayout)gameUI.GetWidget("Panel_bossHP");
            ProgressBar_playerHP = (UILoadingBar)gameUI.GetWidget("ProgressBar_playerHP");
            ProgressBar_playerHP.AddNode(playerHPSpark);

            ProgressBar_bossHP1 = (UILoadingBar)Panel_bossHP.GetWidget("ProgressBar_bossHP1");
            ProgressBar_bossHP2 = (UILoadingBar)Panel_bossHP.GetWidget("ProgressBar_bossHP2");
            Panel_bossHP.IsVisible = false;
            Panel_frenzyBar = (UILayout)gameUI.GetWidget("Panel_frenzyBar");
            ProgressBar_playerFrenzy = (UILoadingBar)Panel_frenzyBar.GetWidget("ProgressBar_playerFrenzy");
            Panel_frenzyBar.IsVisible = false;
            ProgressBar_playerFrenzy.Percent = 100;

            bossWaring = new BossWaring();
            bossWaring.PostionX = Config.SCREEN_PosX_Center;
            bossWaring.PostionY = Config.SCREEN_PosY_Center + 150;
            gameUILayer.AddChild(bossWaring);

            resumeTimer = new ResumeTimer();
            //resumeTimer.Postion = Config.ScreenCenter;
            //gameUILayer.AddChild(resumeTimer);

            fightWin = new FightWin();

            skillProgressTimer = new CCProgressTimer(new CCSprite("effects_019.png", true));
            Button_skill.AddNode(skillProgressTimer);
            skillProgressTimer.ReverseDirection = true;
            skillProgressTimer.Scale = 1.5f;

            redScreenLeft = new UIImageView();
            redScreenLeft.LoadTexture(ResID.PIC_UI_beijihongping_01);
            redScreenLeft.AnchorPoint = new Vector2(1, 0.5f);
            redScreenLeft.PostionX = 0;
            redScreenLeft.PostionY = Config.SCREEN_PosY_Center;
            redScreenLeft.ScaleX = -2;
            redScreenLeft.ScaleY = Config.SCREEN_HEIGHT / redScreenLeft.Size.height;
            gameUILayer.AddWidget(redScreenLeft);

            redScreenRight = new UIImageView();
            redScreenRight.LoadTexture(ResID.PIC_UI_beijihongping_01);
            redScreenRight.AnchorPoint = new Vector2(1, 0.5f);
            redScreenRight.PostionX = Config.SCREEN_WIDTH;
            redScreenRight.PostionY = Config.SCREEN_PosY_Center;
            redScreenRight.ScaleX = 2;
            redScreenRight.ScaleY = Config.SCREEN_HEIGHT / redScreenRight.Size.height;
            gameUILayer.AddWidget(redScreenRight);
            //
            Button_pause.TouchEndedEvent += this.OnGamePause;
            Button_skill.TouchEndedEvent += this.OnGameSkill;
            Button_shield.TouchEndedEvent += this.OnGameShield;

            SuppriseEffect effect = new SuppriseEffect();
            Button_supprise.AddNode(effect);
            Button_supprise.TouchBeganEvent += button =>
            {
                Function.GoTo(UIFunction.购买惊喜礼包);
            };
            //
            if (EDebug.swMap)
            {
                this.AddChild(gameMap);
            }

            this.AddChild(gameLayer);
            this.AddChild(gameUILayer);
            //
            gameUI.ZOrder = 10;
            gameUILayer.AddWidget(gameUI);

            //Test LevelManager
            GameData.Instance.CurLevelName = "lv0.json";
            if (EDebug.InEditor && (EDebug.Mode != EDebug.DebugMode.Debug_Normal))
            {
                Console.WriteLine("EditorDebuging");
                Console.WriteLine("DebugMode:" + EDebug.Mode);
                Console.WriteLine("DebugLevel:" + EDebug.DebugLevel);
                Console.WriteLine("DebugEvent:" + EDebug.DebugEvent);
                GameData.Instance.CurLevelName = EDebug.DebugLevel + ".json";
            }

            levelManager = new LevelManager(gameLayer, GameData.Instance.CurLevelName);
            levelManager.GameMap = gameMap;
            levelManager.GameLayer = gameLayer;
            levelManager.PlayingLayer = this;

            Instance = this;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        public override IEnumerable<UI.LoadingScene.Percent> LoadSync()
        {
            var loadAble = LoadingScene.GetPercentsWithSeq(100);
            levelManager.SetLevel(GameData.Instance.CurLevelName);

            //设当前的战机数据
            CurFighterData = GameData.Instance.GetFighterData(PlayerSpawner.Instanse.CurPlayerID);
            CurFighterLevelData = CurFighterData.GetLevelData(CurFighterData.curLevel);

            this.AttachShield = CurFighterLevelData.attachShieldCount;

            //load level
            foreach (var item in levelManager.LoadLevel(loadAble))
            {
                yield return item;
            }
        }

        public bool LoadImm()
        {
            //设当前的战机数据
            CurFighterData = GameData.Instance.GetFighterData(PlayerSpawner.Instanse.CurPlayerID);
            CurFighterLevelData = CurFighterData.GetLevelData(CurFighterData.curLevel);

            levelManager.SetLevel(GameData.Instance.CurLevelName);
            return levelManager.LoadLevel();
        }

        public override void UnLoad()
        {
            try
            {
                this.levelManager.Dispose();
                //TODO:会崩未知原因
                ArmatureResManager.Instance.Release();
            }
            catch (Exception e)
            {

                Console.WriteLine(e);
            }

            base.UnLoad();
        }

        public override void OnEnter()
        {
            base.OnEnter();

            GameData.Instance.GameState = GameState.Playing;

            this.SetTouchMode(TouchMode.Single);
            this.SetState(LayerState.Touch, true);

            gameGolds = 0;
            gameScore = 0;
            displayGolds = 0;
            displayScore = 0;

            levelManager.Start();

            Panel_bossHP.IsVisible = false;
            Panel_frenzyBar.IsVisible = false;

            ResetUIData();
            //
            GameAudio.PlayMusic(GameAudio.Music.combat_bg, true);
            //
            this.OnResume();
            //
            this.shouldShowVIP = true;
        }

        public override void OnEnterTransitionFinish()
        {
            base.OnEnterTransitionFinish();
        }

        public override void OnExit()
        {
            base.OnExit();
            GameData.Instance.GameState = GameState.UI;
        }

        public override void OnExitTransitionStart()
        {
            base.OnExitTransitionStart();
        }

        private int HPDir = 1;
        public override void OnUpdate(float dTime)
        {
            base.OnUpdate(dTime);

            if (this.displayGolds < gameGolds)
            {
                this.displayGolds += (gameGolds - this.displayGolds) / (7000.0f * dTime);
            }
            if (this.displayScore < gameScore)
            {
                this.displayScore += (gameScore - this.displayScore) / (4500.0f * dTime);
            }

            if (!HPWaringFlag)
            {
                redAlpha -= 1000 * dTime;
            }
            else
            {
                if (redAlpha >= 255)
                    HPDir = -1;
                else if (redAlpha <= 0)
                    HPDir = 1;

                redAlpha += 800 * dTime * HPDir;
            }

            redAlpha = redAlpha < 0 ? 0 : redAlpha;
            redAlpha = redAlpha > 255 ? 255 : redAlpha;

            redScreenLeft.Alpha = (int)redAlpha;
            redScreenRight.Alpha = (int)redAlpha;

            Button_supprise.Enabled = !GameData.Instance.IsGetSuppriseGift;
            this.ResetUIData();
        }

        //Events
        private void OnGamePause(UIWidget widget)
        {
            Function.GoTo(UIFunction.游戏暂停);
        }

        //Boss警告结束回调
        private void OnBossWaringOver()
        {
            GameAudio.PlayMusic(GameAudio.Music.boss_bg);
            this.gameLayer.Player.OpenFire();
            WingmanSpawner.Instance.OpenFire();
            Function.GoTo(UIFunction.购买技能);
        }
        //释放护盾
        private void OnGameShield(UIWidget widget)
        {
            if (GuideWindow.Instance.Command == GuideCommand.使用护盾)
            {
                GameData.Instance.PlayerData.shields += 1;

                gameMap.PlayBlack(0.5f);
                gameLayer.Player.UsingShield();

                GuideWindow.Instance.Show = false;
                GuideWindow.Instance.Command = GuideCommand.Null;
            }
            else
            {
                if (GuideWindow.Instance.GetGuideData(GuideCommand.使用护盾).IsPlay == true)
                {
                    if (GameData.Instance.PlayerData.shields + this.AttachShield > 0)
                    {
                        gameMap.PlayBlack(0.5f);
                        gameLayer.Player.UsingShield();
                    }
                    else
                    {
                        //弹出购买护循
                        Function.GoTo(UIFunction.购买护循);
                    }
                }
            }
        }
        //释放技能
        private double oldSkillTime;
        private void OnGameSkill(UIWidget widget)
        {
            TimeSpan timeSpan = new TimeSpan(DateTime.Now.Ticks);
            double currentTime = timeSpan.TotalSeconds;

            if (GuideWindow.Instance.Command == GuideCommand.使用技能)
            {
                GameData.Instance.PlayerData.skills += 1;

                gameMap.PlayBlack(1.0f);
                gameLayer.Player.UsingSkill();

                GuideWindow.Instance.Show = false;
                GuideWindow.Instance.Command = GuideCommand.Null;
            }
            else
            {
                if (GuideWindow.Instance.GetGuideData(GuideCommand.使用技能).IsPlay == true)
                {
                    if (GameData.Instance.PlayerData.skills > 0)
                    {
                        if (currentTime - oldSkillTime > this.timeOfSkill)
                        {
                            gameMap.PlayBlack(1.0f);
                            gameLayer.Player.UsingSkill();

                            skillProgressTimer.RunAction(new CCActionProgressFromTo((float)this.timeOfSkill, 100, 0));
                            oldSkillTime = currentTime;
                        }
                    }
                    else
                    {
                        //弹出购买必杀
                        Function.GoTo(UIFunction.购买技能);
                    }
                }
            }
        }

        public bool IsPause
        {
            get
            {
                return isPause;
            }
            set
            {
                if (CurGameScene == this)
                {
                    isPause = value;
                    if (isPause)
                    {
                        this.OnPause();
                    }
                    else
                    {
                        //this.OnResume();

                        if (!VIPMsg.Instance.IsShowing)
                        {
                            resumeTimer.Play(this.OnResume);
                        }
                    }
                }
            }
        }
        //
        private bool shouldShowVIP;
        public virtual void ShowVIP()
        {
            if (this.shouldShowVIP && GameData.Instance.CurLevelIndex >= 3)
            {
                if (!GameData.Instance.IsVip)
                {
                    Function.GoTo(UIFunction.游戏VIP);
                }
                shouldShowVIP = false;
            }

        }
        //播放boss警告
        public virtual void PlayBossWaring()
        {
            this.bossWaring.Play(this.OnBossWaringOver);
            Panel_bossHP.IsVisible = true;
            ProgressBar_bossHP1.Percent = 0;
            ProgressBar_bossHP2.Percent = 0;
            ProgressBar_bossHP2.RunAction(new ActionLoadingBar(2.0f));
            ProgressBar_bossHP1.RunSequenceActions(new CCActionDelayTime(2.0f), new ActionLoadingBar(2.0f));
            GameAudio.StopMusic();

            this.gameLayer.Player.CloseFire();
            WingmanSpawner.Instance.CloseFire();
        }
        //播放玩家暴走效果
        private CCNode frenyLayer = new CCNode();
        public virtual void PlayFrenzyEffect()
        {
            this.PlayFrenzyBar();
            frenyLayer.RemoveAllChildren(true);
            GameAudio.PlayEffect(GameAudio.Effect.play_fury);

            Vector2 pos = new Vector2(Config.SCREEN_PosX_Center, Config.SCREEN_PosY_Center + 100);
            CCSprite shandia1 = new CCSprite("effects_shandian02.png", true);
            CCSprite shandia2 = new CCSprite("effects_shandian01.png", true);
            CCSprite light = new CCSprite("effects_light02.png", true);
            CCSprite title1 = new CCSprite("effects_zhandoutishi01.png", true);
            CCSprite title2 = new CCSprite("effects_zhandoutishi01.png", true);
            CCParticleSystem spark1 = new CCParticleSystem(ResID.Particles_spark1);

            shandia1.Postion = pos;
            shandia2.Postion = pos;
            light.Postion = pos;
            title1.Postion = pos;
            title2.Postion = pos;
            spark1.Postion = pos;

            shandia1.Scale = 2;
            shandia2.Scale = 2;
            light.Scale = 2;

            light.IsVisible = false;
            title1.Scale = 0.7f;
            title2.Scale = 0.7f;

            shandia1.BlendFunc = BlendFunc.Additive;
            shandia2.BlendFunc = BlendFunc.Additive;
            light.BlendFunc = BlendFunc.Additive;
            title1.BlendFunc = BlendFunc.Additive;
            title2.BlendFunc = BlendFunc.Additive;

            spark1.IsAutoRemoveOnFinish = true;
            spark1.Play();

            shandia1.RunAction(new CCActionSequence(new CCActionFadeIn(0.1f), new CCActionFadeOut(0.1f), new RemoveSelf()));
            shandia2.RunAction(new CCActionSequence(new CCActionFadeIn(0.3f), new CCActionFadeOut(0.5f), new RemoveSelf()));
            light.RunAction(new CCActionSequence(new CCActionDelayTime(0.5f), new CCActionShow(), new CCActionFadeIn(0.3f), new CCActionFadeOut(0.5f), new RemoveSelf()));
            title1.RunAction(new CCActionSequence(new CCActionSpawn(new CCActionFadeIn(0.3f), new CCActionDelayTime(1.0f), new CCActionScaleTo(0.3f, 1.0f)), new CCActionFadeOut(0.8f), new RemoveSelf()));
            title2.RunAction(new CCActionSequence(new CCActionSpawn(new CCActionFadeIn(0.3f), new CCActionScaleTo(0.3f, 1.0f)), new CCActionSpawn(new CCActionFadeOut(0.5f), new CCActionScaleTo(0.5f, 2.0f)), new RemoveSelf()));

            frenyLayer.ZOrder = 0;
            frenyLayer.AddChild(light);
            frenyLayer.AddChild(shandia1);
            frenyLayer.AddChild(shandia2);
            frenyLayer.AddChild(title1);
            frenyLayer.AddChild(title2);
            frenyLayer.AddChild(spark1);

            this.gameUILayer.AddChild(frenyLayer);
        }
        //
        private void HitFrenzyBar()
        {
            Panel_frenzyBar.IsVisible = false;
        }
        public virtual void PlayFrenzyBar()
        {
            Panel_frenzyBar.IsVisible = true;
            ProgressBar_playerFrenzy.RunSequenceActions(new ActionLoadingBar(this.gameLayer.Player.Info.critTime, 100, 0), new CCActionCallFunc(this.HitFrenzyBar));
        }
        //
        public virtual void PlayFightWin()
        {
            fightWin.Play(this.OnFightWinOver);
        }
        protected virtual void OnFightWinOver()
        {
            Function.GoTo(UIFunction.游戏胜利);
        }
        //Boss相关回调
        public virtual void OnBossOut(Boss boss)
        {

        }
        public virtual void OnBossDead(Boss boss)
        {
            Panel_bossHP.IsVisible = false;
            gameScore += boss.Info.score;
        }
        public virtual void OnBossHP(Boss boss)
        {
            float curHP;
            float totalHP;
            if (boss.Info.HP > boss.Info.frenzyHp)
            {
                ProgressBar_bossHP2.Percent = 100;
                curHP = boss.Info.HP - boss.Info.frenzyHp;
                totalHP = boss.Info.totalHp - boss.Info.frenzyHp;
                ProgressBar_bossHP1.Percent = (int)(100 * curHP / totalHP);
            }
            else
            {
                ProgressBar_bossHP1.Percent = 0;
                curHP = boss.Info.HP;
                totalHP = boss.Info.frenzyHp;
                ProgressBar_bossHP2.Percent = (int)(100 * curHP / totalHP);
            }
        }

        public virtual void OnPlayerHit(float damage)
        {
            if (!HPWaringFlag)
            {
                redAlpha = 255;
            }
        }
        //
        public virtual void OnPause()
        {
            GameAudio.PauseMusic();

            this.PauseSchedulerAndActions();
            this.gameLayer.OnPause();
            this.gameMap.OnPause();
            this.levelManager.OnPause();
        }

        public virtual void OnResume()
        {
            GameAudio.ResumeMusic();

            this.ResumeSchedulerAndActions();
            this.gameLayer.OnResume();
            this.gameMap.OnResume();
            this.levelManager.OnResume();
        }

        //按返回键暂停
        public override void OnKeyBackClicked()
        {
            //Function.GoTo(UIFunction.游戏暂停);
        }

        public virtual void ResetUIData()
        {
            Player player = gameLayer.Player;
            float percent = player.Info.HP / player.Info.totalHp;

            AtlasLabel_golds.Text = Convert.ToString((int)this.displayGolds);
            AtlasLabel_scroe.Text = Convert.ToString((int)this.displayScore);
            AtlasLabel_shield.Text = Convert.ToString(GameData.Instance.PlayerData.shields + this.AttachShield);
            AtlasLabel_skill.Text = Convert.ToString(GameData.Instance.PlayerData.skills);

            playerHPSpark.IsVisible = percent > 0.99f ? false : true;
            percent = player.IsDead ? 0 : percent;
            ProgressBar_playerHP.Percent = (int)(100.0f * percent);
            playerHPSpark.PostionX = -116 + 230 * percent;
            playerHPSpark.PostionY = 0;

            if (percent < 0.4 && percent > 0)
            {
                HPWaringFlag = true;
            }
            else
            {
                HPWaringFlag = false;
            }

        }
    }
}
