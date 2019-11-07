
function OnEventCall(id)
	if(id == EVENT_ID_BEGIN)then
		MoveV(1,5,-200)
	elseif(id == 1)then
		OpenFire()
		MoveV(2,10,-100)
	elseif(id == 2)then
		MoveH(3,20,-100)
	elseif(id == 3)then
		MoveH(2,20,100)
	elseif(id == EVENT_ID_OVER)then
	end
end

function OnEventOver(id)
end