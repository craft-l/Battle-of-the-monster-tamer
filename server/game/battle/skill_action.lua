local Ac = require "lualib.Action.Action"
local If = Ac.If
local Delay = Ac.Delay
local Probability = Ac.Probability
local Sequence = Ac.Sequence
local And = Ac.And
local Or = Ac.Or
local Repeat = Ac.Repeat
local Hurt = require "game.battle.Hurt"
local PickTarget = require "game.battle.PickTarget"
local Recover = require "game.battle.Recover"
local AddBuff = require "game.battle.AddBuff"

local skill_action = {}

function skill_action:Init()
    self.action = {}

    self.action[2001] = function(cfg)
        return Sequence{
            PickTarget{cfg.details.target_details},
            Hurt{cfg.details.damage,cfg.details.damage_type}
        }
    end

    self.action[2002] = function(cfg)
        return Sequence{
            PickTarget{cfg.details.target_details},
            AddBuff{cfg}
        }
    end

    self.action[2003] = function(cfg)
        return Sequence{
            PickTarget{cfg.details.target_details},
            Hurt{cfg.details.damage,cfg.details.damage_type},
            If{Probability{cfg.details.buff_probability},AddBuff{cfg}}
        }
    end

    self.action[2004] = function(cfg)
        return Sequence{
            PickTarget{cfg.details.target_details},
            Hurt{cfg.details.damage,cfg.details.damage_type}
        }
    end

end

function skill_action:GetActionCreator(skill_id)
    return self.action[skill_id]
end

return skill_action
