using System;
using System.Threading.Tasks;

namespace CourseManagement.Core.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        Task CommitAsync();
    }
}