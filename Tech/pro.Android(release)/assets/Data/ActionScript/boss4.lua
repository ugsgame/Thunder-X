
function OnInit()
	print("lua OnInit")
	
	zidan1 = CreateBullet("boss-f0.bt")
	zidan2 = CreateBullet("boss-f1.bt")
	zidan3 = CreateBullet("boss-f2.bt")
	zidan4 = CreateBullet("boss-f2.bt")
	zidan5 = CreateBullet("boss-f3.bt")
	zidan6 = CreateBullet("boss-f3.bt")
	zidan7 = CreateBullet("boss-f4.bt")
	zidan8 = CreateBullet("boss-f5.bt")
	zidan9 = CreateBullet("boss-f5.bt")
	zidan10 = CreateBullet("boss7.bt")
	zidan11 = CreateBullet("xiaoguai13.bt")
	zidan12 = CreateBullet("xiaoguai13.bt")
end

function OnFrenzy()	
	zidanzhu1 = zidanzhuF1
	zidanzhu2 = zidanzhuF2
	
	zidanzhu1()
end

function OnAttackOver()
end

function Ready()
	CloseFire()			
	UnbindAllEmitter()	
end

function zidanzhu1()
	Ready()
	BindEmitter(Emit_1,zidan3)
    BindEmitter(Emit_2,zidan5)
    BindEmitter(Emit_3,zidan6)	
	OpenFire()
end

function zidanzhu2()
	Ready()
	BindEmitter(Emit_1,zidan2)		
	BindEmitter(Emit_2,zidan3)
	BindEmitter(Emit_3,zidan4)
	OpenFire()
end

function zidanzhuF1()
	Ready()
	BindEmitter(Emit_1,zidan2)		
	BindEmitter(Emit_2,zidan5)
	BindEmitter(Emit_3,zidan6)
	OpenFire()
end

function zidanzhuF2()
	Ready()	
	BindEmitter(Emit_2,zidan5)
	BindEmitter(Emit_3,zidan6)
	BindEmitter(Emit_4,zidan8)
	BindEmitter(Emit_5,zidan9)
	OpenFire()
end

function OnEventCall(id)
	if(id == EVENT_ID_BEGIN)then
		MoveV(1,2,-400)
	elseif(id == 1)then
		zidanzhu2()		
		MoveH(2,10,0)
	elseif(id == 2)then
	    zidanzhu1()
		MoveH(3,10,0)
	elseif(id == 3)then
		MoveH(4,3,-200)	
	elseif(id == 4)then
	    zidanzhu2()
		MoveH(5,3,400)
	elseif(id == 5)then
	    zidanzhu1()
		MoveH(1,3,-200)
	elseif(id == EVENT_ID_OVER)then
	end
end

function OnEventOver(id)
end