using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LitJson;

using MatrixEngine.Cocos2d;
using Thunder.Game;
using Thunder.GameLogic.Gaming.Actors;
using MatrixEngine.Math;
using Thunder.GameLogic.Common;
using Thunder.Common;
using Thunder.GameLogic.Gaming.Actors.Drops;
using Thunder.GameLogic.UI;
using Thunder.GameLogic.UI.Guide;

namespace Thunder.GameLogic.Gaming
{
    /// <summary>
    /// 关卡管理器
    /// </summary>
    public class LevelManager : InterfaceGameState
    {
        /// <summary>
        /// 关卡数据文件
        /// </summary>
        string LevelFilePath;

        /// <summary>
        /// 关卡主节点
        /// </summary>
        JsonData mainRoot;

        /// <summary>
        /// 事件缓存
        /// </summary>
        List<EventSpawner> eventCache = new List<EventSpawner>();
        List<EventInfo> eventInfoCache = new List<EventInfo>();

        public EventSpawner CurrentEvent
        {
            get;
            protected set;
        }
        /// <summary>
        /// 游戏图层
        /// </summary>
        GameLayer gameLayer;
        public GameLayer GameLayer
        {
            set { gameLayer = value; }
            get { return gameLayer; }
        }

        GameMap gameMap;
        public GameMap GameMap
        {
            set { gameMap = value; }
            get { return gameMap; }
        }

        PlayingScene playingLayer;
        public PlayingScene PlayingLayer
        {
            set { playingLayer = value; }
            get { return playingLayer; }
        }


        public void Dispose()
        {
            foreach (var item in eventCache)
            {
                if (item != null)
                {
                    item.RemoveFromParent();
                    item.Dispose();
                }
                else
                {
                    Console.WriteLine("eventCache:" + eventCache.Count);
                }
            }
            eventCache.Clear();
            eventInfoCache.Clear();

            if (EDebug.swMap)
            {
                gameMap.UnloadMap();
            }
        }

        public LevelManager(GameLayer gameLayer)
        {
            this.GameLayer = gameLayer;
        }

        public LevelManager(GameLayer gameLayer, string levelConfigFile)
        {
            this.GameLayer = gameLayer;
            SetLevel(levelConfigFile);
        }

        public void SetLevel(string levelFile)
        {
            LevelFilePath = Utils.CoverLevelPath(levelFile);

            try
            {
                string data = CCFileUtils.GetFileDataToString(LevelFilePath);
                mainRoot = JsonMapper.ToObject(data);
            }
            catch (Exception e)
            {
                Utils.PrintException(e);
            }
        }

        /************************************************************/
        /*解释关卡数据**/
        /// <summary>
        /// 解释关卡配置
        /// TODO:返回加载进度
        /// </summary>
        /// <param name="levelConfig"></param>
        protected void ParseLevel(JsonData levelConfig, ref List<EventInfo> _eventInfoCache)
        {
            string map_name = (string)levelConfig["map_name"];
            int map_layer = (int)levelConfig["map_layer"];
            double map_speed = (double)levelConfig["map_speed"];
            double map_speedRate = (double)levelConfig["map_speedRate"];
            JsonData event_data = (JsonData)levelConfig["event_data"];

            if (EDebug.swMap)
            {
                GameMap.layerCount = map_layer;

                gameMap.MoveSpeed = (float)map_speed;
                gameMap.MoveRate = (float)map_speedRate;
                gameMap.LoadMap(map_name);
            }

            //             Console.WriteLine("map_name:" + map_name);
            //             Console.WriteLine("map_layer:" + map_layer);
            //             Console.WriteLine("map_speed:" + map_speed);
            //             Console.WriteLine("map_speedRate:" + map_speedRate);
            //             Console.WriteLine("actorCount:" + event_data.Count);

            for (int i = 0; i < event_data.Count; i++)
            {
                EventInfo eventInfo = new EventInfo();
                ParseEventSpawner(event_data[i], ref eventInfo);
                _eventInfoCache.Add(eventInfo);
            }


        }
        /// <summary>
        /// 解释事件
        /// TODO:返回加载进度
        /// </summary>
        /// <param name="spawnerConfig"></param>
        protected void ParseEventSpawner(JsonData spawnerConfig, ref EventInfo eventInfo)
        {
            string describe = (string)spawnerConfig["describe"];
            string name = (string)spawnerConfig["name"];
            bool crossCondition = (bool)spawnerConfig["crossCondition"];
            double crossTime = (double)spawnerConfig["crossTime"];
            double waitingTime = (double)spawnerConfig["waitingTime"];
            JsonData actors = (JsonData)spawnerConfig["actors"];

            eventInfo.describe = describe;
            eventInfo.name = name;
            eventInfo.crossCondition = crossCondition;
            eventInfo.crossTime = (float)crossTime;
            eventInfo.waitingTime = (float)waitingTime;
            eventInfo.spawnInfoCache = new List<SpawnInfo>(4);

            foreach (JsonData actor in actors)
            {
                SpawnInfo spawnInfo = new SpawnInfo();
                if (ParseActor(actor, ref spawnInfo))
                {
                    eventInfo.spawnInfoCache.Add(spawnInfo);
                }
            }

            //             Console.WriteLine("crossCondition:" + crossCondition);
            //             Console.WriteLine("crossTime:" + crossTime);
            //             Console.WriteLine("waitingTime:" + waitingTime);
            //             Console.WriteLine("actorsCount:" + actors.Count);
        }
        /// <summary>
        /// 解释角色
        /// TODO:返回加载进度
        /// </summary>
        /// <param name="actorConfig"></param>
        protected bool ParseActor(JsonData actorConfig, ref SpawnInfo spawnInfo)
        {
            int bType = (int)actorConfig["browseType"];
            if (bType > 0) return false;

            int actorId = (int)actorConfig["actorId"];
            int spawnType = (int)actorConfig["spawnType"];
            string armaName = (string)actorConfig["armaName"];
            string animName = (string)actorConfig["animName"];
            string name = (string)actorConfig["name"];
            int level = (int)actorConfig["level"];
            double critTime = (double)actorConfig["critTime"];
            double hp = (double)actorConfig["hp"];
            double frenzyHp = (double)actorConfig["frenzyHp"];
            double damage = (double)actorConfig["damage"];
            double speed = (double)actorConfig["speed"];
            double delay = (double)actorConfig["delayTime"];
            double posX = (double)actorConfig["posX"];
            double posY = (double)actorConfig["posY"];
            string actionScript = (string)actorConfig["actionScript"];
            JsonData emitters = (JsonData)actorConfig["emitters"];
            JsonData drops = (JsonData)actorConfig["drops"];

            spawnInfo.actorId = (ActorID)actorId;
            spawnInfo.spawnType = (SpawnType)spawnType;
            spawnInfo.armaName = armaName;
            spawnInfo.animName = animName;
            spawnInfo.name = name;
            spawnInfo.level = level;
            spawnInfo.critTime = (float)critTime;
            spawnInfo.HP = (float)hp;
            spawnInfo.totalHp = (float)hp;
            spawnInfo.frenzyHp = (float)frenzyHp;
            spawnInfo.damage = (float)damage;
            spawnInfo.speed = (float)speed;
            spawnInfo.delayTime = (float)delay;
            spawnInfo.position = new Vector2((float)posX, (float)posY);

            string scriptPath = Utils.CoverActionScriptPath(actionScript);
            //Console.WriteLine("ScriptFile:" + scriptPath);
            if (CCFileUtils.IsFileExist(scriptPath))
            {
                spawnInfo.actionScript = CCFileUtils.GetFileDataToString(scriptPath);
            }
            else
            {
                spawnInfo.actionScript = "";
            }


            switch (spawnInfo.spawnType)
            {
                case SpawnType.Actor_Player:
                    spawnInfo.resPath = ResID.Armatures_Player;
                    spawnInfo.filter = FilterType.FilterPlayer;
                    break;
                case SpawnType.Actor_Boss:
                    {
                        spawnInfo.filter = FilterType.FilterEnemy;
                        switch (spawnInfo.actorId)
                        {
                            case ActorID.Boss1:
                                spawnInfo.resPath = ResID.Armatures_Boss1;
                                break;
                            case ActorID.Boss2:
                                spawnInfo.resPath = ResID.Armatures_Boss2;
                                break;
                            case ActorID.Boss3:
                                spawnInfo.resPath = ResID.Armatures_Boss3;
                                break;
                            case ActorID.Boss4:
                                spawnInfo.resPath = ResID.Armatures_Boss4;
                                break;
                            case ActorID.Boss5:
                                spawnInfo.resPath = ResID.Armatures_Boss5;
                                break;
                            case ActorID.Boss6:
                                spawnInfo.resPath = ResID.Armatures_Boss6;
                                break;
                            case ActorID.Boss7:
                                spawnInfo.resPath = ResID.Armatures_Boss7;
                                break;
                            case ActorID.Boss8:
                                spawnInfo.resPath = ResID.Armatures_Boss8;
                                break;
                            case ActorID.Boss9:
                                spawnInfo.resPath = ResID.Armatures_Boss9;
                                break;
                            case ActorID.Boss10:
                                spawnInfo.resPath = ResID.Armatures_Boss10;
                                break;
                            case ActorID.Boss11:
                                spawnInfo.resPath = ResID.Armatures_Boss11;
                                break;
                            case ActorID.Boss12:
                                spawnInfo.resPath = ResID.Armatures_Boss12;
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                case SpawnType.Actor_Emeny:
                    spawnInfo.resPath = ResID.Armatures_Enemy;
                    spawnInfo.filter = FilterType.FilterEnemy;
                    break;
                case SpawnType.Actor_Drop:
                    spawnInfo.resPath = ResID.Armatures_Objects;
                    spawnInfo.filter = FilterType.AllFilter;
                    break;
                default:
                    break;
            }

            //             Console.WriteLine("actorId:" + actorId);
            //             Console.WriteLine("spawnType:" + spawnType);
            //             Console.WriteLine("name:" + name);
            //             Console.WriteLine("level:" + level);
            //             Console.WriteLine("critTime:" + critTime);
            //             Console.WriteLine("hp:" + hp);
            //             Console.WriteLine("frenzyHp:" + frenzyHp);
            //             Console.WriteLine("speed:" + speed);
            //             Console.WriteLine("posX:" + posX);
            //             Console.WriteLine("posY:" + posY);
            //             Console.WriteLine("actionScript:" + actionScript);

            int emitterCount = emitters.Count;
            //不能超过规定的发射点个数
            if (emitterCount > (int)Actor.EmitPoint.Count)
            {
                emitterCount = (int)Actor.EmitPoint.Count;
            }
            for (int i = 0; i < emitterCount; i++)
            {
                string bt = (string)emitters[i];
                //Console.WriteLine(bt);
                spawnInfo.Emitters.Add(bt);
            }

            //掉落
            spawnInfo.drops = new DropHelper();
            foreach (var item in drops)
            {
                JsonData dropTable = (JsonData)item;
                spawnInfo.drops.Add((DropType)(int)dropTable["type"], (int)dropTable["num"], (int)dropTable["numVar"]);
            }

            return true;
        }
        /// <summary>
        /// 解释子弹
        /// TODO:返回加载进度
        /// </summary>
        /// <param name="actorConfig"></param>
        protected void ParseBullet(JsonData bulletConfig)
        {

        }
        /***********************************************************/
        /// <summary>
        /// 是否是最后一关
        /// </summary>
        /// <returns></returns>
        public bool IsLastEvent(EventSpawner _event)
        {
            return (eventCache.Count - 1 == eventCache.IndexOf(_event));
        }

        /// <summary>
        /// 当关卡事件结事回调
        /// </summary>
        /// <param name="_event"></param>
        public void OnEventOver(EventSpawner _event)
        {
            if (eventCache.Count != 0 && eventCache.Contains(_event))
            {
                int index = eventCache.IndexOf(_event);
                //释放当前事件
                _event.RemoveFromParent(true);
                //可能引发性能消耗
                //System.GC.Collect();
                //播放下一事件
                if (index == eventCache.Count - 1)
                {
                    //说明所有事件已播放完毕，过关处理
                    this.OnEnd();
                }
                else if (EDebug.InEditor && EDebug.Mode == EDebug.DebugMode.Debug_Event)
                {
                    //测试完当前事件后直接结束
                    this.OnEnd();
                }
                else
                {
                    CurrentEvent = eventCache[index + 1];
                    CurrentEvent.Play();

                    //触发使用护盾引导
                    if (index + 1 == 2)
                    {
                        if (!GuideWindow.Instance.GetGuideData(GuideCommand.使用护盾).IsPlay)
                        {
                            CurrentEvent.RunSequenceActions(new CCActionDelayTime(3.0f), new CCActionCallFunc(this.ShowShieldGuide));
                        }
                    }
                    if (index + 1 == 5)
                    {
                        if (!GuideWindow.Instance.GetGuideData(GuideCommand.使用技能).IsPlay)
                        {
                            CurrentEvent.RunSequenceActions(new CCActionDelayTime(3.0f), new CCActionCallFunc(this.ShowSkillGuide));
                        }
                    }

                }

            }
        }

        private void ShowShieldGuide()
        {
            Console.WriteLine("ShowShieldGuide");
            GuideWindow.Instance.Command = GuideCommand.使用护盾;
            GuideWindow.Instance.Show = true;
        }

        private void ShowSkillGuide()
        {
            Console.WriteLine("ShowSkillGuide");
            GuideWindow.Instance.Command = GuideCommand.使用技能;
            GuideWindow.Instance.Show = true;
        }

        /// <summary>
        /// 加载关卡数据
        /// TODO：反回加载进度
        /// </summary>
        /// <param name="levelFile"></param>
        /// <returns></returns>
        public bool LoadLevel()
        {
            if (mainRoot == null)
            {
                return false;
            }
            else
            {
                //List<EventInfo> eventInfoCache = new List<EventInfo>(4);
                ParseLevel(mainRoot, ref eventInfoCache);
                foreach (var item in eventInfoCache)
                {
                    EventSpawner eventSpawner = new EventSpawner(item, this);
                    eventCache.Add(eventSpawner);
                    this.playingLayer.AddChild(eventSpawner);
                }
            }
            return true;
        }

        public IEnumerable<LoadingScene.Percent> LoadLevel(LoadingScene.PercentCounter percentCounter)
        {
            //List<EventInfo> eventInfoCache = new List<EventInfo>(4);
            ParseLevel(mainRoot, ref eventInfoCache);
            int i = 0;
            foreach (var item in eventInfoCache)
            {
                EventSpawner eventSpawner = new EventSpawner(item, this);
                eventCache.Add(eventSpawner);
                this.playingLayer.AddChild(eventSpawner);
                yield return percentCounter.NextPercent(i, eventInfoCache.Count);
                i++;
            }
        }

        /// <summary>
        /// 全屏子弹变宝
        /// </summary>
        public void ScreenBulletToGems(DropType droptype = DropType.Drop_Gem_Blue_2)
        {
            for (int i = 0; i < eventCache.IndexOf(CurrentEvent) + 1; i++)
            {
                eventCache[i].TansToGems(droptype);
            }
        }
        /// <summary>
        /// 全屏敌人清杀
        /// </summary>
        public void ScreenEnemyKilled(float damage)
        {
            for (int i = 0; i < eventCache.IndexOf(CurrentEvent) + 1; i++)
            {
                eventCache[i].KillAllEnemys(damage);
            }
        }

        public virtual void OnPause()
        {
            for (int i = 0; i < eventCache.IndexOf(CurrentEvent) + 1; i++)
            {
                eventCache[i].OnPause();
            }
        }

        public virtual void OnResume()
        {
            for (int i = 0; i < eventCache.IndexOf(CurrentEvent) + 1; i++)
            {
                eventCache[i].OnResume();
            }
        }

        public bool Start()
        {
            if (eventCache.Count >= 1)
            {
                if (EDebug.InEditor && (EDebug.Mode == EDebug.DebugMode.Debug_Event))
                {
                    foreach (var item in eventCache)
                    {
                        if (item.Name == EDebug.DebugEvent)
                        {
                            CurrentEvent = item;
                            break;
                        }
                    }
                }
                else
                {
                    CurrentEvent = eventCache[0];
                }

                CurrentEvent.Play();
            }
            return true;
        }

        /// <summary>
        /// 关卡结束回调处理
        /// 进入打分关什么的~~
        /// </summary>
        protected void OnEnd()
        {
            Console.WriteLine("过关!!!!!!!!!");
            //TODO:
            if (GameData.Instance.CurLevelIndex < GameData.Instance.LevelCount)
            {
                GameData.Instance.CurLevelIndex = GameData.Instance.CurLevelIndex + 1;
                LevelData level = GameData.Instance.GetLevelData(GameData.Instance.CurLevelIndex);
                level.isOpen = true;
                if (GameData.Instance.CurLevelIndex == 3 && !GameData.Instance.PlayerData.withWingman)
                {
                    //开启僚机功能
                    GameData.Instance.PlayerData.withWingman = true;
                    GameData.Instance.PlayerData.curWingman = WingmanSpawner.WingmanID.Wingman1;
                }
            }
            else
            {
                GameData.Instance.CurLevelIndex = 1;
            }
            //Function.GoTo(UIFunction.游戏胜利);
            PlayingScene.Instance.PlayFightWin();
        }
    }
}
