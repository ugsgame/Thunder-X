using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Thunder.GameLogic.Common;
using Thunder.GameLogic.Gaming;
using Thunder.GameLogic.Gaming.Actors.Players;
using Thunder.GameLogic.UI.Guide;

namespace Thunder.GameLogic.UI
{
    public class MainMenuScene :GameScene
    {
        MainMenuUIlayer mainMenu;
        public bool shouldShowRichGift;
        public static MainMenuScene Instance;

        public MainMenuScene()
        {
            mainMenu = new MainMenuUIlayer();
            this.AddChild(mainMenu);
            Instance = this;
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

            if (shouldShowRichGift)
            {
                Function.GoTo(UIFunction.购买礼包);
                shouldShowRichGift = false;
            }
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
//            GameData.Instanse.UnloadBossArmature();
        }

        public override void OnUpdate(float dTime)
        {
            base.OnUpdate(dTime);
        }

        public override void OnKeyBackClicked()
        {
            if(!GuideWindow.Instance.Show)
                Function.GoTo(UIFunction.游戏退出);
        }
    }
}
