using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thunder.GameLogic.Gaming.Actors.Drops
{
    /// <summary>
    /// 掉落物类型
    /// </summary>
    public enum DropType:int
    {
        Drop_Gem_Blue_1 = ActorID.Drop1,        //蓝宝石 小
        Drop_Gem_Blue_2 = ActorID.Drop2,        //蓝宝石 大
        
        Drop_Gem_Yellow_1 = ActorID.Drop3,                      //黄宝石 小 
        Drop_Gem_Yellow_2 = ActorID.Drop4,                      //黄宝石 大

        Drop_Gem_Green_1 = ActorID.Drop5,                       //绿宝石 小
        Drop_Gem_Green_2 = ActorID.Drop6,                       //绿宝石 大

        Drop_Gem_White = ActorID.Drop7,                         //钻石

        Drop_Power = ActorID.Drop8,                             //升级
        Drop_Shield = ActorID.Drop9,                            //护盾
        Drop_Skill = ActorID.Drop10,                              //大招
        Drop_Bomb = ActorID.Drop11                              //技能
    }
}
