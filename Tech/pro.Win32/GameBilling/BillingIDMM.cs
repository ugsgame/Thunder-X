using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thunder.GameBilling
{
    class BillingIDMM
    {
        static string ID0_Null = "00000000000000";
        static string ID1_BuyGolds1W = "30000898754302";
        static string ID2_BuyGolds3W = "30000898754303";
        static string ID3_BuySkills = "30000898754304";
        static string ID4_BuyShields = "30000898754305";
        static string ID5_BuySuperGift = "30000898754306";
        static string ID6_BuyLotteryAll = "30000898754307";
        static string ID7_BuyRichGift = "30000898754308";
        static string ID8_BuyLevelUp = "30000898754309";
        static string ID9_BuyWingman = "30000898754310";
        static string ID10_BuyResurrection = "30000898754311";
        static string ID11_BuyGolds30W = "30000898754312";
        static string ID12_SurpriseGift = "30000898754301";

        public static BillingID CoverToGameBillingID(string id)
        {
            Console.WriteLine("MM:" + id);
            if (id == ID0_Null)
                return BillingID.ID0_Null;
            else if (id == ID1_BuyGolds1W)
                return BillingID.ID1_BuyGolds1W;
            else if (id == ID2_BuyGolds3W)
                return BillingID.ID2_BuyGolds3W;
            else if (id == ID3_BuySkills)
                return BillingID.ID3_BuySkills;
            else if (id == ID4_BuyShields)
                return BillingID.ID4_BuyShields;
            else if (id == ID5_BuySuperGift)
                return BillingID.ID5_BuySuperGift;
            else if (id == ID6_BuyLotteryAll)
                return BillingID.ID6_BuyLotteryAll;
            else if (id == ID7_BuyRichGift)
                return BillingID.ID7_BuyRichGift;
            else if (id == ID8_BuyLevelUp)
                return BillingID.ID8_BuyLevelUp;
            else if (id == ID9_BuyWingman)
                return BillingID.ID9_BuyWingman;
            else if (id == ID10_BuyResurrection)
                return BillingID.ID10_BuyResurrection;
            else if (id == ID11_BuyGolds30W)
                return BillingID.ID11_BuyGolds30W;
            else if (id == ID12_SurpriseGift)
                return BillingID.ID12_SurpriseGift;
            else
                return BillingID.ID0_Null;
        }
        public static string CoverToNativeBillingID(BillingID id)
        {
            string nativeId = ID0_Null;
            switch (id)
            {
                case BillingID.ID0_Null:
                    nativeId = ID0_Null;
                    break;
                case BillingID.ID1_BuyGolds1W:
                    nativeId = ID1_BuyGolds1W;
                    break;
                case BillingID.ID2_BuyGolds3W:
                    nativeId = ID2_BuyGolds3W;
                    break;
                case BillingID.ID3_BuySkills:
                    nativeId = ID3_BuySkills;
                    break;
                case BillingID.ID4_BuyShields:
                    nativeId = ID4_BuyShields;
                    break;
                case BillingID.ID5_BuySuperGift:
                    nativeId = ID5_BuySuperGift;
                    break;
                case BillingID.ID6_BuyLotteryAll:
                    nativeId = ID6_BuyLotteryAll;
                    break;
                case BillingID.ID7_BuyRichGift:
                    nativeId = ID7_BuyRichGift;
                    break;
                case BillingID.ID8_BuyLevelUp:
                    nativeId = ID8_BuyLevelUp;
                    break;
                case BillingID.ID9_BuyWingman:
                    nativeId = ID9_BuyWingman;
                    break;
                case BillingID.ID10_BuyResurrection:
                    nativeId = ID10_BuyResurrection;
                    break;
                case BillingID.ID11_BuyGolds30W:
                    nativeId = ID11_BuyGolds30W;
                    break;
                case BillingID.ID12_SurpriseGift:
                    nativeId = ID12_SurpriseGift;
                    break;
                default:
                    break;
            }

            return nativeId;
        }
    }
}
