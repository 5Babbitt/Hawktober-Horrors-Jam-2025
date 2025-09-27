@echo off
SET /P commit_message="Enter commit message: "
git pull --rebase origin Wwise
git add .
git commit -m "%commit_message%"
git push
echo Git operations completed.
pause