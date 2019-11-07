using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UniLua;

using MatrixEngine.Math;
using MatrixEngine.Cocos2d;


namespace Thunder.GameLogic.Gaming.ScripSystem.Binding
{
    public class Node:ScriptBinding
    {
        public Node(ScriptHelper scriptHelper)
            : base(scriptHelper)
        {
            L.L_RequireF(className, RegisterLib, true);
        }
        //注册函数
        private static int RegisterLib(ILuaState lua)
        {
            try
            {
                var define = new NameFuncPair[]
                {
                    RegisterFuc(Create),
                    RegisterFuc(SetZOrder),
                    RegisterFuc(GetZOrder),
                    RegisterFuc(SetScaleX),
                    RegisterFuc(GetScaleX),
                    RegisterFuc(SetScaleY),
                    RegisterFuc(GetScaleY),
                    RegisterFuc(SetScale),
                    RegisterFuc(GetScale),
                    RegisterFuc(SetPosition),
                    RegisterFuc(GetPosition),
                    RegisterFuc(SetRotation),
                    RegisterFuc(GetRotation),
                    RegisterFuc(SetVisible),
                    RegisterFuc(GetVisible),
                    RegisterFuc(GetTag),
                    RegisterFuc(SetTag),
                    RegisterFuc(AddChild),
                    RegisterFuc(GetChild),
                    RegisterFuc(GetChildrenCount),
                    RegisterFuc(SetParent),
                    RegisterFuc(GetParent),
                    RegisterFuc(RemoveFromParent),
                    RegisterFuc(RemoveChild),
                    RegisterFuc(RemoveAllChildren),
                    RegisterFuc(RunAction),
                    RegisterFuc(StopAction),
                    RegisterFuc(StopAllAction)
                };

                lua.L_NewLib(define);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return 1;
        }
        //创建节点
        private static int Create(ILuaState lua)
        {
            CCNode node = new CCNode();
            lua.PushLightUserData(node);
            return 1;
        }
        private static int SetZOrder(ILuaState lua)
        {
            CCNode node = (CCNode)lua.ToObject(1);
            int ZOrder = lua.ToInteger(2);
            node.ZOrder = ZOrder;

            return 0;
        }
        private static int GetZOrder(ILuaState lua)
        {
            CCNode node = (CCNode)lua.ToObject(1);
            lua.PushInteger(node.ZOrder);
            return 1;
        }
        private static int SetScaleX(ILuaState lua)
        {
            CCNode node = (CCNode)lua.ToObject(1);
            float x = (float)lua.ToNumber(2);
            node.ScaleX = x;
            return 0;
        }
        private static int GetScaleX(ILuaState lua)
        {
            CCNode node = (CCNode)lua.ToObject(1);
            float x = node.ScaleX;
            lua.PushNumber(x);
            return 1;
        }
        private static int SetScaleY(ILuaState lua)
        {
            CCNode node = (CCNode)lua.ToObject(1);
            float y = (float)lua.ToNumber(2);
            node.ScaleY = y;
            return 0;
        }
        private static int GetScaleY(ILuaState lua)
        {
            CCNode node = (CCNode)lua.ToObject(1);
            float y = node.ScaleY;
            lua.PushNumber(y);
            return 1;
        }
        private static int SetScale(ILuaState lua)
        {
            CCNode node = (CCNode)lua.ToObject(1);
            float x = (float)lua.ToNumber(2);
            float y = (float)lua.ToNumber(3);
            node.Scale = new Vector2(x, y);
            return 0;
        }
        private static int GetScale(ILuaState lua)
        {
            CCNode node = (CCNode)lua.ToObject(1);
            Vector2 scale = node.Scale;
            lua.PushNumber(scale.X);
            lua.PushNumber(scale.Y);
            return 2;
        }
        //
        private static int SetPosition(ILuaState lua)
        {
            CCNode node = (CCNode)lua.ToObject(1);

            float x = (float)lua.ToNumber(2);
            float y = (float)lua.ToNumber(3);
            node.Postion = new Vector2(x, y);

            return 0;
        }
        //
        private static int GetPosition(ILuaState lua)
        {
            CCNode node = (CCNode)lua.ToObject(1);
            Vector2 pos = node.Postion;

            lua.PushNumber(pos.X);
            lua.PushNumber(pos.Y);

            return 2;
        }

        private static int SetRotation(ILuaState lua)
        {
            CCNode node = (CCNode)lua.ToObject(1);
            float rotation = (float)lua.ToNumber(2);
            node.Rotation = rotation;
            return 0;
        }
        private static int GetRotation(ILuaState lua)
        {
            CCNode node = (CCNode)lua.ToObject(1);
            float rotation = node.Rotation;

            lua.PushNumber(rotation);

            return 1;
        }

        private static int SetVisible(ILuaState lua)
        {
            CCNode node = (CCNode)lua.ToObject(1);
            bool visible = lua.ToBoolean(2);
            node.SetVisible(visible);
            return 0;
        }
        private static int GetVisible(ILuaState lua)
        {
            CCNode node = (CCNode)lua.ToObject(1);
            bool visible = node.GetVisible();

            if (visible)
                lua.PushInteger(1);
            else
                lua.PushNil();

            return 1;
        }

        private static int GetTag(ILuaState lua)
        {
            CCNode node = (CCNode)lua.ToObject(1);
            int tag = node.Tag;

            lua.PushInteger(tag);

            return 1;
        }
        private static int SetTag(ILuaState lua)
        {
            CCNode node = (CCNode)lua.ToObject(1);
            node.Tag = lua.ToInteger(2);

            return 0;
        }

        private static int AddChild(ILuaState lua)
        {
            CCNode node = (CCNode)lua.ToObject(1);
            CCNode child = (CCNode)lua.ToObject(2);

            if (lua.GetTop() == 2)
            {
                node.AddChild(child);
            }
            else if (lua.GetTop() == 3)
            {
                int zOrder = lua.ToInteger(3);
                node.AddChild(child, zOrder);
            }
            else if (lua.GetTop() == 4)
            {
                int zOrder = lua.ToInteger(3);
                int tag = lua.ToInteger(4);
                node.AddChild(child, zOrder, tag);
            }
            return 0;
        }

        private static int GetChild(ILuaState lua)
        {
            CCNode node = (CCNode)lua.ToObject(1);
            int tag = lua.ToInteger(2);
            lua.PushLightUserData(node.GetChild(tag));
            return 1;
        }

        private static int GetChildrenCount(ILuaState lua)
        {
            CCNode node = (CCNode)lua.ToObject(1);
            int count = node.GetChildrenCount();
            lua.PushInteger(count);
            return 1;
        }

        private static int SetParent(ILuaState lua)
        {
            CCNode node = (CCNode)lua.ToObject(1);
            CCNode parent = (CCNode)lua.ToObject(2);
            node.Parent = parent;
            return 0;
        }

        private static int GetParent(ILuaState lua)
        {
            CCNode node = (CCNode)lua.ToObject(1);
            CCNode parent = node.Parent;
            lua.PushLightUserData(parent);
            return 1;
        }

        private static int RemoveFromParent(ILuaState lua)
        {
            CCNode node = (CCNode)lua.ToObject(1);

            if (lua.GetTop() == 1)
            {
                node.RemoveFromParent();
            }
            else if (lua.GetTop() == 2)
            {
                bool cleanup = lua.ToBoolean(2);
                node.RemoveFromParent(cleanup);
            }
            return 0;
        }

        private static int RemoveChild(ILuaState lua)
        {
            CCNode node = (CCNode)lua.ToObject(1);
            CCNode child = (CCNode)lua.ToObject(2);

            if (lua.GetTop() == 2)
            {
                node.RemoveChild(child);
            }
            else if (lua.GetTop() == 3)
            {
                bool cleanup = lua.ToBoolean(3);
                node.RemoveChild(child, cleanup);
            }

            return 0;
        }

        private static int RemoveAllChildren(ILuaState lua)
        {
            CCNode node = (CCNode)lua.ToObject(1);
            if (lua.GetTop() == 1)
            {
                node.RemoveAllChildren();
            }
            else if (lua.GetTop() == 2)
            {
                bool cleanup = lua.ToBoolean(2);
                node.RemoveAllChildren(cleanup);
            }

            return 0;
        }

        private static int RunAction(ILuaState lua)
        {
            CCNode node = (CCNode)lua.ToObject(1);
            CCAction action = (CCAction)lua.ToObject(2);
            node.RunAction(action);
            return 0;
        }

        private static int StopAction(ILuaState lua)
        {
            CCNode node = (CCNode)lua.ToObject(1);
            CCAction action = (CCAction)lua.ToObject(2);
            node.StopAction(action);
            return 0;
        }

        private static int StopAllAction(ILuaState lua)
        {
            CCNode node = (CCNode)lua.ToObject(1);
            node.StopAllAction();
            return 0;
        }
    }
}
