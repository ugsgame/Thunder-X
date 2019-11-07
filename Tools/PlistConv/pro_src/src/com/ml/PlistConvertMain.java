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

			// ���outĿ¼
			File out = new File("./out/");
			File outlist[] = out.listFiles();
			for (int i = 0; i < outlist.length; i++)
			{
				outlist[i].delete();
			}

			// ��ʼ����
			FilePo fp = null;
			File imgf = null;
			File file = null;
			for (int i = 0; i < packList.size(); i++)
			{
				// ��ȡ�ļ�
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

				// ��ʼת��
				file = new File("./pack/" + packList.get(i) + ".pack");
				BufferedReader br = new BufferedReader(new InputStreamReader(
						new FileInputStream(file)));
				String c = null;

				// ����.pack�ļ�
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
								// ���ǰһ��ͼƬ
								PlistHandler.getinstance().outPlistFile(list, fp);
								list.clear();
								
								//����ڶ���ͼƬ
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

				// ƴ��pList�ļ�
				PlistHandler.getinstance().outPlistFile(list, fp);
			}

		} catch (Exception e)
		{
			e.printStackTrace();
		}

	}

}
