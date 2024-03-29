using Demo.BLL.Interfaces;
using Demo.DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class UnitOfWork : IUnitOfWork,IDisposable
    {
        private readonly MVCAppDbContext _dbcontext;

        public IEmployeeRepository EmployeeRepository { get; set; }
        public IDepartmentRepository DepartmentRepository { get; set; }

        public UnitOfWork(MVCAppDbContext dbcontext) // ASK CLR For Object Of DBContext
        {
            EmployeeRepository=new EmployeeRepository(dbcontext);
            DepartmentRepository=new DepartmentRepository(dbcontext);
            _dbcontext = dbcontext;
        }

        public int Complete()
        =>  _dbcontext.SaveChanges();

        public void Dispose()
        {
            _dbcontext.Dispose();
        }
    }
}
