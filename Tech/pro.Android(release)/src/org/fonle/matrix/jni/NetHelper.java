package org.fonle.matrix.jni;

import org.matrix.thunder.Matrix;

import android.content.Intent;
import android.net.Uri;

public class NetHelper {

	public static void OpenUrl(String url) {
		Uri uri = Uri.parse(url);
		Intent it = new Intent(Intent.ACTION_VIEW, uri);
		Matrix.share().startActivity(it);
		it = null;
	}
	
	public static void JniTest(int i_0,int i_1,int i_2,boolean bool,float float_)
	{
		System.out.println("str_1:"+i_0);
		System.out.println("str_2:"+i_1);
		System.out.println("int_:"+i_2);
		System.out.println("bool:"+bool);
		System.out.println("float:"+float_);
	}
}
