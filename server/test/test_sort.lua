local skynet = require "skynet"
local sort = require "sort"
local timer = require "timer"
local command = require "command"
local tool = require "tool"
require "skynet.manager"


local arr = {2,5,13,13}

--添加
function command.add_num(num)
	table.insert(arr,num)
	skynet.error("get:",num)
end

--调用排序
function command.sort()
	if tool.table_len(arr) ~= 0 then
        arr = sort.sort(arr,tool.table_len(arr))
        skynet.error("排序完成")
    end
end

--删除最后一位数
function command.remove_last_num()
    if tool.table_len(arr) ~= 0 then
        skynet.error("删除数字：",arr[tool.table_len(arr)])
	    arr[tool.table_len(arr)] = nil
    end
end

--打印数组
function command.print_arr()
	command.sort()
    if tool.table_len(arr) ~= 0 then
        print("arr:")
	    local text = ""
	    for k,v in pairs(arr) do
	    	text = text..'  '..tostring(v)
	    end
	    print(text)
    end
end

local function i()
	while true do
		skynet.sleep(100)
		num = math.random(1,300)
		skynet.error("插入数字：",num)
		table.insert(arr,num)
		command.print_arr()
	end	
end

--定时打印数组
function command.add_print_timer()
	timer.add_forever_timer(1,2,command.print_arr)
end

local function init()
	skynet.dispatch("lua", function(session, address,cmd, ...)
		local f = command[cmd]
		if f then --如果有对应函数则执行
			f(...)
			skynet.response()(true)
		else
			skynet.error(string.format("错误命令 %s", tostring(cmd)))
		end
	end)
	skynet.register(".sort")
    --command.register_callback("timer_callback", timer.timer_callback)
    --command.add_print_timer()
	skynet.fork(i)
	
end

skynet.start(function()
    init()
end
)
