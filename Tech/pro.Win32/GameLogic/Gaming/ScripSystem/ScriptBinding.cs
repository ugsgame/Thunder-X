using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UniLua;
using Thunder.GameLogic.Gaming.Actors;

namespace Thunder.GameLogic.Gaming.ScripSystem
{
    public class ScriptBinding
    {
        protected string className;
        protected ScriptHelper scriptHelper;
        protected ILuaState L;

        public ScriptBinding(ScriptHelper scriptHelper)
        {
            this.scriptHelper = scriptHelper;
            L = scriptHelper.L;
            className = GetType().Name;
        }

        public virtual void Dispose()
        {
            L = null;
        }

        protected virtual void SetFileldValue<T>(string name, T val)
        {

        }

        protected static NameFuncPair RegisterFuc(CSharpFunctionDelegate fuc)
        {
            if (fuc == null)
            {
                throw new NullReferenceException("注册函数不能为空！");
            }
            return new NameFuncPair("x"+fuc.Method.Name, fuc);
        }
    }
}
