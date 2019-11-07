using MatrixEngine.Cocos2d;
using MatrixEngine.CocoStudio.Armature;
using MatrixEngine.CocoStudio.GUI;
using MatrixEngine.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thunder.Common;
using Thunder.GameLogic.Common;
using Thunder.GameLogic.GameSystem;
using Thunder.GameLogic.UI.Effects;
using Thunder.GameLogic.UI.Guide;

namespace Thunder.GameLogic.UI
{
    public class MainMenuUIlayer : UILayer,IUpdateUI
    {
        UIWidget mainMenu;

        UIButton Button_begin;
        UIButton Button_endless;
        UIButton Button_shop;
        UIButton Button_ranking;
        UIButton Button_setting;
        UIButton Button_vip;
        UIButton Button_about;
        UIButton Button_supprise;

        UIImageView Image_soundOn;
        UIImageView Image_soundOff;
        UIImageView Image_vipOn;
        UIImageView Image_vipOff;

        UIImageView Image_endlesslock;
        UIImageView Image_shopLock;
        UIImageView Image_rankingLock;

        CCArmature titleAnim;
        CCSprite titleImg;

        CCSprite miniTitle;

        public static MainMenuUIlayer Instance;

        public MainMenuUIlayer()
        {
            mainMenu = UIReader.GetWidget(ResID.UI_UI_MainMenu);

            Button_begin = (UIButton)mainMenu.GetWidget("Button_begin");
            Button_endless = (UIButton)mainMenu.GetWidget("Button_endless");
            Button_shop = (UIButton)mainMenu.GetWidget("Button_shop");
            Button_ranking = (UIButton)mainMenu.GetWidget("Button_ranking");
            Button_setting = (UIButton)mainMenu.GetWidget("Button_setting");
            Button_vip = (UIButton)mainMenu.GetWidget("Button_vip");
            Button_about = (UIButton)mainMenu.GetWidget("Button_about");
            Button_supprise = (UIButton)mainMenu.GetWidget("Button_supprise");

            Image_soundOn = (UIImageView)Button_setting.GetWidget("Image_on");
            Image_soundOff = (UIImageView)Button_setting.GetWidget("Image_off");

            Image_vipOn = (UIImageView)Button_vip.GetWidget("Image_on");
            Image_vipOff = (UIImageView)Button_vip.GetWidget("Image_off");

            Image_endlesslock = (UIImageView)Button_endless.GetWidget("Image_lock");
            Image_shopLock = (UIImageView)Button_shop.GetWidget("Image_lock");
            Image_rankingLock = (UIImageView)Button_ranking.GetWidget("Image_lock");

            VIPButtionEffect vipEffect = new VIPButtionEffect();
            vipEffect.Postion = Button_vip.Postion;
            vipEffect.ZOrder = 0;
            mainMenu.AddNode(vipEffect);

            // title effect
            titleAnim = new CCArmature("UIArmature");
            MatrixEngine.CocoStudio.Armature.CCAnimation anim = titleAnim.GetAnimation();
            anim.Play("logo",true);
            titleAnim.PostionX = Config.SCREEN_PosX_Center;
            titleAnim.PostionY = Config.SCREEN_HEIGHT - 180;

            titleImg = new CCSprite("title_1.png", true);
            titleImg.Postion = titleAnim.Postion;

            TileEffect titleEffect = new TileEffect();
            titleEffect.Postion = new Vector2(titleImg.ContextSize.width/2, titleImg.ContextSize.height / 2);
            titleEffect.Play(true);
            titleImg.AddChild(titleEffect);

            TileLight light = new TileLight();
            light.Postion = new Vector2(100,50);
            light.Play(true);
            titleImg.AddChild(light);

            miniTitle = new CCSprite("title_1_2.png",true);
            miniTitle.Postion = titleAnim.Postion;
            miniTitle.PostionX = miniTitle.PostionX + 70;
            miniTitle.PostionY = miniTitle.PostionY + 20;
            MiniTileEffect miniTileEffect = new MiniTileEffect();
            miniTileEffect.Postion = 0;
            miniTileEffect.Play(true);
            miniTitle.AddChild(miniTileEffect);

            mainMenu.AddNode(titleAnim);
            mainMenu.AddNode(titleImg);
            mainMenu.AddNode(miniTitle);
            //
            UIImageView Image_backGroup = (UIImageView)mainMenu.GetWidget("Image_backGroup");
            Image_backGroup.ScaleY = 1/Config.SCREEN_RATE.X;
            //
            SuppriseEffect effect = new SuppriseEffect();
            Button_supprise.AddNode(effect);
            //

            Button_begin.TouchEndedEvent += button =>
            {
                Console.WriteLine("Button_begin");

                GuideWindow.Instance.Show = false;

                GameData.Instance.GameMode = GameMode.Normal;
                Function.GoTo(UIFunction.战机选择);
            };
            Button_endless.TouchEndedEvent += button =>
            {
                Console.WriteLine("Button_endless");
                if (GameData.Instance.IsEndlessOpen)
                {
                    GameData.Instance.GameMode = GameMode.Endless;
                }
                else
                {
                    InfoShow.AddInfo("暂未开启");
                }
            };
            Button_shop.TouchEndedEvent += button =>
            {
                Console.WriteLine("Button_shop");
                if (GameData.Instance.IsShopOpen)
                {
                    Function.GoTo(UIFunction.游戏商店);
                }
            };
            Button_ranking.TouchEndedEvent += button =>
            {
                Console.WriteLine("Button_ranking");
                if (GameData.Instance.IsRankListOpen)
                {
                    Function.GoTo(UIFunction.排行榜);
                }
                else
                {
                    InfoShow.AddInfo("暂未开启");
                }
            };
            Button_setting.TouchEndedEvent += button =>
            {
                Console.WriteLine("Button_setting");
                GameData.Instance.IsSoundOpen = !GameData.Instance.IsSoundOpen;
                GameAudio.IsEnable = GameData.Instance.IsSoundOpen;
                this.ResetUIData();
            };
            Button_vip.TouchEndedEvent += button =>
            {
                Console.WriteLine("Button_vip");
                Function.GoTo(UIFunction.游戏VIP);
            };
            Button_about.TouchEndedEvent += button =>
            {
                Console.WriteLine("Button_about");
                Function.GoTo(UIFunction.游戏关于);
            };
            Button_supprise.TouchBeganEvent += button =>
            {
                Console.WriteLine("Button_supprise");
                Function.GoTo(UIFunction.购买惊喜礼包);
            };

            Button_begin.TouchBeganEvent += Function.PlayButtonEffect;
            Button_endless.TouchBeganEvent += Function.PlayButtonEffect;
            Button_shop.TouchBeganEvent += Function.PlayButtonEffect;
            Button_ranking.TouchBeganEvent += Function.PlayButtonEffect;
            Button_setting.TouchBeganEvent += Function.PlayButtonEffect;
            Button_vip.TouchBeganEvent += Function.PlayButtonEffect;
            Button_about.TouchBeganEvent += Function.PlayButtonEffect;
            Button_supprise.TouchBeganEvent += Function.PlayBackButtonEffect;

            this.AddWidget(mainMenu);

            Instance = this;
        }

        protected override void Dispose(bool disposing)
        {

            base.Dispose(disposing);
        }

        public override void OnEnter()
        {
            base.OnEnter();

            this.ResetUIData();

            Image_endlesslock.IsVisible = !GameData.Instance.IsEndlessOpen;
            Image_shopLock.IsVisible = !GameData.Instance.IsShopOpen;
            Image_rankingLock.IsVisible = !GameData.Instance.IsRankListOpen;

            if (GameAudio.CurMusic != GameAudio.Music.menu_bg)
            {
                GameAudio.PlayMusic(GameAudio.Music.menu_bg, true);
            }

            //
            GuideWindow.Instance.Command = GuideCommand.开始游戏;
            GuideWindow.Instance.Show = true;
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

        public void ResetUIData()
        {
            Image_soundOn.IsVisible = GameData.Instance.IsSoundOpen;
            Image_soundOff.IsVisible = !GameData.Instance.IsSoundOpen;

            Image_vipOn.IsVisible = GameData.Instance.IsVip;
            Image_vipOff.IsVisible = !GameData.Instance.IsVip;

            Button_supprise.Enabled = !GameData.Instance.IsGetSuppriseGift;
        }
    }
}
