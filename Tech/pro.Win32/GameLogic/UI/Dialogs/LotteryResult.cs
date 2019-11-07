using MatrixEngine.Cocos2d;
using MatrixEngine.CocoStudio.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thunder.Common;
using Thunder.GameLogic.Common;
using Thunder.GameLogic.ExternAction;
using Thunder.GameLogic.GameSystem;

namespace Thunder.GameLogic.UI.Dialogs
{
    public class LotteryResult:GameWindow
    {
        GameLottery gameLottery;
        UIWidget Panel_main;
        UIButton Button_close;

        CCSprite Image_light;
        UIImageView Image_golds;

        UILabelAtlas AtlasLabel_goldNum;

        CCAction rotation;

        public LotteryResult(GameLottery _gameLottery)
        {
            gameLottery = _gameLottery;
            Panel_main = UIReader.GetWidget(ResID.UI_UI_LotteryResult);

            Button_close = (UIButton)Panel_main.GetWidget("Button_close");
            AtlasLabel_goldNum = (UILabelAtlas)Panel_main.GetWidget("AtlasLabel_goldNum");
            //
            Image_light = new CCSprite("effects_light01.png",true);
            Image_light.BlendFunc = BlendFunc.Additive;
            Image_light.Postion = Config.ScreenCenter; 

            Image_golds = new UIImageView();
            Image_golds.Postion = Config.ScreenCenter;
            Image_golds.IsVisible = false;

            rotation = new CCActionRepeatForever(new CCActionRotateBy(1,90f));
            Image_light.RunAction(rotation);

            Panel_main.AddNode(Image_light);
            Panel_main.AddChild(Image_golds);
            //TODO:
            Button_close.TouchBeganEvent += button =>
            {
                this.Show(false);
            };

            this.AddChildCenter(Panel_main);
        }

        public override void init()
        {
            base.init();
            Image_light.Scale = 1.0f;
            Image_light.RunAction(new CCActionEaseElasticOut(new CCActionScaleTo(1.5f, 2f)));

            Image_golds.Scale = 1.0f;
            Image_golds.RunAction(new CCActionEaseElasticOut(new CCActionScaleTo(1.8f, 1.5f)));

            GameAudio.PlayEffect(GameAudio.Effect.boxopen);
        }

        public override void Show(bool b)
        {
            base.Show(b);
            if (b)
            {
                Image_golds.IsVisible = true;
            }
            else
            {
                gameLottery.OnLotterResult();
            }
        }

        public virtual void SetGoldsInfo(Thunder.GameLogic.UI.Dialogs.GameLottery.GoldInfo info)
        {
            Image_golds.LoadTexture(info.image, TextureResType.UI_TEX_TYPE_PLIST);
            AtlasLabel_goldNum.Text = Convert.ToString(info.golds);
            AtlasLabel_goldNum.RunAction(new CCActionEaseIn(new ActionAtlasRolling(1.5f, 0, info.golds), 0.3f));
        }
    }
}
