using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UniLua;

using MatrixEngine.Cocos2d;
using MatrixEngine.Math;
using Thunder.GameLogic.Gaming.Actors;
using Thunder.GameLogic.Gaming.ScripSystem.Events;
using Thunder.GameLogic.ExternAction;
using Thunder.Common;
using Thunder.GameLogic.Gaming.BulletSystems;
using Thunder.Game;

namespace Thunder.GameLogic.Gaming.ScripSystem.Binding
{
    public class Actor : ScriptBinding
    {
        public Actor(ScriptHelper scriptHelper)
            : base(scriptHelper)
        {
            L.L_RequireF(className, RegisterLib, true);
        }

        private static void CompAction(Actors.Actor actor,CCAction action,int nextID)
        {
            Events.EventAction ev = new Events.EventAction(nextID);
            actor.EventManager.RunEvent(ev);
            CCAction newAction = new CCActionSequence(action, ev.Action);
            actor.RunAction(newAction);

            actor.AddAutonReleaseAction(newAction);
        }

        private static int RegisterLib(ILuaState lua)
        {
            try
            {
                var define = new NameFuncPair[]
                {
                    RegisterFuc(MoveTo),
                    RegisterFuc(MoveBy),
                    RegisterFuc(MoveH),
                    RegisterFuc(MoveV),
                    RegisterFuc(MoveFree),
                    RegisterFuc(MoveFreeH),
                    RegisterFuc(MoveFreeV),
                    RegisterFuc(Circle),
                    RegisterFuc(GotoState),
                    RegisterFuc(KillSelf),
                    RegisterFuc(OpenFire),
                    RegisterFuc(CloseFire),
                    RegisterFuc(BindEmitter),
                    RegisterFuc(UnbindEmitter),
                    RegisterFuc(UnbindAllEmitter),
                    RegisterFuc(CreateBullet),
                };

                lua.L_NewLib(define);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return 1;
        }
        //Actions

        private static int MoveTo(ILuaState lua)
        {
            Actors.Actor actor = (Actors.Actor)lua.ToObject(1);
            int nextID = (int)lua.ToInteger(2);

            float time = (float)lua.ToNumber(3);
            float posX = (float)lua.ToNumber(4);
            float posY = (float)lua.ToNumber(5);

            CompAction(actor,new CCActionMoveTo(time,new Vector2(posX,posY)),nextID);

            return 0;
        }

        private static int MoveBy(ILuaState lua)
        {
            Actors.Actor actor = (Actors.Actor)lua.ToObject(1);
            int nextID = (int)lua.ToInteger(2);

            float time = (float)lua.ToNumber(3);
            float posX = (float)lua.ToNumber(4);
            float posY = (float)lua.ToNumber(5);

            CompAction(actor, new CCActionMoveBy(time, new Vector2(posX, posY)), nextID);
            return 0;
        }

        private static int MoveH(ILuaState lua)
        {
            Actors.Actor actor = (Actors.Actor)lua.ToObject(1);
            int nextID = (int)lua.ToInteger(2);

            float time = (float)lua.ToNumber(3);
            float len = (float)lua.ToNumber(4);

            CompAction(actor, new CCActionMoveBy(time, new Vector2(len,0)), nextID);
            return 0;
        }
        private static int MoveV(ILuaState lua)
        {
            Actors.Actor actor = (Actors.Actor)lua.ToObject(1);
            int nextID = (int)lua.ToInteger(2);

            float time = (float)lua.ToNumber(3);
            float len = (float)lua.ToNumber(4);

            CompAction(actor, new CCActionMoveBy(time, new Vector2(0, len)), nextID);
            return 0;
        }

        private static int MoveFree(ILuaState lua)
        {
            Actors.Actor actor = (Actors.Actor)lua.ToObject(1);
            int nextID = (int)lua.ToInteger(2);
            float time = (float)lua.ToNumber(3);
            float speed = (float)lua.ToNumber(4);
            Rect rect = new Rect(0,Config.GAME_HEIGHT - Config.GAME_HEIGHT / 3, Config.GAME_WIDTH, Config.GAME_HEIGHT / 3);
            if (actor.PostionY < Config.GAME_HEIGHT - Config.GAME_HEIGHT / 3)
            {
                float offsetY = Config.GAME_HEIGHT - Config.GAME_HEIGHT / 3 + (Config.GAME_HEIGHT / 3 * 0.5f);
                CompAction(actor, new CCActionSequence(new CCActionMoveTo(1, actor.PostionX, offsetY), new ActionMoveFree(time, speed, rect)), nextID);
            }
            else
            {
                CompAction(actor, new ActionMoveFree(time, speed, rect), nextID);
            }
            return 0;
        }

        private static int MoveFreeH(ILuaState lua)
        {
            Actors.Actor actor = (Actors.Actor)lua.ToObject(1);
            int nextID = (int)lua.ToInteger(2);
            float time = (float)lua.ToNumber(3);
            float speed = (float)lua.ToNumber(4);
            Rect rect = new Rect(0, Config.GAME_HEIGHT - Config.GAME_HEIGHT / 3, Config.GAME_WIDTH, Config.GAME_HEIGHT / 3);
            CompAction(actor, new ActionMoveFreeH(time, speed, rect), nextID);
            return 0;
        }

        private static int MoveFreeV(ILuaState lua)
        {
            Actors.Actor actor = (Actors.Actor)lua.ToObject(1);
            int nextID = (int)lua.ToInteger(2);
            float time = (float)lua.ToNumber(3);
            float speed = (float)lua.ToNumber(4);
            Rect rect = new Rect(0, Config.GAME_HEIGHT - Config.GAME_HEIGHT / 3, Config.GAME_WIDTH, Config.GAME_HEIGHT / 3);
            if (actor.PostionY < Config.GAME_HEIGHT - Config.GAME_HEIGHT / 3)
            {
                float offsetY = Config.GAME_HEIGHT - Config.GAME_HEIGHT / 3 + (Config.GAME_HEIGHT / 3*0.5f);
                CompAction(actor, new CCActionSequence(new CCActionMoveTo(1, actor.PostionX, offsetY), new ActionMoveFreeV(time, speed, rect)), nextID);
            }
            else
            {
                CompAction(actor, new ActionMoveFreeV(time, speed, rect), nextID);
            }
            return 0;
        }

        private static int Circle(ILuaState lua)
        {
            Actors.Actor actor = (Actors.Actor)lua.ToObject(1);
            int nextID = (int)lua.ToInteger(2);

            float time = (float)lua.ToNumber(3);
            float posX = (float)lua.ToNumber(4);
            float posY = (float)lua.ToNumber(5);

            float angle = (float)lua.ToNumber(6);
            CompAction(actor, new ActionCircle(time, new Vector2(posX,posY), angle), nextID);
            return 0;
        }

        //
        private static int GotoState(ILuaState lua)
        {
            Actors.Actor actor = (Actors.Actor)lua.ToObject(1);
            int state = (int)lua.ToInteger(2);
            actor.GotoState((Actors.Actor.State)state);
            return 0;
        }

        private static int KillSelf(ILuaState lua)
        {
            Actors.Actor actor = (Actors.Actor)lua.ToObject(1);
            //阻在EventManager再次把event释放
            actor.EventManager.curEvent = null;
            //
            actor.Destroy();
            return 0;
        }

        private static int OpenFire(ILuaState lua)
        {
            Actors.Actor actor = (Actors.Actor)lua.ToObject(1);
            if (lua.GetTop() == 2)
            {
                Thunder.GameLogic.Gaming.Actors.Actor.EmitPoint emitPoit = (Thunder.GameLogic.Gaming.Actors.Actor.EmitPoint)(int)lua.ToInteger(2);
                actor.OpenFire(emitPoit);
            }
            else
            {
                actor.OpenFire();
            }
            return 0;
        }


        private static int CloseFire(ILuaState lua)
        {
            Actors.Actor actor = (Actors.Actor)lua.ToObject(1);
            if (lua.GetTop() == 2)
            {
                Thunder.GameLogic.Gaming.Actors.Actor.EmitPoint emitPoit = (Thunder.GameLogic.Gaming.Actors.Actor.EmitPoint)(int)lua.ToInteger(2);
                actor.CloseFire(emitPoit);
            }
            else
            {
                actor.CloseFire();
            }

            return 0;
        }

        private static int BindEmitter(ILuaState lua)
        {
            Actors.Actor actor = (Actors.Actor)lua.ToObject(1);
            Thunder.GameLogic.Gaming.Actors.Actor.EmitPoint emitPoit = (Thunder.GameLogic.Gaming.Actors.Actor.EmitPoint)(int)lua.ToInteger(2);
            BulletSystem emiter = (BulletSystem)lua.ToObject(3);
            actor.BindEmitter(emitPoit, emiter);
            return 0;
        }

        private static int UnbindEmitter(ILuaState lua)
        {
            Actors.Actor actor = (Actors.Actor)lua.ToObject(1);
            Thunder.GameLogic.Gaming.Actors.Actor.EmitPoint emitPoit = (Thunder.GameLogic.Gaming.Actors.Actor.EmitPoint)(int)lua.ToInteger(2);
            actor.UnbindEmitter(emitPoit);
            return 0;
        }

        private static int UnbindAllEmitter(ILuaState lua)
        {
            Actors.Actor actor = (Actors.Actor)lua.ToObject(1);
            actor.UnbindEmitter();
            return 0;
        }

        private static int CreateBullet(ILuaState lua)
        {
            string path = (string)lua.ToString(1);
            path = Utils.CoverBulletPath(path);
            BulletSystem bullet = new BulletSystem(path);
            lua.PushLightUserData(bullet);
            return 1;
        }
    }
}
