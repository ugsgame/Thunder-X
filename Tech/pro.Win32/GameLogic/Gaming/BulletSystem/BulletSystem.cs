
using MatrixEngine.Math;
using System;
using System.Collections.Generic;
using Thunder.Game;
using Thunder.GameLogic.Gaming.Actors;
using Thunder.GameLogic.Gaming.Actors.Drops;

namespace Thunder.GameLogic.Gaming.BulletSystems
{
    /// <summary>
    /// TODO:弹幕系统
    /// version: 0.0.1
    /// Athor:Dean
    /// 可以同时管理一个以上的BulletEmitter
    /// 后期会添加子弹事件，脚本什么的
    /// </summary>
    public class BulletSystem : IDisposable
    {
        private List<BulletEmitter> bulletEmitters = new List<BulletEmitter>();

        public BulletSystem()
        {

        }

        public BulletSystem(params string[] emitterFile)
        {
            for (int i = 0; i < emitterFile.Length; i++)
            {
                BulletEmitter emitter = new BulletEmitter(emitterFile[i]);
                this.bulletEmitters.Add(emitter);
            }
        }

        public BulletSystem(params BulletEmitter[] emitters)
        {
            for (int i = 0; i < emitters.Length; i++)
            {
                if (!this.bulletEmitters.Contains(emitters[i]))
                {
                    this.bulletEmitters.Add(emitters[i]);
                }
            }
        }

        public void AddEmitter(params BulletEmitter[] emitters)
        {
            for (int i = 0; i < emitters.Length; i++)
            {
                if (!this.bulletEmitters.Contains(emitters[i]))
                {
                    this.bulletEmitters.Add(emitters[i]);
                }
            }
        }

        public bool RemoveEmitter(BulletEmitter emitter)
        {
            return this.bulletEmitters.Remove(emitter);
        }

        public virtual List<BulletEmitter> GetEmitter()
        {
            return this.bulletEmitters;
        }

        public virtual BulletEmitter GetEmitter(int index)
        {
            try
            {
                return bulletEmitters[index];
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public static bool RegisterTarget(Actor target)
        {
            return BulletEmitter.RegisterTarget(target);
        }

        public static bool UnregisterTarget(Actor target)
        {
            return BulletEmitter.UnregisterTarget(target);
        }

        Actor m_pUser;
        public Actor User
        {
            set
            {
                this.m_pUser = value;
                foreach (var item in bulletEmitters)
                {
                    item.User = value;
                }
            }
            get { return m_pUser; }
        }

        public virtual void TransToGem(DropType droptype)
        {
            foreach (var item in bulletEmitters)
            {
                item.TransToGem(droptype);
            }
        }

        public virtual float UserDamage
        {
            set
            {
                foreach (var item in bulletEmitters)
                {
                    item.UserDamage = value;
                }
            }
        }

        public virtual void Recycling()
        {
            foreach (var item in bulletEmitters)
            {
                item.Recycling();
            }
        }

        public virtual void SetHitEffect(params string[] effects)
        {
            foreach (var item in bulletEmitters)
            {
                item.SetEmitterEffect(effects);
            }
        }

        public virtual void SetHitEffect(int index,params string[] effects)
        {
            bulletEmitters[index].SetEmitterEffect(effects);
        }

        public virtual void SetUserEffect(BulletEffects effect)
        {
            foreach (var item in bulletEmitters)
            {
                item.SetUserEffect(effect);
            }
        }

        private Vector2 postion;
        public Vector2 Postion
        {
            set 
            {
                foreach (var item in bulletEmitters)
                {
                    item.Postion = value;
                }
                postion = value;
            }

            get { return postion; }
        
        }

        public virtual void OnUpdate(float dt)
        {
            foreach (var item in bulletEmitters)
            {
                item.OnUpdate(dt);
            }
        }

        bool isPause;
        public virtual bool IsPause
        {
            get { return isPause; }
        }

        bool isStop;
        public virtual bool IsStop
        {
            get { return isStop; }
        }

        public virtual void StartSystem()
        {
            foreach (var item in bulletEmitters)
            {
                item.StartSystem();
            }
            isStop = false;
            isPause = false;
        }

        public virtual void PauseSystem()
        {
            foreach (var item in bulletEmitters)
            {
                item.PauseSystem();
            }
            isPause = true;
        }

        public virtual void StopSystem()
        {
            foreach (var item in bulletEmitters)
            {
                item.StopSystem();
            }
            isStop = true;
        }

        public void Dispose()
        {
            foreach (var item in bulletEmitters)
            {
                BulletEmitter emitter = item;
                if (emitter != null)
                {
                    emitter.Dispose();
                    emitter = null;
                }
            }
            bulletEmitters.Clear();
        }
    }
}
