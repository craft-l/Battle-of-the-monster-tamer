
local skynet = require "skynet"
local s = require "service"

s.cat_food_price = 5
s.cat_food_cnt = 0

s.resp.buy = function (source)
	-- call（addr, type） 用type类型发送一个消息到addr并等待回应
	--node1 是地址， worker1是服务，change_money为加钱函数
	--减去价格，扣费
    local left_money = s.call("node1", "worker1", "change_money", -s.cat_food_price)

    if left_money >= 0 then
        s.cat_food_cnt = s.cat_food_cnt + 1
        skynet.error("buy cat food ok, current cnt: " .. tostring(s.cat_food_cnt))
        return true
    end
	--如果购买失败，则把钱加回去
    skynet.error("buy cat food failed, money not enough")
    s.call("node1", "worker1", "change_money", s.cat_food_price)
    return false
end

s.start(...)


