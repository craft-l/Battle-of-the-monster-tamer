local skynet = require "skynet"
require "skynet.manager"

----------------------------------------------------------------------------
--另规定一个协议是想把事件相关的消息做个区分

local PNAME = "event"
local PID = 51
skynet.register_protocol {
    name = PNAME,
    id = PID,
    --pack = skynet.pack,
    unpack = skynet.unpack,
}
----------------------------------------------------------------------------
local ECMD = {}

local subscribe_list = {}

--回调函数接收的参数由发布者给订阅者
--订阅是用PNAME协议，发布还是使用lua协议，直接匹配其event_callback
--TODO 改成多个 跨服务订阅时一个服务订阅一个事件只能传一个callback，如果该服务有多个地方都要订阅该事件，那可以合成一个函数在函数内再进行判断
function ECMD.notify(_,event_type,...)
    local suber_list = subscribe_list[event_type]
    --若之前没有注册对应的事件，则添加该事件表
    if not suber_list then 
        subscribe_list[event_type] = {}
        return
    end
    for subscriber,callback in pairs(suber_list) do
        skynet.send(subscriber,"lua",callback,...)
    end
end

--TODO 没有必要传名字、可以采用timer的方案传输id并且把回调函数地址储存在本服务
--为了方便服务集中管理自身的数据
function ECMD.subscribe(address,event_type,callback)
    if not subscribe_list[event_type] then
        subscribe_list[event_type] = {}
    end
    subscribe_list[event_type][address] = callback
    skynet.error("订阅成功！","address:",address,"  callback:",callback)
    return skynet.retpack(true)
end

function ECMD.unsubscribe(address,event_type)
    if not subscribe_list[event_type][address] then
        return skynet.retpack(false)
    end
    subscribe_list[event_type][address] = nil 
    return skynet.retpack(true)
end

skynet.dispatch(PNAME,function(_,address,cmd,event_type,callback,...)
    assert(ECMD[cmd],cmd)
    return ECMD[cmd](address,event_type,callback,...)
end
)

-----------------------------------------------------------------------------


-----------------------------------------------------------------------------

local CMD = {}

local function init()
    skynet.dispatch("lua",function(_,address,cmd,...)
        local f = assert(CMD[cmd],cmd)
        f(...)
        end)
    skynet.register(".event_sys")
end

skynet.start(function()
    init()
end)