local Delay = require "lualib.Action.Delay"
local CallFunc = require "lualib.Action.CallFunc"
local Sequence = require "lualib.Action.Sequence"
local Probability = require "lualib.Action.Probability"
local If = require "lualib.Action.If"
local Repeat = require "lualib.Action.Repeat"


local Action = {
    Delay = Delay,
    Sequence = Sequence,
    Probability = Probability,
    If = If,
    Repeat = Repeat,
}

return Action