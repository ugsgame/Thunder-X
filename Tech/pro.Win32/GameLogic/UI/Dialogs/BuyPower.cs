using MatrixEngine.CocoStudio.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thunder.GameBilling;
using Thunder.GameLogic.Common;
using Thunder.GameLogic.GameSystem;

namespace Thunder.GameLogic.UI.Dialogs
{
    public class BuyPower:GameWindow
    {
        private bool isInit;

        private UIWidget supplyPower;
        private UIWidget Button_close;

        private UIButton Button_buy1;
        private UIButton Button_buy2;

        private UILabelAtlas AtlasLabel_power1;
        private UILabelAtlas AtlasLabel_power2;
        private UILabelAtlas AtlasLabel_golds1;
        private UILabelAtlas AtlasLabel_golds2;

        private readonly int power1Num = 1;
        private readonly int power2Num = 10;
        private readonly int power1Golds = 3000;
        private readonly int power2Golds = 30000;

        public BuyPower()
        {

        }

        public override void init()
        {
            if (!this.isInit)
            {
                base.init();

                UIWidget main = UIReader.GetWidget(ResID.UI_UI_SupplyPower);
                supplyPower = main.GetWidget("Panel_main");

                Button_buy1 = (UIButton)(supplyPower.GetWidget("Panel_buy1").GetWidget("Button_buy"));
                Button_buy2 = (UIButton)(supplyPower.GetWidget("Panel_buy2").GetWidget("Button_buy"));
                Button_close = (UIButton)supplyPower.GetWidget("Button_close");
                AtlasLabel_power1 = (UILabelAtlas)(supplyPower.GetWidget("Panel_buy1").GetWidget("AtlasLabel_power"));
                AtlasLabel_power2 = (UILabelAtlas)(supplyPower.GetWidget("Panel_buy2").GetWidget("AtlasLabel_power"));
                AtlasLabel_golds1 = (UILabelAtlas)(Button_buy1.GetWidget("AtlasLabel_golds"));
                AtlasLabel_golds2 = (UILabelAtlas)(Button_buy2.GetWidget("AtlasLabel_golds"));

                AtlasLabel_power1.Text = Convert.ToString(power1Num);
                AtlasLabel_power2.Text = Convert.ToString(power2Num);
                AtlasLabel_golds1.Text = Convert.ToString(power1Golds);
                AtlasLabel_golds2.Text = Convert.ToString(power2Golds);

                Button_buy1.TouchBeganEvent += button =>
                    {
                        if (GameData.Instance.PlayerData.golds > power1Golds)
                        {
                            GameData.Instance.PlayerData.golds -= power1Golds;
                            GameData.Instance.PlayerData.power += power1Num;
                            Function.gameTotalUI.levelSelecting.levelSelecting.ResetUIData();
                            this.Show(false);
                        }
                        else
                        {
                            //弹出购买
                            Function.GoTo(UIFunction.购买金币);
                        }
                    };
                Button_buy2.TouchBeganEvent += button =>
                    {
                        if (GameData.Instance.PlayerData.golds > power2Golds)
                        {
                            GameData.Instance.PlayerData.golds -= power2Golds;
                            GameData.Instance.PlayerData.power += power2Num;
                            Function.gameTotalUI.levelSelecting.levelSelecting.ResetUIData();
                            this.Show(false);
                        }
                        else
                        {
                            //弹出购买
                            Function.GoTo(UIFunction.购买金币);
                        }
                    };
                Button_close.TouchBeganEvent += button =>
                    {
                        this.Show(false);
                    };

                Button_buy1.TouchBeganEvent += Function.PlayButtonEffect;
                Button_buy2.TouchBeganEvent += Function.PlayButtonEffect;
                Button_close.TouchBeganEvent += Function.PlayBackButtonEffect;

                this.AddChildCenter(main);

                this.isInit = true;
            }

        }
    }
}
