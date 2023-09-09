


#https://codewithmukesh.com/blog/in-memory-caching-in-aspnet-core/
#https://codewithmukesh.com/blog/redis-caching-in-aspnet-core/



Points To Remember 
Your application should never depend on the Cached data as it is highly probable to be unavailable at any given time. 
Traditionally it should depend on your actual data source. Caching is just an enhancement that is to be used only if it is available/valid.
你的应用程序永远不应该依赖于缓存的数据，因为在任何给定的时间它都可能是无效的.


Try to restrict the growth of the cache in memory. This is crucial as Caching may take up your server resources if not configured properly. 
You can make use of the Size property to limit the cache used for entries. 
尝试限制内存中缓存的增长。这是至关重要的，因为如果配置不当，缓存可能会占用您的服务器资源。可以使用 Size 属性限制用于条目的缓存



Use Absolute Expiration / Sliding Expiration to make your application much faster and smarter.
It also helps restricts cache memory usage. 
使用绝对过期/滑动过期使您的应用程序更快更智能。它还有助于限制缓存内存的使用



Try to avoid external inputs as cache keys. Always set your keys in code. 
尽量避免外部输入作为缓存键。始终在代码中设置键


Background Cache Update 后台缓存更新
Additionally, as an improvement, you can use Background Jobs to update cache at a regular interval. 
If the Absolute Cache Expiration is set to 5 minutes, you can run a recurring job every 6 minutes to update the cache entry to it’s latest available version. 
You can use Hangfire to achieve the same in ASP.NET Core Applications.
作为一种改进，您可以使用后台作业定期更新缓存。如果将绝对缓存过期设置为5分钟，则可以每6分钟运行一次循环作业，
将缓存条目更新为最新可用版本。您可以在 ASP.NET Core应用程序中使用 Hangfire 来实现同样的功能。