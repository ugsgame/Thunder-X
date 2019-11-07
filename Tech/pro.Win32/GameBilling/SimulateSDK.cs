using MatrixEngine.CocoStudio.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thunder.GameLogic.Common;
using Thunder.GameLogic.GameSystem;

namespace Thunder.GameBilling
{
    /// <summary>
    /// 模拟计费sdk
    /// </summary>
    public class SimulateSDK : GameWindow
    {
        private string simID;

        private UIWidget main_panel;

        private UIButton Button_sure;
        private UIButton Button_cancle;
        private UILabel Label_billingText;

        public static SimulateSDK instance;
        public SimulateSDK()
        {

            UIWidget main = UIReader.GetWidget(ResID.UI_UI_SimulateSDK);
            main_panel = main.GetWidget("Panel_main");
            Button_sure = (UIButton)main_panel.GetWidget("Button_sure");
            Button_cancle = (UIButton)main_panel.GetWidget("Button_cancle");
            Label_billingText = (UILabel)main_panel.GetWidget("Label_billingText");

            Button_sure.TouchBeganEvent += button =>
            {
                this.OnBillingSuccess(simID);
                this.Show(false);
            };

            Button_cancle.TouchBeganEvent += button =>
            {
                this.OnBillingFail(simID);
                this.Show(false);
            };

            this.AddChildCenter(main);
            instance = this;
        }

        public void DoBilling(string id,float money, string billingName)
        {
            simID = id;
            BillingID billingID = (BillingID)Convert.ToInt32(id);
            Label_billingText.Text = "你将消费人民币" + money + "元，" + "购买[" + billingName + "]";
            this.Show(true);
        }

        public virtual void OnBillingSuccess(string id)
        {
            BillingHelper.OnNativeBilling(true, id);
        }

        public virtual void OnBillingFail(string id)
        {
            BillingHelper.OnNativeBilling(false, id);
        }
    }
}
