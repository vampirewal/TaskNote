﻿#region << 文 件 说 明 >>
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
using System.Collections.ObjectModel;
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
            taskDtlModels = new ObservableCollection<TaskDtlModel>();
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

        //private string _UserId;
        //[Display(Name = "关联用户")]
        //[Column("UserId")]
        //public string UserId
        //{
        //    get { return _UserId; }
        //    set { _UserId = value; DoNotify(); }
        //}

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

        private int _GroupSort;
        [Display(Name = "排序编号")]
        [Column("GroupSort")]
        public int GroupSort
        {
            get { return _GroupSort; }
            set { _GroupSort = value; DoNotify(); }
        }

        private bool _IsFinishedTag;
        [Display(Name = "是否完成标记")]
        [Column("IsFinishedTag")]
        public bool IsFinishedTag
        {
            get { return _IsFinishedTag; }
            set { _IsFinishedTag = value; }
        }

        private TaskGroupType _taskGroupType;
        [Display(Name = "任务组类型")]
        [Column("taskGroupType")]
        public TaskGroupType taskGroupType
        {
            get { return _taskGroupType; }
            set { _taskGroupType = value; DoNotify(); }
        }




        [NotMapped]
        public ObservableCollection<TaskDtlModel> taskDtlModels { get; set; }
        #endregion

        
    }

    public enum TaskGroupType
    {
        [Display(Name ="系统创建")]
        SystemCreate=0,
        [Display(Name = "用户创建")]
        CustomCreate =1
    }
}
