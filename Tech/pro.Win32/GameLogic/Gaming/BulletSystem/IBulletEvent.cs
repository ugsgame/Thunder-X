using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MatrixEngine.Math;

namespace Thunder.GameLogic.Gaming.BulletSystems
{
    interface  IBulletEvent
    {
        void BulletHit(BulletEmitter bulletEmitter,ref Bullet bullet,Vector2 hitPos);
    }
}
