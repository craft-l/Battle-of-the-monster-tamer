local events = require "events"
local skynet = require "skynet"
require "skynet.manager"

local command = {}

----------------------------------------------
local time = 0

--发布
function command.time_update(time)
    events.publish_local("time_update_event",time)
end

function command.turn_on(time)
    if time % 2 == 0 then
        skynet.error("turn on")
    end
end

function command.turn_down(time)
    if time % 2 == 1 then
        skynet.error("turn down")
    end
end

local function get_time_update(time)
    skynet.error(time)
    command.turn_on(time)
    command.turn_down(time)
end

--模拟数据变动
local function _time_increases()
    while true do
        skynet.sleep(100)
        time = time % 24 + 1
        command.time_update(time)
    end
end
----------------------------------------------
---------------服务内---------------
function command.subscribe_local(event_type,callback)
    events.subscribe_local(event_type,command[callback])
end

function command.unsubscribe_local(event_type,callback)
    events.unsubscribe_local(event_type,command[callback])
end

skynet.init(function()
    skynet.fork(_time_increases)

    command.time_update(0)

    --events.subscribe_local("time_update_event",get_time_update)
    events.subscribe_local("time_update_event",command["turn_on"])
    events.subscribe_local("time_update_event",command["turn_down"])


end
)

skynet.start(function()
    skynet.dispatch("lua", function(session, address,cmd, ...)
        local f = command[cmd]
        if f then
            f(...)
            skynet.response(true)
        else
            skynet.error(string.format("错误命令 %s",tostring(cmd)))
        end
    end)
    skynet.register(".event_single_srv")
end

)