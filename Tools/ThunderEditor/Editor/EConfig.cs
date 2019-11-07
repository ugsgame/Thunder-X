using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LitJson;
using ThunderEditor.Utils;

namespace ThunderEditor.Editor
{
    public class EConfig
    {
        readonly string configFile = "config.json";
        JsonData mainConfigData;

        public static EConfig Instance = new EConfig();

        public EConfig()
        {
            try
            {
                //根目录
                string json = FileSever.ReadFileToString(configFile);
                mainConfigData = JsonMapper.ToObject(json);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.WriteLine("version:"+mainConfigData["version"]);
        }

        public JsonData Json
        {
            get { return mainConfigData; }
        }

        public double SceneWidth
        {
            get 
            {
                return (double)mainConfigData["sceneWidth"];
            }
            set 
            {
                mainConfigData["sceneWidth"] = value;
            }
        }

        public double SceneHeight
        {
            get
            {
                return (double)mainConfigData["sceneHeight"];
            }
            set
            {
                mainConfigData["sceneHeight"] = value;
            }
        }

        public string ResPath
        {
            get
            {
                return (string)mainConfigData["resPath"];
            }
            set
            {
                mainConfigData["resPath"] = value;
            }
        }

        public string ProjectPath
        {
            get
            {
                return (string)mainConfigData["projectPath"];
            }
            set
            {
                mainConfigData["projectPath"] = value;
            }
        }

        public string SimulatorPath
        {
            get
            {
                return (string)mainConfigData["simulatorPath"];
            }
            set
            {
                mainConfigData["simulatorPath"] = value;
            }
        }

        public JsonData ActorData
        {
            get 
            {
                return (JsonData)mainConfigData["actorData"];
            }
            set
            {
                mainConfigData["actorData"] = value;
            }
        }

        public void Save()
        {

        }
    }
}
