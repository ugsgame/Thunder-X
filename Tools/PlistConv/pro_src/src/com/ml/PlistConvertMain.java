package com.ml;

import java.awt.image.BufferedImage;
import java.io.BufferedReader;
import java.io.File;
import java.io.FileInputStream;
import java.io.InputStreamReader;
import java.util.ArrayList;
import java.util.List;

import javax.imageio.ImageIO;

public class PlistConvertMain
{

	/**
	 * @param args
	 */
	public static void main(String[] args)
	{

		try
		{

			File ff = new File("./pack");
			if (!ff.exists())
			{
				ff.mkdir();
				ff = new File("./out");
				ff.mkdir();
				return;
			}

			File filelist[] = ff.listFiles();
			int l = filelist.length;
			List<String> packList = new ArrayList<String>();
			for (int i = 0; i < l; i++)
			{
				if (filelist[i].getName().endsWith(".pack"))
				{
					int index = filelist[i].getName().lastIndexOf(".");
					packList.add(filelist[i].getName().substring(0, index));
				}
			}

			if (packList.size() == 0)
			{
				return;
			}

			// 清空out目录
			File out = new File("./out/");
			File outlist[] = out.listFiles();
			for (int i = 0; i < outlist.length; i++)
			{
				outlist[i].delete();
			}

			// 开始生产
			FilePo fp = null;
			File imgf = null;
			File file = null;
			for (int i = 0; i < packList.size(); i++)
			{
				// 获取文件
				fp = new FilePo();
				fp.setName(packList.get(i));
				imgf = new File("./pack/" + packList.get(i) + ".png");
				if (!imgf.exists())
				{
					imgf = new File("./pack/" + packList.get(i) + ".jpg");
					fp.setType("jpg");
				}
				if (!imgf.exists())
				{
					continue;
				}
				BufferedImage Bi = ImageIO.read(imgf);
				fp.setW(Bi.getWidth() + "");
				fp.setH(Bi.getHeight() + "");

				// 开始转换
				file = new File("./pack/" + packList.get(i) + ".pack");
				BufferedReader br = new BufferedReader(new InputStreamReader(
						new FileInputStream(file)));
				String c = null;

				// 解析.pack文件
				List<ImagePo> list = new ArrayList<ImagePo>();
				ImagePo temp = null;
				int index = 0;
				while (true)
				{
					if ((c = br.readLine()) != null)
					{
						if(c.contains(".png") || c.contains(".jpg"))
						{
							if(index > 0)
							{
								// 输出前一个图片
								PlistHandler.getinstance().outPlistFile(list, fp);
								list.clear();
								
								//输出第二个图片
								String str[] = c.split("\\.");
								String name = str[0].trim();
								String type = fp.getType();
								fp = new FilePo();
								fp.setName(name);
								imgf = new File("./pack/" + c.trim());
								Bi = ImageIO.read(imgf);
								fp.setW(Bi.getWidth() + "");
								fp.setH(Bi.getHeight() + "");
								fp.setType(type);
							}
							index ++;
						}
						else if (!c.contains(":") && !c.contains(".png")&& !c.contains(".jpg")
								&& !"".equals(c))
						{
							temp = new ImagePo();
							temp.setName(c);
							list.add(temp);
						} else if (temp != null)
						{
							temp.parseValue(c);
						}
					} else
					{
						break;
					}
				}
				br.close();

				// 拼接pList文件
				PlistHandler.getinstance().outPlistFile(list, fp);
			}

		} catch (Exception e)
		{
			e.printStackTrace();
		}

	}

}
