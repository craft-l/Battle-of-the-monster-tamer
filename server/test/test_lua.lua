local monster = require "game.config.conf_monster"
local Delay = require "lualib.Action.Delay"
local CallFunc = require "lualib.Action.CallFunc"
local Sequence = require "lualib.Action.Sequence"
local Probability = require "lualib.Action.Probability"
local If = require "lualib.Action.If"
local Repeat = require "lualib.Action.Repeat"
local battle_sys = require "game.battle.battle_sys"
local utilities = require "lualib.utilities"


local function TestDelay()
    local action = Delay{2}
    action:Start()
    action:Update(1)
    local a,b = action:IsDone()
    print("a",a,"b",b)
    action:Update(1)
    a,b = action:IsDone()
    print("a",a,"b",b)
    action:Update(1)
    a,b = action:IsDone()
    print("a",a,"b",b)
end

local function TestSequence()
    local actionA = CallFunc {function(data)
		data.value = "a"
	end}
	local actionB = CallFunc {function(data)
		data.value = "b"
	end}
	local actionC = CallFunc {function(data)
		data.value = "c"
	end}
	local action = Sequence{ actionA, Delay{1}, actionB, actionC }
	local testTbl = {value = "0"}
	action:Start(testTbl)
    print(action:IsDone())
    print("未开始")
    print("A",actionA:IsDone())
    print("B",actionB:IsDone())
    print("C",actionC:IsDone())
    action:Update(1)
    print("A完成")
    print("A",actionA:IsDone())
    print("B",actionB:IsDone())
    print("C",actionC:IsDone())
	print(action:IsDone())
	--在Sequence里的Delay,就算是时间到了，也要下次Update才会运行Delay后面的Action
    action:Update(1)
    print("全部完成")
    print(action:IsDone())
    print("A",actionA:IsDone())
    print("B",actionB:IsDone())
    print("C",actionC:IsDone())

	local action = Sequence{ actionA, Delay{1000}, actionB, Delay{1000}, actionC }
	local testTbl = {value = "0"}
	action:Start(testTbl)
	action:Update(1000)
	print(action:IsDone())
	action:Update(1000)

	--最后一个是Delay
	local action = Sequence{ actionA, Delay{1000} }
	local testTbl = {value = "0"}
	action:Start(testTbl)
	action:Update(500)
	print(action:IsDone())
    action:Update(500)
    
end

local function TestProbability()
    local a = Probability{500}
    print("a",a.type)
    mt = getmetatable(a)
    print("a.mt",mt.type)
    print("mtmt",getmetatable(mt).type)
    for i=1,10 do
        print(a())
    end
end

local function TestIf()
    local TAction = CallFunc{function (data)
        data.value = "true"
    end}
    local FAction = CallFunc{function (data)
        data.value = "False"
    end}
    local testData =  {value = "testValue"}
    local IfAction = If{true,TAction,FAction}
    IfAction:Start(testData)
    print(testData.value)
    IfAction:Update()
    print(testData.value)
    testData.value = "testValue"
    print(testData.value)
    local IfAction = If{Probability{500},TAction,FAction}
    for i=1,10 do
        IfAction:Start(testData)
        IfAction:Update()
        print(testData.value)
    end
end

local function TestRepeat()
    addAction = CallFunc{function(data)
        data.loop = data.loop + 1
        print("add,value:",data.loop)
    end}
    subAction = CallFunc{function(data)
        data.loop = data.loop - 1
        print("sub,value:",data.loop)
    end}
    local testValue = {loop = 1}
    local sequence = Sequence{Repeat{5,addAction},Repeat{3,subAction}}
    sequence:Start(testValue)
    for i=1,6 do
        sequence:Update(1)
    end
    print(sequence:IsDone())
    sequence:Update(1)
    print(sequence:IsDone())
end

--TestDelay()
--TestSequence()
--TestProbability()
--TestIf()
--TestRepeat()


local function TestBattle()
    --myTeam = {1 = monster[0003],3 = monster[0002]}
    local enemy = {monster_list = {[1] =utilities.deep_clone_table(monster[3]),[3] = utilities.deep_clone_table(monster[2])},details = {role = 1}}
    local own = {monster_list = {[1] = utilities.deep_clone_table(monster[9]),[2] = utilities.deep_clone_table(monster[7]),[3] = utilities.deep_clone_table(monster[6])},details = {role = 2}}

    --enemy.monster_list[3].name = "修改"

    battle_sys.battle{own,enemy}
    for _,m in pairs(enemy.monster_list) do
        print(m.name,m.attr_list.hp)
    end
    for _,m in pairs(own.monster_list) do
        print(m.name,m.attr_list.hp)
    end

end

TestBattle()

