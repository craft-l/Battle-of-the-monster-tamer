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

local buff_action = {}

function buff_action:Init()
    self.action = {}

    self.action[3001] = function(cfg)
        return Sequence{
            Repeat{cfg.details.exist_round,
                Sequence{
                        Hurt{cfg.details.damage,cfg.details.damage_type},
                        Recover{cfg.details.recover_rate}} 
            }
        }
    end

    self.action[3002] = function(cfg)
        return Sequence{
            Repeat{cfg.details.exist_round,
                    Hurt{cfg.details.damage,cfg.details.damage_type},
            }
        }
    end


end

function buff_action:GetActionCreator(buff_id)
    return self.action[buff_id]
end

return buff_action
