--状态
--空
State_Null = 0
--待机
State_Idle = 1
--移动
State_Walk = 2
--后退
State_Retreat = 3
--准备攻击状态
State_Ready = 4
--被攻击
State_ByAttack = 5
--狂暴
State_Frenzy = 6
--攻击
State_Attack = 7
--技能
State_Skill = 8
--逃跑
State_Escape = 9
--死亡
State_Dead = 10
--事件
State_Event = 11
--

--发射点
Emit_1 = 0
Emit_2 = 1
Emit_3 = 2
Emit_4 = 3
Emit_5 = 4
Emit_6 = 5
Emit_7 = 6
Emit_8 = 7
Emit_9 = 8
Emit_10 = 9
--
function MoveTo(id,time,x,y)
	Actor.xMoveTo(Self,id,time,x,y)
end

function MoveBy(id,time,x,y)
	Actor.xMoveBy(Self,id,time,x,y)
end

function MoveH(id,time,len)
	Actor.xMoveH(Self,id,time,len)
end

function MoveV(id,time,len)
	Actor.xMoveV(Self,id,time,len)
end

function MoveFree(id,time,speed)
	Actor.xMoveFree(Self,id,time,speed)
end

function MoveFreeH(id,time,speed)
	Actor.xMoveFreeH(Self,id,time,speed)
end

function MoveFreeV(id,time,speed)
	Actor.xMoveFreeV(Self,id,time,speed)
end

function Circle(id,time,posX,posY,angle)
	if(angle==nil)then
		angle = 360
	end
	Actor.xCircle(Self,id,time,posX,posY,angle)
end

function GotoState(state)
	Actor.xGotoState(Self,state)
end

function KillSelf()
	Actor.xKillSelf(Self)
end

function OpenFire()
	Actor.xOpenFire(Self)
end

function OpenFireEmit(emit)
	Actor.xOpenFire(Self,emit)
end

function CloseFire()
	Actor.xCloseFire(Self)
end

function CloseFireEmit(emit)
	Actor.xCloseFire(Self,emit)
end

function BindEmitter(emit,bullet)
	Actor.xBindEmitter(Self,emit,bullet)
end

function UnbindEmitter(emit)
	Actor.xUnbindEmitter(Self,emit)
end

function UnbindAllEmitter()
	Actor.xUnbindAllEmitter(Self)
end

function CreateBullet(name)
	return Actor.xCreateBullet(name)
end

function GotoEvent(id,nextID)
	Event.xGoTo(Self,id,nextID)
end

function Transform(id,next)
	GotoState(State_Frenzy)
	local a_delay = Action.xDelayTime(1)
	local a_event = Action.xEventAction(Self,id,next)
 	local actions = Action.xSequence(a_delay,a_event)
	Node.xRunAction(Self,actions)
end

