
function OnEventCall(id)
	if(id == EVENT_ID_BEGIN)then
		OpenFire()
		MoveBy(1,1,0,-600)
	elseif(id == 1)then
		MoveFreeV(1,10,100)
	elseif(id == EVENT_ID_OVER)then
	end
end

function OnEventOver(id)
end	
	