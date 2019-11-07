package com.ml;

import java.io.File;
import java.io.FileOutputStream;
import java.io.OutputStreamWriter;
import java.util.List;
import java.util.Stack;

public class PlistHandler {
	private static PlistHandler instance;
	private PlistHandler()
	{};
	
	private static final String d = "dict";
	private static final String k = "key";
	public static PlistHandler getinstance()
	{
		if(instance == null)
		{
			instance = new PlistHandler();
		}
		return instance;
	}
	
	public void outPlistFile(List<ImagePo> list,FilePo file)
	{
		StringBuffer root = new StringBuffer();
		root.append(getHeader());
		root.append("<plist version=\"1.0\">").append("\r\n");
		root.append("<").append(d).append(">").append("\r\n");
		
		//添加frames结点
		root.append(setTag(k,"frames"));
		root.append("<").append(d).append(">").append("\r\n");
		
		//循环添加图片
		int count = list.size();
		ImagePo img = null;
		for(int i = 0 ;i <count ;i++)
		{
			img = list.get(i);
			root.append(setTag(k, img.getName() + ".png"));
			root.append("<").append(d).append(">").append("\r\n");
			
			root.append(setTag(k, "frame"));
			root.append(setTag("string", getFrame(img)));
			
			root.append(setTag(k, "offset"));
			root.append(setTag("string", getOffSet(img)));
			
			root.append(setTag(k, "rotated"));
			root.append("<false/>\r\n");
			
			root.append(setTag(k, "sourceColorRect"));
			root.append(setTag("string", getColorRect(img)));
			
			root.append(setTag(k, "sourceSize"));
			root.append(setTag("string", getSize(img)));
			
			root.append("</").append(d).append(">").append("\r\n");
		}
		
		root.append("</").append(d).append(">").append("\r\n");
		
		
		//添加metadata数据
		root.append(setTag(k,"metadata"));
		root.append("<").append(d).append(">").append("\r\n");
		root.append(setTag(k,"format"));
		root.append(setTag("integer","2"));
		root.append(setTag(k,"realTextureFileName"));
		root.append(setTag("string",file.getName()+file.getType()));//文件名称
		root.append(setTag(k,"size"));
		root.append(setTag("string","{"+file.getW()+","+file.getH()+"}"));
		root.append(setTag(k,"textureFileName"));
		root.append(setTag("string",file.getName()+file.getType()));//文件名称
		root.append("</").append(d).append(">").append("\r\n");
		
		
		root.append("</").append(d).append(">").append("\r\n");
		root.append("</").append("plist").append(">").append("\r\n");
		
		//写文件
		File out = new File("./out/"+file.getName()+".plist");
		if(out.exists())
		{
			out.deleteOnExit();
		}
		
		try
		{
			out.createNewFile();
			
			OutputStreamWriter wt = new OutputStreamWriter(new FileOutputStream(out));
			wt.write(root.toString());
			wt.close();
			
		}
		catch(Exception e)
		{
			e.printStackTrace();
		}
	}
	
	
	private String getFrame(ImagePo img)
	{
		StringBuffer sb = new StringBuffer();
		sb.append("{{").append(img.getX()).append(",").append(img.getY()).append("},{");
		sb.append(img.getW()).append(",").append(img.getH()).append("}}");
		return sb.toString();
	}
	
	private String getColorRect(ImagePo img)
	{
		StringBuffer sb = new StringBuffer();
		sb.append("{{").append(0).append(",").append(0).append("},{");
		sb.append(img.getW()).append(",").append(img.getH()).append("}}");
		return sb.toString();
	}
	
	private String getSize(ImagePo img)
	{
		StringBuffer sb = new StringBuffer();
		sb.append("{").append(img.getW()).append(",").append(img.getH()).append("}");
		return sb.toString();
	}
	
	private String getOffSet(ImagePo img)
	{
		StringBuffer sb = new StringBuffer();
		sb.append(img.getOffset());
		return sb.toString();
	}
	
	private String setTag(String tag,String value)
	{
		StringBuffer sb = new StringBuffer();
		sb.append("<").append(tag).append(">");
		sb.append(value);
		sb.append("</").append(tag).append(">").append("\r\n");
		return sb.toString();
	}
	
	public String getHeader()
	{
		StringBuffer s = new StringBuffer("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n");
		s.append("<!DOCTYPE plist PUBLIC \"-//Apple Computer//DTD PLIST 1.0//EN\" \"http://www.apple.com/DTDs/PropertyList-1.0.dtd\">\r\n");
		return s.toString();
	}
	
}
