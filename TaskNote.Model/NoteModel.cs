#region << 文 件 说 明 >>
/*----------------------------------------------------------------
// 文件名称：NoteModel
// 创 建 者：杨程
// 创建时间：2021/6/3 13:23:40
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
    [Table("Notes")]
    public class NoteModel : TopBase
    {
        public NoteModel()
        {
            //构造函数
        }

        #region 属性
        private string _FolderId;
        [Display(Name = "文件夹ID")]
        [Column("FolderId")]
        public string FolderId
        {
            get { return _FolderId; }
            set { _FolderId = value; }
        }

        private string _UserID;
        [Display(Name = "关联用户")]
        [Column("User")]
        public string UserID
        {
            get { return _UserID; }
            set { _UserID = value; DoNotify(); }
        }

        private User _User;
        [Display(Name = "关联用户")]
        [Column("User")]
        public User User
        {
            get { return _User; }
            set { _User = value; DoNotify(); }
        }



        private string _NoteName;
        [Display(Name = "笔记名称")]
        [Column("NoteName")]
        public string NoteName
        {
            get { return _NoteName; }
            set { _NoteName = value; DoNotify(); }
        }

        private string _NoteContext;
        [Display(Name = "笔记内容")]
        [Column("NoteContext")]
        public string NoteContext
        {
            get { return _NoteContext; }
            set { _NoteContext = value; DoNotify(); }
        }

        private bool _IsFocus;
        [Display(Name = "是否关注")]
        [Column("IsFocus")]
        public bool IsFocus
        {
            get { return _IsFocus; }
            set { _IsFocus = value; DoNotify(); }
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
