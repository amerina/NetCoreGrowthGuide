[TOC]

#### 0、Visual Studio Code

```
Ctrl+Space, Ctrl+I Trigger suggestion
Ctrl+N New File

Ctrl+` Show integrated terminal
Ctrl+. Quick Fixes --此快捷键也是中英标点切换快捷键,输入法快捷键关掉即可

Ctril+C 关闭终端
Ctrl+B 关闭左边Tab页
```



#### 1、新建项目添加依赖项

输出当前版本

```powershell
dotnet --version
```

新建WebAPI项目

```powershell
dotnet new webapi -n PlatformService
```

打开PlatformService项目

```powershell
code -r PlatformService
```

添加AutoMapper依赖

```powershell
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection
```

添加EFCore依赖

```powershell
dotnet add package Microsoft.EntityFrameworkCore
```



```powershell
dotnet add package Microsoft.EntityFrameworkCore.Design
```

添加内存数据库支持

```powershell
 dotnet add package Microsoft.EntityFrameworkCore.Inmemory
```

添加SqlServer支持

```powershell
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
```



#### 2、新建Models

新建Model类：Platform

```
Prop+Tab --新建属性
```



#### 3、新建DbContext

新建AppDbContext

```
Ctor+Tab --新建构造函数
```

修改Program类：

```
builder.Services.AddDbContext<AppDbContext>(opt =>
                 opt.UseInMemoryDatabase("InMem"));
```

#### 4、新建仓储Repository

创建接口IPlatformRepo

```

    public interface IPlatformRepo
    {
        bool SaveChanges();

        IEnumerable<Platform> GetAllPlatforms();

        Platform GetPlatformById(int id);

        void CreatePlatform(Platform platform)
    }
```

实现仓储接口PlatformRepo

添加仓储到依赖注入系统：

```
builder.Services.AddScoped<IPlatformRepo,PlatformRepo>();
```

#### 5、Build项目

```
 dotnet build
```

#### 6、添加SeedData类

```
public static class PrepDb
```

```
//初始化数据
PrepDb.PrepPopulation(app);
```

Build项目

```
dotnet build
```

运行项目

```
dotnet run
```

#### 7、添加DTO

```
public class PlatformCreateDto
public class PlatformReadDto
```

#### 8、添加Profiles-AutoMapper

```
public class PlatformsProfile : Profile
```

#### 9、添加Controller

```
public class PlatformsController:ControllerBase
```

#### 10、Insomnia查看Controller

```
如果提示错误：
SSL peer certificate or SSH remote key was not OK
注释：app.UseHttpsRedirection();
```

#### 11、添加Doker

![02](..\Image\02.png)

![02](..\Image\03.png)



![02](..\Image\04.png)

#### 12、添加Docker file

[.NET samples | Docker Documentation](https://docs.docker.com/samples/dotnet/)

1. 创建一个Dockerfile文件

   ```dockerfile
   # Get base SDK Image from Microsoft
   FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
   WORKDIR /app
   
   # Copy the CSPROJ file and restore any dependecies(via NUGET)
   COPY *.csproj ./
   # Restore Package
   RUN dotnet restore
   
   # Copy the project files and build our release
   COPY . ./
   RUN dotnet publish -c Release -o out
   
   # Generate runtime image
   FROM mcr.microsoft.com/dotnet/aspnet:6.0
   WORKDIR /app
   COPY --from=build-env /app/out .
   ENTRYPOINT ["dotnet","PlatformService.dll"]
   ```

2. Build Image构建映像

   ```powershell
   docker --version
   ```

   ```powershell
   --构建映像 Tag Name=platformservice
   docker build -t wzyandi/platformservice .
   ```

   

3. Run Image AS  A Container

   ```powershell
   docker run -p 8080:80 -d wzyandi/platformservice
   ```

   ```powershell
   --list running containers
   docker ps 
   ```

   ```
   CONTAINER ID 
   a39b91c9ae9a
   ```

   ```
   docker stop a39b91c9ae9a
   ```

   ```
   cls  --清空命令行
   ```

   ```
   docker start a39b91c9ae9a
   ```

   ```
   docker push wzyandi/platformservice
   ```

   [docker desktop for windows 启用kubernetes](https://blog.csdn.net/Berzingou/article/details/106607782)

   

4. Docker启用Kubernetes一直不能启动

   根据阿里解决方案一般肯定能够解决，给Docker一点时间启动

   [AliyunContainerService/k8s-for-docker-desktop](https://github.com/AliyunContainerService/k8s-for-docker-desktop)

   

#### 13、添加Kubernetes

![02](..\Image\05.png)

```
--准备创建解决方案
cd ..
cd ..
Code -r MicroCode
```

1、新建K8S文件夹->新建platforms-depl.yaml文件

创建一个Pod

![02](..\Image\06.png)

```c#
#specifying api version
apiVersion: apps/v1
#what kind is it,we are deploying something into kubernetes
kind: Deployment
metadata:
  name: platforms-depl
spec:
  replicas: 1 #节点至少一个
  selector: #selecting the template that we are creating
    matchLabels:
      app: platformservice
  template: #defining the pod and the container that we're going to use
    metadata:
      labels:
        app: platformservice
    spec: #Specify the containers that we want to run
      containers:
        - name: platformservice
          image: wzyandi/platformservice:latest  
```



```
 kubectl version
```

```
输出：
Client Version: version.Info{Major:"1", Minor:"25", GitVersion:"v1.25.2", GitCommit:"5835544ca568b757a8ecae5c153f317e5736700e", GitTreeState:"clean", BuildDate:"2022-09-21T14:33:49Z", GoVersion:"go1.19.1", Compiler:"gc", Platform:"windows/amd64"}
Kustomize Version: v4.5.7
Server Version: version.Info{Major:"1", Minor:"25", GitVersion:"v1.25.2", GitCommit:"5835544ca568b757a8ecae5c153f317e5736700e", GitTreeState:"clean", BuildDate:"2022-09-21T14:27:13Z", GoVersion:"go1.19.1", Compiler:"gc", Platform:"linux/amd64"}
```

```
cd K8S
```

```
kubectl apply -f platforms-depl.yaml
```

```
输出：
deployment.apps/platforms-depl created
```

```
kubectl get deployments
```

```
输出：
NAME             READY   UP-TO-DATE   AVAILABLE   AGE
platforms-depl   1/1     1            1           80s
```

```
kubectl get pods
```

```
输出：
NAME                              READY   STATUS    RESTARTS   AGE
platforms-depl-769c6cffd5-t7pcg   1/1     Running   0          2m26s
```

```
kubectl get deployments
```

```
这次输出：
NAME             READY   UP-TO-DATE   AVAILABLE   AGE
platforms-depl   1/1     1            1           3m5s
```

```
kubectl delete deployment platforms-depl
```

```
输出：
deployment.apps "platforms-depl" deleted
```

2、创建一个Node Port

![02](..\Image\07.png)

新建platforms-np-srv.yaml文件

```
apiVersion: v1
kind: Service
metadata:
  name: platformnpservice-src
spec:
  type: NodePort
  selector:
    app: platformservice
  ports:
    - name: platformservice
      protocol: TCP
      port: 80
      targetPort: 80
```

```
kubectl apply -f platforms-np-srv.yaml
```

```
输出:
service/platformnpservice-src created
```

```
kubectl get services
```

#### 14、创建CommandsService

```
dotnet new webapi -n CommandsService
```

切换工作目录

```
cd C*
```

#### 15、添加依赖项

添加AutoMapper依赖

```powershell
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection
```

添加EFCore依赖

```powershell
dotnet add package Microsoft.EntityFrameworkCore
```



```powershell
dotnet add package Microsoft.EntityFrameworkCore.Design
```

添加内存数据库支持

```powershell
 dotnet add package Microsoft.EntityFrameworkCore.Inmemory
```

添加SqlServer支持

```powershell
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
```



```
dotnet run
```

#### 16、新建Controller

```
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [Route("api/c/[controller]")]
    [ApiController]
    public class PlatformsController:ControllerBase
    {
        public PlatformsController()
        {
            
        }

        [HttpPost]
        public ActionResult TestInboundConnection()
        {
            Console.WriteLine("--> Inbound POST # Command Service");

            return Ok("Inbound test of from Platforms Controller");
        }
    }
}
```



#### 17、Insomnia查看Controller

如果报错：**Error: SSL peer certificate or SSH remote key was not OK**

[Insomnia : Error: SSL peer certificate or SSH remote key was not OK - Stack Overflow](https://stackoverflow.com/questions/72417847/insomnia-error-ssl-peer-certificate-or-ssh-remote-key-was-not-ok)

或注释：

```
app.UseHttpsRedirection();
```



#### 18、Messaging

Synchronous & Asynchronous Messaging

1. **Synchronous Messaging**

   - Request/Response Cycle
   - Requester will "wait" for response
   - Externally facing services usually synchronous(e.g. http requests)
   - Services usually need to "know" about each other
   - We are using 2 forms:
     - Http
     - Grpc
   - **34**

   **C#代码的异步Async**

   - From a messaging perspective this method is still synchronous
   - The client still has to wait for a response
   - Async in this context means that the action will not wait for a long running operation
   - It will hand back it's thread pool,where it can be reused
   - When the operation finishes it will re-acquire a thread and complete,(and respond back to the requestor)
   - So Async here is about thread exhaustion-the requestor still has to wait(the call is synchronous)

   **服务间的同步消息**

   - 服务间互相调用
   - 可能导致长调用链

   ![00](..\Image\08.png)

   一个解决方案是服务间使用**Grpc**调用

2. **Asynchronous Messaging**

   - No Request/Response Cycle
   - Requester does not wait
   - Event model,e.g. publish-subscribe
   - Typically used between services
   - Event bus is often used(here is RabbitMQ)
   - Service don't need to know about each other,just the bus
   - Introduces its own range of complexities-not a magic bullet

   ![00](..\Image\09.png)



#### 19、PlatformService新建SyncDataServices

SyncDataServices文件夹下新建HttpCommandDataClient

```
public async Task SendPlatformToCommand(PlatformReadDto platform)
        {
            var httpContent = new StringContent(JsonSerializer.Serialize(platform),Encoding.UTF8,"application/json");

            var response = await _httpClient.PostAsync($"{_configuration["CommandService"]}",httpContent);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> Sync POST to CommandService was OK!");
            }
            else
            {
                Console.WriteLine("--> Sync POST to CommandService was NOT OK!");
            }
        }
```

#### 20、修改PlatformsController

新建Platform时调用CommandsService

```
  try
            {
                await _commandDataClient.SendPlatformToCommand(platformReadDto);
            }
            catch (System.Exception ex)
            {
               Console.WriteLine($"--> Could not send synchronously:{ex.Message}");
            }
```

#### 21、添加Docker file

1. 创建一个Dockerfile文件

   ```dockerfile
   # Get base SDK Image from Microsoft
   FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
   WORKDIR /app
   
   # Copy the CSPROJ file and restore any dependecies(via NUGET)
   COPY *.csproj ./
   # Restore Package
   RUN dotnet restore
   
   # Copy the project files and build our release
   COPY . ./
   RUN dotnet publish -c Release -o out
   
   # Generate runtime image
   FROM mcr.microsoft.com/dotnet/aspnet:6.0
   WORKDIR /app
   COPY --from=build-env /app/out .
   ENTRYPOINT ["dotnet","CommandsService.dll"]
   ```

2. Build Image构建映像

   ```powershell
   docker --version
   ```

   ```powershell
   --构建映像 Tag Name=platformservice
   docker build -t wzyandi/commandservice .
   ```

   ```
   docker push wzyandi/commandservice
   ```

   

3. Run Image AS  A Container

   ```powershell
   docker run -p 8080:80 -d wzyandi/commandservice
   ```

   

#### 22、Add Cluster IP Service

**platforms-depl.yaml**

```
---
apiVersion: v1
kind: Service
metadata:
  name: platforms-clusterip-src
spec:
  type: ClusterIP
  selector:
    app: platformservice
  ports:
  - name: platformservice
    protocol: TCP
    port: 80
    targetPort: 80  
```

#### 23、Create a new deployment for CommandsService

新建commands-depl.yaml

```
#specifying api version
apiVersion: apps/v1
#what kind is it,we are deploying something into kubernetes
kind: Deployment
metadata:
  name: commands-depl
spec:
  replicas: 1 #Kubernetes保证总是有1个节点在运行
  selector: #selecting the template that we are creating
    matchLabels:
      app: commandservice
  template: #defining the pod and the container that we're going to use
    metadata:
      labels:
        app: commandservice
    spec: #Specify the containers that we want to run
      containers:
        - name: commandservice
          image: wzyandi/commandservice:latest
---
apiVersion: v1
kind: Service
metadata:
  name: commands-clusterip-src
spec:
  type: ClusterIP
  selector:
    app: commandservice
  ports:
  - name: commandservice
    protocol: TCP
    port: 80
    targetPort: 80                 
```

#### 24、add appeettings.Production.json

```
{
    "CommandService":"http://commands-clusterip-src:80/api/c/Platforms"
}
```

修改配置文件后需要重建Docker映像

```
docker build -t wzyandi/commandservice/platformservice .
```

```
docker push wzyandi/platformservice
```

**K8S默认使用Production环境所以会自动使用Production配置文件**

#### 25、Deployment K8S

```
kubectl get deployments
```

```
kubectl get pods 
```

```
kubectl get services  
```

```
kubectl apply -f platforms-depl.yaml
```

```
输出：
deployment.apps/platforms-depl unchanged
service/platforms-clusterip-src created
```

上面我们重构了Platform Image需要K8S获取新的Image重启

```
kubectl get deployments
```

```
输出：
NAME             READY   UP-TO-DATE   AVAILABLE   AGE     
platforms-depl   1/1     1            1           4d21h
```



```
kubectl rollout restart deployment platforms-depl
```



```
kubectl apply -f commands-depl.yaml
```

```
deployment.apps/commands-depl created
service/commands-clusterip-src created
```

```
kubectl get services
```































