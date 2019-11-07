using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UniLua;

using Thunder.GameLogic.Gaming.Actors;
using Thunder.GameLogic.Gaming.ScripSystem.Events;

namespace Thunder.GameLogic.Gaming.ScripSystem.Binding
{
    public class Event : ScriptBinding
    {
        public Event(ScriptHelper scriptHelper)
            : base(scriptHelper)
        {
            //注册类
            L.L_RequireF(className      // 库的名字
                , RegisterLib           // 库的初始化函数
                , true                  // 是否默认放到全局命名空间 (在需要的地方用require获取)
                );
        }
        //
        //注册函数
        private static int RegisterLib(ILuaState lua)
        {
            try
            {
                var define = new NameFuncPair[]
                {
                    RegisterFuc(GoTo),
                    //..
                };

                lua.L_NewLib(define);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return 1;
        }

        private static int GoTo(ILuaState lua)
        {

            Actors.Actor actor = (Actors.Actor)lua.ToObject(1);
            var nodeID = lua.ToInteger(2);
            var nextID = lua.L_CheckInteger(3);

            GoTo ev = new GoTo(actor, nodeID, nextID);
            actor.EventManager.RunEvent(ev);

            return 0;
        }
    }
}
