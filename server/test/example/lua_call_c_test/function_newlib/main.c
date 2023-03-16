#include <stdarg.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <math.h>
#include <lua.h>
#include <lauxlib.h>
#include <lualib.h>
#include <dirent.h>
#include <errno.h>

static double sub(double a , double b)
{
	return a - b;
}


static double multi(double a, double b)
{
	return a * b;
}
//注意这里的返回值不能是double
static int l_sub(lua_State* L)
{
	double d = luaL_checknumber(L, 1);
	double e = luaL_checknumber(L, 2);
	lua_pushnumber(L, sub(d,e));
	return 1;
}

/* 需要一个luaL_Reg类型的结构体，其中每一个元素对应一个提供给lua的函数
 * 每一个元素中包含此函数在Lua中的名字，以及该函数在C库中的函数指针。
 * 最后一个元素为“哨兵元素”（两个"NULL"），用于告诉Lua没有其他的函数需要注册。
 */
static const struct luaL_Reg mySub[] = {
	{"Sub",l_sub},
	{NULL, NULL}
};

/* 此函数为C库中的“特殊函数”。
 * 通过调用它注册所有C库中的函数，并将它们存储在适当的位置。
 * 此函数的命名规则应遵循：
 * 1、使用"luaopen_"作为前缀。
 * 2、前缀之后的名字将作为"require"的参数。
 */
extern int luaopen_mySub(lua_State* L)
{
    /* void luaL_newlib (lua_State *L, const luaL_Reg l[]);
     * 创建一个新的"table"，并将"l"中所列出的函数注册为"table"的域。
     */ 
	luaL_newlib(L, mySub);

    return 1;
}
