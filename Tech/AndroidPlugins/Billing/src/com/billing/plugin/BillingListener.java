package com.billing.plugin;
/**
 * 
 * @author Administrator
 * 计费接口回调
 */

public interface BillingListener {	
	
	void setBillingHelper(BillingHelper billingHelper);
	BillingHelper getBillingHelper();
	
	void onPause();
	void onDestroy();
	void onResume();
	
	void onDoBilling(String productID, float money , String msg);
	//更多游戏
	void onCallMoreGame();
}
