local skynet = require "skynet"
local s = require "service"

s.sum = 0
s.arg1 = 0
s.arg2 = 2

s.init = function()

end

s.resp.getRandomNum = function(source, ...)
	--skynet.error("start GetRandomNum")
		s.arg2 = math.random(-100,100)
		local strTime = tostring(os.time())
		local strRev = string.reverse(strTime)
		local strRandomTime = string.sub(strRev, 1, 6)
		math.randomseed(strRandomTime)

		s.arg1 = math.random(-100, 100)
end

s.resp.sendNum = function(source)
	skynet.error("send...arg1:"..s.arg1.."	arg2:"..s.arg2)
	skynet.send("outputCal","lua","cal_add",s.arg1, s.arg2)
end

s.start(...)
