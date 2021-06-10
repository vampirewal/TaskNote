#region << 文 件 说 明 >>
/*----------------------------------------------------------------
// 文件名称：RecycleModel
// 创 建 者：杨程
// 创建时间：2021/6/9 9:35:08
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
    [Table("Recycle")]
    public class RecycleModel:TopBase
    {
        public RecycleModel()
        {
            //构造函数
        }

        private string _SourceID;
        /// <summary>
        /// 源单ID
        /// </summary>
        [Display(Name = "源单ID")]
        [Column("SourceID")]
        public string SourceID
        {
            get { return _SourceID; }
            set { _SourceID = value;DoNotify(); }
        }

        private string _SourceName;
        /// <summary>
        /// 源单名称
        /// </summary>
        [Display(Name = "源单名称")]
        [Column("SourceName")]
        public string SourceName
        {
            get { return _SourceName; }
            set { _SourceName = value; DoNotify(); }
        }

        private SourceType _sourceType;
        /// <summary>
        /// 源单类型
        /// </summary>
        [Display(Name = "源单类型")]
        [Column("sourceType")]
        public SourceType sourceType
        {
            get { return _sourceType; }
            set { _sourceType = value; }
        }

        [NotMapped]
        public bool IsSelected { get; set; }
    }
    /// <summary>
    /// 源单类型
    /// </summary>
    public enum SourceType
    {
        /// <summary>
        /// 任务
        /// </summary>
        [Display(Name = "任务")]
        Task =0,
        /// <summary>
        /// 任务明细
        /// </summary>
        [Display(Name = "任务明细")]
        TaskDtl =1,
        /// <summary>
        /// 任务组
        /// </summary>
        [Display(Name = "任务组")]
        TaskGroup =2,
        /// <summary>
        /// 笔记
        /// </summary>
        [Display(Name = "笔记")]
        Note =3,
        /// <summary>
        /// 附件
        /// </summary>
        [Display(Name = "附件")]
        Attachment =4
    }
}
