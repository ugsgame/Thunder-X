using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thunder.Common;
using Thunder.GameLogic.Gaming.Actors;
using Thunder.GameLogic.Gaming.Actors.Drops;
using Thunder.GameLogic.Gaming.BulletSystems;

namespace Thunder.GameLogic.Gaming
{
    /// <summary>
    /// 子弹发射器产生器
    /// TODO:其实是直接管理BulletSystem的
    /// 现在为了测试先以BulletEmitter为管理单位
    /// </summary>
    public class BulletSpawner : IDisposable
    {
        HashMap<Actor.EmitPoint, BulletSystem> emitPoints = new HashMap<Actor.EmitPoint, BulletSystem>((int)Actor.EmitPoint.Count);
        List<BulletSystem> bullets = new List<BulletSystem>((int)Actor.EmitPoint.Count);

        Actor.EmitPoint[] emits;
        string[] emiterNames;

        Actor m_User;
        public Actor User
        {
            set { m_User = value; }
            get { return m_User; }
        }

        bool canFire;
        public bool CanFire
        {
            get { return canFire; }
        }

        public BulletSpawner(Actor user)
        {
            m_User = user;
            emits = (Actor.EmitPoint[])Enum.GetValues(typeof(Actor.EmitPoint));
            emiterNames = new string[emits.Length];
            for (int i = 0; i < emits.Length; i++)
            {
                emiterNames[i] = emits[i].ToString();
            }
        }

        ~BulletSpawner()
        {
            this.Dispose(false);
        }

        public virtual void BindEmitter(Actor.EmitPoint emitPoint, BulletSystem _bullet, bool fire)
        {
            if (!bullets.Contains(_bullet))
            {
                bullets.Add(_bullet);
                _bullet.User = m_User;
                //注意:后面弹幕系统优化可能用到becthNode,不用添加到用户上
                List<BulletEmitter> emitters = _bullet.GetEmitter();
                foreach (var item in emitters)
                {
                    m_User.AddChild(item);
                }

            }

            if (emitPoints[emitPoint] == null)
            {
                emitPoints[emitPoint] = _bullet;
            }
            else
            {
                emitPoints[emitPoint].StopSystem();
                emitPoints[emitPoint] = _bullet;
            }
            if (fire) emitPoints[emitPoint].StartSystem();
        }

        public virtual void UnbindEmitter(Actor.EmitPoint emitPoint)
        {
            if (emitPoints[emitPoint] != null)
            {
                emitPoints[emitPoint].StopSystem();
                emitPoints[emitPoint] = null;
            }
        }

        public virtual void UnbindEmitter()
        {
            for (int i = 0; i < (int)Actor.EmitPoint.Count; i++)
            {
                Actor.EmitPoint point = (Actor.EmitPoint)i;
                if (emitPoints[point] != null)
                {
                    emitPoints[point].StopSystem();
                }
                emitPoints[point] = null;
            }
        }

        public virtual void ReplaceEmitter(Actor.EmitPoint emitPoint, BulletEmitter _bullet, bool fire)
        {

        }

        public virtual float UserDamage
        {
            set
            {
                foreach (var item in bullets)
                {
                    item.UserDamage = value;
                }
            }
        }

        public int EmitterCount
        {
            get { return bullets.Count; }
        }

        /// <summary>
        /// 把当前子弹变成宝石
        /// </summary>
        public virtual void ToGem(DropType droptype = DropType.Drop_Gem_Blue_2)
        {
            foreach (var item in bullets)
            {
                item.TransToGem(droptype);
            }
        }

        public BulletSystem GetBulletSystem(Actor.EmitPoint emitPoint)
        {
            return emitPoints[emitPoint];
        }

        /// <summary>
        /// 交换两个发射点上的子弹
        /// </summary>
        /// <param name="emitPoint1"></param>
        /// <param name="emitPoint2"></param>
        /// <returns></returns>
        public virtual bool Swap(Actor.EmitPoint emitPoint1, Actor.EmitPoint emitPoint2)
        {
            var temp = emitPoints[emitPoint1];
            emitPoints[emitPoint1] = emitPoints[emitPoint2];
            emitPoints[emitPoint2] = temp;
            return true;
        }

        public virtual bool IsFiring
        {
            get
            {
                return canFire;
            }
        }

        /// <summary>
        /// 打开所有挂在发射点的发射器
        /// </summary>
        public virtual void OpenFire()
        {
            canFire = true;
        }

        /// <summary>
        /// 打开指定发射点的发射器
        /// </summary>
        /// <param name="emitterid"></param>
        public virtual void OpenFire(Actor.EmitPoint emitPoint)
        {
            if (canFire && emitPoints[emitPoint] != null)
            {
                emitPoints[emitPoint].StartSystem();
            }
        }

        /// <summary>
        /// 关闭所有挂在发射点的发射器
        /// </summary>
        public virtual void CloseFire()
        {
            foreach (var item in emitPoints)
            {
                if (item.Value != null)
                {
                    item.Value.StopSystem();
                }
            }
            canFire = false;
        }

        /// <summary>
        /// 关闭指定发射点的发射器
        /// </summary>
        /// <param name="emitPoint"></param>
        public virtual void CloseFire(Actor.EmitPoint emitPoint)
        {
            if (canFire && emitPoints[emitPoint] != null)
            {
                emitPoints[emitPoint].StopSystem();
            }
        }

        public virtual void Recycling()
        {
            foreach (var item in emitPoints)
            {
                if (item.Value != null)
                {
                    item.Value.Recycling();
                }
            }
        }

        /// <summary>
        /// 更新当前动画发射点的坐标
        /// 不用实时更新
        /// </summary>
        /// <param name="dt"></param>
        public virtual void OnUpdate(float dt)
        {
            for (int i = 0; i < emits.Length; i++)
            {
                string emitName = emiterNames[i];
                BulletSystem bullet = null;
                if (emitPoints.TryGetValue(emits[i], out bullet) && bullet != null)
                //if (emitPoints[emits[i]]!=null)
                {
                    if (m_User.Animable.BoneIsDisplay(emitName))
                    {
                        emitPoints[emits[i]].Postion = m_User.Animable.LogicPoint(emitName);
                        //Console.WriteLine(emits[i] + ":" + emitPoints[emits[i]].Postion);
                        if (this.canFire)
                        {
                            if (emitPoints[emits[i]].IsStop)
                            {
                                emitPoints[emits[i]].StartSystem();
                            }
                        }
                    }
                    else
                    {
                        emitPoints[emits[i]].StopSystem();
                    }
                }

            }

            foreach (var item in bullets)
            {
                item.OnUpdate(dt);
            }
        }

        #region IDisposable 成员

        public void Dispose()
        {
            Dispose(true);
            System.GC.SuppressFinalize(this);
        }
        #endregion
        protected virtual void Dispose(bool disposing)
        {
            //if (disposing)
            {
                this.ToGem(DropType.Drop_Gem_Blue_1);
                foreach (var item in bullets)
                {
                    List<BulletEmitter> emitters = item.GetEmitter();
                    //注意:后面弹幕系统优化可能用到becthNode,不用添加到用户上
                    foreach (var bullet in emitters)
                    {
                        m_User.RemoveChild(bullet);
                        bullet.Dispose();
                    }
                    //TODO:交回actor的BulletList管理删除,因为不是在这里创建的，所以最好不要在这里删除
                    emitters = null;
                    //
                }

                bullets.Clear();
                emitPoints.Clear();

                bullets = null;
                emitPoints = null;
            }
        }
    }
}
