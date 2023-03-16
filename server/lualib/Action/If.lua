--local OO = require "OO"
local OO = require "lualib.OO"

--[[
    Action.If{条件，action1，action2}
    三元运算，第一个参数为条件action，为true则运行第一个action，false运行第二个
    条件情况：①直接输入bool类型
            ②Action1发生的可能性Probability
]]
local If = OO.Class{
    __index = {
        Start = function(self,data)
            self.data = data
            self.action = nil
        end,
        IsDone = function(self)
            local isDone
            if self.action then
                isDone = self.action:IsDone()
            else
                isDone = true
            end
            return isDone
        end,
        Update = function(self,deltaTime)
            if self.action == nil then
                local condition = self[1]
                local isTrue

                if type(condition) == "table" then
                    isTrue = condition()
                elseif type(condition) == "boolean" then
                    isTrue = condition
                end
                
                if isTrue then
                    self.action = self[2]
                else
                    self.action = self[3]
                end

                if self.action then
                    self.action:Start(self.data)
                end
            end

            if self.action then
                self.action:Update(deltaTime)
            end
        end
    },
}

return If