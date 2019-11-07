
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LitJson;
using Thunder.GameLogic.Gaming.Actors.Players;
using Thunder.GameLogic.Gaming;
using MatrixEngine.CocoStudio.Armature;
using Thunder.GameLogic.Gaming.Actors;
using Thunder.GameLogic.UI;
using MatrixEngine.Cocos2d;
using Thunder.Game;
using Thunder.Common;
using System.IO;

namespace Thunder.GameLogic.Common
{
    /// <summary>
    /// 引导数据
    /// </summary>
    public class GuideData
    {
        public bool 开始游戏;
        public bool 战机选择;
        public bool 关卡选择;
        public bool 使用护盾;
        public bool 使用技能;
        public bool 跳到升级;
        public bool 点击升级;
        public bool 升级返回;

        public void SerializeJson(JsonData guideData)
        {
            try
            {
                开始游戏 = (bool)guideData["开始游戏"];
                战机选择 = (bool)guideData["战机选择"];
                关卡选择 = (bool)guideData["关卡选择"];
                使用护盾 = (bool)guideData["使用护盾"];
                使用技能 = (bool)guideData["使用技能"];
                跳到升级 = (bool)guideData["跳到升级"];
                点击升级 = (bool)guideData["点击升级"];
                升级返回 = (bool)guideData["升级返回"];
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        public void UnserializeJson(ref StringBuilder guideData)
        {
            guideData.Append("\t\"guideData\": {");
            guideData.AppendLine();

            guideData.AppendFormat("\t\t\t\"{0}\":{1},\n", "开始游戏", this.开始游戏.ToString().ToLower());
            guideData.AppendFormat("\t\t\t\"{0}\":{1},\n", "战机选择", this.战机选择.ToString().ToLower());
            guideData.AppendFormat("\t\t\t\"{0}\":{1},\n", "关卡选择", this.关卡选择.ToString().ToLower());
            guideData.AppendFormat("\t\t\t\"{0}\":{1},\n", "使用护盾", this.使用护盾.ToString().ToLower());
            guideData.AppendFormat("\t\t\t\"{0}\":{1},\n", "使用技能", this.使用技能.ToString().ToLower());
            guideData.AppendFormat("\t\t\t\"{0}\":{1},\n", "跳到升级", this.跳到升级.ToString().ToLower());
            guideData.AppendFormat("\t\t\t\"{0}\":{1},\n", "点击升级", this.点击升级.ToString().ToLower());
            guideData.AppendFormat("\t\t\t\"{0}\":{1}\n", "升级返回", this.升级返回.ToString().ToLower());
            
            guideData.Append("\t}");
        }
    }
    /// <summary>
    /// 关卡数据
    /// </summary>
    public /*struct*/class LevelData
    {
        public int levelIndex;
        public string levelName;
        public bool isOpen;
        //boss相关
        public string bossArmaturePath;
        public string bossArmatureName;
        public string bossAnimationName;
        public string bossName;

        public CCArmature armatureData;
        public MatrixEngine.CocoStudio.Armature.CCAnimation animationData;

        public int bossPowerAttack = 1; //攻击
        public int bossPowerDef = 1;    //防御
        public int bossPowerSpeed = 1;  //速度
        //
        public Object userData;

        public void SerializeJson(JsonData levelData)
        {
            try
            {
                levelIndex = (int)levelData["levelIndex"];
                levelName = (string)levelData["levelName"];
                isOpen = (bool)levelData["isOpen"];
                bossArmaturePath = (string)levelData["bossArmaturePath"];
                bossArmatureName = (string)levelData["bossArmatureName"];
                bossAnimationName = (string)levelData["bossAnimationName"];
                bossName = (string)levelData["bossName"];
                bossPowerAttack = (int)levelData["bossPowerAttack"];
                bossPowerDef = (int)levelData["bossPowerDef"];
                bossPowerSpeed = (int)levelData["bossPowerSpeed"];

                if (EDebug.dLevel)
                {
                    isOpen = true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        public void UnserializeJson(ref StringBuilder levelData)
        {
            levelData.Append("\t\t{");
            levelData.AppendLine();
            levelData.AppendFormat("\t\t\t\"{0}\":{1},\n", "levelIndex", (int)this.levelIndex);
            levelData.AppendFormat("\t\t\t\"{0}\":\"{1}\",\n", "levelName", this.levelName);
            levelData.AppendFormat("\t\t\t\"{0}\":{1},\n", "isOpen", this.isOpen.ToString().ToLower());
            levelData.AppendFormat("\t\t\t\"{0}\":\"{1}\",\n", "bossArmaturePath", this.bossArmaturePath);
            levelData.AppendFormat("\t\t\t\"{0}\":\"{1}\",\n", "bossArmatureName", this.bossArmatureName);
            levelData.AppendFormat("\t\t\t\"{0}\":\"{1}\",\n", "bossAnimationName", this.bossAnimationName);
            levelData.AppendFormat("\t\t\t\"{0}\":\"{1}\",\n", "bossName", this.bossName);
            levelData.AppendFormat("\t\t\t\"{0}\":{1},\n", "bossPowerAttack", this.bossPowerAttack);
            levelData.AppendFormat("\t\t\t\"{0}\":{1},\n", "bossPowerDef", this.bossPowerDef);
            levelData.AppendFormat("\t\t\t\"{0}\":{1}\n", "bossPowerSpeed", this.bossPowerSpeed);
            levelData.Append("\t\t}");
        }
    }
    /// <summary>
    /// 战机数据
    /// </summary>
    public /*struct*/ class FighterData
    {
        public class FighterLevelData
        {
            public int level;                      //等级
            public int unlockGolds;                //下等级升级花费
            public int attachPlayerDamage;         //叠加主机攻击火力
            public int attachWingmanDamage;        //叠加僚机攻击火力
            public int attachHP;                   //叠加血量
            public int attachSkillDamage;          //叠加必杀伤害
            public int attachShieldCount;          //进入游戏时送的护盾数
            public int attachCritTime;             //叠加暴走时间
            public int attachGolds;                //金币加成
            public int attachScore;                //分数加成

            public void SerializeJson(JsonData data)
            {
                level = (int)data["level"];
                unlockGolds = (int)data["unlockGolds"];
                attachPlayerDamage = (int)data["attachPlayerDamage"];
                attachWingmanDamage = (int)data["attachWingmanDamage"];
                attachHP = (int)data["attachHP"];
                attachSkillDamage = (int)data["attachSkillDamage"];
                attachShieldCount = (int)data["attachShieldCount"];
                attachCritTime = (int)data["attachCritTime"];
                attachGolds = (int)data["attachGolds"];
                attachScore = (int)data["attachScore"];
            }

            public void UnserializeJson(ref StringBuilder levelData)
            {
                levelData.AppendLine();
                levelData.Append("\t\t\t\t{");
                levelData.AppendLine();
                levelData.AppendFormat("\t\t\t\t\t\"{0}\":{1},\n", "level", level);
                levelData.AppendFormat("\t\t\t\t\t\"{0}\":{1},\n", "unlockGolds", unlockGolds);
                levelData.AppendFormat("\t\t\t\t\t\"{0}\":{1},\n", "attachPlayerDamage", attachPlayerDamage);
                levelData.AppendFormat("\t\t\t\t\t\"{0}\":{1},\n", "attachWingmanDamage", attachWingmanDamage);
                levelData.AppendFormat("\t\t\t\t\t\"{0}\":{1},\n", "attachHP", attachHP);
                levelData.AppendFormat("\t\t\t\t\t\"{0}\":{1},\n", "attachSkillDamage", attachSkillDamage);
                levelData.AppendFormat("\t\t\t\t\t\"{0}\":{1},\n", "attachShieldCount", attachShieldCount);
                levelData.AppendFormat("\t\t\t\t\t\"{0}\":{1},\n", "attachCritTime", attachCritTime);
                levelData.AppendFormat("\t\t\t\t\t\"{0}\":{1},\n", "attachGolds", attachGolds);
                levelData.AppendFormat("\t\t\t\t\t\"{0}\":{1}\n", "attachScore", attachScore);
                levelData.Append("\t\t\t\t}");
            }
        }

        public PlayerSpawner.PlayerID id;
        public bool isOpen;
        //等级属性
        public int topLevel = 5;
        public int curLevel = 1;          //等级

        public int hp = 100;                //战机血量
        public int damage = 10;             //攻击伤害值
        public int unlockGolds = 9999;      //解锁战机所要金币 

        public List<FighterLevelData> levelDatas = new List<FighterLevelData>();

        public FighterLevelData GetLevelData()
        {
            if (curLevel <= topLevel)
            {
                return levelDatas[curLevel];
            }
            return null;
        }

        public FighterLevelData GetLevelData(int level)
        {
            foreach (var item in levelDatas)
            {
                if (item.level == level)
                    return item;
            }
            return null;
        }

        public void SerializeJson(JsonData fighterData)
        {
            try
            {
                id = (PlayerSpawner.PlayerID)((int)fighterData["id"]);
                hp = (int)fighterData["hp"];
                damage = (int)fighterData["damage"];
                unlockGolds = (int)fighterData["unlockGolds"];
                isOpen = (bool)fighterData["isOpen"];
                topLevel = (int)fighterData["topLevel"];
                curLevel = (int)fighterData["curLevel"];

                JsonData jLevelData = fighterData["levelData"];
                for (int i = 0; i < jLevelData.Count; i++)
                {
                    FighterLevelData data = new FighterLevelData();
                    data.SerializeJson(jLevelData[i]);
                    levelDatas.Add(data);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        public void UnserializeJson(ref StringBuilder fighterData)
        {
            fighterData.Append("\t\t{");
            fighterData.AppendLine();
            fighterData.AppendFormat("\t\t\t\"{0}\":{1},\n", "id", (int)this.id);
            fighterData.AppendFormat("\t\t\t\"{0}\":{1},\n", "hp", this.hp);
            fighterData.AppendFormat("\t\t\t\"{0}\":{1},\n", "damage", this.damage);
            fighterData.AppendFormat("\t\t\t\"{0}\":{1},\n", "unlockGolds", this.unlockGolds);
            fighterData.AppendFormat("\t\t\t\"{0}\":{1},\n", "isOpen", this.isOpen.ToString().ToLower());
            fighterData.AppendFormat("\t\t\t\"{0}\":{1},\n", "topLevel", this.topLevel);
            fighterData.AppendFormat("\t\t\t\"{0}\":{1},\n", "curLevel", this.curLevel);

            fighterData.Append("\t\t\t\"levelData\":[");
            for (int i = 0; i < levelDatas.Count; i++)
            {
                levelDatas[i].UnserializeJson(ref fighterData);
                if (i < levelDatas.Count - 1) fighterData.Append(",");
            }
            fighterData.AppendLine();
            fighterData.Append("\t\t\t]\n");
            fighterData.Append("\t\t}");
        }
    }
    /// <summary>
    /// 僚机数据
    /// </summary>
    public /*struct*/ class WingmanData
    {
        public WingmanSpawner.WingmanID id;
        public bool isOpen;
        public int damage;
        public int unlockGolds = 1000;

        public void SerializeJson(JsonData wingmanData)
        {
            try
            {
                id = (WingmanSpawner.WingmanID)((int)wingmanData["id"]);
                isOpen = (bool)wingmanData["isOpen"];
                damage = (int)wingmanData["damage"];
                unlockGolds = (int)wingmanData["unlockGolds"];
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        public void UnserializeJson(ref StringBuilder wingmanData)
        {
            wingmanData.Append("\t\t{");
            wingmanData.AppendLine();
            wingmanData.AppendFormat("\t\t\t\"{0}\":{1},\n", "id", (int)this.id);
            wingmanData.AppendFormat("\t\t\t\"{0}\":{1},\n", "isOpen", this.isOpen.ToString().ToLower());
            wingmanData.AppendFormat("\t\t\t\"{0}\":{1},\n", "damage", this.damage);
            wingmanData.AppendFormat("\t\t\t\"{0}\":{1}\n", "unlockGolds", this.unlockGolds);
            wingmanData.Append("\t\t}");
        }
    }
    /// <summary>
    /// 玩家数据
    /// </summary>
    public /*struct*/ class PlayerData
    {
        //金钱
        public int golds = 1000;
        //分数
        public int score;
        //体力
        public int power = 10;
        //必杀
        public int skills = 5;
        //护盾
        public int shields = 5;

        //当前战机
        public PlayerSpawner.PlayerID curFighter;
        //当前僚机
        public WingmanSpawner.WingmanID curWingman;

        //是否开启僚机
        public bool withWingman;

        public void SerializeJson(JsonData playerData)
        {
            try
            {
                golds = (int)playerData["golds"];
                score = (int)playerData["score"];
                power = (int)playerData["power"];
                skills = (int)playerData["skills"];
                shields = (int)playerData["shields"];
                withWingman = (bool)playerData["withWingman"];
                curFighter = (PlayerSpawner.PlayerID)((int)playerData["curPlayer"]);
                curWingman = (WingmanSpawner.WingmanID)((int)playerData["curWingman"]);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        public void UnserializeJson(ref StringBuilder playerData)
        {
            playerData.Append("\t\"playerData\": {");
            playerData.AppendLine();

            playerData.AppendFormat("\t\t\"{0}\":{1},\n", "golds", this.golds);
            playerData.AppendFormat("\t\t\"{0}\":{1},\n", "score", this.score);
            playerData.AppendFormat("\t\t\"{0}\":{1},\n", "power", this.power);
            playerData.AppendFormat("\t\t\"{0}\":{1},\n", "skills", this.skills);
            playerData.AppendFormat("\t\t\"{0}\":{1},\n", "shields", this.shields);
            playerData.AppendFormat("\t\t\"{0}\":{1},\n", "withWingman", withWingman.ToString().ToLower());
            playerData.AppendFormat("\t\t\"{0}\":{1},\n", "curPlayer", (int)this.curFighter);
            playerData.AppendFormat("\t\t\"{0}\":{1}\n", "curWingman", (int)this.curWingman);

            playerData.Append("\t}");
        }
    }
    /// <summary>
    /// 游戏模式
    /// </summary>
    public enum GameMode
    {
        Normal,
        Endless,
        Boss,
    }

    public enum GameState
    {
        UI,
        Playing
    }

    /// <summary>
    /// 游戏所有数据
    /// </summary>
    public class GameData
    {


        List<LevelData> levelDatas = new List<LevelData>();
        List<FighterData> fighterDatas = new List<FighterData>();
        List<WingmanData> wingmanDatas = new List<WingmanData>();

        PlayerData playerData;
        GuideData guideData;

        public string CurLevelName;
        private int curLevelIndex = 1;
        public int CurLevelIndex
        {
            get
            {
                return curLevelIndex;
            }
            set
            {
                curLevelIndex = value;
            }
        }
        ///是否为VIP
        public bool IsVip = true;
        ///是否领最了惊喜礼包
        public bool IsGetSuppriseGift = false;
        ///是否开启声音     
        public bool IsSoundOpen = true;
        ///是否开启无限框模式
        public bool IsEndlessOpen;
        ///是否开启商店  
        public bool IsShopOpen = true;
        ///是否开启排行榜  
        public bool IsRankListOpen = true;
        ///默认大招伤害值 
        public float SkillDamage = 2000;
        ///默认暴走时间
        public float CritTime = 5;
        ///默认金币加成
        public float GoldsAdditionPercent = 0;
        ///默认分数加成
        public float ScoreAddtionPercent = 0;
        ///默认最大回复体力数
        public readonly int PowerRecoveryCount = 10;

        public static GameData Instance = new GameData();

        private JsonData mainRoot;
        public GameData()
        {
        }

        //加载关卡选择的boss动画数据
        public void LoadBossArmature()
        {
            foreach (var item in levelDatas)
            {
                ArmatureResManager.Instance.Add(item.bossArmaturePath);
                //ArmatureResManager.Instance.AutoRelease(item.armaturePath);
                if (item.armatureData == null)
                    item.armatureData = new CCArmature(item.bossArmatureName);
                if (item.animationData == null)
                    item.animationData = item.armatureData.GetAnimation();

            }
        }

        public IEnumerable<LoadingScene.Percent> LoadBossArmature(LoadingScene.PercentCounter percentCounter)
        {
            int i = 0;
            foreach (var item in levelDatas)
            {
                CCSpriteFrameCache.AddSpriteFramesWithFile(ResID.Armatures_logicRect);
                ArmatureResManager.Instance.Add(item.bossArmaturePath);
                //ArmatureResManager.Instance.AutoRelease(item.armaturePath);
                if (item.armatureData == null)
                    item.armatureData = new CCArmature(item.bossArmatureName);
                if (item.animationData == null)
                    item.animationData = item.armatureData.GetAnimation();
                yield return percentCounter.NextPercent(i, levelDatas.Count);
                i++;
            }
        }

        public void UnloadBossArmature()
        {
            foreach (var item in levelDatas)
            {
                if (item.animationData != null)
                {
                    item.animationData.Dispose();
                    item.animationData = null;
                }

                if (item.armatureData != null)
                {
                    item.armatureData.Dispose();
                    item.armatureData = null;
                }
            }
            //ArmatureResManager.Instance.Release();
        }
        //
        GameMode gameMode = GameMode.Normal;
        public GameMode GameMode
        {
            set { gameMode = value; }
            get { return gameMode; }
        }

        GameState gameState = GameState.UI;
        public GameState GameState
        {
            set { gameState = value; }
            get { return gameState; }
        }

        public List<LevelData> LevelDatas
        {
            get { return levelDatas; }
        }

        public int LevelCount
        {
            get { return levelDatas.Count; }
        }

        public List<FighterData> FighterDatas
        {
            get { return fighterDatas; }
        }

        public List<WingmanData> WingmanDatas
        {
            get { return wingmanDatas; }
        }

        public PlayerData PlayerData
        {
            get { return playerData; }
        }

        public GuideData GuideData
        {
            get { return guideData; }
        }

        public LevelData GetLevelData(string name)
        {
            foreach (var level in levelDatas)
            {
                if (level.levelName == name)
                {
                    return level;
                }
            }
            return null;
        }

        public LevelData GetLevelData(int index)
        {
            if (index <= levelDatas.Count)
            {
                return levelDatas[index - 1];
            }
            return null;
        }

        public LevelData GetLevelData()
        {
            return GetLevelData(CurLevelIndex);
        }

        public FighterData GetFighterData(PlayerSpawner.PlayerID id)
        {
            for (int i = 0; i < fighterDatas.Count; i++)
            {
                if (fighterDatas[i].id == id)
                    return fighterDatas[i];
            }
            return null;
        }

        public WingmanData GetWingmanData(WingmanSpawner.WingmanID id)
        {
            foreach (var item in wingmanDatas)
            {
                if (item.id == id)
                    return item;
            }
            return null;
        }

        Player curPlayer;
        public Player CurPlayer
        {
            set { curPlayer = value; }
            get { return curPlayer; }
        }

        public LevelData CurLevel
        {
            get
            {
                return GetLevelData();
            }
        }

        public void LoadDataFile()
        {
            //   
            string configFile = CCFileUtils.GetWritablePath()+ "/Config.cfg";
            string jsonData;
            if (CCFileUtils.IsFileExist(configFile))
            {
                jsonData = CCFileUtils.GetFileDataToString(configFile);
            }
            else
            {
                jsonData = CCFileUtils.GetFileDataToString("Config/Config.cfg");
            }
            try
            {
                mainRoot = JsonMapper.ToObject(jsonData);
                this.SerializeJson(mainRoot);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void SaveDataFile()
        {
            StringBuilder jsonText = new StringBuilder();
            string path = CCFileUtils.GetWritablePath();
            UnserializeJson(ref jsonText);
            string _jsonText = jsonText.ToString();
            try
            {
                this.WriteStringToFile(path + "/Config.cfg", _jsonText);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            //保存时间
            PowerRecovery.Instance.OnExit();
        }

        protected void WriteStringToFile(string filePath, string str)
        {
            FileStream fs = new FileStream(filePath, FileMode.Create);
            UTF8Encoding utf8 = new UTF8Encoding(false);
            StreamWriter sw = new StreamWriter(fs, utf8);
            //开始写入
            sw.Write(str);
            //清空缓冲区
            sw.Flush();
            //关闭流
            sw.Close();
            fs.Close();
        }

        protected void SerializeJson(JsonData levelData)
        {
            try
            {
                //BaseData
                curLevelIndex = (int)mainRoot["curLevelIndex"];
                CurLevelName = (string)mainRoot["curLevelName"];
                IsVip = (bool)mainRoot["isVip"];
                IsGetSuppriseGift = (bool)mainRoot["isGetSuppriseGift"];
                IsSoundOpen = (bool)mainRoot["isSoundOpen"];
                IsEndlessOpen = (bool)mainRoot["isEndlessOpen"];
                IsShopOpen = (bool)mainRoot["isShopOpen"];
                IsRankListOpen = (bool)mainRoot["isRankListOpen"];
                SkillDamage = (int)mainRoot["skillDamage"];
                //PlayerData
                playerData = new PlayerData();
                playerData.SerializeJson(mainRoot["playerData"]);
                //GuideData
                guideData = new GuideData();
                guideData.SerializeJson(mainRoot["guideData"]);
                //FighterData
                JsonData jFighterDatas = mainRoot["fighterData"];
                fighterDatas.Clear();
                for (int i = 0; i < jFighterDatas.Count; i++)
                {
                    FighterData data = new FighterData();
                    data.SerializeJson(jFighterDatas[i]);
                    fighterDatas.Add(data);
                }
                //WingmanData
                JsonData jWingmanData = mainRoot["wingmanData"];
                wingmanDatas.Clear();
                for (int i = 0; i < jWingmanData.Count; i++)
                {
                    WingmanData data = new WingmanData();
                    data.SerializeJson(jWingmanData[i]);
                    wingmanDatas.Add(data);
                }
                //LevelData
                JsonData jLevelData = mainRoot["levelData"];
                levelDatas.Clear();
                for (int i = 0; i < jLevelData.Count; i++)
                {
                    LevelData data = new LevelData();
                    data.SerializeJson(jLevelData[i]);
                    levelDatas.Add(data);
                }
                //
            }
            catch (Exception e)
            {
                Utils.PrintException(e);
            }
        }
        protected void UnserializeJson(ref StringBuilder levelData)
        {
            levelData.Append("{");
            levelData.AppendLine();

            levelData.AppendFormat("\t\"{0}\":{1},\n", "curLevelIndex", this.curLevelIndex);
            levelData.AppendFormat("\t\"{0}\":\"{1}\",\n", "curLevelName", this.CurLevelName);
            levelData.AppendFormat("\t\"{0}\":{1},\n", "isVip", this.IsVip.ToString().ToLower());
            levelData.AppendFormat("\t\"{0}\":{1},\n", "isGetSuppriseGift", this.IsGetSuppriseGift.ToString().ToLower());
            levelData.AppendFormat("\t\"{0}\":{1},\n", "isSoundOpen", this.IsSoundOpen.ToString().ToLower());
            levelData.AppendFormat("\t\"{0}\":{1},\n", "isEndlessOpen", this.IsEndlessOpen.ToString().ToLower());
            levelData.AppendFormat("\t\"{0}\":{1},\n", "isShopOpen", this.IsShopOpen.ToString().ToLower());
            levelData.AppendFormat("\t\"{0}\":{1},\n", "isRankListOpen", this.IsRankListOpen.ToString().ToLower());
            levelData.AppendFormat("\t\"{0}\":{1},\n", "skillDamage", this.SkillDamage);
            
            //
            this.playerData.UnserializeJson(ref levelData);
            levelData.Append(",\n");
            //
            this.guideData.UnserializeJson(ref levelData);
            levelData.Append(",\n");
            //
            levelData.Append("\t\"fighterData\":[");
            for (int i = 0; i < this.fighterDatas.Count; i++)
            {
                levelData.AppendLine();
                fighterDatas[i].UnserializeJson(ref levelData);
                if (i < this.fighterDatas.Count - 1) levelData.Append(",");
            }
            levelData.AppendLine();
            levelData.Append("\t],\n");
            //
            levelData.Append("\t\"wingmanData\":[");
            for (int i = 0; i < this.wingmanDatas.Count; i++)
            {
                levelData.AppendLine();
                wingmanDatas[i].UnserializeJson(ref levelData);
                if (i < this.wingmanDatas.Count - 1) levelData.Append(",");
            }
            levelData.AppendLine();
            levelData.Append("\t],\n");
            //
            levelData.Append("\t\"levelData\":[");
            for (int i = 0; i < this.levelDatas.Count; i++)
            {
                levelData.AppendLine();
                levelDatas[i].UnserializeJson(ref levelData);
                if (i < this.levelDatas.Count - 1) levelData.Append(",");
            }
            levelData.AppendLine();
            levelData.Append("\t]\n");
            //
            levelData.Append("}");
        }
    }
}
