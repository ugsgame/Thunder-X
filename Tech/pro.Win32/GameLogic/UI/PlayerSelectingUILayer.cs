using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MatrixEngine.CocoStudio.GUI;
using Thunder.GameLogic.Common;
using MatrixEngine.Cocos2d;
using MatrixEngine.Math;
using Thunder.GameLogic.Gaming.BulletSystems;
using Thunder.GameLogic.Gaming;
using Thunder.GameLogic.Gaming.Actors;
using MatrixEngine.CocoStudio;
using Thunder.GameLogic.Gaming.Actors.Players;
using Thunder.GameLogic.UI.Dialogs;
using Thunder.GameLogic.GameSystem;
using Thunder.GameLogic.ExternAction;
using Thunder.GameLogic.UI.Guide;


namespace Thunder.GameLogic.UI
{
    public class PlayerSelectingUILayer : UILayer, IPageEventListener, IUpdateUI
    {
        UIWidget PlayerSelecting;


        UIButton Button_addGold;    //添加钻石
        UIButton Button_wingman;    //
        UIButton Button_left;
        UIButton Button_right;
        UIButton Button_levelUp;
        UIButton Button_selected;
        UIButton Button_back;
        UIButton Button_gifts;

        UIImageView Image_platform;
        UIImageView Image_backGroup;

        UIImageView Image_levelUp;
        UILayout Panel_open;
        UILabelAtlas UnloackGolds;

        UIPageView PageView_showPlayer;
        UILayout[] pages;
        CCSprite[] planes;

        CCClippingNode showingWorldNode = new CCClippingNode();

        private CCAction scaleAction1;
        private CCAction scaleAction2;

        private UILabelAtlas AtlasLabel_gold;
        private UILabelAtlas AtlasLabel_score;
        private UILabelAtlas AtlasLabel_level;

        private DialogWindow dialog_buy = new DialogWindow(Window.Priority.PRIORITY_DIALOG_SYSTEM);
        private DialogWindow dialog_lock = new DialogWindow(Window.Priority.PRIORITY_DIALOG_SYSTEM);

        private Vector2 stayPoint;
        private Vector2 playerInitPoint;

        private PlayerData playerData;
        private FighterData curFighterData;

        private WingmanSelecting wingmanSelecting;

        private CCSprite scanline;
        private Color32 lockColor = new Color32(50, 50, 50);

        private readonly int lock_tag = 101;

        private CCSprite goldLight;
        private CCSprite giftLight;
        private CCSprite selectLight;

        private int curPageIndex;

        public PlayerSelectingUILayer()
        {
            wingmanSelecting = new WingmanSelecting(this);

            PlayerSelecting = UIReader.GetWidget(ResID.UI_UI_PlayerSelecting);

            Button_addGold = (UIButton)(PlayerSelecting.GetWidget("Panel_gold").GetWidget("Button_addGold"));
            Button_wingman = (UIButton)PlayerSelecting.GetWidget("Button_wingman");
            Button_left = (UIButton)PlayerSelecting.GetWidget("Button_left");
            Button_right = (UIButton)PlayerSelecting.GetWidget("Button_right");
            Button_levelUp = (UIButton)PlayerSelecting.GetWidget("Button_levelUp");
            Button_selected = (UIButton)PlayerSelecting.GetWidget("Button_selected");
            Button_back = (UIButton)PlayerSelecting.GetWidget("Button_back");
            Button_gifts = (UIButton)PlayerSelecting.GetWidget("Button_gifts");

            Image_backGroup = (UIImageView)PlayerSelecting.GetWidget("Image_backGroup");
            Image_platform = (UIImageView)PlayerSelecting.GetWidget("Image_platform");

            //UI Effect
            giftLight = new CCSprite("pause_libao.png", true);
            giftLight.IsVisible = false;
            giftLight.BlendFunc = BlendFunc.Additive;
            var giftLight_action = new CCActionSequence(new CCActionShow(), new ActionSetScale(1.2f), new CCActionSpawn(new CCActionScaleTo(1f, 2), new CCActionFadeOut(1f)), new CCActionHide(), new CCActionDelayTime(1.2f));
            giftLight.RunAction(new CCActionRepeatForever(giftLight_action));
            Button_gifts.AddNode(giftLight);


            selectLight = new CCSprite("anniu_chuji.png", true);
            selectLight.IsVisible = false;
            selectLight.BlendFunc = BlendFunc.Additive;
            var selectLight_action = new CCActionSequence(new CCActionShow(), new ActionSetPosition(0), new ActionSetScale(1), new CCActionSpawn(new CCActionMoveBy(1f, 0, 20), new CCActionScaleTo(1f, 1.2f), new CCActionFadeOut(1f)), new CCActionHide(), new CCActionDelayTime(1.3f));
            selectLight.RunAction(new CCActionRepeatForever(selectLight_action));
            Button_selected.AddNode(selectLight);

            CCParticleSystem smoke1 = new CCParticleSystem(ResID.Particles_lansehuo);
            smoke1.Play();
            smoke1.Postion = new Vector2(-175, -50);
            smoke1.Rotation = 60;
            Image_platform.AddNode(smoke1);

            CCParticleSystem smoke2 = new CCParticleSystem(ResID.Particles_lansehuo);
            smoke2.Play();
            smoke2.Postion = new Vector2(175, -50);
            smoke2.Rotation = 300;
            Image_platform.AddNode(smoke2);

            CCSprite light1 = new CCSprite("effects_light03.png", true);
            light1.Color = new Color32(50, 230, 255);
            light1.Scale = 1;
            light1.BlendFunc = BlendFunc.Additive;
            light1.Postion = new Vector2(-175, -50);
            light1.RunAction(new CCActionRepeatForever(new CCActionSequence(new CCActionFadeIn(1), new CCActionFadeOut(1))));
            Image_platform.AddNode(light1);

            CCSprite light2 = new CCSprite("effects_light03.png", true);
            light2.Color = new Color32(50, 230, 255);
            light2.Scale = 1;
            light2.BlendFunc = BlendFunc.Additive;
            light2.Postion = new Vector2(175, -50);
            light2.RunAction(new CCActionRepeatForever(new CCActionSequence(new CCActionFadeIn(1), new CCActionFadeOut(1))));
            Image_platform.AddNode(light2);

            //Image_platform.RunAction(new CCActionRepeatForever(new CCActionSequence(new CCActionMoveBy(2f, 0, -8), new CCActionMoveBy(2f, 0, 8))));
            //
            UILayout Panel_playerShow = (UILayout)PlayerSelecting.GetWidget("Panel_playerShow");
            UILayout panel_player = (UILayout)PlayerSelecting.GetWidget("Panel_player");
            PageView_showPlayer = (UIPageView)PlayerSelecting.GetWidget("PageView_showPlayer");
            PageView_showPlayer.AddEventListenerPageView(this);

            Vector2 pos = new Vector2(5, Panel_playerShow.Size.height + 30);
            scanline = new CCSprite("effects_scanline.png", true);
            scanline.Postion = pos;
            //scanline.Color = new Color32(224, 186, 18);
            //scanline.BlendFunc = BlendFunc.Additive;
            scanline.Alpha = 150;
            scanline.ScaleX = 6;
            scanline.ScaleY = 2;
            scanline.RunAction(new CCActionRepeatForever(new CCActionSequence(new CCActionMoveBy(5, 0, -pos.Y - 30), new ActionSetPosition(pos))));
            Panel_playerShow.AddNode(scanline);

            AtlasLabel_gold = (UILabelAtlas)(PlayerSelecting.GetWidget("Panel_gold").GetWidget("AtlasLabel_gold"));
            AtlasLabel_score = (UILabelAtlas)(PlayerSelecting.GetWidget("Panel_score").GetWidget("AtlasLabel_score"));

            UIImageView Image_gold = (UIImageView)(PlayerSelecting.GetWidget("Panel_gold").GetWidget("Image_13"));
            goldLight = new CCSprite("zhuanshi.png", true);
            goldLight.IsVisible = false;
            goldLight.BlendFunc = BlendFunc.Additive;
            var action = new CCActionSequence(new CCActionShow(), new ActionSetScale(1), new CCActionSpawn(new CCActionScaleTo(1f, 2), new CCActionFadeOut(1f)), new CCActionHide(), new CCActionDelayTime(1));
            goldLight.RunAction(new CCActionRepeatForever(action));
            Image_gold.AddNode(goldLight);

            Image_levelUp = (UIImageView)Button_levelUp.GetWidget("Image_levelUp");
            Panel_open = (UILayout)Button_levelUp.GetWidget("Panel_open");
            UnloackGolds = (UILabelAtlas)Panel_open.GetWidget("AtlasLabel_golds");
            AtlasLabel_level = (UILabelAtlas)(Panel_playerShow.GetWidget("AtlasLabel_level"));

            pages = new UILayout[4];
            planes = new CCSprite[4];
            for (int i = 0; i < pages.Length; i++)
            {
                pages[i] = new UILayout();
                pages[i].Size = PageView_showPlayer.Size;

                planes[i] = new CCSprite("ui_play0" + (i + 1) + ".png", true);
                planes[i].Postion = new Vector2(pages[i].Size.width / 2, pages[i].Size.height / 2);
                planes[i].Color = lockColor;
                CCSprite lockImg = new CCSprite("suotou.png",true);
                lockImg.Tag = lock_tag;
                lockImg.PostionX = planes[i].ContextSize.width / 2;
                lockImg.PostionY = planes[i].ContextSize.height / 2;
                planes[i].AddChild(lockImg);
                pages[i].AddNode(planes[i]);

                CCAction actionUp = new CCActionMoveTo(1.0f, new Vector2(planes[i].Postion.X, planes[i].Postion.Y + 10));
                CCAction actionDown = new CCActionMoveTo(1.0f, new Vector2(planes[i].Postion.X, planes[i].Postion.Y - 10));
                CCAction floating = new CCActionRepeatForever(new CCActionSequence(actionUp, actionDown));
                planes[i].RunAction(floating);

                PageView_showPlayer.AddPage(pages[i]);
            }
            //设置当前战机
            //
            showingWorldNode.ContextSize = new Size(Panel_playerShow.Size.width, Panel_playerShow.Size.height);
            showingWorldNode.Postion = Panel_playerShow.Postion;
            PlayerSelecting.AddNode(showingWorldNode);
            showingWorldNode.AddDefaultStendcil();

            //
            var action1 = new CCActionSequence(new CCActionScaleTo(0.2f, 1.1f), new CCActionScaleTo(0.2f, 0.9f));
            var action2 = new CCActionSequence(new CCActionScaleTo(0.2f, 1.1f), new CCActionScaleTo(0.2f, 0.9f));

            scaleAction1 = new CCActionRepeatForever(action1);
            scaleAction2 = new CCActionRepeatForever(action2);

            Button_left.RunAction(scaleAction1);
            Button_right.RunAction(scaleAction2);

            Button_addGold.TouchEndedEvent += button =>
                {
                    Console.WriteLine("Button_addGold");
                    Function.GoTo(UIFunction.购买金币);
                };
            Button_wingman.TouchEndedEvent += button =>
                {
                    LevelData leve = GameData.Instance.GetLevelData(3);
                    if (leve.isOpen)
                    {
                        GameData.Instance.PlayerData.withWingman = true;
                        wingmanSelecting.Show(true);
                    }
                    else
                    {
                        this.dialog_lock.Show("第三关开启僚机功能！", DialogWindow.ShowType.OK, null);
                    }
                };
            Button_left.TouchEndedEvent += button =>
                {
                    Console.WriteLine("Button_left");
                    PageView_showPlayer.ScrollToPage(PageView_showPlayer.GetCurPageIndex() - 1);
                    SelectingPlayer();
                };
            Button_right.TouchEndedEvent += button =>
                {
                    Console.WriteLine("Button_right");
                    PageView_showPlayer.ScrollToPage(PageView_showPlayer.GetCurPageIndex() + 1);
                    SelectingPlayer();
                };

            Button_levelUp.TouchEndedEvent += Button_levelUp_TouchEndedEvent;
            Button_selected.TouchEndedEvent += Button_selected_TouchEndedEvent;

            Button_back.TouchEndedEvent += button =>
                {
                    Console.WriteLine("Button_back");
                    GameAudio.PlayEffect(GameAudio.Effect.back);
                    Function.GoTo(UIFunction.主菜单);
                };
            Button_gifts.TouchEndedEvent += button =>
                {
                    Function.GoTo(UIFunction.购买礼包);
                };

            Button_addGold.TouchBeganEvent += Function.PlayButtonEffect;
            Button_wingman.TouchBeganEvent += Function.PlayButtonEffect;
            Button_left.TouchBeganEvent += Function.PlayButtonEffect;
            Button_right.TouchBeganEvent += Function.PlayButtonEffect;
            Button_levelUp.TouchBeganEvent += Function.PlayButtonEffect;
            Button_selected.TouchBeganEvent += Function.PlayButtonEffect;
            Button_gifts.TouchBeganEvent += Function.PlayButtonEffect;

            this.AddWidget(PlayerSelecting);
        }

        private void Button_selected_TouchEndedEvent(UIWidget widget)
        {
            if (curFighterData.isOpen)
            {
                GuideWindow.Instance.Show = false;

                Function.GoTo(UIFunction.关卡选择);
            }
            else
            {
                //:弹出购买战机对话框
                dialog_buy.Show("立刻花费" + curFighterData.unlockGolds + "金币开启战机！", DialogWindow.ShowType.YesNo, event_buy);
            }
        }

        private void Button_levelUp_TouchEndedEvent(UIWidget widget)
        {
            if (curFighterData.isOpen)
            {
                Function.GoTo(UIFunction.强化战机);
            }
            else
            {
                buyFighter();
            }
        }

        private void buyFighter()
        {
            if (playerData.golds >= curFighterData.unlockGolds)
            {
                curFighterData.isOpen = true;
                playerData.golds -= curFighterData.unlockGolds;

                Function.ShowInfo("战机开启成功!");
            }
            else
            {
                Function.GoTo(UIFunction.购买金币);
            }

            this.ResetUIData();
        }

        private void event_buy(DialogWindow.ClickType clickType)
        {
            if (clickType == DialogWindow.ClickType.Yes)
            {
                this.buyFighter();
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        public override void OnEnter()
        {
            base.OnEnter();
            //设置当前选择的战机
            int index = ((int)GameData.Instance.PlayerData.curFighter - (int)ActorID.Player1);
            PageView_showPlayer.ScrollToPage(index);
            curPageIndex = PageView_showPlayer.GetCurPageIndex();

            playerData = GameData.Instance.PlayerData;
            ///////////////////////////////////////////////////////////////////////
            //设置演示框的战机
            PlayerSpawner.Instanse.IsShowing = true;
            WingmanSpawner.Instance.IsShowing = true;
            foreach (var item in PlayerSpawner.Instanse.AllPlayer())
            {
                item.GotoBehavior(Player.Behavior.Null);
                item.RemoveFromParent();
            }
            WingmanSpawner.Instance.RemoveFromWorld();
            //
            BulletEmitter.WorldNode = showingWorldNode;
            PlayerSpawner.Instanse.PlayerLayer = showingWorldNode;
            PlayerSpawner.Instanse.ActivatePlayer();
            //PlayerSpawner.Instanse.CurPlayer.OpenFire();

            stayPoint = new Vector2(showingWorldNode.ContextSize.width / 2, showingWorldNode.ContextSize.height / 3);
            playerInitPoint = new Vector2(showingWorldNode.ContextSize.width / 2, -100);

            foreach (var item in PlayerSpawner.Instanse.AllPlayer())
            {
                showingWorldNode.AddChild(item);
                item.IsVisible = false;
                item.CloseFire();
            }

            WingmanSpawner.Instance.Player = PlayerSpawner.Instanse.CurPlayer;
            PlayerSpawner.Instanse.CurPlayer.StayPoint = stayPoint;
            PlayerSpawner.Instanse.CurPlayer.InitPoint = playerInitPoint;
            PlayerSpawner.Instanse.CurPlayer.Postion = playerInitPoint;

            WingmanSpawner.Instance.AddToWorld(showingWorldNode);

            WingmanSpawner.Instance.Count = GameData.Instance.PlayerData.withWingman ? 1 : 0;
            PlayerSpawner.Instanse.CurPlayer.FlyIn();
            ///////////////////////////////////////////////////////////////////////
            if (GameAudio.CurMusic != GameAudio.Music.menu_bg)
            {
                GameAudio.PlayMusic(GameAudio.Music.menu_bg, true);
            }
            //
            GuideWindow.Instance.Command = GuideCommand.战机选择;
            GuideWindow.Instance.Show = true;
            //
            ResetUIData();
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void OnEnterTransitionFinish()
        {
            base.OnEnterTransitionFinish();
        }

        public override void OnExitTransitionStart()
        {
            base.OnExitTransitionStart();
        }

        protected virtual void SelectingPlayer()
        {
            int pageIndex = PageView_showPlayer.GetCurPageIndex();
            if (pageIndex != curPageIndex)
            {
                PlayerSpawner.PlayerID playerID = (PlayerSpawner.PlayerID)(pageIndex + ActorID.Player1);
                FighterData fighterData = GameData.Instance.GetFighterData(playerID);
                if (fighterData != null)
                {
                    if (!fighterData.isOpen)
                    {
                        //:弹出购买战机对话框
                        dialog_buy.Show("立刻花费" + fighterData.unlockGolds + "金币开启战机！", DialogWindow.ShowType.YesNo, event_buy);
                    }
                    //换演示飞机
                    PlayerSpawner.Instanse.CurPlayer.FlyOut();
                    PlayerSpawner.Instanse.SetCurPlayer(playerID);
                    PlayerSpawner.Instanse.CurPlayer.StayPoint = stayPoint;
                    PlayerSpawner.Instanse.CurPlayer.InitPoint = playerInitPoint;
                    PlayerSpawner.Instanse.CurPlayer.Postion = playerInitPoint;

                    UnloackGolds.Text = Convert.ToString(fighterData.unlockGolds);

                    WingmanSpawner.Instance.Player = PlayerSpawner.Instanse.CurPlayer;

                    //this.showingWorldNode.AddChild(PlayerSpawner.Instanse.CurPlayer);
                    PlayerSpawner.Instanse.CurPlayer.FlyIn();
                    GameData.Instance.PlayerData.curFighter = playerID;
                }

                curPageIndex = pageIndex;
            }

            if (pageIndex == 0)
                Button_left.Enabled = false;
            else
                Button_left.Enabled = true;

            if (pageIndex == PageView_showPlayer.GetChildrenCount() - 1)
                Button_right.Enabled = false;
            else
                Button_right.Enabled = true;

            curFighterData = GameData.Instance.GetFighterData(GameData.Instance.PlayerData.curFighter);
            ResetUIData();
            //Console.WriteLine(curPageIndex);
        }

        public void PageListener(int eventType)
        {
            //Console.WriteLine("eventType:"+eventType);
            SelectingPlayer();
        }

        public void ResetUIData()
        {
            AtlasLabel_gold.SetText(GameData.Instance.PlayerData.golds.ToString());
            AtlasLabel_score.SetText(GameData.Instance.PlayerData.score.ToString());
            AtlasLabel_level.Text = Convert.ToString(curFighterData.curLevel);

            int pageIndex = PageView_showPlayer.GetCurPageIndex();
            if (curFighterData.isOpen)
            {
                Image_levelUp.IsVisible = true;
                Panel_open.IsVisible = false;
                planes[pageIndex].Color = new Color32(255, 255, 255);
                planes[pageIndex].GetChild(lock_tag).IsVisible = false;
            }
            else
            {
                Image_levelUp.IsVisible = false;
                Panel_open.IsVisible = true;
                planes[pageIndex].Color = lockColor;
                planes[pageIndex].GetChild(lock_tag).IsVisible = true;
            }
        }
    }
}
