using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thunder.GameBilling
{
    public enum BillingID
    {           
        /// <summary>
        /// 无计费
        /// </summary>
        ID0_Null,
        /// <summary>
        /// 计费点1 宝石1W
        /// </summary>
        ID1_BuyGolds1W,  
        
        /// <summary>
        /// 计费点2 宝石3W
        /// </summary>
        ID2_BuyGolds3W,

        /// <summary>
        /// 计费点3 必杀X5
        /// </summary>
        ID3_BuySkills,

        /// <summary>
        /// 计费点4 护盾X5
        /// </summary>
        ID4_BuyShields,

        /// <summary>
        /// 计费点5 宝石5W必杀5护盾5【超值礼包】
        /// </summary>
        ID5_BuySuperGift,

        /// <summary>
        /// 计费点6 购买获得全部钻石6W
        /// </summary>
        ID6_BuyLotteryAll,   
  
        /// <summary>
        /// 计费点7 宝石188888必杀10护盾15体力X10【女神大礼包】
        /// </summary>
        ID7_BuyRichGift,

        /// <summary>
        /// 计费点8 战机一键满级
        /// </summary>
        ID8_BuyLevelUp,

        /// <summary>
        /// 计费点9 购买僚机：狂暴
        /// </summary>
        ID9_BuyWingman,

        /// <summary>
        /// 计费点10 死亡复活，必杀X1护盾X2  
        /// </summary>  
        ID10_BuyResurrection,
        /// <summary>
        /// 计费点11 宝石X300000
        /// </summary>
        ID11_BuyGolds30W,

        /// <summary>
        /// 计费点12 惊喜礼包
        /// </summary>
        ID12_SurpriseGift
    }
}
