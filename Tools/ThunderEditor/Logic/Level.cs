using LitJson;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using ThunderEditor.Editor;
using ThunderEditor.Utils;

namespace ThunderEditor.Logic
{
    /// <summary>
    /// 关卡
    /// </summary>
    public class Level
    {

        LevelManager levelManager;
        string levelFilePath;

        public enum Map
        {
            Map1,
            Map2,
            Map3,
            Map4,
        }

        List<Event> events = new List<Event>(6);

        string discribe = "关卡";
        [Category("描述")]
        public string Discribe
        {
            get { return discribe; }
            set 
            { 
                discribe = value;
                if (bindingItem != null)
                {
                    bindingItem.Header = name + "(" + discribe + ")";
                }
            }
        }

        string name = "lv1";
        [Category("描述")]
        public string Name
        {
            get { return name; }
            set 
            { 
                name = value;
                if (bindingItem != null)
                {
                    bindingItem.Header = name + "(" + discribe + ")";
                }
            }
        }

        public string LevelFile
        {
            get { return levelFilePath + this.name + ".json"; }
            set { levelFilePath = value; }
        }

        Map map_name = Map.Map1;
        public Map LevelMap
        {
            get { return map_name; }
            set { map_name = value; }
        }

        int map_layer = 2;
        public int Layer
        {
            get { return map_layer; }
            set { map_layer = value; }
        }

        float map_speed = 50.0f;
        public float Speed
        {
            get { return map_speed; }
            set { map_speed = value; }
        }

        float map_speedRate = 2.5f;
        public float SpeedRate
        {
            get { return map_speedRate; }
            set { map_speedRate = value; }
        }

        TreeViewItem bindingItem;
        /// <summary>
        /// 当前邦定的Item
        /// </summary>
        internal TreeViewItem BindingItem
        {
            get { return bindingItem; }
            set { bindingItem = value; }
        }

        Event currentEvent;
        /// <summary>
        /// 当前编辑的事件
        /// </summary>
        internal Event CurrentEvent
        {
            get { return currentEvent; }
            set { currentEvent = value; }
        }

        JsonData jsonData = new JsonData();
        internal JsonData JsonData
        {
            get { return jsonData; }
            set { jsonData = value; }
        }

        internal bool MoveUp()
        {
            return true;
        }

        internal bool MoveUp(Event _event)
        {
            return true;
        }

        public Event AddEvent(string name)
        {
            Event _event = new Event(this, name);
            events.Add(_event);
            return _event;
        }

        public Event AddEvent(JsonData eventData)
        {
            Event _event = null;
            try
            {
                jsonData = eventData;

                string describe = (string)eventData["describe"];
                string name = (string)eventData["name"];
                bool crossCondition = (bool)eventData["crossCondition"];
                double crossTime = (double)eventData["crossTime"];
                double waitingTime = (double)eventData["waitingTime"];
                double wOriginX = (double)eventData["wOriginX"];
                double wOriginY = (double)eventData["wOriginY"];
                _event = new Event(this,name);

                _event.Discribe = describe;
                _event.是否为时间过关 = crossCondition;
                _event.过关时间 = (float)crossTime;
                _event.开场时间 = (float)waitingTime;
                _event.ActorManager.WorldOrigin = new System.Windows.Point(wOriginX, wOriginY);

                foreach (JsonData item in (JsonData)eventData["actors"])
                {
                    _event.AddActor(item);
                }

                this.events.Add(_event);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return _event;
        }

        public int Count
        {
            get { return events.Count; }
        }

        public List<Event> GetEvents()
        {
            return events;
        }

        public Level(LevelManager manager, string name)
        {
            levelManager = manager;
            Name = name;
        }

        public virtual void UnserializeJson(ref StringBuilder mainRoot)
        {
            mainRoot.Append("{");
            mainRoot.AppendLine();

            mainRoot.AppendFormat("\t\"{0}\":\"{1}\",\n", "describe", this.discribe);
            mainRoot.AppendFormat("\t\"{0}\":\"{1}\",\n", "name", this.name);
            mainRoot.AppendFormat("\t\"{0}\":\"{1}\",\n", "map_name", this.map_name);
            mainRoot.AppendFormat("\t\"{0}\":{1},\n", "map_layer", this.map_layer);
            mainRoot.AppendFormat("\t\"{0}\":{1:F4},\n", "map_speed", this.map_speed);
            mainRoot.AppendFormat("\t\"{0}\":{1:F4},\n", "map_speedRate", this.map_speedRate);
            mainRoot.Append("\t\"event_data\": [");
            //Unserialize Event
            for (int i = 0; i < this.events.Count; i++)
            {
                mainRoot.AppendLine();
                events[i].UnserializeJson(ref mainRoot);
                if (i < this.events.Count-1) mainRoot.Append(",");
            }
            mainRoot.AppendLine();
            //
            mainRoot.Append("\t]\n");
            mainRoot.Append("}");
        }

        public virtual void Save()
        {
            StringBuilder jsonText = new StringBuilder();
            UnserializeJson(ref jsonText);
            FileSever.WriteStringToFile(this.levelFilePath + this.name + ".json", jsonText.ToString());
        }
    }
}
