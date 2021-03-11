# Coco
Coco is an open source, cross-platform message queuing service. It is very simple and lightweight. It uses .Net native development and does not rely on redundant libraries. Simple to use and easy to configure

![GitHub Workflow Status](https://img.shields.io/github/workflow/status/sbchong/Coco/.NET%20Core)
![GitHub](https://img.shields.io/github/license/sbchong/Coco)
![GitHub all releases](https://img.shields.io/github/downloads/sbchong/Coco/total)


- [x] **Lightweight and simple, the standalone version is less than 50MB in size, the runtime dependent version depends on .Net Core 5, and the size is less than 1MB**
- [x] **Easy to use, support json file (low) and startup parameter (high) configuration**
- [x] Use memory efficient store and forward 
- [x] Upgrade version to .Net 5

## Instructions

Please go to the release page to view the published files and download the corresponding version. The file correspondence is as follows

File name | version
-----|------
Coco|Linux stand-alone version
Coco.exe|Windows standalone version
Coco.7z|Runtime dependent cross-platform universal version

**The stand-alone version can be run directly, the general version must ensure that the system has .Net Core 3.1 Runtime support, and use the `dotnet` command to start**

### Project installation corresponding package

Please install [Coco.Producer](https://github.com/sbchong/Coco.Producer) and [Coco.Comsummer](https://github.com/sbchong/Coco.Comsumer) NuGet programs separately in the project Package, use according to its readme instructions
