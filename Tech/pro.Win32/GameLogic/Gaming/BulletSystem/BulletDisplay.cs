using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MatrixEngine.Cocos2d;
using MatrixEngine.CocoStudio.Armature;
using MatrixEngine.Math;

namespace Thunder.GameLogic.Gaming.BulletSystems
{
    /// <summary>
    /// 子弹显示形态
    /// </summary>
    public class BulletDisplay : CCNode
    {
        protected CCNode displayNode;
        private DisplayType displayType;

        protected bool usingBatch;
        protected string batchImagePath;

        public BulletDisplay()
        {
            blendFunc.src = BlendValue.GL_SRC_ALPHA;
            blendFunc.dst = BlendValue.GL_ONE_MINUS_SRC_ALPHA;

        }

        public virtual CCNode DisplayNode
        {
            get { return displayNode; }
        }

        public virtual DisplayType Type
        {
            set { displayType = value; }
            get { return displayType; }
        }

        public virtual bool IsUsingBatch
        {
            set { usingBatch = value; }
            get { return usingBatch; }
        }

        public virtual string BatchImage
        {
            set { batchImagePath = value; }
            get { return batchImagePath; }
        }

        protected float rotationOffset;
        public virtual float RotationOffset
        {
            set
            {
                rotationOffset = value;
                displayNode.Rotation = value;
            }
            get { return rotationOffset; }
        }

        protected float animationSpeed = 10;
        public virtual float AnimationSpeed
        {
            set
            {
                animationSpeed = value;
            }
            get
            {
                return animationSpeed;
            }
        }



        public virtual void Play()
        {
            this.Play(true);
        }
        public virtual void Play(bool loop)
        {
            throw new NotImplementedException();
        }

        private BlendFunc blendFunc;
        public virtual BlendFunc Blend
        {
            set { blendFunc = value; }
            get { return blendFunc; }
        }

        //         public virtual BulletDisplay Copy()
        //         {
        //             return new BulletDisplay();
        //         }
    }

    public class SpriteDisplay : BulletDisplay
    {
        string spritePath;
        string plistFile;

        public SpriteDisplay(string spriteFileName)
        {
            spritePath = spriteFileName;
            Type = DisplayType.dTypeSprite;

            this.displayNode = new CCSprite(spritePath);

            if (this.displayNode == null)
            {
                this.displayNode = new CCSprite();
            }

            this.AddChild(this.displayNode);
        }
        public SpriteDisplay(string plisFileName, string spriteFileName)
        {
            plistFile = plisFileName;
            spritePath = spriteFileName;
            Type = DisplayType.dTypeSpriteFrame;
            CCSpriteFrameCache.AddSpriteFramesWithFile(plisFileName);
            this.displayNode = new CCSprite(spriteFileName, true);
            this.AddChild(this.displayNode);

            this.BatchImage = plisFileName.Replace(".plist", ".png");
        }

        public override bool IsUsingBatch
        {
            get
            {
                return base.IsUsingBatch;
            }
            set
            {
                base.IsUsingBatch = value;
                if (value)
                {
                    this.RemoveChild(this.displayNode);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            //if (disposing)
            {
                this.displayNode.RemoveFromParent(true);
                //this.RemoveChild(this.displayNode);
                this.displayNode.Dispose();
                this.displayNode = null;
            }
            base.Dispose(disposing);
        }

        public override void Play()
        {
            this.Play(true);
        }

        public override void Play(bool loop)
        {
            //base.Play(loop);
        }

        public override BlendFunc Blend
        {
            get
            {
                return base.Blend;
            }
            set
            {
                base.Blend = value;
                ((CCSprite)displayNode).BlendFunc = value;
            }
        }

        public override /*BulletDisplay*/ CCObject Copy()
        {
            BulletDisplay display = null;
            if (Type == DisplayType.dTypeSprite)
            {
                display = new SpriteDisplay(this.spritePath);
            }
            else if (Type == DisplayType.dTypeSpriteFrame)
            {
                display = new SpriteDisplay(this.plistFile, this.spritePath);
            }
            display.Blend = this.Blend;
            display.RotationOffset = this.RotationOffset;
            display.Rotation = this.Rotation;
            display.Scale = this.Scale;
            display.IsUsingBatch = this.IsUsingBatch;
            display.BatchImage = this.BatchImage;
            return display;
        }

        //override for batch       
        public override bool IsVisible
        {
            get
            {
                if (usingBatch)
                    return this.displayNode.IsVisible;
                else
                    return base.IsVisible;
            }
            set
            {
                if (usingBatch)
                    this.displayNode.IsVisible = value;
                else
                    base.IsVisible = value;
            }
        }
        public override MatrixEngine.Math.Vector2 Postion
        {
            get
            {
                if (usingBatch)
                    return this.displayNode.Postion;
                else
                    return base.Postion;
            }
            set
            {
                if (usingBatch)
                    this.displayNode.Postion = value;
                else
                    base.Postion = value;
            }
        }

        public override float Rotation
        {
            get
            {
                if (usingBatch)
                    return this.displayNode.Rotation - rotationOffset;
                else
                    return base.Rotation;
            }
            set
            {
                if (usingBatch)
                    this.displayNode.Rotation = value + rotationOffset;
                else
                    base.Rotation = value;
            }
        }

        public override Vector2 Scale
        {
            get
            {
                if (usingBatch)
                    return this.displayNode.Scale;
                else
                    return base.Scale;
            }
            set
            {
                if (usingBatch)
                    this.displayNode.Scale = value;
                else
                    base.Scale = value;
            }
        }

        public override int ZOrder
        {
            get
            {
                if (usingBatch)
                    return this.displayNode.ZOrder;
                else
                    return base.ZOrder;
            }
            set
            {
                if (usingBatch)
                    this.displayNode.ZOrder = value;
                else
                    base.ZOrder = value;
            }
        }
        //
    }

    public class ParticleDisplay : BulletDisplay
    {
        string particleFile;
        CCParticleSystem particle;

        public ParticleDisplay(string particlePath)
        {
            this.particleFile = particlePath;
            particle = new CCParticleSystem(particleFile);
            displayNode = particle;
            particle.Postion = 0;
            particle.PositionType = tCCPositionType.kCCPositionTypeFree;
            Type = DisplayType.dTypeParticle;
            this.AddChild(particle);
        }

        public override void Play(bool loop)
        {
            if (!particle.IsActive())
            {
                particle.Play();
            }
        }

        public override float Rotation
        {
            get
            {
                return base.Rotation;
            }
            set
            {
                //base.Rotation = value;
                //Do nothing
            }
        }

        public override BlendFunc Blend
        {
            get
            {
                return base.Blend;
            }
            set
            {
                base.Blend = value;
                //TODO
            }
        }

        public override /*BulletDisplay*/CCObject Copy()
        {
            BulletDisplay display = new ParticleDisplay(this.particleFile);
            display.Blend = this.Blend;
            display.RotationOffset = this.RotationOffset;
            display.Rotation = this.Rotation;
            display.Scale = this.Scale;
            return display;
        }
    }

    /// <summary>
    /// cocos帧动画
    /// TODO:
    /// </summary>
    public class AnimationDisplay : BulletDisplay
    {
        string plistFile;
        string[] frameNames;
        MatrixEngine.Cocos2d.CCAnimation animation;

        public AnimationDisplay(string plistFileName, params string[] frames)
        {
            this.Type = DisplayType.dTypeAnimation;
            plistFile = plistFileName;
            frameNames = frames;
            CCSpriteFrameCache.AddSpriteFramesWithFile(plistFile);
            animation = new MatrixEngine.Cocos2d.CCAnimation();
            this.displayNode = new CCSprite(frames[0], true);

            for (int i = 0; i < frames.Length; i++)
            {
                CCSpriteFrame frame = CCSpriteFrameCache.SpriteFrameByName(frames[i]);
                animation.AddSpriteFrame(frame);
            }

            animation.DelayPerUnit = (1.0f / this.animationSpeed);
            animation.RestoreOriginalFrame = true;

            this.AddChild(displayNode);

            this.BatchImage = plistFileName.Replace(".plist", ".png");
        }

        public override bool IsUsingBatch
        {
            get
            {
                return base.IsUsingBatch;
            }
            set
            {
                base.IsUsingBatch = value;
                if (value)
                {
                    this.RemoveChild(this.displayNode);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            //if (disposing)
            {
                //this.RemoveAllChildren(true);
                displayNode.RemoveFromParent(true);
                displayNode.RemoveAllChildren(true);
                displayNode.StopAllAction();

                displayNode.Dispose();
                animation.Dispose();

                displayNode = null;
                animation = null;
                displayNode = null;
            }
            base.Dispose(disposing);
        }

        public override void Play(bool loop)
        {
            try
            {
                var action = new CCActionAnimate(animation);
                displayNode.StopAllAction();
                if (loop)
                {
                    displayNode.RunAction(new CCActionRepeatForever(action));
                }
                else
                {
                    displayNode.RunAction(action);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }

        public override float AnimationSpeed
        {
            get
            {
                return base.AnimationSpeed;
            }
            set
            {
                base.AnimationSpeed = value;
                animation.DelayPerUnit = (1.0f / value);
            }
        }

        public override BlendFunc Blend
        {
            get
            {
                return base.Blend;
            }
            set
            {
                base.Blend = value;
                ((CCSprite)displayNode).BlendFunc = value;
            }
        }

        public override CCObject Copy()
        {
            AnimationDisplay display = new AnimationDisplay(this.plistFile, this.frameNames);
            display.Blend = this.Blend;
            display.RotationOffset = this.RotationOffset;
            display.Rotation = this.Rotation;
            display.Scale = this.Scale;
            display.AnimationSpeed = this.AnimationSpeed;
            display.IsUsingBatch = this.IsUsingBatch;
            display.BatchImage = this.BatchImage;
            return display;
        }

        //override for batch       
        public override bool IsVisible
        {
            get
            {
                if (usingBatch)
                    return this.displayNode.IsVisible;
                else
                    return base.IsVisible;
            }
            set
            {
                if (usingBatch)
                    this.displayNode.IsVisible = value;
                else
                    base.IsVisible = value;
            }
        }
        public override MatrixEngine.Math.Vector2 Postion
        {
            get
            {
                if (usingBatch)
                    return this.displayNode.Postion;
                else
                    return base.Postion;
            }
            set
            {
                if (usingBatch)
                    this.displayNode.Postion = value;
                else
                    base.Postion = value;
            }
        }

        public override float Rotation
        {
            get
            {
                if (usingBatch)
                    return this.displayNode.Rotation - rotationOffset;
                else
                    return base.Rotation;
            }
            set
            {
                if (usingBatch)
                    this.displayNode.Rotation = value + rotationOffset;
                else
                    base.Rotation = value;
            }
        }

        public override Vector2 Scale
        {
            get
            {
                if (usingBatch)
                    return this.displayNode.Scale;
                else
                    return base.Scale;
            }
            set
            {
                if (usingBatch)
                    this.displayNode.Scale = value;
                else
                    base.Scale = value;
            }
        }

        public override int ZOrder
        {
            get
            {
                if (usingBatch)
                    return this.displayNode.ZOrder;
                else
                    return base.ZOrder;
            }
            set
            {
                if (usingBatch)
                    this.displayNode.ZOrder = value;
                else
                    base.ZOrder = value;
            }
        }
        //     
    }

    /// <summary>
    /// cocostudio的动画作来显示形态
    /// 效率最低
    /// </summary>
    public class ArmatureDisplay : BulletDisplay
    {
        string armatureFile;

        string armatureName;
        string animationName;

        CCArmature armature;
        MatrixEngine.CocoStudio.Armature.CCAnimation animtion;

        public ArmatureDisplay(string armaName, string animName)
        {
            armatureName = armaName;
            animationName = animName;

            armature = new CCArmature(armatureName);
            animtion = armature.GetAnimation();
            displayNode = armature;
            this.AddChild(armature);
        }

        public ArmatureDisplay(string armaFile, string armaName, string animName)
        {
            armatureFile = armaFile;
            armatureName = armaName;
            animationName = animName;

            CCArmDataManager.AddArmatureFile(armatureFile);
            armature = new CCArmature(armatureName);
            animtion = armature.GetAnimation();
            displayNode = armature;
            this.AddChild(armature);
        }

        protected override void Dispose(bool disposing)
        {
            //if (disposing)
            {
                animtion.Dispose();
                animtion = null;

                armature.RemoveFromParent(true);
                armature.Dispose();
                armature = null;

                displayNode = null;
            }

            base.Dispose(disposing);
        }

        public override void Play(bool loop)
        {
            animtion.Play(animationName, loop);
        }

        public override BlendFunc Blend
        {
            get
            {
                return base.Blend;
            }
            set
            {
                base.Blend = value;
                armature.BlendFunc = value;
            }
        }

        public override /*BulletDisplay*/CCObject Copy()
        {
            BulletDisplay display = null;
            if (armatureFile == null || armatureFile == "")
            {
                display = new ArmatureDisplay(armatureName, animationName);
            }
            else
            {
                display = new ArmatureDisplay(armatureFile, armatureName, animationName);
            }
            display.Blend = this.Blend;
            display.RotationOffset = this.RotationOffset;
            display.Rotation = this.Rotation;
            display.Scale = this.Scale;

            return display;
        }
    }
}
