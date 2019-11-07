using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thunder.Common;
using Thunder.Game;
using Thunder.GameLogic.Gaming.Actors.Players;
using Thunder.GameLogic.Gaming.BulletSystems;

namespace Thunder.GameLogic.Gaming.Actors.Players
{
    public class Player2 : Player
    {
        public Player2(SpawnInfo spawnInfo)
            : base(spawnInfo)
        {
            this.PlayerID = PlayerSpawner.PlayerID.Player2;
        }

        protected override void Init(SpawnInfo spawnInfo)
        {
            base.Info.animName = "player2";
            base.Init(spawnInfo);

            //分配子弹
            if (EDebug.swBullet)
            {
                InitBullets();
            }
            //
        }

        BulletSystem lv1_01;

        BulletSystem lv2_01;

        BulletSystem lv3_01;
        BulletSystem lv3_06;

        BulletSystem lv4_01;

        BulletSystem lv5_01;
        BulletSystem lv5_02;
        BulletSystem lv5_03;
        BulletSystem lv5_04;
        BulletSystem lv5_05;

        protected virtual void InitBullets()
        {
            BulletEffects comEffect = EffectSpawner.Instanse.GetBulletEffect(EffectSpawner.BulletEffectID.PlayerHitEffect);

            lv1_01 = new BulletSystem(Utils.CoverBulletPath("play2-lv1-01.bt"));
            lv1_01.SetUserEffect(comEffect);

            lv2_01 = new BulletSystem(Utils.CoverBulletPath("play2-lv2-01.bt"));
            lv2_01.SetUserEffect(comEffect);

            lv3_01 = new BulletSystem(Utils.CoverBulletPath("play2-lv3-01.bt"));
            lv3_06 = new BulletSystem(Utils.CoverBulletPath("play2-lv3-02.bt"));
            lv3_01.SetUserEffect(comEffect);
            lv3_06.SetUserEffect(comEffect);

            lv4_01 = new BulletSystem(Utils.CoverBulletPath("play2-lv4-01.bt"));
            lv4_01.SetUserEffect(comEffect);

            lv5_01 = new BulletSystem(Utils.CoverBulletPath("play2-lv5-01.bt"));
            lv5_02 = new BulletSystem(Utils.CoverBulletPath("play2-lv5-01.bt"));
            lv5_03 = new BulletSystem(Utils.CoverBulletPath("play2-lv5-01.bt"));
            lv5_04 = new BulletSystem(Utils.CoverBulletPath("play2-lv5-01.bt"));
            lv5_05 = new BulletSystem(Utils.CoverBulletPath("play2-lv5-01.bt"));
            lv5_01.SetUserEffect(comEffect);
            lv5_02.SetUserEffect(comEffect);
            lv5_03.SetUserEffect(comEffect);
            lv5_04.SetUserEffect(comEffect);
            lv5_05.SetUserEffect(comEffect);
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
            }
            else if (level == 3)
            {
                this.UnbindEmitter();
                this.BindEmitter(EmitPoint.Emit1, lv3_01);
                this.BindEmitter(EmitPoint.Emit6, lv3_06);
            }
            else if (level == 4)
            {
                this.UnbindEmitter();
                this.BindEmitter(EmitPoint.Emit1, lv4_01);
            }
        }

        protected override void OnFrenzy()
        {
            base.OnFrenzy();
            this.UnbindEmitter();
            this.BindEmitter(EmitPoint.Emit1, lv5_01);
            this.BindEmitter(EmitPoint.Emit2, lv5_02);
            this.BindEmitter(EmitPoint.Emit3, lv5_03);
            this.BindEmitter(EmitPoint.Emit4, lv5_04);
            this.BindEmitter(EmitPoint.Emit5, lv5_05);
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
