--面向对象父类
local OO = {}

OO.Class = {
    type   = "Class",
    __call = function (class, o) return setmetatable(o, class) end,
}
setmetatable(OO.Class, OO.Class)

return OO