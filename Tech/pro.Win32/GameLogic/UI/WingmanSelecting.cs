using MatrixEngine.CocoStudio.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thunder.GameBilling;
using Thunder.GameLogic.Common;
using Thunder.GameLogic.GameSystem;
using Thunder.GameLogic.Gaming;

namespace Thunder.GameLogic.UI
{
    public class WingmanSelecting : GameWindow,BillingWindow, IUpdateUI
    {

        private UIWidget wingmanSelecting;
        private UILayout Panel_wingmen;

        private UIButton[] Button_wingmans;
        private UIImageView[] Image_buys;
        private UIImageView[] Image_equips;
        private WingmanData[] wingmanDatas;

        private PlayerData playerData;

        private UIButton Button_back;

        private IUpdateUI playerSelectionLayer;

        public WingmanSelecting(IUpdateUI updateUp)
        {
            playerSelectionLayer = updateUp;

            wingmanSelecting = UIReader.GetWidget(ResID.UI_UI_WingmanSelecting);
            Panel_wingmen = (UILayout)wingmanSelecting.GetWidget("Panel_wingmen");

            Button_back = (UIButton)wingmanSelecting.GetWidget("Button_back");

            playerData = GameData.Instance.PlayerData;

            wingmanDatas = new WingmanData[4];
            wingmanDatas[0] = GameData.Instance.GetWingmanData(WingmanSpawner.WingmanID.Wingman1);
            wingmanDatas[1] = GameData.Instance.GetWingmanData(WingmanSpawner.WingmanID.Wingman2);
            wingmanDatas[2] = GameData.Instance.GetWingmanData(WingmanSpawner.WingmanID.Wingman3);
            wingmanDatas[3] = GameData.Instance.GetWingmanData(WingmanSpawner.WingmanID.Wingman4);

            Button_wingmans = new UIButton[4];
            Button_wingmans[0] = (UIButton)(Panel_wingmen.GetWidget("Panel_wingman1").GetWidget("Button_wingman1"));
            Button_wingmans[1] = (UIButton)(Panel_wingmen.GetWidget("Panel_wingman2").GetWidget("Button_wingman2"));
            Button_wingmans[2] = (UIButton)(Panel_wingmen.GetWidget("Panel_wingman3").GetWidget("Button_wingman3"));
            Button_wingmans[3] = (UIButton)(Panel_wingmen.GetWidget("Panel_wingman4").GetWidget("Button_wingman4"));

            Image_buys = new UIImageView[4];
            Image_buys[0] = (UIImageView)(Panel_wingmen.GetWidget("Panel_wingman1").GetWidget("Image_buy"));
            Image_buys[1] = (UIImageView)(Panel_wingmen.GetWidget("Panel_wingman2").GetWidget("Image_buy"));
            Image_buys[2] = (UIImageView)(Panel_wingmen.GetWidget("Panel_wingman3").GetWidget("Image_buy"));
            Image_buys[3] = (UIImageView)(Panel_wingmen.GetWidget("Panel_wingman4").GetWidget("Image_buy"));

            Image_equips = new UIImageView[4];
            Image_equips[0] = (UIImageView)(Panel_wingmen.GetWidget("Panel_wingman1").GetWidget("Image_equip"));
            Image_equips[1] = (UIImageView)(Panel_wingmen.GetWidget("Panel_wingman2").GetWidget("Image_equip"));
            Image_equips[2] = (UIImageView)(Panel_wingmen.GetWidget("Panel_wingman3").GetWidget("Image_equip"));
            Image_equips[3] = (UIImageView)(Panel_wingmen.GetWidget("Panel_wingman4").GetWidget("Image_equip"));

            Button_wingmans[0].TouchBeganEvent += button =>
            {
                SelectingWingman(wingmanDatas[0]);
            };

            Button_wingmans[1].TouchBeganEvent += button =>
            {
                SelectingWingman(wingmanDatas[1]);
            };

            Button_wingmans[2].TouchBeganEvent += button =>
            {
                SelectingWingman(wingmanDatas[2]);
            };

            Button_wingmans[3].TouchBeganEvent += button =>
            {
                SelectingWingman(wingmanDatas[3]);
            };

            Button_back.TouchBeganEvent += button =>
            {
                this.Show(false);
            };


            for (int i = 0; i < Button_wingmans.Length; i++)
            {
                Button_wingmans[i].TouchBeganEvent += Function.PlayButtonEffect;
            }

            this.AddChildCenter(wingmanSelecting);
        }

        public override void init()
        {
            base.init();

            ResetUIData();
        }

        protected override void WindowBackGround()
        {
            base.WindowBackGround();
            backGround.SetBackGroundColorOpacity(0);
        }


        private void SwitchWingman(WingmanSpawner.WingmanID id)
        {
            if (playerData.curWingman != id)
            {
                WingmanSpawner.Instance.Count = 0;
                playerData.curWingman = id;
                WingmanSpawner.Instance.SetCurWingman(id);
                WingmanSpawner.Instance.Count = 1;
            }
        }

        protected virtual void SelectingWingman(WingmanData wingmandata)
        {
            if (playerData.withWingman)
            {
                if (wingmandata.isOpen)
                {
                    this.SwitchWingman(wingmandata.id);
                }
                else
                {
                    if (wingmandata.id == WingmanSpawner.WingmanID.Wingman4)
                    {
                        //TODO:买买买,直接弹出sdk
                        BillingHelper.DoBilling(BillingID.ID9_BuyWingman,this);
                    }
                    else
                    {
                        if (playerData.golds >= wingmandata.unlockGolds)
                        {
                            playerData.golds -= wingmandata.unlockGolds;
                            wingmandata.isOpen = true;
                            //this.SwitchWingman(wingmandata.id);
                            Function.ShowInfo("开启僚机成功!");
                        }
                        else 
                        {
                            Function.GoTo(UIFunction.购买金币);
                        }
                    }

                }
            }
            //
            this.ResetUIData();
        }

        public void ResetUIData()
        {
            for (int i = 0; i < Image_equips.Length; i++)
            {
                Image_equips[i].IsVisible = false;
            }

            if (playerData.withWingman)
            {
                switch (playerData.curWingman)
                {
                    case WingmanSpawner.WingmanID.Wingman1:
                        Image_equips[0].IsVisible = true;
                        break;
                    case WingmanSpawner.WingmanID.Wingman2:
                        Image_equips[1].IsVisible = true;
                        break;
                    case WingmanSpawner.WingmanID.Wingman3:
                        Image_equips[2].IsVisible = true;
                        break;
                    case WingmanSpawner.WingmanID.Wingman4:
                        Image_equips[3].IsVisible = true;
                        break;
                    default:
                        break;
                }
            }

            for (int i = 0; i < wingmanDatas.Length; i++)
            {
                if (wingmanDatas[i].isOpen)
                {
                    Image_buys[i].IsVisible = false;
                }
                else
                {
                    Image_buys[i].IsVisible = true;
                }
                UILabelAtlas AtlasLabel_golds = (UILabelAtlas)Image_buys[i].GetWidget("AtlasLabel_golds");
                AtlasLabel_golds.Text = Convert.ToString(wingmanDatas[i].unlockGolds);
            }

            playerSelectionLayer.ResetUIData();
        }

        public void OnBillingSuccess(BillingID id)
        {
            //能来到这里肯定是买这台僚机的
            wingmanDatas[3].isOpen = true;
            Function.ShowInfo("开启僚机成功!");
            this.ResetUIData();
        }

        public void OnBillingFail(BillingID id)
        {
            
        }
    }
}
