using MatrixEngine.CocoStudio.GUI;
using MatrixEngine.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thunder.GameLogic.Common;

namespace Thunder.GameLogic.UI.Guide
{
    public class GuideText : UIWidget
    {
        private string text;

        UILabel label;
        UIImageView background;

        Size offsetSize = new Size(50, 50);

        public GuideText()
        {
            label = new UILabel();
            background = (UIImageView)(UIReader.GetWidget(ResID.UI_UI_Templates).GetWidget("msg_background").Copy());
            background.IsVisible = true;
            background.Postion = 0;

            label.SetFontSize(30);
            label.Color = new Color32(0, 218, 226);
            background.AddChild(label);

            this.AddChild(background);
        }

        public string Text
        {
            set
            {
                text = value;
                label.Text = text;
                background.Size = label.Size + offsetSize;
            }
            get { return text; }
        }
    }
}
