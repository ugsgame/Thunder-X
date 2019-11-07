
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MatrixEngine.CocoStudio.GUI;
using Thunder.GameLogic.GameSystem;
using Thunder.GameLogic.Common;
using Thunder.GameBilling;

namespace Thunder.GameLogic.Gaming
{
    public class GamePause : GameWindow, BillingWindow
    {
        private bool isInit;

        private UIWidget gamePause;

        private UILayout Panel_addSkill;
        private UILayout Panel_addShield;
        private UILayout Panel_addSuperGifts;

        private UIButton Button_resume;
        private UIButton Button_mainMenu;
        private UIButton Button_gifts;

        private UILabelAtlas AtlasLabel_giftGold;
        private UILabelAtlas AtlasLabel_giftShield;
        private UILabelAtlas AtlasLabel_giftSkill;

        private UILabelAtlas AtlasLabel_Shield;
        private UILabelAtlas AtlasLabel_Skill;

        public static GamePause Instance; 

        public GamePause()
        {
            Instance = this;
        }

        public override void init()
        {
            if (!this.isInit)
            {
                base.init();
                gamePause = UIReader.GetWidget(ResID.UI_UI_GamePause);

                Panel_addShield = (UILayout)gamePause.GetWidget("Panel_addShield");
                Panel_addSkill = (UILayout)gamePause.GetWidget("Panel_addSkill");
                Panel_addSuperGifts = (UILayout)gamePause.GetWidget("Panel_addSuperGifts");

                Button_resume = (UIButton)gamePause.GetWidget("Button_resume");
                Button_mainMenu = (UIButton)gamePause.GetWidget("Button_mainMenu");
                Button_gifts = (UIButton)gamePause.GetWidget("Button_gifts");

                AtlasLabel_giftGold = (UILabelAtlas)Panel_addSuperGifts.GetWidget("AtlasLabel_35");
                AtlasLabel_giftShield = (UILabelAtlas)Panel_addSuperGifts.GetWidget("AtlasLabel_shield");
                AtlasLabel_giftSkill = (UILabelAtlas)Panel_addSuperGifts.GetWidget("AtlasLabel_skill");

                AtlasLabel_Shield = (UILabelAtlas)Panel_addShield.GetWidget("AtlasLabel_39");
                AtlasLabel_Skill = (UILabelAtlas)Panel_addSkill.GetWidget("AtlasLabel_skill");

                AtlasLabel_giftGold.Text = Convert.ToString(BillingHelper.ID5_BuySuperGift_goldNum);
                AtlasLabel_giftSkill.Text = Convert.ToString(BillingHelper.ID5_BuySuperGift_skillNum);
                AtlasLabel_giftShield.Text = Convert.ToString(BillingHelper.ID5_BuySuperGift_shieldNum);

                AtlasLabel_Shield.Text = Convert.ToString(BillingHelper.ID4_BuyShields_shieldNum);
                AtlasLabel_Skill.Text = Convert.ToString(BillingHelper.ID3_BuySkills_skillNum);

                Button_resume.TouchEndedEvent += button =>
                    {
                        this.Show(false);
                        PlayingScene.Instance.IsPause = false;
                    };
                Button_mainMenu.TouchEndedEvent += button =>
                    {
                        Function.GoTo(UIFunction.主菜单, "loading");
                        this.Show(false);
                    };
                Button_gifts.TouchEndedEvent += button =>
                    {
                        Function.GoTo(UIFunction.购买礼包);
                    };
                Panel_addShield.TouchEndedEvent += button =>
                    {
                        BillingHelper.DoBilling(BillingID.ID4_BuyShields,this);
                    };
                Panel_addSkill.TouchEndedEvent += button =>
                    {
                        BillingHelper.DoBilling(BillingID.ID3_BuySkills, this);
                    };
                Panel_addSuperGifts.TouchBeganEvent += button =>
                    {
                        BillingHelper.DoBilling(BillingID.ID5_BuySuperGift,this);
                    };

                this.AddChildCenter(gamePause);
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
        }



        public void OnBillingSuccess(BillingID id)
        {
            BillingHelper.OnBllingDeal(id);
            Function.ShowInfo("购买成功!");
        }

        public void OnBillingFail(BillingID id)
        {
            
        }
    }
}
