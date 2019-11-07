using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace ThunderEditor.Logic
{
    /// <summary>
    /// 角色管理器
    /// </summary>
    public class EActorManager
    {
        List<EActor> actors = new List<EActor>();

        //public static EActorManager Instanse = new EActorManager();

        public EActorManager()
        {

        }

        Point origin = new Point();
        /// <summary>
        /// 世界坐标原点
        /// </summary>
        public Point WorldOrigin
        {
            get { return origin; }
            set { origin = value; }
        }

        EWorld world;
        public EWorld World
        {
            get { return world; }
            set { world = value; }
        }

        public EActor AddActor(EActorType actorInfo)
        {
            EActor actor = new EActor(actorInfo);
            actor.ActorManager = this;
            this.actors.Add(actor);
            return actor;
        }

        public void AddActor(EActor actor)
        {
            this.actors.Add(actor);
            actor.ActorManager = this;
        }

        public void RemoveActor(EActor actor)
        {
            this.actors.Remove(actor);
        }

        public EActor GetActor(int index)
        {
            return actors[index];
        }

        public EActor GetActor(Visual visual)
        {
            foreach (var item in actors)
            {
                if (item.Visual == visual)
                    return item;
            }
            return null;
        }

        public List<EActor> GetActors()
        {
            return actors;
        }

        public int Count
        {
            get { return actors.Count; }
        }

        /// <summary>
        /// 重绘所有元素
        /// </summary>
        internal virtual void Draw()
        {
            foreach (var item in actors)
            {
                item.Draw();
            }
        }
    }
}
