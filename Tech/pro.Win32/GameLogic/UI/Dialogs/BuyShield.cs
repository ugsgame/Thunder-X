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
    public class BuyShield:GameWindow,BillingWindow
    {
        private bool isInit;
        private UIWidget supplyShield;
        private UIButton Button_close;
        private UIButton Button_buy;
        private UILabelAtlas AtlasLabel_num;

        private readonly int shieldNum = 5;

        public BuyShield()
        {

        }

        public override void init()
        {
            if (!this.isInit)
            {
                base.init();

                UIWidget main = UIReader.GetWidget(ResID.UI_UI_SupplyShield);
                supplyShield = main.GetWidget("Panel_main");
                Button_close = (UIButton)supplyShield.GetWidget("Button_close");
                Button_buy = (UIButton)supplyShield.GetWidget("Button_buy");
                AtlasLabel_num = (UILabelAtlas)supplyShield.GetWidget("AtlasLabel_num");

                AtlasLabel_num.Text = Convert.ToString(BillingHelper.ID4_BuyShields_shieldNum);

                Button_close.TouchBeganEvent += button =>
                {
                    this.Show(false);
                };
                Button_buy.TouchBeganEvent += button =>
                {
                    //呼叫计费
                    BillingHelper.DoBilling(BillingID.ID4_BuyShields,this);
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
                PlayingScene.Instance.IsPause = true;
            }
            else
            {
                PlayingScene.Instance.IsPause = false;
            }
        }

        public void OnBillingSuccess(BillingID id)
        {

            BillingHelper.OnBllingDeal(id);

            PlayingScene.Instance.GameLayer.Player.WillUseShield = true;
            this.Show(false);
        }


        public void OnBillingFail(BillingID id)
        {
            this.Show(false);
        }
    }
}
