#include "min-heap.h"

//使用uthash哈希表，储存的结构体和被储存的结构体需要是同一类型的结构体
hash_node hash_head = NULL;

//初始化	init
void l_init_heap()
{
	mh.capacity = 10;
	malloc_node();
	memset(mh.node, 0, (mh.capacity + 1) * sizeof(struct heap_node_struct));
	init_min_node();
	mh.size = 0;
}

//哨兵节点初始化
void init_min_node()
{
	mh.node[0].id = MINDATA;
	mh.node[0].expire = MINDATA;
	mh.node[0].idx = MINDATA;
}

//堆是否为空	
bool is_empty()
{
	return (mh.size <= 0) ? true : false;
}

//堆是否已满	
bool is_full()
{
	return (mh.size >= mh.capacity - 1) ? true : false;
}

//上升操作	
int shift_up(int pos)
{
	int parent;
	while (1)
	{
		parent = pos / 2;
		if (parent == pos || less_than(parent, pos))
		{
			break;
		}
		swap_node(parent, pos);
		pos = parent;
	}
	return parent;
}

//下降操作	
bool shift_down(int pos)
{
	int idx = pos;
	int last = mh.size;
	while (1)
	{
		int left = 2 * idx;
		if ((left > last) || (left < 1))
		{
			break;
		}
		int min = left;//找到更小值
		int right = left + 1;
		if (right < last && !less_than(left, right))
		{
			min = right;
		}
		if (less_than(idx, min))
		{
			break;
		}
		swap_node(idx, min);
		idx = min;
	}
	//是否成功下降
	return idx > pos;
}

//交换两节点的值	
void swap_node(int a, int b)
{
	//交换的时候更新idx
	temp_heap_node = mh.node[a];
	mh.node[a] = mh.node[b];
	mh.node[a].idx = a;
	mh.node[b] = temp_heap_node;
	mh.node[b].idx = b;
	find_hash_node(mh.node[a].id);
	temp_hash_node->idx = a;
	find_hash_node(mh.node[b].id);
	temp_hash_node->idx = b;
}

//比较节点之间时间大小	
bool less_than(int a, int b)
{
	return (mh.node[a].expire < mh.node[b].expire) ? true : false;
}

//删除堆顶	
void pop_root()
{
	if (is_empty())
	{
		return;
	}
	int last = mh.size;
	int idx = 1;
	if (last != idx)
	{
		swap_node(last, idx);
		remove_last_node();
		shift_down(idx);
	}
	else
	{
		remove_last_node();
	}
	reduce_capacity();
}

//查找特定id节点	
int find_node(int id)
{
	temp_hash_node = NULL;
	HASH_FIND_INT(hash_head, &id, temp_hash_node);
	if (temp_hash_node != NULL)
	{
		return temp_hash_node->idx;
	}
	else {
		return -1;
	}
}

//删除指定元素
int delete_node(int id)
{
	if (is_empty())
	{
		return 0;
	}

	int last = mh.size;
	int idx = find_node(id);
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

//释放空间
void remove_last_node()
{
	delete_hash_node(mh.node[mh.size].id);
	memset(mh.node + mh.size, 0, sizeof(heap_node));
	mh.size--;
}

//根据接受的消息生成节点
void add_node_c(int id, int expire)
{
	if (is_full())
	{
		expand_capacity();
	}

	int pos = ++mh.size;
	mh.node[pos].idx = pos;
	mh.node[pos].id = id;
	mh.node[pos].expire = expire;

	add_hash_node(mh.node[pos].id, mh.node[pos].idx);
	shift_up(pos);
}


//两倍扩容
//TODO空间的处理还有点小问题
bool expand_capacity()
{
	mh.capacity *= 2;
	if(malloc_node())
	{
		memset(mh.node + mh.capacity / 2 + 1, 0, (mh.capacity / 2) * sizeof(struct heap_node_struct));
		return true;
	}
	else
	{
		return false;
	}

}

//减容,若当前size小于capacity/3，则容量减半,最小值为50
void reduce_capacity()
{
	if (mh.size < mh.capacity / 3 && mh.capacity > 50)
	{
		mh.capacity /= 2;
	 	malloc_node();
	}
}

//申请空间
bool malloc_node()
{
	//malloc和realloc可能会造成内存泄漏，需要小心使用
	heap_node* temp_ptr = (heap_node*)realloc(mh.node, (mh.capacity + 1) * sizeof(struct heap_node_struct));
	if (temp_ptr != NULL) 
	{
		mh.node = temp_ptr;
		return true;
	}
	else
	{
		return false;
	}
}

//释放最小堆空间
void l_release_minheap()
{
	free(mh.node);
}

//hashMap
//添加	
void add_hash_node(int id, int idx)
{
	//A:hash库的key值不能为负数
	temp_hash_node = NULL;
	HASH_FIND_INT(hash_head, &id, temp_hash_node);
	if (temp_hash_node == NULL)
	{
		temp_hash_node = (hash_node)malloc(sizeof(struct hash_node_struct));
		temp_hash_node->id = id;
		temp_hash_node->idx = idx;
		HASH_ADD_INT(hash_head, id, temp_hash_node);
	}
	else {
		//		printf("insisted\n");
	}
}

//查找	
int find_hash_node(int id)
{
	//不能初始化为null
	temp_hash_node = NULL;
	HASH_FIND_INT(hash_head, &id, temp_hash_node);
	if (temp_hash_node != NULL)
	{
		return 1;
	}
	else {
		return 0;
	}
}

//删除需要先找到其指针	
void delete_hash_node(int id)
{
	if (find_hash_node(id))
	{
		HASH_DEL(hash_head, temp_hash_node);
	}
	else
	{
		//printf("不存在");
	}
}

