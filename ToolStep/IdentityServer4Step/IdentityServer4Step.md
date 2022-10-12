## 1、IdentityServer4是什么？

IdentityServer4 是用于 ASP.NET Core 的 OpenID Connect 和 OAuth 2.0 框架。
它在您的应用程序中启用以下功能：

**身份验证即服务**

所有应用程序（Web、原生、移动、服务）的集中登录逻辑和工作流。

IdentityServer 是 OpenID Connect 的官方 [认证](https://openid.net/certification/) 实现。

**单点登录/注销**

通过多种应用程序类型进行单点登录（和注销）。

**API访问控制**

为各种类型的客户端发布 API 的访问令牌，例如 服务器到服务器、Web 应用程序、SPA 和原生/移动应用程序。

**联合网关**

支持外部身份提供商，如 Azure Active Directory、Google、Facebook 等。



## 2、OpenID Connect(协议) 

当应用程序需要知道当前用户的身份时，就需要进行身份验证。 通常，这些应用程序代表该用户管理数据，并且需要确保该用户只能访问他被允许访问的数据。 最常见的例子是（典型的）Web 应用程序。

最常见的身份验证协议是 SAML2p、WS-Federation 和 OpenID Connect —— SAML2p 是最流行和最广泛部署的。

OpenID Connect 是三者中最新的协议，但被认为是未来，因为它在现代应用程序中最有潜力。它从一开始就是为移动应用场景而构建的，旨在对 API 友好。



## 3、OAuth 2.0(协议) 

数据所有者告诉系统,同意授权第三方应用进入系统,获取这些数据。系统从而产生一个短期的进入令牌，用来代替密码，供第三方应用使用。

OAuth 2.0协议规范了下授权的流程,五种模式:

- 客户端凭证(client credentials)
- 密码式(password)
- 隐藏式(implicit)
- 授权码(authorization-code)
- 混合式(Hybrid)



## 4、Step By Step Sample







参考：

1、[欢迎使用 IdentityServer4](https://identityserver4docs.readthedocs.io/zh_CN/latest/index.html)

2、[理解OAuth 2.0](https://www.ruanyifeng.com/blog/2014/05/oauth_2_0.html)

