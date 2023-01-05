using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ProductService.Api.Queries;
using ProductService.Api.Queries.Dtos;
using ProductService.Domain;

namespace ProductService.Queries
{
    /// <summary>
    /// FindAllProductsHandler实现了IRequestHandler接口,并定义了输入和输出类型
    /// 
    /// FindAllProductsHandler响应FindAllProductsQuery并返回IEnumerable<ProductDto>
    /// </summary>
    public class FindAllProductsHandler : IRequestHandler<FindAllProductsQuery, IEnumerable<ProductDto>>
    {
        private readonly IProductRepository productRepository;

        public FindAllProductsHandler(IProductRepository productRepository)
        {
            this.productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));            
        }

        public async Task<IEnumerable<ProductDto>> Handle(FindAllProductsQuery request, CancellationToken cancellationToken)
        {
            var result = await productRepository.FindAllActive();

            return result.Select(p => new ProductDto
            {
                Code = p.Code,
                Name = p.Name,
                Description = p.Description,
                Image = p.Image,
                MaxNumberOfInsured = p.MaxNumberOfInsured,
                Icon = p.ProductIcon,
                Questions = p.Questions != null ? ProductMapper.ToQuestionDtoList(p.Questions) : null,
                Covers = p.Covers.Any() ? ProductMapper.ToCoverDtoList(p.Covers) : null
            }).ToList();
        }      
        
    }
}
