local mongo = require "skynet.db.mongo"
local bson = require "bson"
local dbsave = require "dbsave"
local skynet = require "skynet"
local events = require "events"
require "skynet.manager"

local host, port, db_name, username, password, collection = "127.0.0.1",27017,"game_data","gameadmin","349578","data"
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

local CMD = {}

local subscribed_list = {}        --记录下被订阅的字段
local player_id_list = {          --玩家id
	[1] = 2513,
	[2] = 330
}         
local data = {}                   --总数据

--初始读取数据库中数据
local function init_data()
    
end

--每次数据更新都判断该数据是否被订阅，若被订阅则广播
--直接广播就行
local function _publish()
    --if subscribe_list[] then
    --    events.publish()
    --end
end

--记录订阅字段消息
function CMD.publish()
    --if not subscribe_list[] then
    --    subscribe_list[] = 
    --end
end

function CMD.update_statistics_data(playerid,update_data)
    player_id_list[playerid] = playerid
    data[playerid] = update_data
    dbsave.total_data_save(collection,player_id_list,data)
    skynet.error(playerid)
end

local function init()
    skynet.dispatch("lua",function(_,address,cmd,...)
        local f = assert(CMD[cmd],cmd)
        f(...)
        end)
    skynet.register(".statistics_sys")
end

skynet.start(function()
    init()
end)