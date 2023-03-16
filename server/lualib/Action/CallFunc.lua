--local OO = require "OO"
local OO = require "lualib.OO"

--测试用
local CallFunc = OO.Class {
    __index = {
        Start = function(self, data)
            self.data = data
        end,
        IsDone = function(self)
            return self.isDone
        end,
        Update = function(self)
            --以data表中只有一个函数
            self[1](self.data)
            self.isDone = true
        end
    },
}

return CallFunc