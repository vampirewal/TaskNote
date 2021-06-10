#region << 文 件 说 明 >>
/*----------------------------------------------------------------
// 文件名称：FileAttachmentModel
// 创 建 者：杨程
// 创建时间：2021/4/21 14:26:22
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
using System.Text;
using TaskNote.Core.Models;

namespace TaskNote.Model
{
    /// <summary>
    /// 文件的附件类
    /// </summary>
    [Table("FileAttachment")]
    public class FileAttachmentModel :TopBase
    {
        #region 属性
        
        private string _AttachmentName;
        [Column("AttachmentName")]
        public string AttachmentName
        {
            get { return _AttachmentName; }
            set { _AttachmentName = value; DoNotify(); }
        }


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

        private byte[] _Attachment;
        /// <summary>
        /// 附件存byte
        /// </summary>
        [Column("Attachment")]
        public byte[] Attachment
        {
            get { return _Attachment; }
            set { _Attachment = value; DoNotify(); }
        }

        private string _AttachmentType;
        /// <summary>
        /// 附件类型，1是图片，2是文件（word或excel这种）
        /// </summary>
        [Column("AttachmentType")]
        public string AttachmentType
        {
            get { return _AttachmentType; }
            set { _AttachmentType = value; DoNotify(); }
        }

        private int _AttachmentByte;
        [Column("AttachmentByte")]
        public int AttachmentByte
        {
            get { return _AttachmentByte; }
            set { _AttachmentByte = value; DoNotify(); }
        }

        #endregion
    }
}
