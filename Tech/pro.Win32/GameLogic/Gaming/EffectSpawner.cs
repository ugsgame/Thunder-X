using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thunder.GameLogic.Common;
using Thunder.GameLogic.Gaming.BulletSystems;

namespace Thunder.GameLogic.Gaming
{
    public class EffectSpawner:IDisposable
    {
        public enum BulletEffectID
        {
            PlayerHitEffect,
            Conut
        }

        protected Dictionary<BulletEffectID,BulletEffects> bulletEffects = new Dictionary<BulletEffectID,BulletEffects>();

        public static EffectSpawner Instanse;

        public  EffectSpawner()
        {
            bulletEffects[BulletEffectID.PlayerHitEffect] = new BulletEffects(ResID.Particles_hitSpark1, ResID.Particles_hitSpark2);
        }

        public BulletEffects GetBulletEffect(BulletEffectID id)
        {
            return bulletEffects[id];
        }

        public void Dispose()
        {
            foreach (var item in bulletEffects)
            {
                item.Value.Dispose();
            }
            bulletEffects.Clear();
        }
    }
}
