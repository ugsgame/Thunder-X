using MatrixEngine.CocoStudio.GUI;
using MatrixEngine.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thunder.Common;
using Thunder.Game;
using Thunder.GameLogic.Common;
using Thunder.GameLogic.GameSystem;

namespace Thunder.GameLogic.UI.Dialogs
{
    public class DialogWindow : GameWindow
    {

        //public int jjjjjjjjj;
        public enum ShowType
        {
            OK,
            YesNo,
            YesNoCancle,
        }

        public enum ClickType
        {
            Yes, No, Cancle, OK = Yes
        }

        public delegate void OnClickCallback(DialogWindow.ClickType clickType);

        private bool isInit;
        private UIImageView img_title;
        private UIDocText title;
        private UIDocText text;
        private UIButton button_yes;
        private UIButton button_no;
        private UIButton button_cancel;
        private OnClickCallback callback;
        private UILayout layout;
        private UIWidget dialog;
        private UIWidget dialog_background;

        public DialogWindow()
            : this(Priority.PRIORITY_DIALOG)
        {
        }

        public DialogWindow(UIWidget widget)
            : this(Priority.PRIORITY_DIALOG, widget)
        {
        }


        public DialogWindow(Priority priority)
            : this(priority, null)
        {
        }

        public DialogWindow(Priority priority, UIWidget widget)
            : base(priority, widget)
        {
        }

        protected override void InitDefaultConfig()
        {
        }

        //public override CCAction GetShowAction(bool isShowing)
        //{
        //    return isShowing ? base.GetShowAction(isShowing) : null;
        //}


        public override void init()
        {
            base.init();

            if (!isInit)
            {
                var template = UIReader.GetWidget(ResID.UI_UI_Templates);
                var template_dialog_background = template.GetWidget("dialog_background");
                template_dialog_background.Postion = 0;
                template_dialog_background.IsVisible = true;

                var panel_buttons = template.GetWidget("panel_buttons");
                var template_btn_yes = panel_buttons.GetWidget("btn_yes");
                var template_btn_no = panel_buttons.GetWidget("btn_no");
                var template_btn_cancle = panel_buttons.GetWidget("btn_cancle");
                var template_img_title = panel_buttons.GetWidget("img_title");
                var template_label_text = panel_buttons.GetWidget("label_text");
                template_img_title.Postion = 0;

                //this.TouchEnabled = true;
                //layout = new UILayout();
                //layout.ContextSize = layout.Size = Config.ScreenSize;
                //layout.SetBackGroundColorType(LayoutBackGroundColorType.LAYOUT_COLOR_GRADIENT);
                //layout.SetBackGroundColorSE(new Color32(0xffff00),new Color32(0xffff00));
                //this.AddChild(layout);

                dialog_background = template_dialog_background.Copy();
                dialog = new UILayout();
                dialog.AddChild(dialog_background);

                this.AddChild(dialog);

                img_title = (UIImageView)template_img_title.Copy();
                img_title.PostionY = 30;
                dialog.AddChild(img_title);

                var label_title = (UILabel)img_title.GetWidget("label_title");
                //初始化标题
                title = new UserDocText(label_title, true);
                title.DefalutColor = 0xffc879;

                var label_text = (UILabel)template_label_text.Copy();
                //初始化文本框
                text = new UserDocText(label_text, true);
                dialog.AddChild(text);

                //窗口“是”按钮
                button_yes = (UIButton)template_btn_yes.Copy();
                button_yes.PostionY = -40;
                button_yes.TouchEndedEvent += TouchEventEnded;
                button_yes.TitleText = "是";
                dialog.AddChild(button_yes);

                //窗口“否”按钮
                button_no = (UIButton)template_btn_no.Copy();
                button_no.PostionY = -40;
                button_no.TouchEndedEvent += TouchEventEnded;
                button_no.TitleText = "否";
                dialog.AddChild(button_no);

                //取消按钮
                button_cancel = (UIButton)template_btn_cancle.Copy();
                button_cancel.PostionY = -40;
                button_cancel.TouchEndedEvent += TouchEventEnded;
                button_no.TitleText = "取消";
                dialog.AddChild(button_cancel);


                this.img_title.AnchorPoint = Utils.AnchorPoint_Center;
                this.text.AnchorPoint = Utils.AnchorPoint_TopCenter;

                this.button_yes.AnchorPoint = Utils.AnchorPoint_Center;
                button_no.AnchorPoint = button_yes.AnchorPoint;
                button_cancel.AnchorPoint = button_yes.AnchorPoint;

                dialog.Postion = Config.ScreenCenter;

                isInit = true;
            }
            dialog.Postion = Config.ScreenCenter;
        }

        public override void Show(bool b)
        {
            base.Show(b);
        }

        public void Show(string text)
        {
            this.Show("温馨提示", text);
        }

        public void Show(string title, string text)
        {
            this.Show(title, text, ShowType.OK, null);
        }

        public void Show(string text, ShowType type, OnClickCallback callback)
        {
            this.Show("温馨提示", text, type, callback);
        }

        public void Show(string title, string text, ShowType type, OnClickCallback callback)
        {
            Show(title, text, "是", "否", type == ShowType.OK ? "确定" : "取消", type, callback);
        }

        //         public void Show(string text, string yes, string no, string cancel, OnClickCallback callback)
        //         {
        //             Show("温馨提示", text, yes, no, cancel, callback);
        //         }

        public void Show(string title, string text, string yes, string no, string cancel, OnClickCallback callback)
        {
            Show(title, text, yes, no, cancel, ShowType.YesNoCancle, callback);
        }

        //         public void Show(string text, string yes, string no, OnClickCallback callback)
        //         {
        //             Show("温馨提示", text, yes, no, null, callback);
        //         }

        public void Show(string title, string text, string yes, string no, OnClickCallback callback)
        {
            Show(title, text, yes, no, null, ShowType.YesNo, callback);
        }

        public void Show(string title, string text, string cancel, OnClickCallback callback)
        {
            Show(title, text, null, null, cancel, ShowType.OK, callback);
        }

        private void Show(string title, string text, string yes, string no, string cancel, ShowType type, OnClickCallback callback)
        {
            Show(true);

            title = title == null ? "" : title;
            text = text == null ? "" : text;
            //text = new XmlBuilder().AlignCenter().FontXML(text).ToString();

            this.title.SetDocText(title);
            this.text.SetDocText(text, 460, true);
            this.callback = callback;

            float defalutButtonHeight = button_yes.Size.height;

            float maxHeight = this.text.Size.height + defalutButtonHeight + this.img_title.Size.height + 100;

            float half = maxHeight / 2;
            this.img_title.PostionY = half - this.img_title.Size.height / 2 - 10;
            this.text.PostionY = this.img_title.PostionY - this.img_title.Size.height - 10;

            button_yes.PostionY = this.text.PostionY - this.text.Size.height - 40 - defalutButtonHeight / 2;
            button_no.PostionY = button_yes.PostionY;
            button_cancel.PostionY = button_yes.PostionY;


            switch (type)
            {
                case ShowType.OK:
                    {
                        dialog_background.Size = new Size(400, maxHeight);
                        button_yes.IsVisible = true;
                        button_no.IsVisible = false;
                        button_cancel.IsVisible = false;
                        button_yes.TitleText = cancel;

                        button_yes.PostionX = 0;
                        break;
                    }
                case ShowType.YesNo:
                    {
                        dialog_background.Size = new Size(400, maxHeight);
                        button_yes.IsVisible = true;
                        button_no.IsVisible = true;
                        button_cancel.IsVisible = false;
                        button_yes.TitleText = yes;
                        button_no.TitleText = no;

                        button_yes.PostionX = 110;
                        button_no.PostionX = -110;
                        break;
                    }
                case ShowType.YesNoCancle:
                    {
                        dialog_background.Size = new Size(480, maxHeight);
                        button_yes.IsVisible = true;
                        button_no.IsVisible = true;
                        button_cancel.IsVisible = true;
                        button_yes.TitleText = yes;
                        button_no.TitleText = no;
                        button_cancel.TitleText = cancel;

                        button_yes.PostionX = 170;
                        button_no.PostionX = 0;
                        button_cancel.PostionX = -170;
                        break;
                    }
            }

            button_yes.TouchEnabled = button_yes.IsVisible;
            button_no.TouchEnabled = button_no.IsVisible;
            button_cancel.TouchEnabled = button_cancel.IsVisible;
        }

        protected override void TouchEventEnded(UIWidget widget)
        {
            if (widget.Equals(button_cancel))
            {
                //Console.WriteLine("UIWidget = " + widget_login);
                Show(false);
                if (callback != null)
                {
                    callback(ClickType.Cancle);
                }
            }
            else if (widget.Equals(button_yes))
            {
                Show(false);
                if (callback != null)
                {
                    callback(ClickType.Yes);
                }
            }
            else if (widget.Equals(button_no))
            {
                Show(false);
                if (callback != null)
                {
                    callback(ClickType.No);
                }
            }
        }
    }
}
