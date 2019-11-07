using MatrixEngine.Cocos2d;
using MatrixEngine.CocoStudio.GUI;
using MatrixEngine.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thunder.Common;
using Thunder.Game;
using Thunder.GameLogic.Common;
using Thunder.GameLogic.GameSystem;
using Thunder.GameLogic.Gaming;

namespace Thunder.GameLogic.UI.Guide
{
    public class GuideWindow : UILayer
    {
        public delegate void StrikeTouchEventHandle(float x, float y);

        public event StrikeTouchEventHandle TouchBeganEvent;
        public event StrikeTouchEventHandle TouchCanceledEvent;
        public event StrikeTouchEventHandle TouchEndedEvent;
        public event StrikeTouchEventHandle TouchMovedEvent;

        protected UILayout backGroundLeft = new UILayout();
        protected UILayout backGroundRight = new UILayout();
        protected UILayout backGroundTop = new UILayout();
        protected UILayout backGroundBottom = new UILayout();

        CCSprite res_Npc;
        CCSprite res_Arrow;

        GuideData guideData;
        GuideText guideText;

        Window guideAgent;
        //
        开始游戏 c开始游戏;
        战机选择 c战机选择;
        关卡选择 c关卡选择;
        跳到升级 c跳到升级;
        点击升级 c点击升级;
        升级返回 c升级返回;
        使用护盾 c使用护盾;
        使用技能 c使用技能;
        //
        public static GuideWindow Instance;

        public GuideWindow()
        {
            backGroundLeft.ContextSize = Config.ScreenSize;
            backGroundLeft.TouchEnabled = false;
            backGroundLeft.Size = Config.ScreenSize;
            backGroundLeft.SetBackGroundColorType(LayoutBackGroundColorType.LAYOUT_COLOR_SOLID);
            backGroundLeft.SetBackGroundColorOpacity(200);
            backGroundLeft.SetBackGroundColorS(Color32.Black);
            this.AddChild(backGroundLeft);

            backGroundRight.ContextSize = Config.ScreenSize;
            backGroundRight.TouchEnabled = false;
            backGroundRight.Size = Config.ScreenSize;
            backGroundRight.SetBackGroundColorType(LayoutBackGroundColorType.LAYOUT_COLOR_SOLID);
            backGroundRight.SetBackGroundColorOpacity(200);
            backGroundRight.SetBackGroundColorS(Color32.Black);
            this.AddChild(backGroundRight);

            backGroundTop.ContextSize = Config.ScreenSize;
            backGroundTop.TouchEnabled = false;
            backGroundTop.Size = Config.ScreenSize;
            backGroundTop.SetBackGroundColorType(LayoutBackGroundColorType.LAYOUT_COLOR_SOLID);
            backGroundTop.SetBackGroundColorOpacity(200);
            backGroundTop.SetBackGroundColorS(Color32.Black);
            this.AddChild(backGroundTop);

            backGroundBottom.ContextSize = Config.ScreenSize;
            backGroundBottom.TouchEnabled = false;
            backGroundBottom.Size = Config.ScreenSize;
            backGroundBottom.SetBackGroundColorType(LayoutBackGroundColorType.LAYOUT_COLOR_SOLID);
            backGroundBottom.SetBackGroundColorOpacity(200);
            backGroundBottom.SetBackGroundColorS(Color32.Black);
            this.AddChild(backGroundBottom);


            guideAgent = new Window();

            //
            res_Npc = new CCSprite("npc.png", true);
            res_Arrow = new CCSprite("yindaojiantou.png", true);
            guideText = new GuideText();

            this.AddChild(res_Npc);
            this.AddChild(res_Arrow);
            this.AddChild(guideText);
            //
            c开始游戏 = new 开始游戏();
            c战机选择 = new 战机选择();
            c关卡选择 = new 关卡选择();
            c跳到升级 = new 跳到升级();
            c点击升级 = new 点击升级();
            c升级返回 = new 升级返回();
            c使用护盾 = new 使用护盾();
            c使用技能 = new 使用技能();
            //
            guideAgent.AddNode(this);
            //
            Instance = this;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            LayerTouchEnable = true;
            SetTouchMode(TouchMode.Single);
            SetSwallowsTouches(true);
        }

        public bool LayerTouchEnable;

        public override bool OnTouchBegan(float x, float y)
        {
            if (!LayerTouchEnable) return true;
            if (guideData.touchRect.ContainsPoint(new Vector2(x, y)))
                return false;
            if (TouchBeganEvent != null)
                TouchBeganEvent(x, y);
            return true;
        }

        public override void OnTouchEnded(float x, float y)
        {
            if (!LayerTouchEnable) return;
            if (!guideData.touchRect.ContainsPoint(new Vector2(x, y)))
                return;
            if (TouchEndedEvent != null)
                TouchEndedEvent(x, y);
        }

        public override void OnTouchMoved(float x, float y)
        {
            if (!LayerTouchEnable) return;
            if (!guideData.touchRect.ContainsPoint(new Vector2(x, y)))
                return;
            if (TouchMovedEvent != null)
                TouchMovedEvent(x, y);
        }

        public override void OnTouchCancelled(float x, float y)
        {
            if (!LayerTouchEnable) return;
            if (TouchCanceledEvent != null)
                TouchCanceledEvent(x, y);
        }

        private GuideCommand command;
        public virtual GuideCommand Command
        {
            set
            {
                command = value;
                switch (command)
                {
                    case GuideCommand.开始游戏:
                        guideData = c开始游戏;
                        break;
                    case GuideCommand.战机选择:
                        guideData = c战机选择;
                        break;
                    case GuideCommand.关卡选择:
                        guideData = c关卡选择;
                        break;
                    case GuideCommand.使用护盾:
                        guideData = c使用护盾;
                        break;
                    case GuideCommand.使用技能:
                        guideData = c使用技能;
                        break;
                    case GuideCommand.跳到升级:
                        guideData = c跳到升级;
                        break;
                    case GuideCommand.点击升级:
                        guideData = c点击升级;
                        break;
                    case GuideCommand.升级返回:
                        guideData = c升级返回;
                        break;
                    default:
                        break;
                }
                this.ResetGuide();
            }
            get
            {
                return command;
            }
        }

        public GuideData GetGuideData(GuideCommand _Command)
        {
            GuideData data = null;
            switch (_Command)
            {
                case GuideCommand.开始游戏:
                    data = c开始游戏;
                    break;
                case GuideCommand.战机选择:
                    data = c战机选择;
                    break;
                case GuideCommand.关卡选择:
                    data = c关卡选择;
                    break;
                case GuideCommand.使用护盾:
                    data = c使用护盾;
                    break;
                case GuideCommand.使用技能:
                    data = c使用技能;
                    break;
                case GuideCommand.跳到升级:
                    data = c跳到升级;
                    break;
                case GuideCommand.点击升级:
                    data = c点击升级;
                    break;
                case GuideCommand.升级返回:
                    data = c升级返回;
                    break;
                default:
                    break;
            }
            return data;
        }

        private void ResetGuide()
        {
            if (guideData.npcDir)
            {
                res_Npc.ScaleX = 1;
            }
            else
            {
                res_Npc.ScaleX = -1;
            }
            res_Npc.Postion = guideData.npcPos;

            if (guideData.arrowDir)
            {
                res_Arrow.ScaleY = 1;
            }
            else
            {
                res_Arrow.ScaleY = -1;
            }
            res_Arrow.Postion = guideData.arrowPos;

            guideText.Text = guideData.guideText;
            guideText.Postion = guideData.guidePos;

            backGroundLeft.AnchorPoint = new Vector2(0, 0f);
            backGroundLeft.Postion = 0;
            backGroundLeft.Size = new Size(guideData.touchRect.origin.X, Config.ScreenSize.height);

            backGroundRight.AnchorPoint = new Vector2(0, 0f);
            backGroundRight.Postion = new Vector2(guideData.touchRect.size.width + guideData.touchRect.origin.X, 0);
            backGroundRight.Size = new Size(Config.ScreenSize.width, Config.ScreenSize.height);

            backGroundBottom.AnchorPoint = new Vector2(0f, 1f);
            backGroundBottom.Postion = new Vector2(guideData.touchRect.origin.X, guideData.touchRect.origin.Y);
            backGroundBottom.Size = new Size(guideData.touchRect.size.width, guideData.touchRect.origin.Y);

            backGroundTop.AnchorPoint = new Vector2(0f, 0f);
            backGroundTop.Postion = new Vector2(guideData.touchRect.origin.X, guideData.touchRect.origin.Y + guideData.touchRect.size.height);
            backGroundTop.Size = new Size(guideData.touchRect.size.width, Config.ScreenSize.height);
        }

        private bool show;
        public virtual bool Show
        {
            set
            {
                show = value;
                if (show)
                {
                    if (!guideData.IsPlay && this.command != GuideCommand.Null)
                    {
                        this.IsVisible = true;
                        this.LayerTouchEnable = true;

                        res_Arrow.StopAllAction();
                        res_Arrow.RunAction(new CCActionRepeatForever(new CCActionSequence(new CCActionMoveBy(0.3f, 0, 30), new CCActionMoveBy(0.3f, 0, -30))));

                        if (guideData.npcAction != null)
                        {
                            res_Npc.RunAction(guideData.npcAction);
                        }
                        if (guideData.guideAction != null)
                        {
                            guideText.RunAction(guideData.guideAction);
                        }

                        this.guideAgent.Show(true);

                        if (GameData.Instance.GameState == GameState.Playing)
                        {
                            PlayingScene.Instance.OnPause();
                        }
                    }
                    else
                    {
                        show = false;
                    }
                }
                else
                {
                    this.IsVisible = false;
                    this.LayerTouchEnable = false;

                    this.guideAgent.Show(false);

                    if (!guideData.IsPlay)guideData.IsPlay = true;

                    if (GameData.Instance.GameState == GameState.Playing)
                    {
                        PlayingScene.Instance.OnResume();
                    }
                }

            }
            get
            {
                return show;
            }

        }
    }
}
