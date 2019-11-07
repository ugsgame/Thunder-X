using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MatrixEngine.Math;

namespace Thunder.Common
{
    public static class Config
    {


        public static MatrixEngine.Engine.System.PLATFORM TARGET_PLATFORM = MatrixEngine.Engine.System.TARGET_PLATFORM;
        //设计屏幕宽高
        public static int SCREEN_WIDTH = 480;
        public static int SCREEN_HEIGHT = 800;
        //游戏宽高
        public static int GAME_WIDTH = 600;
        public static int GAME_HEIGHT = 800;

        public static Vector2 SCREEN_RATE = new Vector2();

        public static int SCREEN_PosX_Center
        {
            get { return SCREEN_WIDTH / 2; }
        }

        public static int SCREEN_PosY_Center
        {
            get { return SCREEN_HEIGHT / 2; }
        }

        public static int GAME_PosX_Center
        {
            get { return GAME_WIDTH / 2; }
        }

        public static int GAME_PosY_Center
        {
            get { return GAME_HEIGHT / 2; }
        }

        /// <summary>
        /// 屏幕的大小
        /// </summary>
        public static Size ScreenSize
        {
            get
            {
                return new Size(SCREEN_WIDTH, SCREEN_HEIGHT);
            }
        }

        /// <summary>
        /// 屏幕中心坐标
        /// </summary>
        public static Vector2 ScreenCenter
        {
            get
            {
                return new Vector2(SCREEN_PosX_Center, SCREEN_PosY_Center);
            }
        }

        public static Vector2 GameCenter
        {
            get
            {
                return new Vector2(GAME_PosX_Center, GAME_PosY_Center);
            }
        }

        public static Size ScreenFrameSize;


        public static Vector2 ScreenOffsetPosition
        {
            get { return new Vector2(SCREEN_WIDTH - SCREEN_WIDTH, SCREEN_HEIGHT - SCREEN_HEIGHT); }
        }
    }
}
