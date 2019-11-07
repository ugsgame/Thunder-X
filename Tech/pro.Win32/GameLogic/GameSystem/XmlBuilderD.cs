using System;
using System.Collections.Generic;
using System.Text;

namespace Thunder.GameLogic.GameSystem
{
    public class XmlBuilderD
    {
        internal readonly StringBuilder sb = new StringBuilder(1024);

        public void DefineXml(string label,string str)
        {

        }
    }


    public class XmlAttribute
    {
        internal readonly XmlBuilderD xb;
        internal XmlAttribute(XmlBuilderD xb)
        {
            this.xb = xb;
        }




    }
}
