
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MatrixEngine.CocoStudio.GUI;
using Thunder.GameLogic.GameSystem;
using Thunder.GameLogic.Common;

namespace Thunder.GameLogic.UI
{
    public class GameRankingList:GameWindow
    {
        private bool isInit;

        private UIWidget rankingList;
        private UIButton Button_back;

        public GameRankingList()
        {

        }

        public override void init()
        {
            if (!this.isInit)
            {
                base.init();
                rankingList = UIReader.GetWidget(ResID.UI_UI_RankingList);

                Button_back = (UIButton)rankingList.GetWidget("Button_back");

                Button_back.TouchBeganEvent += button =>
                    {
                        this.Show(false);
                        GameAudio.PlayEffect(GameAudio.Effect.back);
                    };

                this.AddChildCenter(rankingList);
                this.isInit = true;
            }
        }
    }
}
