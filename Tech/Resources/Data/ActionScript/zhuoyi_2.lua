
function OnEventCall(id)
	if(id == EVENT_ID_BEGIN)then
		MoveH(1,1.4,-570)
	elseif(id == 1)then
		OpenFire()
		MoveV(2,1.5,100)
	elseif(id == 2)then
		MoveH(3,10,-10)
	elseif(id == 3)then
		MoveH(2,10,10)
	elseif(id == EVENT_ID_OVER)then
	end
end

function OnEventOver(id)
end