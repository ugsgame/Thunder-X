
function OnEventCall(id)
	if(id == EVENT_ID_BEGIN)then
		MoveV(1,0.8,-400)
		CloseFire()
	elseif(id == 1)then
		MoveV(2,1,80)
	elseif(id == 2)then
	    OpenFire()
		MoveH(3,3,-30)
	elseif(id == 3)then
		MoveH(2,3,30)
	elseif(id == EVENT_ID_OVER)then
	end
end

function OnEventOver(id)
end
--快速下移和回弹