#include <stdarg.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <math.h>

#include <lua.h>
#include <lauxlib.h>
#include <lualib.h>


//打印
static void stackDump(lua_State* L){
    printf("\nbegin dump lua stack");

    int i = 0;
    int top = lua_gettop(L);
    for (i = 1; i <= top; ++i) {
        int t = lua_type(L, i);
        switch (t) {
            case LUA_TSTRING:
            {
                printf("'%s' ", lua_tostring(L, i));
            }
                break;
            case LUA_TBOOLEAN:
            {
                printf(lua_toboolean(L, i) ? "true " : "false ");
            }break;
            case LUA_TNUMBER:
            {
                printf("%g ", lua_tonumber(L, i));
            }
                break;
            default:
            {
                printf("%s ", lua_typename(L, t));
            }
                break;
        }
    }
    printf("\nend dump lua stack");
}

static void print_arr(int *arr, int n)
{
	printf("\n----------\n");
	for(int i = 0; i< n; i++)
	{
		printf("%d", arr[i]);
	}
	printf("\n-----------\n");
}

static int l_sort(lua_State* L)
{
	int len;
	int *arr;	

	if(!lua_istable(L,1))                      //如果第一个参数不是表则退出
	{
		lua_pushnil(L);
		return -1;
	}
		
	len = luaL_checknumber(L,2);               //获取第二个参数--数组长度
	arr = (int*)malloc(len * sizeof(int));     //申请数组空间
	for(int i = 1; i <= len; i++)
	{
	lua_rawgeti(L, 1, i);                      //获取表对应下表的元素
	arr[i - 1] = lua_tointeger(L, -1);
	lua_pop(L, 1);                             //把上一个内容出栈
	}
	lua_settop(L, 0);

    //排序调用
	insert_sort(arr,len);

	lua_newtable(L);                           //新建一个表用来传递数组
	for(int i = 0; i < len; i++)
	{
		lua_pushinteger(L, arr[i]);
		lua_seti(L, -2, i+1);
	}
	return 1;  //一个参数
}


/* 需要一个luaL_Reg类型的结构体，其中每一个元素对应一个提供给lua的函数
 * 每一个元素中包含此函数在Lua中的名字，以及该函数在C库中的函数指针。
 * 最后一个元素为“哨兵元素”（两个"NULL"），用于告诉Lua没有其他的函数需要注册。
 */
static const struct luaL_Reg sort[] = {
	{"sort",l_sort},
	{NULL, NULL}
};

/* 此函数为C库中的“特殊函数”。
 * 通过调用它注册所有C库中的函数，并将它们存储在适当的位置。
 * 此函数的命名规则应遵循：
 * 1、使用"luaopen_"作为前缀。
 * 2、前缀之后的名字将作为"require"的参数。
 */
extern int luaopen_sort(lua_State* L)
{
    /* void luaL_newlib (lua_State *L, const luaL_Reg l[]);
     * 创建一个新的"table"，并将"l"中所列出的函数注册为"table"的域。
     */ 
	luaL_newlib(L, sort);

    return 1;
}
