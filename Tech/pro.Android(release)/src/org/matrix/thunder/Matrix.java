/****************************************************************************
Copyright (c) 2010-2012 cocos2d-x.org

http://www.cocos2d-x.org

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
****************************************************************************/
package org.matrix.thunder;

import java.io.DataOutputStream;
import java.io.File;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;

import org.cocos2dx.lib.Cocos2dxActivity;
import org.fonle.matrix.jni.NetHelper;
import org.operator.OperatorHelper;
import org.operator.OperatorMM;

import android.content.Context;
import android.content.pm.ApplicationInfo;
import android.content.res.AssetManager;
import android.os.Bundle;
import android.util.Log;

public class Matrix extends Cocos2dxActivity{

	private static Matrix handle;

	protected void onCreate(Bundle savedInstanceState){
		super.onCreate(savedInstanceState);
		
		ApplicationInfo info = this.getApplicationInfo();
		String packageName = info.packageName;
		
		copyAllResFiles(this,"ScriptAssemblies","/data/data/"+packageName);
		copyAllResFiles(this,"SystemAssemblies","/data/data/"+packageName);
		
		handle = this;
		OperatorHelper.Init(this);
	}
	
	@Override
	protected void onPause()
	{
		super.onPause();
		OperatorHelper.billingHelper.onPause();
	}
	
	@Override
	protected void onResume()
	{
		super.onResume();
		OperatorHelper.billingHelper.onResume();
	}	
	
	@Override
	protected void onRestart() {
		// TODO Auto-generated method stub
		super.onRestart();
		System.out.println("Matrix onRestart");
	}	
	
	public void onDestory() 
	{
		OperatorHelper.billingHelper.onDestroy();
		super.onDestroy();
	}
	
	public static Matrix share()
	{
		return handle;
	}
	
	private  void copyAllResFiles(final Context context,String Data,String Dest)
	{
		AssetManager assmgr = context.getAssets();

		String[] files;
		try {
			files = assmgr.list(Data);

			for(int i = 0;i<files.length;i++)
			{
				files[i] = Data+File.separator+files[i];
				
				
				int index = files[i].lastIndexOf(".");
				if(index>1)
				{
					copyAssetFile(context,files[i],Dest);
				}
				else
				{
					copyAllResFiles(context,files[i],Dest);
				}
				
			}
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		Log.d( "copy", "copy end");
	}	
	private  void copyAssetFile(final Context context,String assetName, String target)
	{
		try
		{		
			int removeFrist = assetName.indexOf("/");
			String myassetName = assetName.substring(removeFrist);
			String desFileName = target+myassetName;
			int desFileDirStart = desFileName.lastIndexOf("/");
			String desFileDir = desFileName.substring(0,desFileDirStart);
			File desDir = new File(desFileDir);
			if(!desDir.exists())
			{
				if(!desDir.mkdirs())
				{
					Log.d( "mkdirsfailed", desFileDir);
				}
			}
	
			InputStream in = context.getAssets().open(assetName);
			File desFile = new File(desFileName);
			if(!desFile.exists())
			{
				desFile.createNewFile();	
			}
			
			FileOutputStream stream = new FileOutputStream(desFile);
			DataOutputStream fos    = new DataOutputStream(stream);
			
			byte[] buff = new byte[4096];
			int n = in.read(buff);
			
			while(n>0)
			{
				fos.write(buff,0,n);
				n = in.read(buff);
			}
			fos.close();
			in.close();		
			
		}
		catch(Exception e)
		{	
			e.printStackTrace();
			
			Log.d( "copy", e.getMessage());
			Log.d( "copy", e.toString());
		}
	
	}	
	
	static {
         System.loadLibrary("matrix");
    }
}
