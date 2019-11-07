package com.ml;

public class ImagePo {
	
	private String name;
	private boolean rotate;
	private String x;
	private String y;
	private String w;
	private String h;
	private String orig;
	private String offset = "{0,0}";
	private int index;
	
	public void parseValue(String value)
	{
		String t[] = value.split(":");
		if(t.length != 2)
		{
			return;
		}
		
		if(t[0].contains("xy"))
		{
			String i[] = t[1].split(",");
			x = i[0].trim();
			y = i[1].trim();
		}
		else if(t[0].contains("size"))
		{
			String i[] = t[1].split(",");
			w = i[0].trim();
			h = i[1].trim();
		}
	}
	
	public String getName() {
		return name;
	}
	public void setName(String name) {
		this.name = name;
	}
	public boolean isRotate() {
		return rotate;
	}
	public void setRotate(boolean rotate) {
		this.rotate = rotate;
	}
	public String getX() {
		return x;
	}

	public void setX(String x) {
		this.x = x;
	}

	public String getY() {
		return y;
	}

	public void setY(String y) {
		this.y = y;
	}

	public String getW() {
		return w;
	}

	public void setW(String w) {
		this.w = w;
	}

	public String getH() {
		return h;
	}

	public void setH(String h) {
		this.h = h;
	}

	public String getOrig() {
		return orig;
	}
	public void setOrig(String orig) {
		this.orig = orig;
	}
	public String getOffset() {
		return offset;
	}
	public void setOffset(String offset) {
		this.offset = offset;
	}
	public int getIndex() {
		return index;
	}
	public void setIndex(int index) {
		this.index = index;
	}
	
}
