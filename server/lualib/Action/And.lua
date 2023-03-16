local OO = require "OO"

local And = OO.Class{
    __index = {
        Start = function(self,data)
            self.data = data
        end,
        IsDone = function(self)
        end,
        Update = function(self,deltaTime)

        end
    },
}

return And