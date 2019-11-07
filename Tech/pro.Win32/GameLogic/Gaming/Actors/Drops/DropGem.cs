
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MatrixEngine.Cocos2d;
using Thunder.GameLogic.Gaming.BulletSystems;
using Thunder.GameLogic.Common;
using MatrixEngine.Math;

namespace Thunder.GameLogic.Gaming.Actors.Drops
{

    public class DrapGem : Drop
    {
        bool isShowEffect;
        public virtual bool ShowEffect
        {
            get{ return isShowEffect; }
            set{ isShowEffect = value; }
        }

        protected override void OnAdsorbent()
        {
            Function.PlayGemEffect();
        }
    }

    public class DropGemBlue1 : DrapGem
    {
        private CCSprite effect;
        private CCParticleSystem star;
        private CCAction effectAction;

        private static DropEmitter mEmitter;
        public static DropEmitter Emitter
        {
            get 
            {
                if (mEmitter == null)
                {
                    mEmitter = new DropEmitter(new DropGemBlue1());
                }
                return mEmitter;
            }
        }
        

        public DropGemBlue1()
        {
            this.mType = DropType.Drop_Gem_Blue_1;
            //设置diplay
            //TODO:要改为AnimationDisplay 以提高效率
            //this.Display = new ArmatureDisplay("Objects","drop_10");
            //this.Display = new SpriteDisplay(ResID.Armatures_sucai, "sucai_shuijing1-1.png");
            BulletDisplay display = new AnimationDisplay(ResID.Armatures_zidan, "zidan_diji (5).png", "zidan_diji (6).png", "zidan_diji (7).png", "zidan_diji (8).png");
            display.IsUsingBatch = true;
            this.Display = display;

            effect = new CCSprite("effects_006.png", true);
            star = new CCParticleSystem(ResID.Particles_xingxing);

            effect.IsVisible = false;
            star.IsVisible = false;

            BlendFunc blend = new BlendFunc();
            blend.src = BlendValue.GL_SRC_ALPHA;
            blend.dst = BlendValue.GL_ONE;

            effect.Color = new Color32(23, 176, 221);
            effect.BlendFunc = blend;

            effect.Postion = 0;
            star.Postion = 0;

            var action = new CCActionSpawn(new CCActionEaseIn(new CCActionScaleTo(0.3f, 2.5f), 0.3f), new CCActionFadeOut(1f));
            effectAction = new CCActionSequence(action, new CCActionCallFunc(this.OnEffectOver));

            this.AddChild(effect);
            this.AddChild(star);
        }

        protected override void Dispose(bool disposing)
        {
            if (this.effect != null)
            {
                this.RemoveChild(effect);
                if (disposing)
                {
                    effect.Dispose();
                }
                this.effect = null;
            }
            if (this.star != null)
            {
                this.RemoveChild(star);
                if (disposing)
                {
                    star.Dispose();
                }
                this.star = null;
            }
            effectAction = null;
            base.Dispose(disposing);
        }

        public override CCObject Copy()
        {
            DropGemBlue1 drap = new DropGemBlue1();
            this.BaseConfig(drap);
            return drap;
        }

        protected virtual void OnEffectOver()
        {
            effect.IsVisible = false;
            star.IsVisible = false;
        }

        public override bool ShowEffect
        {
            get
            {
                return base.ShowEffect;
            }
            set
            {
                base.ShowEffect = value;
                if (value)
                {
                    effect.IsVisible = true;
                    star.IsVisible = true;
                    effect.Scale = 0;
                    //                     var action = new CCActionSpawn(new CCActionEaseIn(new CCActionScaleTo(0.3f,2.5f),0.3f),new CCActionFadeOut(1f));
                    //                     effect.RunAction(new CCActionSequence(action,new CCActionCallFunc(this.OnEffectOver)));
                    effect.RunAction(effectAction);
                    star.Play();
                }
            }
        }
    }

    public class DropGemBlue2 : DrapGem
    {
        private CCSprite effect;
        private CCParticleSystem star;
        private CCAction effectAction;

        private static DropEmitter mEmitter;
        public static DropEmitter Emitter
        {
            get
            {
                if (mEmitter == null)
                {
                    mEmitter = new DropEmitter(new DropGemBlue2());
                }
                return mEmitter;
            }
        }

        public DropGemBlue2()
        {
            this.mType = DropType.Drop_Gem_Blue_2;
            //TODO
            //this.Display = new ArmatureDisplay("Objects", "drop_7");   
            //this.Display = new SpriteDisplay(ResID.Armatures_sucai, "sucai_shuijing1-1.png");
            BulletDisplay display = new AnimationDisplay(ResID.Armatures_zidan, "zidan_diji (17).png", "zidan_diji (18).png", "zidan_diji (19).png", "zidan_diji (20).png");
            display.IsUsingBatch = true;
            display.AnimationSpeed = 10;
            this.Display = display;

            //this.Display = new SpriteDisplay("Data/Armatures/Particles/byhit.png");
            


            effect = new CCSprite("effects_006.png", true);
            star = new CCParticleSystem(ResID.Particles_xingxing);

            effect.IsVisible = false;
            star.IsVisible = false;

            BlendFunc blend = new BlendFunc();
            blend.src = BlendValue.GL_SRC_ALPHA;
            blend.dst = BlendValue.GL_ONE;

            effect.Color = new Color32(23,176,221);
            effect.BlendFunc = blend;

            effect.Postion = 0;
            star.Postion = 0;

            var action = new CCActionSpawn(new CCActionEaseIn(new CCActionScaleTo(0.3f, 2.5f), 0.3f), new CCActionFadeOut(1f));
            effectAction = new CCActionSequence(action, new CCActionCallFunc(this.OnEffectOver));

            this.AddChild(effect);
            this.AddChild(star);

        }

        protected override void Dispose(bool disposing)
        {           
            if (this.effect != null)
            {
                this.RemoveChild(effect);
                if (disposing)
                {
                    effect.Dispose();
                }
                this.effect = null;
            }
            if (this.star != null)
            {
                this.RemoveChild(star);
                if (disposing)
                {
                    star.Dispose();
                }
                this.star = null;
            }
            effectAction = null;
            base.Dispose(disposing);
        }

        protected virtual void OnEffectOver()
        {
            effect.IsVisible = false;
            star.IsVisible = false;
        }

        public override void Reset()
        {
            effect.IsVisible = false;
            effect.StopAllAction();
            star.Stop();

            base.Reset();
        }

        public override bool ShowEffect
        {
            get
            {
                return base.ShowEffect;
            }
            set
            {
                base.ShowEffect = value;
                if (value)
                {
                    effect.IsVisible = true;
                    star.IsVisible = true;
                    effect.Scale = 0;
//                     var action = new CCActionSpawn(new CCActionEaseIn(new CCActionScaleTo(0.3f,2.5f),0.3f),new CCActionFadeOut(1f));
//                     effect.RunAction(new CCActionSequence(action,new CCActionCallFunc(this.OnEffectOver)));
                    effect.RunAction(effectAction);
                    star.Play();
                }
            }
        }

        public override CCObject Copy()
        {
            DropGemBlue2 drap = new DropGemBlue2();
            this.BaseConfig(drap);
            return drap;
        }
    }

    public class DropGemYellow1 : DrapGem
    {
        private static DropEmitter mEmitter;
        public static DropEmitter Emitter
        {
            get
            {
                if (mEmitter == null)
                {
                    mEmitter = new DropEmitter(new DropGemYellow1());
                }
                return mEmitter;
            }
        }

        public DropGemYellow1()
        {
            this.mType = DropType.Drop_Gem_Yellow_1;
            //TODO
            //this.Display = new ArmatureDisplay("Objects", "drop_11");
            //this.Display = new SpriteDisplay(ResID.Armatures_sucai, "sucai_shuijing1-1.png");
            BulletDisplay display = new AnimationDisplay(ResID.Armatures_zidan, "zidan_diji (9).png", "zidan_diji (10).png", "zidan_diji (11).png", "zidan_diji (12).png");
            display.IsUsingBatch = true;
            this.Display = display;
        }

        public override CCObject Copy()
        {
            DropGemYellow1 drap = new DropGemYellow1();
            this.BaseConfig(drap);
            return drap;
        }

    }

    public class DropGemYellow2 : DrapGem
    {
        private static DropEmitter mEmitter;
        public static DropEmitter Emitter
        {
            get
            {
                if (mEmitter == null)
                {
                    mEmitter = new DropEmitter(new DropGemYellow2());
                }
                return mEmitter;
            }
        }

        public DropGemYellow2()
        {
            this.mType = DropType.Drop_Gem_Yellow_2;
            //TODO
            //this.Display = new ArmatureDisplay("Objects", "drop_8");
            //this.Display = new SpriteDisplay(ResID.Armatures_sucai, "sucai_shuijing1-1.png");
            BulletDisplay display = new AnimationDisplay(ResID.Armatures_zidan, "zidan_diji (21).png", "zidan_diji (22).png", "zidan_diji (23).png");
            display.IsUsingBatch = true;
            this.Display = display;
        }

        public override CCObject Copy()
        {
            DropGemYellow2 drap = new DropGemYellow2();
            this.BaseConfig(drap);
            return drap;
        }

    }

    public class DropGemGreen1 : DrapGem
    {
        private static DropEmitter mEmitter;
        public static DropEmitter Emitter
        {
            get
            {
                if (mEmitter == null)
                {
                    mEmitter = new DropEmitter(new DropGemGreen1());
                }
                return mEmitter;
            }
        }

        public DropGemGreen1()
        {
            this.mType = DropType.Drop_Gem_Green_1;
            //TODO
            //this.Display = new ArmatureDisplay("Objects", "drop_12");
            //this.Display = new SpriteDisplay(ResID.Armatures_sucai, "sucai_shuijing1-1.png");
            BulletDisplay display = new AnimationDisplay(ResID.Armatures_zidan, "zidan_diji (13).png", "zidan_diji (14).png", "zidan_diji (15).png", "zidan_diji (16).png");
            display.IsUsingBatch = true;
            this.Display = display;
        }

        public override CCObject Copy()
        {
            DropGemGreen1 drap = new DropGemGreen1();
            this.BaseConfig(drap);
            return drap;
        }

    }

    public class DropGemGreen2 : DrapGem
    {
        private static DropEmitter mEmitter;
        public static DropEmitter Emitter
        {
            get
            {
                if (mEmitter == null)
                {
                    mEmitter = new DropEmitter(new DropGemGreen2());
                }
                return mEmitter;
            }
        }

        public DropGemGreen2()
        {
            this.mType = DropType.Drop_Gem_Green_2;
            //TODO
            //this.Display = new ArmatureDisplay("Objects", "drop_9");
            BulletDisplay display = new AnimationDisplay(ResID.Armatures_zidan, "zidan_diji (1).png", "zidan_diji (2).png", "zidan_diji (3).png", "zidan_diji (4).png");
            display.IsUsingBatch = true;
            this.Display = display;
        }

        public override CCObject Copy()
        {
            DropGemGreen2 drap = new DropGemGreen2();
            this.BaseConfig(drap);
            return drap;
        }

    }
}
