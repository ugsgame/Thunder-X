using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thunder.GameLogic.Gaming.Actors;

namespace Thunder.GameLogic.Gaming
{
    /// <summary>
    /// 事件数据
    /// </summary>
    public struct EventInfo
    {
        /// <summary>
        /// 角色信息缓存
        /// </summary>
        public List<SpawnInfo> spawnInfoCache;

        /// <summary>
        /// 描述
        /// </summary>
        public string describe;

        /// <summary>
        /// 名字
        /// </summary>
        public string name;

        /// <summary>
        /// 事件通关条件
        /// false 杀光通过
        /// true 时间通过
        /// </summary>
        public bool crossCondition;

        /// <summary>
        /// 通过时间
        /// 通过条件为时间时使用
        /// /s
        /// </summary>
        public float crossTime;

        /// <summary>
        /// 开场待时间
        /// /s
        /// </summary>
        public float waitingTime;
    }
}
