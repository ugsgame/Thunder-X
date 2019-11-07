using LitJson;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Thunder.GameLogic.Gaming;
using Thunder.GameLogic.Gaming.Actors;
using Thunder.GameLogic.Gaming.Actors.Drops;
using ThunderEditor.Utils;

namespace ThunderEditor.Logic
{
    /// <summary>
    /// 关卡事件
    /// </summary>
    public class Event
    {
        private Level level;

        private EActorManager actorManager;

        private EWorld world;

        string discribe = "事件";
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

        string name;
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

        bool crossCondition;
        public bool 是否为时间过关
        {
            get { return crossCondition; }
            set { crossCondition = value; }
        }

        float crossTime = 10.0f;
        public float 过关时间
        {
            get { return crossTime; }
            set { crossTime = value; }
        }

        float waitingTime;
        public float 开场时间
        {
            get { return waitingTime; }
            set { waitingTime = value; }
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

        internal EActor AddActor(EActorType actorInfo)
        {
            return this.actorManager.AddActor(actorInfo);
        }

        internal void AddActor(EActor actor)
        {
            this.actorManager.AddActor(actor);
        }

        internal EActor AddActor(JsonData actorData)
        {
            EActor actor = null;
            try
            {
                
                string describe = (string)actorData["describe"];
                int actorId = (int)actorData["actorId"];
                int spawnType = (int)actorData["spawnType"];
                string armaName = (string)actorData["armaName"];
                string animName = (string)actorData["animName"];
                string name = (string)actorData["name"];
                int level = (int)actorData["level"];
                double critTime = (double)actorData["critTime"];
                double hp = (double)actorData["hp"];
                double frenzyHp = (double)actorData["frenzyHp"];
                double damage = (double)actorData["damage"];
                double speed = (double)actorData["speed"];
                double delayTime = (double)actorData["delayTime"]; 
                double posX = (double)actorData["posX"];
                double posY = (double)actorData["posY"];
                string actionScript = (string)actorData["actionScript"];
                JsonData emitters = (JsonData)actorData["emitters"];
                JsonData drops = (JsonData)actorData["drops"];
                /*
                string describe = (string)actorData["describe"];
                int actorId = (int)FileSever.JsonNumber(actorData, "actorId");
                int spawnType = (int)FileSever.JsonNumber(actorData,"spawnType");
                string armaName = (string)actorData["armaName"];
                string animName = (string)actorData["animName"];
                string name = (string)actorData["name"];
                int level = (int)FileSever.JsonNumber(actorData, "level");
                float critTime = FileSever.JsonNumber(actorData,"critTime");             
				float hp = FileSever.JsonNumber(actorData,"hp");
			    float frenzyHp = FileSever.JsonNumber(actorData,"frenzyHp");
                float damage = FileSever.JsonNumber(actorData,"damage");
				float	speed = FileSever.JsonNumber(actorData,"speed");
                float delayTime = FileSever.JsonNumber(actorData,"delayTime");
				float	posX  = FileSever.JsonNumber(actorData,"posX");
				float	posY  = FileSever.JsonNumber(actorData,"posY");
                string actionScript = (string)actorData["actionScript"];
                JsonData emitters = (JsonData)actorData["emitters"];
                */
                EActorType actorType = EActorBrowse.Instanse.ActorTypes[(ActorID)actorId];

                if (actorType != null)
                {
                    actorType.SpawnInfo.spawnType = (SpawnType)spawnType;
                    actorType.SpawnInfo.armaName = armaName;
                    actorType.SpawnInfo.animName = animName;
                    actorType.SpawnInfo.describe = describe;
                    actorType.SpawnInfo.name = name;
                    actorType.SpawnInfo.level = level;
                    actorType.SpawnInfo.critTime = (float)critTime;
                    actorType.SpawnInfo.HP = (float)hp;
                    actorType.SpawnInfo.frenzyHp = (float)frenzyHp;
                    actorType.SpawnInfo.damage = (float)damage;
                    actorType.SpawnInfo.speed = (float)speed;
                    actorType.SpawnInfo.delayTime = (float)delayTime;
                    actorType.SpawnInfo.position.X = (float)posX;
                    actorType.SpawnInfo.position.Y = (float)posY;
                    actorType.SpawnInfo.actionScript = actionScript;

                    actor = this.actorManager.AddActor(actorType);
                    actor.WorldPosition = actorType.SpawnInfo.position;
                    actor.Position = this.world.ToCanvasPosition(actorType.SpawnInfo.position);

                    EActor.Emitter[] emtrs = new EActor.Emitter[emitters.Count];
                    for (int i = 0; i < emitters.Count; i++)
                    {
                        emtrs[i] = new EActor.Emitter((string)emitters[i]);
                    }
                    actor.发射器 = emtrs;

                    //掉落
                    foreach (var item in drops)
                    {
                        JsonData dropTable = (JsonData)item;
                        DropType type = (DropType)(int)dropTable["type"];
                        int num = (int)dropTable["num"];
                        int numVar = (int)dropTable["numVar"];

                        switch (type)
                        {
                            case DropType.Drop_Gem_Blue_1:
                                actor.蓝宝石小 = new Size((double)num,(double)numVar);
                                break;
                            case DropType.Drop_Gem_Blue_2:
                                actor.蓝宝石大 = new Size((double)num, (double)numVar);
                                break;
                            case DropType.Drop_Gem_Yellow_1:
                                actor.黄宝石小 = new Size((double)num, (double)numVar);
                                break;
                            case DropType.Drop_Gem_Yellow_2:
                                actor.黄宝石大 = new Size((double)num, (double)numVar);
                                break;
                            case DropType.Drop_Gem_Green_1:
                                actor.绿宝石小 = new Size((double)num, (double)numVar);
                                break;
                            case DropType.Drop_Gem_Green_2:
                                actor.绿宝石大 = new Size((double)num, (double)numVar);
                                break;
                            case DropType.Drop_Gem_White:
                                actor.钻石 = new Size((double)num, (double)numVar);
                                break;
                            case DropType.Drop_Power:
                                actor.道具 = EActor.EProp.升级;
                                break;
                            case DropType.Drop_Shield:
                                actor.道具 = EActor.EProp.护盾;
                                break;
                            default:
                                break;
                        }
                    }
                    //

                    //Console.WriteLine(this.world.ToWorldPosition(actor.Position));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return actor;
        }



        internal void RemoveActor(EActor actor)
        {
            this.actorManager.RemoveActor(actor);
        }

        internal EActor GetActor(int index)
        {
            return actorManager.GetActor(index);
        }

        internal EActor GetActor(Visual visual)
        {
            return actorManager.GetActor(visual);
        }

        internal EActorManager ActorManager
        {
            get { return actorManager; }
        }

        internal EWorld World
        {
            get { return world; }
        }

        public Event(Level level)
        {
            this.level = level;

            actorManager = new EActorManager();
            world = new EWorld(actorManager);
            InitWorld();
            actorManager.World = world;
        }

        public Event(Level level, string name)
        {
            this.level = level;
            this.name = name;

            actorManager = new EActorManager();
            world = new EWorld(actorManager);
            InitWorld();
            actorManager.World = world;
        }

        private void InitWorld()
        {
            //world.PositionX = (float)MainWindow.sceneInstanse.ActualWidth / 2;
            //world.PositionY = (float)MainWindow.sceneInstanse.ActualHeight / 2;
        }

        public virtual void UnserializeJson(ref StringBuilder mainRoot)
        {
            mainRoot.Append("\t\t{");
            mainRoot.AppendLine();
            mainRoot.AppendFormat("\t\t\"{0}\":\"{1}\",\n", "describe", this.discribe);
            mainRoot.AppendFormat("\t\t\"{0}\":\"{1}\",\n", "name", this.name);
            mainRoot.AppendFormat("\t\t\"{0}\":{1},\n", "crossCondition", this.crossCondition.ToString().ToLower());
            mainRoot.AppendFormat("\t\t\"{0}\":{1:F4},\n", "crossTime", this.crossTime);
            mainRoot.AppendFormat("\t\t\"{0}\":{1:F4},\n", "waitingTime", this.waitingTime);
            mainRoot.AppendFormat("\t\t\"{0}\":{1:F4},\n", "wOriginX", (float)this.actorManager.WorldOrigin.X);
            mainRoot.AppendFormat("\t\t\"{0}\":{1:F4},\n", "wOriginY", (float)this.actorManager.WorldOrigin.Y);
            mainRoot.Append("\t\t\"actors\":[");
            for (int i = 0; i < this.actorManager.Count; i++)
            {
                List<EActor> actors = this.actorManager.GetActors();
                mainRoot.AppendLine();
                actors[i].UnserializeJson(ref mainRoot);
                if (i < this.actorManager.Count - 1) mainRoot.Append(",");
            }
            mainRoot.AppendLine();
            mainRoot.Append("\t\t\t]\n");
            mainRoot.Append("\t\t}");
        }
    }
}
