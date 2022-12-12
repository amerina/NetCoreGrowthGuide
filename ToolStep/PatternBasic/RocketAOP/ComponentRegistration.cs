using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketAOP
{
    /// <summary>
    /// Four Step:实现IRegistration接口并注册你的组件
    /// 注册拦截器后跟业务组件。指定要与每个业务组件一起使用的拦截器。您可能已经注意到，LoggingInterceptor附加到我们唯一的业务组件Rocket
    /// </summary>
    public class ComponentRegistration : IRegistration
    {
        public void Register(IKernelInternal kernel)
        {
            kernel.Register(
              Component.For<LoggingInterceptor>()
                .ImplementedBy<LoggingInterceptor>());

            //关联拦截器与业务组件
            kernel.Register(
              Component.For<IRocket>()
                  .ImplementedBy<Rocket>()
                  .Interceptors(InterceptorReference.ForType<LoggingInterceptor>()).Anywhere);
        }
    }
}
