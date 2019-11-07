package com.billing.plugin;
import com.matrix.player.*;
import android.opengl.GLSurfaceView;
import com.billing.plugin.BillingListener;

/**
 * 
 * @author Administrator
 * 计费插件
 *
 */
public class BillingHelper 
{	
	static int BillingOperator = OperatorEnum.Test;
	static BillingListener billingSDK = null;
	
	static int OperatorID = OperatorEnum.Test;
	
	public  BillingHelper(BillingListener billing)
	{
		billingSDK = billing;
		billingSDK.setBillingHelper(this);
	}
	public  void setGLSurfaceView(GLSurfaceView surfaceView)
	{

	}

	public  void onPause()
	{
		billingSDK.onPause();
	}
	
	public void onResume()
	{
		billingSDK.onResume();
	}

	public  void  onDestroy()
	{
		billingSDK.onDestroy();
	}
	public static void onDoBilling(String productID, float money , String msg)
	{
		billingSDK.onDoBilling(productID, money, msg);
	}
	
	public static void onCallMoreGame()
	{
		billingSDK.onCallMoreGame();
	}	
	
	public static int getOperator()
	{
		return OperatorID;
	}
	
	public static void setOperator(int id)
	{
		OperatorID = id;
	}
	
	public void onBillingSuccess(String productID, String msg) {
		CSharpBridge.CallStaticVoidFuction("OnNativeBilling", "BillingHelper", "Thunder.GameBilling",true, productID);
	}

	public void onBillingFail(String productID, String msg) {
		CSharpBridge.CallStaticVoidFuction("OnNativeBilling", "BillingHelper", "Thunder.GameBilling",false, productID);
	}	
}
