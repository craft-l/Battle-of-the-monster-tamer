local skynet = require "skynet"
local sprotoloader = require "sprotoloader"

local max_client = 64

local function start_timer()
	local timer_srv = skynet.newservice("timer_sys")
end

local function start_events()
	local event_sys = skynet.newservice("event_sys")
end

local function start_data()
	local data_sys = skynet.newservice("statistics_sys","127.0.0.1",27017,"game_data","gameadmin","349578")
end

local function start_test()
	--skynet.newservice("test_db_event_fight")
	--skynet.newservice(
	--	"test_db_event_fight","127.0.0.1",27017,"bag_data","gameadmin","349578","weapon")
	--skynet.newservice(
	--	"test_db_event_resource","127.0.0.1",27017,"game_data","gameadmin","349578")
	--skynet.newservice("test_event_publish")
	--skynet.newservice("test_event_subscribe")
	--skynet.newservice("test_sort")
	--skynet.newservice("test_single_srv_event")
	--skynet.newservice("test_timer")

end

local function start_watchdog()
	skynet.error("Server start")
	skynet.uniqueservice("protoloader")
	if not skynet.getenv "daemon" then
		local console = skynet.newservice("console")
	end
	--skynet.newservice("simpledb")
	local watchdog = skynet.newservice("watchdog")
	skynet.call(watchdog, "lua", "start", {
		port = 8888,
		maxclient = max_client,
		nodelay = true,
	})
	skynet.error("Watchdog listen on", 8888)
end

skynet.start(function()
	skynet.newservice("debug_console",8000)

	start_timer()
	start_events()
	--start_data()
	start_watchdog();
	start_test()

	print("服务启动")
	skynet.exit()
end)
