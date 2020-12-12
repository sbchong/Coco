# Coco

中文 | [English](https://github.com/sbchong/Coco/blob/master/README.EN.md)
 
Coco 是一个开源，跨平台的消息队列服务，他非常简单并且轻量，使用.Net原生开发，不依赖多余库。使用简单，配置方便

![GitHub Workflow Status](https://img.shields.io/github/workflow/status/sbchong/Coco/.NET%20Core)
![GitHub](https://img.shields.io/github/license/sbchong/Coco)
![GitHub all releases](https://img.shields.io/github/downloads/sbchong/Coco/total)

- [x] **轻量简单，独立版体积不到50MB，运行时依赖版依赖于 .Net Core 5 ，体积小于1M**
- [x] **使用方便，支持json文件（低）和启动参数（高）配置**
- [ ] 默认使用内存高效存储转发，支持数据库（计划使用 Entity Framewrok Core）和文件进行持久化处理
- [x] 升级版本到 .Net 5

## 使用方法

### 下载coco文件

请前往发布页查看已发布文件，下载对应的版本，文件对应关系如下

文件名| 版本 
-----|------
Coco|Linux独立运行版
Coco.exe|Windows独立运行版
Coco.7z|运行时依赖跨平台通用版
 
**独立版直接运行即可，通用版必须保证系统有 .Net 5 Runtime 支持，使用 `dotnet` 命令启动**

### 项目安装对应包

请在项目里分别安装[Coco.Producer](https://github.com/sbchong/Coco.Producer)和[Coco.Comsummer](https://github.com/sbchong/Coco.Comsumer)的NuGet程序包，根据其readme指导使用
