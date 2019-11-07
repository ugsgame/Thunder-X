using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Thunder.Common
{
    static public class EDebug
    {

        public enum DebugMode
        {
            Debug_Normal,
            Debug_Event,
            Debug_Level
        }
        //模块开关
        public static readonly bool swBullet = true;       //子弹
        public static readonly bool swDrop = true;         //掉落
        public static readonly bool swScript = true;      //脚本
        public static readonly bool swMap = true;         //地图
        public static readonly bool swActorActionPool = false;  //Actor的action池管理
        //
        public static readonly bool dLevel = false;      //默认打开所有关卡

        static bool inEditor;
        public static bool InEditor
        {
            get { return inEditor; }
            set { inEditor = value; }
        }

        static DebugMode mode = DebugMode.Debug_Normal;
        public static DebugMode Mode
        {
            get { return mode;}
            set { mode = value; }
        }

        static string debugLevel;
        public static string DebugLevel
        {
            get { return debugLevel; }
            set { debugLevel = value; }
        }

        static string debugEvent;
        public static string DebugEvent
        {
            get { return debugEvent; }
            set { debugEvent = value; }
        }

        public static string SerializeJson()
        {
            StringBuilder jsonText = new StringBuilder();

            jsonText.Append("{");
            jsonText.AppendLine();
            jsonText.AppendFormat("\t\"inEditor\":{0},\n",inEditor.ToString().ToLower());
            jsonText.AppendFormat("\t\"debugMode\":{0},\n",(int)mode);
            jsonText.AppendFormat("\t\"debugLevel\":\"{0}\",\n", debugLevel);
            jsonText.AppendFormat("\t\"debugEvent\":\"{0}\"\n", debugEvent);
            jsonText.Append("}");
            jsonText.AppendLine();
            return jsonText.ToString();
        }

        public static void UnserializeJson(string json)
        {
            try
            {
                Console.WriteLine(json);
                LitJson.JsonData jsonData = LitJson.JsonMapper.ToObject(json);

                InEditor = (bool)jsonData["inEditor"];
                Mode = (DebugMode)(int)jsonData["debugMode"];
                DebugLevel = (string)jsonData["debugLevel"];
                DebugEvent = (string)jsonData["debugEvent"];
            }
            catch (Exception e)
            {
                InEditor = false;
                Console.WriteLine(e);
            }

        }
    }
}
