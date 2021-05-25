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

namespace TaskNote.Core.Models
{
    public class TopBase
    {
        public TopBase()
        {
            //构造函数
        }

        #region 属性
        private Guid _id;

        /// <summary>
        /// Id
        /// </summary>
        [Key]
        public Guid ID
        {
            get
            {
                if (_id == Guid.Empty)
                {
                    _id = Guid.NewGuid();
                }
                return _id;
            }
            set
            {
                _id = value;
            }
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
