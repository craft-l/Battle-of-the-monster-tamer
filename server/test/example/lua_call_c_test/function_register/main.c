#include <stdarg.h>
#include <stdio.h>
#include <stdlib.h>
#include <math.h>
#include <lua.h>
#include <lauxlib.h>
#include <lualib.h>



static double add(double a , double b)
{
return a + b;
}

static double addTest(double c)
{
return 1 + c;
}

static int l_add(lua_State* L)
{
//取得给定虚拟栈中指定索引元素并且转换为数字
double d = luaL_checknumber(L,1);
double e = luaL_checknumber(L,2);
//压入所取数字
//当前栈为 3.0（结果） 2（add(1,2d)的第二个参数） 1（add（1，2）的第一个参数）后面的为地址
lua_pushnumber(L,add(d,e));
//返回的结果个数，如果返回太多则无法显示前面的结果
return 4;
}

int main(void)
{
//创建虚拟机
lua_State* L = luaL_newstate();
//打开lua状态机中“L”中的所有Lua标准库
luaL_openlibs(L);
//将c函数定义为lua的全局变量addFunc
lua_register(L, "add", l_add);


//所要执行的命令
const char* testfunc = "print(add(1,2))";

if(luaL_dostring(L, testfunc))
{
	printf(printf("Filed to invoke.\n"));
}
lua_close(L);

return 0;

}
