#region << 文 件 说 明 >>
/*----------------------------------------------------------------
// 文件名称：TaskGroup
// 创 建 者：杨程
// 创建时间：2021/5/26 17:29:50
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
    [Table("TaskGroup")]
    public class TaskGroup:TopBase
    {
        public TaskGroup()
        {
            //构造函数
        }

        #region 属性
        private string _taskModelID;
        [Display(Name = "关联Task")]
        [Column("taskModelID")]
        public string taskModelID
        {
            get { return _taskModelID; }
            set { _taskModelID = value; DoNotify(); }
        }

        private TaskModel _taskModel;
        [Display(Name = "关联Task")]
        [Column("taskModel")]
        public TaskModel taskModel
        {
            get { return _taskModel; }
            set { _taskModel = value; DoNotify(); }
        }

        private string _GroupName;
        [Display(Name = "任务组名称")]
        [Column("GroupName")]
        public string GroupName
        {
            get { return _GroupName; }
            set { _GroupName = value; DoNotify(); }
        }

        private string _GroupBackgroundColor;
        [Display(Name = "任务组背景色")]
        [Column("GroupBackgroundColor")]
        public string GroupBackgroundColor
        {
            get { return _GroupBackgroundColor; }
            set { _GroupBackgroundColor = value; DoNotify(); }
        }

        private bool _IsCanDelete;
        [Display(Name = "是否能删除")]
        [Column("IsCanDelete")]
        public bool IsCanDelete
        {
            get { return _IsCanDelete; }
            set { _IsCanDelete = value; DoNotify(); }
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
