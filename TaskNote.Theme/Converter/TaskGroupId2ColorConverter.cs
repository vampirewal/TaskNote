#region << 文 件 说 明 >>
/*----------------------------------------------------------------
// 文件名称：TaskGroupId2ColorConverter
// 创 建 者：杨程
// 创建时间：2021/6/3 19:58:12
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
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using TaskNote.DataAccess;
using TaskNote.Model;

namespace TaskNote.Theme.Converter
{
    public class TaskGroupId2ColorConverter:IValueConverter
    {
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value!=null)
            {
                string GroupId = value.ToString();
                using(TaskNoteDataAccess task=new TaskNoteDataAccess())
                {
                    
                    var current = task.TaskGroups.Where(w => w.ID == GroupId).FirstOrDefault();
                    if (current!=null)
                    {
                        Color color = (Color)ColorConverter.ConvertFromString(current.GroupBackgroundColor);
                        //return color;
                        return current.GroupBackgroundColor;
                    }
                    return "";
                }
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }

    public class TaskGroupID2TaskGroupNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string GroupName = "";
            if (value!=null)
            {
                string str = value.ToString();
                
                //ObservableCollection<TaskGroup> current = parameter as ObservableCollection<TaskGroup>;
                using (TaskNoteDataAccess task=new TaskNoteDataAccess())
                {
                    GroupName = task.TaskGroups.Where(w => w.ID == str).FirstOrDefault().GroupName;
                }
                
                 
                return GroupName;
            }
            return GroupName;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
