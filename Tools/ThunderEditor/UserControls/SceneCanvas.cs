using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using Thunder.GameLogic.Gaming.Actors;
using ThunderEditor.Editor;
using ThunderEditor.Logic;

namespace ThunderEditor.UserControls
{
    /// <summary>
    /// 场景画布
    /// </summary>
    public class SceneCanvas : Canvas/*ZoomableCanvas*/
    {
        public enum Mode
        {
            Null,           //空
            Selecting,      //选择
            Brushing,       //添加元素
            Droping,        //拖动元素
            Transforming,   //变换世界
        }

        public Mode PreviousMode
        {
            get;
            protected set;
        }
        public Mode CurrentMode
        {
            get;
            protected set;
        }

        private Brush selectedDrawingBrush = Brushes.LightGreen;
        private DrawingVisual selectedDrawingVisual = new DrawingVisual();
        private Pen drawingPen = new Pen(Brushes.SteelBlue, 3);

        private Vector drapClickOffset;
        private Vector worldClickOffset;

        private DrawingVisual selectedVisual;
        private EActorType displayBrush;

        //private Point selectionSquareTopLeft;

        //Transform
        //
        private Size curSize;

        private Event curEvent;

        private List<Visual> visuals = new List<Visual>();
        private Dictionary<Visual, Object> visualData = new Dictionary<Visual, Object>();


        public SceneCanvas()
        {
            CurrentMode = Mode.Selecting;
            //添加事件
            this.Initialized += SceneCanvas_Initialized;
            this.Loaded += SceneCanvas_Loaded;

            this.MouseEnter += SceneCanvas_MouseEnter;
            this.MouseLeave += SceneCanvas_MouseLeave;
            this.MouseLeftButtonDown += SceneCanvas_MouseLeftButtonDown;
            this.MouseLeftButtonUp += SceneCanvas_MouseLeftButtonUp;
            this.MouseRightButtonDown += SceneCanvas_MouseRightButtonDown;
            this.MouseRightButtonUp += SceneCanvas_MouseRightButtonUp;
            this.MouseWheel += SceneCanvas_MouseWheel;
            this.MouseMove += SceneCanvas_MouseMove;
            this.SizeChanged += SceneCanvas_SizeChanged;
            this.ContextMenuOpening += SceneCanvas_ContextMenuOpening;

            this.PreviewKeyDown += SceneCanvas_PreviewKeyDown;
            this.PreviewKeyUp += SceneCanvas_PreviewKeyUp;
            //
            //
            PopSelectedMenu();
        }

        public void PopSelectedMenu()
        {

            ContextMenu selectedMenu = new ContextMenu();

            MenuItem menuItem1 = new MenuItem();
            menuItem1.Name = "Remove";
            menuItem1.Header = "删除";
            menuItem1.Click += new RoutedEventHandler(SceneCanvas_SelectedMenu_Remove_Click);
            //
            //Add
            //
            selectedMenu.Items.Add(menuItem1);
            this.ContextMenu = selectedMenu;
        }

        void SceneCanvas_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            //保存当前编辑的关
        }

        void SceneCanvas_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //Console.WriteLine("p key:" + e.Key);
            if (CurrentMode == Mode.Selecting)
            {
                if (selectedVisual != null)
                {
                    EActor actor = (EActor)visualData[selectedVisual];
                    if (Keyboard.IsKeyDown(Key.Right))
                    {
                        actor.PositionX += 1f;
                        actor.Draw();
                        MainWindow.propertyInstanse.SelectedObject = actor; 
                    }
                    else if (Keyboard.IsKeyDown(Key.Left))
                    {
                        actor.PositionX -= 1f;
                        actor.Draw();
                        MainWindow.propertyInstanse.SelectedObject = actor; 
                    }
                    else if (Keyboard.IsKeyDown(Key.Up))
                    {
                        actor.PositionY -= 1f;
                        actor.Draw();
                        MainWindow.propertyInstanse.SelectedObject = actor; 
                    }
                    else if (Keyboard.IsKeyDown(Key.Down))
                    {
                        actor.PositionY += 1f;
                        actor.Draw();
                        MainWindow.propertyInstanse.SelectedObject = actor; 
                    }
                    else if (Keyboard.IsKeyDown(Key.Delete))
                    {
                        DeleteSelected();
                    }
//                     else if (Keyboard.IsKeyDown(Key.Space))
//                     {
//                         this.ClearSelected();
//                         this.CurrentMode = Mode.Transforming;
//                         this.Cursor = BitmapCursor.Create(EditorRes.Instance.Img_Pointer_Hand);
//                     }
                }
            }
        }

        void SceneCanvas_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            if (selectedVisual != null)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        public void OnResetScene(Event Event)
        {
            curEvent = Event;

            this.ClearBrush();
            this.ClearSelected();
            this.ClearVisual();

            //画在屏幕中间
            //             curEvent.World.PositionX = (float)this.ActualWidth / 2;
            //             curEvent.World.PositionY = (float)this.ActualHeight / 2;
            curEvent.World.Draw();
            this.AddVisual(curEvent.World.Visual, curEvent.World);

            //添加元素
            foreach (var item in curEvent.World.ActorManager.GetActors())
            {
                item.Draw();
                this.AddVisual(item.Visual, item);
            }
            //

        }

        void SceneCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //throw new NotImplementedException();
            //Console.WriteLine(e.NewSize);
            curSize = e.NewSize;
        }

        private void SceneCanvas_Initialized(object sender, EventArgs e)
        {
            //画屏幕矩形

        }

        private void SceneCanvas_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void SceneCanvas_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {

        }

        void SceneCanvas_MouseLeave(object sender, MouseEventArgs e)
        {
            if (curEvent == null) return;
            /*
            switch (CurrentMode)
            {
                case Mode.Transforming:
                    this.CurrentMode = Mode.Selecting;
                    break;
                default:
                    break;
            }
            */
        }

        private void SceneCanvas_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (curEvent == null) return;
            Point pointClicked = e.GetPosition(this);
            //
            this.ClearBrush();

            DrawingVisual visual = this.GetVisual(pointClicked);
            if (visual == null)
            {
                this.ClearSelected();
                this.CurrentMode = Mode.Transforming;
                this.Cursor = BitmapCursor.Create(EditorRes.Instance.Img_Pointer_Hand);
                worldClickOffset = curEvent.ActorManager.WorldOrigin - pointClicked;
            }
            else
            {
                //右键选中对象
                if (selectedVisual != null && selectedVisual == visual)
                {
                    EActor actor = (EActor)visualData[visual];
                    actor.IsSelected = true;
                    actor.Draw();
                }
                else
                {
                    this.CurrentMode = Mode.Transforming;
                    this.Cursor = BitmapCursor.Create(EditorRes.Instance.Img_Pointer_Hand);
                    worldClickOffset = curEvent.ActorManager.WorldOrigin - pointClicked;

                    this.ClearSelected();
                }
            }
            //clear select
        }
        void SceneCanvas_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Focus();
            if (curEvent == null) return;
            Point pointClicked = e.GetPosition(this);

            switch (CurrentMode)
            {
                case Mode.Transforming:
                    {
                        this.ClearBrush();
                    }
                    break;
                default:
                    break;
            }
        }

        private void SceneCanvas_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (curEvent == null) return;
            Point pointClicked = e.GetPosition(this);

            switch (CurrentMode)
            {
                case Mode.Null:
                    break;
                case Mode.Selecting:
                    {
                        DrawingVisual visual = this.GetVisual(pointClicked);
                        if (visual != null)
                        {
                            if (visualData[visual] is EWorld)
                            {
                                EWorld world = (EWorld)visualData[visual];
                                MainWindow.propertyInstanse.SelectedObject = world;

                                this.ClearSelected();
                            }
                            else if (visualData[visual] is EActor)
                            {
                                EActor actor = (EActor)visualData[visual];
                                MainWindow.propertyInstanse.SelectedObject = actor;
                                if (actor != null)
                                {
                                    actor.IsSelected = true;
                                    actor.Draw();
                                    drapClickOffset = actor.GetPostion() - pointClicked;

                                    if (selectedVisual != null && selectedVisual != visual)
                                    {
                                        this.ClearSelected();
                                    }
                                    selectedVisual = visual;
                                    CurrentMode = Mode.Droping;
                                }
                            }

                        }
                        else
                        {
                            MainWindow.propertyInstanse.SelectedObject = null;
                            this.ClearSelected();
                        }
                    }
                    break;
                case Mode.Brushing:
                    {
                        if (displayBrush != null)
                        {

                            EActor actor = new EActor(displayBrush);
                            curEvent.ActorManager.AddActor(actor);
                            Point newPos = new Point();
                            newPos.X = pointClicked.X - curEvent.ActorManager.WorldOrigin.X;
                            newPos.Y = pointClicked.Y - curEvent.ActorManager.WorldOrigin.Y;
                            actor.SetPosition(newPos);
                            actor.Draw();
                            this.AddVisual(actor.Visual, actor);
                        }

                        this.ClearSelected();
                    }
                    break;
                case Mode.Droping:
                    break;
                default:
                    break;
            }
        }

        private void SceneCanvas_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (curEvent == null) return;
            Point pointClicked = e.GetPosition(this);

            switch (CurrentMode)
            {
                case Mode.Null:
                    break;
                case Mode.Selecting:
                    break;
                case Mode.Brushing:
                    break;
                case Mode.Droping:
                    {
                        CurrentMode = Mode.Selecting;
                        EActor actor = (EActor)visualData[selectedVisual];
                        MainWindow.propertyInstanse.SelectedObject = actor;
                    }
                    break;
                case Mode.Transforming:
                    CurrentMode = Mode.Selecting;
                    break;
                default:
                    break;
            }
        }

        private void SceneCanvas_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (curEvent == null) return;
            Point pointClicked = e.GetPosition(this);

            switch (CurrentMode)
            {
                case Mode.Null:
                    break;
                case Mode.Brushing:
                    {

                    }
                    break;
                case Mode.Droping:
                    {
                        if (selectedVisual != null)
                        {
                            EActor actor = (EActor)visualData[selectedVisual];
                            actor.SetPosition(e.GetPosition(this) + drapClickOffset);
                            actor.Draw();
                            //TODO:如果在这里指定属性，会卡死你~~
                            //MainWindow.propertyInstanse.SelectedObject = actor; 

                        }
                    }
                    break;
                case Mode.Transforming:
                    {
                        curEvent.ActorManager.WorldOrigin = (e.GetPosition(this) + worldClickOffset);
                        curEvent.World.Draw();
                        curEvent.ActorManager.Draw();
                    }
                    break;
                default:
                    break;
            }
        }

        void SceneCanvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (curEvent == null) return;
            Point zoomCenter = e.GetPosition(this);
            //TODO:滑轮缩放
            /*
            //使用ZoomableCanvas缩放
            var x = Math.Pow(2, e.Delta / 10.0 / Mouse.MouseWheelDeltaForOneLine);
            this.Scale *= x;

            this.Width = curSize.Width;
            this.Height = curSize.Height;
            /*
            if (this.Scale >= 1.0f)
            {
                this.Scale = 1.0f;
            }
            else
            {
          
            }
            

            /*
            //使用RenderTransform缩放
            TransformGroup transformGroup = new TransformGroup();
            ScaleTransform scaletransform = new ScaleTransform();
            scaletransform.ScaleX = e.Delta / 10.0 / Mouse.MouseWheelDeltaForOneLine;
            scaletransform.ScaleY = e.Delta / 10.0 / Mouse.MouseWheelDeltaForOneLine;
            transformGroup.Children.Add(scaletransform);
            this.RenderTransform = transformGroup;
            */

            //Console.WriteLine("Scale:"+this.Scale);
        }

        void SceneCanvas_SelectedMenu_Remove_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender;
            if (menuItem.Name == "Remove")
            {
                DeleteSelected();
                CurrentMode = Mode.Selecting;
            }

        }
        /*******************************************/
        /**Visual**/
        protected override Visual GetVisualChild(int index)
        {
            return visuals[index];
        }
        protected override int VisualChildrenCount
        {
            get
            {
                return visuals.Count;
            }
        }

        public void AddVisual(Visual visual)
        {
            visuals.Add(visual);

            base.AddVisualChild(visual);
            base.AddLogicalChild(visual);
        }

        public void AddVisual(Visual visual, Object data)
        {
            this.AddVisual(visual);
            visualData[visual] = data;
        }

        public void DeleteVisual(Visual visual)
        {

            if (visualData[visual] != null)
            {
                visualData.Remove(visual);
            }

            visuals.Remove(visual);

            base.RemoveVisualChild(visual);
            base.RemoveLogicalChild(visual);
        }

        public void ClearVisual()
        {
            visualData.Clear();
            //visuals.Clear();

            foreach (var item in visuals)
            {
                base.RemoveVisualChild(item);
                base.RemoveLogicalChild(item);
            }
            visuals.Clear();
        }

        public DrawingVisual GetVisual(Point point)
        {
            HitTestResult hitResult = VisualTreeHelper.HitTest(this, point);
            return hitResult.VisualHit as DrawingVisual;
        }

        private List<DrawingVisual> hits = new List<DrawingVisual>();
        public List<DrawingVisual> GetVisuals(Geometry region)
        {
            hits.Clear();
            GeometryHitTestParameters parameters = new GeometryHitTestParameters(region);
            HitTestResultCallback callback = new HitTestResultCallback(this.HitTestCallback);
            VisualTreeHelper.HitTest(this, null, callback, parameters);
            return hits;
        }

        private HitTestResultBehavior HitTestCallback(HitTestResult result)
        {
            GeometryHitTestResult geometryResult = (GeometryHitTestResult)result;
            DrawingVisual visual = result.VisualHit as DrawingVisual;
            if (visual != null &&
                geometryResult.IntersectionDetail == IntersectionDetail.FullyInside)
            {
                hits.Add(visual);
            }
            return HitTestResultBehavior.Continue;
        }
        /**************************************************/

        public void GotoMode()
        {

        }

        /// <summary>
        /// 通过角色类型绑定画刷
        /// </summary>
        /// <param name="actorType"></param>
        public void BindingBrush(EActorType actorType)
        {
            if (curEvent != null)
            {
                displayBrush = actorType;
                CurrentMode = Mode.Brushing;

                this.Cursor = BitmapCursor.Create(displayBrush.Display);
            }

        }
        /// <summary>
        /// 清理画刷
        /// </summary>
        public void ClearBrush()
        {
            CurrentMode = Mode.Selecting;

            if (this.Cursor != null)
            {
                this.Cursor.Dispose();
                this.Cursor = null;
            }

        }

        public void ClearSelected()
        {
            if (selectedVisual != null)
            {
                EActor selectedActor = (EActor)visualData[selectedVisual];
                selectedActor.IsSelected = false;
                selectedActor.Draw();
                selectedVisual = null;
            }
        }

        protected void DeleteSelected()
        {
            if (selectedVisual != null)
            {
                EActor actor = (EActor)visualData[selectedVisual];
                curEvent.ActorManager.RemoveActor(actor);
                this.DeleteVisual(selectedVisual);
                selectedVisual = null;
            }
        }
    }
}
