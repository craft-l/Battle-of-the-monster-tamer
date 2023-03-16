local M = {}

--注册回调
function M.register_callback(callback_name,callback)
    M[callback_name] = callback
end

return M