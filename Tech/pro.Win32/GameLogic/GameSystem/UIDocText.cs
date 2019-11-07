using System;
using MatrixEngine.CocoStudio.GUI;
using MatrixEngine.Math;
using MatrixEngine.Cocos2d;
using System.Text;
using System.Collections.Generic;
using MatrixEngine.CocoStudio.Armature;
using Thunder.Game;

namespace Thunder.GameLogic.GameSystem
{
    public class UIDocText : UILayout
    {
        //private UIWidget docWidget;
        private int testLabelSize = -1;

        public int DefalutFontSize
        {
            get
            {
                return label_template.FontSize;
            }
            set
            {
                label_template.FontSize = value;
            }
        }

        public int DefalutColor
        {
            get
            {
                return label_template.Color;
            }
            set
            {
                label_template.Color = value;
            }
        }

        //private bool isScroll = false;
        //private bool isAutoWidth;

        protected UILabel label_template;

        public UIDocText()
        {
            label_template = new UILabel();
            label_template.FontSize = 20;
            label_template.Color = 0xffffff;
            AnchorPoint = Utils.AnchorPoint_Center;
        }

        public UIDocText(UILabel template)
        {
            this.label_template = (UILabel)template.Copy();
            this.label_template.IsVisible = true;
            this.label_template.Text = "国";
            this.label_template.Postion = 0;
            this.label_template.SizeType = SizeType.SIZE_ABSOLUTE;
            AnchorPoint = template.AnchorPoint;
        }

        public virtual void SetDocText(XmlBuilder xmlStr)
        {
            SetDocText(xmlStr.ToString());
        }

        public virtual void SetDocText(XmlBuilder xmlStr, float width)
        {
            SetDocText(xmlStr.ToString(), width);
        }

        public virtual void SetDocText(XmlBuilder xmlStr, float width, bool isAutoWidth)
        {
            SetDocText(xmlStr.ToString(), width, isAutoWidth);
        }

        /// <summary>
        /// 不限宽度的设置，文本大小为文本内容的大小
        /// </summary>
        /// <param name="xmlStr"></param>
        public virtual void SetDocText(string xmlStr)
        {
            //             if (isScroll)
            //             {
            //             }
            //             else
            {
                this.SetDocText(xmlStr, int.MaxValue, true);
            }
        }


        /// <summary>
        /// 限宽度的设置，文本宽度无论文字多少就是设置的宽度
        /// </summary>
        /// <param name="xmlStr"></param>
        public virtual void SetDocText(string xmlStr, float width)
        {
            this.SetDocText(xmlStr, width, false);
        }

        /// <summary>
        /// 解释文档
        /// </summary>
        /// <param name="xmlStr">文本内容</param>
        /// <param name="width">换行宽度</param>
        /// <param name="isAutoWidth">是否自动宽，如果该值是真，自动大小最大值是设置宽，最小宽度根据文字定义，并且无法使用对齐</param>
        public virtual void SetDocText(string xmlStr, float width, bool isAutoWidth)
        {
            try
            {
                //Console.WriteLine("start width=" + width);
                UIWidget docWidget = this;
                docWidget.RemoveAllChildren(true);
                docWidget.ContextSize = 0;
                docWidget.Size = 0;
                if (xmlStr == null || xmlStr.Length == 0)
                {
                    return;
                }
                //                 if (xmlStr[0] != '<')
                //                 {
                //                     xmlStr = new XmlBuilder().FontXML(xmlStr).ToString();
                //                 }
                Xml xml = new Xml(xmlStr);
                //string sb = "";
                int count = xml.XmlNodeCount;
                if (count <= 0)
                {
                    return;
                }
                //Utils.PrintCurrentUseTime();
                //const string testStr = "l";
                const string testStr = "测";
                float startPosX = 0;

                UILabel startBr = (UILabel)label_template.Copy();
                //startBr.SetFontSize(DefalutFontSize);
                startBr.SetText(testStr);
                float oneCharWidth = startBr.ContextSize.width;
                float oneCharHeight = startBr.ContextSize.height;
                startBr = null;
                const int maxPixel = int.MaxValue / 2;

                //  bool isContextSize = width < 0;
                float slipWidth = width;
                //                 if (isAutoWidth)
                //                 {
                //                     slipWidth = maxPixel;
                //                 }
                if (slipWidth < oneCharWidth)
                {
                    slipWidth = oneCharWidth;
                }
                else if (slipWidth > maxPixel)
                {
                    slipWidth = maxPixel;
                }
                //Console.WriteLine("isContextSize=" + isContextSize + " width=" + width);

                float nowWidth = slipWidth;

                int alignType = FontLabel.Value_Align_Left;

                int oneLineChars = (int)(slipWidth / oneCharWidth);
                List<DocWidget> list_docWidget = new List<DocWidget>();

                DocWidget widget = new DocWidget(list_docWidget, alignType);
                //widget.MaxWidth = oneCharWidth;
                widget.MaxHeight = oneCharHeight;
                for (int i = 0; i < count; i++)
                {
                    XmlNode xmlNode = xml[i];
                    string name = xmlNode.GetName();
                    //Console.WriteLine("name=" + name + " t=" + xmlNode.GetText() + " a=" + widget.alignType);
                    switch (name)
                    {
                        case FontLabel.LABEL_FONT:
                            {
                                string text = xmlNode.GetText();
                                if (text != null)
                                {
                                    int length = text.Length;
                                    if (length > 0)
                                    {
                                        string fsize = xmlNode.GetAttribute(FontLabel.ATT_FONT_fsize);
                                        int size = Parse(fsize, 10, label_template.FontSize);
                                        //计算文本是否超出了宽度
                                        UILabel newLabel = InitUILabel(xmlNode);
                                        newLabel.SetFontSize(size);
                                        if ((size > 0 && testLabelSize != size) || oneCharWidth < 0)
                                        {
                                            testLabelSize = size;
                                            UILabel testLabel = newLabel;
                                            //testLabel.SetFontSize(size);
                                            testLabel.SetText(testStr);
                                            oneCharWidth = testLabel.ContextSize.width;
                                            oneCharHeight = testLabel.ContextSize.height;

                                            oneLineChars = (int)(slipWidth / oneCharWidth);
                                        }
                                        string fcolor = xmlNode.GetAttribute(FontLabel.ATT_FONT_fcolor);
                                        int defalutColor = Parse(fcolor, 10, label_template.Color);

                                        bool isNewLine = false;
                                        for (int startIndex = 0; startIndex < length; )
                                        {
                                            nowWidth = slipWidth - startPosX;
                                            if (nowWidth < oneCharWidth || isNewLine)
                                            {
                                                startPosX = 0;
                                                widget = new DocWidget(list_docWidget, widget.alignType);
                                                isNewLine = false;
                                                continue;
                                            }
                                            //int j = length;
                                            int j = startIndex + oneLineChars;
                                            if (j > length)
                                            {
                                                j = length;
                                            }

                                            //int kcount=0;
                                            //bool isK = false;
                                            for (int k = j; k < length; )
                                            {
                                                // kcount++;
                                                //UILabel newLabel = InitUILabel(xmlNode);
                                                //newLabel.SetFontSize(size);
                                                string subString = text.Substring(startIndex, k - startIndex);
                                                newLabel.SetText(subString);
                                                Size newLabelSize = newLabel.ContextSize;
                                                //++testCount;
                                                if (newLabelSize.width >= nowWidth)
                                                {
                                                    //Console.WriteLine("预判断 = " + kcount);
                                                    j = k;
                                                    break;
                                                }
                                                else
                                                {
                                                    //if (!isK)
                                                    //{
                                                    float residueWidth = nowWidth - newLabelSize.width;
                                                    float averageOneCharWidth = newLabelSize.width / subString.Length;
                                                    int residueLength = (int)(residueWidth / averageOneCharWidth);
                                                    if (residueLength > 0)
                                                    {
                                                        int dok = k + residueLength;
                                                        //Console.WriteLine("k=" + k + " residueLength=" + residueLength + " nowWidth=" + nowWidth + "  newLabelSize.width=" + newLabelSize.width);
                                                        if (dok > length)
                                                        {
                                                            k = length;
                                                        }
                                                        else
                                                        {
                                                            k = dok;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        k++;
                                                    }
                                                    //    isK = true;
                                                    //}
                                                    //else
                                                    //{
                                                    //    k++;
                                                    //}
                                                    //break;
                                                }
                                            }
                                            for (; j >= startIndex; j--)
                                            {
                                                string subString = text.Substring(startIndex, j - startIndex);
                                                newLabel.SetText(subString);
                                                Size newLabelSize = newLabel.ContextSize;
                                                if (newLabelSize.width <= nowWidth || isNewLine)
                                                {
                                                    newLabel.AnchorPoint = Utils.AnchorPoint_LeftCenter;
                                                    newLabel.PostionX = startPosX;
                                                    newLabel.Color = new Color32(defalutColor);
                                                    widget.AddChild(newLabel);
                                                    startPosX += newLabelSize.width;
                                                    startIndex = j;
                                                    if (widget.MaxHeight < newLabelSize.height)
                                                    {
                                                        widget.MaxHeight = newLabelSize.height;
                                                    }
                                                    if (widget.MaxWidth < startPosX)
                                                    {
                                                        widget.MaxWidth = startPosX;
                                                    }
                                                    newLabel = InitUILabel(xmlNode);
                                                    newLabel.SetFontSize(size);
                                                    break;
                                                }
                                                else
                                                {
                                                    //newLabel.Release();
                                                    isNewLine = subString.Length <= 1;
                                                }
                                            }
                                            //if (j < startIndex)
                                            //{
                                            //    throw new ArithmeticException("设置宽度太小，无法排列文本！");
                                            //}
                                        }
                                    }
                                }
                                break;
                            }
                        case FontLabel.LABEL_BR:
                            {
                                startPosX = 0;
                                DocWidget oldWidget = widget;
                                widget = new DocWidget(list_docWidget, alignType);
                                if (oldWidget != null)
                                {
                                    if (oldWidget.MaxHeight <= 0)
                                    {
                                        oldWidget.MaxHeight = oneCharHeight;
                                    }
                                }
                                //widget.MaxHeight = oldWidget.MaxHeight;
                                break;
                            }
                        case FontLabel.LABEL_PNG:
                            {
                                string resid = xmlNode.GetAttribute(FontLabel.ATT_id);
                                if (resid != null)
                                {
                                    CCSprite sprite = new CCSprite(resid);
                                    string spriteW = xmlNode.GetAttribute(FontLabel.ATT_width);
                                    string spriteH = xmlNode.GetAttribute(FontLabel.ATT_height);
                                    Size size;
                                    if (spriteW != null && spriteH != null)
                                    {
                                        size = new Size(Parse(spriteW, 0), Parse(spriteH, 0));
                                    }
                                    else
                                    {
                                        size = sprite.ContextSize;
                                    }
                                    nowWidth = slipWidth - startPosX;
                                    if (size.width > nowWidth)
                                    {
                                        startPosX = 0;
                                        widget = new DocWidget(list_docWidget, alignType);
                                    }
                                    sprite.AnchorPoint = Utils.AnchorPoint_LeftCenter;
                                    sprite.PostionX = startPosX;
                                    widget.AddNode(sprite);
                                    startPosX += size.width;
                                    if (widget.MaxHeight < size.height)
                                    {
                                        widget.MaxHeight = size.height;
                                    }
                                    if (widget.MaxWidth < startPosX)
                                    {
                                        widget.MaxWidth = startPosX;
                                    }
                                    //Console.WriteLine(size);
                                }
                                break;
                            }
                        case FontLabel.LABEL_ANI:
                            {
                                string resid = xmlNode.GetAttribute(FontLabel.ATT_id);
                                if (resid != null)
                                {
                                    string[] res = resid.Split('-');
                                    if (res.Length < 3)
                                    {
                                        break;
                                    }

                                    UIWidget layout = new UIWidget();
                                    CCArmDataManager.AddArmatureFile(res[0]);
                                    CCArmature armature = new CCArmature(res[1]);
                                    MatrixEngine.CocoStudio.Armature.CCAnimation animation = armature.GetAnimation();
                                    animation.Play(res[2], true);
                                    layout.AddNode(armature);

                                    string spriteW = xmlNode.GetAttribute(FontLabel.ATT_width);
                                    string spriteH = xmlNode.GetAttribute(FontLabel.ATT_height);
                                    string scale = xmlNode.GetAttribute(FontLabel.ATT_scale);
                                    Size size;
                                    if (spriteW != null && spriteH != null)
                                    {
                                        size = new Size(Parse(spriteW, 70), Parse(spriteH, 70));
                                    }
                                    else
                                    {
                                        size = new Size(70, 70);
                                    }
                                    float scaleVal = 1;
                                    if (scale != null)
                                    {
                                        scaleVal = Parse(scale, 1f);
                                        size = scaleVal * size;
                                        armature.Scale = scaleVal;
                                    }
                                    armature.PostionX += size.width / 2;
                                    armature.PostionY -= size.height / 2;

                                    nowWidth = slipWidth - startPosX;
                                    if (size.width > nowWidth)
                                    {
                                        startPosX = 0;
                                        widget = new DocWidget(list_docWidget, alignType);
                                    }

                                    layout.AnchorPoint = Utils.AnchorPoint_LeftCenter;
                                    layout.PostionX = startPosX;
                                    widget.AddChild(layout);
                                    startPosX += size.width;
                                    if (widget.MaxHeight < size.height)
                                    {
                                        widget.MaxHeight = size.height;
                                    }
                                    if (widget.MaxWidth < startPosX)
                                    {
                                        widget.MaxWidth = startPosX;
                                    }
                                    //Console.WriteLine(size);
                                }
                                break;
                            }
                        case FontLabel.LABEL_Align:
                            {
                                if (isAutoWidth)
                                {
                                    break;
                                }
                                string align = xmlNode.GetAttribute(FontLabel.ATT_Type);
                                int type = Parse(align, (int)alignType);

                                alignType = type;
                                widget.alignType = type;
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }
                }
                float posy = 0;
                float autoMaxWidth = 0;
                float autoMaxHeight = 0;


                foreach (var docs in list_docWidget)
                {
                    autoMaxHeight += docs.MaxHeight;
                    if (autoMaxWidth < docs.MaxWidth)
                    {
                        autoMaxWidth = docs.MaxWidth;
                    }
                }
                //计算对齐宽度，最大宽度只是会比设置的宽小
                float alignWidth = isAutoWidth ? autoMaxWidth : slipWidth;
                float alignCenterX = alignWidth / 2;

                int childCount = list_docWidget.Count;
                float startY = 0;
                int lastIndexOf = childCount - 1;
                if (lastIndexOf >= 0)
                {
                    DocWidget docs = list_docWidget[lastIndexOf];
                    startY = docs.MaxHeight / 2;

                }
                for (int i = lastIndexOf; i >= 0; i--)
                {
                    DocWidget docs = list_docWidget[i];
                    docWidget.AddChild(docs);

                    Size docSize = new Size(docs.MaxWidth, docs.MaxHeight);
                    docs.ContextSize = docSize;
                    docs.Size = docSize;
                    switch (docs.alignType)
                    {
                        case FontLabel.Value_Align_Left:
                        default:
                            {
                                docs.AnchorPoint = Utils.AnchorPoint_LeftBottom;
                                docs.Postion = new Vector2(0, posy + startY);
                                break;
                            }
                        case FontLabel.Value_Align_Center:
                            {
                                docs.AnchorPoint = Utils.AnchorPoint_BottomCenter;
                                docs.Postion = new Vector2(alignCenterX, posy + startY);
                                break;
                            }
                        case FontLabel.Value_Align_Right:
                            {
                                docs.AnchorPoint = Utils.AnchorPoint_RightBottom;
                                docs.Postion = new Vector2(alignWidth, posy + startY);
                                break;
                            }
                    }
                    if (i > 0)
                    {
                        DocWidget nextdocs = list_docWidget[i - 1];
                        posy += (docs.MaxHeight + nextdocs.MaxHeight) / 2;
                    }
                }

                //好像setContextSize没效果，Size会影响到ContextSize
                Size autoMaxSize = new Size(autoMaxWidth, autoMaxHeight);
                //Size maxSize = new Size(slipWidth, autoMaxHeight);
                //                 if (isScroll)
                //                 {
                //                     //UIScrollView scroll = new UIScrollView();
                //                 }
                //                 else
                {
                    docWidget.ContextSize = autoMaxSize;
                    //docWidget.AnchorPoint
                    if (isAutoWidth)
                    {
                        docWidget.Size = autoMaxSize;
                    }
                    else
                    {
                        docWidget.Size = new Size(slipWidth, autoMaxHeight);
                    }

                    //                     this.AddChild(docWidget);
                    // 
                    //                     this.Size = docWidget.Size;
                    //                     this.ContextSize = docWidget.ContextSize;
                }



                //Console.WriteLine("ContextSize = " + docWidget.ContextSize);
                //Console.WriteLine("Size = " + docWidget.Size);
                //                 Console.WriteLine("maxSize = " + maxSize);
                //docWidget.AnchorPoint = new Vector2(1, 0f);
                //this.SetSize(maxSize);

                //this.ContextSize = new Size(width, maxHeight);
                //Console.WriteLine("maxSize=" + maxSize);
            }
            catch (Exception e)
            {
                Utils.PrintException(e);
            }
        }

        protected virtual UILabel InitUILabel(XmlNode xmlNode)
        {
            return (UILabel)label_template.Copy();
        }

        public override string ToString()
        {
            StringBuilder docLength = new StringBuilder();
            int count = this.GetChildrenCount();
            for (int i = 0; i < count; i++)
            {
                DocWidget docwidget = this[i] as DocWidget;
                if (docwidget != null)
                {
                    int textCount = docwidget.GetChildrenCount();
                    for (int j = 0; j < textCount; j++)
                    {
                        UILabel label = docwidget[j] as UILabel;
                        if (label != null)
                        {
                            //docLength += label.Text.Length;
                            docLength.Append(label.Text);
                        }
                    }
                }
            }
            return docLength.ToString();
        }

        private static float Parse(string val, float def)
        {
            if (val != null)
            {
                try
                {

                    return Convert.ToSingle(val);
                }
                catch (Exception)
                {
                }
            }
            return def;
        }

        private static int Parse(string val, int def)
        {
            if (val != null)
            {
                try
                {

                    return Convert.ToInt32(val);
                }
                catch (Exception)
                {
                }
            }
            return def;
        }

        private static int Parse(string val, int formBase, int def)
        {
            if (val != null)
            {
                try
                {

                    return Convert.ToInt32(val, formBase);
                }
                catch (Exception)
                {
                    try
                    {
                        return Convert.ToInt32(val);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            return def;
        }

        private static uint Parse(string val, int formBase, uint def)
        {
            if (val != null)
            {
                try
                {
                    try
                    {
                        return Convert.ToUInt32(val);
                    }
                    catch (Exception)
                    {
                    }
                }
                catch (Exception)
                {
                    return Convert.ToUInt32(val, formBase);
                }
            }
            return def;
        }
    }


    public class DocWidget : UILayout
    {
        internal float MaxHeight;
        internal float MaxWidth;
        internal int alignType;

        //internal DocWidget(UIWidget text, int alignType)
        //{
        //    this.alignType = alignType;
        //    text.AddChild(this);
        //}

        internal DocWidget(List<DocWidget> text, int alignType)
        {
            this.alignType = alignType;
            text.Add(this);
        }

    }
}
