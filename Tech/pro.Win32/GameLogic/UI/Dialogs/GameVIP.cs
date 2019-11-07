using MatrixEngine.Cocos2d;
using MatrixEngine.CocoStudio.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thunder.GameBilling;
using Thunder.GameLogic.Common;
using Thunder.GameLogic.GameSystem;
using Thunder.GameLogic.Gaming;

namespace Thunder.GameLogic.UI.Dialogs
{
    public class GameVIP : GameWindow
    {
        private UIWidget gameVIP;

        private UIButton Button_close;
        private UIButton Button_buy;
        private UIImageView Image_vip;

        public GameVIP()
        {
            UIWidget main = UIReader.GetWidget(ResID.UI_UI_GameVIP);
            gameVIP = main.GetWidget("Panel_vip");


            Button_close = (UIButton)gameVIP.GetWidget("Button_close");
            Button_buy = (UIButton)gameVIP.GetWidget("Button_buy");
            Image_vip = (UIImageView)gameVIP.GetWidget("Image_vip");


            Button_close.TouchBeganEvent += button =>
            {
                if (GameData.Instance.GameState == GameState.Playing)
                {
                    if(PlayingScene.Instance.IsPause)
                        PlayingScene.Instance.IsPause = false;
                }
                                     
                this.Show(false);
            };
            Button_buy.TouchBeganEvent += button =>
            {
                //购买
                this.Show(false);
                Function.GoTo(UIFunction.购买金币);
            };

            Button_close.TouchBeganEvent += Function.PlayBackButtonEffect;
            Button_buy.TouchBeganEvent += Function.PlayButtonEffect;

            this.AddChildCenter(main);
        }

        public override void init()
        {
            gameVIP.IsVisible = true;

            if (GameData.Instance.IsVip)
            {
                Button_buy.Enabled = false;
                Image_vip.IsVisible = true;
            }
            else
            {
                Button_buy.Enabled = true;
                Image_vip.IsVisible = false;
            }
        }


        public override void Show(bool b)
        {
            base.Show(b);
            if (b)
            {
                if (GameData.Instance.GameState == GameState.Playing)
                    PlayingScene.Instance.IsPause = true;
            }
            else
            {
//                 if (GameData.Instance.GameState == GameState.Playing)
//                     PlayingScene.Instance.IsPause = false;

                MainMenuUIlayer.Instance.ResetUIData();
            }
        }
    }
}
