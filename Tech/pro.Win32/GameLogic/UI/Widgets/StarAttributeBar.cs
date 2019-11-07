using MatrixEngine.Cocos2d;
using MatrixEngine.CocoStudio.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thunder.GameLogic.UI.Widgets
{
    public class StarAttributeBar:UIWidget
    {

        class Star:CCNode
        {
            public CCSprite star1;
            public CCSprite star2;

            public Star()
            {
                star1 = new CCSprite("xingxing02_off.png",true);
                star2 = new CCSprite("xingxing02_on.png",true);

                this.AddChild(star1);
                this.AddChild(star2);

                this.Enable = false;
            }

            private bool enable;
            public bool Enable
            {
                set
                {
                    enable = value;
                    if (enable)
                    {
                        star1.IsVisible = true;
                        star2.IsVisible = false;
                    }
                    else
                    {
                        star1.IsVisible = false;
                        star2.IsVisible = true;
                    }
                }
                get
                {
                    return enable;
                }
            }
        }

        private Star[] stars = new Star[5]; 
        private readonly float interval = 20;

        public StarAttributeBar()
        {
            for (int i = 0; i < stars.Length; i++)
            {
                stars[i] = new Star();
                stars[i].PostionY = 0;
                stars[i].PostionX = i*interval;

                this.AddNode(stars[i]);
            }

            this.Level = 0;
        }

        private int level;
        public int Level
        {
            set 
            {
                level = value > 5 ? 5 : value;
                for (int i = 0; i < stars.Length; i++)
                {
                    if(i<level)
                        stars[i].Enable = true;
                    else
                        stars[i].Enable = false;
                }
            }
            get
            {
                return level;
            }
        }
        
    }
}
