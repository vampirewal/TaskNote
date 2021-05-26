#region << 文 件 说 明 >>
/*----------------------------------------------------------------
// 文件名称：TaskNoteDataAccess
// 创 建 者：杨程
// 创建时间：2021/5/25 16:47:10
// 文件版本：V1.0.0
// ===============================================================
// 功能描述：
//		
//
//----------------------------------------------------------------*/
#endregion

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskNote.Core.DataBaseContext;
using TaskNote.Model;

namespace TaskNote.DataAccess
{
    public class TaskNoteDataAccess : DbContext
    {
        public TaskNoteDataAccess()
        {
            //构造函数
        }

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite("Data Source=TaskNoteDataBase.db");
        }
    }
}
