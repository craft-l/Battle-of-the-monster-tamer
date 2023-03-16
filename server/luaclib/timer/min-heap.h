#ifndef _MinHeap_h
#define _MinHeap_h

#include <uthash.h>

#include <stdio.h>
#include <stdlib.h>
#include <string.h>

//哨兵值，依据实际使用情况而定
#define MINDATA -1

typedef enum
{
    true=1, false=0
}bool;

//哈希储存
typedef struct hash_node_struct
{
    int id;
    int idx;
    //hash标识，不需要初始化
    UT_hash_handle hh;
}*hash_node;

//堆节点结构
//这里定义数据类型的时候加了一层指针，导致后面操作变成了指针的指针，值得注意
typedef struct heap_node_struct
{
    int id;         //id用于查找、删除、修改等
    int idx;        //索引用于确定节点位置
    int expire;     //运行时刻  runtime = 当前时刻 + 定时时间
}heap_node;

//最小堆结构
typedef struct min_heap_struct
{
    int size;       //当前数组大小
    int capacity;   //设置最大值用来扩容,控制空间大小
    heap_node* node;
} min_heap;


//最小堆
min_heap mh;
//为避免频繁申请释放空间所以声明以下结构用来中转数据
heap_node temp_heap_node;
//使用了find_hash_node之后temp数据不会被清除可以继续使用,注意这是指针所以可以重复使用
hash_node temp_hash_node;
//本最小堆设有哨兵节点0，所以节点索引从1开始，左子节点为2*idx,右子节点为2*idx+1
//哨兵节点
heap_node* min_node;

//初始化
void l_init_heap();
//释放空间
void l_release_minheap();
//删除指定id节点
int delete_node(int id);
//删除堆顶
void pop_root();
//增加节点
bool add_node_c(int id, int expire);
//哨兵节点初始化
void init_min_node();
//堆是否为空
bool is_empty();
//堆是否已满
bool is_full();
//上升操作
int shift_up( int pos);
//下降操作
bool shift_down( int pos);
//交换两节点
void swap_node(int a, int b);
//比较节点之间时间大小
bool less_than( int l, int r);
//删除最后一个节点
void remove_last_node();
//查找特定id节点
int find_node(int id);
//扩容
bool expand_capacity();
//减容
void reduce_capacity();
//分配空间
void malloc_node();
//hash
void add_hash_node(int id, int idx);
int find_hash_node(int id);
void delete_hash_node(int id);
void change_hash_node(int id);



#endif

