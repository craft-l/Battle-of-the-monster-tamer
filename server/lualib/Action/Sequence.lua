--local OO = require "OO"
local OO = require "lualib.OO"

--[[

]]
local Sequence = OO.Class {
    __index = {
        Start = function(self,data)
            self.data = data
            self.curActionIndex = 1
            self.startActionIndex = 0
            self.isDone = false
        end,
        IsDone = function(self)
            return self.isDone
        end,
        Update = function(self, deltaTime)
            local actionNum = #self
            for i = self.curActionIndex,actionNum do
                self.curActionIndex = i
                local action = self[i]
                --判断当前action是否初始化
                if i > self.startActionIndex then
                    action:Start(self.data)
                    self.startActionIndex = i
                end
                action:Update(deltaTime)
                if action:IsDone() then
                    if i == actionNum then
                        self.isDone = true
                    end
                else
                    break
                end
            end
        end
    },
}

return Sequence