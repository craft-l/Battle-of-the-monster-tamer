local skynet = require "skynet"
local mongo = require "skynet.db.mongo"
local dbsave = require "dbsave"
local timer = require "timer"
local events = require "events"
local command = require "command"

local data = {}

local host, port, db_name, username, password, collection = "127.0.0.1",27017,"bag_data","gameadmin","349578","weapon"
 
local player_id = {
	[1] = 2513,
	[2] = 330
}
if port then
	port = math.tointeger(port)
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

local c = _create_client()
local db = c[db_name]

local function _read_db_data()

	for _,playerid in pairs(player_id) do
		data[playerid] = db[collection]:findOne({playerid = playerid})
	end

end

local function test()
	local ok, err, ret, cur



	db.fightdata:drop()
	--db.fightdata:safe_insert({player_id = 77})
	--db.fightdata:safe_update({player_id = 77},{["$set"] = {["怪物击杀"]={["小拳石击杀数"] = 6,["蓝蘑菇击杀数"] = 7}}}, true)
	--嵌套更新
	--db.fightdata:safe_update({player_id = 77},{["$set"] = {["怪物击杀.小拳石击杀数"] = 6}}, true)
	--db.fightdata:safe_update({enemyname = "绿蘑菇"},{["$set"] = {killcount = 13}}, true, true)
	

end


local function init()
    skynet.dispatch("lua", function(session, address,cmd, ...)
        local f = assert(command[cmd],cmd)
        f(...)
    end)

	dbsave.dbsave_init(host, port, db_name, username, password)

	if collection then
		_read_db_data()
	end

	command.register_callback("timer_callback", timer.timer_callback)

	--timer.add_forever_timer(1,1,_kill_random_enemy)

	dbsave.timed_save(50,collection,player_id,data)

end

skynet.start(function()
    init()
	test()

end)



