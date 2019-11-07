using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Thunder.GameBilling;
using Thunder.GameLogic.GameSystem;
using MatrixEngine.CocoStudio.GUI;
using Thunder.GameLogic.Common;
using Thunder.GameLogic.Gaming;
using Thunder.GameLogic.UI.Effects;

namespace Thunder.GameLogic.UI.Dialogs
{
    public class BuySuppriseGift : GameWindow, BillingWindow
    {

        private UIButton Button_close;
        private UIButton Button_buy;

        public BuySuppriseGift()
        {

            UIWidget main = UIReader.GetWidget(ResID.UI_UI_SuppriseGift);
            UIWidget Panel_main = main.GetWidget("Panel_main");

            Button_close = (UIButton)Panel_main.GetWidget("Button_close");
            Button_buy = (UIButton)Panel_main.GetWidget("Button_buy");

            BuyButtonEffect effect = new BuyButtonEffect();
            Button_buy.AddNode(effect);

            Button_close.TouchBeganEvent += button =>
            {
                this.Show(false);
            };
            Button_buy.TouchBeganEvent += button =>
            {
                BillingHelper.DoBilling(BillingID.ID12_SurpriseGift, this);
            };

            Button_close.TouchBeganEvent += Function.PlayBackButtonEffect;
            Button_buy.TouchBeganEvent += Function.PlayButtonEffect;

            this.AddChildCenter(main);
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
                if (GameData.Instance.GameState == GameState.Playing)
                    PlayingScene.Instance.IsPause = false;
            }
        }

        public void OnBillingSuccess(BillingID id)
        {
            BillingHelper.OnBllingDeal(id);
            GameData.Instance.IsGetSuppriseGift = true;
            InfoShow.AddInfo("购买惊喜礼包成功!");

            MainMenuUIlayer.Instance.ResetUIData();
            this.Show(false);
        }

        public void OnBillingFail(BillingID id)
        {
            InfoShow.AddInfo("购买惊喜礼包失败!");
        }
    }
}
