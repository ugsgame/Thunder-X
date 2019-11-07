package com.ml;

public class FilePo {
	private String name;
	private String w = "0";
	private String h = "0";
	public String getType()
	{
		return type;
	}
	public void setType(String type)
	{
		this.type = type;
	}
	private String type = "png";
	
	public String getName() {
		return name;
	}
	public void setName(String name) {
		this.name = name;
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
}
