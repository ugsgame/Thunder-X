using MatrixEngine.CocosDenshion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thunder.Common;

namespace Thunder.GameLogic.Common
{
    public class GameAudio
    {
        /// <summary>
        /// mp3
        /// </summary>
        public enum Music
        {
            Null,
            boss_bg,
            combat_bg,
            menu_bg,
            Count,
        }

        /// <summary>
        /// Win32 wav
        /// Android ogg
        /// </summary>
        public enum Effect
        {
            Null,
            boss_await,
            boss_change,
            button,
            crystal,           
            die_boss,
            die_elite,
            die_little,
            intensify,
            loading,
            play_enter,
            play_expedite,
            play_fury,
            play_protect,
            play_seckill,
            back,
            upgrade,
            win,
            resume,
            boxopen,
            score_roll,
            xingxing,
            Count,
        }

        //
        private struct EffectData
        {
            public int id;
            public string fileName;

        };

        public static Effect CurEffect = Effect.Null;
        public static Music CurMusic = Music.Null;

        //
        private static string MusicPath = "Data/Sounds/Music/";
        private static string efffectPath = "Data/Sounds/Effects/";

        private static Dictionary<Music, string> MusicPool = new Dictionary<Music, string>(4);
        private static Dictionary<Effect, EffectData> EffectPool = new Dictionary<Effect, EffectData>(8);

        //
        public static bool IsInit;

        public static void Init()
        {
            if (IsInit) return;

            string mSuffix = ".mp3";
            string eSuffix = ".wav";


            //music
            if (Config.TARGET_PLATFORM == MatrixEngine.Engine.System.PLATFORM.BLACKBERRY || Config.TARGET_PLATFORM == MatrixEngine.Engine.System.PLATFORM.LINUX)
                mSuffix = ".ogg";
            else if (Config.TARGET_PLATFORM == MatrixEngine.Engine.System.PLATFORM.WP8)
                mSuffix = ".wav";
            else
                mSuffix = ".mp3";
            //effect 

            if (Config.TARGET_PLATFORM == MatrixEngine.Engine.System.PLATFORM.MARMALADE)
            {
                eSuffix = ".raw";
                efffectPath += "raw/";
            }
            else if (Config.TARGET_PLATFORM == MatrixEngine.Engine.System.PLATFORM.ANDROID)
            {
                eSuffix = ".ogg";
                efffectPath += "ogg/";
            }
            else
            {
                eSuffix = ".wav";
                efffectPath += "wav/";
            }

            //
            Music[] musics = (Music[])Enum.GetValues(typeof(Music));
            for (int i = 0; i < musics.Length; i++)
            {
                if (i == 0) continue;
                var enumValue = musics[i];
                MusicPool[enumValue] = MusicPath + enumValue.ToString() + mSuffix;
            }

            //
            EffectData effectData = new EffectData();

            Effect[] effects = (Effect[])Enum.GetValues(typeof(Effect));
            for (int i = 0; i < effects.Length; i++)
            {
                if (i == 0) continue;
                var enumValue = effects[i];
                effectData.fileName = efffectPath + enumValue.ToString() + eSuffix;
                EffectPool[enumValue] = effectData;
            }

            IsInit = true;
        }

        public static void PreloadAll()
        {
            foreach (var item in MusicPool)
            {
                PreloadMusic(item.Key);
            }
            foreach (var item in EffectPool)
            {
                PreloadEffect(item.Key);
            }
        }

        public static void PreloadMusic(Music music)
        {
            AudioEngine.PreloadBackgroundMusic(MusicPool[music]);
        }

        public static void PreloadEffect(Effect effect)
        {
            AudioEngine.PreloadEffect(EffectPool[effect].fileName);
        }

        public static void UnloadAll()
        {
            StopMusic(true);

            foreach (var item in EffectPool)
            {
                UnloadeEffect(item.Key);
            }
        }

        public static void UnloadeEffect(Effect effect)
        {
            AudioEngine.UnloadEffect(EffectPool[effect].fileName);
        }

        public static void PlayMusic(Music music)
        {
            CurMusic = music;
            AudioEngine.PlayBackgroundMusic(MusicPool[music]);
        }

        public static void PlayMusic(Music music, bool loop)
        {
            CurMusic = music;
            AudioEngine.PlayBackgroundMusic(MusicPool[music], loop);
        }

        public static void PlayEffect(Effect effect)
        {
            CurEffect = effect;
            EffectData data = EffectPool[effect];
            data.id = AudioEngine.PlayEffect(data.fileName);
        }

        public static void PlayEffect(Effect effect, bool loop)
        {
            CurEffect = effect;
            EffectData data = EffectPool[effect];
            data.id = AudioEngine.PlayEffect(data.fileName, loop);
        }

        public static void StopMusic()
        {
            AudioEngine.StopBackgroundMusic();
        }

        public static void StopMusic(bool release)
        {
            AudioEngine.StopBackgroundMusic(release);
        }

        public static void StopEffect(Effect effect)
        {
            if (EffectPool[effect].id == -1)
            {
                throw new NullReferenceException("音效" + EffectPool[effect].fileName + "未播放过!");
            }
            else
            {
                AudioEngine.StopEffect(EffectPool[effect].id);
            }
        }

        public static void StopAll()
        {
            AudioEngine.StopBackgroundMusic();
            AudioEngine.StopAllEffects();
        }

        public static void PauseMusic()
        {
            AudioEngine.PauseBackgroundMusic();
        }

        public static void PauseEffect(Effect effect)
        {
            if (EffectPool[effect].id == -1)
            {
                throw new NullReferenceException("音效" + EffectPool[effect].fileName + "未播放过!");
            }
            else
            {
                AudioEngine.PauseEffect(EffectPool[effect].id);
            }
        }

        public static void PauseAll()
        {
            AudioEngine.PauseBackgroundMusic();
            AudioEngine.PauseAllEffects();
        }

        public static void ResumeMusic()
        {
            AudioEngine.ResumeBackgroundMusic();
        }

        public static void ResumeEffect(Effect effect)
        {
            if (EffectPool[effect].id == -1)
            {
                throw new NullReferenceException("音效" + EffectPool[effect].fileName + "未播放过!");
            }
            else
            {
                AudioEngine.ResumeEffect(EffectPool[effect].id);
            }
        }

        public static void ResumeAll()
        {
            AudioEngine.ResumeBackgroundMusic();
            AudioEngine.ResumeAllEffects();
        }

        public static void RewindMusic()
        {
            AudioEngine.RewindBackgroundMusic();
        }

        public static bool IsMusicPlaying()
        {
            return AudioEngine.IsBackgroundMusicPlaying();
        }

        private static bool isEnable = true;
        public static bool IsEnable
        {
            get { return isEnable; }
            set
            {
                isEnable = value;
                if (value)
                {
                    AudioEngine.EffectsVolume = effectVolume;
                    AudioEngine.BackgroundMusicVolume = musicVolume;
                    AudioEngine.RewindBackgroundMusic();
                }
                else
                {
                    AudioEngine.EffectsVolume = 0;
                    AudioEngine.BackgroundMusicVolume = 0;
                    AudioEngine.StopAllEffects();
                    AudioEngine.StopBackgroundMusic();
                }
                
            }
        }

        private static bool isEffectEnable = true;
        public  static bool IsEffectEnable
        {
            get { return isEffectEnable; }
            set 
            {
                isEffectEnable = value;
                if (value)
                {
                    AudioEngine.EffectsVolume = effectVolume;
                }
                else
                {
                    AudioEngine.EffectsVolume = 0;
                    AudioEngine.StopAllEffects();
                }
            }
        }

        private static bool isMusicEnable = true;
        public static bool IsMusicEnable
        {
            get { return isMusicEnable; }
            set
            {
                isMusicEnable = value;
                if (value)
                {
                    AudioEngine.BackgroundMusicVolume = musicVolume;
                    AudioEngine.RewindBackgroundMusic();
                }
                else
                {
                    AudioEngine.BackgroundMusicVolume = 0;
                    AudioEngine.StopBackgroundMusic();
                }
            }
        }

        private static float musicVolume = 1.0f;
        public static float MusicVolume
        {
            get 
            {
                //return AudioEngine.BackgroundMusicVolume;
                return musicVolume;
            }
            set 
            {
                musicVolume = value;
                AudioEngine.BackgroundMusicVolume = value;
            }
        }

        private static float effectVolume = 1.0f;
        public static float EffectVolume
        {
            get 
            {
                return effectVolume; 
                //return AudioEngine.EffectsVolume;
            }
            set 
            {
                effectVolume = value;
                AudioEngine.EffectsVolume = value;
            }
        }
    }

}
