/* 项目名称： ViewModelBase.cs
 * 命名空间： TaskNote.Core.SimpleMVVM
 * 类 名 称: ViewModelBase
 * 作   者 : 杨程
 * 概   述 : 
 * 创建时间 : 2021/2/20 18:35:09
 * 更新时间 : 2021/2/20 18:35:09
 * CLR版本 : 4.0.30319.42000
 * ******************************************************
 * Copyright@Administrator 2021 .All rights reserved.
 * ******************************************************
 */

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace TaskNote.Core.SimpleMVVM
{
    /// <summary>
    /// ViewModel基类
    /// </summary>
    public  class ViewModelBase : NotifyBase
    {
        public ViewModelBase()
        {
            InitData();
            MessengerRegister();
        }
        /// <summary>
        /// 判断是不是设计器模式
        /// </summary>
        public bool IsInDesignMode
        {
            get { return DesignerProperties.GetIsInDesignMode(new DependencyObject()); }
        }

        
        /// <summary>
        /// 目标View
        /// </summary>
        public FrameworkElement View { get; set; }

        /// <summary>
        /// UI线程调用
        /// </summary>
        /// <param name="action"></param>
        public void UIInvoke(Action action)
        {
            this.View.Dispatcher.Invoke(action);
        }

        public string Title { get; set; } = "未命名窗体";

        /// <summary>
        /// 当窗体是DialogShow的方式打开的时候，可以通过重写这个方法，将值传回主窗体
        /// </summary>
        /// <returns></returns>
        public virtual object GetResult()
        {
            return null;
        }

        /// <summary>
        /// 初始化页面数据，由ViewModelBase的构造函数调用，重写后不用再调用
        /// </summary>
        public virtual void InitData()
        {

        }
        /// <summary>
        /// 初始化页面数据，由创建时传递参数
        /// </summary>
        /// <param name="obj">传入参数</param>
        public virtual void PassData(object obj)
        {

        }

        /// <summary>
        /// 通用消息注册
        /// </summary>
        public virtual void MessengerRegister()
        {

        }

        #region 命令
        /// <summary>
        /// 通用的窗体关闭命令，但是每个窗体的关闭可能需要做的事情不一样，故非DialogWindow窗体，重写这个命令。
        /// </summary>
        public virtual RelayCommand CloseWindowCommand => new RelayCommand(() =>
        {
            WindowsManager.CloseWindow(View as Window);
        });

        public virtual RelayCommand<Window> MaxWindowCommand => new RelayCommand<Window>((w) =>
        {
            if (w.WindowState == WindowState.Normal)
            {
                w.WindowState = WindowState.Maximized;
            }
            else
            {
                w.WindowState = WindowState.Normal;
            }
        });

        public virtual RelayCommand<Window> MinWindowCommand => new RelayCommand<Window>((w) =>
        {
            w.WindowState = WindowState.Minimized;
        });

        public virtual RelayCommand<Window> WindowMoveCommand => new RelayCommand<Window>((w) =>
        {
            if (w != null)
            {
                w.DragMove();
            }
        });

        public virtual RelayCommand MouseDownCommand => new RelayCommand(() =>
          {
              Keyboard.ClearFocus();
          });
        #endregion
    }
}
