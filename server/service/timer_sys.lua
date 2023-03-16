local skynet = require "skynet"
local min_heap = require "minheaptimer"
local utilities = require "utilities"
require "skynet.manager"

local TIMES_FOREVER = -1
local can_run = false       --申请最小堆空间并初始化为true，释放空间为false

local CMD = {}

local timer_list = {}       --记录定时任务
local address_list = {}     --记录定时的服务地址

local inc_id = 0            --任务递增id

local function _add_timer(delay,interval,times)
    inc_id = inc_id + 1
    local node = {
        id = inc_id,
        expire = os.time() + delay,
        interval = interval,
        times = times or TIMES_FOREVER,
    }
    timer_list[inc_id] = node
    CMD.add_minheap_node(node.id,node.expire)
    return inc_id
end

function CMD.timeout(delay)
    return _add_timer(delay,0,1)
end

function CMD.add_loop_timer(delay, interval,times)
    return _add_timer(delay,interval,times)
end

function CMD.add_forever_timer(delay,interval)
    return _add_timer(delay,interval,nil)
end

function CMD.add_minheap_node(id,expire)
    min_heap.add_node(id,expire)
end

function CMD.timer_switch()
    if can_run then
        can_run = false
    else
        can_run = true
    end
end

function CMD.timer_release()
    min_heap.release()
    CMD.timer_switch()
end

function CMD.timer_init()
    min_heap.init()
    CMD.timer_switch()
end

function CMD.remove_timer(timer_id)
    if timer_list[timer_id] then
        min_heap.delete_node(timer_id)
        timer_list[timer_id] = nil
        return timer_id
    end
end

local function _update()
    local timeout_list = min_heap.update(os.time())     --记录从c层返回的到期任务
    local timer_is_remove_list = {}                     --记录需要删除的任务并返回给timer
    local timer_readd_list = {}

    --1： lua虚拟栈一次性传输的数据量有限，所以需要用while来反复取堆顶符合条件的任务
    --2： 因为要反复去取，那么在取完后就不能在while中又压入，所以增加了一个readd表，当取完超时任务后再将重复任务压入堆中
    while utilities.table_len(timeout_list) ~= 0 do
        for i,id in ipairs(timeout_list) do
            if timer_list[id].times ~= TIMES_FOREVER then
                timer_list[id].times = timer_list[id].times - 1
            end
            if timer_list[id].times ~= 0 then
                table.insert(timer_readd_list,id)
                timer_is_remove_list[id] = false
            else
                timer_list[id] = nil
                timer_is_remove_list[id] = true
            end
        end
        timeout_list = min_heap.update(os.time())
    end

    if #timer_readd_list ~= 0 then
        for _,id in ipairs(timer_readd_list) do
            timer_list[id].expire=timer_list[id].expire+timer_list[id].interval
            CMD.add_minheap_node(timer_list[id].id,timer_list[id].expire)
        end
    end

    return timer_is_remove_list
end

--循环检测到期任务并返回
function CMD.start_timer()
	while can_run do
        --检测间隔，也是任务最小执行间隔
        skynet.sleep(50)

        local timeout_list = _update()
        if utilities.table_len(timeout_list) ~= 0 then
            for id,is_remove in pairs(timeout_list) do
                skynet.send(address_list[id],"lua","timer_callback",id,is_remove)
            end
        end    

    end
end

skynet.init(function()
	CMD.timer_init()
	skynet.fork(CMD.start_timer)
end
)

local function init()
    skynet.dispatch("lua", function(session, address,cmd, ...)
        local f = CMD[cmd]
        if f then
            local id = f(...)
            if id then
                address_list[id] = address
            end
            skynet.retpack(id)
        else
            skynet.error(string.format("错误命令 %s",tostring(cmd)))
        end
    end)
    skynet.register(".timer_sys")
end

skynet.start(function()
    init()
end)