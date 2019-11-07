package org.operator;

import com.billing.plugin.BillingHelper;
import com.billing.plugin.BillingListener;

public class OperatorNull implements BillingListener {

	@Override
	public void setBillingHelper(BillingHelper billingHelper) {
		// TODO Auto-generated method stub
		
	}

	@Override
	public BillingHelper getBillingHelper() {
		// TODO Auto-generated method stub
		return null;
	}

	@Override
	public void onPause() {
		// TODO Auto-generated method stub
		
	}

	@Override
	public void onDestroy() {
		// TODO Auto-generated method stub
		
	}

	@Override
	public void onResume() {
		// TODO Auto-generated method stub
		
	}

	@Override
	public void onDoBilling(String productID, float money, String msg) {
		// TODO Auto-generated method stub
		
	}

	@Override
	public void onCallMoreGame() {
		// TODO Auto-generated method stub
		
	}

}
