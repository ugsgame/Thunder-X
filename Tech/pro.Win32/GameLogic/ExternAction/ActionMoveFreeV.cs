using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MatrixEngine.Cocos2d;
using MatrixEngine.Engine;
using MatrixEngine.Math;

namespace Thunder.GameLogic.ExternAction
{
    public class ActionMoveFreeV : MCActionInterval
    {
        Rect moveRect;
        float moveSpeed;
        Vector2 moveDir;

        CCNode target;

        public ActionMoveFreeV(float time, float speed, Rect rect)
            : base(time)
        {
            moveSpeed = speed;
            moveRect = rect;
        }

        protected override void StartWithTarget(CCNode node)
        {
            target = node;
            moveDir.Y = MathHelper.Random_minus0_1()>0.5f?1:-1;
            moveDir.X = 0;
        }

        public override CCAction Reverse()
        {
            ActionMoveFreeV action = new ActionMoveFreeV(GetTime(), moveSpeed, moveRect);
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
                if (target.PostionY < rect.GetMinY())
                {
                    target.PostionY = rect.GetMinY();
                    dir.Y = 1;
                }
                else if (target.PostionY > rect.GetMaxY())
                {
                    target.PostionY = rect.GetMaxY();
                    dir.Y = -1;
                }
            }
            return dir;
        }
    }
}
