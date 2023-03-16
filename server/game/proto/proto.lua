local sprotoparser = require "sprotoparser"

local proto = {}

--client to server
--REQUEST
proto.c2s = sprotoparser.parse [[
.package {
	type 0 : integer
	session 1 : integer
}

sayhello 1 {
	request {
	  what 0 : string
	}
	response {
	  error_code 0 : integer
	  msg 1 : string
	}
}

]]

--RESPONCE
proto.s2c = sprotoparser.parse [[
.package {
	type 0 : integer
	session 1 : integer
}

heartbeat 2 {}
]]

return proto


