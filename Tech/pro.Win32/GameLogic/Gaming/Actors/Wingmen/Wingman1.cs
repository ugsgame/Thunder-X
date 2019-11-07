using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thunder.Common;
using Thunder.Game;
using Thunder.GameLogic.Gaming.BulletSystems;

namespace Thunder.GameLogic.Gaming.Actors.Wingmen
{
    public class Wingman1 : Wingman
    {
        public Wingman1(SpawnInfo spawnInfo)
            : base(spawnInfo)
        {
            this.WingmanID = WingmanSpawner.WingmanID.Wingman1;
        }

        protected override void Init(Thunder.GameLogic.Gaming.SpawnInfo spawnInfo)
        {
            this.Info.animName = "wingman1";
            base.Init(spawnInfo);

            //设置子弹
            if (EDebug.swBullet)
            {
                InitBullets();
            }

            this.OnLevelUp(1);
        }

        BulletSystem lv1;
        BulletSystem lv2;
        BulletSystem lv3;
        BulletSystem lv4;
        BulletSystem lv5;
        protected virtual void InitBullets()
        {
            BulletEffects comEffect = EffectSpawner.Instanse.GetBulletEffect(EffectSpawner.BulletEffectID.PlayerHitEffect);

            lv1 = new BulletSystem(Utils.CoverBulletPath("liaoji-01-lv1.bt"));
            lv1.SetUserEffect(comEffect);

            lv2 = new BulletSystem(Utils.CoverBulletPath("liaoji-01-lv2.bt"));
            lv2.SetUserEffect(comEffect);

            lv3 = new BulletSystem(Utils.CoverBulletPath("liaoji-01-lv3.bt"));
            lv3.SetUserEffect(comEffect);

            lv4 = new BulletSystem(Utils.CoverBulletPath("liaoji-01-lv4.bt"));
            lv4.SetUserEffect(comEffect);

            lv5 = new BulletSystem(Utils.CoverBulletPath("liaoji-01-lv5.bt"));
            lv5.SetUserEffect(comEffect);

        }
        public override void OnLevelUp(int level)
        {
            if (level == 1)
            {
                this.UnbindEmitter();
                this.BindEmitter(EmitPoint.Emit1, lv1);
            }
            else if (level == 2)
            {
                this.UnbindEmitter();
                this.BindEmitter(EmitPoint.Emit1, lv2);
            }
            else if (level == 3)
            {
                this.UnbindEmitter();
                this.BindEmitter(EmitPoint.Emit1, lv3);
            }
            else if (level == 4)
            {
                this.UnbindEmitter();
                this.BindEmitter(EmitPoint.Emit1, lv4);
            }
            else if (level == 5)
            {
                this.UnbindEmitter();
                this.BindEmitter(EmitPoint.Emit1, lv5);
            }
        }

        public override void OnFrenzy()
        {
            this.UnbindEmitter();
            this.BindEmitter(EmitPoint.Emit1, lv5);
        }
    }
}
