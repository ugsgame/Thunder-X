using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;

namespace ThunderEditor.Logic
{
    /// <summary>
    /// 关卡管理器
    /// </summary>
    public class LevelManager
    {
        List<Level> levels = new List<Level>(16);

        public static LevelManager Instance = new LevelManager();

        public LevelManager()
        {

        }

        public bool MoveUp()
        {
            return MoveUp(levels.IndexOf(CurrentLevel));
        }

        public bool MoveUp(int index)
        {
            if (index <= 1)
            {
                return false;
            }
            else
            {
                TreeViewItem treeItem1 = levels[index].BindingItem;
                TreeViewItem treeItem2 = levels[index - 1].BindingItem;

                var temp = treeItem1.Header;
                treeItem1.Header = treeItem2.Header;
                treeItem2.Header = temp;

                temp = treeItem1.Name;
                treeItem1.Name = treeItem2.Name;
                treeItem2.Name = temp.ToString();

                //                 var temp = levels[index - 1];
                //                 levels[index - 1] = levels[index];
                //                 levels[index] = temp;

                //重新绑定
                //                 Binding binding1 = new Binding();
                //                 binding1.Source = levels[index - 1];
                //                 levels[index].BindingItem.SetBinding(TreeViewItem.ItemsSourceProperty, binding1);
                // 
                //                 Binding binding2 = new Binding();
                //                 binding2.Source = levels[index];
                //                 levels[index - 1].BindingItem.SetBinding(TreeViewItem.ItemsSourceProperty, binding2);
            }
            return true;
        }

        public bool MoveDown()
        {
            return MoveDown(levels.IndexOf(CurrentLevel));
        }
        public bool MoveDown(int index)
        {
            if (index == levels.Count - 1)
            {
                return false;
            }
            else
            {
                TreeViewItem treeItem1 = levels[index].BindingItem;
                TreeViewItem treeItem2 = levels[index + 1].BindingItem;

                var temp = treeItem1.Header;
                treeItem1.Header = treeItem2.Header;
                treeItem2.Header = temp;

                temp = treeItem1.Name;
                treeItem1.Name = treeItem2.Name;
                treeItem2.Name = temp.ToString();

                //                 temp = levels[index + 1];
                //                 levels[index + 1] = levels[index];
                //                 levels[index] = (Level)temp;

                //重新绑定
                Binding binding1 = new Binding();
                binding1.Source = levels[index + 1];
                levels[index].BindingItem.SetBinding(TreeViewItem.ItemsSourceProperty, binding1);

                Binding binding2 = new Binding();
                binding2.Source = levels[index];
                levels[index + 1].BindingItem.SetBinding(TreeViewItem.ItemsSourceProperty, binding2);
            }
            return true;
        }

        /// <summary>
        /// 添加关卡
        /// </summary>
        public Level Add(string name)
        {
            Level level = new Level(this, name);
            levels.Add(level);
            return level;
        }

        public void Add(Level level)
        {
            levels.Add(level);
        }

        public Level Add(JsonData levelData)
        {
            Level level = null;
            try
            {
                string describe = (string)levelData["describe"];
                string name = (string)levelData["name"];
                string map_name = (string)levelData["map_name"];
                int map_layer = (int)levelData["map_layer"];
                double map_speed = (double)levelData["map_speed"];
                double map_speedRate = (double)levelData["map_speedRate"];

                level = new Level(this, name);
                level.Discribe = describe;

                if (map_name == "Map1")
                    level.LevelMap = Level.Map.Map1;
                else if (map_name == "Map2")
                    level.LevelMap = Level.Map.Map2;
                else if (map_name == "Map3")
                    level.LevelMap = Level.Map.Map3;
                else if (map_name == "Map4")
                    level.LevelMap = Level.Map.Map4;

                level.Layer = map_layer;
                level.Speed = (float)map_speed;
                level.SpeedRate = (float)map_speedRate;
                //
                foreach (JsonData item in levelData["event_data"])
                {
                    level.AddEvent(item);
                }

                levels.Add(level);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return level;
        }

        public void Add(TreeViewItem bindingItem)
        {

        }

        public int Count
        {
            get { return levels.Count; }
        }


        Level level;
        /// <summary>
        /// 当前编辑的关卡
        /// </summary>
        internal Level CurrentLevel
        {
            get { return level; }
            set { level = value; }
        }

        /// <summary>
        /// 当前编辑的事件
        /// </summary>
        internal Event CurrentEvent
        {
            get { return CurrentLevel.CurrentEvent; }
        }

        internal void Save()
        {
            foreach (var item in levels)
            {
                item.Save();
            }
        }
    }
}
