using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MatrixEngine;
using MatrixEngine.Cocos2d;
using MatrixEngine.CocoStudio;
using MatrixEngine.CocoStudio.GUI;
using Thunder.GameLogic.Common;
using System.Threading;
using Thunder.Game;
using Thunder.Common;

namespace Thunder.GameLogic.UI
{
    public class LoadingScene : CCScene
    {
        enum State
        {
            Ready,
            Load,
            Loading,
            Waiting,
            End,
            Null,
        }

        private GameScene oldScene;
        private GameScene curScene;

        private volatile bool asyncFlag;
        private State state;


        private string[] nextArgs = new string[0];

        private UILayout uilayout = new UILayout();
        private LoadingUILayer layer = new LoadingUILayer();
        private UILabel label = new UILabel();

        public LoadingScene()
        {
            state = State.Null;
            this.AddChild(layer);

            label.Postion = Config.ScreenCenter;
            label.SetFontSize(50);
            //uilayout.AddChild(label);
            this.AddChild(uilayout);
        }
        ~LoadingScene()
        {

        }

        public void SetNextSence(GameScene scene, params string[] args)
        {
            //layer.RunTurnTableAction();

            nextArgs = args;

            oldScene = curScene;
            curScene = scene;

            state = State.Ready;
            //layer.SetLoadingValue(0);
        }

        protected virtual void StartLoad()
        {
            if (state == State.Ready)
            {
                state = State.Load;
                asyncFlag = false;
                Thread loadThread = new Thread(this.Run);
                loadThread.Start();
            }
            else
            {
                Console.WriteLine("Loading Scene no ready!!!");
            }
        }

        protected virtual void Run()
        {
            try
            {
                if (curScene.LoadAsync())
                {
                    Console.WriteLine();
                }
            }
            catch (Exception e)
            {
                Utils.PrintException(e);
            }
            asyncFlag = true;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            //this.ScheduleStep(0.01f);
        }

        public override void OnExit()
        {
            base.OnExit();
            //this.UnscheduleStep();
        }

        public override void OnEnterTransitionFinish()
        {
            base.OnEnterTransitionFinish();
            StartLoad();
        }

        public override void OnExitTransitionStart()
        {
            base.OnExitTransitionStart();
        }

        private volatile IEnumerator<LoadingScene.Percent> enumerator;
        private volatile LoadingScene.Percent currentPercent;

        private void WaitForLoad()
        {
            if (state == State.Loading)
            {
                //oldState = state;
                state = State.Waiting;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public virtual void NotifyForLoad()
        {
            if (state == State.Waiting)
            {
                state = State.Loading;
            }
        }

        public override void OnUpdate(float dTime)
        {
            base.OnUpdate(dTime);
            //Console.WriteLine("State=" + state + " asyncFlag=" + asyncFlag);
            if (asyncFlag)
            {
                if (state == State.Load)
                {
                    if (oldScene != null)
                    {
                        Console.WriteLine("OldScene:" + oldScene.GetType());
                        oldScene.UnLoad();
                        //保存一下
                        GameData.Instance.SaveDataFile();
                        //有可能引发内存异常
                        CCTextureCache.RemoveUnusedTextures();
                        //
                        System.GC.Collect();
                    }
                    if (curScene != null)
                    {
                        currentPercent = null;
                        curScene.loadingScene = this;
                        enumerator = curScene.LoadSync().GetEnumerator();
                        state = State.Loading;
                        label.Text = "加载中...";
                        layer.SetLoadingValue(0);
                    }
                }
                else if (state == State.Loading)
                {
                    if (enumerator != null)
                    {
                        try
                        {
                            if (enumerator.MoveNext())
                            {
                                var p = enumerator.Current;
                                if (p != null)
                                {
                                    if (p.percent == -1)
                                    {
                                        WaitForLoad();
                                        return;
                                    }
                                    else
                                    {
                                        string toString = Convert.ToString(p);
                                        label.Text = "加载中..." + toString + "％";
                                        layer.SetLoadingValue(p.percent);
                                        currentPercent = p;

                                        //Thread.Sleep(30);
                                    }
                                }
                                else
                                {
                                    Console.Error.WriteLine("数据加载比例没有填写完整，使用数比实际数多！");
                                }
                            }
                            else
                            {
                                if (currentPercent != null && currentPercent.percent < 100)
                                {
                                    Console.Error.WriteLine("数据加载比例没有填写完整，使用数比实际数少！");
                                    label.Text = "加载中..100％";

                                    layer.SetLoadingValue(100);
                                }
                                state = State.End;
                            }

                        }
                        catch (Exception e)
                        {
                            Utils.PrintException(e);
                        }
                    }
                    else
                    {
                        state = State.End;

                    }
                }
                else if (state == State.End)
                {
                    curScene.LoadOver();
                    curScene.loadingScene = null;

                    Function.ReplaceGameScene(curScene, Transition.CCTransitionFade, 1.0f, nextArgs);
                    label.Text = "";
                    asyncFlag = false;

                    //CCTextureCache.DumpCachedTextureInfo();
                }
            }
        }

        ///////////////////////////////////////////////////////////////
        public static PercentCounter GetPercentsWithSum(params int[] percents)
        {
            percents = (int[])percents.Clone();
            int sum = 0;
            for (int i = 0; i < percents.Length; i++)
            {
                var percent = percents[i];
                if (percent < 0)
                {
                    throw new ArgumentOutOfRangeException("数值必须大于等于0");
                }
                sum += percent;
                percents[i] = sum;
            }
            if (sum != 100)
            {
                throw new ArgumentOutOfRangeException("总和值必须等于100");
            }
            return new PercentCounter(percents);
        }

        public static PercentCounter GetPercentsWithSeq(params int[] percents)
        {
            percents = (int[])percents.Clone();
            int sum = -1;
            foreach (var percent in percents)
            {
                if (percent < sum)
                {
                    throw new ArgumentOutOfRangeException("必须比前一个数大且大于零");
                }
                sum = percent;
            }
            if (percents[percents.Length - 1] != 100)
            {
                throw new ArgumentOutOfRangeException("最后一个数须等于100");
            }
            return new PercentCounter(percents);
        }



        public class Percent
        {
            internal int percent;

            private Percent() { }

            internal Percent(int percent)
            {
                this.percent = percent;
            }

            public override string ToString()
            {
                return percent.ToString();
            }
        }

        public class PercentCounter
        {
            private int[] percents;
            private int count;

            private static Percent waitForPercent = new Percent(-1);

            internal PercentCounter(int[] percents)
            {
                this.percents = percents;
            }

            public Percent WaitFor()
            {
                return waitForPercent;
            }

            public Percent NextPercent()
            {
                return NextPercentProgress(1, 1);
            }

            public Percent NextPercent(int index, int length)
            {
                return NextPercentProgress(index + 1, length);
            }

            public Percent NextPercentProgress(int progress, int maxProgress)
            {

                try
                {
                    if (progress > maxProgress)
                    {
                        progress = maxProgress;
                    }
                    if (count < percents.Length)
                    {
                        if (count > 0)
                        {
                            int b = count - 1;
                            return new Percent(percents[b] + (percents[count] - percents[b]) * progress / maxProgress);
                        }
                        return new Percent(percents[count] * progress / maxProgress);
                    }
                    return null;
                }
                catch (Exception e)
                {
                    Utils.PrintException(e);
                    return null;
                }
                finally
                {
                    if (progress >= maxProgress)
                    {
                        count++;
                    }
                }
            }
        }
    }
}
