using MatrixEngine.Cocos2d;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thunder.GameLogic.Common;

namespace Thunder.GameLogic.Gaming
{
    public class SpriteBatchManager
    {
        Dictionary<string, CCSpriteBatchNode> batchNodes = new Dictionary<string, CCSpriteBatchNode>();

        protected CCNode worldNode = new CCNode(); //default
        protected CCSpriteBatchNode defNode;

        public static SpriteBatchManager Instance;

        public SpriteBatchManager()
        {
            //
            //defNode = GetBatchNode(ResID.Armatures_zidan.Replace(".plist", ".png"));
            Instance = this;
        }

        public CCNode WorldNode
        {
            set
            {
                if (worldNode == null || worldNode!=value)
                {
                    //Remove from this node
                    foreach (var item in batchNodes)
                    {
                        worldNode = value;
                        //item.Value.RemoveFromParent();
                        worldNode.AddChild(item.Value);                      
                    }
                }
            }
            get
            {
                return worldNode;
            }
        }

        public CCSpriteBatchNode GetBatchNode(string imagePath)
        {
            CCSpriteBatchNode node;
            if (!batchNodes.TryGetValue(imagePath, out node))
            {
                node = this.CreateBatch(imagePath);
                node.ZOrder = PlayingScene.ZOrder_batch;
                if (node != null)
                {
                    batchNodes[imagePath] = node;
                    worldNode.AddChild(node);
                }
            }
            return node;
        }


        protected CCSpriteBatchNode CreateBatch(string imagePath)
        {
            if (CCFileUtils.IsFileExist(imagePath))
            {
                Console.WriteLine("BatchImage:"+imagePath);
                return new CCSpriteBatchNode(imagePath);
            }
            else
            {
                return null;
            }
        }

    }
}
