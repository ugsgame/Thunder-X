using MatrixEngine.Platform.Android;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thunder.GameBilling
{
    public class JNIBillingHelper
    {
        public AndroidJavaObject OperatorHelper;

        private static JNIBillingHelper instance;
        public static JNIBillingHelper Instance
        {
            get 
            {
                if (instance == null)
                    instance = new JNIBillingHelper();

                return instance;
            }
        }

        public JNIBillingHelper()
        {
            OperatorHelper = new AndroidJavaObject("org/operator/OperatorHelper");
        }

        public void DoBilling(String productID, float money, String msg)
        {
            OperatorHelper.CallStatic("onDoBilling", productID, msg, money);
        }

        public void MoreGame()
        {
            OperatorHelper.CallStatic("onMoreGame");
        }
    }
}
