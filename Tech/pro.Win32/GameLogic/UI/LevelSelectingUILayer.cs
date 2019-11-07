
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MatrixEngine.Cocos2d;
using MatrixEngine.CocoStudio.GUI;
using Thunder.GameLogic.Common;
using MatrixEngine.CocoStudio;
using MatrixEngine.Math;
using Thunder.GameLogic.UI.Widgets;
using Thunder.Common;
using Thunder.GameLogic.ExternAction;
using Thunder.GameLogic.Gaming.BulletSystems;
using MatrixEngine.Engine;
using Thunder.GameLogic.UI.Guide;

namespace Thunder.GameLogic.UI
{
    public class LevelSelectingUILayer : UILayer, IPageEventListener, IUpdateUI
    {
        UIWidget levelSelecting;
        UIWidget levelPages;

        UIPageView PageView_levels;
        UILayout Panel_describe;
        UILayout Panel_power;
        UILabel Label_bossName;
        UILayout Panel_boss;

        UILabelAtlas AtlasLabel_power;
        UILabelAtlas AtlasLabel_minutes;
        UILabelAtlas AtlasLabel_seconds;

        UIButton Button_add;
        UIButton Button_back;
        UIButton Button_play;
        UIButton Button_gifts;
        UIButton Button_left;
        UIButton Button_right;

        UIButton[] levelButtons;

        BulletEffects selectedEffect;
        UIButton selsetedButton;

        private CCAction scaleAction1;
        private CCAction scaleAction2;

        private PowerAttributeBar attriAttack;
        private PowerAttributeBar attriDef;
        private PowerAttributeBar attriSpeed;

        private CCSprite powerLight;
        private CCSprite giftLight;
        private CCSprite playLight;

        bool isSelectedLevel;

        private class ActionRestMeteortite : MCActionInstant
        {
            UIImageView backgroup;
            public ActionRestMeteortite(UIImageView image)
            {
                backgroup = image;
            }

            protected override void OnUpdate(CCNode pNode)
            {
                pNode.PostionY = -backgroup.Size.height / 2 + 100 + MathHelper.Random_minus0_n(200);
                pNode.PostionX = -backgroup.Size.width / 2 - 50;
                pNode.Scale = 0.5f + MathHelper.Random_minus0_1() * 1f;
            }

            public override CCAction Reverse()
            {
                return new ActionRestMeteortite(backgroup);
            }
        }

        public LevelSelectingUILayer()
        {

            levelSelecting = UIReader.GetWidget(ResID.UI_UI_LevelSelecting);
            levelPages = UIReader.GetWidget(ResID.UI_UI_LevelPages);

            PageView_levels = (UIPageView)levelSelecting.GetWidget("PageView_levels");
            Panel_describe = (UILayout)levelSelecting.GetWidget("Panel_describe");
            UIImageView Image_platform = (UIImageView)Panel_describe.GetWidget("Image_platform");

            Panel_power = (UILayout)levelSelecting.GetWidget("Panel_power");
            AtlasLabel_power = (UILabelAtlas)Panel_power.GetWidget("AtlasLabel_power");
            AtlasLabel_minutes = (UILabelAtlas)Panel_power.GetWidget("AtlasLabel_minutes");
            AtlasLabel_seconds = (UILabelAtlas)Panel_power.GetWidget("AtlasLabel_seconds");
            Button_add = (UIButton)Panel_power.GetWidget("Button_add");

            UIImageView image_power = (UIImageView)Panel_power.GetWidget("Image_7");
            powerLight = new CCSprite("tili.png", true);
            powerLight.IsVisible = false;
            powerLight.BlendFunc = BlendFunc.Additive;
            var action = new CCActionSequence(new CCActionShow(),new ActionSetScale(1),new CCActionSpawn(new CCActionScaleTo(1f,2),new CCActionFadeOut(1f)),new CCActionHide(),new CCActionDelayTime(1));
            powerLight.RunAction(new CCActionRepeatForever(action));
            image_power.AddNode(powerLight);

            CCParticleSystem smoke1 = new CCParticleSystem(ResID.Particles_lansehuo);
            smoke1.Play();
            smoke1.Postion = new Vector2(-175, -50);
            smoke1.Rotation = 60;
            Image_platform.AddNode(smoke1);

            CCParticleSystem smoke2 = new CCParticleSystem(ResID.Particles_lansehuo);
            smoke2.Play();
            smoke2.Postion = new Vector2(175, -50);
            smoke2.Rotation = 300;
            Image_platform.AddNode(smoke2);

            UIImageView Image_backGroup = (UIImageView)levelSelecting.GetWidget("Image_backGroup");
            //Image_backGroup.ScaleY = 1 / Config.SCREEN_RATE.X;

            //
            this.SetupPlanet(Image_backGroup, "ui_yunshi1.png");
            this.SetupPlanet(Image_backGroup, "ui_yunshi2.png");
            for (int i = 0; i < 15; i++)
            {
                string name = "ui_yunshi3.png";
                int n = MathHelper.Random_minus0_n(3);
                switch (n)
                {
                    case 0:
                        name = "ui_yunshi3.png";
                        break;
                    case 1:
                        name = "ui_yunshi4.png";
                        break;
                    case 2:
                        name = "ui_yunshi5.png";
                        break;
                    default:
                        break;
                }
                this.SetupMeteorites(Image_backGroup, name);
            }
            //

            UIButton levelButton = (UIButton)(UIReader.GetWidget(ResID.UI_UI_Templates).GetWidget("Button_level"));

            selectedEffect = new BulletEffects(ResID.Particles_guanqiaxuanze_1, ResID.Particles_guanqiaxuanze_2, ResID.Particles_guanqiaxuanze_3);
            selectedEffect.PositionType = tCCPositionType.kCCPositionTypeGrouped;
            selectedEffect.Play();

            PageView_levels.AddEventListenerPageView(this);
            //设置选关按钮
            levelButtons = new UIButton[24];
            int t = 0;
            for (int i = 0; i < 8; i++)
            {
                UILayout page = (UILayout)levelPages.GetWidget("Panel_" + (i + 1));
                for (int j = 0; j < 3; j++)
                {
                    levelButtons[t] = (UIButton)levelButton.Copy();
                    levelButtons[t].IsVisible = true;
                    levelButtons[t].Tag = t;
                    UIImageView buttonTag = (UIImageView)page.GetWidget("Image_" + (t + 1));
                    buttonTag.IsVisible = false;
                    levelButtons[t].Postion = buttonTag.Postion;
                    levelButtons[t].TouchEvent += LevelButtonClicked;
                    levelButtons[t].TouchBeganEvent += Function.PlayButtonEffect;

                    UILabelAtlas AtlasLabel_index = (UILabelAtlas)levelButtons[t].GetWidget("AtlasLabel_index");
                    AtlasLabel_index.Text = (t + 1).ToString();

                    page.AddChild(levelButtons[t]);

                    LevelData level = GameData.Instance.GetLevelData(t + 1);
                    //if (level != null)
                    {
                        level.userData = levelButtons[t];
                        levelButtons[t].UserData = level;
                    }
                    t++;
                }
                page.RemoveFromParent();
                PageView_levels.AddPage(page);
            }
            //
            PageView_levels.AddNode(selectedEffect);
            PageView_levels.TouchCanceledEvent += PageView_levels_TouchCanceledEvent;
            //
            //设置boss动画
            Panel_boss = (UILayout)Panel_describe.GetWidget("Panel_boss");
            //CCAction actionUp = new CCActionMoveTo(1.0f, new Vector2(Panel_boss.Postion.X, Panel_boss.Postion.Y + 5));
            //CCAction actionDown = new CCActionMoveTo(1.0f, new Vector2(Panel_boss.Postion.X, Panel_boss.Postion.Y - 5));
            //CCAction floating = new CCActionRepeatForever(new CCActionSequence(actionUp, actionDown));
            //Panel_boss.RunAction(floating);
            foreach (var item in GameData.Instance.LevelDatas)
            {
                Panel_boss.AddNode(item.armatureData);
                item.armatureData.PostionX = Panel_boss.Size.width / 2;
                item.armatureData.PostionY = Panel_boss.Size.height / 2;

                item.animationData.Play(item.bossAnimationName, true);
                item.armatureData.IsVisible = false;
            }

            UILayout Panel_attribute = (UILayout)Panel_describe.GetWidget("Panel_attribute");
            Label_bossName = (UILabel)Panel_attribute.GetWidget("Label_bossName");

            UIImageView Image_attack = (UIImageView)Panel_attribute.GetWidget("Image_attack");
            UIImageView Image_def = (UIImageView)Panel_attribute.GetWidget("Image_def");
            UIImageView Image_speed = (UIImageView)Panel_attribute.GetWidget("Image_speed");

            attriAttack = new PowerAttributeBar();
            attriDef = new PowerAttributeBar();
            attriSpeed = new PowerAttributeBar();

            Panel_attribute.AddChild(attriAttack);
            Panel_attribute.AddChild(attriDef);
            Panel_attribute.AddChild(attriSpeed);

            attriAttack.Postion = new Vector2(Image_attack.Postion.X + 30, Image_attack.Postion.Y);
            attriDef.Postion = new Vector2(Image_def.Postion.X + 30, Image_def.Postion.Y);
            attriSpeed.Postion = new Vector2(Image_speed.Postion.X + 30, Image_speed.Postion.Y);
            //

            Button_back = (UIButton)levelSelecting.GetWidget("Button_back");
            Button_play = (UIButton)levelSelecting.GetWidget("Button_play");
            Button_gifts = (UIButton)levelSelecting.GetWidget("Button_gifts");
            Button_left = (UIButton)levelSelecting.GetWidget("Button_left");
            Button_right = (UIButton)levelSelecting.GetWidget("Button_right");

            //
            giftLight = new CCSprite("pause_libao.png", true);
            giftLight.IsVisible = false;
            giftLight.BlendFunc = BlendFunc.Additive;
            var giftLight_action = new CCActionSequence(new CCActionShow(), new ActionSetScale(1), new CCActionSpawn(new CCActionScaleTo(1f, 2), new CCActionFadeOut(1f)), new CCActionHide(), new CCActionDelayTime(1.2f));
            giftLight.RunAction(new CCActionRepeatForever(giftLight_action));
            Button_gifts.AddNode(giftLight);

            playLight = new CCSprite("anniu_chuji.png", true);
            playLight.IsVisible = false;
            playLight.BlendFunc = BlendFunc.Additive;
            var playLight_action = new CCActionSequence(new CCActionShow(), new ActionSetPosition(0), new ActionSetScale(1), new CCActionSpawn(new CCActionMoveBy(1f, 0, 20), new CCActionScaleTo(1f, 1.2f), new CCActionFadeOut(1f)), new CCActionHide(), new CCActionDelayTime(1.3f));
            playLight.RunAction(new CCActionRepeatForever(playLight_action));
            Button_play.AddNode(playLight);
            //
            var action1 = new CCActionSequence(new CCActionScaleTo(0.2f, 1.1f), new CCActionScaleTo(0.2f, 0.9f));
            var action2 = new CCActionSequence(new CCActionScaleTo(0.2f, 1.1f), new CCActionScaleTo(0.2f, 0.9f));

            scaleAction1 = new CCActionRepeatForever(action1);
            scaleAction2 = new CCActionRepeatForever(action2);

            Button_left.RunAction(scaleAction1);
            Button_right.RunAction(scaleAction2);

            Button_add.TouchBeganEvent += button =>
                {
                    Function.GoTo(UIFunction.购买体力);
                };
            //返回操作
            Button_back.TouchEndedEvent += button =>
                {
                    Console.WriteLine("Button_back");
                    GameAudio.PlayEffect(GameAudio.Effect.back);
                    Function.GoTo(UIFunction.战机选择);
                };
            //出击操作
            Button_play.TouchEndedEvent += button =>
                {
                    Console.WriteLine("Button_play：" + GameData.Instance.CurLevelName);

                    if (isSelectedLevel)
                    {
                        if (GameData.Instance.PlayerData.power > 0)
                        {
                            Function.GoTo(UIFunction.游戏中);
                            isSelectedLevel = false;
                            GameData.Instance.PlayerData.power--;

                            GuideWindow.Instance.Show = false;
                        }
                        else
                        {
                            Function.GoTo(UIFunction.购买体力);
                        }
                        this.ResetUIData();
                    }
                    else
                    {
                        Console.WriteLine("无效关卡!");
                    }
                };
            //土豪礼包
            Button_gifts.TouchBeganEvent += button =>
                {
                    Console.WriteLine("Button_gifts");
                    Function.GoTo(UIFunction.购买礼包);
                };
            //
            Button_left.TouchBeganEvent += button =>
                {
                    PageView_levels.ScrollToPage(PageView_levels.GetCurPageIndex() - 1);
                    SeletingLevelPage();
                };
            Button_right.TouchBeganEvent += button =>
                {
                    PageView_levels.ScrollToPage(PageView_levels.GetCurPageIndex() + 1);
                    SeletingLevelPage();
                };

            Button_add.TouchBeganEvent += Function.PlayButtonEffect;
            Button_play.TouchBeganEvent += Function.PlayButtonEffect;
            Button_gifts.TouchBeganEvent += Function.PlayButtonEffect;
            Button_left.TouchBeganEvent += Function.PlayButtonEffect;
            Button_right.TouchBeganEvent += Function.PlayButtonEffect;

            this.AddWidget(levelSelecting);
        }

        private void SetupMeteorites(UIImageView backgroup, string meteoriteName)
        {
            CCSprite meteorite = new CCSprite(meteoriteName, true);
            meteorite.PostionY = -backgroup.Size.height / 2 + 100 + MathHelper.Random_minus0_n(200);
            meteorite.PostionX = -backgroup.Size.width / 2 + MathHelper.Random_minus0_n(Config.SCREEN_WIDTH) - 100;
            meteorite.Rotation = MathHelper.Random_minus0_n(360);
            float time = 10f + MathHelper.Random_minus0_1() * 5f;
            float rotation = 300f + MathHelper.Random_minus0_1() * 60f;

            var move_action = new CCActionSequence(new CCActionMoveBy(time, Config.SCREEN_WIDTH + 100, 0), new ActionRestMeteortite(backgroup));
            var rotation_action = new CCActionRotateBy(time, rotation);
            var action = new CCActionSpawn(rotation_action, move_action);

            backgroup.AddNode(meteorite);
            meteorite.RunAction(new CCActionRepeatForever(action));
        }

        private void SetupPlanet(UIImageView backgroup, string planetName)
        {
            CCSprite planet = new CCSprite(planetName, true);
            planet.PostionY = -backgroup.Size.height / 2 + 250;
            planet.PostionX = -backgroup.Size.width / 2 + MathHelper.Random_minus0_n(Config.SCREEN_WIDTH) - 100;
            float time = 30f + MathHelper.Random_minus0_1() * 5f;

            var move_action = new CCActionSequence(new CCActionMoveBy(time, Config.SCREEN_WIDTH + 150, 0), new ActionSetPosition(new Vector2(-backgroup.Size.width / 2 - 50, planet.PostionY)));
            backgroup.AddNode(planet);
            planet.RunAction(new CCActionRepeatForever(move_action));
        }


        private void PageView_levels_TouchCanceledEvent(UIWidget widget)
        {
            UIPageView page = (UIPageView)widget;

        }

        public override void OnEnter()
        {
            base.OnEnter();
            //设置关卡按钮装态
            for (int i = 0; i < levelButtons.Length; i++)
            {
                UIButton button = levelButtons[i];
                LevelData levelData = (LevelData)button.UserData;
                UIImageView cover = (UIImageView)button.GetWidget("Image_isPlay");
                /*                UIImageView Image_selecting = (UIImageView)button.GetWidget("Image_selecting");*/
                if (levelData != null)
                {
                    cover.IsVisible = !levelData.isOpen;
                }

                if (i == GameData.Instance.CurLevelIndex - 1)
                {
                    //Image_selecting.IsVisible = true;
                    selsetedButton = button;
                }
            }
            //
            isSelectedLevel = true;
            LevelData curLevel = GameData.Instance.GetLevelData();
            GameData.Instance.CurLevelName = curLevel.levelName;
            PageView_levels.ScrollToPage((int)Math.Ceiling(GameData.Instance.CurLevelIndex / 3.0f) - 1);

            //
            foreach (var item in GameData.Instance.LevelDatas)
            {
                item.armatureData.PostionX = Panel_boss.Size.width / 2;
                item.armatureData.PostionY = Panel_boss.Size.height / 2;

                item.animationData.Play(item.bossAnimationName, true);
                item.armatureData.IsVisible = false;
            }
            ResetBossShow(curLevel);
            //
            SeletingLevelPage();
            //
            GuideWindow.Instance.Command = GuideCommand.关卡选择;
            GuideWindow.Instance.Show = true;
            //
            ResetUIData();
            //
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void OnEnterTransitionFinish()
        {
            base.OnEnterTransitionFinish();
        }

        public override void OnExitTransitionStart()
        {
            base.OnExitTransitionStart();
        }

        public override void OnUpdate(float dTime)
        {
            base.OnUpdate(dTime);

            attriAttack.Update(dTime);
            attriDef.Update(dTime);
            attriSpeed.Update(dTime);

            selectedEffect.Postion = selsetedButton.Postion + selsetedButton.Parent.Postion;

            this.ResetUIData();
        }

        private void LevelButtonClicked(UIWidget widget, TouchEventType eventType)
        {
            try
            {
                if (eventType == TouchEventType.TOUCH_EVENT_BEGAN)
                {
                    //Console.WriteLine("Clidked level:" + widget.Tag);
                    LevelData perLevel = GameData.Instance.GetLevelData();
                    UIButton perButton = (UIButton)perLevel.userData;

                    LevelData level = (LevelData)widget.UserData;
                    UIButton button = (UIButton)level.userData;

                    if (level.isOpen && (perLevel != level))
                    {
                        isSelectedLevel = true;
                        selsetedButton = button;

                        perLevel.armatureData.IsVisible = false;
                        perLevel.armatureData.PostionX = this.Panel_boss.Size.width / 2;
                        perLevel.armatureData.PostionY = this.Panel_boss.Size.height / 2;
                        perLevel.armatureData.StopAllAction();
                        ResetBossShow(level);
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }
        public void PageListener(int eventType)
        {
            SeletingLevelPage();
        }

        protected void SeletingLevelPage()
        {
            if (PageView_levels.GetCurPageIndex() == 0)
                Button_left.Enabled = false;
            else
                Button_left.Enabled = true;

            if (PageView_levels.GetCurPageIndex() == PageView_levels.GetChildrenCount() - 1)
                Button_right.Enabled = false;
            else
                Button_right.Enabled = true;
        }

        protected void ResetBossShow(LevelData curLevelData)
        {
            GameData.Instance.CurLevelName = curLevelData.levelName;
            GameData.Instance.CurLevelIndex = curLevelData.levelIndex;

            curLevelData.armatureData.StopAllAction();

            curLevelData.armatureData.IsVisible = true;
            curLevelData.armatureData.PostionY = this.Panel_boss.Size.height / 2 + 200;
            curLevelData.armatureData.StopAllAction();
            curLevelData.animationData.Play(curLevelData.bossAnimationName, true);
            curLevelData.armatureData.RunSequenceActions(new CCActionEaseIn(new CCActionSpawn(new CCActionFadeIn(1.0f), new CCActionMoveBy(1.0f, 0, -200)), 0.5f), new CCActionDelayTime(2.0f), new ActionPlayEffect(GameAudio.Effect.boss_change),
                new ActionPlayAnimation(curLevelData.bossArmatureName.ToLower() + "_transform", false), new CCActionDelayTime(2f), new ActionPlayAnimation(curLevelData.bossArmatureName.ToLower() + "_fly2", true));

            Label_bossName.Text = curLevelData.bossName;

            attriAttack.Attribute = curLevelData.bossPowerAttack;
            attriDef.Attribute = curLevelData.bossPowerDef;
            attriSpeed.Attribute = curLevelData.bossPowerSpeed;
        }

        public void ResetUIData()
        {
            if (PowerRecovery.Instance.Minutes < 10)
            {
                AtlasLabel_minutes.Text = "0" + Convert.ToString(PowerRecovery.Instance.Minutes);
            }
            else
            {
                AtlasLabel_minutes.Text = Convert.ToString(PowerRecovery.Instance.Minutes);
            }

            if (PowerRecovery.Instance.Seconds < 10)
            {
                AtlasLabel_seconds.Text = "0" + Convert.ToString(PowerRecovery.Instance.Seconds);
            }
            else
            {
                AtlasLabel_seconds.Text = Convert.ToString(PowerRecovery.Instance.Seconds);
            }

            AtlasLabel_power.Text = Convert.ToString(GameData.Instance.PlayerData.power);
        }
    }
}
