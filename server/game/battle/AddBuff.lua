local OO = require "lualib.Action.OO"
local utilities = require "lualib.utilities"
local buffAc = require "game.battle.buff_action"
local buff = require "game.config.conf_buff"

--TODO 刷新、不能添加、叠加
local function _check_buff()

end

local AddBuff = OO.Class{
    __index = {
        -- data 添加buff者
        Start = function(self,data)
            self.data = data
        end,
        IsDone = function(self)
            return true
        end,
        Update = function(self,deltaTime)
            local buff_id = self[1].details.buff_id
            local skill_data = self[1]
            local target = utilities.clone_table(self.data.hurt_target)
            local buff_cfg = utilities.deep_clone_table(buff[buff_id])
            local buff_action_creator = buffAc:GetActionCreator(buff_id)
            local buff_action = buff_action_creator(buff_cfg)
            local buff_data = {}
            buff_data.hurt_target = utilities.clone_table(self.data.hurt_target)
            buff_data.recover_target = utilities.clone_table(self.data.recover_target)
            buff_data.attr_list = utilities.clone_table(self.data.attr_list)
            buff_action:Start(buff_data)

            --没有效果的buff的select_type也默认为hurt
            if skill_data.details.target_details.select_type.hurt then
                for _,target in pairs(self.data.hurt_target) do
                    _check_buff()
                    print(target.name,"附加了",buff_cfg.name)
                    table.insert(target.buff_list,buff_action)
                end
            else
                for _,target in pairs(self.data.recover_target) do
                    _check_buff()
                    print(target.name,"附加了",buff_cfg.name)
                    table.insert(target.buff_list,buff_action)
                end
            end
        end
    },
}

return AddBuff