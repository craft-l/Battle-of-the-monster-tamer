local skynet = require "skynet"
local timer = require "timer"
local command = require "command"
require "skynet.manager"


local   function _execute(srv_name)
	skynet.error(srv_name,"执行")
end

function command.timeout(delay)
        timer.timeout(delay,_execute,"worker1 timeout")
end

function command.add_loop_timer(delay,interval,times)
        timer.add_loop_timer(delay,interval,times,_execute,"worker1 loop timer")
end

function command.add_forever_timer(delay,interval)
        return timer.add_forever_timer(delay,interval,_execute,"worker1 forever timer")
end

function command.remove_timer(id)
        timer.remove_timer(id)
        skynet.error("删除id为:",id,"任务")
end

local function test_add_timer()
        --数量测试
        --[[while true do
                skynet.sleep(10)
                for i=1,100000 do
                        command.add_forever_timer(2,10)
                        command.add_loop_timer(2,2,2)
                        command.timeout(2)
                end
        end]]

        --
        --skynet.error("第一个forever timer id:",command.add_forever_timer(0,0.5))
        --command.add_loop_timer(0,2,2)

        --释放
        --timer.release()
        --重新申请
        --timer.init()

        local id
        id = timer.add_forever_timer(0,2,_execute,"永久任务")
        skynet.error("添加 永久任务 id为:",id)
        id = timer.add_loop_timer(1,2,5,_execute,"五次重复任务")
        skynet.error("添加 五次重复任务 id为:",id)
        --skynet.error("删除id为:",id,"任务")
        --重复取消同一任务
        id = timer.timeout(3,command.remove_timer,1)
        --timer.timeout(3,command.remove_timer,id)
        id = timer.timeout(0,_execute,"单次定时任务")
        skynet.error("添加 单次定时任务 id为:",id)

end

local function init()
        skynet.dispatch("lua", function(session, address,cmd, ...)
                local f = assert(command[cmd],cmd)
                f(...)
        end
        )
        command.register_callback("timer_callback", timer.timer_callback)
        skynet.register(".test_timer")
        --测试
        skynet.fork(test_add_timer)
end

skynet.start(function()
        init()
end)
