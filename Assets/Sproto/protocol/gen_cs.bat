echo off
set curdir=%~dp0
cd /d %curdir%/../sprotodump
lua ./sprotodump.lua -cs %curdir%/game.sproto -o %curdir%/gen_cs/gamesproto.cs
echo sproto to cs, done
pause
