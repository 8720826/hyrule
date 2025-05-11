# Hyrule 开源博客系统

## 概述

&#8195;&#8195; Hyrule 是一个基于现代化技术栈构建的开源博客系统，采用简洁高效的架构设计，支持高度定制化开发。系统遵循CQRS模式，内置多数据库支持，并提供灵活的主题模板系统。

## 技术栈

- 开发语言: C# (.NET 9.0)
- 核心框架: ASP.NET Core Minimal API
- 架构模式: CQRS with MediatR
- 模板引擎: Liquid
- 数据库支持:MySQL / SQL Server /PostgreSQL
- ORM: Entity Framework Core
- 前端: 原生js + alpine

## 功能特性

- 多用户博客管理
- 文章/分类/标签系统
- 评论与审核功能
- SEO友好
- 自定义URL
- Markdown支持
- 实时主题热重载
- RESTful API
- 支持多种数据库
- 支持多种文件存储方式

## 快速开始

#### 环境要求
- .NET SDK 9.0+
- 支持的数据库任选其一

#### 部署方式

推荐在linux系统通过docker方式一键部署

~~~
docker run -e TZ=Asia/Shanghai -d --restart=always -v /data/hyrule:/app/files --name hyrule -p 8080:8080 registry.cn-guangzhou.aliyuncs.com/yescent/hyrule:latest
订单

~~~

#### 安装运行

1. 部署后访问 http(s)://{domain}:{port}
2. 填写数据库连接信息安装
3. 默认账号密码 admin / admin
4. 后台访问地址 http(s)://{domain}:{port}/admin




#### 演示站点

- [https://hyrule.yescent.com](https://hyrule.yescent.com)
- 账号密码 test  /  test


## 许可证

- MIT License

 您可以自由使用、修改和分发本项目的代码，但请保留原作者的版权声明和许可证信息。

 ---

欢迎通过任何方式参与本项目！

如有疑问请通过 issues 或 discussions 与我们交流。