using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MatrixEngine.Math;
using MatrixEngine.Cocos2d;
using Thunder.GameLogic.Common;
using Thunder.GameLogic.Gaming.Actors;
using Thunder.Common;
using MatrixEngine.CocoStudio.Armature;
using LitJson;
using Thunder.Game;
using Thunder.GameLogic.Gaming.Actors.Drops;
using Thunder.GameLogic.Gaming.Actors.Players;

namespace Thunder.GameLogic.Gaming.BulletSystems
{
    /// <summary>
    /// 弹幕发射器
    /// version: 0.0.1
    /// Athor:Dean
    /// TODO:先用c#实现测试一下算法，
    /// 为提高效率，后面再改用native做
    /// </summary>
    public class BulletEmitter : CCNode/*CCLayer*/
    {
        public enum COLLIDE_MODE
        {
            Point,
            Line,
            Rect
        }
        protected COLLIDE_MODE cMode = COLLIDE_MODE.Point;

        public static int AllBulletCount;
        //! time elapsed since the start of the system (in seconds)
        float m_fElapsed;

        float m_fInterim;
        /// <summary>
        /// modeA的共用参数
        /// </summary>
        struct modeA
        {
            /**初速度*/
            public float speed;
            public float speedVar;
            /**速度衰变值 正加，负减*/
            public float speedDecay;
            /**速度限值 衰变值正为大，负为小*/
            public float speedLimit;

            /**生命时间(-1无限)*/
            public float lifeTime;
            public float lifeTimeVar;
            /**自旋速度*/
            public float spinSpeed;
            public float spinSpeedVar;
        }
        modeA _modeA = new modeA();
        /*
        struct modeB
        {

        }
         */
        //子弹
        List<Bullet> m_pBullets = new List<Bullet>(0);
        //靶子
        static List<Actor> m_pTargets = new List<Actor>(0);
        //碰撞器
        static List<string> m_sColliers = new List<string>(0);

        //! How many bullets can be emitted per second
        float m_fEmitCounter;
        //
        uint m_uAllocatedBullets;
        //!  bullet idx
        uint m_uBulletIdx;
        //!  发射器方向（单位向量）
        Vector2 m_pEmissionDir = Vector2.Zero;

        float m_fEmissionAngleCount;

        float v_fDuration;
        float v_fInterimTime;
        float v_fEmissionRate;

        bool m_bIsActive;
        /// <summary>
        /// 系统是否处于活动状态
        /// </summary>
        public virtual bool IsActive
        {
            get { return m_bIsActive; }
        }

        bool m_bIsPause;
        public virtual bool IsPause
        {
            get { return m_bIsPause; }
        }

        bool m_bIsStop;
        public virtual bool IsStop
        {
            get { return m_bIsStop; }
        }


        uint m_uBulletCount;
        /// <summary>
        /// 当前模拟的子弹数量
        /// </summary>
        public virtual uint BulletCount
        {
            get { return this.m_uBulletCount; }
        }
        /////////////////////////////////////////////////////////////
        uint m_uTotalBullets;
        /// <summary>
        /// 子弹的预设容量
        /// 容量不够会自动扩容
        /// </summary>
        public virtual uint TotalBullets
        {
            set { m_uTotalBullets = value; }
            get { return m_uTotalBullets; }
        }

        BulletMode m_nEmitterMode;
        /// <summary>
        /// 子弹模式
        /// </summary>
        public virtual BulletMode EmitterMode
        {
            set
            {
                m_nEmitterMode = value;
                if (m_nEmitterMode == BulletMode.kBulletModeLaser)
                {
                    this.m_ePositionType = tCCPositionType.kCCPositionTypeRelative;
                }
            }
            get { return m_nEmitterMode; }
        }

        float m_fDuration;
        /// <summary>
        /// 发身器持续时间（单位秒）. -1 表示 '永远'
        /// </summary>
        public virtual float Duration
        {
            set
            {
                this.m_fDuration = value;
                this.v_fDuration = value;
            }
            get { return this.m_fDuration; }
        }

        float m_fDurationVar;
        public virtual float DurationVar
        {
            set { this.m_fDurationVar = value; }
            get { return this.m_fDurationVar; }
        }

        float m_fInterimTime;
        public virtual float InterimTime
        {
            set
            {
                this.m_fInterimTime = value;
                this.v_fInterimTime = value;
            }
            get { return m_fInterimTime; }
        }
        float m_fInterimTimeVar;
        public virtual float InterimTimeVar
        {
            set { this.m_fInterimTimeVar = value; }
            get { return m_fInterimTimeVar; }
        }

        float m_fEmissionAngle;
        /// <summary>
        /// 发射角度
        /// </summary>
        public virtual float EmissionAngle
        {
            set
            {
                m_fEmissionAngle = value;
                m_fEmissionAngleCount = value;
            }
            get { return m_fEmissionAngle; }
        }

        int m_uBeamCount = 1;
        public virtual int BeamCount
        {
            set { m_uBeamCount = value; }
            get { return m_uBeamCount; }
        }
        int m_uBeamCountVar;
        public virtual int BeamCountVar
        {
            set { m_uBeamCountVar = value; }
            get { return m_uBeamCountVar; }
        }

        float m_fEmissionRate;
        /// <summary>
        /// 发射频率
        /// </summary>
        public virtual float EmissionRate
        {
            set
            {
                m_fEmissionRate = value;
                v_fEmissionRate = value;
            }
            get { return m_fEmissionRate; }
        }

        float m_fEmissionRateVar;
        public virtual float EmissionRateVar
        {
            set { m_fEmissionRateVar = value; }
            get { return m_fEmissionRateVar; }
        }
        float m_fFieldAngle;
        public virtual float FieldAngle
        {
            set { m_fFieldAngle = value; }
            get { return m_fFieldAngle; }
        }
        float m_fFieldAngleVar;
        public virtual float FieldAngleVar
        {
            set { m_fFieldAngleVar = value; }
            get { return m_fFieldAngleVar; }
        }

        float m_fSpinSpeed;
        public virtual float SpinSpeed
        {
            set { m_fSpinSpeed = value; }
            get { return m_fSpinSpeed; }
        }

        tCCPositionType m_ePositionType;
        /// <summary>
        /// 坐标类型
        /// TODO:其实是CCNode共有的属性
        /// </summary>
        public virtual tCCPositionType PositionType
        {
            set { this.m_ePositionType = value; }
            get { return m_ePositionType; }
        }

        bool m_bIsPointToTarget;
        public virtual bool IsPointToTarget
        {
            set { this.m_bIsPointToTarget = value; }
            get { return this.m_bIsPointToTarget; }
        }

        protected delegate bool CollistionFunc(ref Bullet bullet, Vector2 newPos);
        private CollistionFunc collideFuc;
        public COLLIDE_MODE CollideMode
        {
            set
            {
                cMode = value;
                switch (cMode)
                {
                    case COLLIDE_MODE.Point:
                        collideFuc = this.VertexCollistion;
                        break;
                    case COLLIDE_MODE.Line:
                        collideFuc = this.LineCollision;
                        break;
                    case COLLIDE_MODE.Rect:
                        collideFuc = this.RectCollision;
                        break;
                    default:
                        break;
                }
            }
            get
            {
                return cMode;
            }
        }

        ////////////////////////////////////////////////////////////
        public virtual float Speed
        {
            set { _modeA.speed = value; }
            get { return _modeA.speed; }
        }
        public virtual float SpeedVar
        {
            set { _modeA.speedVar = value; }
            get { return _modeA.speedVar; }
        }

        public virtual float SpeedDecay
        {
            set { _modeA.speedDecay = value; }
            get { return _modeA.speedDecay; }
        }

        public virtual float SpeedLimit
        {
            set { _modeA.speedLimit = value; }
            get { return _modeA.speedLimit; }
        }

        public virtual float LifeTime
        {
            set { this._modeA.lifeTime = value; }
            get { return this._modeA.lifeTime; }
        }
        public virtual float LifeTimeVar
        {
            set { this._modeA.lifeTimeVar = value; }
            get { return this._modeA.lifeTimeVar; }
        }

        public virtual float BulletSpinSpeed
        {
            set { this._modeA.spinSpeed = value; }
            get { return this._modeA.spinSpeed; }
        }
        public virtual float BulletSpinSpeedVar
        {
            set { this._modeA.spinSpeedVar = value; }
            get { return this._modeA.spinSpeedVar; }
        }

        float userDamage;
        public float UserDamage
        {
            set { this.userDamage = value; }
            get { return this.userDamage; }
        }

        float m_fDamage;
        public float Damage
        {
            set { this.m_fDamage = value; }
            get { return this.m_fDamage; }
        }

        bool m_bIsFollow;
        public bool IsFollow
        {
            set { this.m_bIsFollow = value; }
            get { return this.m_bIsFollow; }
        }

        float m_fCurvity = 0.1f;
        public float Curvity
        {
            set { this.m_fCurvity = value; }
            get { return this.m_fCurvity; }
        }

        float m_fSinAmplitude;
        public float SinAmplitude
        {
            set { this.m_fSinAmplitude = value; }
            get { return this.m_fSinAmplitude; }
        }
        float m_fSinRate;
        public float SinRate
        {
            set { this.m_fSinRate = value; }
            get { return this.m_fSinRate; }
        }

        BulletDisplay m_pDisplay;
        /// <summary>
        /// 子弹形态
        /// TODO:要和BatchNode结合，以提高效率
        /// </summary>
        public virtual BulletDisplay Display
        {
            set { m_pDisplay = value; }
            get { return m_pDisplay; }
        }
        ///////////////////////////////////////////////////////////
        CCColliderFilter m_pFilter = FilterType.AllFilter;
        /// <summary>
        /// 过滤值
        /// </summary>
        public virtual CCColliderFilter Filter
        {
            set { m_pFilter = value; }
            get { return m_pFilter; }
        }

        BulletEffects m_pHitEffet;
        public virtual void SetEmitterEffect(params string[] effects)
        {
            if (m_pHitEffet != null)
            {
                worldNode.RemoveChild(m_pHitEffet);
                m_pHitEffet.Dispose();
            }
            m_pHitEffet = new BulletEffects(effects);
            m_pHitEffet.ZOrder = PlayingScene.ZOrder_bullet + 1;
            m_pHitEffet.PositionType = tCCPositionType.kCCPositionTypeFree;
            //worldNode.AddChild(m_pHitEffet);
        }

        BulletEffects m_userEffect;
        public virtual void SetUserEffect(BulletEffects effect)
        {
            if (m_userEffect != null)
            {
                worldNode.RemoveChild(effect);
            }
            m_userEffect = effect;
            m_userEffect.ZOrder = PlayingScene.ZOrder_bullet + 1;
            m_userEffect.PositionType = tCCPositionType.kCCPositionTypeFree;
        }

        static CCNode worldNode = new CCNode();
        public static CCNode WorldNode
        {
            set { worldNode = value; }
            get { return worldNode; }
        }

        private CCSpriteBatchNode batchNode;
        public CCSpriteBatchNode BatchNode
        {
            set { batchNode = value; }
            get { return batchNode; }
        }

        private bool usingBatch;
        public bool IsUsingBatch
        {
            set { usingBatch = value; }
            get { return usingBatch; }
        }

        Actor m_pUser;
        /// <summary>
        /// 弹幕使用者
        /// </summary>
        public Actor User
        {
            set
            {
                this.m_pUser = value;
                this.m_pFilter = this.m_pUser.Filter;
                this.userDamage = value.Info.damage;
            }
            get { return m_pUser; }
        }

        public static List<Actor> Target
        {
            get { return m_pTargets; }
        }

        public BulletEmitter()
        {
            this.ZOrder = PlayingScene.ZOrder_bullet;
            this.CollideMode = COLLIDE_MODE.Point;
        }

        public BulletEmitter(string configFile)
        {
            this.ZOrder = PlayingScene.ZOrder_bullet;
            JsonData configData = null;
            try
            {
                string data = CCFileUtils.GetFileDataToString(configFile);
                //Console.WriteLine(data);
                configData = JsonMapper.ToObject(data);
            }
            catch (Exception e)
            {
                Utils.PrintException(e);
            }
            Init(configData);
        }

        public BulletEmitter(JsonData configData)
        {
            this.ZOrder = PlayingScene.ZOrder_bullet;
            Init(configData);
        }

        protected override void Dispose(bool disposing)
        {
            //if (disposing)
            {
                foreach (var item in m_pBullets)
                {
                    if (item.display != null)
                    {
                        if (this.usingBatch)
                        {
                            this.batchNode.RemoveChild(item.display.DisplayNode);
                        }
                        else
                        {
                            worldNode.RemoveChild(item.display);
                        }
                        item.display.RemoveFromParent(true);
                        item.display.Dispose();
                        item.display = null;
                    }
                }
                m_pBullets.Clear();
                m_pDisplay.Dispose();

                if (m_pHitEffet != null)
                {
                    m_pHitEffet.RemoveFromParent(true);
                    m_pHitEffet.Dispose();
                    m_pHitEffet = null;
                }
                if (m_userEffect != null)
                {
                    worldNode.RemoveChild(m_userEffect);
                    m_userEffect = null;
                }

                m_pBullets = null;
                m_pDisplay = null;

                AllBulletCount -= (int)m_uBulletCount;
            }

            base.Dispose(disposing);
        }
        /*
        public override void OnExit()
        {
            base.OnExit();
            foreach (var item in m_pBullets)
            {
                item.display.RemoveFromParent(true);
            }
        }
        */
        protected virtual void Init(JsonData configData)
        {
            //预设子弹数
            int initCount = 0;
            try
            {
                float duration = Utils.JsonNumber(configData, "duration", -1);
                float durationVar = Utils.JsonNumber(configData, "durationVar", 0);
                float interimTime = Utils.JsonNumber(configData, "interimTime", 0);
                float interimTimeVar = Utils.JsonNumber(configData, "interimTimeVar", 0);
                float lifeTime = Utils.JsonNumber(configData, "lifeTime", -1);
                float lifeTimeVar = Utils.JsonNumber(configData, "lifeTimeVar", 0);
                initCount = (int)Utils.JsonNumber(configData, "initCount", 50);
                float speed = Utils.JsonNumber(configData, "speed", 100);
                float speedVar = Utils.JsonNumber(configData, "speedVar", 0);
                float speedDecay = Utils.JsonNumber(configData, "speedDecay", 0);
                float speedLimit = Utils.JsonNumber(configData, "speedLimit", 0);
                int positionType = (int)Utils.JsonNumber(configData, "positionType", (int)tCCPositionType.kCCPositionTypeFree);
                bool isPointToTarget = Utils.JsonBoolean(configData, "isPointToTarget", false);
                int emitterMode = (int)Utils.JsonNumber(configData, "emitterMode", (int)BulletMode.kBulletModeNormal);
                float emissionRate = Utils.JsonNumber(configData, "emissionRate", 5);
                float emissionAngle = Utils.JsonNumber(configData, "emissionAngle", 0);
                int beamCount = (int)Utils.JsonNumber(configData, "beamCount", 1);
                int beamCountVar = (int)Utils.JsonNumber(configData, "beamCountVar", 0);
                float fieldAngle = Utils.JsonNumber(configData, "fieldAngle", 0);
                float fieldAngleVar = Utils.JsonNumber(configData, "fieldAngleVar", 0);
                float spinSpeed = Utils.JsonNumber(configData, "spinSpeed", 0);
                float bulletSpinSpeed = Utils.JsonNumber(configData, "bulletSpinSpeed", 0);
                float bulletSpinSpeedVar = Utils.JsonNumber(configData, "bulletSpinSpeedVar", 0);
                float damage = Utils.JsonNumber(configData, "damage", 0);
                bool isFollow = Utils.JsonBoolean(configData, "isFollow", false);
                float curvity = Utils.JsonNumber(configData, "curvity", 0.1f);
                float sinAmplitude = Utils.JsonNumber(configData, "sinAmplitude", 0);
                float sinRate = Utils.JsonNumber(configData, "sinRate", 0);

                //display
                JsonData displayData = (JsonData)configData["display"];
                {
                    BlendFunc blendFuc = new BlendFunc();

                    string file = (string)displayData["file"];
                    int type = (int)Utils.JsonNumber(displayData, "type", (int)DisplayType.dTypeSprite);
                    float rotation = Utils.JsonNumber(displayData, "rotation", 0);
                    float scale = Utils.JsonNumber(displayData, "scale", 1);
                    blendFuc.src = (BlendValue)Utils.JsonNumber(displayData, "blend_src");
                    blendFuc.dst = (BlendValue)Utils.JsonNumber(displayData, "blend_dst");
                    switch ((DisplayType)type)
                    {
                        case DisplayType.dTypeSprite:
                            {
                                this.m_pDisplay = new SpriteDisplay(file);
                            }
                            break;
                        case DisplayType.dTypeSpriteFrame:
                            {
                                string spritePath = (string)displayData["atlas"];

                                this.m_pDisplay = new SpriteDisplay(file, spritePath);                               
                                this.usingBatch = true;
                                if (blendFuc == BlendFunc.Additive) this.usingBatch = false;
                                if (scale > 1.05f || scale < 0.95f) this.usingBatch = false;
                                if (this.usingBatch)
                                {
                                    this.m_pDisplay.IsUsingBatch = true;
                                    this.batchNode = SpriteBatchManager.Instance.GetBatchNode(file.Replace(".plist", ".png"));
                                    SpriteBatchManager.Instance.WorldNode = worldNode;
                                }
                            }
                            break;
                        case DisplayType.dTypeParticle:
                            {
                                this.m_pDisplay = new ParticleDisplay(file);
                            }
                            break;
                        case DisplayType.dTypeAnimation:
                            {
                                JsonData frames = (JsonData)displayData["frames"];
                                string[] frameName = new string[frames.Count];
                                for (int i = 0; i < frames.Count; i++)
                                {
                                    frameName[i] = (string)frames[i];
                                }

                                this.m_pDisplay = new AnimationDisplay(file, frameName);
                                this.usingBatch = true;
                                if (blendFuc == BlendFunc.Additive) this.usingBatch = false;
                                if (scale > 1.05f || scale < 0.95f) this.usingBatch = false;
                                if (this.usingBatch)
                                {
                                    this.m_pDisplay.IsUsingBatch = true;
                                    this.batchNode = SpriteBatchManager.Instance.GetBatchNode(file.Replace(".plist", ".png"));
                                    SpriteBatchManager.Instance.WorldNode = worldNode;
                                }
                            }
                            break;
                        case DisplayType.dTypeArmature:
                            {
                                string armature = (string)displayData["armature"];
                                string animation = (string)displayData["animation"];
                                this.m_pDisplay = new ArmatureDisplay(file, armature, animation);
                            }
                            break;
                        default:
                            break;
                    }
                    if (m_pDisplay != null)
                    {
                        m_pDisplay.Blend = blendFuc;
                        m_pDisplay.RotationOffset = rotation;
                        m_pDisplay.Scale = scale;
                    }
                }
                //

                this.Duration = duration;
                this.DurationVar = durationVar;
                this.InterimTime = interimTime;
                this.InterimTimeVar = interimTimeVar;
                this.LifeTime = lifeTime;
                this.LifeTimeVar = lifeTimeVar;
                this.Speed = speed;
                this.SpeedDecay = speedDecay;
                this.SpeedLimit = speedLimit;
                this.PositionType = (tCCPositionType)positionType;
                this.IsPointToTarget = isPointToTarget;
                this.EmitterMode = (BulletMode)emitterMode;
                this.EmissionRate = emissionRate;
                this.EmissionAngle = emissionAngle;
                this.FieldAngle = fieldAngle;
                this.FieldAngleVar = fieldAngleVar;
                this.BeamCount = beamCount;
                this.BeamCountVar = beamCountVar;
                this.SpinSpeed = spinSpeed;
                this.BulletSpinSpeed = bulletSpinSpeed;
                this.BulletSpinSpeedVar = bulletSpinSpeedVar;
                this.Damage = damage;
                this.IsFollow = isFollow;
                this.Curvity = curvity;
                this.SinAmplitude = sinAmplitude;
                this.SinRate = sinRate;
            }
            catch (Exception e)
            {
                Utils.PrintException(e);
            }
            this.CollideMode = COLLIDE_MODE.Point;
            InitWithTotalBullets((uint)0);
        }

        /// <summary>
        /// 代码实例一
        /// TODO:
        /// </summary>
        /// <returns></returns>
        public static BulletEmitter BulletSample()
        {
            BulletEmitter bullet = new BulletEmitter();

            //bullet.Display = new CCSprite(ResID.PIC_bullet_sucai_007);
            bullet.Speed = 1000;
            bullet.EmitterMode = BulletMode.kBulletModeNormal;
            bullet.Duration = -1;
            bullet.EmissionRate = 10;
            bullet.EmissionAngle = 0;

            //bullet.InitWithTotalBullets(5);

            return bullet;
        }

        /// <summary>
        /// 邦定目标的碰撞器
        /// 在Actor中是用CCArmture中bone来当碰撞器
        /// </summary>
        /// <param name="collider"></param>
        public static void BindingCollider(params string[] collider)
        {
            foreach (string item in collider)
            {
                if (!m_sColliers.Contains(item))
                    m_sColliers.Add(item);
            }
        }
        public static void UnbindingCollider(params string[] collider)
        {
            foreach (string item in collider)
            {
                m_sColliers.Remove(item);
            }
        }
        /// <summary>
        /// 注册靶子
        /// 注册成靶子的Actor才可以被子弹击中
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool RegisterTarget(Actor target)
        {
            if (m_pTargets.Contains(target))
            {
                return false;
            }
            m_pTargets.Add(target);
            return true;
        }
        /// <summary>
        /// 解除靶子
        /// 当目标被击毁，就一定要解除靶子状态
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool UnregisterTarget(Actor target)
        {
            return m_pTargets.Remove(target);
        }

        /************************************************************************/
        /* 与目标碰撞检测相关*/
        /// <summary>
        /// 射线检测（ModeLaser）
        /// </summary>
        /// <param name="bullet"></param>
        /// <returns></returns>
        protected virtual bool LineCollision(ref Bullet bullet, Vector2 newPosition)
        {
            return false;
        }
        /// <summary>
        /// 顶点检测(Mode A B)
        /// </summary>
        /// <param name="bullet"></param>
        /// <returns></returns>      
        protected virtual bool VertexCollistion(ref Bullet bullet, Vector2 newPosition)
        {
            foreach (var target in m_pTargets)
            {
                if (!this.Filter.ShouldCollide(target.Filter)) continue;

                foreach (var collier in m_sColliers)
                {
                    Vector2 pos = this.ConvertToWorldSpace(newPosition);
                    Rect rect = target.Animable.LogicRectInWorld(collier);
                    if (!rect.IsValidity()) continue;
                    if (rect.ContainsPoint(pos))
                    {
                        bullet.live = false;
                        target.BulletHit(this, ref bullet, pos);
                        this.OnHitEffect(ref bullet);
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 矩形检测(Mode A B)
        /// </summary>
        /// <param name="bullet"></param>
        /// <returns></returns>
        protected virtual bool RectCollision(ref Bullet bullet, Vector2 newPosition)
        {
            foreach (var target in m_pTargets)
            {
                if (!this.Filter.ShouldCollide(target.Filter)) continue;

                foreach (var collier in m_sColliers)
                {
                    Rect box = bullet.display.BoundingBox;
                    Vector2 pos = this.ConvertToWorldSpace(newPosition);
                    box.origin.X = pos.X - box.size.width / 2;
                    box.origin.Y = pos.Y - box.size.height / 2;
                    Rect rect = target.Animable.LogicRectInWorld(collier);
                    if (!rect.IsValidity()) continue;
                    if (rect.IntersectsRect(box))
                    {
                        bullet.live = false;
                        target.BulletHit(this, ref bullet, pos);
                        this.OnHitEffect(ref bullet);
                        return true;
                    }
                }
            }
            return false;
        }

        protected virtual bool TargetCollistion(ref Bullet bullet, Vector2 newPosition)
        {
            return collideFuc(ref bullet, newPosition);
        }

        /// <summary>
        /// 与世界矩形（屏幕）检测
        /// </summary>
        /// <param name="bullet"></param>
        /// <param name="newPosition"></param>
        /// <returns></returns>
        private static Rect worldRect = new Rect(0, 0, Config.SCREEN_WIDTH, Config.SCREEN_HEIGHT);
        protected virtual bool WorldCollistion(ref Bullet bullet, Vector2 newPosition)
        {
            bool rel;
            if (worldNode != null)
            {
                worldRect = worldNode.BoundingBox;
            }
            rel = worldRect.ContainsPoint(this.ConvertToWorldSpace(newPosition));
            if (bullet.live) bullet.live = rel;
            return rel;
        }
        /************************************************************************/

        protected virtual void OnHitEffect(ref Bullet bullet)
        {
            if (m_pHitEffet != null)
            {
                if (m_pHitEffet.Parent == null || m_pHitEffet.Parent != worldNode)
                {
                    m_pHitEffet.RemoveFromParent();
                    worldNode.AddChild(m_pHitEffet);
                }
                m_pHitEffet.Start(bullet.display.Postion);
            }
            if (m_userEffect != null)
            {
                if (m_userEffect.Parent == null || m_userEffect.Parent != worldNode)
                {
                    m_userEffect.RemoveFromParent();
                    worldNode.AddChild(m_userEffect);
                }
                m_userEffect.Start(bullet.display.Postion);
            }
        }

        /// <summary>
        /// 添加一颗子弹
        /// </summary>
        /// 
        protected virtual Bullet AddBullet()
        {
            Bullet bullet;
            if (this.IsFull())
            {
                bullet = new Bullet();
                m_pBullets.Add(bullet);
                this.m_uTotalBullets = (uint)m_pBullets.Count;
            }
            else
            {
                bullet = m_pBullets[(int)m_uBulletCount];
                //Console.WriteLine("live:" + bullet.live);
            }
            this.InitBullet(ref bullet);

            ++m_uBulletCount;
            ++AllBulletCount;

            return bullet;
        }

        protected virtual bool InitWithTotalBullets(uint numberOfBullet)
        {
            m_uTotalBullets = numberOfBullet;

            for (int i = 0; i < m_uTotalBullets; i++)
            {
                m_pBullets.Add(new Bullet());
            }

            m_uAllocatedBullets = numberOfBullet;

            m_bIsActive = true;

            return true;
        }

        protected virtual void InitBullet(ref Bullet bullet)
        {
            bullet.live = true;
            bullet.pos = this.Postion;

            if (this.usingBatch)
            {
                SpriteBatchManager.Instance.WorldNode = WorldNode;
                batchNode.ContextSize = WorldNode.ContextSize;
                if (bullet.display == null)
                {
                    bullet.display = (BulletDisplay)this.m_pDisplay.Copy();
                    bullet.display.Play();
                    batchNode.AddChild(bullet.display.DisplayNode);
                }
            }
            else
            {
                if (bullet.display == null)
                {
                    bullet.display = (BulletDisplay)this.m_pDisplay.Copy();
                    bullet.display.Play();
                    WorldNode.AddChild(bullet.display);
                }
                else if (bullet.display.Parent != WorldNode)
                {
                    //最好就重新生成一个，否则粒子类型的display会有问题（就是不会update了）
                    bullet.display.RemoveFromParent();
                    bullet.display.Dispose();
                    bullet.display = (BulletDisplay)this.m_pDisplay.Copy();
                    bullet.display.Play();
                    WorldNode.AddChild(bullet.display);
                }
            }

            // position
            Vector2 wPos = -worldNode.Postion;
            if (m_ePositionType == tCCPositionType.kCCPositionTypeFree)
            {
                bullet.startPos = this.ConvertToWorldSpace(wPos);
            }
            else if (m_ePositionType == tCCPositionType.kCCPositionTypeRelative)
            {
                bullet.startPos = this.Postion;
            }

            bullet.deltaRotation = 0;
            bullet.liveToTime = 0;

            bullet._modeA.speed = _modeA.speed - _modeA.speedVar * MathHelper.Random_minus0_1();
            bullet._modeA.spinSpeed = _modeA.spinSpeed - _modeA.spinSpeedVar * MathHelper.Random_minus0_1();
            bullet._modeA.dir = 0;

            bullet.lifeTime = _modeA.lifeTime - _modeA.lifeTimeVar * MathHelper.Random_minus0_1();
            if (m_bIsFollow)
            {
                bullet._modeA.target = GetOneOfTarget();
            }
        }

        public void StartSystem()
        {
            m_bIsActive = true;
            m_fElapsed = 0;
            m_fInterim = 0;
            m_fEmissionAngleCount = m_fEmissionAngle;

            v_fDuration = m_fDuration - m_fDurationVar * MathHelper.Random_minus0_1();
            v_fInterimTime = m_fInterimTime - m_fInterimTimeVar * MathHelper.Random_minus0_1();

            m_bIsStop = false;
            m_bIsPause = false;
        }

        public void PauseSystem()
        {
            m_bIsActive = false;
            m_fElapsed = m_fDuration;
            m_fEmitCounter = 0;

            v_fDuration = m_fDuration - m_fDurationVar * MathHelper.Random_minus0_1();
            v_fInterimTime = m_fInterimTime - m_fInterimTimeVar * MathHelper.Random_minus0_1();

            m_bIsPause = true;
        }

        public void ResumeSystem()
        {
            m_bIsActive = true;
            m_fElapsed = 0;
            m_fInterim = 0;

            v_fDuration = m_fDuration - m_fDurationVar * MathHelper.Random_minus0_1();
            v_fInterimTime = m_fInterimTime - m_fInterimTimeVar * MathHelper.Random_minus0_1();

            m_bIsPause = false;
        }

        public void StopSystem()
        {
            m_bIsActive = false;
            m_fElapsed = m_fDuration;
            m_fEmitCounter = 0;

            v_fDuration = m_fDuration - m_fDurationVar * MathHelper.Random_minus0_1();
            v_fInterimTime = m_fInterimTime - m_fInterimTimeVar * MathHelper.Random_minus0_1();

            m_bIsPause = false;
            m_bIsStop = true;

        }

        public void ResetSystem()
        {
            m_bIsActive = true;
            m_fElapsed = 0;
            for (m_uBulletIdx = 0; m_uBulletIdx < m_uBulletCount; ++m_uBulletIdx)
            {
                Bullet p = m_pBullets[(int)m_uBulletIdx];
                p.live = true;
            }
            m_bIsPause = false;
        }

        public bool IsFull()
        {
            return (m_uBulletCount == m_uTotalBullets);
        }

        public virtual void Recycling()
        {
            //回收所有子弹
            foreach (var item in m_pBullets)
            {
                if (item.display != null)
                {
                    item.display.IsVisible = false;
                    item.display.RemoveFromParent(true);
                    item.display.Dispose();
                    item.display = null;
                }
                item.live = false;
            }
            m_pBullets.Clear();
            AllBulletCount -= (int)m_uBulletCount;
            m_uBulletIdx = 0;
            m_uBulletCount = 0;
            m_uTotalBullets = 0;
            //
        }

        public void TransToGem(DropType droptype = DropType.Drop_Gem_Blue_2)
        {
            DropConfig config = new DropConfig();
            config.speed = 50;
            config.speedVar = 20;
            config.speedDecay = -2;
            config.speedLimit = 20;
            config.angleVar = 360;
            config.waitingTime = 1.5f;
            config.waitingTimeVar = 0.5f;

            foreach (var item in m_pBullets)
            {
                if (item.live && item.display != null)
                {
                    DrapGem drop = null;
                    switch (droptype)
                    {
                        case DropType.Drop_Gem_Blue_1:
                            {
                                DropGemBlue1.Emitter.Config = config;
                                drop = (DrapGem)DropGemBlue1.Emitter.AddDrap(item.display.Postion);
                            }
                            break;
                        case DropType.Drop_Gem_Blue_2:
                            {
                                DropGemBlue2.Emitter.Config = config;
                                drop = (DrapGem)DropGemBlue2.Emitter.AddDrap(item.display.Postion);
                            }
                            break;
                        case DropType.Drop_Gem_Yellow_1:
                            {
                                DropGemYellow1.Emitter.Config = config;
                                drop = (DrapGem)DropGemYellow1.Emitter.AddDrap(item.display.Postion);
                            }
                            break;
                        case DropType.Drop_Gem_Yellow_2:
                            {
                                DropGemYellow2.Emitter.Config = config;
                                drop = (DrapGem)DropGemYellow2.Emitter.AddDrap(item.display.Postion);
                            }
                            break;
                        case DropType.Drop_Gem_Green_1:
                            {
                                DropGemGreen1.Emitter.Config = config;
                                drop = (DrapGem)DropGemGreen1.Emitter.AddDrap(item.display.Postion);
                            }
                            break;
                        case DropType.Drop_Gem_Green_2:
                            {
                                DropGemGreen2.Emitter.Config = config;
                                drop = (DrapGem)DropGemGreen2.Emitter.AddDrap(item.display.Postion);
                            }
                            break;
                        default:
                            break;
                    }
                    if (drop != null)
                    {
                        drop.ShowEffect = true;
                        item.live = false;
                        item.display.IsVisible = false;
                    }

                }
            }
        }

        protected CCNode GetTarget()
        {
            foreach (var target in m_pTargets)
            {
                if (this.Filter.ShouldCollide(target.Filter))
                {
                    return target;
                }
            }
            return null;
        }

        protected CCNode GetOneOfTarget()
        {
            List<CCNode> targets = GetTargets();
            if (targets.Count >= 1)
            {
                return targets[MathHelper.Random_minus0_n(targets.Count - 1)];
            }
            else
            {
                return null;
            }
        }

        protected List<CCNode> GetTargets()
        {
            List<CCNode> targets = new List<CCNode>();
            foreach (var target in m_pTargets)
            {
                if (this.Filter.ShouldCollide(target.Filter))
                {
                    targets.Add(target);
                }
            }
            return targets;
        }

        protected Vector2 ConvertToEmitterSpace(CCNode worldNode)
        {
            return this.ConvertToNodeSpaceAR(worldNode.Postion) + WorldNode.Postion;
        }

        protected float TargetAngle()
        {
            CCNode target = GetTarget();
            if (target != null)
            {
                Vector2 pos = this.ConvertToWorldSpace(this.Postion) - WorldNode.Postion;
                Vector2 dir = target.Postion - pos;
                return dir.ToDegrees();
            }
            return m_fEmissionAngleCount;
        }

        protected virtual void MakeBeam()
        {
            Vector2 newDir;
            if (m_fFieldAngleVar > m_fFieldAngle) m_fFieldAngleVar = m_fFieldAngle;

            int v_uBeamCount = (int)(m_uBeamCount - m_uBeamCountVar * MathHelper.Random_minus0_1());
            float v_fFieldAngle = m_fFieldAngle - m_fFieldAngleVar * MathHelper.Random_minus0_1();
            float perAngle = v_fFieldAngle;

            float tempAngle;
            if (v_uBeamCount > 1)
            {
                perAngle = v_fFieldAngle / (v_uBeamCount - 1);
                float beginAngle = this.m_fEmissionAngleCount - v_fFieldAngle / 2;
                for (int i = 0; i < v_uBeamCount; i++)
                {
                    Bullet b = this.AddBullet(); ;
                    float angle;
                    if (m_fFieldAngle == 360)
                    {
                        angle = i * (m_fFieldAngle / v_uBeamCount) + this.m_fEmissionAngleCount;
                    }
                    else
                    {
                        angle = beginAngle + i * perAngle;
                    }
                    //                     newDir.X = MathHelper.Sin(MathHelper.DegreesToRadians(angle));
                    //                     newDir.Y = MathHelper.Cos(MathHelper.DegreesToRadians(angle));
                    tempAngle = angle * 0.01745329f;
                    newDir.X = MathHelper.Sin(tempAngle);
                    newDir.Y = MathHelper.Cos(tempAngle);
                    b._modeA.dir = newDir;
                }
            }
            else
            {
                Bullet b = this.AddBullet(); ;

                //                 newDir.X = MathHelper.Sin(MathHelper.DegreesToRadians(m_fEmissionAngleCount));
                //                 newDir.Y = MathHelper.Cos(MathHelper.DegreesToRadians(m_fEmissionAngleCount));
                tempAngle = m_fEmissionAngleCount * 0.01745329f;
                newDir.X = MathHelper.Sin(tempAngle);
                newDir.Y = MathHelper.Cos(tempAngle);
                b._modeA.dir = newDir;
            }
        }

        //! should be overridden by subclasses
        protected virtual void UpdateQuadWithBullet(ref Bullet bullet, Vector2 newPosition)
        {
            if (bullet.display != null)
            {
                if (this.usingBatch)
                {
                    bullet.display.DisplayNode.IsVisible = bullet.live;
                    bullet.display.DisplayNode.Postion = this.ConvertToWorldSpace(newPosition) - WorldNode.Postion;
                    bullet.display.DisplayNode.Rotation = bullet.rotation + bullet.display.RotationOffset;
                    bullet.display.DisplayNode.ZOrder = PlayingScene.ZOrder_bullet - (int)newPosition.Y;
                }
                else
                {
                    bullet.display.IsVisible = bullet.live;
                    bullet.display.Postion = this.ConvertToWorldSpace(newPosition) - WorldNode.Postion;
                    bullet.display.Rotation = bullet.rotation;
                    bullet.display.ZOrder = PlayingScene.ZOrder_bullet - (int)newPosition.Y;
                }

            }
        }

        /*public override void OnUpdate(float dt)*/
        public virtual void OnUpdate(float dt)
        {
            //base.OnUpdate(dt);

            Vector2 wPos = -worldNode.Postion;

            //发射器相关
            if (m_bIsActive && m_fEmissionRate != 0)
            {
                //this.m_fEmissionAngle += this.m_fSpinSpeed * dt;
                this.m_fEmissionAngleCount += this.m_fSpinSpeed * dt;
                float rate = 1.0f / m_fEmissionRate;

                //Console.WriteLine("m_uBulletCount:" + m_uBulletCount);
                //Console.WriteLine("m_uTotalBullets:" + m_uTotalBullets);

                m_fEmitCounter += dt;

                while (m_uBulletCount <= m_uTotalBullets && m_fEmitCounter > rate)
                {
                    if (this.m_bIsPointToTarget)
                    {
                        this.m_fEmissionAngleCount = this.TargetAngle();
                    }
                    this.MakeBeam();

                    m_fEmitCounter -= rate;
                }

                m_fElapsed += dt;
                if (m_fDuration != -1 && v_fDuration < m_fElapsed)
                {
                    this.PauseSystem();
                }
            }
            else if (m_bIsPause)
            {
                m_fInterim += dt;
                if (m_fInterimTime != -1 && m_fInterim >= v_fInterimTime)
                {
                    this.ResumeSystem();
                }
            }

            m_uBulletIdx = 0;

            Vector2 currentPosition = wPos;
            if (m_ePositionType == tCCPositionType.kCCPositionTypeFree)
            {
                currentPosition = this.ConvertToWorldSpace(currentPosition);
            }
            else if (m_ePositionType == tCCPositionType.kCCPositionTypeRelative)
            {
                currentPosition = this.Postion;
            }

            if (this.IsVisible)
            {
                while (m_uBulletIdx < m_uBulletCount)
                {
                    Bullet p = m_pBullets[(int)m_uBulletIdx];

                    if (p.live)
                    {
                        Vector2 tmp, dir;
                        //减速
                        if (_modeA.speedDecay > 0)
                        {
                            if (p._modeA.speed < _modeA.speedLimit)
                                p._modeA.speed += _modeA.speedDecay;
                            else
                                p._modeA.speed = _modeA.speedLimit;
                        }
                        else if (_modeA.speedDecay < 0)
                        {
                            if (p._modeA.speed > _modeA.speedLimit)
                                p._modeA.speed += _modeA.speedDecay;
                            else
                                p._modeA.speed = _modeA.speedLimit;
                        }
                        //跟踪
                        if (this.m_bIsFollow && m_pTargets.Contains((Actor)p._modeA.target))
                        {
                            Vector2 tDir = (p._modeA.target.Postion - p.display.Postion).Normalize;
                            p._modeA.dir = (p._modeA.dir + tDir * m_fCurvity).Normalize;
                        }
                        //简谐
                        float t = p._modeA.dir.ToRadians() + (MathHelper.Sin(p.sinRotation) + MathHelper.Cos(p.sinRotation)) * m_fSinAmplitude;
                        //
                        dir.X = MathHelper.Sin(t);
                        dir.Y = MathHelper.Cos(t);

                        tmp = dir * p._modeA.speed * dt;
                        p.pos = p.pos + tmp;
                        // angle
                        p.deltaRotation += p._modeA.spinSpeed * dt;
                        p.rotation = dir.ToDegrees() + p.deltaRotation;
                        p.sinRotation += m_fSinRate;

                        p.liveToTime += dt;
                        if (_modeA.lifeTime != -1 && p.liveToTime >= p.lifeTime)
                        {
                            p.live = false;
                        }

                        // size
                        p.size += (p.deltaSize * dt);
                        p.size = 0.0f > p.size ? 0.0f : p.size;

                        Vector2 newPos;

                        if (m_ePositionType == tCCPositionType.kCCPositionTypeFree || m_ePositionType == tCCPositionType.kCCPositionTypeRelative)
                        {
                            Vector2 diff = currentPosition - p.startPos;
                            newPos = p.pos - diff;
                        }
                        else
                        {
                            newPos = p.pos;
                        }
                        //与世界（屏幕）检测
                        if (WorldCollistion(ref p, newPos))
                        {
                            //与目标检测
                            //this.VertexCollistion(ref p, newPos);
                            this.TargetCollistion(ref p, newPos);
                        }

                        UpdateQuadWithBullet(ref p, newPos);

                        // update bullet counter
                        ++m_uBulletIdx;
                    }
                    else
                    {
                        if (m_uBulletIdx != m_uBulletCount - 1)
                        {
                            var temp = m_pBullets[(int)m_uBulletIdx];
                            m_pBullets[(int)m_uBulletIdx] = m_pBullets[(int)m_uBulletCount - 1];
                            m_pBullets[(int)m_uBulletCount - 1] = temp;
                        }

                        --m_uBulletCount;
                        --AllBulletCount;
                    }
                }
            }
            //IsVisible
        }

    }
}
