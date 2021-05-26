#region << 文 件 说 明 >>
/*----------------------------------------------------------------
// 文件名称：BaseDbContext
// 创 建 者：杨程
// 创建时间：2021/5/25 17:58:10
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
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TaskNote.Core.Interface;
using TaskNote.Core.Models;

namespace TaskNote.Core.DataBaseContext
{
    public class BaseDbContext:DbContext, IDataContext
    {
        public BaseDbContext():base()
        {
            //构造函数
        }

        public void AddEntity<T>(T entity) where T : TopBase
        {
            this.Entry(entity).State = EntityState.Added;
        }

        public void DeleteEntity<T>(T entity) where T : TopBase
        {
            var set = this.Set<T>();
            
            set.Attach(entity);
            set.Remove(entity);
            
        }

        public void UpdateEntity<T>(T entity) where T : TopBase
        {
            this.Entry(entity).State = EntityState.Modified;
        }


        #region 属性

        #endregion

        #region 公共方法

        #endregion

        #region 私有方法

        #endregion

        #region 命令

        #endregion
    }
}
