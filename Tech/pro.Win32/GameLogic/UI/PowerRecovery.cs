using MatrixEngine.Cocos2d;
using MatrixEngine.Math;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using Thunder.GameLogic.Common;

namespace Thunder.GameLogic.UI
{
    public class PowerRecovery
    {
        private readonly int timeOfRecovery = 20;   // 一个体力的回复时间/分钟
        private readonly int timeOfSecond = 60;

        private int numOfMinutes;
        private int numOfSeconds;

        private int minutesOfDisplay;
        public int Minutes
        {
            get { return minutesOfDisplay; }
        }
        private int secondsOfDisplay;
        public int Seconds
        {
            get { return secondsOfDisplay; }
        }

        private TimeSpan oldTimeSpan;
        private TimeSpan curTimeSpan;
        bool flagStart;

        string dataPath;

        public static PowerRecovery Instance;

        public PowerRecovery()
        {
            dataPath = CCFileUtils.GetWritablePath() + "/ts.dat";

            numOfMinutes = timeOfRecovery;
            numOfSeconds = timeOfSecond;

            //TODO:读取上一次贮存的时间,计算出所得的计力
            //oldTimeSpan = new TimeSpan(DateTime.Now.Ticks);
            if (CCFileUtils.IsFileExist(dataPath))
            {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(dataPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                oldTimeSpan = (TimeSpan)formatter.Deserialize(stream);
                stream.Close(); 
            }
            else
            {
                oldTimeSpan = new TimeSpan(DateTime.Now.Ticks);
            }
            //
            PowerTimeSpan();
            //
            this.flagStart = true;
            Thread t = new Thread(this.OnUpdate);
            t.Start();

            Instance = this;
        }

        public void OnExit()
        {
            //TODO:保存最后的时刻
            oldTimeSpan = new TimeSpan(DateTime.Now.Ticks);
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(dataPath, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, oldTimeSpan);
            stream.Close(); 
            //
            this.flagStart = false;
        }

        protected virtual void PowerTimeSpan()
        {
            curTimeSpan = new TimeSpan(DateTime.Now.Ticks);
            if (GameData.Instance.PlayerData.power >= GameData.Instance.PowerRecoveryCount)
            {
                this.OnResetTimeCounter();
            }
            else
            {
                TimeSpan times = curTimeSpan - oldTimeSpan;
                double totalSeconds = times.TotalSeconds;
                double totalMiutes = times.TotalMinutes;

                Console.WriteLine("oldTimeSpan totalSeconds:" + oldTimeSpan.TotalSeconds);
                Console.WriteLine("oldTimeSpan totalMiutes:" + oldTimeSpan.TotalMinutes);

                Console.WriteLine("curTimeSpan totalSeconds:" + curTimeSpan.TotalSeconds);
                Console.WriteLine("curTimeSpan totalMiutes:" + curTimeSpan.TotalMinutes);

                Console.WriteLine("totalSeconds:" + totalSeconds);
                Console.WriteLine("totalMiutes:" + totalMiutes);

                float ceilTotalMiutes = MathHelper.Floor((float)totalMiutes);
                int totalPower = (int)(ceilTotalMiutes / timeOfRecovery);

                if (totalPower > GameData.Instance.PowerRecoveryCount)
                {
                    GameData.Instance.PlayerData.power = GameData.Instance.PowerRecoveryCount;
                    this.OnResetTimeCounter();
                }
                else if (totalPower + GameData.Instance.PlayerData.power > GameData.Instance.PowerRecoveryCount)
                {
                    GameData.Instance.PlayerData.power = GameData.Instance.PowerRecoveryCount;
                    this.OnResetTimeCounter();
                }
                else
                {
                    GameData.Instance.PlayerData.power += totalPower;
                    numOfMinutes = timeOfRecovery - (int)ceilTotalMiutes % timeOfRecovery - 1;
                    numOfSeconds = timeOfSecond - (int)totalSeconds % timeOfSecond;
                }
            }
        }

        public void OnResetTimeCounter()
        {
            numOfMinutes = timeOfRecovery - 1;
            numOfSeconds = timeOfSecond;

            minutesOfDisplay = 0;
            secondsOfDisplay = 0;
        }

        /// <summary>
        /// 一秒调一点
        /// </summary>
        protected virtual void OnUpdate()
        {
            while (flagStart)
            {
                if (GameData.Instance.PlayerData.power < GameData.Instance.PowerRecoveryCount)
                {
                    numOfSeconds--;
                    if (numOfSeconds < 0)
                    {
                        numOfMinutes--;
                        numOfSeconds = timeOfSecond;
                        if (numOfMinutes < 0)
                        {
                            numOfMinutes = timeOfRecovery - 1;
                            GameData.Instance.PlayerData.power++;
                            if (GameData.Instance.PlayerData.power == GameData.Instance.PowerRecoveryCount)
                            {
                                numOfMinutes = 0;
                                numOfSeconds = 0;
                            }
                        }
                    }

                    minutesOfDisplay = numOfMinutes;
                    secondsOfDisplay = numOfSeconds;
                }
                else
                {
                    this.OnResetTimeCounter();
                }

                System.Threading.Thread.Sleep(1000);
            }

        }
    }
}
