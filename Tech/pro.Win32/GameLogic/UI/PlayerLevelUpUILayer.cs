
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MatrixEngine.CocoStudio.GUI;
using Thunder.GameLogic.Common;
using Thunder.GameLogic.Gaming;
using Thunder.Common;
using MatrixEngine.Cocos2d;
using MatrixEngine.Math;
using Thunder.GameLogic.Gaming.BulletSystems;
using Thunder.GameLogic.Gaming.Actors.Players;
using Thunder.GameLogic.ExternAction;
using Thunder.GameLogic.UI.Widgets;
using Thunder.GameBilling;
using Thunder.GameLogic.UI.Guide;

namespace Thunder.GameLogic.UI
{
    public class PlayerLevelUpUILayer : UILayer, IUpdateUI,BillingWindow
    {
        UIWidget playerLevelUp;

        UIButton Button_back;
        UIButton Button_levelUp;
        UIButton Button_levelTop;
        UIButton Button_add;

        UILayout Panel_playerInfo;
        UILayout Panel_nextLevelInfo;

        UILabelAtlas AtlasLabel_gold;
        UILabelAtlas AtlasLabel_score;
        UILabelAtlas AtlasLabel_unlockGolds;
        UILabelAtlas AtlasLabel_level;

        UIImageView Image_gold;

        UILabel Label_info;
        UILabel Label_curGoldAddition;
        UILabel Label_curScroeAddition;
        UILabel Label_nextGoldAddition;
        UILabel Label_nextScroeAddition;
        UILabel Label_topLevel;

        UIButton Button_power;
        UIButton Button_hp;
        UIButton Button_frenzy;
        UIButton Button_wingman;
        UIButton Button_shield;
        UIButton Button_skill;

        CCSprite Image_power;
        CCSprite Image_hp;
        CCSprite Image_frenzy;
        CCSprite Image_wingman;
        CCSprite Image_shield;
        CCSprite Image_skill;

        List<StarAttributeBar> attributeStars = new List<StarAttributeBar>();

        private Vector2 stayPoint;
        private Vector2 playerInitPoint;

        PlayerData playerData;
        FighterData fighterData;
        FighterData.FighterLevelData curLevelData;
        FighterData.FighterLevelData nextLevelData;

        private Player player;

        CCClippingNode showingWorldNode = new CCClippingNode();

        public PlayerLevelUpUILayer()
        {
            playerLevelUp = UIReader.GetWidget(ResID.UI_UI_PlayerLevelUp);

            Button_back = (UIButton)playerLevelUp.GetWidget("Button_back");
            Button_levelUp = (UIButton)playerLevelUp.GetWidget("Button_levelUp");
            Button_levelTop = (UIButton)playerLevelUp.GetWidget("Button_levelTop");
            Button_add = (UIButton)(playerLevelUp.GetWidget("Panel_gold").GetWidget("Button_add"));

            AtlasLabel_gold = (UILabelAtlas)(playerLevelUp.GetWidget("Panel_gold").GetWidget("AtlasLabel_gold"));
            AtlasLabel_score = (UILabelAtlas)(playerLevelUp.GetWidget("Panel_score").GetWidget("AtlasLabel_score"));
            AtlasLabel_unlockGolds = (UILabelAtlas)playerLevelUp.GetWidget("AtlasLabel_unlockGolds");
            Image_gold = (UIImageView)playerLevelUp.GetWidget("Image_gold");

            Panel_playerInfo = (UILayout)playerLevelUp.GetWidget("Panel_playerInfo");
            Panel_nextLevelInfo = (UILayout)Panel_playerInfo.GetWidget("Panel_nextLevelInfo");
            UILayout Panel_show = (UILayout)playerLevelUp.GetWidget("Panel_show");

            Label_curGoldAddition = (UILabel)(Panel_show.GetWidget("Image_golds").GetWidget("Label_addition"));
            Label_curScroeAddition = (UILabel)(Panel_show.GetWidget("Image_scroe").GetWidget("Label_addition"));
            AtlasLabel_level = (UILabelAtlas)Panel_show.GetWidget("AtlasLabel_level");

            Label_info = (UILabel)Panel_playerInfo.GetWidget("Label_info");
            Label_topLevel = (UILabel)Panel_playerInfo.GetWidget("Label_topLevel");
            Label_nextGoldAddition = (UILabel)Panel_nextLevelInfo.GetWidget("Label_goldAddition");
            Label_nextScroeAddition = (UILabel)Panel_nextLevelInfo.GetWidget("Label_scroeAddition");
            //
            UILayout Panel_power = (UILayout)playerLevelUp.GetWidget("Panel_power");
            StarAttributeBar stars = new StarAttributeBar();
            stars.Postion = new Vector2(20,20);
            attributeStars.Add(stars);
            Panel_power.AddChild(stars);
            Button_power = (UIButton)Panel_power.GetWidget("Button");
            Button_power.UserData = "火力提升";
            Button_power.TouchBeganEvent += Function.PlayButtonEffect;
            Button_power.TouchBeganEvent += AttributeButton_TouchBeganEvent;

            UILayout Panel_hp = (UILayout)playerLevelUp.GetWidget("Panel_hp");
            stars = new StarAttributeBar();
            stars.Postion = new Vector2(20, 20);
            attributeStars.Add(stars);
            Panel_hp.AddChild(stars);
            Button_hp = (UIButton)Panel_hp.GetWidget("Button");
            Button_hp.UserData = "生命增加";
            Button_hp.TouchBeganEvent += Function.PlayButtonEffect;
            Button_hp.TouchBeganEvent += AttributeButton_TouchBeganEvent;

            UILayout Panel_frenzy = (UILayout)playerLevelUp.GetWidget("Panel_frenzy");
            stars = new StarAttributeBar();
            stars.Postion = new Vector2(20, 20);
            attributeStars.Add(stars);
            Panel_frenzy.AddChild(stars);
            Button_frenzy = (UIButton)Panel_frenzy.GetWidget("Button");
            Button_frenzy.UserData = "暴走时间";
            Button_frenzy.TouchBeganEvent += Function.PlayButtonEffect;
            Button_frenzy.TouchBeganEvent += AttributeButton_TouchBeganEvent;

            UILayout Panel_wingman = (UILayout)playerLevelUp.GetWidget("Panel_wingman");
            stars = new StarAttributeBar();
            stars.Postion = new Vector2(20, 20);
            attributeStars.Add(stars);
            Panel_wingman.AddChild(stars);
            Button_wingman = (UIButton)Panel_wingman.GetWidget("Button");
            Button_wingman.UserData = "僚机攻击";
            Button_wingman.TouchBeganEvent += Function.PlayButtonEffect;
            Button_wingman.TouchBeganEvent += AttributeButton_TouchBeganEvent;

            UILayout Panel_shield = (UILayout)playerLevelUp.GetWidget("Panel_shield");
            stars = new StarAttributeBar();
            stars.Postion = new Vector2(20, 20);
            attributeStars.Add(stars);
            Panel_shield.AddChild(stars);
            Button_shield = (UIButton)Panel_shield.GetWidget("Button");
            Button_shield.UserData = "护盾数量";
            Button_shield.TouchBeganEvent += Function.PlayButtonEffect;
            Button_shield.TouchBeganEvent += AttributeButton_TouchBeganEvent;

            UILayout Panel_skill = (UILayout)playerLevelUp.GetWidget("Panel_skill");
            stars = new StarAttributeBar();
            stars.Postion = new Vector2(20, 20);
            attributeStars.Add(stars);
            Panel_skill.AddChild(stars);
            Button_skill = (UIButton)Panel_skill.GetWidget("Button");
            Button_skill.UserData = "必杀攻击";
            Button_skill.TouchBeganEvent += Function.PlayButtonEffect;
            Button_skill.TouchBeganEvent += AttributeButton_TouchBeganEvent;
            //
            Image_power = new CCSprite("shenji_01.png", true);
            Image_hp = new CCSprite("shenji_03.png", true);
            Image_frenzy = new CCSprite("shenji_05.png", true);
            Image_wingman = new CCSprite("shenji_02.png", true);
            Image_shield = new CCSprite("shenji_04.png", true);
            Image_skill = new CCSprite("shenji_06.png", true);

            Panel_power.AddNode(Image_power);
            Panel_hp.AddNode(Image_hp);
            Panel_frenzy.AddNode(Image_frenzy);
            Panel_wingman.AddNode(Image_wingman);
            Panel_shield.AddNode(Image_shield);
            Panel_skill.AddNode(Image_skill);

            Image_power.IsVisible = false;
            Image_hp.IsVisible = false;
            Image_frenzy.IsVisible = false;
            Image_wingman.IsVisible = false;
            Image_shield.IsVisible = false;
            Image_skill.IsVisible = false;

            Image_power.Postion = Button_power.Postion;
            Image_hp.Postion = Button_hp.Postion;
            Image_frenzy.Postion = Button_frenzy.Postion;
            Image_wingman.Postion = Button_wingman.Postion;
            Image_shield.Postion = Button_shield.Postion;
            Image_skill.Postion = Button_skill.Postion;
            //
            UIImageView Image_backGroup = (UIImageView)playerLevelUp.GetWidget("Image_backgroup");
            //Image_backGroup.ScaleY = 1 / Config.SCREEN_RATE.X;
            //
            showingWorldNode.ContextSize = new Size(Panel_show.Size.width, Panel_show.Size.height);
            showingWorldNode.Postion = Panel_show.Postion;
            playerLevelUp.AddNode(showingWorldNode);
            showingWorldNode.AddDefaultStendcil();
            //
            Button_back.TouchEndedEvent += button =>
            {
                if (GameScene.PerGameScene is PlayingScene)
                {
                    Function.GoTo(UIFunction.关卡选择);
                }
                else
                {
                    Function.GoTo(UIFunction.战机选择);
                }

                GuideWindow.Instance.Show = false;
            };

            Button_add.TouchEndedEvent += button =>
            {
                Function.GoTo(UIFunction.购买金币);
            };

            Button_levelUp.TouchEndedEvent += Button_levelUp_TouchEndedEvent;
            Button_levelTop.TouchEndedEvent += Button_levelTop_TouchEndedEvent;


            Button_back.TouchBeganEvent += Function.PlayBackButtonEffect;
            Button_levelUp.TouchBeganEvent += Function.PlayButtonEffect;
            Button_levelTop.TouchBeganEvent += Function.PlayButtonEffect;
            Button_add.TouchBeganEvent += Function.PlayButtonEffect;

            this.AddWidget(playerLevelUp);
        }

        private void Button_levelTop_TouchEndedEvent(UIWidget widget)
        {
            if (fighterData.curLevel < fighterData.topLevel)
            {
                //弹出计费点
                BillingHelper.DoBilling(BillingID.ID8_BuyLevelUp, this);
            }
     
            
        }

        private void Button_levelUp_TouchEndedEvent(UIWidget widget)
        {

            if (fighterData.curLevel < fighterData.topLevel)
            {
                if (playerData.golds < nextLevelData.unlockGolds)
                {
                    Function.GoTo(UIFunction.购买金币);
                }
                else
                {                   
                    fighterData.curLevel++;

                    AtlasLabel_gold.RunAction(new CCActionEaseIn(new ActionAtlasRolling(1f, playerData.golds, playerData.golds - nextLevelData.unlockGolds), 0.3f));
                    playerData.golds -= nextLevelData.unlockGolds;

                    this.LevelUpOver();

                    GuideWindow.Instance.Show = false;
                    GuideWindow.Instance.Command = GuideCommand.升级返回;
                    GuideWindow.Instance.Show = true;
                }
            }
            this.ResetUIData();
        }

        private void AttributeButton_TouchBeganEvent(UIWidget widget)
        {
            UIButton button = (UIButton)widget;
            string info = "";
            if (button == Button_power)
            {
                info = "+" + curLevelData.attachPlayerDamage; 
            }
            else if (button == Button_hp)
            {
                info = "+" + curLevelData.attachHP;
            }
            else if (button == Button_frenzy)
            {
                info = "+" + curLevelData.attachCritTime;
            }
            else if (button == Button_wingman)
            {
                info = "+" + curLevelData.attachWingmanDamage;
            }
            else if (button == Button_shield)
            {
                info = "+" + curLevelData.attachShieldCount;
            }
            else if(button == Button_skill)
            {
                info = "+" + curLevelData.attachSkillDamage;
            }
            Label_info.Text = (string)button.UserData + info;
        }

        public override void OnEnter()
        {
            if (GameAudio.CurMusic != GameAudio.Music.menu_bg)
            {
                GameAudio.PlayMusic(GameAudio.Music.menu_bg, true);
            }

            //设置演示框的战机
            PlayerSpawner.Instanse.IsShowing = true;
            WingmanSpawner.Instance.IsShowing = true;
            foreach (var item in PlayerSpawner.Instanse.AllPlayer())
            {
                item.GotoBehavior(Player.Behavior.Null);
                item.RemoveFromParent();
            }
            WingmanSpawner.Instance.RemoveFromWorld();
            //
            player = PlayerSpawner.Instanse.CurPlayer;

            BulletEmitter.WorldNode = showingWorldNode;
            PlayerSpawner.Instanse.PlayerLayer = showingWorldNode;
            PlayerSpawner.Instanse.ActivatePlayer();

            stayPoint = new Vector2(showingWorldNode.ContextSize.width / 2, showingWorldNode.ContextSize.height / 3 + 20);
            playerInitPoint = new Vector2(showingWorldNode.ContextSize.width / 2, -100);

            showingWorldNode.AddChild(player);

            WingmanSpawner.Instance.Player = player;
            player.StayPoint = stayPoint;
            player.InitPoint = playerInitPoint;
            player.Postion = playerInitPoint;

            WingmanSpawner.Instance.AddToWorld(showingWorldNode);
            WingmanSpawner.Instance.Count = GameData.Instance.PlayerData.withWingman ? 1 : 0;
            player.FlyIn();

            if (!GuideWindow.Instance.GetGuideData(GuideCommand.点击升级).IsPlay)
            {
                GuideWindow.Instance.Command = GuideCommand.点击升级;
                GuideWindow.Instance.Show = true;

                GameData.Instance.PlayerData.golds += 2000;
            }

            ResetUIData();
        }

        public override void OnEnterTransitionFinish()
        {
            base.OnEnterTransitionFinish();
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        private void SetStarsLevel(int level)
        {
            for (int i = 0; i < attributeStars.Count; i++)
            {
                if (attributeStars[i].Level!=level)
                    attributeStars[i].Level = level;
            }
        }

        protected virtual void LevelUpOver()
        {
            Image_power.IsVisible = true;
            Image_hp.IsVisible = true;
            Image_frenzy.IsVisible = true;
            Image_wingman.IsVisible = true;
            Image_shield.IsVisible = true;
            Image_skill.IsVisible = true;

            Image_power.Scale = 1;
            Image_hp.Scale = 1;
            Image_frenzy.Scale = 1;
            Image_wingman.Scale = 1;
            Image_shield.Scale = 1;
            Image_skill.Scale = 1;

            Image_power.RunSequenceActions(new CCActionSpawn(new CCActionFadeOut(1), new CCActionScaleTo(1f,2f)),new CCActionHide());
            Image_hp.RunSequenceActions(new CCActionSpawn(new CCActionFadeOut(1), new CCActionScaleTo(1f, 2f)), new CCActionHide());
            Image_frenzy.RunSequenceActions(new CCActionSpawn(new CCActionFadeOut(1), new CCActionScaleTo(1f, 2f)), new CCActionHide());
            Image_wingman.RunSequenceActions(new CCActionSpawn(new CCActionFadeOut(1), new CCActionScaleTo(1f, 2f)), new CCActionHide());
            Image_shield.RunSequenceActions(new CCActionSpawn(new CCActionFadeOut(1), new CCActionScaleTo(1f, 2f)), new CCActionHide());
            Image_skill.RunSequenceActions(new CCActionSpawn(new CCActionFadeOut(1), new CCActionScaleTo(1f, 2f)), new CCActionHide());
        }

        public virtual void ResetUIData()
        {
            playerData = GameData.Instance.PlayerData;
            fighterData = GameData.Instance.GetFighterData(PlayerSpawner.Instanse.CurPlayerID);

            curLevelData = fighterData.GetLevelData();

            if (fighterData.curLevel < fighterData.topLevel)
            {
                nextLevelData = fighterData.GetLevelData(fighterData.curLevel + 1);

                Label_nextGoldAddition.Text = Convert.ToString(nextLevelData.attachGolds) + "%";
                Label_nextScroeAddition.Text = Convert.ToString(nextLevelData.attachScore) + "%";
                AtlasLabel_unlockGolds.Text = Convert.ToString(nextLevelData.unlockGolds);

                Button_levelUp.Enabled = true;
                Button_levelTop.Enabled = true;
                AtlasLabel_unlockGolds.IsVisible = true;
                Panel_nextLevelInfo.IsVisible = true;
                Label_topLevel.IsVisible = false;
                Image_gold.IsVisible = true;
            }
            else
            {
                Button_levelUp.Enabled = false;
                Button_levelTop.Enabled = false;
                AtlasLabel_unlockGolds.IsVisible = false;
                Panel_nextLevelInfo.IsVisible = false;
                Label_topLevel.IsVisible = true;
                Image_gold.IsVisible = false;
            }

            Label_curGoldAddition.Text = "金币加成" + curLevelData.attachGolds + "%";
            Label_curScroeAddition.Text = "分数加成" + curLevelData.attachScore + "%";
            AtlasLabel_level.Text = Convert.ToString(fighterData.curLevel);

            AtlasLabel_gold.SetText(playerData.golds.ToString());
            AtlasLabel_score.SetText(playerData.score.ToString());

            this.SetStarsLevel(fighterData.curLevel);
        }

        public void OnBillingSuccess(BillingID id)
        {
            fighterData.curLevel = fighterData.topLevel;
            this.LevelUpOver();
            this.ResetUIData();
            Function.ShowInfo("已升至最高级");
        }


        public void OnBillingFail(BillingID id)
        {
            Function.ShowInfo("升级失败!");
        }
    }
}
