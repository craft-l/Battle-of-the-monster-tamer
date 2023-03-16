
local skynet = require "skynet"
local s = require "service"

s.resp.cal_add = function(source, arg1, arg2)
	local sum = arg1 + arg2
	skynet.error("sum:"..tostring(sum).."	arg1:"..tostring(arg1).."	s.arg2:"..tostring(arg2))
end

s.start(...)
