using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Media.Imaging;
using Thunder.GameLogic.Gaming;
using Thunder.GameLogic.Gaming.Actors;

namespace ThunderEditor.Logic
{
    public class EActorType
    {
        SpawnInfo spawnInfo = new SpawnInfo();

        private BrowseType browseType;
        internal BrowseType BType
        {
            set { browseType = value; }
            get { return browseType; }
        }

        public SpawnType 类型
        {
            set { spawnInfo.spawnType = value; }
            get { return spawnInfo.spawnType; }
        }

        public ActorID 角色ID
        {
            set { spawnInfo.actorId = value; }
            get { return spawnInfo.actorId; }
        }

        public string 名字
        {
            set{ spawnInfo.name = value; }
            get{ return spawnInfo.name; }
        }

        
        internal SpawnInfo SpawnInfo
        {
            get { return spawnInfo; }
        }

        private string displayRes;
        public string DisplayRes
        {
            set { displayRes = value; }
            get { return displayRes; }
        }

        private BitmapImage display;
        internal BitmapImage Display
        {
            set { display = value; }
            get { return display; }
        }
    
        private object bindingControl;
        internal object BindingControl
        {
            set { bindingControl = value; }
            get { return bindingControl; }
        }
        internal void CleanBinding()
        {
            bindingControl = null;
        }
    }
}
