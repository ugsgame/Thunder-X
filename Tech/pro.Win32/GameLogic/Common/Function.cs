using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MatrixEngine.Engine;
using MatrixEngine.Math;
using MatrixEngine.Cocos2d;
using MatrixEngine.CocoStudio.GUI;
using Thunder.GameLogic.UI;
using Thunder.GameLogic.Gaming;
using MatrixEngine.CocoStudio.Armature;
using Thunder.Common;
using Thunder.GameLogic.UI.Dialogs;
using Thunder.GameBilling;
using Thunder.GameLogic.GameSystem;
using Thunder.GameLogic.UI.Guide;

namespace Thunder.GameLogic.Common
{
    public class Function
    {
        public enum ShowMode
        {
            mode_ui,
            mode_game,
        }

        public static readonly double attackedTimeSpan = 100;  //受攻击时限ms
        public static TotalGameUILoadable gameTotalUI;
        public static LoadingScene loadingScene;
        public static PlayingScene gameScene;
        //windows
        //

        public static void InitGameWindows()
        {
            loadingScene = new LoadingScene();
            gameTotalUI = new TotalGameUILoadable();
        }

        public static void GoTo(UIFunction functionID, params object[] _args)
        {
            ////必须要转换为string，不能删除此代码
            string[] args = new string[_args.Length];
            for (int i = 0; i < args.Length; i++)
            {
                args[i] = _args[i].ToString();
            }

            Console.WriteLine("Goto：" + functionID);
            System.GC.Collect();

            switch (functionID)
            {
                case UIFunction.初始化:
                    {
                        loadingScene.SetNextSence(gameTotalUI, args);
                        CCDirector.RunWithScene(loadingScene);
                    }
                    break;
                case UIFunction.游戏中:
                    {
                        //逻辑矩形是共公的，保证不被释放掉
                        CCSpriteFrameCache.AddSpriteFramesWithFile(ResID.Armatures_logicRect);

                        ShowGameScene(gameScene, Transition.CCTransitionFade, 1.0f);
                    }
                    break;
                case UIFunction.游戏暂停:
                    {
                        gameTotalUI.gamePuase.Show(true);
                    }
                    break;
                case UIFunction.游戏胜利:
                    {
                        gameTotalUI.gameWin.Show(true);
                    }
                    break;
                case UIFunction.主菜单:
                    {
                        if (args.Length > 0 && args[0] == "loading")
                        {
                            ShowGameScene(gameTotalUI.mainMenu);
                        }
                        else
                        {
                            ShowUIScene(gameTotalUI.mainMenu);
                        }
                    }
                    break;
                case UIFunction.游戏商店:
                    {
                        gameTotalUI.gameStore.Show(true);
                    }
                    break;
                case UIFunction.战机选择:
                    {
                        ShowUIScene(gameTotalUI.palyerSelecting);
                    }
                    break;
                case UIFunction.关卡选择:
                    {
                        if (args.Length > 0 && args[0] == "loading")
                        {
                            ShowGameScene(gameTotalUI.levelSelecting);
                        }
                        else
                        {
                            ShowUIScene(gameTotalUI.levelSelecting);
                        }
                    }
                    break;
                case UIFunction.排行榜:
                    {
                        gameTotalUI.gameRankingList.Show(true);
                    }
                    break;
                case UIFunction.强化战机:
                    {
                        if (args.Length > 0 && args[0] == "loading")
                        {
                            ShowGameScene(gameTotalUI.playerLevelUp);
                        }
                        else
                        {
                            ShowUIScene(gameTotalUI.playerLevelUp);
                        }
                    }
                    break;
                case UIFunction.战机复活:
                    {
                        gameTotalUI.gameResurrection.Show(true);
                    }
                    break;
                case UIFunction.游戏退出:
                    {
                        gameTotalUI.gameExit.Show(true);
                    }
                    break;
                case UIFunction.游戏关于:
                    {
                        gameTotalUI.gameAbout.Show(true);
                    }
                    break;
                case UIFunction.游戏VIP:
                    {
                        gameTotalUI.gameVIP.Show(true);
                    }
                    break;
                case UIFunction.购买护循:
                    {

                        gameTotalUI.buyShield.Show(true);
                    }
                    break;
                case UIFunction.购买技能:
                    {
                        gameTotalUI.buySkill.Show(true);
                    }
                    break;
                case UIFunction.购买金币:
                    {
                        gameTotalUI.buyGolds.Show(true);
                    }
                    break;
                case UIFunction.购买体力:
                    {
                        gameTotalUI.buyPower.Show(true);
                    }
                    break;
                case UIFunction.购买礼包:
                    {
                        gameTotalUI.buyRichGift.Show(true);
                    }
                    break;
                case UIFunction.购买惊喜礼包:
                    {
                        gameTotalUI.buySuppriseGift.Show(true);
                    }
                    break;
                case UIFunction.VIP提示:
                    {
                        gameTotalUI.vipMsg.Show(true);
                    }
                    break;
                default:
                    break;
            }
        }

        public static void ShowScene(GameScene Scene, ShowMode mode)
        {
            switch (mode)
            {
                case ShowMode.mode_ui:
                    ShowUIScene(Scene);
                    break;
                case ShowMode.mode_game:
                    ShowGameScene(Scene);
                    break;
                default:
                    break;
            }
        }

        //
        public static void ShowUIScene(GameScene uiScene)
        {
            ShowUIScene(uiScene, Transition.CCTransitionFade, 1.0f);
        }
        public static void ShowUIScene(GameScene uiScene, Transition tran, float dTime)
        {
            if (uiScene == null)
            {
                return;
            }
            //说明是第一次
            CCScene runingScene = CCDirector.GetRuningScene();
            if (runingScene == null)
            {
                RunWithGameScene(uiScene);
            }
            else
            {
                PushGameScene(uiScene, tran, dTime, null);
            }
        }

        public static void ShowGameScene(GameScene gameScene)
        {
            ShowGameScene(gameScene, Transition.CCTransitionFade, 1.0f);
        }
        /// <summary>
        /// 从其它界面到loading界面
        /// </summary>
        /// <param name="gameScene"></param>
        /// <param name="tran"></param>
        /// <param name="dTime"></param>
        public static void ShowGameScene(GameScene gameScene, Transition tran, float dTime, params string[] args)
        {
            if (gameScene == null)
            {
                return;
            }
            //说明是第一次
            CCScene runingScene = CCDirector.GetRuningScene();
            if (runingScene == null)
            {
                RunWithGameScene(gameScene, args);
            }
            else
            {
                loadingScene.SetNextSence(gameScene, args);
                //CCDirector.ReplaceScene(LoadingScene, tran, dTime);
                //CCDirector.PopScene();
                CCDirector.PushScene(loadingScene, tran, dTime);
            }
        }

        public static void ReplaceGameScene(GameScene gameScene, params string[] args)
        {
            gameScene.onTransitionArgs = args;
            CCDirector.ReplaceScene(gameScene);
            gameScene.ResetWindowManager();
        }

        public static void PushGameScene(GameScene gameScene, params string[] args)
        {
            gameScene.onTransitionArgs = args;
            CCDirector.PushScene(gameScene);
            gameScene.ResetWindowManager();
        }

        public static void ReplaceGameScene(GameScene gameScene, Transition tran, float dTime, params string[] args)
        {
            gameScene.onTransitionArgs = args;
            CCDirector.ReplaceScene(gameScene, tran, dTime);
            gameScene.ResetWindowManager();
        }

        public static void PushGameScene(GameScene gameScene, Transition tran, float dTime, params string[] args)
        {
            gameScene.onTransitionArgs = args;
            CCDirector.PushScene(gameScene, tran, dTime);
            gameScene.ResetWindowManager();
        }

        public static void RunWithGameScene(GameScene gameScene, params string[] args)
        {
            gameScene.onTransitionArgs = args;
            CCDirector.RunWithScene(gameScene);
            gameScene.ResetWindowManager();
        }

        /// <summary>
        /// 普通按钮声音
        /// </summary>
        public static void PlayButtonEffect(UIWidget widget)
        {
            GameAudio.PlayEffect(GameAudio.Effect.button);
        }
        /// <summary>
        /// 返回按钮声音
        /// </summary>
        public static void PlayBackButtonEffect(UIWidget widget)
        {
            GameAudio.PlayEffect(GameAudio.Effect.back);
        }
        /// <summary>
        /// 吃分声音
        /// </summary>
        static double oldTime;
        public static void PlayGemEffect()
        {
            double currentTime = DateTime.Now.ToOADate();
            if (currentTime - oldTime > 0.000001)
            {
                GameAudio.PlayEffect(GameAudio.Effect.crystal);
                oldTime = currentTime;
            }
        }
        public static void PlayGemEffect(UIWidget widget)
        {
            PlayGemEffect();
        }
        /// <summary>
        /// 吃道具声音
        /// </summary>
        public static void PlayProEffect()
        {
            GameAudio.PlayEffect(GameAudio.Effect.upgrade);
        }

        public static void ShowInfo(string msg)
        {
            if (!VIPMsg.Instance.IsShowing)
            {
                InfoShow.AddInfo(msg);
            }
        }
    }

    //Just for loading
    public class TotalGameUILoadable : GameScene
    {
        public MainMenuScene mainMenu;
        public PlayerSelectingScene palyerSelecting;
        public LevelSelectingScene levelSelecting;
        public PlayerLevelUpScene playerLevelUp;

        public SimulateSDK simulateSDK;

        public PowerRecovery powerRecovery;
        public GamePause gamePuase;
        public GameWin gameWin;
        public GameExit gameExit;
        public GameStore gameStore;
        public GameAbout gameAbout;
        public GameVIP gameVIP;
        public BuyPower buyPower;
        public BuyShield buyShield;
        public BuySkill buySkill;
        public BuyGolds buyGolds;
        public BuyRichGift buyRichGift;
        public BuySuppriseGift buySuppriseGift;
        public GameResurrection gameResurrection;
        public GameRankingList gameRankingList;
        public WingmanSelecting wingmanSelecting;

        public GuideWindow guideWindow;

        public VIPMsg vipMsg;
        public SpriteBatchManager batchManager;
        //
        public static UIWidget Panel_templates;
        //

        List<GameScene> UIScenes = new List<GameScene>();

        public TotalGameUILoadable()
        {
            //假数据
            /*
            GameData.Instanse.PlayerData.golds = 1000;
            GameData.Instanse.PlayerData.score = 100;
            GameData.Instanse.PlayerData.power = 1;

            GameData.Instanse.PlayerData.curFighter = PlayerSpawner.PlayerID.Player1;
            GameData.Instanse.PlayerData.curWingman = WingmanSpawner.WingmanID.Wingman1;

            GameData.Instanse.CurLevelIndex = 1;

            GameData.Instanse.PlayerData.withWingman = false;
            */
        }

        public override IEnumerable<LoadingScene.Percent> LoadSync()
        {
            var loadAble = LoadingScene.GetPercentsWithSum(20, 20, 20, 20, 20);
            //       
            GameData.Instance.LoadDataFile();
            //加载公共资源
            ArmatureResManager.Instance.Add(ResID.Armatures_Objects);
            ArmatureResManager.Instance.Add(ResID.Armatures_Effects);

            //加载关卡选的boss动画数据
            GameData.Instance.LoadBossArmature();
            //加载声音资源
            GameAudio.Init();
            GameAudio.PreloadAll();
            //

            yield return loadAble.NextPercent();
            
            mainMenu = new MainMenuScene();
            palyerSelecting = new PlayerSelectingScene();
            levelSelecting = new LevelSelectingScene();
            playerLevelUp = new PlayerLevelUpScene();

            yield return loadAble.NextPercent();
           
            Panel_templates = UIReader.GetWidget(ResID.UI_UI_Templates);

            simulateSDK = new SimulateSDK();

            powerRecovery = new PowerRecovery();
            gamePuase = new GamePause();
            gameExit = new GameExit();
            gameStore = new GameStore();
            gameWin = new GameWin();
            gameAbout = new GameAbout();
            gameVIP = new GameVIP();
            buyPower = new BuyPower();
            buyShield = new BuyShield();
            buySkill = new BuySkill();
            buyGolds = new BuyGolds();
            buyRichGift = new BuyRichGift();
            buySuppriseGift = new BuySuppriseGift();
            gameResurrection = new GameResurrection();
            gameRankingList = new GameRankingList();

            guideWindow = new GuideWindow();

            vipMsg = new VIPMsg();

            batchManager = new SpriteBatchManager();

            yield return loadAble.NextPercent();
            //         
            EffectSpawner.Instanse = new EffectSpawner();

            PlayerSpawner.Instanse = new PlayerSpawner();
            PlayerSpawner.Instanse.SetCurPlayer(GameData.Instance.PlayerData.curFighter);

            WingmanSpawner.Instance = new WingmanSpawner();
            WingmanSpawner.Instance.SetCurWingman(GameData.Instance.PlayerData.curWingman);

            yield return loadAble.NextPercent();
            //
            Function.gameScene = new PlayingScene();

            yield return loadAble.NextPercent(); 
            //从编辑器进入游戏
            if (EDebug.InEditor)
            {
                Function.gameScene.LoadImm();
                Function.loadingScene.SetNextSence(Function.gameScene);
            }
            else
            {
                Function.loadingScene.SetNextSence(mainMenu);
            }
            
        }

        public override bool OnTouchBegan(float x, float y)
        {
            Function.GoTo(UIFunction.主菜单);
            return false;
        }

        protected void RegisterUI()
        {

        }
       
        public UIWidget Temple_EnemyHPBar()
        {
            if (Panel_templates == null) Panel_templates = UIReader.GetWidget(ResID.UI_UI_Templates);
            return Panel_templates.GetWidget("Panel_enemyProgressBar");
        }

    }
}
