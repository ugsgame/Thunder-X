using MatrixEngine.Cocos2d;
using MatrixEngine.CocoStudio.GUI;
using MatrixEngine.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thunder.Common;
using Thunder.GameLogic.Common;

namespace Thunder.GameLogic.UI.Guide
{
    public class GuideData : CCNode
    {
        public Rect touchRect;
        public Vector2 arrowPos;
        public bool arrowDir;

        public Vector2 npcPos;
        public bool npcDir;
        public CCAction npcAction;

        public Vector2 guidePos;
        public string guideText;
        public CCAction guideAction;

        private bool isPlay;
        public virtual bool IsPlay
        {
            get { return isPlay; }
            set { isPlay = value; }
        }

        public GuideCommand command;

        public GuideData()
        {

        }

        public virtual GuideCommand Command
        {
            get
            {
                return command;
            }
        }
    }
    /// <summary>
    /// 开始游戏引导
    /// </summary>
    public class 开始游戏 : GuideData
    {
        public 开始游戏()
        {
            touchRect = new Rect(Config.SCREEN_PosX_Center - 100, 450, 200, 60);

            arrowPos = new Vector2(Config.SCREEN_PosX_Center, 380);
            arrowDir = true;

            npcPos = new Vector2(600, 200);
            npcDir = false;
            npcAction = new CCActionEaseIn(new CCActionMoveTo(1.0f, 380, 200), 0.3f); 

            guidePos = new Vector2(-100, 320);
            guideAction = new CCActionEaseIn(new CCActionMoveTo(1.0f, 200, 320), 0.3f); 
            guideText = "点击开始游戏！";

            command = GuideCommand.开始游戏;
        }

        public override bool IsPlay
        {
            get
            {
                return GameData.Instance.GuideData.开始游戏;
            }
            set
            {
                base.IsPlay = value;
                GameData.Instance.GuideData.开始游戏 = value;
            }
        }
    }

    public class 战机选择 : GuideData
    {
        public 战机选择()
        {
            touchRect = new Rect(Config.SCREEN_PosX_Center - 100, 10, 200, 60);

            arrowPos = new Vector2(Config.SCREEN_PosX_Center, 100);
            arrowDir = false;

            npcPos = new Vector2(600, 200);
            npcDir = false;
            npcAction = new CCActionEaseIn(new CCActionMoveTo(1.0f,400,200),0.3f);

            guidePos = new Vector2(-100, 320);
            guideAction = new CCActionEaseIn(new CCActionMoveTo(1.0f, 180, 320), 0.3f);
            guideText = "点击出击进入关卡界面";

            command = GuideCommand.战机选择;
        }
        public override bool IsPlay
        {
            get
            {
                return GameData.Instance.GuideData.战机选择;
            }
            set
            {
                base.IsPlay = value;
                GameData.Instance.GuideData.战机选择 = value;
            }
        }
    }

    public class 关卡选择 : GuideData
    {
        public 关卡选择()
        {
            touchRect = new Rect(Config.SCREEN_PosX_Center - 100, 10, 200, 60);

            arrowPos = new Vector2(Config.SCREEN_PosX_Center, 100);
            arrowDir = false;

            npcPos = new Vector2(600, 200);
            npcDir = false;
            npcAction = new CCActionEaseIn(new CCActionMoveTo(1.0f, 400, 200), 0.3f);

            guidePos = new Vector2(-100, 320);
            guideAction = new CCActionEaseIn(new CCActionMoveTo(1.0f, 200, 320), 0.3f);
            guideText = "点击出击进入战斗";

            command = GuideCommand.关卡选择;
        }

        public override bool IsPlay
        {
            get
            {
                return GameData.Instance.GuideData.关卡选择;
            }
            set
            {
                GameData.Instance.GuideData.关卡选择 = value;
                base.IsPlay = value;
            }
        }
    }

    public class 跳到升级 : GuideData
    {
        public 跳到升级()
        {
            touchRect = new Rect(Config.SCREEN_PosX_Center - 210, 42, 200, 70);

            arrowPos = new Vector2(Config.SCREEN_PosX_Center - 110, 150);
            arrowDir = false;

            npcPos = new Vector2(600, 200);
            npcDir = false;
            npcAction = new CCActionEaseIn(new CCActionMoveTo(0.5f, 400, 200), 0.3f);

            guidePos = new Vector2(-100, 320);
            guideAction = new CCActionEaseIn(new CCActionMoveTo(0.5f, 200, 320), 0.3f);
            guideText = "点击进入战机升级界面";

            command = GuideCommand.跳到升级;
        }

        public override bool IsPlay
        {
            get
            {
                return GameData.Instance.GuideData.跳到升级;
            }
            set
            {
                base.IsPlay = value;
                GameData.Instance.GuideData.跳到升级 = value;
            }
        }
    }

    public class 点击升级 : GuideData
    {
        public 点击升级()
        {
            touchRect = new Rect(Config.SCREEN_PosX_Center - 115, 20, 146, 65);

            arrowPos = new Vector2(Config.SCREEN_PosX_Center - 50, 120);
            arrowDir = false;

            npcPos = new Vector2(600, 200);
            npcDir = false;
            npcAction = new CCActionEaseIn(new CCActionMoveTo(1.0f, 400, 200), 0.3f);

            guidePos = new Vector2(-100, 320);
            guideAction = new CCActionEaseIn(new CCActionMoveTo(1.0f, 200, 320), 0.3f);
            guideText = "点击提升战机属性";

            command = GuideCommand.点击升级;
        }
        public override bool IsPlay
        {
            get
            {
                return GameData.Instance.GuideData.点击升级;
            }
            set
            {
                base.IsPlay = value;
                GameData.Instance.GuideData.点击升级 = value;
            }
        }
    }

    public class 升级返回 : GuideData
    {
        public 升级返回()
        {
            touchRect = new Rect(20, 16, 80, 80);

            arrowPos = new Vector2(50, 120);
            arrowDir = false;

            npcPos = new Vector2(400, 200);
            npcDir = false;
            //npcAction = new CCActionEaseIn(new CCActionMoveTo(1.0f, 400, 200), 0.3f);

            guidePos = new Vector2(200, 320);
            //guideAction = new CCActionEaseIn(new CCActionMoveTo(1.0f, 200, 320), 0.3f);
            guideText = "反回战斗吧！";

            command = GuideCommand.升级返回;
        }
        public override bool IsPlay
        {
            get
            {
                return GameData.Instance.GuideData.升级返回;
            }
            set
            {
                base.IsPlay = value;
                GameData.Instance.GuideData.升级返回 = value;
            }
        }
    }

    public class 使用护盾 : GuideData
    {
        public 使用护盾()
        {
            touchRect = new Rect(Config.SCREEN_WIDTH - 90, 14, 80, 80);

            arrowPos = new Vector2(Config.SCREEN_WIDTH - 50, 120);
            arrowDir = false;

            npcPos = new Vector2(-100, 200);
            npcDir = true;
            npcAction = new CCActionEaseIn(new CCActionMoveTo(0.5f, 100, 200), 0.3f);

            guidePos = new Vector2(600, 320);
            guideAction = new CCActionEaseIn(new CCActionMoveTo(0.5f, 300, 320), 0.3f);
            guideText = "点击使用强力护盾！";

            command = GuideCommand.升级返回;
        }
        public override bool IsPlay
        {
            get
            {
                return GameData.Instance.GuideData.使用护盾;
            }
            set
            {
                base.IsPlay = value;
                GameData.Instance.GuideData.使用护盾 = value;
            }
        }
    }

    public class 使用技能 : GuideData
    {
        public 使用技能()
        {
            touchRect = new Rect(5, 14, 80, 80);

            arrowPos = new Vector2(50, 120);
            arrowDir = false;

            npcPos = new Vector2(600, 200);
            npcDir = false;
            npcAction = new CCActionEaseIn(new CCActionMoveTo(0.5f, 400, 200), 0.3f);

            guidePos = new Vector2(-100, 320);
            guideAction = new CCActionEaseIn(new CCActionMoveTo(0.5f, 200, 320), 0.3f);
            guideText = "点击使用高空轰炸！";

            command = GuideCommand.使用技能;
        }
        public override bool IsPlay
        {
            get
            {
                return GameData.Instance.GuideData.使用技能;
            }
            set
            {
                base.IsPlay = value;
                GameData.Instance.GuideData.使用技能 = value;
            }
        }
    }
}
