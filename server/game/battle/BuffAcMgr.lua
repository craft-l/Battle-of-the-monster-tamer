local OO = require "lualib.Action.OO"

local BuffAcMgr = OO.Class{
    __index = {
        Start = function(self)
        end,
        IsDone = function(self)
        end,
        Update = function(self,deltaTime)
            for _,v in pairs(self) do
                if v:IsDone() then
                    v = nil
                else
                    v:Update(deltaTime)
                end
            end
        end
    },
}

return BuffAcMgr