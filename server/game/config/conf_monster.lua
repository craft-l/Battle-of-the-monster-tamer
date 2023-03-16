--[[
    attr_list:属性列表
    skill_list:技能列表
]]
local _attr = function(attr)
    a = {}
    a.hp = attr[1]
    a.atk = attr[2]
    a.def = attr[3]
    a.spA = attr[4]
    a.spD = attr[5]
    a.spe = attr[6]
    return a
end
--[[
    element 元素
    role 战士、法师、护卫、射手
]]
local monster = function(id,name,element,attr,skills,role)
    m = {}
    m.id = id
    m.name = name
    m.element = element
    m.attr_list = _attr(attr)
    m.skill_list = skills
    m.role = role
    return m
end


local config = {
    --[[
    [0001] = {
        id = 0001, name = "妙蛙种子", type = 
        attr_list = {450,49,49,65,65,45},
        skill_list = {skill_id = 2001},
    },
    [0002] = {
        id = 0002, name = "妙蛙草"，
        attr_list = {600,62,63,80,80,60},
        skill_list = {skill_id = 2001},
    },
    [0003] = {
        id = 0003, name = "妙蛙花",
        attr_list = {800,82,83,100,100,80},
        skill_list = {skill_id = 2001},
    },
    [0004] = {
        id = 0004, name = "小火龙",
        attr_list = {390,52,43,60,50,65},
        skill_list = {skill_id = 2002},
    },
    [0005] = {
        id = 0005, name = "火恐龙",
        attr_list = {580,64,58,80,65,80},
        skill_list = {skill_id = 2002},
    },
    [0006] = {
        id = 0006, name = "喷火龙",
        attr_list = {780,84,78,109,85,100},
        skill_list = {skill_id = 2002},
    },
    [0007] = {
        id = 0007, name = "杰尼龟",
        attr_list = {440,48,65,50,64,43},
        skill_list = {skill_id = 2003},
    },
    [0008] = {
        id = 0008, name = "卡咪龟",
        attr_list = {590,63,80,65,80,58},
        skill_list = {skill_id = 2003},
    },
    [0009] = {
        id = 0009, name = "水箭龟",
        attr_list = {790,83,100,85,105,78},
        skill_list = {skill_id = 2003},
    },]]
    [0001] = monster(0001,"妙蛙种子",       "草",  {450,49,49,65,65,45},       {[1] = 2002,[4] =  2001},    {0.8,0.7,1.2,1}),
    [0002] = monster(0002,"妙蛙草",         "草",  {600,62,63,80,80,60},       {[1] = 2002,[4] =  2001},    {0.8,0.7,1.2,1}),
    [0003] = monster(0003,"妙蛙花",         "草",  {800,82,83,100,100,80},     {[1] = 2002,[4] =  2001},    {0.8,0.7,1.2,1}),
    [0004] = monster(0004,"小火龙",         "火",  {390,52,43,60,50,65},       {[1] = 2003,[4] =  2001},    {1.2,1,1,0.8}),
    [0005] = monster(0005,"火恐龙",         "火",  {580,64,58,80,65,80},       {[1] = 2003,[4] =  2001},    {1.2,1,1,0.8}),
    [0006] = monster(0006,"喷火龙",         "火",  {780,84,78,109,85,100},     {[1] = 2003,[4] =  2001},    {1.2,1,1,0.8}),
    [0007] = monster(0007,"杰尼龟",         "水",  {440,48,65,50,64,43},       {[1] = 2004,[4] =  2001},    {0.7,0.7,1.2,1.2}),
    [0008] = monster(0008,"卡咪龟",         "水",  {590,63,80,65,80,58},       {[1] = 2004,[4] =  2001},    {0.7,0.7,1.2,1.2}),
    [0009] = monster(0009,"水箭龟",         "水",  {790,83,100,85,105,78},     {[1] = 2004,[4] =  2001},    {0.7,0.7,1.2,1.2}),
}


return config