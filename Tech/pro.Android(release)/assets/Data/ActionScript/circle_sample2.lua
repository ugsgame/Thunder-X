
function OnEventCall(id)
	if(id == EVENT_ID_BEGIN)then
		OpenFire()
		Circle(1,5,576,430)
	elseif(id == 1)then
		KillSelf()
	elseif(id == EVENT_ID_OVER)then
	end
end

function OnEventOver(id)
end	
	