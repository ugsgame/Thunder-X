
function OnEventCall(id)
	if(id == EVENT_ID_BEGIN)then
		OpenFire()
		MoveBy(1,1,100,0)
	elseif(id == 1)then
		MoveTo(2,5,300,800)
	elseif(id == 3)then
		MoveTo(1,3,100,10)
	elseif(id == EVENT_ID_OVER)then
	end
end

function OnEventOver(id)
end	
	