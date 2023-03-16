--local OO = require "OO"
local OO = require "lualib.OO"
local utilities = require "lualib.utilities"

local Probability = OO.Class{
    type = "Probability",
    __call = function (self)
        return utilities.random(1,1000) <= self[1]
    end
}

return Probability