
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MatrixEngine.CocoStudio.GUI;
using Thunder.GameLogic.GameSystem;
using Thunder.GameLogic.Common;
using Thunder.GameLogic.UI.Dialogs;
using MatrixEngine.Cocos2d;
using Thunder.Common;
using Thunder.GameLogic.ExternAction;
using MatrixEngine.Math;
using Thunder.GameLogic.UI.Guide;

namespace Thunder.GameLogic.Gaming
{
    public class GameWin : GameWindow
    {
        private UIWidget gameWin;

        private UIButton Button_strengthen;
        private UIButton Button_continue;
        private UIImageView Image_title;
        private UILabelAtlas AtlasLabel_score;
        private UILabelAtlas AtlasLabel_golds;

        private UILabel Label_addition;

        private CCSprite Image_light;
        private UIImageView Image_addition;
        private CCSprite Image_achieve;
        private CCSprite[] Image_achieves = new CCSprite[4];

        private CCParticleSystem starParticle;

        private GameLottery gameLottery;

        public static GameWin Instance;

        public GameWin()
        {
            gameLottery = new GameLottery(this);

            this.gameWin = UIReader.GetWidget(ResID.UI_UI_GameWin);

            this.Button_strengthen = (UIButton)gameWin.GetWidget("Button_strengthen");
            this.Button_continue = (UIButton)gameWin.GetWidget("Button_continue");
            this.Image_title = (UIImageView)gameWin.GetWidget("Image_title");
            this.AtlasLabel_score = (UILabelAtlas)gameWin.GetWidget("AtlasLabel_score");
            this.AtlasLabel_golds = (UILabelAtlas)(gameWin.GetWidget("Panel_golds").GetWidget("AtlasLabel_golds"));

            this.Button_strengthen.TouchEndedEvent += button =>
                {
                    //TODO:战机升级
                    Function.GoTo(UIFunction.强化战机, "loading");
                    this.Show(false);
                    GuideWindow.Instance.Show = false;
                };
            this.Button_continue.TouchEndedEvent += button =>
                {
                    Function.GoTo(UIFunction.关卡选择, "loading");
                    this.Show(false);
                };

            Image_light = new CCSprite("effects_light01.png", true);
            Image_addition = (UIImageView)gameWin.GetWidget("Image_addition");
            Label_addition = (UILabel)Image_addition.GetWidget("Label_addition");

            Image_achieves[0] = new CCSprite("jiesuan_pingfen01.png", true);
            Image_achieves[1] = new CCSprite("jiesuan_pingfen02.png", true);
            Image_achieves[2] = new CCSprite("jiesuan_pingfen03.png", true);
            Image_achieves[3] = new CCSprite("jiesuan_pingfen04.png", true);
            Image_achieve = Image_achieves[0];

            Image_light.BlendFunc = BlendFunc.Additive;
            Image_light.ZOrder = 0;
            Image_light.Scale = 3f;
            Image_light.PostionX = Config.SCREEN_PosX_Center;
            Image_light.PostionY = Image_title.PostionY + 130;
            Image_light.RunAction(new CCActionRepeatForever(new CCActionRotateBy(1f, 60)));
            this.gameWin.AddNode(Image_light);

            this.starParticle = new CCParticleSystem(ResID.Particles_stars);
            this.starParticle.PostionX = Image_light.PostionX;
            this.starParticle.PostionY = Image_light.PostionY + 50;
            this.starParticle.Stop();
            this.gameWin.AddNode(this.starParticle);

            for (int i = 0; i < Image_achieves.Length; i++)
            {
                Image_achieves[i].Postion = Image_light.Postion;
                Image_achieves[i].ZOrder = 10;
                this.gameWin.AddNode(Image_achieves[i]);
            }

            this.AddChild(gameWin);
            Instance = this;
        }

        public override void init()
        {
            base.init();

            this.Button_strengthen.Enabled = false;
            this.Button_continue.Enabled = false;

            AtlasLabel_score.Text = Convert.ToString(GameData.Instance.PlayerData.score);
            AtlasLabel_golds.Text = Convert.ToString(GameData.Instance.PlayerData.golds);

            Image_addition.IsVisible = false;
            Image_addition.Scale = 0.1f;

            for (int i = 0; i < Image_achieves.Length; i++)
            {
                Image_achieves[i].IsVisible = false;
            }

            float HP = PlayingScene.Instance.GameLayer.Player.Info.HP / PlayingScene.Instance.GameLayer.Player.Info.totalHp;

            if (HP > 0.9)
            {
                Image_achieve = Image_achieves[3];
            }
            else if (HP > 0.8)
            {
                Image_achieve = Image_achieves[1];
            }
            else if (HP > 0.6)
            {
                Image_achieve = Image_achieves[0];
            }
            else
            {
                Image_achieve = Image_achieves[2];
            }
        }

        public override void Show(bool b)
        {
            base.Show(b);
            if (b)
            {
                gameLottery.Show(true);

            }
            else
            {

            }
        }

        protected void OnButtonShow()
        {
            this.Button_strengthen.Enabled = true;
            this.Button_continue.Enabled = true;

            this.Button_strengthen.Scale = 0.5f;
            this.Button_continue.Scale = 0.5f;

            this.Button_strengthen.RunAction(new CCActionEaseElasticOut(new CCActionScaleTo(1.3f, 1.0f)));
            this.Button_continue.RunAction(new CCActionEaseElasticOut(new CCActionScaleTo(1.3f, 1.0f)));

            GuideWindow.Instance.Command = GuideCommand.跳到升级;
            GuideWindow.Instance.Show = true;
        }

        public virtual void OnLotteryOver(int _golds)
        {
            int score = (int)PlayingScene.gameScore;
            int golds = (int)PlayingScene.gameGolds + _golds;

            AtlasLabel_score.RunSequenceActions(new CCActionDelayTime(1.0f), new ActionPlayEffect(GameAudio.Effect.score_roll), new CCActionEaseIn(new ActionAtlasRolling(1.5f, 0, score), 0.3f));
            AtlasLabel_golds.RunSequenceActions(new CCActionDelayTime(2.0f), new CCActionEaseIn(new ActionAtlasRolling(1.5f, GameData.Instance.PlayerData.golds, GameData.Instance.PlayerData.golds + golds), 0.3f), new CCActionCallFunc(this.OnButtonShow));


            if (GameData.Instance.PlayerData.score <= score)
            {
                GameData.Instance.PlayerData.score = score;
            }
            GameData.Instance.PlayerData.golds += golds;

            Image_achieve.Scale = 0.1f;
            Image_achieve.RunSequenceActions(new CCActionDelayTime(0.6f), new CCActionShow(), new ActionPlayEffect(GameAudio.Effect.xingxing), new CCActionEaseElasticOut(new CCActionScaleTo(1.3f, 1.1f)));
            this.starParticle.RunSequenceActions(new CCActionDelayTime(0.6f), new ActionPlayParticle());

            Image_addition.RunSequenceActions(new CCActionDelayTime(0.6f),new CCActionShow(), new CCActionEaseElasticOut(new CCActionScaleTo(1.3f, 1.1f)));

            Label_addition.Text = PlayingScene.Instance.CurFighterLevelData.attachGolds + "%";
        }
    }
}
