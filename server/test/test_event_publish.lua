local events = require "events"
local skynet = require "skynet"
require "skynet.manager"

local command = {}

local time = 0

function command.test_pub(time)
    events.publish("time_update_event",time)
end

--模拟数据变动
local function _time_increases()
    while true do
        skynet.sleep(100)
        time = time % 24 + 1
        command.test_pub(time)
        skynet.error("发布：",time)
    end
end

local function init()
    skynet.dispatch("lua", function(session, address,cmd, ...)
        local f = assert(command[cmd],cmd)
        f(...)
    end)
    skynet.register(".publisher")
    skynet.fork(_time_increases)
end

skynet.start(function()
    init()
end
)