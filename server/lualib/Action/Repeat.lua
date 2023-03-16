--local OO = require "OO"
local OO = require "lualib.OO"

local Repeat = OO.Class{
    __index = {
        Start = function(self,data)
            self.data = data
            self.loop = self[1]
            self.hadLoop = 0
            self.action = self[2]
            self.loopActionIndex = 0
        end,
        IsDone = function(self)
            return self.hadLoop >= self.loop
        end,
        Update = function(self,deltaTime)
            if self.loop - self.hadLoop > 0 then
                if self.hadLoop == self.loopActionIndex then
                    self.loopActionIndex = self.loopActionIndex + 1
                    self.action:Start(self.data)
                end
                self.action:Update(deltaTime)
                if self.action:IsDone() then
                    self.hadLoop = self.hadLoop + 1
                end
            end
        end
    },
}

return Repeat