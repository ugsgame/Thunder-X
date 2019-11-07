using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MatrixEngine.Math;
using MatrixEngine.Cocos2d;

namespace Thunder.GameLogic.Gaming.BulletSystems
{
    /// <summary>
    /// 基本子弹
    /// </summary>
    /**strut Bullet*/
    public class Bullet
    {
        public Vector2 pos;
        public Vector2 startPos;

        public float size;
        public float deltaSize;

        public float rotation;
        public float deltaRotation;

        public bool live;

        public float liveToTime;
        public float lifeTime;

        public uint atlasIndex;

        public float sinRotation;

        public struct modeA
        {
            public Vector2 dir;
            public CCNode target;
            public float speed;
            public float spinSpeed;
        }
        public modeA _modeA = new modeA();

        //显示形态相关:
        /////////////////////////////
        public BulletDisplay display;
        //击中效果
        public CCNode hitEffect;
        //开火效果
        public CCNode fireEffect;
        //////////////////////////////
    }
}
