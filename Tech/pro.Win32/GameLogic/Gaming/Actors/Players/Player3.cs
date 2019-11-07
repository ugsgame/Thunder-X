using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thunder.Common;
using Thunder.Game;
using Thunder.GameLogic.Gaming.BulletSystems;

namespace Thunder.GameLogic.Gaming.Actors.Players
{
    public class Player3:Player
    {
        public Player3(SpawnInfo spawnInfo)
            : base(spawnInfo)
        {
            this.PlayerID = PlayerSpawner.PlayerID.Player3;
        }

        protected override void Init(SpawnInfo spawnInfo)
        {
            base.Info.animName = "player3";
            base.Init(spawnInfo);

            //分配子弹
            if (EDebug.swBullet)
            {
                InitBullets();
            }
        }

        BulletSystem lv1_01;


        BulletSystem lv2_01;
        BulletSystem lv2_02;
        BulletSystem lv2_03;

        BulletSystem lv3_02;
        BulletSystem lv3_03;
        BulletSystem lv3_04;
        BulletSystem lv3_05;

        BulletSystem lv4_01;
        BulletSystem lv4_02;
        BulletSystem lv4_03;

        BulletSystem lv5_01;
        BulletSystem lv5_02;
        BulletSystem lv5_03;

        protected virtual void InitBullets()
        {
            BulletEffects comEffect = EffectSpawner.Instanse.GetBulletEffect(EffectSpawner.BulletEffectID.PlayerHitEffect);


            lv1_01 = new BulletSystem(Utils.CoverBulletPath("play3-lv1-01.bt"));
            lv1_01.SetUserEffect(comEffect);


            lv2_01 = new BulletSystem(Utils.CoverBulletPath("play3-lv2-01.bt"));
            lv2_02 = new BulletSystem(Utils.CoverBulletPath("play3-lv2-03.bt"));
            lv2_03 = new BulletSystem(Utils.CoverBulletPath("play3-lv2-02.bt"));
            lv2_01.SetUserEffect(comEffect);
            lv2_02.SetUserEffect(comEffect);
            lv2_03.SetUserEffect(comEffect);

            lv3_02 = new BulletSystem(Utils.CoverBulletPath("play3-lv3-01.bt"));
            lv3_03 = new BulletSystem(Utils.CoverBulletPath("play3-lv3-01.bt"));
            lv3_04 = new BulletSystem(Utils.CoverBulletPath("play3-lv3-03.bt"));
            lv3_05 = new BulletSystem(Utils.CoverBulletPath("play3-lv3-02.bt"));
            lv3_02.SetUserEffect(comEffect);
            lv3_03.SetUserEffect(comEffect);
            lv3_04.SetUserEffect(comEffect);
            lv3_05.SetUserEffect(comEffect);

            lv4_01 = new BulletSystem(Utils.CoverBulletPath("play3-lv4-01.bt"));
            lv4_02 = new BulletSystem(Utils.CoverBulletPath("play3-lv4-03.bt"));
            lv4_03 = new BulletSystem(Utils.CoverBulletPath("play3-lv4-02.bt"));
            lv4_01.SetUserEffect(comEffect);
            lv4_02.SetUserEffect(comEffect);
            lv4_03.SetUserEffect(comEffect);

            lv5_01 = new BulletSystem(Utils.CoverBulletPath("play3-lv5-01.bt"));
            lv5_02 = new BulletSystem(Utils.CoverBulletPath("play3-lv5-03.bt"));
            lv5_03 = new BulletSystem(Utils.CoverBulletPath("play3-lv5-02.bt"));
            lv5_01.SetUserEffect(comEffect);
            lv5_02.SetUserEffect(comEffect);
            lv5_03.SetUserEffect(comEffect);
        }

        protected override void OnLevelUp(int level, bool def)
        {
            base.OnLevelUp(level, def);

            if (level == 1)
            {
                this.UnbindEmitter();
                this.BindEmitter(EmitPoint.Emit1, lv1_01);
            }
            else if (level == 2)
            {
                this.UnbindEmitter();
                this.BindEmitter(EmitPoint.Emit1, lv2_01);
                this.BindEmitter(EmitPoint.Emit2, lv2_02);
                this.BindEmitter(EmitPoint.Emit3, lv2_03);
            }
            else if (level == 3)
            {
                this.UnbindEmitter();
                this.BindEmitter(EmitPoint.Emit2, lv3_02);
                this.BindEmitter(EmitPoint.Emit3, lv3_03);
                this.BindEmitter(EmitPoint.Emit4, lv3_04);
                this.BindEmitter(EmitPoint.Emit5, lv3_05);
            }
            else if (level == 4)
            {
                this.UnbindEmitter();
                this.BindEmitter(EmitPoint.Emit1, lv4_01);
                this.BindEmitter(EmitPoint.Emit2, lv4_02);
                this.BindEmitter(EmitPoint.Emit3, lv4_03);
            }
        }

        protected override void OnFrenzy()
        {
            base.OnFrenzy();
            this.UnbindEmitter();
            this.BindEmitter(EmitPoint.Emit1, lv5_01);
            this.BindEmitter(EmitPoint.Emit2, lv5_02);
            this.BindEmitter(EmitPoint.Emit3, lv5_03);
        }

        protected override void OnEnter()
        {
            base.OnEnter();
        }

        protected override void OnExit()
        {
            base.OnExit();
            this.CloseFire();
        }
    }
}
