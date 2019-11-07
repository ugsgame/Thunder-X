package org.operator;

import java.util.HashMap;

import android.app.Activity;

import com.billing.plugin.BillingHelper;
import com.billing.plugin.BillingListener;

import mm.purchasesdk.OnPurchaseListener;
import mm.purchasesdk.Purchase;
import mm.purchasesdk.PurchaseCode;
import mm.purchasesdk.PurchaseSkin;

public class OperatorMM implements BillingListener , OnPurchaseListener  {

	Activity mActivity;
	BillingHelper billingHelper;
	//
	public static Purchase purchase;
	// 计费信息 (现网环境)
	private static final String APPID = "300008987543";
	private static final String APPKEY = "D8B14C025C646A4A391D1C156225416A";
	// 计费点信息
	String gameBillingID;
	// 价格
	float      money;	
	
	public OperatorMM(Activity activity)
	{
		mActivity = activity;
		
		purchase = Purchase.getInstance();
		purchase.setAppInfo(APPID, APPKEY,PurchaseSkin.SKIN_SYSTEM_TWO);
		purchase.init(mActivity, this);	
		
	}
	
	@Override
	public void onBillingFinish(int code, HashMap arg1) {
		String result = "订购结果：订购成功";
		// 此次订购的orderID
		String orderID = null;
		// 商品的paycode
		String paycode = null;
		// 商品的有效期(仅租赁类型商品有效)
		String leftday = null;
		// 商品的交易 ID，用户可以根据这个交易ID，查询商品是否已经交易
		String tradeID = null;
		
		String ordertype = null;
		if (code == PurchaseCode.ORDER_OK || (code == PurchaseCode.AUTH_OK)||(code == PurchaseCode.WEAK_ORDER_OK)) {
			/**
			 * 商品购买成功或者已经购买。 此时会返回商品的paycode，orderID,以及剩余时间(租赁类型商品)
			 */
			if (arg1 != null) {
				leftday = (String) arg1.get(OnPurchaseListener.LEFTDAY);
				if (leftday != null && leftday.trim().length() != 0) {
					result = result + ",剩余时间 ： " + leftday;
				}
				orderID = (String) arg1.get(OnPurchaseListener.ORDERID);
				if (orderID != null && orderID.trim().length() != 0) {
					result = result + ",OrderID ： " + orderID;
				}
				paycode = (String) arg1.get(OnPurchaseListener.PAYCODE);
				if (paycode != null && paycode.trim().length() != 0) {
					result = result + ",Paycode:" + paycode;
				}
				tradeID = (String) arg1.get(OnPurchaseListener.TRADEID);
				if (tradeID != null && tradeID.trim().length() != 0) {
					result = result + ",tradeID:" + tradeID;
				}
				ordertype = (String) arg1.get(OnPurchaseListener.ORDERTYPE);
				if (tradeID != null && tradeID.trim().length() != 0) {
					result = result + ",ORDERTYPE:" + ordertype;
				}
			}			
			billingHelper.onBillingSuccess(gameBillingID, result);
			
		} else {
			/**
			 * 表示订购失败。
			 */
			result = "订购结果：" + Purchase.getReason(code);
			billingHelper.onBillingFail(gameBillingID, result);
		}
		System.out.println(result);		
	}

	@Override
	public void onDoBilling(String productID, float money, String msg) {
		this.money = money;
		final String _msg = "购买 "+msg;
		gameBillingID = productID;
		final String sdkBillingID = productID;
	
		mActivity.runOnUiThread(new Runnable() {
			
			@Override
			public void run() {
				// TODO Auto-generated method stub
				purchase.order(mActivity, sdkBillingID, OperatorMM.this);
			}
		});	
	}

	@Override
	public void onCallMoreGame() {
		// TODO Auto-generated method stub
		
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
	public void setBillingHelper(BillingHelper billingHelper) {
		this.billingHelper = billingHelper;
	}

	@Override
	public BillingHelper getBillingHelper() {
		return this.billingHelper;
	}

	@Override
	public void onAfterApply() {
		// TODO Auto-generated method stub
		
	}

	@Override
	public void onAfterDownload() {
		// TODO Auto-generated method stub
		
	}

	@Override
	public void onBeforeApply() {
		// TODO Auto-generated method stub
		
	}

	@Override
	public void onBeforeDownload() {
		// TODO Auto-generated method stub
		
	}
	
	@Override
	public void onInitFinish(int arg0) {
		// TODO Auto-generated method stub
		
	}

	@Override
	public void onQueryFinish(int arg0, HashMap arg1) {
		// TODO Auto-generated method stub
		
	}

	@Override
	public void onUnsubscribeFinish(int arg0) {
		// TODO Auto-generated method stub
		
	}
}
