
function OnInit()
	print("lua OnInit")
	
	emit1 = CreateBullet("boss-f0.bt")
	emit2 = CreateBullet("boss-f1.bt")
	emit3 = CreateBullet("boss-f1.bt")
	
	FrenzyEmit1 = CreateBullet("boss2.bt")
	FrenzyEmit2 = CreateBullet("boss-f3.bt")
	FrenzyEmit3 = CreateBullet("boss-f3.bt")
	
	FrenzyEmit5 = CreateBullet("boss9.bt")
	
	FrenzyEmit6 = CreateBullet("boss7.bt")
	FrenzyEmit7 = CreateBullet("jingying3.bt")	
	FrenzyEmit8 = CreateBullet("jingying3.bt")	
	
	attackEmit2 = CreateBullet("boss7.bt")
	attackEmit3 = CreateBullet("boss7.bt")
	
	attackEmit2_2 = CreateBullet("jingying25.bt")
	attackEmit3_2 = CreateBullet("jingying25.bt")
	
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

--下一个id
NextID = 1

Left = 100		
Right = 550
Mid = 300

function Wait(id,time)
	MoveH(id,time,0)
end

function Move(id,time,dir)
	local x,y = Node.xGetPosition(Self)
	MoveTo(id,time,dir,y)
end

function OnEventCall(id)
	print("id:"..id)
	if(id == EVENT_ID_BEGIN)then
		local x,y = Node.xGetPosition(Self)
		MoveTo(1,2,x,600)
	elseif(id == 1)then
		if frenyFlag then
			Attack1()
		else
			ChangBullets1()	
		end	
		Wait(2,3)
	elseif(id == 2)then
		Move(3,3,Right)
	elseif(id == 3)then
		if frenyFlag then
			Attack2()
		end
		Move(4,3,Left)
	elseif(id == 4)then
		Move(5,3,Right)
		if frenyFlag then
			ChangBulletsF3()
		else
			ChangBullets2()
		end	
	elseif(id == 5)then
		NextID = 6
		if frenyFlag then			
			Move(20,1,Right)	
		else		
			Move(6,3,Mid)
		end
   elseif(id == 6)then
		if frenyFlag then
			Attack1()
		else
			ChangBullets1()	
		end	
		Wait(7,3)
   elseif(id == 7)then
		Move(8,3,Right)
	elseif(id == 8)then
		if frenyFlag then
			Attack2()
		end
		Move(9,3,Left)
	elseif(id == 9)then
		Move(10,3,Right)
	elseif(id == 10)then
		if frenyFlag then
			ChangBulletsF4()
		else
			ChangBullets2()
		end
		Move(11,3,Mid)	
	elseif(id == 11)then
		Attack1()
		Wait(12,3)	
	elseif(id == 12)then
		Move(13,3,Left)	
	elseif(id == 13)then
		NextID = 14
		if frenyFlag then			
			Move(20,1,Right)	
		else
			Move(14,3,Mid)	
		end	
	elseif(id == 14)then
		Attack2()	
		Wait(1,3)			
	--超级射线	
	elseif(id == 20)then
		Ready()
		BindEmitter(Emit_1,laser)
		OpenFire()
		Move(21,2,Left)
	elseif(id == 21) then
		local x,y = Node.xGetPosition(Self)
		Move(22,2,Right)
	elseif(id == 22) then
		Attack1()
		local x,y = Node.xGetPosition(Self)
		Move(NextID,1,Mid)
	--				
	elseif(id == EVENT_ID_OVER)then
	end
end

function OnEventOver(id)
end