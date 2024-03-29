using Demo.BLL.Interfaces;
using Demo.DAL.Context;
using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        private readonly MVCAppDbContext _dbContext;

        public EmployeeRepository(MVCAppDbContext dbContext) :base(dbContext) 
        {
            _dbContext = dbContext;
        }
        public IQueryable<Employee> GetEmployeesByAddress(string address)      
        =>   _dbContext.Employees.Where(a=>a.Address == address);

        public IQueryable<Employee> GetEmployeesByName(string SearchValue)
        {
            var result = _dbContext.Employees.Include(e=>e.Department).Where(Employee => Employee.Name.Trim().ToLower().Contains(SearchValue.Trim().ToLower()));
                                                
            return result;
        }
    }
}
