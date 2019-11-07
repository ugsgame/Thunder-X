using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Thunder.GameLogic.Gaming.Actors;

using MatrixEngine;
using MatrixEngine.Cocos2d;
using MatrixEngine.CocoStudio.Armature;
using MatrixEngine.Math;

namespace Thunder.GameLogic.Gaming
{
    public class Animable : CCObject
    {
        private ActorBehavior spawn;
        public MatrixEngine.CocoStudio.Armature.CCAnimation animation;
        public CCArmature armature;

        private string resPath;
        private static List<string> paths = new List<string>();

        public Animable(Spawn _spawn, string resPath, string armName)
        {
            this.spawn = _spawn;
            //Console.WriteLine("Animable resPath=" + resPath + "| armName=" + armName + "|");
            ///异步加载可能用到
            //temp--
            //CCArmDataManager.AddArmatureFile(resPath);
            ArmatureResManager.Instance.Add(resPath);
            //--temp
            this.resPath = resPath;

            armature = new CCArmature(armName);
            animation = armature.GetAnimation();
            animation.SetFrameEvent(_spawn);
            animation.SetMovementEvent(_spawn);

            this.spawn.AddChild(armature);

        }

        protected override void Dispose(bool disposing)
        {
            this.spawn.RemoveChild(armature);
            //if (disposing)
            {
                //要先释放animation,再释放armature，因为CCArmature 会再释放一次，但有安全检测
                if (animation != null)
                {
                    animation.Dispose();
                    animation = null;
                }
                if (armature != null)
                {
                    armature.Dispose();
                    armature = null;
                }
            }
            spawn = null;

            base.Dispose(disposing);
        }

        public string ArmatureRes
        {
            get { return this.resPath; }
        }

        /// <summary>
        /// 注册碰撞
        /// </summary>
        public void RegisterCollider()
        {
            armature.BindGameActor(spawn);
        }
        /// <summary>
        /// 解除碰撞
        /// 这个游戏可以不用box2d做碰撞
        /// </summary>
        public void UnRegisterCollider()
        {
            armature.UnBindGameActor();
            this.spawn.RemoveChild(armature);
            armature = null;

            spawn.RemoveRigibody();
        }

        public string CurrentAnim()
        {
            if (animation != null)
            {
                return animation.GetCureentMovementID();
            }
            return "null";
        }

        /// <summary>
        /// 设置动画碰撞过滤
        /// </summary>
        /// <param name="filter"></param>
        public void SetFilter(CCColliderFilter filter)
        {
            if (armature != null)
            {
                armature.SetColliderFilter(filter);
            }
        }

        public void PlayAnim(string name, bool loop)
        {
            //Console.WriteLine("PlayAnim name=" + name + "| loop=" + loop + "| " + _Spawn);
            //Utils.PrintStack();
            if (animation != null)
            {
                animation.Play(name, loop);
            }
        }

        public void FilpX(bool filp)
        {
            if (armature != null)
            {
                if (filp)
                {
                    armature.ScaleX = -1;
                }
                else
                {
                    armature.ScaleX = 1;
                }
            }

        }

        public float Scale
        {
            set
            {
                armature.SetScale(value);
            }
            get
            {
                return armature.GetScale();
            }
        }

        public void Resume()
        {
            if (animation != null)
            {
                animation.Resume();
            }
        }

        public void Pause()
        {
            if (animation != null)
            {
                animation.Pause();
            }
        }

        public void Stop()
        {
            if (animation != null)
            {
                animation.Stop();
            }
        }

        public void RunAction(CCAction action)
        {
            if (armature != null)
            {
                armature.RunAction(action);
            }
        }

        public Rect LogicRect(string collier)
        {
            if (armature != null)
            {
                return armature.GetBoneRect(collier);
            }
            return Rect.Zero;
        }

        public Rect LogicRectInWorld(string collier)
        {
            if (armature != null)
            {
                return armature.GetBoneRectInWorld(collier);
            }
            return Rect.Zero;
        }

        public Vector2 LogicPoint(string boneName)
        {
            if (armature != null)
            {
                return armature.GetBonePosition(boneName);
            }
            return Vector2.Zero;
        }

        public Vector2 LogicPointInWorld(string boneName)
        {
            if (armature != null)
            {
                return armature.GetBonePositionInWorld(boneName);
            }
            return Vector2.Zero;
        }

        public bool BoneIsDisplay(string boneNme)
        {
            if (armature != null)
            {
                return armature.BoneIsDisplay(boneNme);
            }
            return false;
        }
    }
}
