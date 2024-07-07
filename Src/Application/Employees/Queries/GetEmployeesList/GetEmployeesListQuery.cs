using System.Collections.Generic;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Northwind.Application.Common.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Northwind.Application.Employees.Queries.GetEmployeesList;

public class GetEmployeesListQuery : IRequest<EmployeesListVm>
{
    public class GetEmployeesListQueryHandler : IRequestHandler<GetEmployeesListQuery, EmployeesListVm>
    {
        private readonly INorthwindDbContext _context;
        private readonly IMapper _mapper;

        public GetEmployeesListQueryHandler(INorthwindDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<EmployeesListVm> Handle(GetEmployeesListQuery request, CancellationToken cancellationToken)
        {
            List<EmployeeLookupDto> employees = await _context.Employees
                .ProjectTo<EmployeeLookupDto>(_mapper.ConfigurationProvider)
                .OrderBy(e => e.Name)
                .ToListAsync(cancellationToken);

            EmployeesListVm vm = new EmployeesListVm
            {
                Employees = employees
            };
                 
            return vm;
        }
    }
}