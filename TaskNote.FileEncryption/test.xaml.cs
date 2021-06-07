using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TaskNote.Core;

namespace TaskNote.FileEncryption
{
    /// <summary>
    /// test.xaml 的交互逻辑
    /// </summary>
    [Serializable]
    [Export(typeof(Core.IView))]
    [CustomExportMetadata(1, "test", "测试窗体", "测试到底能不能打开", "杨程", "1.0")]
    public partial class test : UserControl, IView
    {
        public test()
        {
            InitializeComponent();
            this.DataContext = new testViewModel();
        }

        public object Window => this;
    }
}
