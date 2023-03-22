echo off
set curdir=%~dp0
cd /d %curdir%/../sprotodump
lua ./sprotodump.lua -cs %curdir%/TroopProto.sproto -o %curdir%/gen_cs/TroopProto.cs
echo sproto to cs, done
pause
