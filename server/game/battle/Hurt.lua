local Ac = require "lualib.Action.Action"
local OO = require "lualib.OO"
local utilities = require "lualib.utilities"

local function _damage(self,target)
    local damage = {}
    damage.atk =
        function()
            target.attr_list.hp = target.attr_list.hp - math.floor(self[1]/100*self.data.attr_list.atk)
        end
    damage.spA =
        function()
            target.attr_list.hp = target.attr_list.hp - math.floor(self[1]/100*self.data.attr_list.spA)
        end
    damage.True = 
        function()
            target.attr_list.hp = target.attr_list.hp - self[1]
        end
    damage[self[2]]()
end

--TODO 检索表返回是否成功
local function _check_buff()

end

local Hurt = OO.Class{
    __index = {
        Start = function(self,data)
            self.data = data
            self.target = utilities.clone_table(self.data.hurt_target)
        end,
        IsDone = function(self)
            return true
        end,
        Update = function(self)            
            for _,target in pairs(self.target) do
                _check_buff()
                _damage(self,target)
                if target.attr_list.hp <= 0 then
                    target.attr_list.hp = 0
                end
                print(target.name,"got hurt",self[1],"当前血量:",target.attr_list.hp)
            end       
        end
    },
}

return Hurt