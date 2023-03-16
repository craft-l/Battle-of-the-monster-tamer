local Ac = require "lualib.Action.Action"
local OO = require "lualib.OO"
local utilities = require "lualib.utilities"

local function _pick_all_random(buff_list)
    local result
    for _,v in pairs(buff_list) do
        --TODO 在buff中添加混乱表直接匹配
    end
    return result
end

local function _get_target(min_num,max_num, target)
    local target_num = math.random(min_num,max_num)
    if #target <= target_num then
        return
    end

    local len = #target - target_num
    for i=1,len do
        utilities.remove_random_value(target)
    end
end

local function _target(self,type)
    local target = {}
    local flag = self.target_details.select_type[type]
    if self.data.buff_list then
        if _pick_all_random(self.data.buff_list) then  
            flag = 3
        end
    end

    if flag == 1 then
        for _,monster in pairs(self.friend) do
            table.insert(target,monster)
        end
    elseif flag == 2 then              
        for _,monster in pairs(self.enemy) do
            table.insert(target,monster)
        end
    elseif flag == 3 then
        for _,monster in pairs(self.friend) do
            table.insert(target,monster)
        end
        for _,monster in pairs(self.enemy) do
            table.insert(target,monster)
        end
    end

    _get_target(self.target_details.min_num[type],self.target_details.max_num[type],target)
    return target
end

local PickTarget = OO.Class{
    __index = {
        --data 施法者
        Start = function(self,data)
            self.data = data
            self.target_details = self[1]
            self.friend = self.data.friend
            self.enemy = self.data.enemy
        end,
        IsDone = function(self)
            return true
        end,
        Update = function(self)
            self.data.hurt_target = {}
            self.data.recover_target = {}
            if self.target_details.select_type.hurt then
                local hurt_target = _target(self,"hurt")
                self.data.hurt_target = hurt_target
            end
            if self.target_details.select_type.recover then
                local recover_target = _target(self,"recover")  
                self.data.recover_target = recover_target
            end
        end
    },
}

return PickTarget