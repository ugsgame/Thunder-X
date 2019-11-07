
function OnInit()
	print("lua OnInit")
	
	emit1 = CreateBullet("boss-f0.bt")
	emit2 = CreateBullet("boss-f2.bt")
	emit3 = CreateBullet("boss-f2.bt")
	
	FrenzyEmit1 = CreateBullet("boss2.bt")
	FrenzyEmit2 = CreateBullet("boss-f3.bt")
	FrenzyEmit3 = CreateBullet("boss-f3.bt")
	
	FrenzyEmit5 = CreateBullet("boss9.bt")
	
	FrenzyEmit6 = CreateBullet("boss7.bt")
	FrenzyEmit7 = CreateBullet("jingying3.bt")	
	FrenzyEmit8 = CreateBullet("jingying3.bt")	
	
	attackEmit2 = CreateBullet("xiaoguai13.bt")
	attackEmit3 = CreateBullet("xiaoguai13.bt")
	
	attackEmit2_2 = CreateBullet("jingying27.bt")
	attackEmit3_2 = CreateBullet("jingying27.bt")
	
	laser = CreateBullet("laser1.bt")
end

function OnFrenzy()	
	ChangBullets1 = ChangBulletsF1
	ChangBullets2 = ChangBulletsF2
	
	ChangBullets1()
	frenyFlag = true
	print("OnFrenzy")
end

function OnAttackOver()
	ChangBullets1()
	print("OnAttackOver")
end

function Attack1()
	if frenyFlag then
		Ready()				
		GotoState(7)
		BindEmitter(Emit_2,attackEmit2)
		BindEmitter(Emit_3,attackEmit3)
		OpenFire()
		print("Attack1")
	end
end

function Attack2()
	if frenyFlag then
		Ready()				
		GotoState(7)
		BindEmitter(Emit_2,attackEmit2_2)
		BindEmitter(Emit_3,attackEmit3_2)
		OpenFire()
		print("Attack2")
	end
end

function Ready()
	CloseFire()			
	UnbindAllEmitter()	
end

function ChangBullets1()
	Ready()
	BindEmitter(Emit_1,emit1)		
	OpenFire()
end

function ChangBullets2()
	Ready()
	BindEmitter(Emit_1,emit1)
	BindEmitter(Emit_2,emit2)		
	BindEmitter(Emit_3,emit3)
	OpenFire()
end

function ChangBulletsF1()
	Ready()
	BindEmitter(Emit_2,FrenzyEmit2)		
	BindEmitter(Emit_3,FrenzyEmit3)
	OpenFire()
end

function ChangBulletsF2()
	Ready()
	BindEmitter(Emit_1,FrenzyEmit1)
	BindEmitter(Emit_2,FrenzyEmit2)		
	BindEmitter(Emit_3,FrenzyEmit3)
	OpenFire()
end

function ChangBulletsF3()
	Ready()
	BindEmitter(Emit_1,FrenzyEmit5)
	OpenFire()
end

function ChangBulletsF4()
	Ready()
	BindEmitter(Emit_1,FrenzyEmit6)
	BindEmitter(Emit_2,FrenzyEmit7)		
	BindEmitter(Emit_3,FrenzyEmit8)
	OpenFire()
end

NextID = 1

function OnEventCall(id)
	print("id:"..id)
	if(id == EVENT_ID_BEGIN)then
		MoveV(1,3,-400)
	elseif(id == 1)then
		if frenyFlag then
			Attack1()
		else
			ChangBullets1()	
		end	
		MoveH(2,1,0)
	elseif(id == 2)then
		MoveH(3,1,0)
	elseif(id == 3)then
		if frenyFlag then
			Attack2()
		end
		MoveH(4,1,-0)
	elseif(id == 4)then
		MoveH(5,1,0)
		if frenyFlag then
			ChangBulletsF3()
		else
			ChangBullets2()
		end	
	elseif(id == 5)then
		MoveH(6,3,-200)
   elseif(id == 6)then
		if frenyFlag then
			Attack1()
		else
			ChangBullets1()	
		end	
		MoveH(7,1,0)
   elseif(id == 7)then
		MoveH(8,3,200)
	elseif(id == 8)then
		if frenyFlag then
			Attack2()
		end
		MoveH(9,3,250)
	elseif(id == 9)then
		MoveH(10,3,-250)
	elseif(id == 10)then
		if frenyFlag then
			ChangBulletsF4()
		else
			ChangBullets2()
		end
		MoveH(11,3,-200)	
	elseif(id == 11)then
		Attack1()
		MoveH(12,2,0)	
	elseif(id == 12)then
		MoveH(13,0,-0)	
	elseif(id == 13)then
		NextID = 14
		if frenyFlag then			
			MoveH(20,1.5,500)	
		else
			MoveH(14,3,200)	
		end	
	elseif(id == 14)then
		Attack2()	
		MoveH(1,1,0)	
		
	elseif(id == 20)then
		Ready()
		BindEmitter(Emit_1,laser)
		OpenFire()
		local x,y = Node.xGetPosition(Self)
		MoveTo(21,2,100,y)
	elseif(id == 21) then
		local x,y = Node.xGetPosition(Self)
		MoveTo(22,2,550,y)
	elseif(id == 22) then
		Attack1()
		local x,y = Node.xGetPosition(Self)
		MoveTo(NextID,1,300,y)
	--				
	elseif(id == EVENT_ID_OVER)then
	end
end

function OnEventOver(id)
end