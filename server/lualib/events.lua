local skynet = require "skynet"

local PNAME = "event"
local PID = 51
skynet.register_protocol {
    name = PNAME,
    id = PID,
    pack = skynet.pack,
    unpack = skynet.unpack,
}

local M = {}

--------------------------------------------------------------------------------
--跨服callback传名字

local events_sys_name = ".event_sys"

--发布
function M.publish(event_type,...)
    skynet.send(events_sys_name,PNAME,"notify",event_type,...)
end

--订阅
function M.subscribe(event_type,callback)
    skynet.send(events_sys_name,PNAME,"subscribe",event_type,callback)
end

--取消订阅
function M.unsubscribe(event_type)
    skynet.send(events_sys_name,PNAME,"unsubscribe",event_type)
end

--------------------------------------------------------------------------------
--本服callback传地址

--储存本服发布的事件
local event_list = {}

--发布本服务消息
function M.publish_local(event_type,...)
    local callback_list = event_list[event_type]
    if not callback_list then
        event_list[event_type] = {}
        return
    end
    for _,callback in pairs(callback_list) do
        callback(...)
    end
end

--订阅本服务事件
function M.subscribe_local(event_type,callback)
    if not event_list[event_type] then
        skynet.error("订阅未注册事件")
        return false
    end
    event_list[event_type][callback] = callback
    return true
end

--取消订阅本服务事件
function M.unsubscribe_local(event_type,callback)
    if not event_list[event_type][callback] then
        skynet.error("未订阅事件:")
        return false
    end
    event_list[event_type][callback] = nil
    return true
end

--------------------------------------------------------------------------------
--订阅数据库内容
--因为订阅时将令data_sys自动注册对应事件，所以用另外一套接口

local data_sys_name = ".data_sys"

function M.subscribe_db()
    --skynet.send(data_sys_name,"lua","publish",event_type,...)                    --告知data_sys广播对应内容
    --skynet.send(events_sys_name,PNAME,"subscribe",event_type,callback)          --向events_sys订阅对应内容
end

function M.unsubscribe_db()
    --skynet.send(data_sys_name,"lua","publish",event_type,...)
    --skynet.send(events_sys_name,PNAME,"subscribe",event_type,callback)
end

--------------------------------------------------------------------------------

return M