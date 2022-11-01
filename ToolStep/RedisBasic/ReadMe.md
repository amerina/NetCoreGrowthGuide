

[TOC]

## 1、Redis Basic

### 1、**Redis数据类型**

字符串String、字典Hash、列表List、集合Set、有序集合SortedSet

![01](Image\01.png)





### 2、Redis特性

- 速度快

  10w OPS，每秒读取十万次

  快的原因：数据存在内存中、单线程模型

- 持久化

  Redis所有数据保持在内存中，对数据的更新将异步地保存到磁盘上。

  - RDB(Redis DataBase)：简而言之，就是在不同的时间点，将 redis 存储的数据生成快照并存储到磁盘等介质上
  - AOF(Append Only File)：则是换了一个角度来实现持久化，那就是将 redis 执行过的所有写指令记录下来，在下次 redis 重新启动时，只要把这些写指令从前到后再重复执行一遍，就可以实现数据恢复

  RDB 和 AOF 两种方式可以同时使用，在这种情况下，如果 redis 重启的话，则会优先采用 AOF 方式来进行数据恢复，这是因为 AOF 方式的数据恢复完整度更高。

  如果你没有数据持久化的需求，也完全可以关闭 RDB 和 AOF 方式，这样的话，Redis 将变成一个纯内存数据库，就像 memcache 一样。

- 多种数据结构

- 支持多种编辑语言

- 功能丰富

  - 发布订阅
  - Lua脚本
  - 事务
  - Pipeline

- 使用简单

  - 不依赖外部库
  - 单线程模型，服务端或客户端开发简单

- 主从复制

  高可用分布式的基础

- 高可用、分布式

  - 高可用：Redis-Sentinel(v2.8)支持高可用
  - 分布式：Redis-Cluster(v3.0)支持分布式

### 3、Redis典型应用场景

- 缓存系统

- 计数器

  单线程Increase命令

- 消息队列系统

- 排行榜

- 实时系统

### 4、Redis安装

- Redis安装

  - Linux安装

    ```powershell
    wget http://download.redis.io/releases/redis-3.0.7.tar.gz
    tar -xzf redis-3.0.7.tar.gz
    ln -s redis-3.0.7 redis
    cd redis
    make&&make install
    --如果权限不够sudo -s
    ```

  - Windows安装

- 可执行文件说明

  - redis-server：Redis服务器
  - redis-cli：Redis命令行客户端
  - redis-benchmark：Redis性能测试工具
  - redis-check-aof：AOF文件修复工具
  - redis-check-dump：RDB文件检查工具
  - redis-sentinel：Sentinel服务器(2.8以后版本)

- 三种启动方法

  - 最简启动

    ```powershell
    redis-server
    
    ps -ef | grep redis-server
    ps -ef | grep redis-server | grep -v grep
    ```

  - 动态参数启动

    ```powershell
    --指定端口
    redis-server --port 6380
    ```

  - 配置文件启动

    ```powershell
    redis-server configPath
    ```

- 简单的客户端连接

  ```powershell
  redis-cli -h 127.0.0.1 -p 6379
  ping
  ```

- Redis客户端返回值

  - 状态回复：ping->PONG
  - 错误回复：hget hello field->返回错误消息
  - 整数回复：incr hello
  - 字符串回复：get hello
  - 多行字符串回复：mget hello foo

- Redis常用配置

  - daemonize：是否是守护进程
  - port：Redis对外端口号(默认6379)
  - logfile：Redis系统日志
  - dir：Redis工作目录

### 5、Redis API的使用

- 通用命令
- 字符串类型
- 哈希类型
- 列表类型
- 集合类型
- 有序集合类型















参考：

1. [在 windows 上安装 Redis](https://www.redis.com.cn/redis-installation.html)
2. [Redis文档中心 -- Redis中国用户组（CRUG）](http://www.redis.cn/documentation.html)



