
function OnEventCall(id)
	if(id == EVENT_ID_BEGIN)then
		MoveV(1,0.6,-350)
		CloseFire()
	elseif(id == 1)then
		MoveV(2,1,80)
	elseif(id == 2)then
	    OpenFire()
		MoveFree(3,8,60)
	elseif(id == 3)then
		MoveFree(4,10,100)
	elseif(id == 4)then
		MoveFree(5,2,50)
	elseif(id == 5)then
		MoveFree(1,3,50)
	elseif(id == EVENT_ID_OVER)then
	end
end

function OnEventOver(id)
end	
	