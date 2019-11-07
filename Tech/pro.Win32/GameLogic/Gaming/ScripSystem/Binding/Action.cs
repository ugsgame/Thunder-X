using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UniLua;

using MatrixEngine.Math;
using MatrixEngine.Cocos2d;
using MatrixEngine.Engine;
using MatrixEngine.CocoStudio.Armature;

namespace Thunder.GameLogic.Gaming.ScripSystem.Binding
{
    public class Action:ScriptBinding
    {
        class ActionPlayAnim : MCAction
        {
            CCArmature armature;
            string animation;
            bool loop;

            bool flag;

            public ActionPlayAnim(CCArmature arm, string anim, bool loop)
                : base(0.01f)
            {
                armature = arm;
                animation = anim;
                this.loop = loop;

                flag = false;
            }

            protected override void OnUpdate(float percent)
            {
                base.OnUpdate(percent);

                if (percent >= 1 && !flag)
                {
                    armature.GetAnimation().Play(animation, loop);
                }
            }
        }

        public Action(ScriptHelper scriptHelper)
            : base(scriptHelper)
        {
            L.L_RequireF(className, RegisterLib, true);
        }

        private static int RegisterLib(ILuaState lua)
        {
            try
            {
                var define = new NameFuncPair[]
                {
                    RegisterFuc(Sequence),
                    RegisterFuc(Spawn),
                    RegisterFuc(Repeat),
                    RegisterFuc(RepeatForever),
                    RegisterFuc(RotateTo),
                    RegisterFuc(RotateBy),
                    RegisterFuc(MoveTo),
                    RegisterFuc(MoveBy),
                    RegisterFuc(SkewTo),
                    RegisterFuc(SkewBy),
                    RegisterFuc(JumpTo),
                    RegisterFuc(JumpBy),
                    RegisterFuc(BezierTo),
                    RegisterFuc(BezierBy),
                    RegisterFuc(ScaleTo),
                    RegisterFuc(ScaleBy),
                    RegisterFuc(Blink),
                    RegisterFuc(FadeIn),
                    RegisterFuc(FadeOut),
                    RegisterFuc(FadeTo),
                    RegisterFuc(TintTo),
                    RegisterFuc(TintBy),
                    RegisterFuc(DelayTime),
                    RegisterFuc(EaseIn),
                    RegisterFuc(EaseOut),
                    RegisterFuc(EaseInOut),
                    RegisterFuc(EaseExponentialIn),
                    RegisterFuc(EaseExponentialOut),
                    RegisterFuc(EaseExponentialInOut),
                    RegisterFuc(EaseSineIn),
                    RegisterFuc(EaseSineOut),
                    RegisterFuc(EaseSineInOut),
                    RegisterFuc(EaseElastic),
                    RegisterFuc(EaseElasticIn),
                    RegisterFuc(EaseElasticOut),
                    RegisterFuc(EaseElasticInOut),
                    RegisterFuc(EaseBounce),
                    RegisterFuc(EaseBounceIn),
                    RegisterFuc(EaseBounceOut),
                    RegisterFuc(EaseBounceInOut),
                    RegisterFuc(EaseBackIn),
                    RegisterFuc(EaseBackOut),
                    RegisterFuc(EaseBackInOut),
                    RegisterFuc(EventAction),
                    RegisterFuc(PlayAnim),
                };

                lua.L_NewLib(define);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return 1;
        }

        private static int Sequence(ILuaState lua)
        {
            CCAction[] actions = new CCAction[lua.GetTop()];
            for (int i = 1; i <= lua.GetTop(); i++)
            {
                actions[i - 1] = (CCAction)lua.ToObject(i);
            }
            CCAction sequence = new CCActionSequence(actions);

            lua.PushLightUserData(sequence);

            return 1;
        }

        private static int Spawn(ILuaState lua)
        {
            CCAction[] actions = new CCAction[lua.GetTop()];
            for (int i = 1; i <= lua.GetTop(); i++)
            {
                actions[i] = (CCAction)lua.ToObject(i);
            }
            CCAction spawn = new CCActionSpawn(actions);

            lua.PushLightUserData(spawn);

            return 1;
        }

        private static int Repeat(ILuaState lua)
        {
            CCActionInterval _action = (CCActionInterval)lua.ToObject(1);
            int times = lua.ToInteger(2);

            CCAction action = new CCActionRepeat(_action, times);

            lua.PushLightUserData(action);

            return 1;
        }

        private static int RepeatForever(ILuaState lua)
        {
            CCActionInterval _action = (CCActionInterval)lua.ToObject(1);
            CCAction action = new CCActionRepeatForever(_action);
            lua.PushLightUserData(action);
            return 1;
        }

        private static int RotateTo(ILuaState lua)
        {
            float time = (float)lua.ToNumber(1);
            float rotate = (float)lua.ToNumber(2);
            CCAction action = new CCActionRotateTo(time, rotate);
            lua.PushLightUserData(action);
            return 1;
        }
        private static int RotateBy(ILuaState lua)
        {
            float time = (float)lua.ToNumber(1);
            float rotate = (float)lua.ToNumber(2);
            CCAction action = new CCActionRotateBy(time, rotate);
            lua.PushLightUserData(action);
            return 1;
        }

        private static int MoveTo(ILuaState lua)
        {
            float time = (float)lua.ToNumber(1);
            float x = (float)lua.ToNumber(2);
            float y = (float)lua.ToNumber(3);
            CCAction action = new CCActionMoveTo(time, x, y);
            lua.PushLightUserData(action);
            return 1;
        }
        private static int MoveBy(ILuaState lua)
        {
            float time = (float)lua.ToNumber(1);
            float x = (float)lua.ToNumber(2);
            float y = (float)lua.ToNumber(3);
            CCAction action = new CCActionMoveBy(time, x, y);
            lua.PushLightUserData(action);
            return 1;
        }

        private static int SkewTo(ILuaState lua)
        {
            float time = (float)lua.ToNumber(1);
            float x = (float)lua.ToNumber(2);
            float y = (float)lua.ToNumber(3);
            CCAction action = new CCActionSkewTo(time, x, y);
            lua.PushLightUserData(action);
            return 1;
        }
        private static int SkewBy(ILuaState lua)
        {
            float time = (float)lua.ToNumber(1);
            float x = (float)lua.ToNumber(2);
            float y = (float)lua.ToNumber(3);
            CCAction action = new CCActionSkewBy(time, x, y);
            lua.PushLightUserData(action);
            return 1;
        }

        private static int JumpTo(ILuaState lua)
        {
            float time = (float)lua.ToNumber(1);
            float x = (float)lua.ToNumber(2);
            float y = (float)lua.ToNumber(3);
            float height = (float)lua.ToNumber(4);
            int jumps = lua.ToInteger(5);
            CCAction action = new CCActionJumpTo(time, new Vector2(x, y), height, jumps);
            lua.PushLightUserData(action);
            return 1;
        }
        private static int JumpBy(ILuaState lua)
        {
            float time = (float)lua.ToNumber(1);
            float x = (float)lua.ToNumber(2);
            float y = (float)lua.ToNumber(3);
            float height = (float)lua.ToNumber(4);
            int jumps = lua.ToInteger(5);
            CCAction action = new CCActionJumpBy(time, new Vector2(x, y), height, jumps);
            lua.PushLightUserData(action);
            return 1;
        }

        private static int BezierTo(ILuaState lua)
        {
            float time = (float)lua.ToNumber(1);
            float x1 = (float)lua.ToNumber(2);
            float y1 = (float)lua.ToNumber(3);
            float x2 = (float)lua.ToNumber(4);
            float y2 = (float)lua.ToNumber(5);
            float x3 = (float)lua.ToNumber(6);
            float y3 = (float)lua.ToNumber(7);
            CCAction action = new CCActionBezierTo(time, new Vector2(x1, y1), new Vector2(x2, y2), new Vector2(x3, y3));
            return 1;
        }
        private static int BezierBy(ILuaState lua)
        {
            float time = (float)lua.ToNumber(1);
            float x1 = (float)lua.ToNumber(2);
            float y1 = (float)lua.ToNumber(3);
            float x2 = (float)lua.ToNumber(4);
            float y2 = (float)lua.ToNumber(5);
            float x3 = (float)lua.ToNumber(6);
            float y3 = (float)lua.ToNumber(7);
            CCAction action = new CCActionBezierBy(time, new Vector2(x1, y1), new Vector2(x2, y2), new Vector2(x3, y3));
            return 1;
        }

        private static int ScaleTo(ILuaState lua)
        {
            float time = (float)lua.ToNumber(1);
            float s = (float)lua.ToNumber(2);
            CCAction action = new CCActionScaleTo(time, s);
            lua.PushLightUserData(action);
            return 1;
        }

        private static int ScaleBy(ILuaState lua)
        {
            float time = (float)lua.ToNumber(1);
            float s = (float)lua.ToNumber(2);
            CCAction action = new CCActionScaleBy(time, s);
            lua.PushLightUserData(action);
            return 1;
        }

        private static int Blink(ILuaState lua)
        {
            float time = (float)lua.ToNumber(1);
            int blink = lua.ToInteger(2);
            CCAction action = new CCActionBlink(time, blink);
            lua.PushLightUserData(action);
            return 1;
        }

        private static int FadeIn(ILuaState lua)
        {
            float time = (float)lua.ToNumber(1);
            CCAction action = new CCActionFadeIn(time);
            lua.PushLightUserData(action);
            return 1;
        }
        private static int FadeOut(ILuaState lua)
        {
            float time = (float)lua.ToNumber(1);
            CCAction action = new CCActionFadeOut(time);
            lua.PushLightUserData(action);
            return 1;
        }
        private static int FadeTo(ILuaState lua)
        {
            float time = (float)lua.ToNumber(1);
            int opacity = lua.ToInteger(2);
            CCAction action = new CCActionFadeTo(time, opacity);
            lua.PushLightUserData(action);
            return 1;
        }

        private static int TintTo(ILuaState lua)
        {
            float time = (float)lua.ToNumber(1);
            int r = lua.ToInteger(2);
            int g = lua.ToInteger(3);
            int b = lua.ToInteger(4);
            CCAction action = new CCActionTintTo(time, new Color32((byte)r, (byte)g, (byte)b));
            lua.PushLightUserData(action);
            return 1;
        }
        private static int TintBy(ILuaState lua)
        {
            float time = (float)lua.ToNumber(1);
            int r = lua.ToInteger(2);
            int g = lua.ToInteger(3);
            int b = lua.ToInteger(4);
            CCAction action = new CCActionTintBy(time, new Color32((byte)r, (byte)g, (byte)b));
            lua.PushLightUserData(action);
            return 1;
        }
        private static int DelayTime(ILuaState lua)
        {
            float time = (float)lua.ToNumber(1);
            CCAction action = new CCActionDelayTime(time);
            lua.PushLightUserData(action);
            return 1;
        }

        private static int EaseIn(ILuaState lua)
        {
            CCActionInterval _action = (CCActionInterval)lua.ToObject(1);
            float rate = (float)lua.ToNumber(2);
            CCAction action = new CCActionEaseIn(_action, rate);
            lua.PushLightUserData(action);
            return 1;
        }
        private static int EaseOut(ILuaState lua)
        {
            CCActionInterval _action = (CCActionInterval)lua.ToObject(1);
            float rate = (float)lua.ToNumber(2);
            CCAction action = new CCActionEaseOut(_action, rate);
            lua.PushLightUserData(action);
            return 1;
        }
        private static int EaseInOut(ILuaState lua)
        {
            CCActionInterval _action = (CCActionInterval)lua.ToObject(1);
            float rate = (float)lua.ToNumber(2);
            CCAction action = new CCActionEaseInOut(_action, rate);
            lua.PushLightUserData(action);
            return 1;
        }

        private static int EaseExponentialIn(ILuaState lua)
        {
            CCActionInterval _action = (CCActionInterval)lua.ToObject(1);
            CCAction action = new CCActionEaseExponentialIn(_action);
            lua.PushLightUserData(action);
            return 1;
        }
        private static int EaseExponentialOut(ILuaState lua)
        {
            CCActionInterval _action = (CCActionInterval)lua.ToObject(1);
            CCAction action = new CCActionEaseExponentialOut(_action);
            lua.PushLightUserData(action);
            return 1;
        }

        private static int EaseExponentialInOut(ILuaState lua)
        {
            CCActionInterval _action = (CCActionInterval)lua.ToObject(1);
            CCAction action = new CCActionEaseExponentialInOut(_action);
            lua.PushLightUserData(action);
            return 1;
        }

        private static int EaseSineIn(ILuaState lua)
        {
            CCActionInterval _action = (CCActionInterval)lua.ToObject(1);
            CCAction action = new CCActionEaseSineIn(_action);
            lua.PushLightUserData(action);
            return 1;
        }
        private static int EaseSineOut(ILuaState lua)
        {
            CCActionInterval _action = (CCActionInterval)lua.ToObject(1);
            CCAction action = new CCActionEaseSineOut(_action);
            lua.PushLightUserData(action);
            return 1;
        }
        private static int EaseSineInOut(ILuaState lua)
        {
            CCActionInterval _action = (CCActionInterval)lua.ToObject(1);
            CCAction action = new CCActionEaseSineInOut(_action);
            lua.PushLightUserData(action);
            return 1;
        }

        private static int EaseElastic(ILuaState lua)
        {
            CCActionInterval _action = (CCActionInterval)lua.ToObject(1);
            CCAction action = new CCActionEaseElastic(_action);
            lua.PushLightUserData(action);
            return 1;
        }
        private static int EaseElasticIn(ILuaState lua)
        {
            CCActionInterval _action = (CCActionInterval)lua.ToObject(1);
            CCAction action = new CCActionEaseElasticIn(_action);
            lua.PushLightUserData(action);
            return 1;
        }
        private static int EaseElasticOut(ILuaState lua)
        {
            CCActionInterval _action = (CCActionInterval)lua.ToObject(1);
            CCAction action = new CCActionEaseElasticOut(_action);
            lua.PushLightUserData(action);
            return 1;
        }
        private static int EaseElasticInOut(ILuaState lua)
        {
            CCActionInterval _action = (CCActionInterval)lua.ToObject(1);
            CCAction action = new CCActionEaseElasticInOut(_action);
            lua.PushLightUserData(action);
            return 1;
        }

        private static int EaseBounce(ILuaState lua)
        {
            CCActionInterval _action = (CCActionInterval)lua.ToObject(1);
            CCAction action = new CCActionEaseBounce(_action);
            lua.PushLightUserData(action);
            return 1;
        }
        private static int EaseBounceIn(ILuaState lua)
        {
            CCActionInterval _action = (CCActionInterval)lua.ToObject(1);
            CCAction action = new CCActionEaseBounceIn(_action);
            lua.PushLightUserData(action);
            return 1;
        }
        private static int EaseBounceOut(ILuaState lua)
        {
            CCActionInterval _action = (CCActionInterval)lua.ToObject(1);
            CCAction action = new CCActionEaseBounceOut(_action);
            lua.PushLightUserData(action);
            return 1;
        }
        private static int EaseBounceInOut(ILuaState lua)
        {
            CCActionInterval _action = (CCActionInterval)lua.ToObject(1);
            CCAction action = new CCActionEaseBounceInOut(_action);
            lua.PushLightUserData(action);
            return 1;
        }

        private static int EaseBackIn(ILuaState lua)
        {
            CCActionInterval _action = (CCActionInterval)lua.ToObject(1);
            CCAction action = new CCActionEaseBackIn(_action);
            lua.PushLightUserData(action);
            return 1;
        }
        private static int EaseBackOut(ILuaState lua)
        {
            CCActionInterval _action = (CCActionInterval)lua.ToObject(1);
            CCAction action = new CCActionEaseBackOut(_action);
            lua.PushLightUserData(action);
            return 1;
        }
        private static int EaseBackInOut(ILuaState lua)
        {
            CCActionInterval _action = (CCActionInterval)lua.ToObject(1);
            CCAction action = new CCActionEaseBackInOut(_action);
            lua.PushLightUserData(action);
            return 1;
        }
        //
        private static int EventAction(ILuaState lua)
        {
            Actors.Actor actor = (Actors.Actor)lua.ToObject(1);

            int id = lua.ToInteger(2);
            int nextID = lua.ToInteger(3);

            Events.EventAction ev = new Events.EventAction(nextID);
            ev.ID = id;
            actor.EventManager.RunEvent(ev);
            lua.PushLightUserData(ev.Action);
            return 1;
        }

        private static int PlayAnim(ILuaAPI lua)
        {
            CCArmature arm = (CCArmature)lua.ToObject(1);
            string anim = lua.ToString(2);
            bool loop = lua.ToBoolean(3);

            CCAction action = new ActionPlayAnim(arm, anim, loop);
            lua.PushLightUserData(action);
            return 1;
        }
    }
}
