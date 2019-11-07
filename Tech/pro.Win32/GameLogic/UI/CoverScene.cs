using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MatrixEngine.Cocos2d;
using Thunder.GameLogic.Common;

namespace Thunder.GameLogic.UI
{
    public class CoverScene:GameScene
    {
        CoverScene()
        {

        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            //if (disposing)
            {
                //TODO:释放非托管资源
            }
        }

        public override bool OnTouchBegan(float x, float y)
        {
            Function.GoTo(UIFunction.主菜单);
            return true;
        }
    }
}
