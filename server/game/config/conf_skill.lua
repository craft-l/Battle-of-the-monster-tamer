--[[
prepare_round:准备回合数
condition:学习条件 
cd:cooldown
attack_num:攻击目标数量
target_type:释放对象，1为友方，2为敌方，3为全体
damage_rate百分制
]]
local function details(type,element,prepare_round,cooldown,max_num,min_num,target_type,damage_rate)
    local table = {}
    local target_details = {}
    target_details.max_num = max_num
    target_details.min_num = min_num
    target_details.target_type = target_type
    table.lv = lv
    table.type = type
    table.element = element
    table.prepare_round = prepare_round
    table.cooldown = cooldown
    table.damage_rate = damage_rate
    table.target_details = target_details
    return table
end

local config = {
    [2001] = {
        id = 2001, name = "普通攻击", details = {
            damage = 100,cooldown = 0,damage_type = "atk",
            target_details = {select_type = {hurt = 2},min_num = {hurt = 1},max_num = {hurt = 1}},
        }, 
        desc = {"普通攻击"}
    },
    [2002] = {
        id = 2002, name = "寄生种子", details = {
            prepare_round = 1,cooldown = 2,buff_id = 3001,
            target_details = {select_type = {hurt = 2, recover = 1},min_num = {hurt = 1,recover = 1},max_num = {hurt = 1, recover = 1}}
        }, 
        desc = {"准备一回合，随机选取一个敌人种下持续三回合寄生种子并为我方一人恢复血量"}
    },
    [2003] = {
        id = 2003, name = "火花",  details = {
            damage = 50,cooldown = 0,buff_probability = 500,damage_type = "spA",buff_id = 3002,
            target_details = {select_type = {hurt = 2}, min_num = {hurt = 1},max_num = { hurt = 1}}
        }, 
        desc = {"对一个敌人发射火花，并有概率灼烧敌人"}
    },
    [2004] = {

        id = 2004, name = "水炮",  details = {
            damage = 70,cooldown = 0,damage_type = "atk",
            target_details = {select_type = {hurt = 2},min_num= {hurt = 2},max_num = {hurt = 2}}
        }, desc = {"对两个敌人发射水炮，并且使其附着水元素"}
    },
}

return config