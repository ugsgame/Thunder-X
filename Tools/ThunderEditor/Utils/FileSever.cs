
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using LitJson;

namespace ThunderEditor.Utils
{
    public class FileSever
    {
        public static string ReadFileToString(string filePath)
        {
            FileStream fs = new FileStream(filePath, FileMode.Open);
            StreamReader m_streamReader = new StreamReader(fs);
            m_streamReader.BaseStream.Seek(0, SeekOrigin.Begin);
            string arry = "";
            string strLine = m_streamReader.ReadLine();
            do
            {
                arry += strLine + "\n";
                strLine = m_streamReader.ReadLine();

            } while (strLine != null && strLine != "");
            m_streamReader.Close();
            m_streamReader.Dispose();
            fs.Close(); 
            fs.Dispose();
            return arry;
        }

        public static void WriteStringToFile(string filePath,string str)
        {
            FileStream fs = new FileStream(filePath, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            //开始写入
            sw.Write(str);
            //清空缓冲区
            sw.Flush();
            //关闭流
            sw.Close();
            fs.Close();
        }


        /// <summary>
        /// 查找指定文件夹下指定后缀名的文件
        /// </summary>
        /// <param name="directory">文件夹</param> 
        /// <param name="pattern">后缀名</param> 
        /// <returns>文件路径</returns>
        public static List<string> GetDirectoryFiles(DirectoryInfo directory, string pattern)
        {
            List<string> result = new List<string>();
            if (directory.Exists || pattern.Trim() != string.Empty)
            {
                try
                {
                    foreach (FileInfo info in directory.GetFiles(pattern))
                    {
                        result.Add(info.FullName.ToString());
                        //num++;
                    }
                }
                catch { }
                foreach (DirectoryInfo info in directory.GetDirectories())
                {
                    GetDirectoryFiles(info, pattern);
                }

            }
            return result;
        }

        /// <summary>
        /// 获取文件名称
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public static string GetFileName(string path)
        {
            if (path.Contains("\\"))
            {
                string[] arr = path.Split('\\');
                return arr[arr.Length - 1];
            }
            else
            {
                string[] arr = path.Split('/');
                return arr[arr.Length - 1];
            }
        }

        public static float JsonNumber(JsonData json, string value)
        {
            float var;
            try
            {
                var = (int)json[value];
            }
            catch (Exception)
            {
                try
                {
                    var = (float)(double)json[value];
                }
                catch (Exception e2)
                {
                    throw e2;
                }
            }
            return var;
        }

        public static float JsonNumber(JsonData json, string value, float defaultValue)
        {
            float var;
            try
            {
                var = JsonNumber(json, value);
            }
            catch (Exception)
            {
                var = defaultValue;
            }
            return var;
        }

        public static string JsonSctring(JsonData json, string value, string defaultValue)
        {
            string var;
            try
            {
                var = (string)json[value];
            }
            catch (Exception)
            {
                var = defaultValue;
            }
            return var;
        }
    }
}
