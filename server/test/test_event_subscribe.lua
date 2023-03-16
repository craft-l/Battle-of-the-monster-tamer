local events = require "events"
local skynet = require "skynet"
local command = require "command"
local timer = require "timer"
local achievement = require "achievement"
require "skynet.manager"

function command.send_num_to_sort(time)
    skynet.error("接受：",time)
    if time % 2 == 0 then
        local num = math.random(1,300)
        skynet.error("发送",num," 给sort")
        --skynet.send(".sort","lua","add_num",num)
    end
    if time % 3 == 0 then
        local num = math.random(1,300)
        skynet.error("删除最后一个数")
        --skynet.send(".sort","lua","remove_last_num")
    end
end

--[[
function command.kill_enemy_achievement(event_type,killcount)
    skynet.error("event type",event_type,"killcount",killcount)
    if achievement[event_type] then
        if achievement[event_type].killcount == killcount then
            skynet.error("恭喜达成",achievement[event_type].achievement,"成就！")
        end
    end
end]]

function command.subscribe(event_type,callback)
    events.subscribe(event_type,callback)
end

function command.unsubscribe(event_type)
    events.unsubscribe(event_type)
end

local function init()
    skynet.dispatch("lua", function(session, address,cmd, ...)
        local f = assert(command[cmd],cmd)
        f(...)
    end)
    skynet.register(".subscribe")
    command.register_callback("timer_callback", timer.timer_callback)

    --events.subscribe("time_update_event","send_num_to_sort",time)
    --2秒后订阅事件
    timer.timeout(1,command.subscribe,"time_update_event","send_num_to_sort",time)
end

skynet.start(function()
    init()
end)