using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UniLua;
using System.IO;
using System.Text.RegularExpressions;
using MatrixEngine.Cocos2d;

namespace Thunder.GameLogic.Gaming.ScripSystem
{
    /// <summary>
    /// 脚本环境相关
    /// </summary>
    public class ScriptHelper
    {
        private ILuaState Lua;

        public ScriptHelper()
        {
        }

        ~ScriptHelper()
        {
            Lua = null;
            //Console.WriteLine("~ScriptHelper");
        }

        public ScriptHelper(string script)
        {
            DoString(script);
        }

        public ILuaState L
        {
            get
            {
                if (Lua == null)
                {
                    try
                    {
                        Lua = LuaAPI.NewState();
                        Lua.L_OpenLibs();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                }
                return Lua;
            }
        }

        public DumpStatus Dump(byte[] bytes, int start, int length)
        {
            Console.WriteLine(bytes);
            return DumpStatus.OK;
        }

        public bool DoFile(string filePath)
        {
            string strTxt = "";
            try
            {
                StreamReader txtStreamReader = new StreamReader(filePath);
                strTxt = txtStreamReader.ReadToEnd();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
            return DoString(strTxt);
        }

        public bool DoString(string script)
        {
            try
            {
                string result = Regex.Replace(script, "[\t]*--[^\n]*\n", "");
                var status = L.L_DoString(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
            return true;
        }
        /// <summary>
        /// 调一个lua的全局函数
        /// </summary>
        /// <param name="fName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public object[] CallFuction(string fName, params object[] args)
        {
            try
            {
                return CallFuction(L, fName, args);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return null;
        }

        public object[] CallFuction(ILuaState l, string fName, params object[] args)
        {

            l.GetGlobal(fName);

            if (!l.IsFunction(-1))
            {
                //Console.WriteLine(fName + " is not a funciton!!");
                throw new Exception(string.Format("function not found：" + fName, fName));
            }
            //Console.WriteLine("args:" + args.Length);

            for (int i = 0; i < args.Length; i++)
            {
                var param = args[i];
                if (param is int)
                {
                    l.PushInteger((int)param);
                }
                else if (param is float || param is double)
                {
                    float t = (float)param;
                    l.PushNumber((double)t);
                }
                else if (param is string)
                {
                    l.PushString((string)param);
                }
                else if (param is bool)
                {
                    l.PushBoolean((bool)param);
                }
                else if (param is CCObject)
                {
                    l.PushLightUserData((CCObject)param);
                }
                else
                {
                    throw new Exception(string.Format("can't set this param!", fName));
                }
            }
            //调用函数
            l.Call(args.Length, 0);
            //TODO:返回函数返回值列表
            return null;
        }

        /// <summary>
        /// 设置值到Lua全局变量
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="valueName"></param>
        /// <param name="val"></param>
        public void FileldValue<T>(string valueName, T val)
        {
            FileldValue(L, valueName, val);
        }
        /// <summary>
        /// 设置值到Lua表变量
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="l"></param>
        /// <param name="valueName"></param>
        /// <param name="val"></param>
        public void FileldValue<T>(ILuaState l, string valueName, T val)
        {
            var value = val;
            object t = (object)val;
            //TODO:把值邦定到变量中
            if (value is int)
            {
                l.PushInteger((int)t);
                l.SetGlobal(valueName);
            }
            else if (value is float || value is double)
            {
                l.PushNumber((double)t);
                l.SetGlobal(valueName);
            }
            else if (value is string)
            {
                l.PushString((string)t);
                l.SetGlobal(valueName);
            }
            else if (value is bool)
            {
                l.PushBoolean((bool)t);
                l.SetGlobal(valueName);
            }
            else if (value is CCObject)
            {
                l.PushLightUserData(value);
                l.SetGlobal(valueName);
            }
            else
            {
                throw new Exception(string.Format("can't set this type fileld!", valueName));
            }
        }
        //         public ILuaState CreateMetatable(string tableName)
        //         {
        //             
        //         }
        //         public ILuaState CreateMetatable(ILuaState l, string tableName)
        //         {
        // 
        //         }
    }
}
