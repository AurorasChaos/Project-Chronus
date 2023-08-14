@echo off
title Git Pusher
color 1b

echo Git Pusher by: Auwhora
echo ######################
: execute

echo Please type your commit message here:
set /p cmd=Command:

git add .
git commit -m %cmd%
git push

goto execute
