using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thunder.GameBilling;
using Thunder.GameLogic.GameSystem;

using MatrixEngine.CocoStudio.GUI;
using Thunder.GameLogic.Common;
using Thunder.GameLogic.UI.Effects;

namespace Thunder.GameLogic.UI.Dialogs
{
    public class BuyRichGift : GameWindow, BillingWindow
    {

        private UIButton Button_close;
        private UIButton Button_buy;
        private UILabel Label_text;

        private int goldsNum = 88888;
        private int skillNum = 10;
        private int shieldNum = 15;
        private int powerNum = 10;

        private int money = 29;

        public BuyRichGift()
        {
            UIWidget main = UIReader.GetWidget(ResID.UI_UI_RichGift);
            UIWidget Panel_main = main.GetWidget("Panel_main");

            Button_close = (UIButton)Panel_main.GetWidget("Button_close");
            Button_buy = (UIButton)Panel_main.GetWidget("Button_buy");
            Label_text = (UILabel)Panel_main.GetWidget("Label_text");

            BuyButtonEffect effect = new BuyButtonEffect();
            Button_buy.AddNode(effect);

            Label_text.Text = "礼包总价值78元，现在仅需" + BillingHelper.CoverToBillingMoney(BillingID.ID7_BuyRichGift)+"元";

            Button_close.TouchBeganEvent += button =>
                {
                    this.Show(false);
                };
            Button_buy.TouchBeganEvent += button =>
            {
                //呼叫计费
                BillingHelper.DoBilling(BillingID.ID7_BuyRichGift, this);
            };

            this.AddChildCenter(main);
        }

        public override void init()
        {
            base.init();
        }

        public void OnBillingSuccess(BillingID id)
        {
            this.Show(false);

            BillingHelper.OnBllingDeal(id);

            Function.gameTotalUI.palyerSelecting.playerSelecting.ResetUIData();
            Function.gameTotalUI.playerLevelUp.playerLevelUp.ResetUIData();
            Function.gameTotalUI.levelSelecting.levelSelecting.ResetUIData();
        }

        public void OnBillingFail(BillingID id)
        {
       
        }
    }
}
