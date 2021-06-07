#region << 文 件 说 明 >>
/*----------------------------------------------------------------
// 文件名称：SqlHelper
// 创 建 者：杨程
// 创建时间：2021/6/7 10:13:17
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
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using TaskNote.Core.DataBaseContext;
using TaskNote.Core.Models;

namespace TaskNote.DataAccess
{
    public class SqlHelper
    {
        /// <summary>
        /// 查询单个
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="express"></param>
        /// <returns></returns>
        public static T GetOne<T>(Expression<Func<T, bool>> express) where T : TopBase
        {
            using (TaskNoteDataAccess task = new TaskNoteDataAccess())
            {
                return task.Set<T>().SingleOrDefault(express);
            }
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="express"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetInfoLst<T>(Expression<Func<T, bool>> express) where T : TopBase 
        { 
            using (TaskNoteDataAccess task = new TaskNoteDataAccess()) 
            {
                if (express!=null)
                {
                    return task.Set<T>().Where(express).ToList();
                }
                else
                {
                    return task.Set<T>().ToList();
                }
                
            }
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public static void AddEntity<T>(T t)where T:TopBase
        {
            using(TaskNoteDataAccess task=new TaskNoteDataAccess())
            {
                task.Entry<T>(t).State = EntityState.Added;
                task.SaveChangesAsync();
            }
        }

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ListEntity"></param>
        public static void AddEntityList<T> (List<T> ListEntity) where T : TopBase
        {
            using (TaskNoteDataAccess task = new TaskNoteDataAccess())
            {
                task.Entry(ListEntity).State = EntityState.Added;
                //task.Set<T>().AddRangeAsync(ListEntity);
                task.SaveChangesAsync();
            }
        }

        /// <summary>
        /// 修改某一模型数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        public static void Update<T>(T model) where T : TopBase
        {
            using (TaskNoteDataAccess task = new TaskNoteDataAccess())
            {
                if (task.Entry<T>(model).State == EntityState.Detached)
                {
                    task.Set<T>().Attach(model);
                    task.Entry<T>(model).State = EntityState.Modified;
                }
                task.SaveChangesAsync();
            }
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="EntityList"></param>
        public static void UpdateList<T>(List<T> EntityList) where T : TopBase
        {
            using (TaskNoteDataAccess task = new TaskNoteDataAccess())
            {
                if (task.Entry(EntityList).State == EntityState.Detached)
                {
                    task.Set<List<T>>().Attach(EntityList);
                    task.Entry(EntityList).State = EntityState.Modified;
                }
                task.SaveChangesAsync();
            }
        }

        /// <summary>
        /// 根据条件进行真删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="express"></param>
        public static void Delete<T>(Expression<Func<T, bool>> express) where T : TopBase
        { 
            using (TaskNoteDataAccess task = new TaskNoteDataAccess()) 
            { 
                T v = task.Set<T>().SingleOrDefault(express);
                if (v == null)
                { 
                    return;
                }
                //task.Set<T>().Remove(v);
                task.Entry(v).State = EntityState.Deleted;
                task.SaveChangesAsync();
            } 
        }

        /// <summary>
        /// 根据实体进行删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public static void Delete<T>(T t) where T : TopBase
        {
            using (TaskNoteDataAccess task = new TaskNoteDataAccess())
            {
                task.Entry(t).State = EntityState.Deleted;
                task.SaveChangesAsync();
            }
        }
        /// <summary>
        /// 假删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="express"></param>
        public static void FalseDelete<T>(Expression<Func<T, bool>> express) where T : TopBase
        { 
            using (TaskNoteDataAccess task = new TaskNoteDataAccess()) 
            {
                var list= task.Set<T>().Where(express).ToList();
                foreach (var item in list)
                {
                    item.IsDelete = true;
                }

                task.Entry(list).State = EntityState.Modified;
                task.SaveChangesAsync();
            } 
        }

        /// <summary>
        /// 真删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="express"></param>
        public static void RealDelete<T>(Expression<Func<T, bool>> express) where T : TopBase
        {
            using (TaskNoteDataAccess task = new TaskNoteDataAccess())
            {
                var list = task.Set<T>().Where(express).ToList();
                
                task.Entry(list).State = EntityState.Deleted;
                task.SaveChangesAsync();
            }
        }
    }
}
