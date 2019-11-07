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
    public class BuyGolds : GameWindow, BillingWindow
    {
        private bool isInit;
        private UIWidget supplyGolds;
        private UIButton Button_close;
        private UIButton Button_buy;
        private UILabelAtlas AtlasLabel_num;


        public BuyGolds()
        {

        }

        public override void init()
        {
            if (!this.isInit)
            {
                base.init();

                UIWidget main = UIReader.GetWidget(ResID.UI_UI_SupplyGolds);
                supplyGolds = main.GetWidget("Panel_main");
                Button_close = (UIButton)supplyGolds.GetWidget("Button_close");
                Button_buy = (UIButton)supplyGolds.GetWidget("Button_buy");
                AtlasLabel_num = (UILabelAtlas)supplyGolds.GetWidget("AtlasLabel_num");

                AtlasLabel_num.Text = Convert.ToString(BillingHelper.ID11_BuyGolds30W_goldNum);

                Button_close.TouchBeganEvent += button =>
                {
                    this.Show(false);
                };
                Button_buy.TouchBeganEvent += button =>
                {
                    //呼叫快速购买
                    BillingHelper.DoBilling(BillingID.ID11_BuyGolds30W,this);
                };

                Button_close.TouchBeganEvent += Function.PlayBackButtonEffect;
                Button_close.TouchBeganEvent += Function.PlayButtonEffect;
                this.AddChildCenter(main);

                this.isInit = true;
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
                if (GameData.Instance.GameState == GameState.Playing)
                    PlayingScene.Instance.IsPause = false;
            }
        }

        public void OnBillingSuccess(BillingID id)
        {
            this.Show(false);
            BillingHelper.OnBllingDeal(id);
            Function.gameTotalUI.palyerSelecting.playerSelecting.ResetUIData();
            Function.gameTotalUI.playerLevelUp.playerLevelUp.ResetUIData();

            Function.ShowInfo("购买金币成功!");
        }

        public void OnBillingFail(BillingID id)
        {
            
        }
    }
}
