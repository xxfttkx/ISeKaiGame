@echo off

rem 设置循环来改名文件
for /l %%i in (2, 1, 20) do (
    ren Player_%%i\Dead.anim Dead_%%i.anim
    ren Player_%%i\Stand.anim Stand_%%i.anim
    ren Player_%%i\Move.anim Move_%%i.anim
)

echo 文件改名完成！
pause
