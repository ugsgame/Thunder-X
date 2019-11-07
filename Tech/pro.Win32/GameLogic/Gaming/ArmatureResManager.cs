using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MatrixEngine.Cocos2d;
using MatrixEngine.CocoStudio.Armature;

namespace Thunder.GameLogic.Gaming
{
    public class ArmatureResManager
    {
        List<string> resStack = new List<string>();
        List<string> autoStack = new List<string>();

        public static ArmatureResManager Instance = new ArmatureResManager();

        public ArmatureResManager()
        {

        }

        public void Add(string resPath)
        {
            if (!resStack.Contains(resPath))
            {
                resStack.Add(resPath);
                CCArmDataManager.AddArmatureFile(resPath);
            }
        }

        public void AutoRelease(string resPath)
        {
            if (!autoStack.Contains(resPath))
            {
                autoStack.Add(resPath);
                Add(resPath);
            }
        }

        public void Remove(string resPath)
        {
            if (resStack.Contains(resPath))
            {
                resStack.Remove(resPath);
                CCArmDataManager.RemoveArmatureFile(resPath);
            }
            if (autoStack.Contains(resPath))
            {
                autoStack.Remove(resPath);
            }
        }

        public void ReleaseAll()
        {
            foreach (var item in resStack)
            {
                if (CCFileUtils.IsFileExist(item))
                {
                    Console.WriteLine("Remove ArmatureFile:" + item);
                    CCArmDataManager.RemoveArmatureFile(item);
                }
            }
            resStack.Clear();
            autoStack.Clear();
        }

        public void Release()
        {
            for (int i = autoStack.Count - 1; i >= 0; --i)
            {
                Remove(autoStack[i]);
            }
            autoStack.Clear();
        }
    }
}
