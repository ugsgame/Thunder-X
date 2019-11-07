using MatrixEngine.CocoStudio.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thunder.GameBilling;
using Thunder.GameLogic.Common;
using Thunder.GameLogic.GameSystem;
using Thunder.GameLogic.UI.Dialogs;

namespace Thunder.GameLogic.UI
{
    public class GameStore:GameWindow,BillingWindow
    {
        private bool isInit;
        private UIWidget gameStore;

        private UIButton Button_buy1;
        private UIButton Button_buy2;
        private UIButton Button_buy3;
        private UIButton Button_buy4;
        private UIButton Button_buyGift;
        private UIButton Button_close;


        public GameStore()
        {

        }

        public override void init()
        {
            if (!this.isInit)
            {
                base.init();

                UIWidget main = UIReader.GetWidget(ResID.UI_UI_GameStore);
                gameStore = main.GetWidget("Panel_store");

                Button_buy1 = (UIButton)(gameStore.GetWidget("Panel_buy1").GetWidget("Button_buy"));
                Button_buy2 = (UIButton)(gameStore.GetWidget("Panel_buy2").GetWidget("Button_buy"));
                Button_buy3 = (UIButton)(gameStore.GetWidget("Panel_buy3").GetWidget("Button_buy"));
                Button_buy4 = (UIButton)(gameStore.GetWidget("Panel_buy4").GetWidget("Button_buy"));
                Button_buyGift = (UIButton)(gameStore.GetWidget("Panel_gifts").GetWidget("Button_buy"));
                Button_close = (UIButton)gameStore.GetWidget("Button_close");

                UILabelAtlas AtlasLabel_golds1 = (UILabelAtlas)(gameStore.GetWidget("Panel_buy1").GetWidget("AtlasLabel_golds"));
                AtlasLabel_golds1.Text = Convert.ToString(BillingHelper.ID1_BuyGolds1W_goldNum);

                UILabelAtlas AtlasLabel_golds2 = (UILabelAtlas)(gameStore.GetWidget("Panel_buy2").GetWidget("AtlasLabel_golds"));
                AtlasLabel_golds2.Text = Convert.ToString(BillingHelper.ID2_BuyGolds3W_goldNnum);

                UILabelAtlas AtlasLabel_shield1 = (UILabelAtlas)(gameStore.GetWidget("Panel_buy3").GetWidget("AtlasLabel_golds"));
                AtlasLabel_shield1.Text = Convert.ToString(BillingHelper.ID4_BuyShields_shieldNum);

                UILabelAtlas AtlasLabel_skill1 = (UILabelAtlas)(gameStore.GetWidget("Panel_buy4").GetWidget("AtlasLabel_golds"));
                AtlasLabel_skill1.Text = Convert.ToString(BillingHelper.ID3_BuySkills_skillNum);

                UILabelAtlas AtlasLabel_shield2 = (UILabelAtlas)(gameStore.GetWidget("Panel_gifts").GetWidget("AtlasLabel_87"));
                AtlasLabel_shield2.Text = Convert.ToString(BillingHelper.ID5_BuySuperGift_shieldNum);

                UILabelAtlas AtlasLabel_skill2 = (UILabelAtlas)(gameStore.GetWidget("Panel_gifts").GetWidget("AtlasLabel_88"));
                AtlasLabel_skill2.Text = Convert.ToString(BillingHelper.ID5_BuySuperGift_skillNum);

                UILabelAtlas AtlasLabel_golds3 = (UILabelAtlas)(gameStore.GetWidget("Panel_gifts").GetWidget("AtlasLabel_89"));
                AtlasLabel_golds3.Text = Convert.ToString(BillingHelper.ID5_BuySuperGift_goldNum);

                Button_buy1.TouchBeganEvent += button =>
                {
                    BillingHelper.DoBilling(BillingID.ID1_BuyGolds1W,this);
                };
                Button_buy2.TouchBeganEvent += button =>
                {
                    BillingHelper.DoBilling(BillingID.ID2_BuyGolds3W, this);
                };
                Button_buy3.TouchBeganEvent += button =>
                {
                    BillingHelper.DoBilling(BillingID.ID4_BuyShields, this);
                };
                Button_buy4.TouchBeganEvent += button =>
                {
                    BillingHelper.DoBilling(BillingID.ID3_BuySkills, this);
                };
                Button_buyGift.TouchBeganEvent += button =>
                {
                    BillingHelper.DoBilling(BillingID.ID5_BuySuperGift,this);
                };
                Button_close.TouchBeganEvent += button =>
                {
                    this.Show(false);
                };

                Button_buy1.TouchBeganEvent += Function.PlayButtonEffect;
                Button_buy2.TouchBeganEvent += Function.PlayButtonEffect;
                Button_buy3.TouchBeganEvent += Function.PlayButtonEffect;
                Button_buy4.TouchBeganEvent += Function.PlayButtonEffect;
                Button_buyGift.TouchBeganEvent += Function.PlayButtonEffect;
                Button_close.TouchBeganEvent += Function.PlayButtonEffect;

                this.AddChildCenter(main);
                this.isInit = true;
            }
        }

        public void OnBillingSuccess(BillingID id)
        {
            BillingHelper.OnBllingDeal(id);
            Function.ShowInfo("购买成功!");
        }


        public void OnBillingFail(BillingID id)
        {
            //do somethings
        }
    }
}
