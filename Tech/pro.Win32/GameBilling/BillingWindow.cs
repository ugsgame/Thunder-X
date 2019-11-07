using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thunder.GameBilling
{
    public interface BillingWindow
    {
        void OnBillingSuccess(BillingID id);
        void OnBillingFail(BillingID id);
    }
}
