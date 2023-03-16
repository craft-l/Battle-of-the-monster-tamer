local M = {}

M.seed = 1

local function _get_seed()
    if (M.seed > 99999) then
        M.seed = 1
    end
    M.seed = M.seed + 1
    return M.seed * os.time()
end

--表长
function M.table_len(list)
    local len = 0
    if list ~= nil then
        for _ in pairs(list) do
            len = len + 1
        end
    else
        return 0
    end
    return len
end

--插入排序,table_sort(table,key1,key2)
function M.table_sort(table,...)
    local i = 1
    local j = 1
    local arg = {...}
    for i=2,#table do
        for j=i,2,-1 do
            if #arg == 1 then
                if table[j][arg[1]] < table[j-1][arg[1]] then
                    table[j],table[j-1] = table[j-1],table[j]
                end
            elseif #arg == 2 then
                if table[j][arg[1]][arg[2]] < table[j-1][arg[1]][arg[2]] then
                    table[j],table[j-1] = table[j-1],table[j]
                end
            end
        end
    end
end

--合并表
function M.table_merge(table1,table2)
    local table = {}
    local i = 1
    for _,v in pairs(table1) do
        table[i] = v
        i = i + 1
    end
    for _,v in pairs(table2) do
        table[i] = v
        i = i + 1
    end
    return table
end

function M.random(min,max)
    math.randomseed(_get_seed())
    return math.random(min,max)
end

--随机取值
function M.get_random_value(table)
    math.randomseed(_get_seed())
    return table[math.random(1,#table)]
end

function M.get_random_value_hash(table)
    local key = {}
    local n = 1
    for k in pairs(table) do
        key[n] = k
        n = n + 1
    end
    math.randomseed(_get_seed())
    --math.randomseed(tostring(os.time()):reverse():sub(1, 6))
    return table[key[math.random(1,n-1)]]
end

--随机移除表中一个元素
function M.remove_random_value(table)
    local key = {}
    local n = 1
    for k in pairs(table) do
        key[n] = k
        n = n + 1
    end
    math.randomseed(_get_seed())
    table[key[math.random(1,n-1)]] = nil
end

--克隆一份table
function M.clone_table(tab)
    local t = {}
    for k,v in pairs(tab) do
        t[k] = v
    end
    return t
end
function M.deep_clone_table(tab)
    local function copy(target, res)
        for k,v in pairs(target) do
            if type(v) ~= "table" then
                res[k] = v;
            else
                res[k] = {};
                copy(v, res[k])
            end
        end
    end

    local result = {}
    copy(tab, result)
    return result
end

--将一个表加入到另外一个表
function M.insert_table(a,b)
    for _,v in pairs(b) do
        table.insert(a,v)
    end
end

--打印table
function M.print_table(table)
    for k1,v1 in pairs(table) do
        print("k1,",k1,"    v1,",v1)
        if type(v1) == "table" then
            for k2,v2 in pairs(v1) do
                print("k2,",k2,"    v2,",v2)
                if type(v2) == "table" then
                    for k3,v3 in pairs(v2) do
                        print("k3,",k3,"    v3,",v3)
                    end
                end
            end
        end
    end
end

return M