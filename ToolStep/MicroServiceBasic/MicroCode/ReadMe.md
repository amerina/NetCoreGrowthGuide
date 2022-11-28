[TOC]

#### 引言

https://kubernetes.io/zh-cn/docs/tutorials/kubernetes-basics/



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

#### 26、Test in Insomnia

#### 27、Add API Gateway

[kubernetes/ingress-nginx: Ingress-NGINX Controller for Kubernetes (github.com)](https://github.com/kubernetes/ingress-nginx)

[Installation Guide - NGINX Ingress Controller (kubernetes.github.io)](https://kubernetes.github.io/ingress-nginx/deploy/#docker-desktop)

```
kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v1.5.1/deploy/static/provider/cloud/deploy.yaml
```

国内网络环境可能获取是有问题的可以新建yaml文件,拷贝内容到本地然后apply

```
kubectl apply -f ingress-nginx-deploy.yaml 
```

```
kubectl get namespace
```

```
kubectl get pods --namespace=ingress-nginx
```

```
kubectl get services --namespace=ingress-nginx  
```

#### 28、Create the Routing File

```
apiVersion: networking.k8s.io/v1
kind: Ingress
metadata:
  name: ingress-src
  annotations:
    kubernetes.io/ingress.class: nginx
    nginx.ingress.kubernetes.io/use-regex: 'true'
spec:
  rules:
  - host: acme.com
    http:
      paths:
        - pathType: Prefix
          path: /api/platforms
          backend:
            service:
              name: platforms-clusterip-src
              port: 
                number: 80
        - pathType: Prefix
          path: /api/c/platforms
          backend:
            service:
              name: commands-clusterip-src
              port: 
                number: 80

```

配置Host文件

```
127.0.0.1 acme.com
```

```
kubectl apply -f ingress-srv.yaml
```

报错：

```
Unable to connect to the server: dial tcp 127.0.0.1:6443: connectex: No connection could be made because the target machine actively refused it.
```

是因为没有启用Kubernetes

如果无法启动重启系统



Routing文件如果修改了不需要重新Apply,Docker会自动更新配置

#### 29、Test in Insomnia

```
http://acme.com:32563/api/Platforms

//记得输入端口..或配置到Host文件
```

#### 30、Setting Sql Server

```
kubectl get storageclass
```

1. **Persistent Volume Claim**
2. Persistent Volume
3. Storage Class

新建**local-pvc.yaml** file

```
apiVersion: v1
kind: PersistentVolumeClaim
metadata:
  name: mssql-claim
spec:
  resources:
    requests:
      storage: 200Mi
  volumeMode: Filesystem
  accessModes:
    - ReadWriteMany

```

```
kubectl apply -f local-pvc.yaml
```

```
kubectl get pvc
```

```
kubectl get storageclass
```

```
kubectl create secret generic mssql --from-literal=SA_PASSWORD="Famous901"

输出：
secret/mssql created
```

新建**mssql-plat-depl.yaml** 文件

```yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: mssql-depl
spec:
  replicas: 1
  selector:
    matchLabels:
      app: mssql
  template:
    metadata:
      labels:
        app: mssql
    spec:
      containers:
        - name: mssql
          image: mcr.microsoft.com/mssql/server:2019-latest
          ports:
            - containerPort: 1433
          env:
          - name: MSSQL_PID
            value: "Express"
          - name: ACCEPT_EULA
            value: "Y"
          - name: SA_PASSWORD
            valueFrom:
              secretKeyRef:
                name: mssql
                key: SA_PASSWORD
          volumeMounts:
          - mountPath: /var/opt/mssql/data
            name: mssqldb
      volumes:
        - name: mssqldb
          persistentVolumeClaim:
            claimName: mssql-claim
---
apiVersion: v1
kind: Service
metadata:
  name: mssql-clusterip-src
spec:
  type: ClusterIP
  selector:
    app: mssql
  ports:
  - name: mssql
    protocol: TCP
    port: 1433
    targetPort: 1433                
---
apiVersion: v1
kind: Service
metadata:
  name: mssql-loadbalancer
spec:
  type: LoadBalancer
  selector:
    app: mssql
  ports:
  - protocol: TCP
    port: 1433
    targetPort: 1433      
```

[Microsoft Artifact Registry](https://mcr.microsoft.com/en-us/product/mssql/rhel/server/about)

```
kubectl apply -f mssql-plat-depl.yaml
```

```
输出：
deployment.apps/mssql-depl created
service/mssql-clusterip-src created
service/mssql-loadbalancer created
```

#### 31、Platform Service With Sql Server

添加调用

```
context.Database.Migrate();
```

生成Migrate

```
dotnet ef migrations add initialmigration
```

```
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlatformService.Migrations
{
    /// <inheritdoc />
    public partial class initialmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Platforms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Publisher = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cost = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Platforms", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Platforms");
        }
    }
}

```

31、重建Docker Image

有了以上修改需要重建Docker Image

```
docker build -t wzyandi/platformservice .
```

```
docker push wzyandi/platformservice
```



```
kubectl get deployments
```

如果映像在运行就，重启K8S platformService映像，如果发生错误需要删掉重建

```
kubectl rollout restart deployment platforms-depl
```

查看重启状态

```
platforms-depl-58994798d9-tlph8   0/1     Error               1 (54s ago)     96s
```

如果报错去Docker Doesktop查看报错信息

```
2022-11-26 19:04:48 Unhandled exception. Microsoft.Data.SqlClient.SqlException (0x80131904): A connection was successfully established with the server, but then an error occurred during the pre-login handshake. (provider: TCP Provider, error: 35 - An internal exception was caught)
2022-11-26 19:04:48  ---> System.Security.Authentication.AuthenticationException: The remote certificate was rejected by the provided RemoteCertificateValidationCallback.
```



解决错误后需要删除Deployment重新创建

```
kubectl get deployments
```

```
kubectl delete deployment platforms-depl
```

重建Docker Image

```
docker build -t wzyandi/platformservice .
```

```
docker push wzyandi/platformservice
```

```
cd ..
cd k8s
kubectl get deployments
kubectl apply -f platforms-depl.yaml
```

输出：

```
deployment.apps/platforms-depl created
service/platforms-clusterip-src unchanged
```

```
kubectl get pods
```

输出：









#### 99：运行项目

运行Platform项目

```
cd PlatformService
dotnet run
```

运行Commands项目

```
cd CommandsService
dotnet run
```



Pod AND Clusterip

```
kubectl apply -f platforms-depl.yaml
```

Node Pod

```
kubectl apply -f platforms-np-srv.yaml
```

Pod

```
kubectl apply -f commands-depl.yaml
```

API Gateway

```
kubectl apply -f ingress-nginx-deploy.yaml 
```

Routing

```
kubectl apply -f ingress-srv.yaml
```

Sql Server

```
kubectl apply -f mssql-plat-depl.yaml
```

```
kubectl get services
```

```
kubectl get pods
```

```
kubectl get deployments 
```

```
kubectl rollout restart deployment commands-depl
kubectl rollout restart deployment platforms-depl
kubectl rollout restart deployment mssql-depl
```

```
kubectl delete deployment platforms-depl

kubectl delete deployment --namespace=ingress-nginx ingress-srv 

```

```
kubectl get pods --namespace=ingress-nginx
```

```
kubectl get services --namespace=ingress-nginx 

kubectl get deployments --namespace=ingress-nginx 

kubectl rollout restart deployment ingress-srv
```

检查各项服务Pods是否都已经启动



[使用 kubectl 管理 Secret | Kubernetes](https://kubernetes.io/zh-cn/docs/tasks/configmap-secret/managing-secret-using-kubectl/)

[Kubectl Delete Deployment: Deleting Kubernetes Deployment (linuxhandbook.com)](https://linuxhandbook.com/kubectl-delete-deployment/)