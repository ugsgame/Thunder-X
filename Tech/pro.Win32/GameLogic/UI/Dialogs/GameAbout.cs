using MatrixEngine.CocoStudio.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thunder.GameLogic.Common;
using Thunder.GameLogic.GameSystem;

namespace Thunder.GameLogic.UI.Dialogs
{
    public class GameAbout : GameWindow
    {
        private bool isInit;

        private UIWidget gameAbout;
        private UIButton Button_close;

        public GameAbout()
        {

        }

        public override void init()
        {
            if (!this.isInit)
            {
                base.init();

                UIWidget main = UIReader.GetWidget(ResID.UI_UI_GameAbout);
                gameAbout = main.GetWidget("Panel_1");
                Button_close = (UIButton)main.GetWidget("Button_close");

                Button_close.TouchBeganEvent += button =>
                    {
                        this.Show(false);
                    };
                Button_close.TouchBeganEvent += Function.PlayBackButtonEffect;
                this.AddChildCenter(main);
                this.isInit = true;
            }
        }
    }
}
