using MatrixEngine.Cocos2d;
using MatrixEngine.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thunder.GameLogic.Common;
using Thunder.GameLogic.Gaming.Actors;
using Thunder.GameLogic.Gaming.Actors.Players;
using Thunder.GameLogic.Gaming.Actors.Wingmen;

namespace Thunder.GameLogic.Gaming
{
    public class WingmanSpawner : InterfaceGameState
    {
        public enum WingmanID
        {
            Wingman1 = ActorID.Wingman1,
            Wingman2 = ActorID.Wingman2,
            Wingman3 = ActorID.Wingman3,
            Wingman4 = ActorID.Wingman4,
            Null,
        }

        class WingmanGroup : InterfaceGameState
        {
            struct WingmanBooth
            {
                public Wingman left;
                public Wingman right;

                public Player Player
                {
                    set
                    {
                        left.Player = value;
                        right.Player = value;
                    }
                }

                public void PosOffset(Vector2 rightPos)
                {
                    right.PosOffset = rightPos;
                    left.PosOffset = new Vector2(-rightPos.X, rightPos.Y);
                }

                public float Speed
                {
                    set
                    {
                        right.Speed = value;
                        left.Speed = value;
                    }
                }

                public void Open()
                {
                    left.Open();
                    right.Open();
                }
                public void Close()
                {
                    left.Close();
                    right.Close();
                }
                public void Reset()
                {
                    left.Reset();
                    right.Reset();
                }
                public bool Frenzy
                {
                    set
                    {
                        if (value)
                        {
                            left.GotoState(Actor.State.Frenzy);
                        }
                        else
                        {
                            left.GotoState(Actor.State.Idle);
                        }
                    }
                }

                public void FlyOut()
                {
                    left.FlyOut(true);
                    right.FlyOut(false);
                }

                public void OnPause()
                {
                    left.PauseSchedulerAndActions();
                    right.PauseSchedulerAndActions();
                }

                public void OnResume()
                {
                    left.ResumeSchedulerAndActions();
                    right.ResumeSchedulerAndActions();
                }

                public bool IsShowing
                {
                    set
                    {
                        left.IsShowing = value;
                        right.IsShowing = value;
                    }
                }

                public void OnLevelUp(int level)
                {
                    left.OnLevelUp(level);
                    right.OnLevelUp(level);
                }

                public void OpenFire()
                {
                    left.OpenFire();
                    right.OpenFire();
                }
                public void CloseFire()
                {
                    left.CloseFire();
                    right.CloseFire();
                }
            }

            readonly static int boothCount = 3;
            Vector2[] posOffsets = new Vector2[boothCount];
            float[] speeds = new float[boothCount];
            WingmanBooth[] wingmanBoths = new WingmanBooth[boothCount];

            int curLevel;

            public WingmanGroup(WingmanID id)
            {
                posOffsets[0] = new Vector2(80, 0);
                posOffsets[1] = new Vector2(70, -50);
                posOffsets[2] = new Vector2(45, -80);

                speeds[0] = 1500;
                speeds[1] = 1500;
                speeds[2] = 1500;

                for (int i = 0; i < wingmanBoths.Length; i++)
                {
                    wingmanBoths[i] = new WingmanBooth();
                }
                switch (id)
                {
                    case WingmanID.Wingman1:
                        {
                            for (int i = 0; i < wingmanBoths.Length; i++)
                            {
                                wingmanBoths[i].right = new Wingman1(comInfo.Clone());
                                wingmanBoths[i].left = new Wingman1(comInfo.Clone());
                            }
                        }
                        break;
                    case WingmanID.Wingman2:
                        {
                            for (int i = 0; i < wingmanBoths.Length; i++)
                            {
                                wingmanBoths[i].right = new Wingman2(comInfo.Clone());
                                wingmanBoths[i].left = new Wingman2(comInfo.Clone());
                            }
                        }
                        break;
                    case WingmanID.Wingman3:
                        {
                            for (int i = 0; i < wingmanBoths.Length; i++)
                            {
                                wingmanBoths[i].right = new Wingman3(comInfo.Clone());
                                wingmanBoths[i].left = new Wingman3(comInfo.Clone());
                            }
                        }
                        break;
                    case WingmanID.Wingman4:
                        {
                            for (int i = 0; i < wingmanBoths.Length; i++)
                            {
                                wingmanBoths[i].right = new Wingman4(comInfo.Clone());
                                wingmanBoths[i].left = new Wingman4(comInfo.Clone());
                            }
                        }
                        break;
                    default:
                        break;
                }

                for (int i = 0; i < wingmanBoths.Length; i++)
                {
                    wingmanBoths[i].PosOffset(posOffsets[i]);
                    wingmanBoths[i].Speed = speeds[i];
                }

            }

            public Player Player
            {
                set
                {
                    for (int i = 0; i < wingmanBoths.Length; i++)
                    {
                        wingmanBoths[i].right.Player = value;
                        wingmanBoths[i].left.Player = value;
                    }
                }
            }

            public void AddToWorld(CCNode world)
            {
                for (int i = 0; i < wingmanBoths.Length; i++)
                {
                    world.AddChild(wingmanBoths[i].right);
                    world.AddChild(wingmanBoths[i].left);
                }
            }

            public void RemoveFromWorld(CCNode world)
            {
                for (int i = 0; i < wingmanBoths.Length; i++)
                {
                    world.RemoveChild(wingmanBoths[i].right);
                    world.RemoveChild(wingmanBoths[i].left);
                }
            }

            public int Count
            {
                set
                {
                    if (value <= 0)
                    {
                        curLevel = 0;
                    }
                    else if (value >= boothCount)
                    {
                        curLevel = boothCount;
                    }
                    else
                    {
                        curLevel = value;
                    }

                    for (int i = 0; i < boothCount; i++)
                    {
                        if (i < curLevel)
                        {
                            wingmanBoths[i].Open();
                        }
                        else
                        {
                            wingmanBoths[i].Close();
                        }
                    }

                }
                get
                {
                    return curLevel;
                }
            }

            public void Reset()
            {
                for (int i = 0; i < wingmanBoths.Length; i++)
                {
                    wingmanBoths[i].Reset();
                }
            }

            public void FlyOut()
            {
                for (int i = 0; i < wingmanBoths.Length; i++)
                {
                    wingmanBoths[i].FlyOut();
                }
            }



            public void OnPause()
            {
                for (int i = 0; i < wingmanBoths.Length; i++)
                {
                    wingmanBoths[i].OnPause();
                }
            }

            public void OnResume()
            {
                for (int i = 0; i < wingmanBoths.Length; i++)
                {
                    wingmanBoths[i].OnResume();
                }
            }

            public bool IsShowing
            {
                set
                {
                    for (int i = 0; i < wingmanBoths.Length; i++)
                    {
                        wingmanBoths[i].IsShowing = value;
                    }
                }
            }

            public void OnLevelUp(int level)
            {
                for (int i = 0; i < wingmanBoths.Length; i++)
                {
                    wingmanBoths[i].OnLevelUp(level);
                }
            }

            public void OpenFire()
            {
                for (int i = 0; i < wingmanBoths.Length; i++)
                {
                    wingmanBoths[i].OpenFire();
                }
            }
            public void CloseFire()
            {
                for (int i = 0; i < wingmanBoths.Length; i++)
                {
                    wingmanBoths[i].CloseFire();
                }
            }
        }

        List<WingmanGroup> wingmanGroups = new List<WingmanGroup>();

        Player player;
        public Player Player
        {
            set
            {
                player = value;

                foreach (var item in wingmanGroups)
                {
                    item.Player = value;
                }
            }
            get { return player; }
        }

        static SpawnInfo comInfo;

        WingmanGroup curWingman;


        public static WingmanSpawner Instance;

        public WingmanSpawner()
        {
            comInfo = new SpawnInfo();
            comInfo.resPath = ResID.Armatures_Player;
            comInfo.armaName = "Player";
            comInfo.speed = 1000;

            wingmanGroups.Add(new WingmanGroup(WingmanID.Wingman1));
            wingmanGroups.Add(new WingmanGroup(WingmanID.Wingman2));
            wingmanGroups.Add(new WingmanGroup(WingmanID.Wingman3));
            wingmanGroups.Add(new WingmanGroup(WingmanID.Wingman4));

            curWingman = wingmanGroups[0];
        }

        public void SetCurWingman(WingmanID id)
        {
            switch (id)
            {
                case WingmanID.Wingman1:
                    curWingman = wingmanGroups[0];
                    break;
                case WingmanID.Wingman2:
                    curWingman = wingmanGroups[1];
                    break;
                case WingmanID.Wingman3:
                    curWingman = wingmanGroups[2];
                    break;
                case WingmanID.Wingman4:
                    curWingman = wingmanGroups[3];
                    break;
                default:
                    break;
            }
            GameData.Instance.PlayerData.curWingman = id;
        }

        CCNode worldNode;
        public void AddToWorld(CCNode world)
        {
            worldNode = world;
            //curWingman.AddToWorld(world);
            foreach (var item in wingmanGroups)
            {
                item.AddToWorld(world);
            }
        }

        public void RemvoeFromWorld(CCNode world)
        {
            //curWingman.RemoveFromWorld(world);
            foreach (var item in wingmanGroups)
            {
                item.RemoveFromWorld(world);
            }
        }

        public void RemoveFromWorld()
        {
            if (worldNode != null)
            {
                foreach (var item in wingmanGroups)
                {
                    item.RemoveFromWorld(worldNode);
                }
            }
        }

        public int Count
        {
            set { curWingman.Count = value; }
            get { return curWingman.Count; }
        }

        public void Reset()
        {
            foreach (var item in wingmanGroups)
            {
                item.Reset();
            }
        }

        public void FlyOut()
        {
            curWingman.FlyOut();
        }

        public void OnPause()
        {

            curWingman.OnPause();

        }

        public void OnResume()
        {

            curWingman.OnResume();
        }

        public bool IsShowing
        {
            set
            {
                foreach (var item in wingmanGroups)
                {
                    item.IsShowing = value;
                }
            }
        }

        public void OnLevelUp(int level)
        {

            curWingman.OnLevelUp(level);

        }

        public void CloseFire()
        {
            curWingman.CloseFire();
        }

        public void OpenFire()
        {
            curWingman.OpenFire();
        }
    }
}
