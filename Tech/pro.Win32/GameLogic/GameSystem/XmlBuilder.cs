using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Thunder.Game;

namespace Thunder.GameLogic.GameSystem
{
    public sealed class XmlBuilder
    {
        private readonly StringBuilder sb;

        public XmlBuilder()
            : this(64)
        {
        }

        public XmlBuilder(int capacity)
        {
            sb = new StringBuilder(capacity);
        }

        /// <summary>
        /// 返回构造生成的文本的长度
        /// </summary>
        public int Length
        {
            get
            {
                return sb.Length;
            }
            set
            {
                if (value <= 0)
                {
                    sb.Length = 0;
                }
                else
                {
                    int lastIndex = value;
                    int lastIndexOf = Utils.LastIndexOf(sb, ">");
                    if ((lastIndex <= lastIndexOf) || (lastIndexOf == lastIndex - 1))
                    {
                        int lastIndexOf_start = Utils.LastIndexOf(sb, "<");
                        if (lastIndexOf_start >= 0)
                        {
                            sb.Length = lastIndexOf_start;
                        }
                        else
                        {
                            sb.Length = 0;
                        }
                    }
                    else
                    {
                        sb.Length = lastIndex;
                    }
                }
            }
        }

        /// <summary>
        /// 重置Xml构造器
        /// </summary>
        public void Reset()
        {
            sb.Length = 0;
        }

        /// <summary>
        /// 复制一个Xml内容到当前对象
        /// </summary>
        /// <param name="xmlBuilder">要复制的对象</param>
        /// <returns>此对象的一个引用。</returns>
        public XmlBuilder Copy(XmlBuilder xmlBuilder)
        {
            sb.Append(xmlBuilder.ToString());
            return this;
        }

        /// <summary>
        /// 指定颜色的xml文本
        /// </summary>
        /// <param name="str">文本</param>
        /// <returns>此对象的一个引用。</returns>
        public XmlBuilder FontXML(string str)
        {
            sb.Append('<');
            sb.Append(FontLabel.LABEL_FONT);
            sb.Append('>');
            sb.Append(str);
            return this;
        }

        /// <summary>
        /// 指定颜色的xml文本
        /// </summary>
        /// <param name="size">大小</param>
        /// <param name="str">文本</param>
        /// <returns>此对象的一个引用。</returns>
        public XmlBuilder FontXML(int size, string str)
        {
            sb.Append('<');
            sb.Append(FontLabel.LABEL_FONT);
            sb.Append(' ');
            sb.Append(FontLabel.ATT_FONT_fsize);
            sb.Append('=');
            sb.Append(size);
            sb.Append('>');
            sb.Append(str);
            return this;
        }

        /// <summary>
        /// 指定颜色的xml文本
        /// </summary>
        /// <param name="str">文本</param>
        /// <param name="color">文本颜色</param>
        /// <returns>此对象的一个引用。</returns>
        public XmlBuilder FontXML(string str, int color)
        {
            sb.Append('<');
            sb.Append(FontLabel.LABEL_FONT);
            sb.Append(' ');
            sb.Append(FontLabel.ATT_FONT_fcolor);
            sb.Append('=');
            sb.Append(color);
            sb.Append('>');
            sb.Append(str);
            return this;
        }

        /// <summary>
        /// 指定颜色的xml文本
        /// </summary>
        /// <param name="size">大小</param>
        /// <param name="str">文本</param>
        /// <param name="color">文本颜色</param>
        /// <returns>此对象的一个引用。</returns>
        public XmlBuilder FontXML(int size, string str, int color)
        {
            sb.Append('<');
            sb.Append(FontLabel.LABEL_FONT);
            sb.Append(' ');
            sb.Append(FontLabel.ATT_FONT_fsize);
            sb.Append('=');
            sb.Append(size);
            sb.Append(' ');
            sb.Append(FontLabel.ATT_FONT_fcolor);
            sb.Append('=');
            sb.Append(color);
            sb.Append('>');
            sb.Append(str);
            return this;
        }

        /// <summary>
        /// 指定颜色的xml文本
        /// </summary>
        /// <param name="str">文本</param>
        /// <param name="color">文本颜色</param>
        /// <param name="lineColor">下划线的颜色</param>
        /// <returns>此对象的一个引用。</returns>
        public XmlBuilder FontXML(string str, int color, int lineColor)
        {
            sb.Append('<');
            sb.Append(FontLabel.LABEL_FONT);
            sb.Append(' ');
            sb.Append(FontLabel.ATT_FONT_fcolor);
            sb.Append('=');
            sb.Append(color);
            sb.Append(' ');
            sb.Append(FontLabel.ATT_FONT_line);
            sb.Append('=');
            sb.Append(lineColor);
            sb.Append('>');
            sb.Append(str);
            return this;
        }

        /// <summary>
        /// 指定颜色的xml文本
        /// </summary>
        /// <param name="size">大小</param>
        /// <param name="str">文本</param>
        /// <param name="color">文本颜色</param>
        /// <param name="lineColor">下划线的颜色</param>
        /// <returns>此对象的一个引用。</returns>
        public XmlBuilder FontXML(int size, string str, int color, int lineColor)
        {
            sb.Append('<');
            sb.Append(FontLabel.LABEL_FONT);
            sb.Append(' ');
            sb.Append(FontLabel.ATT_FONT_fsize);
            sb.Append('=');
            sb.Append(size);
            sb.Append(' ');
            sb.Append(FontLabel.ATT_FONT_fcolor);
            sb.Append('=');
            sb.Append(color);
            sb.Append(' ');
            sb.Append(FontLabel.ATT_FONT_line);
            sb.Append('=');
            sb.Append(lineColor);
            sb.Append('>');
            sb.Append(str);
            return this;
        }

        /// <summary>
        /// 换行 
        /// </summary>
        /// <returns>此对象的一个引用。</returns>
        public XmlBuilder NewLine()
        {
            sb.Append('<');
            sb.Append(FontLabel.LABEL_BR);
            sb.Append('>');
            return this;
        }

        /// <summary>
        /// 换行 
        /// </summary>
        /// <param name="clearAlign">是否清除对齐方式</param>
        /// <returns>此对象的一个引用。</returns>
        public XmlBuilder NewLine(bool clearAlign)
        {
            sb.Append('<');
            sb.Append(FontLabel.LABEL_BR);
            sb.Append('>');
            if (clearAlign)
            {
                return AlignLeft();
            }
            return this;
        }


        private XmlBuilder Align(int align)
        {
            sb.Append('<');
            sb.Append(FontLabel.LABEL_Align);
            sb.Append(' ');
            sb.Append(FontLabel.ATT_Type);
            sb.Append('=');
            sb.Append(align);
            sb.Append('>');
            return this;
        }

        /// <summary>
        /// 设置为中心对齐
        /// </summary>
        /// <returns>此对象的一个引用。</returns>
        public XmlBuilder AlignCenter()
        {
            return Align(FontLabel.Value_Align_Center);
        }

        /// <summary>
        /// 设置为左对齐
        /// </summary>
        /// <returns>此对象的一个引用。</returns>
        public XmlBuilder AlignLeft()
        {
            return Align(FontLabel.Value_Align_Left);
        }

        /// <summary>
        /// 设置为右对齐
        /// </summary>
        /// <returns>此对象的一个引用。</returns>
        public XmlBuilder AlignRight()
        {
            return Align(FontLabel.Value_Align_Right);
        }

        /// <summary>
        /// 获取一个动作的XML
        /// </summary>
        /// <param name="resid"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <returns></returns>
        public XmlBuilder AnimXML(string resid, int width, int height)
        {
            sb.Append('<');
            sb.Append(FontLabel.LABEL_ANI);
            sb.Append(' ');
            sb.Append(FontLabel.ATT_id);
            sb.Append('=');
            sb.Append(resid);
            sb.Append(' ');
            sb.Append(FontLabel.ATT_width);
            sb.Append('=');
            sb.Append(width);
            sb.Append(' ');
            sb.Append(FontLabel.ATT_height);
            sb.Append('=');
            sb.Append(height);
            sb.Append('>');
            return this;
        }

        /// <summary>
        /// 获取一个动作的XML
        /// </summary>
        /// <param name="resid"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        public XmlBuilder AnimXML(string resid, int width, int height, float scale)
        {
            sb.Append('<');
            sb.Append(FontLabel.LABEL_ANI);
            sb.Append(' ');
            sb.Append(FontLabel.ATT_id);
            sb.Append('=');
            sb.Append(resid);
            sb.Append(' ');
            sb.Append(FontLabel.ATT_width);
            sb.Append('=');
            sb.Append(width);
            sb.Append(' ');
            sb.Append(FontLabel.ATT_height);
            sb.Append('=');
            sb.Append(height);
            sb.Append(' ');
            sb.Append(FontLabel.ATT_scale);
            sb.Append('=');
            sb.Append(scale);
            sb.Append('>');
            return this;
        }

        /// <summary>
        /// 获取一个图片的XML
        /// </summary>
        /// <param name="resid"></param>
        /// <returns></returns>
        public XmlBuilder PngXML(int resid)
        {
            sb.Append('<');
            sb.Append(FontLabel.LABEL_PNG);
            sb.Append(' ');
            sb.Append(FontLabel.ATT_id);
            sb.Append('=');
            sb.Append(resid);
            sb.Append('>');
            return this;
        }

        /// <summary>
        /// 获取一个图片的XML
        /// </summary>
        /// <param name="resid"></param>
        /// <returns></returns>
        public XmlBuilder PngXML(string resid)
        {
            sb.Append('<');
            sb.Append(FontLabel.LABEL_PNG);
            sb.Append(' ');
            sb.Append(FontLabel.ATT_id);
            sb.Append('=');
            sb.Append(resid);
            sb.Append('>');
            return this;
        }

        /// <summary>
        /// 获取一个图片的XML
        /// </summary>
        /// <param name="resid"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <returns></returns>
        public XmlBuilder PngXML(int resid, int width, int height)
        {
            sb.Append('<');
            sb.Append(FontLabel.LABEL_PNG);
            sb.Append(' ');
            sb.Append(FontLabel.ATT_id);
            sb.Append('=');
            sb.Append(resid);
            sb.Append(' ');
            sb.Append(FontLabel.ATT_width);
            sb.Append('=');
            sb.Append(width);
            sb.Append(' ');
            sb.Append(FontLabel.ATT_height);
            sb.Append('=');
            sb.Append(height);
            sb.Append('>');
            return this;
        }

        /// <summary>
        /// 获取一个图片的XML
        /// </summary>
        /// <param name="resid"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <returns></returns>
        public XmlBuilder PngXML(string resid, int width, int height)
        {
            sb.Append('<');
            sb.Append(FontLabel.LABEL_PNG);
            sb.Append(' ');
            sb.Append(FontLabel.ATT_id);
            sb.Append('=');
            sb.Append(resid);
            sb.Append(' ');
            sb.Append(FontLabel.ATT_width);
            sb.Append('=');
            sb.Append(width);
            sb.Append(' ');
            sb.Append(FontLabel.ATT_height);
            sb.Append('=');
            sb.Append(height);
            sb.Append('>');
            return this;
        }

        /// <summary>
        /// 创建可以点击的用户名字
        /// </summary>
        /// <param name="roleID">用户id</param>
        /// <param name="name">用户名</param>
        /// <param name="color">名字的颜色</param>
        /// <returns></returns>
        public XmlBuilder RoleInfoXML(long roleID, string name, int color)
        {
            sb.Append('<');
            sb.Append(FontLabel.LABEL_FONT);
            sb.Append(' ');
            sb.Append(FontLabel.ATT_roleID);
            sb.Append('=');
            sb.Append(roleID);
            sb.Append(' ');
            sb.Append(FontLabel.ATT_FONT_fcolor);
            sb.Append('=');
            sb.Append(color);
            sb.Append('>');
            sb.Append(name);
            return this;
        }


        /// <summary>
        /// 自定义形式的标签
        /// </summary>
        /// <param name="label">标签类型</param>
        /// <param name="str">标签带的字符串</param>
        /// <param name="attributes">标签的属性</param>
        /// <returns>此对象的一个引用。</returns>
        public XmlBuilder DefineXML(string label, string str, params string[] attributes)
        {
            //该方法不是最优，临时实现
            if (label == null)
            {
                //以防被StringBuffer组合null
                throw new NullReferenceException("label connot be null");
            }
            sb.Append('<');
            sb.Append(label);
            foreach (string attribute in attributes)
            {
                sb.Append(' ');
                sb.Append(attribute);
            }
            sb.Append('>');
            sb.Append(str);
            return this;
        }

        public XmlBuilder DefineXML(string label, params string[] attributes)
        {
            //该方法不是最优，临时实现
            if (label == null)
            {
                //以防被StringBuffer组合null
                throw new NullReferenceException("label connot be null");
            }
            sb.Append('<');
            sb.Append(label);
            foreach (string attribute in attributes)
            {
                sb.Append(' ');
                sb.Append(attribute);
            }
            sb.Append('>');
            return this;
        }

        public XmlBuilder AppendText(string str)
        {
            if (Utils.IndexOf(sb, ">") < 0)
            {
                throw new InvalidOperationException("must have to a label");
            }
            sb.Append(str);
            return this;
        }

        /// <summary>
        /// 对象格式化为字符串的形式
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            //this.DefineXML("aaa","ab","b","c","d");
            return sb.ToString();
        }

        public string ToFormatString()
        {
            return ToFormatString(ToString());
        }

        public static string ToFormatString(string xmlStr)
        {
            //             int start = xmlStr.IndexOf('<');
            //             while (start >= 0)
            //             {
            //                 int length = xmlStr.IndexOf('>', start) - start + 1;
            //                 //Console.WriteLine("length = " + length);
            //                 if (length > 0)
            //                 {
            //                     string rep = xmlStr.Substring(start, length);
            //                     xmlStr = xmlStr.Replace(rep, "");
            // 
            //                 }
            //                 start = xmlStr.IndexOf('<', start + 1);
            //             }
            //return xmlStr;
            xmlStr = xmlStr.Replace("<" + FontLabel.LABEL_BR + ">", "\n");
            return Regex.Replace(xmlStr, "\\<[^<>]*\\>", "");
        }
    }
}
