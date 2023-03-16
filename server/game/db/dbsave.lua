local skynet = require "skynet"
local mongo = require "skynet.db.mongo"
local bson = require "bson"
local checkout = require "dbsave_checkout"

local M = {}

local host, port, db_name, username, password 

local function _create_client()
	return mongo.client(
		{
			host = host, port = port,
			username = username, password = password,
			authdb = db_name,
		}
	)
end

local ok, err
local c
local db

local statistics_data = {}

--初始化
function M.dbsave_init(host1, port1, db_name1, username1, password1)
	host, port, db_name, username, password = host1, port1, db_name1, username1, password1
	if port then
		port = math.tointeger(port)
	end
	c = _create_client()
	db = c[db_name]
end

--发送更新数据至data_sys
--TODO可以把一部分数据判断放在本服务再传输给统计服务
local function _update_statistics_sys(playerid)
	skynet.send(".statistics_sys","lua","update_statistics_data",playerid,statistics_data)
	statistics_data = {}
end

--TODO 从配置文件中读取格式对更新数据进行校验
local function _checkout(collection)
	return true
end
		--db[collection]:safe_update({player_id = playerid},data[playerid],true)

--合成key，方便将需要传输的数据转换成统计库的形式
local function _transfer_format(value,key)
	if type(value) == "table" then
		for k,v in pairs(value) do
			if	key then
				_transfer_format(v,tostring(key).."-"..tostring(k))
			else
				_transfer_format(v,tostring(k))
			end
		end
	end	

	if type(value) ~= "table" then
		statistics_data[key] = value
	end

	return key,value
end

--全量更新
function M.total_data_save(collection,player_id_list,data)
	c = _create_client()
	db = c[db_name]
	local check = _checkout(data)
	
	_transfer_format(data)

	if check then
		for _,playerid in pairs(player_id_list) do
			db[collection]:safe_update({player_id = playerid},{["$set"] = data[playerid]},true)
			_update_statistics_sys(playerid,statistics_data)
		end
	end

    return ok, err
end

--[[
--二层嵌套文档部分更新
--TODO key的命名
function M.dbsave_second_level(collection,playerid,key,...)
	local args = {...}

	for i = 1,#args,2 do
		ok,err = db[collection]:safe_update({player_id = playerid},{["$set"] = {[key.."."..args[i]-] = args[i+1]}}, true)
	end

	_checkout(...)
	_update_data_sys()

    return ok, err
end
s
----TODO 做以下文档处理时 数据处理(转换)、校验
--一、二层嵌套文档处理,传入整个表进行更新
--query 用于 player_id 的匹配
function M.second_level_update(query,data)
	for k1,v1 in pairs(data) do
		if type(v1) == "table" then
			for key2,value2 in pairs(v1) do
				db[collection]:safe_update(query,{["$set"] = {[k1.."."..key2] = value2}}, true)
				skynet.error("key",key2,"value",value2)
			end
		else
			db[collection]:safe_update(query,{["$set"] = {[k1] = v1}}, true)
		end
	end
end
]]


--定时存盘
local function _timed_save(interval,collection,playerid,data)
	while true do
		skynet.sleep(interval)
		M.second_level_update({player_id = playerid},data)
	end
end

function timed_save(interval,collection,playerid,data)
	skynet.fork(_timed_save,interval,collection,playerid,data)
end

return M