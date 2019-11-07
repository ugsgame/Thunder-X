
function OnEventCall(id)
	if(id == EVENT_ID_BEGIN)then
		MoveV(1,4,-1200)
	elseif(id == 1)then
		KillSelf()
	elseif(id == EVENT_ID_OVER)then
	end
end

function OnEventOver(id)
end
--撞击机专用