using MatrixEngine.Cocos2d;
using MatrixEngine.CocoStudio.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thunder.GameLogic.Common;
using Thunder.GameLogic.GameSystem;

namespace Thunder.GameLogic.UI.Dialogs
{
    public class GameExit : GameWindow
    {
        private bool isInit;

        private UIWidget UI_GameExit;

        private UIButton Button_cancel;
        private UIButton Button_sure;

        public GameExit()
        {

        }

        public override void init()
        {
            if (!this.isInit)
            {
                base.init();

                UI_GameExit = UIReader.GetWidget(ResID.UI_UI_GameExit);
                UIWidget main = UI_GameExit.GetWidget("Panel_gameExit");
                Button_cancel = (UIButton)main.GetWidget("Button_cancel");
                Button_sure = (UIButton)main.GetWidget("Button_sure");

                Button_cancel.TouchBeganEvent += button =>
                    {
                        this.Show(false);
                        GameAudio.PlayEffect(GameAudio.Effect.button);
                    };
                Button_sure.TouchBeganEvent += button =>
                    {
                        this.Show(false);
                        GameAudio.PlayEffect(GameAudio.Effect.button);
                        //保存数据                      
                        GameData.Instance.SaveDataFile();
                        //
                        CCDirector.End();
                    };


                this.AddChildCenter(UI_GameExit);
                this.isInit = true;
            }
        }
    }
}
