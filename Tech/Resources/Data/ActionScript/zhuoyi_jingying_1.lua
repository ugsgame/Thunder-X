
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
		Transform(1,2)
	elseif(id == 2)then
		OpenFire()
		GotoEvent(2,3)
	elseif(id == 3)then
		MoveV(4,3,80)
	elseif(id == 4)then
		MoveH(5,3,100)
	elseif(id == 5)then
		MoveV(6,3,-80)
	elseif(id == 6)then
		MoveH(3,3,-100)
	elseif(id == EVENT_ID_OVER)then
	end
end

function OnEventOver(id)
end	
	