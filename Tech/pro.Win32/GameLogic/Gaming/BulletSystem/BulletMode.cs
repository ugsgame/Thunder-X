using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thunder.GameLogic.Gaming.BulletSystems
{
    public enum BulletMode : int
    {
        /**常规子弹 （A mode）*/
        kBulletModeNormal = 0,
        /**跟踪弹 （B mode）*/
        kBulletModeFallow,
        /**激光 （C mode）*/
        kBulletModeLaser,
        /**其它子弹 (D mode)*/
        kBulletModeOther,
    }

    public enum DisplayType : int
    {
        /**普通精灵*/
        dTypeSprite = 0,
        /**精灵帧*/
        dTypeSpriteFrame,
        /**粒子*/
        dTypeParticle,
        /**cocos动画*/
        dTypeAnimation,
        /**cocostudio动画*/
        dTypeArmature
    }
}
