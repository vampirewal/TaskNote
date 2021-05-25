using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskNote.Core.Models;

namespace TaskNote.Core.Interface
{
    public interface IDataContext : IDisposable
    {
        void AddEntity<T>(T entity) where T : TopBase;

        void DeleteEntity<T>(T entity) where T : TopBase;
        
        //
        // 摘要:
        //     Set
        //
        // 类型参数:
        //   T:
        DbSet<T> Set<T>() where T : class;
        
        //
        // 摘要:
        //     UpdateEntity
        void UpdateEntity<T>(T entity) where T : TopBase;
        
    }
}
