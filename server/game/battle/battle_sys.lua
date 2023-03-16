--local skynet = require "skynet"
local skill = require "game.config.conf_skill"
local monster = require "game.config.conf_monster"
local buff = require "game.config.conf_buff"
local Ac = require "lualib.Action.Action"
local utilities = require "lualib.utilities"
local skillAc = require "game.battle.skill_action"
local buffAc = require "game.battle.buff_action"
local SkillAcMgr = require "game.battle.SkillAcMgr"
local BuffAcMgr = require "game.battle.BuffAcMgr"

local battle_sys = {}

local function _init(t1,t2)
    local function init(a,b)
        for _,v in pairs(a) do
            v.priority = 1
            v.enemy = {}
            for _,e in pairs(b) do
                table.insert(v.enemy,e)
            end
            v.friend = {}
            for _,f in pairs(a) do
                table.insert(v.friend,f)
            end
            v.buff_list = {}
        end
    end

    init(t1,t2)
    init(t2,t1)
end

--后续出先攻等效果再加
local function _update_sequence(sequence)
    utilities.table_sort(sequence,"attr_list","spe")
    utilities.table_sort(sequence,"priority")
end

local function _update_skill_sequence(monster)
    monster.skill_updated = true
    --print("将突击战法调整至普通攻击之后")
end

local function _update_buff_state(monster)
    print(monster.name,"buff list",#monster.buff_list)
    --伤害类型效果需要在行动前检测
    if utilities.table_len(monster.buff_list) then
        for k,buff in pairs(monster.buff_list) do
            buff:Update(1)
            if buff:IsDone() then
                monster.buff_list[k] = nil
            end
        end
    end
end

local function _update_user_state(monster,skill_id)
    --检查人物状态，再更新技能状态    
    if not monster[skill_id] then
        monster[skill_id] = {cooldown = 0,can_release = true}
    end

    --更新部队列表
    for k,friend in pairs(monster.friend) do
        if friend.attr_list.hp == 0 then
            monster[k] = nil
        end
    end
    for k,enemy in pairs(monster.enemy) do
        if enemy.attr_list.hp == 0 then
            monster.enemy[k] = nil
        end
    end
    for _,v in pairs(monster.buff_list) do
        --cfg检索,麻痹等概率buff效果需要在每一个技能释放前检测
    end

end

local function _check_end(troop1,troop2)
    if troop1.monster_list[1].attr_list.hp == 0 or troop2.monster_list[1].attr_list.hp == 0 then
        return true
    else
        return false
    end
end

local function _skill_update(monster,skill_id)
    if monster[skill_id].cooldown == 0 and monster[skill_id].can_release then
        local skill_action_creator = skillAc:GetActionCreator(skill_id)
        local skill_action = skill_action_creator(skill[skill_id])
        skill_action:Start(monster)
        print(monster.name,"使用",skill[skill_id].name)
        skill_action:Update(1)
        monster[skill_id].cooldown = skill[skill_id].details.cooldown
    else
        print(skill[skill_id].name,"还未冷却")
        monster[skill_id].cooldown = monster[skill_id].cooldown - 1
    end
end

local function _prepare_round(sequence)
    print("准备回合")
    for _,m in pairs(sequence) do
        for _,v in pairs(m.skill_list) do
            --print("检查",m.name,"的",skill[v].name,"技能")
        end
    end
end

local function _update_hp(old,new)
    for k,v in pairs(new.monster_list) do
        old.monster_list[k].attr_list.hp = v.attr_list.hp
    end
end

function battle_sys.battle(data)
    local troop1 = utilities.deep_clone_table(data[1])
    local troop2 = utilities.deep_clone_table(data[2])
    local monster_list1 = troop1.monster_list
    local monster_list2 = troop2.monster_list
    local battle_end = false
    local sequence = utilities.table_merge(monster_list1,monster_list2)
    _init(monster_list1,monster_list2)
    skillAc:Init()
    buffAc:Init()
    --准备回合,发动准备战法
    _prepare_round(sequence)
    --八回合
    for i=1,12 do
        --角色出招排列
        print("第",i,"回合")
        _update_sequence(sequence)  
        for k,m in pairs(sequence) do
            --排序用的是数字键，死亡判断放在这
            if m.attr_list.hp > 0 then
                --技能顺序
                if not m.skill_updated then
                    _update_skill_sequence(m)
                end
                _update_buff_state(m)
                for _,skill_id in pairs(m.skill_list) do
                    --check释放者状态
                    _update_user_state(m,skill_id)
                    _skill_update(m,skill_id)
                    if _check_end(troop1,troop2) then
                        battle_end = true
                        break
                    end
                end
            end
            if battle_end then
                break
            end
        end
        if battle_end then
            break
        end
    end
    print("战斗结束！")
    --调整血量
    _update_hp(data[1],troop1)
    _update_hp(data[2],troop2)
    troop1 = nil
    troop2 = nil
end

return battle_sys