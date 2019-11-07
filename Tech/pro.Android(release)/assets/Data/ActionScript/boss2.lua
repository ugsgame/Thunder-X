
function OnInit()
	print("lua OnInit")
	
	bullet1 = CreateBullet("boss-f0.bt")
	bullet2 = CreateBullet("boss-f1.bt")
	bullet3 = CreateBullet("boss-f2.bt")
	bullet4 = CreateBullet("boss-f2.bt")
	bullet5 = CreateBullet("boss-f3.bt")
	bullet6 = CreateBullet("boss-f3.bt")
	bullet7 = CreateBullet("boss-f4.bt")
	bullet8 = CreateBullet("boss-f5.bt")
	bullet9 = CreateBullet("boss-f5.bt")
	bullet10 = CreateBullet("boss7.bt")
	bullet11 = CreateBullet("boss9.bt")
	bullet12 = CreateBullet("xiaoguai13.bt")
	bullet13 = CreateBullet("boss-f1.bt")
end

function OnFrenzy()	
	ChangBullets1 = ChangBulletsF1
	ChangBullets2 = ChangBulletsF2
	ChangBullets3 = ChangBulletsF3
	
	ChangBullets1()
end

function Ready()
	CloseFire()			
	UnbindAllEmitter()	
end

function ChangBullets1()
	Ready()
	BindEmitter(Emit_1,bullet1)		
	OpenFire()
end

function ChangBullets2()
	Ready()
	BindEmitter(Emit_1,bullet2)		
	BindEmitter(Emit_2,bullet3)
	BindEmitter(Emit_3,bullet4)
	OpenFire()
end

function ChangBullets3()
	Ready()	
	BindEmitter(Emit_2,bullet2)
	BindEmitter(Emit_3,bullet13)
	OpenFire()
end

function OnAttackOver()
end

function ChangBulletsF1()
	Ready()	
	BindEmitter(Emit_2,bullet5)
	BindEmitter(Emit_3,bullet6)
	OpenFire()
end

function ChangBulletsF2()
	Ready()
	BindEmitter(Emit_1,bullet10)		
	BindEmitter(Emit_2,bullet5)
	BindEmitter(Emit_3,bullet6)
	BindEmitter(Emit_6,bullet11)
	OpenFire()
end

function ChangBulletsF3()
	Ready()
	BindEmitter(Emit_6,bullet12)		
	OpenFire()
end

function OnEventCall(id)
	if(id == EVENT_ID_BEGIN)then
		MoveV(1,2,-400)
	elseif(id == 1)then
		ChangBullets2()		
		MoveH(2,10,0)
	elseif(id == 2)then
	    ChangBullets1()
		MoveH(3,5,-250)
	elseif(id == 3)then
	    ChangBullets3()
		MoveH(4,3,500)
	elseif(id == 4)then
	    ChangBullets3()
		MoveH(5,5,-250)
	elseif(id == 5)then
		MoveH(1,2,-0)	
	elseif(id == EVENT_ID_OVER)then
	end
end

function OnEventOver(id)
end