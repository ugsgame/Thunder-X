using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thunder.GameLogic.Common;
using Thunder.GameLogic.Gaming;
using Thunder.GameLogic.UI.Guide;

namespace Thunder.GameLogic.UI
{
    public class PlayerLevelUpScene:GameScene
    {
        public PlayerLevelUpUILayer playerLevelUp;

        public PlayerLevelUpScene()
        {
            playerLevelUp = new PlayerLevelUpUILayer();
            this.AddChild(playerLevelUp);
        }

        public override void OnEnter()
        {
            base.OnEnter();         
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void OnEnterTransitionFinish()
        {
            base.OnEnterTransitionFinish();
        }

        public override void OnExitTransitionStart()
        {
            base.OnExitTransitionStart();
        }

        public override IEnumerable<LoadingScene.Percent> LoadSync()
        {
            var loadAble = LoadingScene.GetPercentsWithSum(100);
            yield return loadAble.NextPercent();
//             foreach (var item in GameData.Instanse.LoadBossArmature(loadAble))
//             {
//                 yield return item;
//             }
        }

        public override void UnLoad()
        {
            //GameData.Instanse.UnloadBossArmature();
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
                {
                    Function.GoTo(UIFunction.关卡选择);
                }
                else if (GameScene.PerGameScene is PlayerSelectingScene)
                {
                    Function.GoTo(UIFunction.战机选择);
                }
            }

        }
    }
}
