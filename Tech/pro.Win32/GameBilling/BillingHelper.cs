using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thunder.GameLogic.Common;

namespace Thunder.GameBilling
{
    public class BillingHelper
    {
        //计费id对应的数值
        public static readonly int ID1_BuyGolds1W_goldNum = 20000;
        public static readonly int ID2_BuyGolds3W_goldNnum = 60000;

        public static readonly int ID3_BuySkills_skillNum = 5;
        public static readonly int ID4_BuyShields_shieldNum = 5;

        public static readonly int ID5_BuySuperGift_goldNum = 100000;
        public static readonly int ID5_BuySuperGift_skillNum = 5;
        public static readonly int ID5_BuySuperGift_shieldNum = 5;

        public static readonly int ID6_BuyLotteryAll_num = 0;

        public static readonly int ID7_BuyRichGift_goldNum = 288888;
        public static readonly int ID7_BuyRichGift_skillNum = 10;
        public static readonly int ID7_BuyRichGift_shieldNum = 15;
        public static readonly int ID7_BuyRichGift_powerNum = 10;

        public static readonly int ID8_BuyLevelUp_num = 0;

        public static readonly int ID9_BuyWingman_num = 0;

        public static readonly int ID10_BuyResurrection_skillNum = 10000;
        public static readonly int ID10_BuyResurrection_shieldNum = 10000;

        public static readonly int ID11_BuyGolds30W_goldNum = 300000;

        public static readonly int ID12_BuySuppriseGift_goldNum = 2888;
        public static readonly int ID12_BuySuppriseGift_skillNum = 1;
        public static readonly int ID12_BuySuppriseGift_shieldNum = 2;
        //
        //运营商
        public enum Operators : int
        {
            Null = 0,           //无
            MM,                 //mm
            Mobile,             //移动
            Telecom,            //电信
            Test                //测试
        }

        //当前运营商
        public static Operators CurrentOperator = Operators.Null;

        public static void SetOperatorID(Operators id)
        {
            CurrentOperator = id;
        }
        public static Operators GetOperatorID()
        {
            return CurrentOperator;
        }

        //统一计费成功回调处理
        public static void OnBllingDeal(BillingID id)
        {
            PlayerData playerData = GameData.Instance.PlayerData;
            switch (id)
            {
                case BillingID.ID1_BuyGolds1W:
                    playerData.golds += ID1_BuyGolds1W_goldNum;
                    break;
                case BillingID.ID2_BuyGolds3W:
                    playerData.golds += ID2_BuyGolds3W_goldNnum;
                    break;
                case BillingID.ID3_BuySkills:
                    playerData.skills += ID3_BuySkills_skillNum;
                    break;
                case BillingID.ID4_BuyShields:
                    playerData.shields += ID4_BuyShields_shieldNum;
                    break;
                case BillingID.ID5_BuySuperGift:
                    playerData.golds += ID5_BuySuperGift_goldNum;
                    playerData.skills += ID5_BuySuperGift_skillNum;
                    playerData.shields += ID5_BuySuperGift_shieldNum;
                    break;
                case BillingID.ID6_BuyLotteryAll:
                    break;
                case BillingID.ID7_BuyRichGift:
                    playerData.golds += ID7_BuyRichGift_goldNum;
                    playerData.skills += ID7_BuyRichGift_skillNum;
                    playerData.shields += ID7_BuyRichGift_shieldNum;
                    playerData.power += ID7_BuyRichGift_powerNum;
                    break;
                case BillingID.ID8_BuyLevelUp:
                    break;
                case BillingID.ID9_BuyWingman:
                    break;
                case BillingID.ID10_BuyResurrection:
                    break;
                case BillingID.ID11_BuyGolds30W:
                    playerData.golds += ID11_BuyGolds30W_goldNum;
                    break;
                case BillingID.ID12_SurpriseGift:
                    playerData.golds += ID12_BuySuppriseGift_goldNum;
                    playerData.skills += ID12_BuySuppriseGift_skillNum;
                    playerData.shields += ID12_BuySuppriseGift_shieldNum;
                    break;
                default:
                    break;
            }
        }

        private static BillingWindow billingWindow;
        public static void DoBilling(BillingID id, BillingWindow _billingWindow = null)
        {
            BillingHelper.billingWindow = _billingWindow;
            string nativeBilling = CoverToNaiveBillingID(id);
            string name = CoverToBillingName(id);
            float money = CoverToBillingMoney(id);
            Console.WriteLine("nativeBilling:" + nativeBilling);
            try
            {
                switch (MatrixEngine.Engine.System.TARGET_PLATFORM)
                {
                    case MatrixEngine.Engine.System.PLATFORM.UNKNOWN:
                        {
                            SimulateSDK.instance.DoBilling(nativeBilling,money, name);
                        }
                        break;
                    case MatrixEngine.Engine.System.PLATFORM.IOS:
                        break;
                    case MatrixEngine.Engine.System.PLATFORM.ANDROID:
                        {                           
                            switch (CurrentOperator)
                            {
                                case Operators.Null:
                                    SimulateSDK.instance.DoBilling(nativeBilling,money, name);
                                    break;
                                case Operators.MM:
                                    JNIBillingHelper.Instance.DoBilling(nativeBilling,money, name);
                                    break;
                                case Operators.Mobile:
                                    break;
                                case Operators.Telecom:
                                    break;
                                case Operators.Test:
                                    SimulateSDK.instance.DoBilling(nativeBilling, money, name);
                                    break;
                                default:
                                    break;
                            }
                        }
                        break;
                    case MatrixEngine.Engine.System.PLATFORM.WIN32:
                        {
                            SimulateSDK.instance.DoBilling(nativeBilling,money, name);
                        }
                        break;
                    default:
                        {

                        }
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }


        }

        /// <summary>
        /// Native层的计费回调
        /// </summary>
        /// <param name="success">是否成功</param>
        /// <param name="nativeBillingID"></param>
        public static void OnNativeBilling(bool success, string nativeBillingID,string nativeBillingMsg = "")
        {
            try
            {
                BillingID id = ConvertToGameBillingID(nativeBillingID);
                Console.WriteLine("OnNativeBilling:" + id);
                if (billingWindow != null)
                {
                    if (success)
                    {
                        float money = CoverToBillingMoney(id);
                        if (!GameData.Instance.IsVip && money >= 2)
                        {
                            Function.GoTo(UIFunction.VIP提示);
                        }
                        billingWindow.OnBillingSuccess(id);
                    }
                    else
                        billingWindow.OnBillingFail(id);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static BillingID ConvertToGameBillingID(string nativeBillingID)
        {
            BillingID id = BillingID.ID0_Null;
            //TODO:跟据不同的计费平台把原生的计费id转为游戏计费id
            switch (CurrentOperator)
            {
                case Operators.Null:
                    id = (BillingID) Convert.ToInt32(nativeBillingID);
                    break;
                case Operators.MM:
                    id = BillingIDMM.CoverToGameBillingID(nativeBillingID);
                    break;
                case Operators.Mobile:
                    break;
                case Operators.Telecom:
                    break;
                case Operators.Test:
                    id = (BillingID)Convert.ToInt32(nativeBillingID);
                    break;
                default:
                    break;
            }

            return id;
        }

        //TODO
        public static string CoverToNaiveBillingID(BillingID gameBillingID)
        {
            string billingID = "";

            switch (CurrentOperator)
            {
                case Operators.Null:
                    billingID = Convert.ToString((int)gameBillingID);
                    break;
                case Operators.MM:
                    billingID = BillingIDMM.CoverToNativeBillingID(gameBillingID);
                    break;
                case Operators.Mobile:
                    break;
                case Operators.Telecom:
                    break;
                case Operators.Test:
                    billingID = Convert.ToString((int)gameBillingID);
                    break;
                default:
                    break;
            }

            return billingID;
        }

        public static string CoverToBillingName(BillingID billingID)
        {
            string name = "";
            switch (billingID)
            {
                case BillingID.ID0_Null:
                    break;
                case BillingID.ID1_BuyGolds1W:
                    name = "宝石30000";
                    break;
                case BillingID.ID2_BuyGolds3W:
                    name = "宝石60000";
                    break;
                case BillingID.ID3_BuySkills:
                    name = "必杀X5";
                    break;
                case BillingID.ID4_BuyShields:
                    name = "护盾X5";
                    break;
                case BillingID.ID5_BuySuperGift:
                    name = "超值礼包";
                    break;
                case BillingID.ID6_BuyLotteryAll:
                    name = "获得全部钻石";
                    break;
                case BillingID.ID7_BuyRichGift:
                    name = "女神大礼包";
                    break;
                case BillingID.ID8_BuyLevelUp:
                    name = "战机一键满级";
                    break;
                case BillingID.ID9_BuyWingman:
                    name = "僚机：狂暴";
                    break;
                case BillingID.ID10_BuyResurrection:
                    name = "复活";
                    break;
                case BillingID.ID11_BuyGolds30W:
                    name = "宝石300000";
                    break;
                case BillingID.ID12_SurpriseGift:
                    name = "惊喜礼包";
                    break;
                default:
                    break;
            }

            return name;
        }

        public static float CoverToBillingMoney(BillingID billingID)
        {
            float money = 0;
            switch (billingID)
            {
                case BillingID.ID0_Null:
                    money = 0;
                    break;
                case BillingID.ID1_BuyGolds1W:
                    money = 5;
                    break;
                case BillingID.ID2_BuyGolds3W:
                    money = 10;
                    break;
                case BillingID.ID3_BuySkills:
                    money = 5;
                    break;
                case BillingID.ID4_BuyShields:
                    money = 5;
                    break;
                case BillingID.ID5_BuySuperGift:
                    money = 15;
                    break;
                case BillingID.ID6_BuyLotteryAll:
                    money = 20;
                    break;
                case BillingID.ID7_BuyRichGift:
                    money = 29;
                    break;
                case BillingID.ID8_BuyLevelUp:
                    money = 10;
                    break;
                case BillingID.ID9_BuyWingman:
                    money = 10;
                    break;
                case BillingID.ID10_BuyResurrection:
                    money = 2;
                    break;
                case BillingID.ID11_BuyGolds30W:
                    money = 30;
                    break;
                case BillingID.ID12_SurpriseGift:
                    money = 0.1f;
                    break;
                default:
                    break;
            }
            return money;
        }

    }
}
