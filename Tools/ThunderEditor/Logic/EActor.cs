
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using MatrixEngine.CocoStudio.Armature;
using Thunder.GameLogic.Gaming;
using Thunder.GameLogic.Gaming.Actors;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using MatrixEngine.Math;
using ThunderEditor.Utils;
using ThunderEditor.Editor;
using Thunder.GameLogic.Gaming.Actors.Drops;


namespace ThunderEditor.Logic
{
    /// <summary>
    /// 角色
    /// </summary>
    public class EActor : Transform
    {
        private Brush selectedDrawingBrush = Brushes.LightGreen;
        private DrawingVisual selectedDrawingVisual = new DrawingVisual();
        private Pen drawingPen = new Pen(Brushes.SteelBlue, 3);

        SpawnInfo spawnInfo;

        public enum EProp
        {
            无,
            升级,
            护盾,
            技能,
        }

        public class Emitter
        {
            string fileName = "";
            public Emitter()
            {
                MainWindow.openFileDialog.Filter = "子弹文件(*.bt)|*.bt";
                if (MainWindow.openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    fileName = FileSever.GetFileName(MainWindow.openFileDialog.FileName);
                }
            }
            public Emitter(string str)
            {
                fileName = str;
            }
            public override string ToString()
            {
                return fileName;
            }
        }

        EActorType actorInfo;
        internal EActorType Info
        {
            get { return actorInfo; }
            set { actorInfo = value; }
        }
        BrowseType browseType = BrowseType.Browse_Actor;
        internal BrowseType BType
        {
            get { return browseType; }
            set { browseType = value; }
        }

        [Category("描述")]
        public string Discribe
        {
            get { return this.spawnInfo.describe; }
            set { this.spawnInfo.describe = value; }
        }

        [Category("描述")]
        public string 名字
        {
            get { return this.spawnInfo.name; }
            set { this.spawnInfo.name = value; }
        }

        [Category("描述"), ReadOnlyAttribute(true)]
        public string 动画集
        {
            get { return this.spawnInfo.armaName; }
            set { this.spawnInfo.armaName = value; }
        }

        [Category("描述"), ReadOnlyAttribute(true)]
        public string 动画前缀
        {
            get { return this.spawnInfo.animName; }
            set { this.spawnInfo.animName = value; }
        }
        [Category("描述"), ReadOnlyAttribute(true)]
        public SpawnType 类型
        {
            set { this.spawnInfo.spawnType = value; }
            get { return this.spawnInfo.spawnType; }
        }

        [Category("描述"), ReadOnlyAttribute(true)]
        public ActorID 角色ID
        {
            set { this.spawnInfo.actorId = value; }
            get { return this.spawnInfo.actorId; }
        }

        [Category("角色")]
        public float HP
        {
            set { this.spawnInfo.HP = value; }
            get { return this.spawnInfo.HP; }
        }

        [Category("角色")]
        public float 狂暴HP
        {
            set { this.spawnInfo.frenzyHp = value; }
            get { return this.spawnInfo.frenzyHp; }
        }

        [Category("角色")]
        public float 伤害
        {
            set { this.spawnInfo.damage = value; }
            get { return this.spawnInfo.damage; }
        }

        [Category("角色")]
        public bool 是否为敌人
        {
            set { this.spawnInfo.isEnemy = value; }
            get { return this.spawnInfo.isEnemy; }
        }

        [Category("角色")]
        public int 等级
        {
            set { this.spawnInfo.level = value; }
            get { return this.spawnInfo.level; }
        }

        [Category("角色")]
        public float 狂暴持续时间
        {
            set { this.spawnInfo.critTime = value; }
            get { return this.spawnInfo.critTime; }
        }

        [Category("角色")]
        public float 速度
        {
            set { this.spawnInfo.speed = value; }
            get { return this.spawnInfo.speed; }
        }

        [Category("角色")]
        public float 延时
        {
            set { this.spawnInfo.delayTime = value; }
            get { return this.spawnInfo.delayTime; }
        }

        System.Windows.Size gemBlue1;
        [Category("掉落")]
        public System.Windows.Size 蓝宝石小
        {
            set { gemBlue1 = value; }
            get { return gemBlue1; }
        }

        System.Windows.Size gemBlue2 = new System.Windows.Size(8,5);
        [Category("掉落")]
        public System.Windows.Size 蓝宝石大
        {
            set { gemBlue2 = value; }
            get { return gemBlue2; }
        }
        System.Windows.Size gemYellow1;
        [Category("掉落")]
        public System.Windows.Size 黄宝石小
        {
            set { gemYellow1 = value; }
            get { return gemYellow1; }
        }
        System.Windows.Size gemYellow2;
        [Category("掉落")]
        public System.Windows.Size 黄宝石大
        {
            set { gemYellow2 = value; }
            get { return gemYellow2; }
        }
        System.Windows.Size gemGreen1;
        [Category("掉落")]
        public System.Windows.Size 绿宝石小
        {
            set { gemGreen1 = value; }
            get { return gemGreen1; }
        }
        System.Windows.Size gemGreen2;
        [Category("掉落")]
        public System.Windows.Size 绿宝石大
        {
            set { gemGreen2 = value; }
            get { return gemGreen2; }
        }

        System.Windows.Size diamond;
        [Category("掉落")]
        public System.Windows.Size 钻石
        {
            set { diamond = value; }
            get { return diamond; }
        }

        EProp prop;
        [Category("掉落")]
        public EProp 道具
        {
            set { prop = value; }
            get { return prop; }
        }

        FilterEnum filterType = FilterEnum.All;
        [Category("碰撞筛选")]
        public FilterEnum Filter
        {
            set
            {
                filterType = value;
            }
            get { return filterType; }
        }

        /// //////////////////////////////////////////////////////////////////
        //发射器 
        Emitter[] emitters;
        [Category("角色")]
        public Emitter[] 发射器
        {
            get 
            { 
                return emitters;
            }
            set 
            { 
                emitters = value;
                this.spawnInfo.Emitters.Clear();
                foreach (var item in emitters)
                {
                    this.spawnInfo.Emitters.Add(item.ToString());
                }
            }
        }

        [Category("角色"), Description("脚本文件"), EditorAttribute(typeof(PropertyGridFileItem), typeof(System.Drawing.Design.UITypeEditor))]
        public string 行为脚本
        {
            set { this.spawnInfo.actionScript = FileSever.GetFileName(value); }
            get { return this.spawnInfo.actionScript; }
        }

        EActorManager actorManager;
        internal EActorManager ActorManager
        {
            set { actorManager = value; }
            get { return actorManager; }
        }

        public virtual void SetPosition(Point point)
        {
            base.PositionX = (float)point.X;
            base.PositionY = (float)point.Y;

            FlashWordPosition();
        }
        public virtual Point GetPostion()
        {
            return new Point(base.PositionX, base.PositionY);
        }

        public override void FlashWordPosition()
        {
            if (this.actorManager != null)
            {
                this.WorldPosition = this.actorManager.World.ToWorldPosition(this.position);
            }
        }

        System.Windows.Size displaySize;
        internal System.Windows.Size DisplaySize
        {
            set { displaySize = value; }
            get { return displaySize; }
        }

        BitmapImage display;
        internal BitmapImage Display
        {
            set
            {
                display = value;
                this.displaySize = new System.Windows.Size(display.PixelWidth, display.PixelHeight);
            }
            get
            {
                return display;
            }
        }

        bool selected;
        internal bool IsSelected
        {
            get { return selected; }
            set { selected = value; }
        }

        DrawingVisual visual;
        internal DrawingVisual Visual
        {
            get
            {
                return visual;
            }
        }

        public EActor(EActorType actorInfo)
        {
            this.actorInfo = actorInfo;
            this.spawnInfo = actorInfo.SpawnInfo.Clone();
            this.Display = actorInfo.Display;
            this.browseType = actorInfo.BType;
            visual = new DrawingVisual();
        }


        public virtual void UnserializeJson(ref StringBuilder mainRoot)
        {
            mainRoot.Append("\t\t\t {");
            mainRoot.AppendLine();
            mainRoot.AppendFormat("\t\t\t\t\"{0}\":\"{1}\",\n", "describe", this.spawnInfo.describe);
            mainRoot.AppendFormat("\t\t\t\t\"{0}\":{1},\n", "browseType", (int)this.browseType);
            mainRoot.AppendFormat("\t\t\t\t\"{0}\":\"{1}\",\n", "name", this.spawnInfo.name);
            mainRoot.AppendFormat("\t\t\t\t\"{0}\":{1},\n", "actorId", (int)this.spawnInfo.actorId);
            mainRoot.AppendFormat("\t\t\t\t\"{0}\":{1},\n", "spawnType", (int)this.spawnInfo.spawnType);
            mainRoot.AppendFormat("\t\t\t\t\"{0}\":\"{1}\",\n", "armaName", this.spawnInfo.armaName);
            mainRoot.AppendFormat("\t\t\t\t\"{0}\":\"{1}\",\n", "animName", this.spawnInfo.animName);
            mainRoot.AppendFormat("\t\t\t\t\"{0}\":{1},\n", "level", this.spawnInfo.level);
            mainRoot.AppendFormat("\t\t\t\t\"{0}\":{1:F4},\n", "critTime", this.spawnInfo.critTime);
            mainRoot.AppendFormat("\t\t\t\t\"{0}\":{1:F4},\n", "hp", this.spawnInfo.HP);
            mainRoot.AppendFormat("\t\t\t\t\"{0}\":{1:F4},\n", "frenzyHp", this.spawnInfo.frenzyHp);
            mainRoot.AppendFormat("\t\t\t\t\"{0}\":{1:F4},\n", "damage", this.spawnInfo.damage);
            mainRoot.AppendFormat("\t\t\t\t\"{0}\":{1:F4},\n", "speed", this.spawnInfo.speed);
            mainRoot.AppendFormat("\t\t\t\t\"{0}\":{1:F4},\n", "delayTime", this.spawnInfo.delayTime);
            mainRoot.AppendFormat("\t\t\t\t\"{0}\":{1:F4},\n", "posX", this.worldPosition.X);
            mainRoot.AppendFormat("\t\t\t\t\"{0}\":{1:F4},\n", "posY", this.worldPosition.Y);
            mainRoot.AppendFormat("\t\t\t\t\"{0}\":\"{1}\",\n", "actionScript", this.spawnInfo.actionScript);
            //bullets
            mainRoot.Append("\t\t\t\t\"emitters\":[");
            if (emitters != null)
            {
                for (int i = 0; i < emitters.Length; i++)
                {
                    mainRoot.AppendLine();
                    mainRoot.AppendFormat("\t\t\t\t\t\"{0}\"", emitters[i]);
                    if (i < emitters.Length - 1) mainRoot.Append(",");
                }
            }
            mainRoot.AppendLine();
            mainRoot.Append("\t\t\t\t],\n");
            //drops
            mainRoot.Append("\t\t\t\t\"drops\":[");
            {
                mainRoot.AppendLine();
                mainRoot.Append("\t\t\t\t {\n");
                mainRoot.AppendFormat("\t\t\t\t\t \"type\":{0},\n", (int)DropType.Drop_Gem_Blue_1);
                mainRoot.AppendFormat("\t\t\t\t\t \"num\":{0},\n", (int)gemBlue1.Width);
                mainRoot.AppendFormat("\t\t\t\t\t \"numVar\":{0}\n", (int)gemBlue1.Height);
                mainRoot.Append("\t\t\t\t },");

                mainRoot.AppendLine();
                mainRoot.Append("\t\t\t\t {\n");
                mainRoot.AppendFormat("\t\t\t\t\t \"type\":{0},\n", (int)DropType.Drop_Gem_Blue_2);
                mainRoot.AppendFormat("\t\t\t\t\t \"num\":{0},\n", (int)gemBlue2.Width);
                mainRoot.AppendFormat("\t\t\t\t\t \"numVar\":{0}\n", (int)gemBlue2.Height);
                mainRoot.Append("\t\t\t\t },");

                mainRoot.AppendLine();
                mainRoot.Append("\t\t\t\t {\n");
                mainRoot.AppendFormat("\t\t\t\t\t \"type\":{0},\n", (int)DropType.Drop_Gem_Yellow_1);
                mainRoot.AppendFormat("\t\t\t\t\t \"num\":{0},\n", (int)gemYellow1.Width);
                mainRoot.AppendFormat("\t\t\t\t\t \"numVar\":{0}\n", (int)gemYellow1.Height);
                mainRoot.Append("\t\t\t\t },");

                mainRoot.AppendLine();
                mainRoot.Append("\t\t\t\t {\n");
                mainRoot.AppendFormat("\t\t\t\t\t \"type\":{0},\n", (int)DropType.Drop_Gem_Yellow_2);
                mainRoot.AppendFormat("\t\t\t\t\t \"num\":{0},\n", (int)gemYellow2.Width);
                mainRoot.AppendFormat("\t\t\t\t\t \"numVar\":{0}\n", (int)gemYellow2.Height);
                mainRoot.Append("\t\t\t\t },");

                mainRoot.AppendLine();
                mainRoot.Append("\t\t\t\t {\n");
                mainRoot.AppendFormat("\t\t\t\t\t \"type\":{0},\n", (int)DropType.Drop_Gem_Green_1);
                mainRoot.AppendFormat("\t\t\t\t\t \"num\":{0},\n", (int)gemGreen1.Width);
                mainRoot.AppendFormat("\t\t\t\t\t \"numVar\":{0}\n", (int)gemGreen1.Height);
                mainRoot.Append("\t\t\t\t },");

                mainRoot.AppendLine();
                mainRoot.Append("\t\t\t\t {\n");
                mainRoot.AppendFormat("\t\t\t\t\t \"type\":{0},\n", (int)DropType.Drop_Gem_Green_2);
                mainRoot.AppendFormat("\t\t\t\t\t \"num\":{0},\n", (int)gemGreen2.Width);
                mainRoot.AppendFormat("\t\t\t\t\t \"numVar\":{0}\n", (int)gemGreen2.Height);
                mainRoot.Append("\t\t\t\t },");

                mainRoot.AppendLine();
                mainRoot.Append("\t\t\t\t {\n");
                mainRoot.AppendFormat("\t\t\t\t\t \"type\":{0},\n", (int)DropType.Drop_Gem_White);
                mainRoot.AppendFormat("\t\t\t\t\t \"num\":{0},\n", (int)diamond.Width);
                mainRoot.AppendFormat("\t\t\t\t\t \"numVar\":{0}\n", (int)diamond.Height);
                mainRoot.Append("\t\t\t\t }");

                if (PropType(this.prop) != -1)
                {
                    mainRoot.Append(",");

                    mainRoot.AppendLine();
                    mainRoot.Append("\t\t\t\t {\n");
                    mainRoot.AppendFormat("\t\t\t\t\t \"type\":{0},\n", PropType(this.prop));
                    mainRoot.AppendFormat("\t\t\t\t\t \"num\":{0},\n", 1);
                    mainRoot.AppendFormat("\t\t\t\t\t \"numVar\":{0}\n", 0);
                    mainRoot.Append("\t\t\t\t }");
                }

            }
            mainRoot.AppendLine();
            mainRoot.Append("\t\t\t\t]\n");

            //
            mainRoot.Append("\t\t\t }");
        }

        protected virtual int PropType(EProp prop)
        {
            switch (prop)
            {
                case EProp.无:
                    return -1;
                case EProp.升级:
                    return (int)DropType.Drop_Power;
                case EProp.护盾:
                    return (int)DropType.Drop_Shield;
                case EProp.技能:
                    return (int)DropType.Drop_Skill;
                default:
                    return -1;
            }
        }

        internal virtual void Draw()
        {
            Point displayPoint = new Point();
            displayPoint.X = PositionX - displaySize.Width / 2;
            displayPoint.Y = PositionY - displaySize.Height / 2;

            Point drawPoint = new Point(displayPoint.X + actorManager.WorldOrigin.X, displayPoint.Y + actorManager.WorldOrigin.Y);
            if (selected)
            {
                using (DrawingContext dc = visual.RenderOpen())
                {
                    dc.DrawImage(this.display, new System.Windows.Rect(drawPoint, this.displaySize));
                    dc.PushOpacity(0.3f);
                    dc.DrawRectangle(selectedDrawingBrush, drawingPen, new System.Windows.Rect(drawPoint, this.displaySize));
                }
            }
            else
            {
                using (DrawingContext dc = visual.RenderOpen())
                {
                    dc.DrawImage(this.display, new System.Windows.Rect(drawPoint, this.displaySize));
                }
            }
        }
    }
}
