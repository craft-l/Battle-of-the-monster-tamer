--洗牌算法，用于等概率打乱

shuffle = {1, 2, 3, 4, 5,6,7,8,9}
function ex4(shuffle)
    for v,k in pairs(shuffle) do
        tempRand = math.random(1, #shuffle)
        --print("v:    ".. v)
        --print("rand: " .. tempRand)
        shuffle[v], shuffle[tempRand] = shuffle[tempRand], shuffle[v]
    end

    for v,k in pairs(shuffle) do
        print(k)
    end
end
