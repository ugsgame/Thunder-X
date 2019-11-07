
function OnEventCall(id)
	if(id == EVENT_ID_BEGIN)then
	    CloseFire()
		MoveBy(1,0.6,0,-350)
	elseif(id == 1)then
		MoveFree(2,3.5,0)
	elseif(id == 2)then
	    OpenFire()
		MoveFree(3,15,60)
	elseif(id == 3)then
		MoveFree(1,3,50)
	elseif(id == EVENT_ID_OVER)then
	end
end

function OnEventOver(id)
end	
	