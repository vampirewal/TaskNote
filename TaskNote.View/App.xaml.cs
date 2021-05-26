using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TaskNote.Core.SimpleMVVM;
using TaskNote.DataAccess;
using TaskNote.ViewModel;

namespace TaskNote.View
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            //base.OnStartup(e);
            //此处调用一下，方便初次打开系统的时候，创建数据库
            using (TaskNoteDataAccess dbContext = new TaskNoteDataAccess())
            {
                dbContext.Database.EnsureCreated();
            }

            if (Convert.ToBoolean(WindowsManager.CreateDialogWindowByViewModelResult(new LoginView(), new LoginViewModel())))
            {
                WindowsManager.CreateWindow("MainView", ShowMode.Dialog, new MainViewModel());
            }
        }
    }
}
