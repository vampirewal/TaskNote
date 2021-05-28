#region << 文 件 说 明 >>
/*----------------------------------------------------------------
// 文件名称：GlobalDataManager
// 创 建 者：杨程
// 创建时间：2021/5/26 11:16:03
// 文件版本：V1.0.0
// ===============================================================
// 功能描述：
//		
//
//----------------------------------------------------------------*/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskNote.Core;
using TaskNote.Model;

namespace TaskNote.ViewModel
{
    public class GlobalDataManager:BaseSingleton<GlobalDataManager>
    {

        #region 属性
        /// <summary>
        /// 当前登陆用户信息
        /// </summary>
        public User LoginUserInfo { get; set; }
        #endregion

        #region 公共方法

        #endregion

        #region 私有方法

        #endregion

        #region 命令

        #endregion
    }
}
