using MatrixEngine.Cocos2d;
using MatrixEngine.CocoStudio.GUI;
using MatrixEngine.Engine;
using MatrixEngine.Math;
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
    public class GameLottery : GameWindow, BillingWindow
    {
        enum State
        {
            Lottering,
            LotteryOver,
        }

        public struct GoldInfo
        {
            public string image;
            public int golds;
        }

        UIWidget Panel_lottery;

        UIButton Button_getAll;
        UIButton Button_close;

        UILabel Label_getAllTips;
        UILabel Label_lotteryTips;

        UILayout[] Panel_lotterys;
        GoldInfo[] goldsInfos;

        List<UILayout> UnlotteryPanels = new List<UILayout>();
        List<GoldInfo> UnlooteryInfoes = new List<GoldInfo>();

        private int getGolds;
        private int allGolds;
        private int valueMoney = 30;
        private int needMoney = 20;

        LotteryResult lotteryResult;
        GameWin gameWin;
        public GameLottery(GameWin _gameWin)
        {

            this.lotteryResult = new LotteryResult(this);
            this.gameWin = _gameWin;

            UIWidget main = UIReader.GetWidget(ResID.UI_UI_CrossLottery);
            Panel_lottery = (UIWidget)main.GetWidget("Panel_lottery");

            Button_getAll = (UIButton)Panel_lottery.GetWidget("Button_getAll");
            Button_close = (UIButton)Panel_lottery.GetWidget("Button_close");

            Label_getAllTips = (UILabel)Panel_lottery.GetWidget("Label_getAllTips");
            Label_lotteryTips = (UILabel)Panel_lottery.GetWidget("Label_lotteryTips");

            UILayout Panel_lotteryPanels = (UILayout)Panel_lottery.GetWidget("Panel_lotteryPanels");
            Panel_lotterys = new UILayout[4];
            Panel_lotterys[0] = (UILayout)Panel_lotteryPanels.GetWidget("Panel_lottery1");
            Panel_lotterys[1] = (UILayout)Panel_lotteryPanels.GetWidget("Panel_lottery2");
            Panel_lotterys[2] = (UILayout)Panel_lotteryPanels.GetWidget("Panel_lottery3");
            Panel_lotterys[3] = (UILayout)Panel_lotteryPanels.GetWidget("Panel_lottery4");

            goldsInfos = new GoldInfo[4];
            goldsInfos[0].image = "shop_commodity01.png";
            goldsInfos[1].image = "shop_commodity02.png";
            goldsInfos[2].image = "shop_commodity05.png";
            goldsInfos[3].image = "shop_commodity06.png";

            for (int i = 0; i < Panel_lotterys.Length; i++)
            {
                Panel_lotterys[i].TouchBeganEvent += GameLottery_TouchBeganEvent;
                Panel_lotterys[i].GetWidget("Image_box").RunAction(new CCActionRepeatForever(new CCActionSequence(new CCActionScaleTo(0.3f, 1.1f), new CCActionScaleTo(0.3f, 0.9f))));
            }

            Button_getAll.TouchBeganEvent += button =>
            {
                BillingHelper.DoBilling(BillingID.ID6_BuyLotteryAll,this);
            };

            Button_close.TouchBeganEvent += button =>
            {
                this.Show(false);
            };

            Button_getAll.TouchBeganEvent += Function.PlayButtonEffect;
            Button_close.TouchBeganEvent += Function.PlayBackButtonEffect;

            this.AddChildCenter(main);
        }

        private void GameLottery_TouchBeganEvent(UIWidget widget)
        {
            try
            {
                UILayout Panel_lottery = (UILayout)widget;
                GoldInfo info = MathHelper.Random_minus0_1() > 0.5f ? goldsInfos[0] : goldsInfos[1];
                lotteryResult.SetGoldsInfo(info);

                UIImageView Image_box = (UIImageView)Panel_lottery.GetWidget("Image_box");
                UIImageView Image_golds = (UIImageView)Panel_lottery.GetWidget("Image_golds");
                UILabelAtlas AtlasLabel_golds = (UILabelAtlas)Panel_lottery.GetWidget("AtlasLabel_golds");
                Image_golds.LoadTexture(info.image, TextureResType.UI_TEX_TYPE_PLIST);
                Image_golds.IsVisible = true;
                AtlasLabel_golds.Text = Convert.ToString(info.golds);
                AtlasLabel_golds.IsVisible = true;
                Image_box.IsVisible = false;

                getGolds = info.golds;

                UnlotteryPanels.Clear();
                UnlooteryInfoes.Clear();

                for (int i = 0; i < Panel_lotterys.Length; i++)
                {
                    if (Panel_lotterys[i] != Panel_lottery)
                    {
                        UnlotteryPanels.Add(Panel_lotterys[i]);
                    }
                }
                for (int i = 0; i < goldsInfos.Length; i++)
                {
                    if (goldsInfos[i].image != info.image)
                    {
                        UnlooteryInfoes.Add(goldsInfos[i]);
                    }
                }

                for (int i = 0; i < 3; i++)
                {
                    UIImageView _Image_golds = (UIImageView)UnlotteryPanels[i].GetWidget("Image_golds");
                    UILabelAtlas _AtlasLabel_golds = (UILabelAtlas)UnlotteryPanels[i].GetWidget("AtlasLabel_golds");
                    _Image_golds.LoadTexture(UnlooteryInfoes[i].image, TextureResType.UI_TEX_TYPE_PLIST);
                    _AtlasLabel_golds.Text = Convert.ToString(UnlooteryInfoes[i].golds);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            //
            lotteryResult.Show(true);
            //
        }

        public override void init()
        {
            base.init();

            Button_getAll.Enabled = false;
            Button_close.Enabled = false;

            Label_getAllTips.IsVisible = false;
            Label_lotteryTips.IsVisible = true;

            for (int i = 0; i < Panel_lotterys.Length; i++)
            {
                Panel_lotterys[i].TouchEnabled = true;
                UIImageView Image_box = (UIImageView)Panel_lotterys[i].GetWidget("Image_box");
                UIImageView Image_golds = (UIImageView)Panel_lotterys[i].GetWidget("Image_golds");
                UILabelAtlas AtlasLabel_golds = (UILabelAtlas)Panel_lotterys[i].GetWidget("AtlasLabel_golds");
                Image_box.IsVisible = true;
                Image_golds.IsVisible = false;
                AtlasLabel_golds.IsVisible = false;
            }
            //
            goldsInfos[0].golds = 500 + MathHelper.Random_minus0_n(100);
            goldsInfos[1].golds = 700 + MathHelper.Random_minus0_n(100);
            goldsInfos[2].golds = 66525;
            goldsInfos[3].golds = 23175;

            allGolds = 0;
            for (int i = 0; i < goldsInfos.Length; i++)
            {
                allGolds += goldsInfos[i].golds;
            }
            //
        }

        public virtual void OnLotterResult()
        {
            Button_getAll.Enabled = true;
            Button_close.Enabled = true;

            Label_getAllTips.IsVisible = true;
            Label_lotteryTips.IsVisible = false;

            for (int i = 0; i < Panel_lotterys.Length; i++)
            {
                Panel_lotterys[i].TouchEnabled = false;
            }

            foreach (var item in UnlotteryPanels)
            {
                UIImageView Image_box = (UIImageView)item.GetWidget("Image_box");
                UIImageView Image_golds = (UIImageView)item.GetWidget("Image_golds");
                UILabelAtlas AtlasLabel_golds = (UILabelAtlas)item.GetWidget("AtlasLabel_golds");
                Image_box.IsVisible = false;
                Image_golds.IsVisible = true;
                AtlasLabel_golds.IsVisible = true;

                Image_golds.Alpha = 0;
                AtlasLabel_golds.Alpha = 0;
                Image_golds.RunAction(new MCActionFadeIn(0.6f));
                AtlasLabel_golds.RunAction(new MCActionFadeIn(0.6f));
            }

            Label_getAllTips.Text = "运气不错，所有宝石面值总共为" + allGolds + "宝石，价值" + valueMoney + "元，现在只需" + BillingHelper.CoverToBillingMoney(BillingID.ID6_BuyLotteryAll) + "元即可获取全部。";
        }

        public override void Show(bool b)
        {
            base.Show(b);
            if (!b)
            {
                //把所得的金币归到游戏数据中
                gameWin.OnLotteryOver(getGolds);
                //
            }
        }

        public void OnBillingSuccess(BillingID id)
        {          
            getGolds = allGolds;    
            this.Show(false);
            Function.ShowInfo("获取" + allGolds + "宝石成功");
        }


        public void OnBillingFail(BillingID id)
        {
            Function.ShowInfo("获取失败！");
        }
    }
}
