using System.ComponentModel;
using System.Globalization;
using System.Threading;
using WinForms=System.Windows.Forms;

namespace XiangJiang.Windows.Form.Common
{
    /// <summary>
    ///     WinForm语言国际化帮助类
    /// </summary>
    /// 时间：2015-09-02 16:45
    /// 备注：
    public static class LanguageHelper
    {
        #region Methods

        /// <summary>
        ///     设置界面国际化
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="form">需要国际化的界面</param>
        /// <param name="lang">language:zh-CN, en-US</param>
        /// 时间：2015-09-02 16:44
        /// 备注：
        public static void SetGlobalization<T>(this T form, string lang)
            where T : WinForms.Form
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);
            if (form != null)
            {
                var formType = form.GetType();
                var resources = new ComponentResourceManager(formType);
                resources.ApplyResources(form, "$this");
                ApplyGlobalization(form, resources);
            }
        }

        private static void ApplyGlobalization(WinForms.Control control, ComponentResourceManager resources)
        {
            if (control is WinForms.MenuStrip)
            {
                resources.ApplyResources(control, control.Name);
                var ms = (WinForms.MenuStrip) control;
                if (ms.Items.Count > 0)
                    foreach (WinForms.ToolStripMenuItem c in ms.Items)
                        ApplyGlobalization(c, resources);
            }

            foreach (WinForms.Control c in control.Controls)
            {
                resources.ApplyResources(c, c.Name);
                ApplyGlobalization(c, resources);
            }
        }

        private static void ApplyGlobalization(WinForms.ToolStripMenuItem item, ComponentResourceManager resources)
        {
            // ReSharper disable once IsExpressionAlwaysTrue
            if (item is WinForms.ToolStripMenuItem)
            {
                resources.ApplyResources(item, item.Name);
                var tsmi = item;
                if (tsmi.DropDownItems.Count > 0)
                    foreach (WinForms.ToolStripMenuItem c in tsmi.DropDownItems)
                        ApplyGlobalization(c, resources);
            }
        }

        #endregion Methods
    }
}