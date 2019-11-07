package org.fonle.matrix.jni;

import java.io.File;

import org.fonle.matrix.Matrix;

import android.os.Environment;

public class SystemFeature {

	private static boolean sdCardExist = Environment.getExternalStorageState()
			.equals(Environment.MEDIA_MOUNTED); // 判断sd卡是否存在

	public static String GetSDPath() {
		String sdDir = null;
		if (sdCardExist) // 如果SD卡存在，则获取跟目录
		{
			sdDir = Environment.getExternalStorageDirectory().getAbsolutePath() + "/";// 获取跟目录
		}
		System.out.println("jni SDPath:"+sdDir);
		return sdDir;
	}

	public static boolean IsSDExist() {
		return sdCardExist;
	}
	
	public static String GetPackageName()
	{		
		return Matrix.share().getApplicationInfo().packageName;
	}
}
