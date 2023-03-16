#include <stdarg.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <math.h>


//插入排序
void insert_sort(int a[],int n)
{
    for(int i=1; i<n; i++)
    {
        int j=0;
        while( (a[j]<a[i]) && (j<i))
        {
            j++;
        }
        if(i != j)
        {
            int temp = a[i];
            for(int k = i; k > j; k--)
            {
                a[k] = a[k-1];
            }
            a[j] = temp;
        }
  }
}

//冒泡排序
void bubble_sort(int a[],int n)
{
    int temp = 0;
    for(int i=0; i<n-1 ; ++i)
    {
        for(int j=0; j<n-1-i;++j)
        {
            if(a[j] < a[j+1])
            {
                temp = a[j];
                a[j] = a[j+1];
                a[j+1] = temp;
            }
        }
    }
}

void count_sort(int arr[], int sorted_arr[], int n)
{
    //100以内排序
	int *count_arr = (int *)malloc(sizeof(int) * 100);
	int i; 
	//初始化计数数组 
	for(i = 0; i<100; i++)
		count_arr[i] = 0;
	//统计i的次数 
	for(i = 0;i<n;i++)
		count_arr[arr[i]]++;
	//对所有的计数累加 
	for(i = 1; i<100; i++)
		count_arr[i] += count_arr[i-1]; 
	//逆向遍历源数组（保证稳定性），根据计数数组中对应的值填充到先的数组中 
	for(i = n; i>0; i--)
	{
		sorted_arr[count_arr[arr[i-1]]-1] = arr[i-1];
		count_arr[arr[i-1]]--;	
	} 
	free(count_arr);
}