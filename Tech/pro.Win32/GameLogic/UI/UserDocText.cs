using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MatrixEngine.CocoStudio.GUI;
using Thunder.GameLogic.GameSystem;
using Thunder.GameLogic.Common;

namespace Thunder.GameLogic.UI
{
    public class UserDocText : UIDocText
    {
        public static readonly UILabel label_yiheiText18;
        public static readonly UILabel label_yiheiText20;
        public static readonly UILabel label_sysText18;
        public static readonly UILabel label_sysText20;


        static UserDocText()
        {
            var Dialog_template = UIReader.GetWidget(ResID.UI_UI_Templates);

            label_yiheiText18 = (UILabel)Dialog_template.GetWidget("label_yiheiText18");
            label_yiheiText20 = (UILabel)Dialog_template.GetWidget("label_yiheiText20");
            label_sysText18 = (UILabel)Dialog_template.GetWidget("label_sysText18");
            label_sysText20 = (UILabel)Dialog_template.GetWidget("label_sysText20");
        }

        public UserDocText()
            : this(label_yiheiText20)
        {
        }


        public UserDocText(UILabel template)
            : this(template, false)
        {
        }


        public UserDocText(UILabel template, bool addToTemplate)
            : base(template)
        {
            if (addToTemplate)
            {
                template.Text = "  ";
                template.AddChild(this);
            }
        }

        private void UserDocClicked(UIWidget widget, TouchEventType type)
        {
            
        }
    }
}
