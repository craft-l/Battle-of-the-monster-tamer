local skynet = require "skynet"
require "skynet.manager"
skynet.start(function()
	skynet.error("[start main]")
	
	-- TODO
	
	local inputCal = skynet.newservice("inputCal", "inputCal",1)
	local outputCal = skynet.newservice("outputCal", "outputCal",1)	
	skynet.send(inputCal,"lua", "getRandomNum")
	skynet.sleep(200)
	skynet.send(inputCal,"lua", "sendNum")	
	skynet.sleep(200)
	skynet.send(outputCal, "lua", "cal_add")	
	skynet.name("outputCal",outputCal)
	skynet.exit()
end
)
