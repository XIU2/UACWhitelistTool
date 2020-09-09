# XIU2/UACWhitelistTool

[![Release Version](https://img.shields.io/github/v/release/XIU2/UACWhitelistTool.svg?style=flat-square&label=Release&color=1784ff)](https://github.com/XIU2/UACWhitelistTool/releases/latest)
[![GitHub license](https://img.shields.io/github/license/XIU2/UACWhitelistTool.svg?style=flat-square&label=License&color=36ab60)](https://github.com/XIU2/UACWhitelistTool/blob/master/LICENSE)
[![GitHub Star](https://img.shields.io/github/stars/XIU2/UACWhitelistTool.svg?style=flat-square&label=Star&color=36ab60)](https://github.com/XIU2/UACWhitelistTool/stargazers)
[![GitHub Fork](https://img.shields.io/github/forks/XIU2/UACWhitelistTool.svg?style=flat-square&label=Fork&color=36ab60)](https://github.com/XIU2/UACWhitelistTool/network/members)

**一个可以添加 UAC 白名单的小工具！**  

如果你为了安全而不想完全关闭 UAC，但是又觉得每次运行一些必须 以管理员身份运行 的软件时 UAC 提示很烦，那么你就可以用 **UAC 白名单小工具**把这些软件添加到 UAC 白名单中。下次运行这些软件时，就不会提示 UAC 了，而且软件依然拥有 管理员权限。  

****

## 软件界面

![软件界面](https://raw.githubusercontent.com/XIU2/UACWhitelistTool/master/img/02.png)  
![右键菜单](https://raw.githubusercontent.com/XIU2/UACWhitelistTool/master/img/01.png)

****

## 下载地址

* 蓝奏云 ：[https://www.lanzoux.com/b073m0dne](https://www.lanzoux.com/b073m0dne)
* Github：[https://github.com/XIU2/UACWhitelistTool/releases](https://github.com/XIU2/UACWhitelistTool/releases)

****

## 使用说明

1. **[拖拽]** 或 **[浏览]** 选择一个应用程序 (.exe) 或快捷方式 (.lnk) 。  
2. [程序名称] 随意，但必须唯一 不可重复。  
3. [启动参数] 与 [起始位置] 均可选。  
4. **[添加至 UAC 白名单]** 后，[桌面] 就会出现一个快捷方式，只有通过该快捷方式运行才不提示 UAC！(运行后默认拥有管理员权限)  

> **注意：** 生成于桌面的快捷方式可以复制、移动、重命名，不影响使用！  
> **注意：** 为了方便寻找和删除，添加白名单时 [程序名称] 前会添加 [noUAC.] 标识。  
> **另外，** 因添加任务计划需要管理员权限，所以本软件运行会提示 UAC，但可以把本软件添加到 UAC 白名单中！

****

## 其他说明

### 运行提示 .NET 错误？

本软件最低依赖是 .NET Framework 4.5，报错说明你系统的该依赖版本低于 4.5（Win10 默认满足该依赖），请安装更高版本的 [.NET Framework](https://dotnet.microsoft.com/download/dotnet-framework) ！

****

### 如何把快捷方式固定为磁贴？

如果要把快捷方式添加到磁贴，那需要用到我写的磁贴美化小工具：  

* [西柚秀：「原创」嫌 Windows10 磁贴丑？试试这个！让你的「磁贴」不！再！难！看！](https://zhuanlan.zhihu.com/p/79630122)  

****

### 原理？手动教程？

* [西柚秀：「技巧」添加 UAC 白名单，让软件运行不再提示 UAC！](https://zhuanlan.zhihu.com/p/113767050)  

****

## 更新日志

### 2020年08月07日，版本 v1.8

1. **美化** 软件界面。  
2. **修复** 当程序名称小于6个字符时，添加到UAC白名单报错的问题。  

### 2020年06月30日，版本 v1.7

1. **优化** 界面。  
2. **修复** 一些细节问题。  

### 2020年03月31日，版本 v1.6

1. **新增** 支持配置 启动参数、起始位置。  
—— 一些程序运行时需要添加 启动参数 或者指定 起始位置 就用得上了。  
—— 如果选择的文件是 .lnk 快捷方式，那么软件会自动读取其 启动参数 与 起始位置。  
2. **修复** 程序名称输入框无法使用快捷键的问题（如 Ctrl+V 粘贴）。  
3. **优化** 当程序名称头部已存在 noUAC. 时，将不会重复添加。  

### 2020年03月29日，版本 v1.5

1. 新增 支持添加 .bat 脚本文件到白名单（启动后默认就是管理员权限）。  

**为避免影响大家观看文章，更多的更新日志请看压缩包内的 [使用说明+更新日志.txt]。**  

****

## 许可证

The GPL-3.0 License.

本软件仅供学习交流，请勿用于商用。  

软件所有权归 X.I.U(XIU2) 所有。  

> 该项目只在 [吾爱破解论坛](https://www.52pojie.cn/thread-1134450-1-1.html)、[知乎文章](https://zhuanlan.zhihu.com/p/114096199) 发布过，其他网站均为转载。当然，**欢迎转载！** 