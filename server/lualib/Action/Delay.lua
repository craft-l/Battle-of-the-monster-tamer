--local OO = require "OO"
local OO = require "lualib.OO"

local Delay = OO.Class{
    __index = {
        Start = function(self)
            self.elpased = 0
        end,
        --不包括本回合
        IsDone = function(self)
            return self.elpased > self[1]
        end,
        Update = function(self,deltaTime)
            self.elpased = self.elpased + deltaTime
        end
    },
}

return Delay