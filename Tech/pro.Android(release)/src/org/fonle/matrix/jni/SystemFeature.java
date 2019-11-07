package org.fonle.matrix.jni;

import java.io.File;

import org.matrix.thunder.Matrix;

import android.os.Environment;

public class SystemFeature {

	private static boolean sdCardExist = Environment.getExternalStorageState()
			.equals(Environment.MEDIA_MOUNTED); // 

	public static String GetSDPath() {
		String sdDir = null;
		if (sdCardExist) //
		{
			sdDir = Environment.getExternalStorageDirectory().getAbsolutePath() + "/";// 
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
