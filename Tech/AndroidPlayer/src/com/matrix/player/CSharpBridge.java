package com.matrix.player;

public class CSharpBridge {
	//
	private static native void nativeOnCallStaticVoidFuctionVoid(String fuc,String klass,String nameSpace);
	private static native void nativeOnCallStaticVoidFuctionInt(String fuc,String klass,String nameSpace,int parm);	
	private static native void nativeOnCallStaticVoidFuctionString(String fuc,String klass,String nameSpace,String parm);	
	private static native void nativeOnCallStaticVoidFuctionBoolean(String fuc,String klass,String nameSpace,boolean parm);
	private static native void nativeOnCallStaticVoidFuctionBooleanString(String fuc,String klass,String nameSpace,boolean parm1,String parm2);
	
	private static native int nativeOnCallStaticIntFuctionVoid(String fuc,String klass,String nameSpace);
	//
	public static void CallStaticVoidFuction(String fuc,String klass,String nameSpace)
	{
		nativeOnCallStaticVoidFuctionVoid(fuc, klass, nameSpace);
	}
	
	public static void CallStaticVoidFuction(String fuc,String klass,String nameSpace, int parm)
	{
		nativeOnCallStaticVoidFuctionInt( fuc, klass, nameSpace, parm);	
	}
	
	public static void CallStaticVoidFuction(String fuc,String klass,String nameSpace, String parm)
	{
		nativeOnCallStaticVoidFuctionString( fuc, klass, nameSpace, parm);	
	}	
	
	public static void CallStaticVoidFuction(String fuc,String klass,String nameSpace, boolean parm)
	{
		nativeOnCallStaticVoidFuctionBoolean( fuc, klass, nameSpace, parm);
	}

	public static void CallStaticVoidFuction(String fuc,String klass,String nameSpace, boolean parm1,String parm2)
	{
		nativeOnCallStaticVoidFuctionBooleanString( fuc, klass, nameSpace, parm1,parm2);
	}
	
	public static int CallStaticInitFuction(String fuc,String klass,String nameSpace)
	{
		return nativeOnCallStaticIntFuctionVoid( fuc, klass, nameSpace);
	}

}
