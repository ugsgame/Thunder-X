
function OnEventCall(id)
	if(id == EVENT_ID_BEGIN)then
		MoveBy(1,1,100,0)
	elseif(id == 1)then
		Transform(1,2)
	elseif(id == 2)then
		OpenFire()
		GotoEvent(2,3)
	elseif(id == 3)then
		MoveTo(4,5,300,800)
	elseif(id == 4)then
		MoveTo(3,3,100,10)
	elseif(id == EVENT_ID_OVER)then
	end
end

function OnEventOver(id)
end	
	