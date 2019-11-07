using MatrixEngine.Cocos2d;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thunder.GameLogic.ExternAction;

namespace Thunder.GameLogic.UI.Effects
{
    public class ActionEffects
    {
        /// <summary>
        /// UI 常用的放在效果
        /// </summary>
        /// <returns></returns>
        public static CCAction ScaleEffect(float scale = 1.3f)
        {
            return new CCActionRepeatForever(new CCActionSequence(new ActionSetScale(1.0f), new ActionSetAlpha(255), new CCActionSpawn(new CCActionScaleTo(1.5f, scale), new CCActionFadeOut(1.5f))));
        }
    }
}
