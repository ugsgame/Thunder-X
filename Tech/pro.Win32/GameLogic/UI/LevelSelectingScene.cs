using MatrixEngine.CocoStudio.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thunder.GameLogic.Common;
using Thunder.GameLogic.Gaming;
using Thunder.GameLogic.UI.Guide;

namespace Thunder.GameLogic.UI
{
    public class LevelSelectingScene:GameScene
    {
        public LevelSelectingUILayer levelSelecting;

        public LevelSelectingScene()
        {
            levelSelecting = new LevelSelectingUILayer();
            this.AddChild(levelSelecting);
        }

        public override void OnEnter()
        {
            base.OnEnter();

            ///////////////////////////////////////////////////////////////////////
            if (GameAudio.CurMusic != GameAudio.Music.menu_bg)
            {
                GameAudio.PlayMusic(GameAudio.Music.menu_bg, true);
            }
            //
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void OnEnterTransitionFinish()
        {
  
        }

        public override void OnExitTransitionStart()
        {

        }

        public override IEnumerable<LoadingScene.Percent> LoadSync()
        {
            var loadAble = LoadingScene.GetPercentsWithSum(100);
            yield return loadAble.NextPercent();
        }

        public override void OnUpdate(float dTime)
        {
            base.OnUpdate(dTime);
        }

        public override void OnKeyBackClicked()
        {
            if (!GuideWindow.Instance.Show)
            {
                if (GameScene.PerGameScene is PlayingScene)
                    Function.GoTo(UIFunction.关卡选择);
                else
                    Function.GoTo(UIFunction.战机选择);
            }
        }
    }
}
