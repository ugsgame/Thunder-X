
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MatrixEngine.CocoStudio.GUI;
using MatrixEngine.Cocos2d;

namespace Thunder.GameLogic.UI.Widgets
{
    public class PowerAttributeBar : UIWidget
    {
        private int post;
        private CCSprite[] imgs = new CCSprite[20];

        private float tickCount;
        private readonly float tick = 0.1f;

        private int attribute;
        public int Attribute
        {
            get { return attribute; }
            set
            {
                attribute = value;
                post = 0;
                for (int i = 0; i < imgs.Length; i++)
                {
                    imgs[i].IsVisible = false;
                }
            }
        }

        public PowerAttributeBar()
        {
            //一定要先加载到UI的纹理
            for (int i = 0; i < imgs.Length; i++)
            {
                imgs[i] = new CCSprite("bossxin_dian.png", true);
                imgs[i].IsVisible = false;
                this.AddNode(imgs[i]);
                imgs[i].PostionX = i * 15;
                imgs[i].PostionY = 0;
            }
        }

        public virtual void Update(float dt)
        {
            tickCount += dt;
            if (tickCount >= tick)
            {
                //do something
                if (post < attribute)
                {
                    post++;
                    for (int i = 0; i < post; i++)
                    {
                        imgs[i].IsVisible = true;
                    }
                }
                //
                tickCount = 0;
            }
        }
    }
}
