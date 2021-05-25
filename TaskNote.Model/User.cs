#region << 文 件 说 明 >>
/*----------------------------------------------------------------
// 文件名称：User
// 创 建 者：杨程
// 创建时间：2021/5/25 18:47:46
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
using TaskNote.Core.Models;

namespace TaskNote.Model
{
    [Table("User")]
    public class User:TopBase
    {
        public User()
        {
            //构造函数
        }

        #region 属性
        [Display(Name ="用户姓名")]
        public string UserName { get; set; }

        [Display(Name = "用户密码")]
        public string Password { get; set; }
        #endregion

        #region 公共方法

        #endregion

        #region 私有方法

        #endregion

        #region 命令

        #endregion
    }
}
