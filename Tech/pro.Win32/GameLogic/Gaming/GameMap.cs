using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LitJson;
using Thunder.Game;
using MatrixEngine.Cocos2d;
using MatrixEngine.CocoStudio.Scene;
using MatrixEngine.Math;
using Thunder.Common;
using MatrixEngine.CocoStudio.Armature;

namespace Thunder.GameLogic.Gaming
{
    /// <summary>
    /// 游戏地图
    /// </summary>
    public class GameMap : CCLayer,InterfaceGameState
    {
        /// <summary>
        /// 地图子图层
        /// </summary>
        public class MapLayer : CCNode
        {

            private float _Speed;
            private float Parallax;
            private static int mapCount = 2;

                    //内存标记缓存
            private List<string> textureCache = new List<string>();
            private List<string> armatureCache = new List<string>();

            List<CCNode> SubMaps = new List<CCNode>(mapCount);

            public MapLayer(String layerPath,Size size)
            {
                this.ContextSize = size;
                Parallax = this.ContextSize.width - Config.SCREEN_WIDTH;
                //TODO:按地图高度与视窗高比较来决定要创建多少个地图
                for (int i = 0; i < mapCount; i++)
                {
                    CCNode map = SceneReader.CreateNodeWithSceneFile(layerPath);
                    SubMaps.Add(map);
                    map.PostionY = i * this.ContextSize.height;
                    this.AddChild(map);
                }

                
                JsonData config = null;
                try
                {
                    string str = CCFileUtils.GetFileDataToString(layerPath);
                    config = JsonMapper.ToObject(str);
                }
                catch (Exception e)
                {
                    Console.WriteLine("解释地图配置文件出现异常，请检查json格式是否正确。");
                    Utils.PrintException(e);
                }

                if (config != null)
                {
                    ParseGameObject(config, this);
                }

                Console.WriteLine("textureCache:" + textureCache.Count);
               
            }

            protected override void Dispose(bool disposing)
            {
                //if (disposing)
                {
                    for (int i = 0; i < SubMaps.Count;i++ )
                    {
                        //item.RemoveAllChildren();
                        this.RemoveChild(SubMaps[i]);
                        //item.Dispose();
                    }
                    SubMaps.Clear();
                }

                //Remove textures
                foreach (var item in textureCache)
                {
                    //Texture key为完整路径
                    Console.WriteLine(CCFileUtils.FullPathForFilename(item));

                    //CCTextureCache.RemoveTextureForKey(CCFileUtils.FullPathForFilename(item));
                }
                //Remove Armature
                foreach (var item in armatureCache)
                {
                    //要完整路径
                    Console.WriteLine(CCFileUtils.FullPathForFilename(item));
                    //CCArmDataManager.RemoveArmatureFile(CCFileUtils.FullPathForFilename(item));
                }

                textureCache.Clear();
                armatureCache.Clear();

                base.Dispose(disposing);
            }

            public float Speed
            {
                get { return _Speed; }
                set
                {
                    _Speed = Math.Abs(value);
                }
            }

            public void Update(float dTime)
            {
                foreach (var map in SubMaps)
                {
                    map.PostionY -= this._Speed * dTime;

                    if (map.PostionY > Config.SCREEN_HEIGHT || (map.PostionY + this.ContextSize.height) < 0)
                        map.IsVisible = false;
                    else
                        map.IsVisible = true;
                }
                CCNode firstNode = GetFirst();
                CCNode lastNode = GetLast();

                if (firstNode.PostionY + this.ContextSize.height < 0)
                {
                    firstNode.PostionY = lastNode.PostionY + this.ContextSize.height;
                }

                //地图在屏幕居中
                this.PostionX = -Parallax * GameLayer.VeiwPercent;
                //
            }

            private CCNode GetLast()
            {
                CCNode lastNode = SubMaps[0];
                foreach (var map in SubMaps)
                {
                    if (map.PostionY >= lastNode.PostionY)
                        lastNode = map;
                }
                return lastNode;
            }

            private CCNode GetFirst()
            {
                CCNode firstNode = SubMaps[0];
                foreach (var map in SubMaps)
                {
                    if (map.PostionY < firstNode.PostionY)
                        firstNode = map;
                }
                return firstNode;
            }


            private JsonData ParseGameObject(JsonData gameObject, CCNode layer)
            {
                JsonData rootObject = (JsonData)gameObject["gameobjects"];
                JsonData rootComponent = (JsonData)gameObject["components"];

                ParseComponents(rootComponent, gameObject, layer);

                foreach (JsonData item in rootObject)
                {
                    if (item.Count <= 0)
                        break;
                    else
                        ParseGameObject(item, layer);
                }


                return gameObject;
            }

            private void ParseComponents(JsonData components, JsonData gameObject, CCNode layer)
            {
                /*            Console.WriteLine("     ParseComponents:");*/
                int tag = (int)gameObject["objecttag"];
                foreach (JsonData item in components)
                {
                    string __type = (string)item["__type"];
                    string klass = (string)item["classname"];
                    string name = (string)item["name"];

                    try
                    {
                        JsonData fileData = (JsonData)item["fileData"];
                        string stringPath = (string)fileData["path"];
                        string plistFile = (string)fileData["plistFile"];
                        int resourceType = (int)fileData["resourceType"];

                        if (resourceType == 0)
                        {
                            if (klass == "CCSprite")
                            {
                                //加入到纹理缓存
                                if (stringPath.Contains(".png") || stringPath.Contains(".jpg"))
                                {
                                    if (!textureCache.Contains(stringPath))
                                    {
                                        textureCache.Add(stringPath);
                                    }
                                }
                            }
                            else if (klass == "CCTMXTiledMap")
                            {

                            }
                            else if (klass == "CCParticleSystemQuad")
                            {
                                //CCParticleSystem particle = SceneReader.GetParticleByTag(tag);
                                //particle.PositionType = tCCPositionType.kCCPositionTypeGrouped;
                            }
                            else if (klass == "CCArmature")
                            {
                                if (stringPath.Contains(".ExportJson"))
                                {
                                    if (!armatureCache.Contains(stringPath))
                                    {
                                        armatureCache.Add(stringPath);
                                    }
                                }
                            }
                        }
                        else if (resourceType == 1)
                        {
                            if (klass == "CCSprite")
                            {
                                if (plistFile.Contains(".plist"))
                                {
                                    string pngFile = plistFile.Replace(".plist", "png");
                                    if (!textureCache.Contains(pngFile))
                                    {
                                        textureCache.Add(pngFile);
                                    }
                                }
                            }
                        }

                    }
                    catch (Exception)
                    {
                        Console.WriteLine("找不到此键值：fileData");
                        //Utils.PrintException(e);
                    }

                }
            }

        }

        /// <summary>
        /// 所有图层
        /// </summary>
        public static int layerCount = 2;
        private List<MapLayer> mapLayers = new List<MapLayer>(layerCount);
        private MapLayer mainLayer;
        /// <summary>
        /// 地图资源路径
        /// </summary>
        private readonly string Custom_Property = "CustomProperty/";
        private readonly string Map_Data = "MapData/";
        private readonly string MapName;
        /// <summary>
        /// 地图水平移动系数
        /// </summary>
        private float _MoveSpeed = 50.0f;
        private float _MoveRate = 1.5f;

        public GameMap()
        {

        }

        public GameMap(string map)
        {
            MapName = map;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                //TODO:释放非托管资源
            }
        }

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void OnEnterTransitionFinish()
        {
            base.OnEnterTransitionFinish();
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void OnExitTransitionStart()
        {
            base.OnExitTransitionStart();
        }

        public override void OnUpdate(float dTime)
        {
            base.OnUpdate(dTime);

            foreach (var item in mapLayers)
            {
               item.Update(dTime);
            }
            //SceneReader.DoOptimization();
        }

        public bool LoadMap()
        {
            return LoadMap(MapName);
        }

        public bool LoadMap(string mapName)
        {
            //再清一次
            this.RemoveAllChildren(true);
            foreach (var item in mapLayers)
            {
                item.Dispose();
            }
            mapLayers.Clear();
            //
            float layerSpeed = this.MoveSpeed;
            for (int i = 1; i <= layerCount; i++)
            {
                string mapPath = mapName + "_layer" + i + ".json";
                //读取自定义配置文件
                JsonData config = null;
                try
                {
                    string str = CCFileUtils.GetFileDataToString(Utils.ConvertMapResPath(Custom_Property + mapPath));
                    config = JsonMapper.ToObject(str);
                }
                catch (Exception e)
                {
                    Console.WriteLine("解释自定义配置文件出现异常，请检查json格式是否正确。");
                    Utils.PrintException(e);
                }
                //

                MapLayer mapLayer = new MapLayer(Utils.ConvertMapResPath(Map_Data + mapPath),new Size((int)config["w"], (int)config["h"]));
                mapLayer.Speed = layerSpeed;
                layerSpeed *= this.MoveRate;
                mapLayers.Add(mapLayer);
                this.AddChild(mapLayer);

                if (i == layerCount - 1)
                {
                    this.MainLayer = mapLayer;
                }
            }
            return true;
        }

        public void UnloadMap()
        {
            foreach (var item in mapLayers)
            {
                this.RemoveChild(item);
                item.Dispose();
            }
            mapLayers.Clear();

            this.MainLayer = null;
        }

        public void PlayBlack(float time)
        {
            this.RunSequenceActions(new CCActionHide(), new CCActionDelayTime(time), new CCActionShow());
        }

        /// <summary>
        /// 图层一移动速度
        /// </summary>
        public float MoveSpeed
        {
            set { _MoveSpeed = value; }
            get { return _MoveSpeed; }
        }
        /// <summary>
        /// 图层递增速率
        /// </summary>
        public float MoveRate
        {
            set { _MoveRate = value; }
            get { return _MoveRate; }
        }

        public MapLayer MainLayer
        {
            set
            {
                this.mainLayer = value;
            }
            get
            {
                return this.mainLayer;
            }
        }


        public void OnPause()
        {
            this.PauseSchedulerAndActions();
        }

        public void OnResume()
        {
            this.ResumeSchedulerAndActions();
        }
    }
}
