using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Thunder.GameLogic.GameSystem
{
    public class Xml
    {
        private const char LEFT = '<';
        private const char RIGTH = '>';
        private const char BLANK = ' ';
        private const char EQUAL = '=';
        private const int FIND_LEFT = 0;         //查找'<'
        private const int FIND_TAG_START = 1;    //查找tag开头
        private const int FIND_TAG = 2;          //查找'<'之后的内容
        private const int FIND_KEY_START = 3;    //查找key的开头
        private const int FIND_KEY = 4;          //查找到key的内容
        private const int FIND_EQUAL = 5;        //查找'='
        private const int FIND_VALUE_START = 6;  //查找value的开头
        private const int FIND_VALUE = 7;        //查找value的内容
        //    private const int FIND_TEXT_START = 8;   //查找text的开头
        //    private const int FIND_TEXT_END = 9;     //查找text的结束
        private const int FIND_TEXT = 8;
        //所有节点
        private List<XmlNode> nodes;


        public Xml(string text)
        {
            if (text.Length == 0)
            {
                return;
            }

            if (text[0] != '<')
            {
                nodes = new List<XmlNode>(1);
                XmlNode node = new XmlNode(FontLabel.LABEL_FONT, text);
                nodes.Add(node);
                return;
            }
            else
            {
                nodes = new List<XmlNode>(16);
            }

            Xml xml = this;
            int index = 0;
            int last = 1;
            int start = 0;
            XmlNode nowXN = null;
            int state = 0;//初始化为0
            int firstKey = 0;
            String key = null;
            int firstValue = 0;
            int length = text.Length;
            for (int i = index; i < length; i++)
            {
                char c = text[i];
                switch (state)
                {
                    case FIND_LEFT://找到'<'，并创建一个新的节点，添加到列表中，确定tag的开始(TODO tag的空格没做。)
                        if (c == LEFT)
                        {
                            nowXN = new XmlNode();
                            xml.nodes.Add(nowXN);
                            start = i + 1;
                            state = FIND_TAG_START;
                        }
                        break;
                    case FIND_TAG_START://找到tag的开头，可能是空格或者
                        if (c != BLANK)
                        {
                            state = FIND_TAG;
                        }
                        else
                        {
                            start++;
                        }
                        break;
                    case FIND_TAG:          //找<的内容，最先遇到的可能是空格，或者>
                        if (c == RIGTH)
                        {   //找到'>'表示这个节点已经结束，查找这个节点有没有文本，进入查找文本开头的状态
                            nowXN.name = substring(text, start, i);

                            last = i + 1;
                            state = FIND_TEXT;
                        }
                        else if (c == BLANK)
                        {
                            nowXN.name = substring(text, start, i);
                            firstKey = i + 1;
                            nowXN.atts = new Dictionary<String, String>();
                            state = FIND_KEY_START;
                        }
                        break;
                    case FIND_KEY_START:    //找查找属性key的开头
                        if (c == RIGTH)
                        {
                            state = FIND_TEXT;
                        }
                        else if (c == BLANK)
                        {
                            firstKey++;
                        }
                        else
                        {
                            state = FIND_KEY;
                        }
                        break;
                    case FIND_KEY:      //找key
                        if (c == EQUAL)
                        {
                            key = substring(text, firstKey, i);
                            firstValue = i + 1;
                            state = FIND_VALUE_START;
                        }
                        else if (c == BLANK)
                        {
                            key = substring(text, firstKey, i);
                            state = FIND_EQUAL;
                        }
                        break;
                    case FIND_EQUAL:
                        if (c == EQUAL)
                        {
                            firstValue = i + 1;
                            state = FIND_VALUE_START;
                        }
                        break;
                    case FIND_VALUE_START:
                        if (c == BLANK)
                        {
                            firstValue++;
                        }
                        else
                        {
                            state = FIND_VALUE;
                        }
                        break;
                    case FIND_VALUE://找value
                        if (c == RIGTH)
                        {
                            String value = substring(text, firstValue, i);
                            nowXN.atts[key] = value;
                            last = i + 1;
                            state = FIND_TEXT;
                        }
                        else if (c == BLANK)
                        {
                            String value = substring(text, firstValue, i);
                            nowXN.atts[key] = value;
                            firstKey = i + 1;
                            state = FIND_KEY_START;
                        }
                        break;
                    case FIND_TEXT:
                        if (c == LEFT)
                        {//遇到'<'检查是否能产生旧节点的text，并产生新节点
                            if (i >= last)
                            {
                                nowXN.text = substring(text, last, i);
                            }
                            nowXN = new XmlNode();
                            xml.nodes.Add(nowXN);
                            start = i + 1;
                            state = FIND_TAG_START;
                        }
                        break;
                }
            }
            if (last <= length && nowXN != null && nowXN.text == null)
            {
                nowXN.text = substring(text, last, length);//处理最后一个不带空格，但又有text的情况
            }
        }


        /**
         * 获得所有节点
         * @return
         */
        public XmlNode this[int index]
        {
            get
            {
                return nodes[index];
            }
        }

        public int XmlNodeCount
        {
            get
            {
                return nodes.Count;
            }
        }


        public override String ToString()
        {
            if (this.nodes == null)
            {
                return "";
            }
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < nodes.Count; i++)
            {
                XmlNode xn = nodes[i];
                sb.Append(xn).Append("\n");
            }
            return sb.ToString();
        }

        public static Xml CreateXml(String text)
        {
            return new Xml(text);
        }

        private static string substring(string str, int startIndex, int endIndex)
        {
            return str.Substring(startIndex, endIndex - startIndex);
        }

    }


    public class XmlNode
    {
        //节点名称
        internal protected string name = "";
        //文本内容
        internal protected string text;
        //属性内容
        internal protected Dictionary<String, String> atts;

        static XmlNode()
        {

        }
        internal XmlNode()
        {
        }

        public XmlNode(string label, string str)
        {
            this.SetXmlNode(label, str);
        }

        public void SetXmlNode(string label, string str)
        {
            if (label == null || label.Length <= 0)
            {
                throw new NullReferenceException();
            }
            this.name = label;
            this.text = str;
        }

        public override int GetHashCode()
        {
            return name.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is XmlNode)
            {
                XmlNode xmlNode = (XmlNode)obj;
                if (xmlNode.name == this.name)
                {
                    if (this.atts != null && xmlNode.atts != null)
                    {
                        if (this.atts.Count == xmlNode.atts.Count)
                        {
                            string[] a = this.atts.Values.ToArray();
                            string[] b = xmlNode.atts.Values.ToArray();
                            for (int i = 0; i < a.Length; i++)
                            {
                                if (a[i] != b[i])
                                {
                                    return false;
                                }
                            }

                        }
                    }
                }
            }
            return false;
        }

        public override string ToString()
        {
            if (atts != null)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append('<');
                sb.Append(this.name);
                foreach (KeyValuePair<string, string> kvp in atts)
                {
                    sb.Append(' ');
                    sb.Append(kvp.Key).Append("=").Append(kvp.Value).Append(",");
                }
                sb.Append('>');
                sb.Append(this.text);
                //return this.name + ":{text=" + text + "," + sb.ToString() + "}";
                return sb.ToString();
            }
            return "";
        }

        /**
         * @return the name
         */
        public string GetName()
        {
            return name;
        }

        /**
         * @return the text
         */
        public string GetText()
        {
            return text;
        }

        /**
         * 获得某个属性内容
         */
        public string GetAttribute(string key)
        {
            if (this.atts == null)
            {
                return null;
            }
            try
            {
                return this.atts[key];
            }
            catch (KeyNotFoundException)
            {
            }
            return null;
        }

        public void SetAttribute(string key, string value)
        {
            if (atts == null)
            {
                this.atts = new Dictionary<String, String>();
            }
            this.atts[key] = value;
        }

        /**
         * 如果此节点没有属性
         * @return true
         */
        public bool IsEmpty()
        {
            return this.atts != null && this.atts.Count == 0;
        }
    }


}
