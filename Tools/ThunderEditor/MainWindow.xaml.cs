
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

using System.Windows;
using System.Windows.Control;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using ThunderEditor.Editor;
using ThunderEditor.Logic;
using ThunderEditor.UserControls;
using ThunderEditor.Utils;
using LitJson;
using System.Diagnostics;

namespace ThunderEditor
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        public static System.Windows.Forms.PropertyGrid propertyInstanse;
        public static System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();

        public static MainWindow mainInstance;
        public static SceneCanvas sceneInstance;

        public MainWindow()
        {
            mainInstance = this;

            InitializeComponent();

            propertyInstanse = wpfPropertyGrid1;
            sceneInstance = mainSceneCanvas;


            InitTreeViewActor();
            InitTreeViewLevel();
        }
        /// <summary>
        /// 窗口大小改变时回调，并重设wpfPropertyGrid1的宽高
        /// </summary>
        /// <param name="sizeInfo"></param>
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            windowsFormsHost1.Width = ropertyGridColumn.ActualWidth - 10;
            windowsFormsHost1.Height = ropertyGridRow.ActualHeight;
        }
        /// <summary>
        /// gridSplitter 拉动时回调，并重设wpfPropertyGrid1的宽
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridSplitter_Move(object sender, RoutedEventArgs e)
        {
            double width = ropertyGridColumn.ActualWidth - 10;
            if (width > 0)
            {
                windowsFormsHost1.Width = width;
            }
        }

        private void File_Click(object sender, RoutedEventArgs e)
        {
            //wpfPropertyGrid1.SelectedObject = sender;

            //this.openFileDialog.Filter = "jpg文件(*.jpg)|*.jpg|gif文件(*.gif)|*.gif"; 
//             openFileDialog.Filter = "项目文件(*.json)|*.json";
//             if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) 
//             { 
//                 string PicFileName = openFileDialog.FileName; 
//             }
        }

        private void Run_Click(object sender, RoutedEventArgs e)
        {
            DebugGame();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            CloseSimulator();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (LevelManager.Instance.CurrentLevel != null)
            {
                LevelManager.Instance.CurrentLevel.Save();
            }
        }

        private void SaveAll_Click(object sender, RoutedEventArgs e)
        {
            LevelManager.Instance.Save();
        }

        private void tabItemLevel_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void treeViewLevel_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void treeViewActor_Loaded(object sender, RoutedEventArgs e)
        {


        }
        /////////////////////////////////////////////////////////////////////////////////
        //关卡相关

        /// <summary>
        /// 点击treeViewLevel空白处弹出功能菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeViewLevel_MouseRightButtonDown(object sender, RoutedEventArgs e)
        {
            TreeViewItem selectedNode = (TreeViewItem)treeViewLevel.SelectedItem;
            if (selectedNode == null)
            {
                ContextMenu treeViewMenu = new ContextMenu();

                MenuItem menuItem1 = new MenuItem();
                menuItem1.Name = "AddLevel";
                menuItem1.Header = "添加关卡";
                menuItem1.Click += new RoutedEventHandler(treeViewMenuItem_Click);

                MenuItem menuItem2 = new MenuItem();
                menuItem2.Name = "Flash";
                menuItem2.Header = "刷新";
                menuItem2.Click += new RoutedEventHandler(treeViewMenuItem_Click);

                treeViewMenu.Items.Add(menuItem1);
                treeViewMenu.Items.Add(menuItem2);

                treeViewLevel.ContextMenu = treeViewMenu;
            }
        }
        private void treeViewMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender;
            if (menuItem.Name == "AddLevel")
            {
                AddLevelItem();
            }
            else if (menuItem.Name == "Flash")
            {

            }
        }
        //初始化关卡
        private void InitTreeViewLevel()
        {
            string filePath = EConfig.Instance.ProjectPath + "Data/Levels/";
            DirectoryInfo dirInfo = new DirectoryInfo(filePath);
            List<string> levelFiles = FileSever.GetDirectoryFiles(dirInfo, ".");

            foreach (var item in levelFiles)
            {
                if (!item.Contains(".json")) continue;
                string jsonData = FileSever.ReadFileToString(item);
                try
                {
                    JsonData levelData = JsonMapper.ToObject(jsonData);
                    Level level = LevelManager.Instance.Add(levelData);
                    level.LevelFile = filePath;
                    if (level != null)
                    {
                        TreeViewItem levelTreeNode = new TreeViewItem();
                        levelTreeNode.Header = level.Name + "(" + level.Discribe + ")";
                        levelTreeNode.Name = "LevelNode";
                        levelTreeNode.IsExpanded = true;
                        //objTreeNode.Selected += levelItem_Selected;
                        levelTreeNode.PreviewMouseDown += levelNode_PreviewMouseDown;
                        levelTreeNode.PreviewMouseLeftButtonDown += levelNode_PreviewMouseLeftButtonDown;
                        treeViewLevel.Items.Add(levelTreeNode);

                        //
                        //Add Event
                        foreach (var event_ in level.GetEvents())
                        {
                            Event _event = (Event)event_;
                            TreeViewItem eventTreeNode = new TreeViewItem();
                            eventTreeNode.Header = _event.Name + "(" + _event.Discribe + ")";
                            eventTreeNode.Name = "EventNode";
                            eventTreeNode.IsExpanded = true;
                            eventTreeNode.PreviewMouseDown += eventItem_PreviewMouseDown;
                            eventTreeNode.PreviewMouseLeftButtonDown += eventNode_PreviewMouseLeftButtonDown;
                            levelTreeNode.Items.Add(eventTreeNode);

                            _event.BindingItem = eventTreeNode;

                            Binding eventbinding = new Binding();
                            eventbinding.Source = _event;
                            eventTreeNode.SetBinding(TreeViewItem.ItemsSourceProperty, eventbinding);
                            //wpfPropertyGrid1.SelectedObject = _event;
                        }
                        //

                        //Binding Level
                        level.BindingItem = levelTreeNode;
                        //邦定数据
                        Binding levelbinding = new Binding();
                        levelbinding.Source = level;
                        levelTreeNode.SetBinding(TreeViewItem.ItemsSourceProperty, levelbinding);
                        //wpfPropertyGrid1.SelectedObject = level;
                    }                   
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

            }
        }

        /// <summary>
        /// 添加关卡
        /// </summary>
        private void AddLevelItem()
        {         
            AddLevelItem("lv" + (LevelManager.Instance.Count));
        }
        private void AddLevelItem(string name)
        {
            Level level = new Level(LevelManager.Instance, name);
            level.LevelFile = EConfig.Instance.ProjectPath + "Levels/" + level.Name + ".json";
            TreeViewItem objTreeNode = new TreeViewItem();
            objTreeNode.Header = name + "("+ level.Discribe +")";
            objTreeNode.Name = "LevelNode";
            objTreeNode.IsExpanded = true;
            //objTreeNode.Selected += levelItem_Selected;
            objTreeNode.PreviewMouseDown += levelNode_PreviewMouseDown;
            objTreeNode.PreviewMouseLeftButtonDown += levelNode_PreviewMouseLeftButtonDown;
            treeViewLevel.Items.Add(objTreeNode);

            //Binding Level
            level.BindingItem = objTreeNode;
            //邦定数据
            Binding binding = new Binding();
            binding.Source = level;
            objTreeNode.SetBinding(TreeViewItem.ItemsSourceProperty, binding);
            LevelManager.Instance.Add(level);
            wpfPropertyGrid1.SelectedObject = level;
            //
        }

        private void levelNode_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem levelTreeNode = (TreeViewItem)sender;
            BindingExpression bindingEx = levelTreeNode.GetBindingExpression(TreeViewItem.ItemsSourceProperty);
            Level level = (Level)bindingEx.DataItem;

            wpfPropertyGrid1.SelectedObject = level;
            LevelManager.Instance.CurrentLevel = level;

            Thunder.Common.EDebug.Mode = Thunder.Common.EDebug.DebugMode.Debug_Level;
        }

        private void levelNode_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem selectedNode = (TreeViewItem)sender;
            ContextMenu levelMenu = new ContextMenu();

            MenuItem menuItem1 = new MenuItem();
            menuItem1.Name = "AddEvent";
            menuItem1.Header = "添加事件";

            MenuItem menuItem2 = new MenuItem();
            menuItem2.Name = "Rename";
            menuItem2.Header = "重命名";

            MenuItem menuItem3 = new MenuItem();
            menuItem3.Name = "Remove";
            menuItem3.Header = "删除";

            MenuItem menuItem4 = new MenuItem();
            menuItem4.Name = "MoveUp";
            menuItem4.Header = "上移";

            MenuItem menuItem5 = new MenuItem();
            menuItem5.Name = "MoveDown";
            menuItem5.Header = "下移";

            MenuItem menuItem6 = new MenuItem();
            menuItem6.Name = "Save";
            menuItem6.Header = "保存";

            menuItem1.Click += new RoutedEventHandler(levelMenuItem_Click);
            menuItem2.Click += new RoutedEventHandler(levelMenuItem_Click);
            menuItem3.Click += new RoutedEventHandler(levelMenuItem_Click);
            menuItem4.Click += new RoutedEventHandler(levelMenuItem_Click);
            menuItem5.Click += new RoutedEventHandler(levelMenuItem_Click);
            menuItem6.Click += new RoutedEventHandler(levelMenuItem_Click);

            levelMenu.Items.Add(menuItem1);
            levelMenu.Items.Add(menuItem2);
            levelMenu.Items.Add(menuItem3);
            levelMenu.Items.Add(menuItem4);
            levelMenu.Items.Add(menuItem5);
            levelMenu.Items.Add(menuItem6);

            selectedNode.ContextMenu = levelMenu;
        }

        private void levelMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender;
            if (menuItem.Name == "AddEvent")
            {
                AddEventItem();
            }
            else if (menuItem.Name == "Rename")
            {

            }
            else if (menuItem.Name == "Remove")
            {

            }
            else if (menuItem.Name == "MoveUp")
            {
                //LevelManager.Instance.MoveUp();

            }
            else if (menuItem.Name == "MoveDown")
            {
                //LevelManager.Instance.MoveDown();
            }
            else if (menuItem.Name == "Save")
            {
                LevelManager.Instance.CurrentLevel.Save();
            }
        }
        private void RemoveLevelItem(string name)
        {

        }

        private void RemoveLevelItem(TreeViewItem item)
        {

        }
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //关卡事件相关
        private void AddEventItem()
        {
            Level level = LevelManager.Instance.CurrentLevel;
            AddEventItem("event" + (level.Count));
        }

        private void AddEventItem(string name)
        {
            Level level = LevelManager.Instance.CurrentLevel;
            if (level != null)
            {
                Event _event = level.AddEvent(name);

                TreeViewItem parentNode = level.BindingItem;
                TreeViewItem objTreeNode = new TreeViewItem();
                objTreeNode.Header = name + "("+_event.Discribe + ")";
                objTreeNode.Name = "EventNode";
                objTreeNode.IsExpanded = true;
                objTreeNode.PreviewMouseDown += eventItem_PreviewMouseDown;
                objTreeNode.PreviewMouseLeftButtonDown += eventNode_PreviewMouseLeftButtonDown;
                parentNode.Items.Add(objTreeNode);



                _event.BindingItem = objTreeNode;

                Binding binding = new Binding();
                binding.Source = _event;
                objTreeNode.SetBinding(TreeViewItem.ItemsSourceProperty, binding);
                wpfPropertyGrid1.SelectedObject = _event;
            }
        }

        private void eventItem_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem levelTreeNode = (TreeViewItem)sender;
            BindingExpression bindingEx = levelTreeNode.GetBindingExpression(TreeViewItem.ItemsSourceProperty);
            Event _event = (Event)bindingEx.DataItem;

            mainSceneCanvas.OnResetScene(_event);
            wpfPropertyGrid1.SelectedObject = _event;
            LevelManager.Instance.CurrentLevel.CurrentEvent = _event;

            Thunder.Common.EDebug.Mode = Thunder.Common.EDebug.DebugMode.Debug_Event;
        }

        private void eventNode_PreviewMouseLeftButtonDown(object sender, RoutedEventArgs e)
        {

        }

        private void RemoveEventItem()
        {

        }

        private void RemoveEventItem(TreeViewItem item)
        {

        }
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //角色相关
        private void InitTreeViewActor()
        {
            //初始化树节点
            treeViewActor.Items.Clear();

            for (int i = 0; i < EConfig.Instance.ActorData.Count; i++)
            {
                JsonData actorFiter = EConfig.Instance.ActorData[i];
                TreeViewItem actorTreeNode = new TreeViewItem();

                BrowseType btype = (BrowseType)(int)actorFiter["type"];
                actorTreeNode.Header = (string)actorFiter["name"];
                actorTreeNode.Name = "ActorNode";
                actorTreeNode.IsExpanded = true;

                foreach (var actor in actorFiter["actors"])
                {

                    EActorType actorInfo;
                    if (EActorBrowse.Instanse.AddActorType((JsonData)actor, out actorInfo,btype))
                    {
                        //StackPanel 
                        StackPanel imgPanel = new StackPanel();
                        imgPanel.Orientation = Orientation.Vertical;
                        imgPanel.PreviewMouseLeftButtonDown += imgPanel_PreviewMouseLeftButtonDown;
                        //
                        TextBlock textBlock = new TextBlock();
                        textBlock.Text = actorInfo.SpawnInfo.name;
                        textBlock.VerticalAlignment = VerticalAlignment.Center;
                        imgPanel.Children.Add(textBlock);
                        //Image 
                        Image img = new Image();
                        img.Source = actorInfo.Display;
                        img.Width = actorInfo.Display.PixelWidth;
                        img.Height = actorInfo.Display.PixelHeight;
                        imgPanel.Children.Add(img);
                        //binding data
                        Binding binding = new Binding();
                        binding.Source = actorInfo;
                        actorInfo.BindingControl = imgPanel;
                        imgPanel.SetBinding(TreeViewItem.ItemsSourceProperty, binding);
                        //
                        actorTreeNode.Items.Add(imgPanel);
                    }
                }

                treeViewActor.Items.Add(actorTreeNode);
            }
        }

        private void imgPanel_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            StackPanel item = (StackPanel)sender;
            BindingExpression binding = item.GetBindingExpression(TreeViewItem.ItemsSourceProperty);
            EActorType actorType = (EActorType)binding.DataItem;
            mainSceneCanvas.BindingBrush(actorType);
            wpfPropertyGrid1.SelectedObject = actorType;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public static void DebugGame()
        {
            Thunder.Common.EDebug.InEditor = true;
            //Thunder.Common.EDebug.Mode = Thunder.Common.EDebug.DebugMode.Debug_Level;
            if (LevelManager.Instance.CurrentLevel != null)
            {
                LevelManager.Instance.CurrentLevel.Save();
                Thunder.Common.EDebug.DebugLevel = LevelManager.Instance.CurrentLevel.Name;
                if (LevelManager.Instance.CurrentEvent != null)
                {
                    Thunder.Common.EDebug.DebugEvent = LevelManager.Instance.CurrentEvent.Name;
                }
            }
            else
            {
                Thunder.Common.EDebug.Mode = Thunder.Common.EDebug.DebugMode.Debug_Normal;
            }

            //save debug config
            FileSever.WriteStringToFile(EConfig.Instance.ProjectPath + "Config/debug.cf", Thunder.Common.EDebug.SerializeJson());
            RunSimulator("debug");
        }

        static int sessionId = -1;
        private static void RunSimulator(string arg)
        {
            CloseSimulator();

            Process process = new Process();
            //process.StartInfo.UseShellExecute = false;
            process.StartInfo.FileName = System.Environment.CurrentDirectory +"/"+ EConfig.Instance.SimulatorPath;
            process.StartInfo.WorkingDirectory = EConfig.Instance.ProjectPath;
            process.StartInfo.Arguments = arg;
            //process.StartInfo.CreateNoWindow = true;
            process.Start();

            sessionId = process.Id;
        }

        private static void CloseSimulator()
        {
            try
            {
                Process myProcessA = Process.GetProcessById(sessionId);     // 通过ID关联进程
                if (myProcessA.ToString().Contains("Matrix"))
                {
                    myProcessA.Kill();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("无此进程！！");
                Console.WriteLine(e);
            }
        }
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }

}
