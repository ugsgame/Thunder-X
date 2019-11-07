
function Transform(id,next)
	GotoState(State_Frenzy)
	local a_delay = Action.xDelayTime(1)
	local a_event = Action.xEventAction(Self,id,next)
 	local actions = Action.xSequence(a_delay,a_event)
	Node.xRunAction(Self,actions)
end

function OnEventCall(id)
	if(id == EVENT_ID_BEGIN)then
		MoveH(1,1,-500)
	elseif(id == 1)then
	    OpenFire()
		Transform(1,2)
	elseif(id == 2)then
		MoveTo(3,1,270,434)
	elseif(id == 3)then
		MoveTo(4,1.7,97,611)
	elseif(id == 4)then
		MoveTo(5,1.7,284,756)
	elseif(id == 5)then
		MoveTo(6,1.7,555,608)
	elseif(id == 6)then
		MoveTo(3,1.5,293,463)
	elseif(id == EVENT_ID_OVER)then
	end
end

function OnEventOver(id)
end	
--进入后静止不动