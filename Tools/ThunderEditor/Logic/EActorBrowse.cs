using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Thunder.GameLogic.Gaming;
using Thunder.GameLogic.Gaming.Actors;
using ThunderEditor.Editor;

namespace ThunderEditor.Logic
{
    /// <summary>
    /// 角色类型
    /// </summary>
    public class EActorBrowse
    {

        public static string ActorResPath = "pack://SiteOfOrigin:,,,/" + EConfig.Instance.ResPath;

        Dictionary<ActorID, EActorType> actorTypes = new Dictionary<ActorID, EActorType>(8);

        public static EActorBrowse Instanse = new EActorBrowse();

        public EActorBrowse()
        {

        }

        public bool AddActorType(EActorType actorType)
        {
            actorTypes[actorType.SpawnInfo.actorId] = actorType;
            return true;
        }

        public bool AddActorType(JsonData actorData)
        {
            EActorType actorType = new EActorType();

            try
            {
                actorType.SpawnInfo.spawnType = (SpawnType)(int)actorData["spawnType"];
                actorType.SpawnInfo.actorId = (ActorID)(int)actorData["actorType"];
                actorType.SpawnInfo.name = (string)actorData["name"];
                actorType.SpawnInfo.armaName = (string)actorData["armaName"]; ;
                actorType.SpawnInfo.animName = (string)actorData["animName"]; ;
                actorType.SpawnInfo.level = (int)actorData["level"];
                actorType.SpawnInfo.critTime = (float)(double)actorData["critTime"];
                actorType.SpawnInfo.HP = (float)(double)actorData["hp"];
                actorType.SpawnInfo.frenzyHp = (float)(double)actorData["frenzyHp"];
                actorType.SpawnInfo.damage = (float)(double)actorData["damage"];
                actorType.SpawnInfo.speed = (float)(double)actorData["speed"];
                actorType.SpawnInfo.delayTime = (float)(double)actorData["delayTime"];

                actorType.DisplayRes = (string)actorData["displayRes"];

                try
                {
                    actorType.Display = new BitmapImage(new Uri(ActorResPath + actorType.DisplayRes, UriKind.RelativeOrAbsolute));
                }
                catch (Exception e)
                {
                    //使用默认图片
                    actorType.Display = new BitmapImage(new Uri(@"pack://application:,,,/Res/EditorIcons/character.bmp", UriKind.Absolute));
                    Console.WriteLine("找不到指定图片！！");
                    Console.WriteLine(e);
                }
                //不能有重复的id~~
                actorTypes[actorType.SpawnInfo.actorId] = actorType;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            return true;
        }

        public bool AddActorType(JsonData actorData, out EActorType actor)
        {
            AddActorType(actorData);
            actor = actorTypes[(ActorID)(int)actorData["actorType"]];
            return (actor != null);
        }

        public bool AddActorType(JsonData actorData, out EActorType actor,BrowseType bType)
        {
            AddActorType(actorData);
            actor = actorTypes[(ActorID)(int)actorData["actorType"]];
            actor.BType = bType;
            return (actor != null);
        }
        public bool LoadConfig()
        {
            return true;
        }

        public bool InitActorType()
        {
            return true;
        }

        public Dictionary<ActorID, EActorType> ActorTypes
        {
            get { return actorTypes; }
        }

        public int Count
        {
            get { return actorTypes.Count; }
        }

        public EActorType GetActorInfo(ActorID actorid)
        {
            return actorTypes[actorid];
        }
    }
}
