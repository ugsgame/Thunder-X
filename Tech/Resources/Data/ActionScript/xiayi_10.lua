
function OnEventCall(id)
	if(id == EVENT_ID_BEGIN)then
	    CloseFire()
		MoveV(1,1,-300)
	elseif(id == 1)then
	    MoveV(2,1,0)
	elseif(id == 2)then
	    OpenFire()
	    MoveH(3,8,500)
	elseif(id == 3)then
		KillSelf()
	elseif(id == EVENT_ID_OVER)then
	end
end

function OnEventOver(id)
end
--爆炸鸡专用