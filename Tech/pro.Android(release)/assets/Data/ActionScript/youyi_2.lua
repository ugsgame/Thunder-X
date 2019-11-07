
function OnEventCall(id)
	if(id == EVENT_ID_BEGIN)then
		MoveH(1,6,600)
	elseif(id == 1)then
		OpenFire()
		MoveH(2,0.8,-50)
	elseif(id == 2)then
		MoveH(3,5,-10)
	elseif(id == 3)then
		MoveH(2,5,10)
	elseif(id == EVENT_ID_OVER)then
	end
end

function OnEventOver(id)
end
--平缓右移，回弹后循环左右摆动