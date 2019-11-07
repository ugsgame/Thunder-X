using MatrixEngine.Cocos2d;
using MatrixEngine.Engine;
using MatrixEngine.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thunder.GameLogic.ExternAction
{
    public class ActionMoveFreeH : MCActionInterval
    {
        Rect moveRect;
        float moveSpeed;
        Vector2 moveDir;

        CCNode target;
        //TODO:由帧取得tick
        float tick = 0.017f;

        public ActionMoveFreeH(float time, float speed, Rect rect)
            : base(time)
        {
            moveSpeed = speed;
            moveRect = rect;
            moveDir.X = MathHelper.Random_minus0_1() > 0.5f ? 1 : -1;
            moveDir.Y = 0;
        }

        protected override void StartWithTarget(CCNode node)
        {
            target = node;
            moveDir.X = MathHelper.Random_minus0_1()>0.5f?1:-1;
            moveDir.Y = 0;
        }

        public override CCAction Reverse()
        {
            ActionMoveFreeH action = new ActionMoveFreeH(GetTime(), moveSpeed, moveRect);
            return action;
        }

        protected override void OnUpdate(float percent)
        {
            moveDir = AABB(moveDir);
            target.Postion += moveDir * moveSpeed * CCDirector.FPS;
        }

        protected Vector2 AABB(Vector2 dir)
        {
            Rect rect = moveRect;
            if (!rect.ContainsPoint(target.Postion))
            {
                //return Utils.AngleToVector(angle + 180);
                if (target.PostionX < rect.GetMinX())
                {
                    target.PostionX = rect.GetMinX();
                    dir.X = 1;
                }
                else if (target.PostionX > rect.GetMaxX())
                {
                    target.PostionX = rect.GetMaxX();
                    dir.X = -1;
                }
            }
            return dir;
        }
    }
}
