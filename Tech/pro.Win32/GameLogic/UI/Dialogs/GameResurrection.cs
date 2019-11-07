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
    public class GameResurrection : GameWindow, BillingWindow
    {
        private bool isInit;

        private UIWidget UI_GameResurrection;
        private UIButton Button_close;
        private UIButton Button_getAll;

        private UILabelAtlas AtlasLabel_shield;
        private UILabelAtlas AtlasLabel_skill;

        private readonly int shieldsNum = 2;
        private readonly int skillsNum = 1;

        public GameResurrection()
        {

        }

        public override void init()
        {
            if (!this.isInit)
            {
                base.init();
                UI_GameResurrection = UIReader.GetWidget(ResID.UI_UI_GameResurrection).GetWidget("Panel_resurrection");
                Button_close = (UIButton)UI_GameResurrection.GetWidget("Button_close");
                Button_getAll = (UIButton)UI_GameResurrection.GetWidget("Button_getAll");

                AtlasLabel_shield = (UILabelAtlas)(UI_GameResurrection.GetWidget("Panel_10").GetWidget("AtlasLabel_shield"));
                AtlasLabel_skill = (UILabelAtlas)(UI_GameResurrection.GetWidget("Panel_10").GetWidget("AtlasLabel_skill"));

                AtlasLabel_shield.Text = Convert.ToString(shieldsNum);
                AtlasLabel_skill.Text = Convert.ToString(skillsNum);

                Button_close.TouchBeganEvent += button =>
                    {
                        this.UnResurrect();
                    };
                Button_getAll.TouchBeganEvent += button =>
                    {
                        this.Show(false);
                        //弹出计费
                        BillingHelper.DoBilling(BillingID.ID10_BuyResurrection,this);
                    };

                Button_close.TouchBeganEvent += Function.PlayButtonEffect;
                Button_getAll.TouchBeganEvent += Function.PlayButtonEffect;

                this.AddChildCenter(UI_GameResurrection);
                this.isInit = true;
            }
        }

        public override void Show(bool b)
        {
            base.Show(b);
//             if (b)
//             {
//                 PlayingScene.Instance.IsPause = true;
//             }
//             else
//             {
//                 PlayingScene.Instance.IsPause = false;
//             }
        }

        private void UnResurrect()
        {
            this.Show(false);
            MainMenuScene.Instance.shouldShowRichGift = true;
            Function.GoTo(UIFunction.主菜单, "loading");
        }

        public void OnBillingSuccess(BillingID id)
        {
            PlayingScene.Instance.GameLayer.ResurrectPlayer();
        }

        public void OnBillingFail(BillingID id)
        {
            this.UnResurrect();
        }
    }

}
