
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MatrixEngine.Cocos2d;
using MatrixEngine.Math;

namespace Thunder.GameLogic.Gaming.BulletSystems
{
    public class BulletEffects : CCNode
    {

        List<CCParticleSystem> mParticles = new List<CCParticleSystem>();


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var item in mParticles)
                {
                    item.Dispose();
                }
            }
            mParticles.Clear();
            this.RemoveAllChildren();
            base.Dispose(disposing);
        }

        public BulletEffects(params string[] particleFiles)
        {
            for (int i = 0; i < particleFiles.Length; i++)
            {
                CCParticleSystem par = new CCParticleSystem(particleFiles[i]);
                par.Postion = 0;
                par.Stop();
                this.AddChild(par);
                mParticles.Add(par);
            }
        }

        public tCCPositionType PositionType
        {
            set
            {
                foreach (var item in mParticles)
                {
                    item.PositionType = value;
                }
            }
        }

        public void Start(Vector2 pos)
        {
            this.Postion = pos;
            foreach (var item in mParticles)
            {
                item.Start();
            }
        }
        public void Play()
        {
            foreach (var item in mParticles)
            {
                item.Play();
            }
        }
    }
}
