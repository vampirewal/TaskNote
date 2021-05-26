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
    public class User : TopBase
    {
        public User()
        {
            //构造函数
        }

        #region 属性
        private string userName;
        [Display(Name = "用户姓名")]
        [Column("UserName")]
        public string UserName { get => userName; set { userName = value; DoNotify(); } }

        private string password;
        [Display(Name = "用户密码")]
        [Column("Password")]
        public string Password { get => password; set { password = value; DoNotify(); } }

        private bool _IsRemember;
        /// <summary>
        /// 是否记住密码，0是不记住，1是记住
        /// </summary>
        [Column("IsRemember")]
        public bool IsRemember
        {
            get { return _IsRemember; }
            set { _IsRemember = value; DoNotify(); }
        }

        private bool _IsLogin;
        
        /// <summary>
        /// 是否登陆
        /// </summary>
        [Column("IsLogin")]
        public bool IsLogin
        {
            get { return _IsLogin; }
            set { _IsLogin = value; DoNotify(); }
        }


        public void ClearInfo()
        {
            this.ID = Guid.Empty.ToString();
            this.UserName = string.Empty;
            this.Password = string.Empty;
            this.IsRemember = false;
            this.IsLogin = false;
        }
        #endregion

        #region 公共方法

        #endregion

        #region 私有方法

        #endregion

        #region 命令

        #endregion
    }
}
