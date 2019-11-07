using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Thunder.GameLogic.Gaming.Actors;
using Thunder.Common;
using MatrixEngine.Math;
using MatrixEngine.CocoStudio.Armature;

namespace Thunder.GameLogic.Gaming
{
    public class SpawnInfo
    {
        private HashMap<Actor.AnimName, string> actions = new HashMap<Actor.AnimName, string>(Enum.GetValues(typeof(Actor.AnimName)).Length);

        private string _animName;
        private string _resPath;


        private List<string> emitters = new List<string>((int)Actor.EmitPoint.Count);
        public List<string> Emitters
        {
            get { return emitters; }
        }

        /// <summary>
        /// 所有动作名字（具体到某个角色再添加具体的动本）
        /// 如：loading 对应的是 qiangbing_loading
        /// </summary>
        public string GetAnimName(Actor.AnimName a)
        {
            if (actions == null)
            {
                return null;
            }
            return actions[a];
        }
        /// <summary>
        /// 动画前缀名字
        /// TODO:
        /// </summary>
        public string animName
        {
            get 
            {
                return _animName;
            }
            set
            {
                _animName = value;
            }
        }

        /// <summary>
        /// 实际动画工程资源名字
        /// 如："Data/Anim/Character/qiangbing/qiangbing.ExportJson"
        /// TODO:
        /// </summary>
        public string resPath
        {
            get { return _resPath; }
            set
            {
                _resPath = value;
            }
        }

        /// <summary>
        /// 动画集名字（通过名字取得动画集）
        /// 如：qiangbing
        /// </summary>
        public string armaName;

        /// <summary>
        /// 相对于玩家的敌人或朋友
        /// true  为敌人
        /// false 为玩家
        /// </summary>
        public bool isEnemy;

        /// <summary>
        /// 对应的游戏元素类
        /// </summary>
        public SpawnType spawnType;

        /// <summary>
        /// 角色id类型
        /// </summary>
        public ActorID actorId;

        /// <summary>
        /// 描述
        /// </summary>
        public string describe;

        /// <summary>
        /// 名字
        /// </summary>
        public string name;

        /// <summary>
        /// 等级
        /// </summary>
        public int level;

        /// <summary>
        /// 暴击时间（特指player的）
        /// </summary>
        public float critTime;

        /// <summary>
        /// 生命值
        /// </summary>
        public float HP;
        /// <summary>
        /// 总生命值
        /// </summary>
        public float totalHp;
        /// <summary>
        /// 狂暴Hp(特指boss的)
        /// </summary>
        public float frenzyHp;

        /// <summary>
        /// 伤害值
        /// </summary>
        public float damage;

        /// <summary>
        /// 移动速度
        /// 单位 像素/s
        /// </summary>
        public float speed;

        /// <summary>
        /// 坐标
        /// </summary>
        public Vector2 position;

        /// <summary>
        /// 过滤
        /// </summary>
        public CCColliderFilter filter;

        /// <summary>
        /// 延迟
        /// </summary>
        public float delayTime;

        /// <summary>
        /// 掉落
        /// </summary>
        public DropHelper drops;
        /// <summary>
        /// 值多少分
        /// </summary>
        public int score;
        /// <summary>
        /// 值多少金
        /// </summary>
        public int golds;
        /// <summary>
        /// 行为脚本
        /// </summary>
        public string actionScript;

        public SpawnInfo Clone()
        {
            return (SpawnInfo)base.MemberwiseClone();
        }
    }
}
