using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace XiangJiang.Windows.Form.Common
{
    /// <summary>
    ///     Application 帮助类
    /// </summary>
    public static class ApplicationHelper
    {
        #region Methods

        /// <summary>
        ///     设置程序唯一实例运行.
        /// </summary>
        /// <param name="oneInstancaAction">委托，参数：是否是唯一实例</param>
        public static void ApplyOnlyOneInstance(Action<bool> oneInstancaAction)
        {
            // ReSharper disable once UnusedVariable
            var mutex = new Mutex(false, Assembly.GetCallingAssembly().FullName, out var onlyRun);
            oneInstancaAction(onlyRun);
        }

       

        /// <summary>
        ///     捕获退出
        /// </summary>
        /// <param name="mainForm">主窗口</param>
        /// <param name="closingFunc">窗体正在关闭事件</param>
        public static void CapturedExit(System.Windows.Forms.Form mainForm, Func<bool> closingFunc)
        {
            mainForm.FormClosing += (sender, e) =>
            {
                if (closingFunc != null)
                    e.Cancel = !closingFunc();
                else
                    e.Cancel = true;
            };
            mainForm.FormClosed += (sender, e) =>
            {
                Environment.Exit(Environment.ExitCode);
                var runMainForm = sender as System.Windows.Forms.Form;
                runMainForm.Dispose();
                runMainForm.Close();
            };
        }

        /// <summary>
        ///     获取exe执行文件夹路径
        /// </summary>
        /// <returns>exe执行文件夹路径</returns>
        public static string GetExecuteDirectory()
        {
            return Path.GetDirectoryName(Application.ExecutablePath);
        }

        /// <summary>
        ///     全屏
        /// </summary>
        /// <param name="form">Form</param>
        public static void ToFullScreen(System.Windows.Forms.Form form)
        {
            ToFullScreen(form, 0);
        }

        /// <summary>
        ///     全屏
        /// </summary>
        /// <param name="form">Form</param>
        /// <param name="screen">0：主屏;1,2分屏</param>
        public static void ToFullScreen(System.Windows.Forms.Form form, int screen)
        {
            form.WindowState = FormWindowState.Maximized;
            form.StartPosition = FormStartPosition.Manual;
            form.Bounds = Screen.AllScreens[screen].Bounds;
        }

        /// <summary>
        ///     该方法只是暂时的将应用程序占用的内存移至虚拟内存，一旦应用程序被激活或者有操作请求时，这些内存又会被重新占用
        /// </summary>
        /// <param name="maxSize">最大虚拟内存，单位字节</param>
        /// <param name="minSize">最小虚拟内存，单位字节</param>
        public static void SetWorkingSet(int maxSize, int minSize)
        {
            var process = Process.GetCurrentProcess();
            process.MaxWorkingSet = (IntPtr) maxSize;
            process.MinWorkingSet = (IntPtr) minSize;
        }

        #endregion Methods
    }
}