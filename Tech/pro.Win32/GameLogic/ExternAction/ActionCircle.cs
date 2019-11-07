
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MatrixEngine.Engine;
using MatrixEngine.Math;
using MatrixEngine.Cocos2d;

namespace Thunder.GameLogic.ExternAction
{
    public class ActionCircle : MCActionInterval
    {
        float angle;
        float radius;
        float startAngle;

        Vector2 center;
        CCNode target;

        Vector2 oldPos;

        //正为顺时，负逆
        int dir = 1;

        public ActionCircle(float time, Vector2 center)
            : base(time)
        {
            this.angle = 360;
            this.center = center;
        }

        public ActionCircle(float time, Vector2 center, float _angle)
            : base(time)
        {
            if (_angle >= 0)
            {
                dir = 1;
            }
            else
            {
                dir = -1;
            }
            this.angle = MathHelper.Abs(_angle);
            this.center = center;
        }

        public override CCAction Reverse()
        {
            ActionCircle action = new ActionCircle(GetTime(), center, this.angle);
            action.dir = -this.dir;
            return action;
        }

        protected override void StartWithTarget(CCNode node)
        {
            target = node;
            this.oldPos = target.Postion;
            this.startAngle = (this.center - target.Postion).ToDegrees();

            double a1 = (this.target.PostionX - this.center.X) * (this.target.PostionX - this.center.X);
            double a2 = (this.target.PostionY - this.center.Y) * (this.target.PostionY - this.center.Y);
            radius = (float)Math.Sqrt(a1 + a2);
        }

        protected override void OnUpdate(float percent)
        {
            float curAngle = 180 + startAngle + percent * angle * dir;
            target.PostionX = this.center.X + radius * MathHelper.Sin(MathHelper.DegreesToRadians(curAngle));
            target.PostionY = this.center.Y + radius * MathHelper.Cos(MathHelper.DegreesToRadians(curAngle));
        }
    }
}
