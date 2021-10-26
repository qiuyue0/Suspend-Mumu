# Suspend-Mumu

## 介绍

公主连结 pcr 一键挂起星云引擎黄MuMu进程，防止滑刀

附带黄MuMu 999次极速连点配置文件

## 使用方法

开启程序后使用F2暂停/恢复黄MuMu进程

如果挂起进程后点击F2不能正常恢复，可以点击程序里的关闭，再开启后就可以正常恢复了。也可以自行去资源监视器恢复。

## 注意事项

只有星云引擎的黄MuMu才可以使用该程序，标准引擎或者其他模拟器不适用，可以自行修改 `SuspendMuMu/KeyboardHook` 151行进行适配。修改147行vkCode的判断逻辑改变热键。

## 特别感谢

[SarathR/ProcessUtil](https://github.com/SarathR/ProcessUtil) 提供挂起进程模块

BOWKeyboardHook 提供了全局监听模块，虽然不知道出处是哪
