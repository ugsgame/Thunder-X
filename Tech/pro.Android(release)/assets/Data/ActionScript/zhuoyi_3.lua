
function OnEventCall(id)
	if(id == EVENT_ID_BEGIN)then
		MoveH(1,2,-220)
	elseif(id == 1)then
		OpenFire()
		MoveH(2,10,-800)
	elseif(id == 2)then
		KillSelf()
	elseif(id == EVENT_ID_OVER)then
	end
end

function OnEventOver(id)
end