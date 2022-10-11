# MongoWebAPI
### 1、安装并配置MongoDB

1、[下载MongoDB社区版](https://www.mongodb.com/try/download/community?tck=docs_server)

2、安装后配置环境变量

```
C:\Program Files\MongoDB\Server\6.0\bin
```

3、安装MongoDBShell(**版本与文档不同需要安装MongoShell**)

[MongoDB Shell Download | MongoDB](https://www.mongodb.com/try/download/shell)

下载后拷贝

![mongoshell](MarkDownImg\mongoshell.png)

这两个文件到MongoDB的bin文件中



4、选择开发计算机上用于存储数据的目录。例如，在 Windows 上为 C:\MongoData，创建目录。 mongo Shell 不会创建新目录。

5、打开命令行界面。 运行以下命令以连接到默认端口 27017 上的 MongoDB。 请记得将 `<data_directory_path>` 替换为上一步中选择的目录。

```shell
mongod --dbpath C:\MongoData
```

6、打开另一个命令行界面实例。 通过运行以下命令来连接到默认测试数据库：

```
mongosh
```

7、在命令行界面中运行以下命令：

```
use BookstoreDb
```

如果它不存在，则将创建名为“BookstoreDb”的数据库。 如果该数据库存在，则将为事务打开其连接。

8、使用以下命令创建 `Books` 集合

```
db.createCollection('Books')
```

显示以下结果：

```
{ "ok" : 1 }
```

9、使用以下命令定义 `Books` 集合的架构并插入两个文档：

```
db.Books.insertMany([{'Name':'Design Patterns','Price':54.93,'Category':'Computers','Author':'Ralph Johnson'}, {'Name':'Clean Code','Price':43.15,'Category':'Computers','Author':'Robert C. Martin'}])
```

显示以下结果：

```
{
  "acknowledged" : true,
  "insertedIds" : [
    ObjectId("5bfd996f7b8e48dc15ff215d"),
    ObjectId("5bfd996f7b8e48dc15ff215e")
  ]
}
```

10、使用以下命令查看数据库中的文档：

```
db.Books.find({}).pretty()
```

显示以下结果：

```
{
  "_id" : ObjectId("5bfd996f7b8e48dc15ff215d"),
  "Name" : "Design Patterns",
  "Price" : 54.93,
  "Category" : "Computers",
  "Author" : "Ralph Johnson"
}
{
  "_id" : ObjectId("5bfd996f7b8e48dc15ff215e"),
  "Name" : "Clean Code",
  "Price" : 43.15,
  "Category" : "Computers",
  "Author" : "Robert C. Martin"
}
```

数据库可供使用了。

11、运行MongoWebAPI

导航到https://localhost:44394/swagger/index.html

![swaggerAPI](MarkDownImg\swaggerAPI.png)



12、添加身份认证支持







参考：

1、[Create a web API with ASP.NET Core and MongoDB | Microsoft Learn](https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-mongo-app?view=aspnetcore-5.0&tabs=visual-studio)



2、[Install MongoDB Community Edition on Windows — MongoDB Manual](https://www.mongodb.com/docs/manual/tutorial/install-mongodb-on-windows/)

[How to Install MongoDB on Windows 10 - Step by Step - WDB24](https://www.wdb24.com/how-to-install-mongodb-windows-10/)