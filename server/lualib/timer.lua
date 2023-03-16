local skynet = require "skynet"

local M = {}

local srv_name = ".timer_sys"

local callback_list = {}

local function _add_callback(id,func,...)
    local node = {
        func = func,
        data = table.pack(...)
    }
    callback_list[id] = node
end

--单次定时任务
function M.timeout(delay,func,...)
    local id = skynet.call(srv_name,"lua","timeout",delay)
    _add_callback(id,func,...)
    return id
end

--TODO  把delay删除
--多次定时任务
function M.add_loop_timer(delay, interval,times,func,...)
    assert(times>0,times)
    local id = skynet.call(srv_name,"lua","add_loop_timer",delay,interval,times)
    _add_callback(id,func,...)
    return id
end

--永久定时任务
function M.add_forever_timer(delay,interval,func,...)
    assert(interval>0,interval)
    local id = skynet.call(srv_name,"lua","add_forever_timer",delay,interval)
    _add_callback(id,func,...)
    return id
end

--移除定时任务
function M.remove_timer(timer_id)
    if callback_list[timer_id] ~= nil then
        local is_remove = skynet.call(srv_name,"lua","remove_timer",timer_id)
        if is_remove ~= nil then
            callback_list[timer_id] = nil
        end
    end
end

--申请最小堆空间
function M.init()
    skynet.send(srv_name,"lua","timer_init")
end

--释放最小堆空间
function M.release()
    skynet.send(srv_name,"lua","timer_release")
end

--回调执行
function M.timer_callback(timer_id,is_remove)
    assert(timer_id>0,timer_id)
    callback_list[timer_id].func(table.unpack(callback_list[timer_id].data))
    if is_remove then
        callback_list[timer_id] = nil
    end
end

return M