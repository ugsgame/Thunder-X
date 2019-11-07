
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Thunder.Game;
using MatrixEngine.Cocos2d;
using MatrixEngine.CocoStudio.GUI;


namespace Thunder.GameLogic.GameSystem
{
    public class ActionWindow : Window
    {
        private bool isClosing = false;

        public event WindowShowHandle initAfterActionEvent;
        public event WindowShowHandle releaseBeforeActionEvent;

        public ActionWindow()
            : this(Priority.PRIORITY_WINDOW)
        {
        }

        public ActionWindow(Priority priority)
            : this(priority, null)
        {
        }

        public ActionWindow(UIWidget showFrom)
            : this(Priority.PRIORITY_WINDOW, showFrom)
        {
        }

        public ActionWindow(Priority priority, UIWidget showFrom)
            : base(priority, showFrom)
        {
            initAfterActionEvent += initAfterAction;
            releaseBeforeActionEvent += releaseBeforeAction;
        }


        protected virtual void initAfterAction()
        {
        }

        protected virtual void releaseBeforeAction()
        {
        }

        private void WindowShow(bool b)
        {
            if (b)
            {
                Window.Add(this);
                isShow = true;
            }
            else
            {
                Window.Remove(this);
                isShow = false;
            }
        }

        protected virtual bool tryInit()
        {
            try
            {
                this.callInitEvent();
            }
            catch (Exception e)
            {
                Utils.PrintException(e);
                return false;
            }
            return true;
        }

        protected virtual void tryInitAfterAction()
        {
            try
            {
                this.callInitAfterActionEvent();
            }
            catch (Exception e)
            {
                Utils.PrintException(e);
            }
        }

        protected bool tryReleaseBeforeAction()
        {
            try
            {
                isClosing = true;
                this.callReleaseBeforeActionEvent();
            }
            catch (Exception e)
            {
                Utils.PrintException(e);
                return false;
            }
            return true;
        }

        protected void tryRelease()
        {
            try
            {
                isClosing = false;
                callReleaseEventEvent();
            }
            catch (Exception e)
            {
                Utils.PrintException(e);
            }
        }

        protected void callInitAfterActionEvent()
        {
            if (initAfterActionEvent != null)
            {
                this.initAfterActionEvent();
            }
        }

        protected void callReleaseBeforeActionEvent()
        {
            if (releaseBeforeActionEvent != null)
            {
                this.releaseBeforeActionEvent();
            }
        }

        public override void Show(bool b)
        {
            if (isShow != b)
            {
                if (b)
                {
                    if (tryInit())
                    {
                        WindowShow(true);
                        var action = GetShowAction(b);
                        //Console.WriteLine("action=" + action + " b=" + b);
                        if (action != null)
                        {
                            //this.StopAllAction();
                            var call = new CCActionCallFunc(args =>
                            {
                                tryInitAfterAction();
                            });
                            var seq = new CCActionSequence(action, call);
                            this.RunAction(seq);
                            //this.RunAction(action);
                        }
                        else
                        {
                            tryInitAfterAction();
                        }
                    }
                }
                else
                {
                    if (isClosing) return;
                    tryReleaseBeforeAction();
                    var action = GetShowAction(b);
                    //Console.WriteLine("action=" + action + " b=" + b);
                    if (action != null)
                    {
                        //this.StopAllAction();
                        var call = new CCActionCallFunc(args =>
                        {
                            WindowShow(false);
                            tryRelease();
                        });
                        var seq = new CCActionSequence(action, call);
                        this.RunAction(seq);
                        //this.RunAction(action);
                    }
                    else
                    {
                        WindowShow(false);
                        tryRelease();
                    }
                }
            }
            else if (isShow && b)
            {
                Window.Add(this);
            }
            //bool isShow = IsShow;
            //if (isShow != b)
            //{
            //    if (b)
            //    {
            //        baseShow(b);
            //        var action = GetShowAction(b);
            //        if (action != null)
            //        {
            //            this.RunAction(action);
            //        }
            //    }
            //    else
            //    {
            //        var action = GetShowAction(b);
            //        if (action != null)
            //        {
            //            var call = new CCActionCallFunc(args => baseShow(b));
            //            var seq = new CCActionSequence(action, call);
            //            this.RunAction(seq);
            //        }
            //        else
            //        {
            //            baseShow(b);
            //        }
            //    }
            //}
            //else if (isShow && b)
            //{
            //    baseShow(b);
            //    var action = GetShowAction(b);
            //    if (action != null)
            //    {
            //        this.RunAction(action);
            //    }
            //}
        }

        private void baseShow(bool b)
        {
            base.Show(b);
        }

        public virtual CCAction GetShowAction(bool isShowing)
        {
            return null;
        }
    }
}
