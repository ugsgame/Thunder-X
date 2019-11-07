
function OnEventCall(id)
	if(id == EVENT_ID_BEGIN)then
		MoveBy(1,0.8,0,-360)
		CloseFire()
	elseif(id == 1)then
	    OpenFire()
		MoveFreeH(1,10,80)
	elseif(id == EVENT_ID_OVER)then
	end
end

function OnEventOver(id)
end	
	