
function OnEventCall(id)
	if(id == EVENT_ID_BEGIN)then
		MoveV(1,3,-300)
		CloseFire()
	elseif(id == 1)then
		MoveV(2,2,50)
	elseif(id == 2)then
	    OpenFire()
		MoveV(3,25,-900)
	elseif(id == 3)then
		KillSelf()
	elseif(id == EVENT_ID_OVER)then
	end
end

function OnEventOver(id)
end
--下移出屏幕