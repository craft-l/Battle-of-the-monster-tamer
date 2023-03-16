local config = {
    --[[
        [] = {
            id = , name = , details = {

            },
        desc = ""
        },
    ]]
    [3001] = {
        id = 3001, name = "寄生", details = {
            damage = 60, recover_rate = 60,damage_type = "True",exist_round = 2
        }
    },
    [3002] = {
        id = 3002, name = "灼烧", details = {
            damage = 50, damage_type = "spA",exist_round = 2
        }
    },
}

return config