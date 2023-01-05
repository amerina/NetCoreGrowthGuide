using System.Collections.Generic;
using MediatR;
using ProductService.Api.Queries.Dtos;

namespace ProductService.Api.Queries
{
    /// <summary>
    /// 使用MediatR创建查询消息
    /// 实现IRequest接口，并指定查询类期望的响应类型(即IEnumerable<ProductDto>)
    /// </summary>
    public class FindAllProductsQuery : IRequest<IEnumerable<ProductDto>>
    {

    }
}
