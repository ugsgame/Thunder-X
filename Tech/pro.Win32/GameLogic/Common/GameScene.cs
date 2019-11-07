using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MatrixEngine.Cocos2d;
using Thunder.GameLogic.UI;
using Thunder.GameLogic.GameSystem;
using MatrixEngine.CocoStudio.GUI;

namespace Thunder.GameLogic.Common
{
    public class GameScene : CCScene
    {
        public static GameScene CurGameScene;
        public static GameScene PerGameScene;

        protected readonly UILayer layer_game = new UILayer();
        protected readonly UILayer layer_ui = new UILayer();

        private static string[] Empty = new string[0];
        /// <summary>
        /// 切换场景之后被改变的参数
        /// </summary>
        protected internal string[] onTransitionArgs = Empty;
        /// <summary>
        /// 和对象有有关的更新
        /// </summary>
        private IList<IRunnable> runs = new List<IRunnable>();

        public GameScene()
        {
            base.AddChild(layer_game);
            base.AddChild(layer_ui);

            this.SetState(LayerState.Keypad, true);
        }

        public override void OnEnter()
        {
            Console.WriteLine("GameScene:OnEnter");
            PerGameScene = (CurGameScene == null) ? this : CurGameScene;
            CurGameScene = this;

            Console.WriteLine("PerGameScene:" + GameScene.PerGameScene.GetType().Name);
            Console.WriteLine("CurGameScene:" + GameScene.CurGameScene.GetType().Name);
        }

        internal void ResetWindowManager()
        {
            Window.LAYER_UIMANAGER_LAYER.RemoveFromParent(false);
            layer_ui.AddChild(Window.LAYER_UIMANAGER_LAYER);

            bool canAddToThis = !this.Equals(layer_ui.Parent);
            if (canAddToThis)
            {
                uiReset();
            }
        }

        ///这里是包装一下场景添加删除子的操作，方便UI管理层
        ///
        /////////////////////////////////////////////////////////////////////

        private void uiReset()
        {
            layer_ui.RemoveFromParent(false);
            base.AddChild(layer_ui);
        }

        public new void AddChild(CCNode node)
        {
            layer_game.AddChild(node);

            uiReset();
        }

        public new void AddChild(CCNode node, int zOrder)
        {
            layer_game.AddChild(node, zOrder);

            uiReset();
        }

        public new void AddChild(CCNode node, int zOrder, int tag)
        {
            layer_game.AddChild(node, zOrder, tag);

            uiReset();
        }

        public new CCNode GetChild(int tag)
        {
            return layer_game.GetChild(tag);
        }

        public new int GetChildrenCount()
        {
            return layer_game.GetChildrenCount();
        }

        public new void RemoveChild(CCNode node)
        {
            layer_game.RemoveChild(node);
        }

        private new void RemoveChild(CCNode node, bool cleanup)
        {
            layer_game.RemoveChild(node, cleanup);
        }

        public new void RemoveChild(int tag)
        {
            layer_game.RemoveChild(tag);
        }

        private new void RemoveChild(int tag, bool cleanup)
        {
            layer_game.RemoveChild(tag, cleanup);
        }

        public new void RemoveAllChildren()
        {
            layer_game.RemoveAllChildren();
        }

        private new void RemoveAllChildren(bool cleanup)
        {
            layer_game.RemoveAllChildren(cleanup);
        }

        /// <summary>
        /// 下面是方便事件处理的
        /// </summary>
        /// <param name="widget"></param>
        /// <param name="eventType"></param>

        public virtual void TouchListener(UIWidget widget, TouchEventType eventType)
        {
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

        /// <summary>
        /// 异步加载
        /// </summary>
        public virtual bool LoadAsync()
        {
            return true;
        }
        /// <summary>
        /// 同步加载
        /// </summary>
        public virtual IEnumerable<LoadingScene.Percent> LoadSync()
        {
            return null;
        }

        public virtual void UnLoad()
        {
        }

        public virtual void LoadOver()
        {
        }

        internal LoadingScene loadingScene;

        public virtual void NotifyForLoad()
        {
            if (loadingScene != null)
            {
                loadingScene.NotifyForLoad();
            }
        }
    }
}
