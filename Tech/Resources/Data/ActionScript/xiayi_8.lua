
function OnEventCall(id)
	if(id == EVENT_ID_BEGIN)then
	    CloseFire()
		MoveV(1,2,-800)
	elseif(id == 1)then
	    OpenFire()
	    MoveV(2,0.9,0)
	elseif(id == 2)then
	    MoveV(3,5,-3000)
	elseif(id == 3)then
		KillSelf()
	elseif(id == EVENT_ID_OVER)then
	end
end

function OnEventOver(id)
end
--爆炸鸡专用