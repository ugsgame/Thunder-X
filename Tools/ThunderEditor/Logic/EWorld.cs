using MatrixEngine.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using ThunderEditor.Editor;

namespace ThunderEditor.Logic
{
    /// <summary>
    /// 相对编辑器的游戏世界
    /// </summary>
    public class EWorld:Transform
    {
        private Brush selectedDrawingBrush = Brushes.LightGreen;
        private Brush normalDrawingBrush = Brushes.LightSkyBlue;
        private Pen drawingPen = new Pen(Brushes.SteelBlue, 3);

        System.Windows.Size worldSize = new System.Windows.Size(600, 800);

        public System.Windows.Size Size
        {
            set { worldSize = value; }
            get { return worldSize; }
        }

        public double Width
        {
            set { worldSize.Width = value; }
            get { return worldSize.Width; }
        }

        public double Height
        {
            set { worldSize.Height = value; }
            get { return worldSize.Height; }
        }

        EActorManager actorManager;
        internal EActorManager ActorManager
        {
            set { actorManager = value; }
            get { return actorManager; }
        }

        bool selected;
        internal bool IsSelected
        {
            get { return selected; }
            set { selected = value; }
        }

        DrawingVisual visual;
        internal DrawingVisual Visual
        {
            get
            {
                return visual;
            }
        }

        internal Vector2 ToWorldPosition(Vector2 pos)
        {
            Vector2 origin = Vector2.Zero;
            origin.X = -this.PositionX - (float)Width / 2;
            origin.Y = -this.PositionY - (float)Height / 2;

            Vector2 newPos = Vector2.Zero;
            newPos.X = pos.X - origin.X;
            newPos.Y = -(pos.Y + origin.Y);
            return newPos;
        }

        internal Vector2 ToCanvasPosition(Vector2 pos)
        {
            Vector2 origin = Vector2.Zero;
            origin.X = this.PositionX + (float)Width / 2;
            origin.Y = this.PositionY + (float)Height / 2;

            Vector2 newPos = Vector2.Zero;
            newPos.X =  pos.X - origin.X;
            newPos.Y =  origin.Y - pos.Y;
            return newPos;
        }

        public EWorld()
        {
            this.visual = new DrawingVisual();

            worldSize.Width = EConfig.Instance.SceneWidth;
            worldSize.Height = EConfig.Instance.SceneHeight;
        }

        public EWorld(EActorManager manager)
        {
            this.visual = new DrawingVisual();
            this.actorManager = manager;

            worldSize.Width = EConfig.Instance.SceneWidth;
            worldSize.Height = EConfig.Instance.SceneHeight;
        }

        public virtual void Draw()
        {
            Point drawPoint = new Point(this.position.X + actorManager.WorldOrigin.X - Width / 2, this.position.X + actorManager.WorldOrigin.Y - Height / 2);
            if (selected)
            {
                using (DrawingContext dc = visual.RenderOpen())
                {
                    dc.PushOpacity(0.3f);
                    dc.DrawRectangle(selectedDrawingBrush, drawingPen, new System.Windows.Rect(drawPoint, this.worldSize));
                }
            }
            else
            {
                using (DrawingContext dc = visual.RenderOpen())
                {
                    dc.PushOpacity(0.3f);
                    dc.DrawRectangle(normalDrawingBrush, drawingPen, new System.Windows.Rect(drawPoint, this.worldSize));
                }
            }
        }
    }
}
