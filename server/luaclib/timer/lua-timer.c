#include "min-heap.h"
#include <lua.h>
#include <lauxlib.h>
#include <lualib.h>
#include <unistd.h>


//删除指定元素
int l_delete_node(lua_State* L)
{
	if (is_empty())
	{
		return 0;
	}

	int last = mh.size;
	int idx = find_node(lua_tointeger(L, 1));
	//若不存在则退出
	if (idx < 0)return 0;

	if (last != idx)
	{
		swap_node(last, idx);
		remove_last_node();
		if (!shift_down(idx))
		{
			shift_up(idx);
		}
	}
	else
	{
		remove_last_node();
	}
	reduce_capacity();
	return 0;
}

//根据接受的消息生成节点
int l_add_node(lua_State* L)
{
	if (is_full())
	{
		if(!expand_capacity())
		{
			//扩容失败需要返回错误
			lua_pushinteger(L,1);
			return 1;
		}
	}
	int pos = ++mh.size;
	mh.node[pos].idx = pos;

	//前面的参数先入栈，按正数取参
	mh.node[pos].id = lua_tointeger(L, 1);
	mh.node[pos].expire = lua_tointeger(L, 2);

	add_hash_node(mh.node[pos].id, mh.node[pos].idx);
	shift_up(pos);
	return 0;
}

//检查任务是否已经到期
int l_update(lua_State*L)
{
    int time = lua_tointeger(L,1);
    lua_pop(L,1);
	
	if(!is_empty() && time != 0)
	{
		int num = 1;
		lua_newtable(L);
		while (time >= mh.node[1].expire&&mh.size > 0)
		{
			//id作为key
			lua_pushnumber(L, num);
			lua_pushnumber(L, mh.node[1].id);
			l_pop_root();
			lua_rawset(L,-3);	

			num++;
			//不能超过栈的容量
			if(num > 1000)
			{
				break;
			}
		}
		return 1;
	}
	else
	{
		return 0;
	}
}

const struct luaL_Reg minheap_timer[] = {
	{"init",l_init_heap},
	{"add_node",l_add_node},
	{"update",l_update},
	{"delete_node",l_delete_node},
	{"release",l_release_minheap},
	{NULL, NULL}
};

extern int luaopen_minheaptimer(lua_State* L)
{
	luaL_newlib(L, minheap_timer);
	return 1;
}

