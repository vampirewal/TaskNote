#region << 文 件 说 明 >>
/*----------------------------------------------------------------
// 文件名称：TaskModel
// 创 建 者：杨程
// 创建时间：2021/5/26 17:04:34
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
    [Table("Tasks")]
    public class TaskModel:TopBase
    {
        public TaskModel()
        {
            //构造函数
        }

        #region 属性
        private string _TaskName;
        [Display(Name = "任务名称")]
        [Column("TaskName")]
        public string TaskName
        {
            get { return _TaskName; }
            set { _TaskName = value;DoNotify(); }
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

        private string _TaskGroupID;
        [Display(Name = "关联任务组")]
        [Column("TaskGroupID")]
        public string TaskGroupID
        {
            get { return _TaskGroupID; }
            set { _TaskGroupID = value; DoNotify(); }
        }

        private TaskGroup _taskGroup;
        [Display(Name = "关联任务组")]
        [Column("TaskGroup")]
        public TaskGroup TaskGroup
        {
            get { return _taskGroup; }
            set { _taskGroup = value; DoNotify(); }
        }


        private DateTime? _StartTime;
        [Display(Name = "开始时间")]
        [Column("StartTime")]
        public DateTime? StartTime
        {
            get { return _StartTime; }
            set { _StartTime = value; DoNotify(); }
        }

        private DateTime? _EndTime;
        [Display(Name = "结束时间")]
        [Column("EndTime")]
        public DateTime? EndTime
        {
            get { return _EndTime; }
            set { _EndTime = value; DoNotify(); }
        }
        #endregion

        #region 公共方法

        #endregion

        #region 私有方法

        #endregion

        #region 命令

        #endregion
    }

    [Table("TaskDtl")]
    public class TaskDtlModel : TopBase
    {
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

        private string _TaskContext;
        [Display(Name = "Task内容")]
        [Column("TaskContext")]
        public string TaskContext
        {
            get { return _TaskContext; }
            set { _TaskContext = value; DoNotify(); }
        }

        private bool _IsChecked;
        [Display(Name = "是否勾选")]
        [Column("IsChecked")]
        public bool IsChecked
        {
            get { return _IsChecked; }
            set { _IsChecked = value; DoNotify(); }
        }


    }
}
