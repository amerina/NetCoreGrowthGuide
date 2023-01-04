using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;

namespace BookWebAPI
{
    public static class AppBuilderExtensions
    {
        public static IApplicationBuilder RegisterConsul(this IApplicationBuilder app, IHostApplicationLifetime lifetime, ServiceEntity serviceEntity)
        {
            //请求注册的 Consul 地址
            using (var consulClient = new ConsulClient(x => x.Address = new Uri($"http://{serviceEntity.ConsulIP}:{serviceEntity.ConsulPort}")))
            {
                //心跳检测
                var httpCheck = new AgentServiceCheck()
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),//服务启动多久后注册
                    Interval = TimeSpan.FromSeconds(10),//健康检查时间间隔，或者称为心跳间隔
                    HTTP = $"http://{serviceEntity.IP}:{serviceEntity.Port}/api/health",//健康检查地址
                    Timeout = TimeSpan.FromSeconds(5)
                };            
                
                // Register service with consul
                var registration = new AgentServiceRegistration()
                {
                    Checks = new[] { httpCheck },
                    ID = Guid.NewGuid().ToString(),//服务编号,不能重复,用Guid最简单
                    Name = serviceEntity.ServiceName,//服务的名字
                    Address = serviceEntity.IP,//我的ip地址(可以被其他应用访问的地址，本地测试可以用127.0.0.1，机房环境中一定要写自己的内网ip地址)
                    Port = serviceEntity.Port,//我的端口
                    Tags = new[] { $"urlprefix-/{serviceEntity.ServiceName}" }//添加 urlprefix-/servicename 格式的 tag 标签，以便 Fabio 识别
                };

                //注册服务到Consul
                consulClient.Agent.ServiceRegister(registration).Wait();//服务启动时注册，内部实现其实就是使用 Consul API 进行注册（HttpClient发起）

                //程序正常退出的时候从Consul注销服务
                //要通过方法参数注入IApplicationLifetime
                lifetime.ApplicationStopping.Register(() =>
                {
                    consulClient.Agent.ServiceDeregister(registration.ID).Wait();//服务停止时取消注册
                }); 
            }
            return app;
        }
    }

    public class ServiceEntity
    {
        public string IP { get; set; }

        public int Port { get; set; }

        public string ServiceName { get; set; }

        public string ConsulIP { get; set; }

        public int ConsulPort { get; set; }
    }
}
