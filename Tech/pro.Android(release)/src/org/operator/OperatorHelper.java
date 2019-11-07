package org.operator;

import org.matrix.thunder.Matrix;

import android.app.Activity;

import com.billing.plugin.BillingHelper;
import com.billing.plugin.BillingListener;
import com.billing.plugin.OperatorEnum;

public class OperatorHelper {
		public static BillingHelper billingHelper;
		
		public static void Init(Matrix matrix)
		{
			int operator = OperatorEnum.MM;
			BillingHelper.setOperator(operator);
			
			if(operator == OperatorEnum.MM)
			{
				BillingListener billingSDK  =  new OperatorMM((Activity)matrix);
				billingHelper = new BillingHelper(billingSDK);
			}
			else if(operator == OperatorEnum.Mobile)
			{
					
			}
			else if(operator == OperatorEnum.Telecom)
			{
			
			}		
			else if(operator == OperatorEnum.Test)
			{
			
			}	
			else
			{
				
			}
			BillingHelper.setOperator(operator);
		}
		
		public static void onDoBilling(final String productID,final  String msg,final float money)
		{
			System.out.println("onCallBilling productID:"+productID);
			System.out.println("onCallBilling money:"+money);
			System.out.println("onCallBilling msg:"+msg);
			BillingHelper.onDoBilling(productID, money, msg);
		}
		
		public static void onMoreGame(String url)
		{
			System.out.println("onMoreGame url:"+url);
			BillingHelper.onCallMoreGame();
		}
		
		public static int getOperator()
		{
			System.out.println("getOperator:"+BillingHelper.getOperator());
			return BillingHelper.getOperator();
		}
}
