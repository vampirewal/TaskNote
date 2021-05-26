#region << 文 件 说 明 >>
/*----------------------------------------------------------------
// 文件名称：TopBase
// 创 建 者：杨程
// 创建时间：2021/5/25 17:09:00
// 文件版本：V1.0.0
// ===============================================================
// 功能描述：
//		
//
//----------------------------------------------------------------*/
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskNote.Core.SimpleMVVM;

namespace TaskNote.Core.Models
{
    public class TopBase:NotifyBase
    {
        public TopBase()
        {
            //构造函数
            IsDelete = false;
        }

        #region 属性
        private string _id;

        /// <summary>
        /// Id
        /// </summary>
        [Key]
        public string ID
        {
            get
            {
                if (string.IsNullOrEmpty( _id)  )
                {
                    _id = Guid.NewGuid().ToString();
                }
                return _id;
            }
            set
            {
                _id = value;
                DoNotify();
            }
        }

        private DateTime _CreateTime;
        [Column("CreateTime")]
        public DateTime CreateTime
        {
            get
            {
                if (_CreateTime==null)
                {
                    _CreateTime = DateTime.Now;
                }
                return _CreateTime;
            }
            set { _CreateTime = value; DoNotify(); }
        }

        private bool _IsDelete;
        [Column("IsDelete")]
        public bool IsDelete
        {
            get { return _IsDelete; }
            set { _IsDelete = value; DoNotify(); }
        }

        #endregion

        #region 公共方法

        public object GetID()
        {
            var idpro = this.GetType().GetProperties().Where(x => x.Name.ToLower() == "id").FirstOrDefault();
            var id = idpro.GetValue(this);
            return id;
        }
        #endregion

        #region 私有方法

        #endregion

        #region 命令

        #endregion
    }
}
