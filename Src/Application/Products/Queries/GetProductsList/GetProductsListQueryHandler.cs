using System.Collections.Generic;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Northwind.Application.Common.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Northwind.Application.Products.Queries.GetProductsList;

public class GetProductsListQueryHandler : IRequestHandler<GetProductsListQuery, ProductsListVm>
{
    private readonly INorthwindDbContext _context;
    private readonly IMapper _mapper;

    public GetProductsListQueryHandler(INorthwindDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ProductsListVm> Handle(GetProductsListQuery request, CancellationToken cancellationToken)
    {
        List<ProductDto> products = await _context.Products
            .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
            .OrderBy(p => p.ProductName)
            .ToListAsync(cancellationToken);

        ProductsListVm vm = new()
        {
            Products = products,
            CreateEnabled = true // TODO: Set based on user permissions.
        };

        return vm;
    }
}