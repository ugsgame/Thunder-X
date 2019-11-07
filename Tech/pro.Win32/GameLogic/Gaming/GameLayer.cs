using System;

using MatrixEngine.Cocos2d;
using MatrixEngine.Math;
using Thunder.Common;
using Thunder.GameLogic.Gaming.Actors.Players;
using Thunder.GameLogic.Common;
using Thunder.GameLogic.Gaming.Actors.Bosses;
using Thunder.GameLogic.Gaming.Actors.Enemys;
using Thunder.GameLogic.Gaming.BulletSystems;
using MatrixEngine.CocoStudio.Armature;
using Thunder.GameLogic.Gaming.Actors;
using Thunder.Game;
using Thunder.GameLogic.Gaming.Actors.Drops;
using Thunder.GameLogic.Gaming.Actors.Wingmen;

namespace Thunder.GameLogic.Gaming
{
    /// <summary>
    /// 游戏图层
    /// </summary>
    public class GameLayer : CCLayer,InterfaceGameState
    {
        /// 触点与玩家的当前距离
        private Vector2 touchDis = new Vector2();
        /// 屏幕触点
        private Vector2 touchPoint = new Vector2();
        /// 游戏中的视点目标
        private Vector2 aimPoint = new Vector2();
        /// 视差值
        private static float parallax;
        /// 视差比
        private static float veiwPercent;
        private float oldAimX;

        //
        private Player player;  //当前玩家
        private Boss boss;      //当前boss

       public GameLayer()
        {
            this.ContextSize = new Size(Config.GAME_WIDTH, Config.SCREEN_HEIGHT);
            parallax = this.ContextSize.width - Config.SCREEN_WIDTH;

            SpriteBatchManager.Instance.WorldNode = this;
            //注册发射器的碰撞器
            BulletEmitter.BindingCollider("RectHurt1", "RectHurt2","RectHurt3");
            BulletEmitter.WorldNode = this;
            //
            DropManager.WorldNode = this;
            DropManager.Instance.Init();

        }

        public override void OnEnter()
        {
            base.OnEnter();
            //
            BulletEmitter.WorldNode = this;
            DropManager.WorldNode = this;
            //

            this.SetTouchMode(TouchMode.Single);
            this.SetState(LayerState.Touch, true);

            PlayerSpawner.Instanse.PlayerLayer = this;
            PlayerSpawner.Instanse.IsShowing = false;
            foreach (var item in PlayerSpawner.Instanse.AllPlayer())
            {
                item.GotoBehavior(Player.Behavior.Null);
                item.RemoveFromParent();
            }
            WingmanSpawner.Instance.RemoveFromWorld();

            player = PlayerSpawner.Instanse.CurPlayer;
            player.StayPoint = new Vector2(this.ContextSize.width / 2, this.ContextSize.height/4);
            player.InitPoint = new Vector2(this.ContextSize.width / 2, -200);
            player.Postion = player.InitPoint;
            PlayerSpawner.Instanse.ActivatePlayer();
            //
            aimPoint = player.StayPoint;
            oldAimX = aimPoint.X;
          
            WingmanSpawner.Instance.Player = player;
            WingmanSpawner.Instance.IsShowing = false;
            WingmanSpawner.Instance.AddToWorld(this);
            WingmanSpawner.Instance.Count = GameData.Instance.PlayerData.withWingman ? 1 : 0;

            this.AddChild(player);         
            ResurrectPlayer();

            //this.Schedule("ScheduleSpeed", 1);
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        //复活战机
        public void ResurrectPlayer()
        {
            aimPoint = player.StayPoint;
            oldAimX = aimPoint.X;
            player.Info.HP = PlayingScene.Instance.CurFighterData.hp + PlayingScene.Instance.CurFighterLevelData.attachHP;
            player.FlyIn();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        public void OnPause()
        {
            this.player.PauseSchedulerAndActions();
            WingmanSpawner.Instance.OnPause();
            DropManager.Instance.OnPause();
        }

        public void OnResume()
        {
            this.player.ResumeSchedulerAndActions();
            WingmanSpawner.Instance.OnResume();
            DropManager.Instance.OnResume();
        }

        public override bool OnTouchBegan(float x, float y)
        {
            touchPoint.X = x;
            touchPoint.Y = y;

            //转成游戏坐标系
            this.touchPoint = this.touchPoint / Config.SCREEN_RATE;
            this.touchDis = this.touchPoint - aimPoint;
            return true;
        }

        public override void OnTouchMoved(float x, float y)
        {
            base.OnTouchMoved(x, y);
            touchPoint.X = x;
            touchPoint.Y = y;

            //转成游戏坐标系
            this.touchPoint /= Config.SCREEN_RATE;
            aimPoint = touchPoint - touchDis;
            ///禁止移出屏幕
            if (aimPoint.X <= 0)
            {
                aimPoint.X = 0;
                if (this.touchPoint.X > 0) this.touchDis.X = this.touchPoint.X;
            }
            else if (aimPoint.X >= Config.GAME_WIDTH)
            {
                aimPoint.X = Config.GAME_WIDTH;
                if (this.touchPoint.X > Config.GAME_WIDTH) this.touchDis.X = Config.GAME_WIDTH - this.touchPoint.X;
            }
            if (aimPoint.Y <= 0)
            {
                aimPoint.Y = 0;
                if (this.touchPoint.Y > 0) this.touchDis.Y = this.touchPoint.Y;
            }
            else if (aimPoint.Y > Config.GAME_HEIGHT)
            {
                aimPoint.Y = Config.GAME_HEIGHT;
                if (this.touchPoint.Y > Config.GAME_HEIGHT) this.touchDis.Y = Config.GAME_HEIGHT - this.touchPoint.Y;
            }
            ///

            //this.player.Postion = aimPoint;
        }

        public override void OnTouchEnded(float x, float y)
        {
            base.OnTouchEnded(x, y);
        }

        public override void OnUpdate(float dTime)
        {
            base.OnUpdate(dTime);
           // if (this.player.CurBehavior == Actors.Players.Player.Behavior.Playing)
            {
                //玩家跟随视点
                this.FellowViewAim(dTime);
                //计算出焦点比例
                veiwPercent = this.player.PostionX / Config.GAME_WIDTH;
                this.PostionX = -parallax * veiwPercent;
            }

        }

        public Player Player
        {
            set { player = value; }
            get { return player; }
        }

        public static float VeiwPercent
        {
            get { return veiwPercent; }
        }

        public static float VeiwParallax
        {
            get { return VeiwPercent * parallax; }
        }

        private void ScheduleSpeed(float t)
        {
            float tickSpeed = aimPoint.X - oldAimX;
            oldAimX = aimPoint.X;
            this.player.TickSpeed(tickSpeed);
            //Console.WriteLine("move speed:" + tickSpeed);
        }

        private void FellowViewAim(float dTime)
        {
            Vector2 newPos = this.player.Postion;
            if (this.player.CurBehavior == Actors.Players.Player.Behavior.Playing)
            {
                Utils.Follow(dTime, this.player.Speed, this.player.Postion, this.aimPoint, ref newPos, 0.5f);
                this.player.Postion = newPos;
            }
        }
    }
}
