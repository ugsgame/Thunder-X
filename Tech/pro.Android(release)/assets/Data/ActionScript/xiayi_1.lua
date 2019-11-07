
function OnEventCall(id)
	if(id == EVENT_ID_BEGIN)then
		CloseFire()
		MoveV(1,2,-400)
	elseif(id == 1)then
	    OpenFire()
		MoveH(2,30,0)
	elseif(id == 2)then
		MoveH(3,4,-230)
	elseif(id == 3)then
		MoveH(4,4,230)
	elseif(id == 4)then
		MoveH(5,4,-230)
	elseif(id == 5)then
		MoveH(6,2.5,230)
	elseif(id == 6)then
		MoveH(7,2.5,230)
	elseif(id == 7)then
		MoveH(8,2.5,-230)
	elseif(id == 8)then
		MoveH(9,4,230)
	elseif(id == 9)then
		MoveH(1,4,-230)
	elseif(id == EVENT_ID_OVER)then
	end
end

function OnEventOver(id)
end