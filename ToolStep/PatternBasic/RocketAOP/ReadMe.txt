https://www.cnblogs.com/chunjin/p/6801335.html

在您的项目中设置Castle DynamicProxy的步骤
1.从NuGet下载并安装“Castle.Windsor”软件包。
2.实现IInterceptor接口。这是DynamicProxy将要使用的接口。
3.实施IRegistration界面并注册您的组件。注册拦截器后跟业务组件。指定要与每个业务组件一起使用的拦截器。
4.创建Windsor容器（IWindsorContainer）的静态实例，使用组件注册信息进行初始化。