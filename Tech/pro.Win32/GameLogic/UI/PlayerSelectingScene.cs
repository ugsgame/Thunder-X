using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Thunder.GameLogic.Common;
using Thunder.GameLogic.UI.Guide;

namespace Thunder.GameLogic.UI
{
    public class PlayerSelectingScene:GameScene
    {
        public PlayerSelectingUILayer playerSelecting;

        public PlayerSelectingScene()
        {
            playerSelecting = new PlayerSelectingUILayer();
            this.AddChild(playerSelecting);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
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

        public override void OnKeyBackClicked()
        {
            if (!GuideWindow.Instance.Show)
                Function.GoTo(UIFunction.主菜单);
        }
    }
}
