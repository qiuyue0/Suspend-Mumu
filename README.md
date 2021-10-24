# Suspend-Mumu

## 介绍

公主连结 pcr 一键挂起星云引擎黄MuMu进程，防止滑刀

附带黄MuMu 999次极速连点配置文件

## 写在前面

使用前，先进入powershell管理员模式，运行

```powershell
Set-ExecutionPolicy RemoteSigned
```

## 使用方法

在管理员下运行 `suspend_mumu.ps1`，通过按下 `F2`来挂起、恢复 `nebula.exe`进程

建议将 `pssuspend.exe`所在文件夹添加到环境变量，否则请将工作目录切换到 `pssuspend.exe`所在文件夹后再运行此程序

## 注意事项

本程序无法运行在 `.NET core`的 `powershell`，请使用系统自带版本
