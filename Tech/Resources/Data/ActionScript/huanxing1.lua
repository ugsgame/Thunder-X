
function OnEventCall(id)
	if(id == EVENT_ID_BEGIN)then
		OpenFire()
		Circle(1,13,0,490,360)
	elseif(id == 1)then
		KillSelf()
	elseif(id == EVENT_ID_OVER)then
	end
end

function OnEventOver(id)
end	
	