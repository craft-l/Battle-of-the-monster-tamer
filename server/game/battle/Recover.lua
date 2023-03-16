local Ac = require "lualib.Action.Action"
local OO = require "lualib.OO"
local utilities = require "lualib.utilities"

--TODO 检索表返回是否成功
local function _check_buff()

end

local Recover = OO.Class{
    __index = {
        Start = function(self,data)
            self.data = data
            self.target = utilities.clone_table(self.data.recover_target)
        end,
        IsDone = function(self)
            return true
        end,
        Update = function(self)        
            for _,v in pairs(self.target) do
                _check_buff()
                v.attr_list.hp = v.attr_list.hp + self[1]
                print(v.name,"回复血量",self[1],"当前血量",v.attr_list.hp)
            end
        end
    },
}

return Recover