
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;
using MatrixEngine.Math;

namespace ThunderEditor.Logic
{
    /// <summary>
    /// 变换
    /// </summary>
    public class Transform
    {
        protected Vector2 position = Vector2.Zero;
        protected Vector2 worldPosition = Vector2.Zero;

        [Category("变换")]
        public virtual float PositionX
        {
            set 
            { 
                position.X = value;
                FlashWordPosition();
            }
            get { return position.X; }
        }
        [Category("变换")]
        public virtual float PositionY
        {
            set 
            { 
                position.Y = value;
                FlashWordPosition();
            }
            get { return position.Y; }
        }

        [Category("变换")]
        public virtual float WPositionX
        {
            set { worldPosition.X = value; }
            get { return worldPosition.X; }
        }
        [Category("变换")]
        public virtual float WPositionY
        {
            set { worldPosition.Y = value; }
            get { return worldPosition.Y; }
        }

        internal virtual Vector2 Position
        {
            set 
            { 
                position = value;
                FlashWordPosition();
            }
            get { return position; }
        }

        internal virtual Vector2 WorldPosition
        {
            get { return worldPosition; }
            set { worldPosition = value; }
        }

        float scale;
        [Category("变换")]
        public virtual float Scale
        {
            set { scale = value; }
            get { return scale; }
        }

        float rotation;
        [Category("变换")]
        public virtual float Rotation
        {
            set { rotation = value; }
            get { return rotation; }
        }

        bool flipH, flipV;
        [Category("变换")]
        public virtual bool FlipH
        {
            set { flipH = value; }
            get { return flipH; }
        }
        [Category("变换")]
        public virtual bool FlipV
        {
            set { flipV = value; }
            get { return flipV; }
        }

        public virtual void FlashWordPosition()
        {
            //throw new NotImplementedException();
        }
    }
}
