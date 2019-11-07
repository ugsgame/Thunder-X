using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MatrixEngine.Cocos2d;
using Thunder.GameLogic.Gaming.Actors.Players;
using MatrixEngine.Math;

namespace Thunder.GameLogic.Gaming.Actors.Drops
{
    /// <summary>
    /// 掉落管理器
    /// TODO:
    /// </summary>
    public class DropManager : CCNode,InterfaceGameState
    {
        public static int AllDrapCount;

        public static DropManager Instance = new DropManager();

        public DropManager()
        {

        }

        protected override void Dispose(bool disposing)
        {

            DropGemBlue1.Emitter.Dispose();
            DropGemBlue2.Emitter.Dispose();
            DropGemYellow1.Emitter.Dispose();
            DropGemYellow2.Emitter.Dispose();
            DropGemGreen1.Emitter.Dispose();
            DropGemGreen2.Emitter.Dispose();

            DropPower.Emitter.Dispose();
            DropShield.Emitter.Dispose();
            DropSkill.Emitter.Dispose();

            base.Dispose(disposing);
        }

        public void Init()
        {
            DropGemBlue1.Emitter.InitWithTotalDraps(100);
            DropGemBlue2.Emitter.InitWithTotalDraps(100);

            DropGemYellow1.Emitter.InitWithTotalDraps(20);
            DropGemYellow2.Emitter.InitWithTotalDraps(20);

            DropGemGreen1.Emitter.InitWithTotalDraps(10);
            DropGemGreen2.Emitter.InitWithTotalDraps(10);

            DropPower.Emitter.InitWithTotalDraps(3);
            DropShield.Emitter.InitWithTotalDraps(3);
            DropSkill.Emitter.InitWithTotalDraps(3);

            mWorldNode.AddChild(DropGemBlue1.Emitter);
            mWorldNode.AddChild(DropGemBlue2.Emitter);

            mWorldNode.AddChild(DropGemYellow1.Emitter);
            mWorldNode.AddChild(DropGemYellow2.Emitter);

            mWorldNode.AddChild(DropGemGreen1.Emitter);
            mWorldNode.AddChild(DropGemGreen2.Emitter);

            mWorldNode.AddChild(DropPower.Emitter);
            mWorldNode.AddChild(DropShield.Emitter);
            mWorldNode.AddChild(DropSkill.Emitter);
        }

        protected Player mPlayer;
        public Player Player
        {
            set
            {
                mPlayer = value;


                DropGemBlue2.Emitter.Player = value;
                DropGemBlue1.Emitter.Player = value;

                DropGemYellow1.Emitter.Player = value;
                DropGemYellow2.Emitter.Player = value;

                DropGemGreen1.Emitter.Player = value;
                DropGemGreen2.Emitter.Player = value;

                DropPower.Emitter.Player = value;
                DropShield.Emitter.Player = value;
                DropSkill.Emitter.Player = value;
            }
            get { return mPlayer; }
        }

        protected static CCNode mWorldNode;
        public static CCNode WorldNode
        {
            set { mWorldNode = value; }
            get { return mWorldNode; }
        }

        public void Drap(DropConfig config, Actor owner, DropType drapType, int num)
        {
            Drap(config, owner, drapType, num, 0);
        }

        public void Drap(DropConfig config, Actor owner, DropType drapType, int num, int numVar)
        {
            switch (drapType)
            {
                case DropType.Drop_Gem_Blue_1:
                    {
                        DropGemBlue1.Emitter.User = owner;
                        DropGemBlue1.Emitter.Config = config;
                        int _num = num - (int)(numVar * MathHelper.Random_minus0_1());
                        DropGemBlue1.Emitter.AddDrap(_num);
                    }
                    break;
                case DropType.Drop_Gem_Blue_2:
                    {
                        DropGemBlue2.Emitter.User = owner;
                        DropGemBlue2.Emitter.Config = config;
                        int _num = num - (int)(numVar * MathHelper.Random_minus0_1());
                        DropGemBlue2.Emitter.AddDrap(_num);
                    }
                    break;
                case DropType.Drop_Gem_Yellow_1:
                    {
                        DropGemYellow1.Emitter.User = owner;
                        DropGemYellow1.Emitter.Config = config;
                        int _num = num - (int)(numVar * MathHelper.Random_minus0_1());
                        DropGemYellow1.Emitter.AddDrap(_num);
                    }
                    break;
                case DropType.Drop_Gem_Yellow_2:
                    {
                        DropGemYellow2.Emitter.User = owner;
                        DropGemYellow2.Emitter.Config = config;
                        int _num = num - (int)(numVar * MathHelper.Random_minus0_1());
                        DropGemYellow2.Emitter.AddDrap(_num);
                    }
                    break;
                case DropType.Drop_Gem_Green_1:
                    {
                        DropGemGreen1.Emitter.User = owner;
                        DropGemGreen1.Emitter.Config = config;
                        int _num = num - (int)(numVar * MathHelper.Random_minus0_1());
                        DropGemGreen1.Emitter.AddDrap(_num);
                    }
                    break;
                case DropType.Drop_Gem_Green_2:
                    {
                        DropGemGreen2.Emitter.User = owner;
                        DropGemGreen2.Emitter.Config = config;
                        int _num = num - (int)(numVar * MathHelper.Random_minus0_1());
                        DropGemGreen2.Emitter.AddDrap(_num);
                    }
                    break;
                case DropType.Drop_Power:
                    {
                        DropPower.Emitter.User = owner;
                        DropPower.Emitter.Config = config;
                        int _num = num - (int)(numVar * MathHelper.Random_minus0_1());
                        DropPower.Emitter.AddDrap(_num);
                    }
                    break;
                case DropType.Drop_Shield:
                    {
                        Console.WriteLine("Drop_Shield");
                        DropShield.Emitter.User = owner;
                        DropShield.Emitter.Config = config;
                        int _num = num - (int)(numVar * MathHelper.Random_minus0_1());
                        DropShield.Emitter.AddDrap(_num);
                    }
                    break;
                case DropType.Drop_Skill:
                    {
                        Console.WriteLine("DropSkill");
                        DropSkill.Emitter.User = owner;
                        DropSkill.Emitter.Config = config;
                        int _num = num - (int)(numVar * MathHelper.Random_minus0_1());
                        DropSkill.Emitter.AddDrap(_num);
                    }
                    break;
                default:
                    break;
            }
        }

        public void DrapEnemy(Actor owner, DropType drapType, int num)
        {
            DrapEnemy(owner, drapType, num, 0);
        }
        public void DrapEnemy(Actor owner, DropType drapType, int num, int numVar)
        {
            DropConfig config = new DropConfig();
            config.speed = 300;
            config.speedVar = 100;
            config.speedDecay = -10;
            config.speedLimit = 50;
            config.angle = 0;
            config.angleVar = 360;
            config.waitingTime = 2;
            config.waitingTimeVar = 0.5f;

            Drap(config, owner, drapType, num, numVar);
        }

        public void DrapPlayer(Actor owner, DropType drapType, int num)
        {
            DrapEnemy(owner, drapType, num, 0);
        }
        public void DrapPlayer(Actor owner, DropType drapType, int num, int numVar)
        {
            DropConfig config = new DropConfig();
            config.speed = 400;
            config.speedVar = 0;
            config.speedDecay = -10;
            config.speedLimit = 50;
            config.angle = 0;
            config.angleVar = 360;
            config.waitingTime = 5;
            config.waitingTimeVar = 0.5f;

            Drap(config, owner, drapType, num, numVar);
        }

        public void DrapBoss(Actor owner, DropType drapType, int num)
        {
            DrapBoss(owner, drapType, num, 0);
        }
        public void DrapBoss(Actor owner, DropType drapType, int num, int numVar)
        {
            DropConfig config = new DropConfig();
            config.speed = 500;
            config.speedVar = 200;
            config.speedDecay = -10;
            config.speedLimit = 50;
            config.angle = 0;
            config.angleVar = 360;
            config.waitingTime = 3;
            config.waitingTimeVar = 0.5f;

            Drap(config, owner, drapType, num, numVar);
        }

        public void OnPause()
        {
            this.PauseSchedulerAndActions();

            DropGemBlue1.Emitter.PauseSchedulerAndActions();
            DropGemBlue2.Emitter.PauseSchedulerAndActions();

            DropGemYellow1.Emitter.PauseSchedulerAndActions();
            DropGemYellow2.Emitter.PauseSchedulerAndActions();

            DropGemGreen1.Emitter.PauseSchedulerAndActions();
            DropGemGreen2.Emitter.PauseSchedulerAndActions();

            DropPower.Emitter.PauseSchedulerAndActions();
            DropShield.Emitter.PauseSchedulerAndActions();
            DropSkill.Emitter.PauseSchedulerAndActions();
        }

        public void OnResume()
        {
            this.ResumeSchedulerAndActions();

            DropGemBlue1.Emitter.ResumeSchedulerAndActions();
            DropGemBlue2.Emitter.ResumeSchedulerAndActions();

            DropGemYellow1.Emitter.ResumeSchedulerAndActions();
            DropGemYellow2.Emitter.ResumeSchedulerAndActions();

            DropGemGreen1.Emitter.ResumeSchedulerAndActions();
            DropGemGreen2.Emitter.ResumeSchedulerAndActions();

            DropPower.Emitter.ResumeSchedulerAndActions();
            DropShield.Emitter.ResumeSchedulerAndActions();
            DropSkill.Emitter.ResumeSchedulerAndActions();
        }
    }
}
