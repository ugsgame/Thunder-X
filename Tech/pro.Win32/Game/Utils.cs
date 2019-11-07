using LitJson;
using MatrixEngine.Math;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Thunder.Game
{
    public class Utils
    {
        /// <summary>
        /// 中心锚点
        /// </summary>
        public static Vector2 AnchorPoint_Center
        {
            get
            {
                return new Vector2(0.5f, 0.5f);
            }
        }

        /// <summary>
        /// 左上角锚点
        /// </summary>
        public static Vector2 AnchorPoint_LeftTop
        {
            get
            {
                return new Vector2(0f, 1f);
            }
        }

        /// <summary>
        /// 左下角锚点
        /// </summary>
        public static Vector2 AnchorPoint_LeftBottom
        {
            get
            {
                return new Vector2(0f, 0f);
            }
        }

        /// <summary>
        /// 左边中心锚点
        /// </summary>
        public static Vector2 AnchorPoint_LeftCenter
        {
            get
            {
                return new Vector2(0f, 0.5f);
            }
        }

        /// <summary>
        /// 右上角锚点
        /// </summary>
        public static Vector2 AnchorPoint_RightTop
        {
            get
            {
                return new Vector2(1f, 1f);
            }
        }

        /// <summary>
        /// 右下角锚点
        /// </summary>
        public static Vector2 AnchorPoint_RightBottom
        {
            get
            {
                return new Vector2(1f, 0f);
            }
        }

        /// <summary>
        /// 右边中心锚点
        /// </summary>
        public static Vector2 AnchorPoint_RightCenter
        {
            get
            {
                return new Vector2(1f, 0.5f);
            }
        }

        /// <summary>
        /// 上边中心锚点
        /// </summary>
        public static Vector2 AnchorPoint_TopCenter
        {
            get
            {
                return new Vector2(0.5f, 1f);
            }
        }

        /// <summary>
        /// 下边中心锚点
        /// </summary>
        public static Vector2 AnchorPoint_BottomCenter
        {
            get
            {
                return new Vector2(0.5f, 0f);
            }
        }

        public static string PrintStack()
        {
            StackTrace stackTrace = new StackTrace(false);
            Console.Error.WriteLine(stackTrace);
            return stackTrace.ToString();
        }

        public static void PrintException(Exception e)
        {
            if (e != null)
            {
                Console.Error.WriteLine(e);
                Console.Error.WriteLine(e.StackTrace);
            }
        }

        public static Vector2 AngleToVector(float roation)
        {
            Vector2 dir = Vector2.Zero;
            dir.X = MathHelper.Sin(roation);
            dir.Y = MathHelper.Cos(roation);
            return dir;
        }

        public static bool Follow(float tick, float speed, Vector2 pos, Vector2 aim, ref Vector2 newPos, float decay = 1f,float disVer = 1.0f)
        {
            decay = decay > 1.0f ? 1.0f : decay;
            float _speed = speed * tick;
            float dis = Vector2.Distance(pos, aim);
            float step = dis > _speed * 2 ? _speed : dis * decay;
            float rate = step / dis;
            Vector2 dir = (pos - aim) * rate;

            if (dis <= disVer)
            {
                newPos = aim;
                return true;
            }
            else
            {
                newPos -= dir;
                return false;
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

        public static bool JsonBoolean(JsonData json, string value, bool defaultValue)
        {
            bool var;
            try
            {
                var = (bool)json[value];
            }
            catch (Exception)
            {
                var = defaultValue;
            }
            return var;
        }

        public static string ConvertMapResPath(string resName)
        {
            return "Data/Map/" + resName;
        }

        public static string CoverActionScriptPath(string resName)
        {
            return "Data/ActionScript/" + resName;
        }

        public static string CoverLevelPath(string resName)
        {
            return "Data/Levels/" + resName;
        }

        public static string CoverBulletPath(string resName)
        {
            return "Data/Bullets/" + resName;
        }

        public static string[] ResNameSplit(string resName)
        {
            string[] resNames = resName.Split('-');
            if (resNames.Length > 0)
            {
                return resNames;
            }
            else
            {
                return new string[] { resName, };
            }
        }


        public static int IndexOf(StringBuilder value, string str)
        {
            return IndexOf(value, str, 0);
        }

        public static int IndexOf(StringBuilder value, string str, int fromIndex)
        {
            return IndexOf(value, 0, value.Length,
                    str, 0, str.Length, fromIndex);
        }

        public static int IndexOf(StringBuilder source, int sourceOffset, int sourceCount,
              string target, int targetOffset, int targetCount,
              int fromIndex)
        {
            if (fromIndex >= sourceCount)
            {
                return (targetCount == 0 ? sourceCount : -1);
            }
            if (fromIndex < 0)
            {
                fromIndex = 0;
            }
            if (targetCount == 0)
            {
                return fromIndex;
            }

            char first = target[targetOffset];
            int max = sourceOffset + (sourceCount - targetCount);

            for (int i = sourceOffset + fromIndex; i <= max; i++)
            {
                if (source[i] != first)
                {
                    while (++i <= max && source[i] != first) ;
                }

                if (i <= max)
                {
                    int j = i + 1;
                    int end = j + targetCount - 1;
                    for (int k = targetOffset + 1; j < end && source[j]
                            == target[k]; j++, k++) ;

                    if (j == end)
                    {
                        return i - sourceOffset;
                    }
                }
            }
            return -1;
        }

        public static int LastIndexOf(StringBuilder source, String str)
        {
            return LastIndexOf(source, str, source.Length);
        }

        public static int LastIndexOf(StringBuilder source, String str, int fromIndex)
        {
            return LastIndexOf(source, 0, source.Length,
                                  str, 0, str.Length, fromIndex);
        }

        public static int LastIndexOf(StringBuilder source, int sourceOffset, int sourceCount,
              string target, int targetOffset, int targetCount,
              int fromIndex)
        {

            int rightIndex = sourceCount - targetCount;
            if (fromIndex < 0)
            {
                return -1;
            }
            if (fromIndex > rightIndex)
            {
                fromIndex = rightIndex;
            }

            if (targetCount == 0)
            {
                return fromIndex;
            }

            int strLastIndex = targetOffset + targetCount - 1;
            char strLastChar = target[strLastIndex];
            int min = sourceOffset + targetCount - 1;
            int i = min + fromIndex;

        startSearchForLastChar:
            while (true)
            {
                while (i >= min && source[i] != strLastChar)
                {
                    i--;
                }
                if (i < min)
                {
                    return -1;
                }
                int j = i - 1;
                int start = j - (targetCount - 1);
                int k = strLastIndex - 1;

                while (j > start)
                {
                    if (source[j--] != target[k--])
                    {
                        i--;
                        goto startSearchForLastChar;
                    }
                }
                return start - sourceOffset + 1;
            }
        }

        /// <summary>
        /// 矩形判断，大小型矩形判断
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="s1"></param>
        /// <param name="p2"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        public static bool CollisionRect(Vector2 p1, Size s1, Vector2 p2, Size s2)
        {
            //先判断右边边，然后再判断上边，再到左边，最后下边，提高效率
            if ((p1.X + s1.width < p2.X) || (p1.Y + s1.height < p2.Y) || (p1.X > p2.X + s2.width) || (p1.Y > p2.Y + s2.height))
            {
                //任意一条边没有进入另一个矩形，则表示没有碰到
                return false;
            }
            return true;
        }

        /// <summary>
        /// 矩形判断，坐标型矩形判断
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="s1"></param>
        /// <param name="p2"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        public static bool CollisionRect(Vector2 p1, Vector2 s1, Vector2 p2, Vector2 s2)
        {
            ////先判断右边边，然后再判断上边，再到左边，最后下边，提高效率
            //if ((p1.X < p2.X) || (p1.Y + s1.height < p2.Y) || (p1.X > p2.X + s2.width) || (p1.Y > p2.Y + s2.height))
            //{
            //    return false;
            //}
            return true;
        }
    }
}
