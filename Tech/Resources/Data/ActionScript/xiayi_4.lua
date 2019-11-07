
function OnEventCall(id)
	if(id == EVENT_ID_BEGIN)then
		MoveV(1,1.1,-300)
		CloseFire()
	elseif(id == 1)then
		MoveV(2,2,50)
	elseif(id == 2)then
	    OpenFire()
		MoveH(3,5,-0)
	elseif(id == 3)then
		MoveH(2,5,0)
	elseif(id == EVENT_ID_OVER)then
	end
end

function OnEventOver(id)
end
--快速下移和回弹