using MatrixEngine.Cocos2d;
using MatrixEngine.Engine;
using MatrixEngine.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thunder.Game;

namespace Thunder.GameLogic.ExternAction
{
    public class ActionMoveFree : MCActionInterval
    {
        Rect moveRect;
        float moveSpeed;
        Vector2 moveDir;

        CCNode target;

        public ActionMoveFree(float time, float speed, Rect rect)
            : base(time)
        {
            moveSpeed = speed;
            moveRect = rect;
        }

        protected override void StartWithTarget(CCNode node)
        {
            target = node;
            moveDir = MathHelper.RadiansToVector2(MathHelper.Random_minus0_1() * MathHelper.Pi);
        }

        public override CCAction Reverse()
        {
            ActionMoveFree action = new ActionMoveFree(GetTime(), moveSpeed, moveRect);
            return action;
        }

        protected override void OnUpdate(float percent)
        {
            moveDir = AABB(moveDir);
            target.Postion += moveDir * moveSpeed * CCDirector.FPS;

            //Console.WriteLine("ActionMoveFree:" + percent);
        }

        protected Vector2 AABB(Vector2 dir)
        {

            float angle = dir.ToDegrees();
            Rect rect = moveRect;
            if (!rect.ContainsPoint(target.Postion))
            {
                //return Utils.AngleToVector(angle + 180);
                if (target.PostionX < rect.GetMinX())
                {
                    target.PostionX = rect.GetMinX();
                }
                else if (target.PostionX > rect.GetMaxX())
                {
                    target.PostionX = rect.GetMaxX();
                }

                if (target.PostionY < rect.GetMinY())
                {
                    target.PostionY = rect.GetMinY();
                }
                else if (target.PostionY > rect.GetMaxY())
                {
                    target.PostionY = rect.GetMaxY();
                }
                return Utils.AngleToVector(angle + 180);
            }
            else
            {
                return dir;
            }

        }
    }
}
