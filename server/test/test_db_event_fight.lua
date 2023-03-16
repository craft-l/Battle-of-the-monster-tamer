local skynet = require "skynet"
local mongo = require "skynet.db.mongo"
local dbsave = require "dbsave"
local timer = require "timer"
local events = require "events"
local command = require "command"

local data = {}

local host, port, db_name, username, password, collection = "127.0.0.1",27017,"fight_data","gameadmin","349578","guanzhong"


local player_id_list = {
	[1] = 2513,
	[2] = 330
}
if port then
	port = math.tointeger(port)
end

local c
local db

local CMD = {}

--print(host, port, db_name, username, password)
data = {
	[player_id_list[1]] = {
		six_level_land = 3,
		five_level_land = 14,
		savage = {
			seven_level_savage = 22,
			six_level_savage = 66
		}
	},
	[player_id_list[2]] = {
		six_level_land = 15,
		five_level_land = 37,
		seven_level_land = 3
	}

}

local function _total_data_save()
	for _,playerid in pairs(player_id_list) do
		db[collection]:safe_update({player_id = playerid},{["$set"] = data[playerid]},true)
		--db[collection]:safe_update({player_id = playerid},data[playerid],true)
	end
end

local function _create_client()
	return mongo.client(
		{
			host = host, port = port,
			username = username, password = password,
			authdb = db_name,
		}
	)
end

local function _print_data()
	for _,id in pairs(player_id_list) do
		--skynet.error("player_id",id,"data[id]",data[id])
		for k,v in pairs(data[id]) do
			if type(v) == "table" then
				for key2,value2 in pairs(v) do
					skynet.error("savage:",key2,"count:",value2)
				end
			else
				skynet.error("land:",k,"count:",v)
			end
		end
	end
end

local function _read_db_data()

	for _,playerid in pairs(player_id_list) do
		data[playerid] = db[collection]:findOne({player_id = playerid})
	end

end

local function test()
	local ok, err, ret, cur

	--db.fightdata:drop()
	--db.fightdata:safe_insert({player_id = 77})
	--db.fightdata:safe_update({player_id = 77},{["$set"] = {["怪物击杀"]={["小拳石击杀数"] = 6,["蓝蘑菇击杀数"] = 7}}}, true)
	--嵌套更新
	--db.fightdata:safe_update({player_id = 77},{["$set"] = {["怪物击杀.小拳石击杀数"] = 6}}, true)
	--db.fightdata:safe_update({enemyname = "绿蘑菇"},{["$set"] = {killcount = 13}}, true, true)
	

end
--[[
local function _kill_enemy(tag)
	if not data[tag] then
		data[tag] = 1
	end
	data[tag] = data[tag] + 1
	--dbsave.dbsave(collection,player_id,tag,data[tag])
	events.publish("击杀怪物",tag,data[tag] )
end

local function _kill_random_enemy()
	local enemytype = math.random(1,4)
	local tag
	if enemytype == 1 then
		_kill_enemy("绿蘑菇击杀数")
	elseif enemytype == 2 then
		_kill_enemy("蓝蘑菇击杀数")
	elseif enemytype == 3 then
		_kill_enemy("小拳石击杀数")
	end
end
]]

function CMD.print_arr(arr,key)
	local getk,getv
	if type(arr) == "table" then
		for k,v in pairs(arr) do
			--getk, getv = 
			if	key then
				CMD.print_arr(v,tostring(key).."-"..tostring(k))
			else
				CMD.print_arr(v,tostring(k))
			end
		end
	end	
	--skynet.error("getkey",getk,"getv",getv)
	if type(arr) ~= "table" then
		skynet.error("key",key,"arr",arr)
	end

	return key,arr
end

local function init()
    skynet.dispatch("lua", function(session, address,cmd, ...)
        local f = assert(command[cmd],cmd)
        f(...)
    end)


	c = _create_client()
	db = c[db_name]

	if collection then
		--_read_db_data()
		--_print_data()
	end

	--_total_data_save()

	--print(data[player_id_list[1]])

	dbsave.dbsave_init(host, port, db_name, username, password)

	dbsave.total_data_save(collection,player_id_list,data)

	command.register_callback("timer_callback", timer.timer_callback)

	--CMD.print_arr(data)

end

skynet.start(function()
    init()
	test()

end)

