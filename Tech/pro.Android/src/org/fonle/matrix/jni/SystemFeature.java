package org.fonle.matrix.jni;

import java.io.File;

import org.fonle.matrix.Matrix;

import android.os.Environment;

public class SystemFeature {

	private static boolean sdCardExist = Environment.getExternalStorageState()
			.equals(Environment.MEDIA_MOUNTED); // �ж�sd���Ƿ����

	public static String GetSDPath() {
		String sdDir = null;
		if (sdCardExist) // ���SD�����ڣ����ȡ��Ŀ¼
		{
			sdDir = Environment.getExternalStorageDirectory().getAbsolutePath() + "/";// ��ȡ��Ŀ¼
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
