
function OnEventCall(id)
	if(id == EVENT_ID_BEGIN)then
	    CloseFire()
		MoveH(1,1.5,-500)
	elseif(id == 1)then
		OpenFire()
		MoveH(2,0.8,50)
	elseif(id == 2)then
		MoveV(3,5,80)
	elseif(id == 3)then
		MoveH(4,10,-0)
	elseif(id == 4)then
		MoveH(3,10,0)
	elseif(id == EVENT_ID_OVER)then
	end
end

function OnEventOver(id)
end

--快速进入，无摆动