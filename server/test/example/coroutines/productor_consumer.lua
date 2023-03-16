
local newProductor

function productor()
    local production = 0
    while true do
        product(production)
    end
end

function consumer()
    while true do
        consume()
    end
end

function product(production)
    while production < 10 do
        production = production + math.random(2,5)
        print("产品数     "..production)
    end
        print("停止生产")
        os.execute(" sleep " .. 1 .. "s")
        coroutine.yield(production)
end

function consume()
    local status,producion = coroutine.resume(newProductor)
    while producion >= 0 do
        producion = producion - 1
        print("消费")
    end
        print("停止消费")
    os.execute(" sleep " .. 1 .. "s")
end

newProductor = coroutine.create(productor)
consumer()
