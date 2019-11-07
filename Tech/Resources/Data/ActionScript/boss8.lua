
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
	zidan13 = CreateBullet("laser1.bt")
end

function OnFrenzy()	
	zidanzhu1 = zidanzhuF1
	zidanzhu2 = zidanzhuF2
	zidanzhu3 = zidanzhuF3
	
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
	BindEmitter(Emit_1,zidan1)	
	OpenFire()
end

function zidanzhu2()
	Ready()	
	BindEmitter(Emit_2,zidan3)
	BindEmitter(Emit_3,zidan4)
	OpenFire()
end

function zidanzhu3()
	Ready()
	BindEmitter(Emit_1,zidan1)		
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
	BindEmitter(Emit_1,zidan8)
	BindEmitter(Emit_6,zidan7)
	OpenFire()
end

function zidanzhuF3()
	Ready()	
	BindEmitter(Emit_1,zidan13)
	OpenFire()
end

function OnEventCall(id)
	if(id == EVENT_ID_BEGIN)then
		MoveV(1,2,-400)
	elseif(id == 1)then
		zidanzhu1()		
		MoveH(2,8,0)
	elseif(id == 2)then
	    zidanzhu2()
		MoveV(3,10,0)
	elseif(id == 3)then
		MoveV(4,1.2,200)	
	elseif(id == 4)then
		MoveV(5,1,-450)
	elseif(id == 5)then
	    zidanzhu1()
		MoveV(6,2,250)
	elseif(id == 6)then
	    zidanzhu2()
		MoveH(7,5,0)
	elseif(id == 7)then
		MoveH(8,3,-250)
	elseif(id == 8)then
		MoveH(9,1.2,500)
	elseif(id == 9)then
	    zidanzhu3()
		MoveH(10,2.5,-500)
	elseif(id == 10)then
	    zidanzhu3()
		MoveH(11,1.5,500)
	elseif(id == 11)then
	    zidanzhu2()
		MoveH(1,3,-250)
	elseif(id == EVENT_ID_OVER)then
	end
end

function OnEventOver(id)
end