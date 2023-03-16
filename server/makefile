TARGET := minheaptimer 

CC := gcc 

CFLAG := -fPIC -shared -Wall 

VPATH=luaclib/sort 
VPATH=luaclib/timer 

INCLUDES := -I skynet/3rd/lua -I 3rd/uthash -I luaclib/timer 

OUTPUT_DIR = luaclib/timer/ 

SRCS := luaclib/timer/lua-timer.o luaclib/timer/min-heap.o 

minheaptimer.so:min-heap.o lua-timer.o
	$(CC) $(CFLAG) $(SRCS) -o $(OUTPUT_DIR)$@ $(INCLUDES)

lua-timer.o:lua-timer.c
	$(CC) $(CFLAG) $^ -o $(OUTPUT_DIR)$@ 

min-heap.o:min-heap.c
	$(CC) $(CFLAG) $^ -o $(OUTPUT_DIR)$@ 

.PHONY: clean
clean: 
	rm *.so
