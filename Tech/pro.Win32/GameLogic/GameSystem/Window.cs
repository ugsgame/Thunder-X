using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MatrixEngine.Math;
using MatrixEngine.Cocos2d;
using MatrixEngine.CocoStudio;
using MatrixEngine.CocoStudio.GUI;


namespace Thunder.GameLogic.GameSystem
{
    /// <summary>
    /// 窗口基本类型，游戏所有逻辑窗口都归此类管理
    /// 同时它是一个UI管理器，所有UI都归窗口管理
    /// 
    /// </summary>
    public class Window : UILayout, ITouchEventListener
    {
        public enum Priority
        {
            PRIORITY_WINDOW,
            PRIORITY_DIALOG,
            PRIORITY_DIALOG_HIGH,
            PRIORITY_DIALOG_MSG,
            PRIORITY_DIALOG_SYSTEM,
            PRIORITY_INFO,
        }

        public interface StaticWindow
        {
        }

        public interface NormalClose
        {
        }

        public delegate void WindowShowHandle();


        public event WindowShowHandle initEvent;
        public event WindowShowHandle releaseEvent;


        //protected UILayout layer = new UILayout();
        protected bool isShow;
        protected UILayout WindowLayout = new UILayout();

        private Priority priority;
        private UIWidget showFrom;
        //private UIWidget _parentLayer;

        /// <summary>
        /// 创建默认优先级的窗口
        /// </summary>
        public Window()
            : this(Priority.PRIORITY_WINDOW)
        {
        }

        /// <summary>
        /// 独立图层管理
        /// </summary>
        /// <param name="priority"></param>
        public Window(UIWidget belongTo)
            : this(Priority.PRIORITY_WINDOW, belongTo)
        {
        }

        /// <summary>
        /// 在UI管理器的优先级
        /// </summary>
        /// <param name="priority"></param>
        public Window(Priority priority)
            : this(priority, null)
        {
        }

        public bool WindowLayoutEnable
        {
            get { return WindowLayout.Enabled; }
            set { WindowLayout.Enabled = value; }
        }

        /// <summary>
        /// 此初始化显示窗口不受到UI管理器管理，独立图层管理
        /// </summary>
        /// <param name="parentLayer"></param>
        public Window(Priority priority, UIWidget showFrom)
        {
            this.priority = priority;
            this.showFrom = showFrom ?? LAYER_WINDOW;
            WindowLayout.ZOrder = (int)priority;
            WindowLayout.AddChild(this);
            initEvent += init;
            releaseEvent += release;
        }

        public Priority GetPriority()
        {
            return priority;
        }

        public UIWidget GetShowFromLayer()
        {
            return showFrom;
        }

        /// <summary>
        /// 窗口显示前执行的初始化，每次显示前都调用
        /// </summary>
        public virtual void init()
        {
        }

        /// <summary>
        /// 窗口关闭执行的释放
        /// </summary>
        public virtual void release()
        {
        }

        protected void callInitEvent()
        {
            if (initEvent != null)
            {
                this.initEvent();
            }
        }

        protected void callReleaseEventEvent()
        {
            if (releaseEvent != null)
            {
                this.releaseEvent();
            }
        }

        /// <summary>
        /// 是否显示窗口
        /// </summary>
        public virtual void Show(bool b)
        {
            if (isShow != b)
            {
                if (b)
                {
                    callInitEvent();
                    Window.Add(this);
                    isShow = true;
                }
                else
                {
                    Window.Remove(this);
                    isShow = false;
                    callReleaseEventEvent();
                }
            }
            else if (isShow && b)
            {
                Window.Add(this);
            }
        }

        public virtual bool IsShow
        {
            get
            {
                return isShow;
            }
            set
            {
                this.Show(value);
            }
        }

        public void AddChildCenter(UIWidget node)
        {
            node.Postion = (this.Size - node.Size) / 2;
            base.AddChild(node);
        }

        public new void AddChild(UIWidget node)
        {
            base.AddChild(node);
        }

        public new void AddChild(UIWidget node, int zOrder)
        {
            base.AddChild(node, zOrder);
        }

        public new void AddChild(UIWidget node, int zOrder, int tag)
        {
            base.AddChild(node, zOrder, tag);
        }

        private void allChildrenAddTouchListener(UIWidget widget)
        {
            int childrenCount = widget.GetChildrenCount();
            for (int i = 0; i < childrenCount; i++)
            {
                UIWidget children = widget[i];
                allChildrenAddTouchListener(children);
            }
            widget.AddTouchEventListener(this);
        }

        private void allChildrenRemoveTouchListener(UIWidget widget)
        {
            int childrenCount = widget.GetChildrenCount();
            for (int i = 0; i < childrenCount; i++)
            {
                UIWidget children = widget[i];
                allChildrenAddTouchListener(children);
                children.AddTouchEventListener(null);
            }
        }

        //public virtual void Add(UIWidget com)
        //{
        //    layer.AddWidget(com);
        //}

        //public virtual void Add(CCNode com)
        //{
        //    layer.AddChild(com);
        //}

        //public virtual void Remove(UIWidget com)
        //{
        //    layer.RemoveWidget(com);
        //}

        //public virtual void Remove(CCNode com)
        //{
        //    layer.RemoveChild(com);
        //}



        public new virtual void TouchListener(UIWidget widget, TouchEventType eventType)
        {
            //throw new NotImplementedException();
            switch (eventType)
            {
                case TouchEventType.TOUCH_EVENT_BEGAN:
                    TouchEventBegan(widget);
                    break;
                case TouchEventType.TOUCH_EVENT_CANCELED:
                    TouchEventCanceled(widget);
                    break;
                case TouchEventType.TOUCH_EVENT_ENDED:
                    TouchEventEnded(widget);
                    break;
                case TouchEventType.TOUCH_EVENT_MOVED:
                    TouchEventMoved(widget);
                    break;
            }
        }



        protected virtual void TouchEventBegan(UIWidget widget)
        {

        }

        protected virtual void TouchEventCanceled(UIWidget widget)
        {

        }

        protected virtual void TouchEventEnded(UIWidget widget)
        {

        }

        protected virtual void TouchEventMoved(UIWidget widget)
        {

        }



        #region UI管理器的内容
        public static List<Window> ALL_WINDOWS = new List<Window>(16);

        public static void CloseAllWindows(bool isKeepStaticWindow)
        {
            Window[] windows = ALL_WINDOWS.ToArray();
            if (isKeepStaticWindow)
            {
                foreach (Window window in windows)
                {
                    if (window is StaticWindow)
                    {
                        continue;
                    }
                    window.Show(false);

                    //                     if (window is NormalClose)
                    //                     {
                    //                         window.Show(false);
                    //                     }
                    //                     else
                    //                     {
                    //                         Remove(window);
                    //                     }
                }
            }
            else
            {
                foreach (Window window in windows)
                {
                    //window.Show(false);
                    if (window is NormalClose)
                    {
                        window.Show(false);
                    }
                    else
                    {
                        Remove(window);
                    }
                }
            }
            ALL_WINDOWS.Clear();
        }

        private readonly static UIWidget LAYER_WINDOW = new UIWidget();
        //private readonly static UIWidget LAYER_DIALOG = new UIWidget();
        //private readonly static UIWidget LAYER_INFO = new UIWidget();
        internal readonly static UILayer LAYER_UIMANAGER_LAYER = new UILayer();


        static Window()
        {
            LAYER_UIMANAGER_LAYER.AddWidget(LAYER_WINDOW);
            //LAYER_UIMANAGER_LAYER.AddWidget(LAYER_DIALOG);
            //LAYER_UIMANAGER_LAYER.AddWidget(LAYER_INFO);

            //AppMain.NotificationLayer.AddChild(LAYER_UIMANAGER_LAYER);
        }

        protected static void Add(Window ui)
        {
            ui.showFrom.AddChild(ui.WindowLayout);
            if (!Window.ALL_WINDOWS.Contains(ui))
            {
                Window.ALL_WINDOWS.Add(ui);
            }
        }


        protected static void Remove(Window ui)
        {
            ui.WindowLayout.RemoveFromParent(false);
            Window.ALL_WINDOWS.Remove(ui);
        }

        #endregion
    }
}
