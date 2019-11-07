﻿
using System;
using System.Windows.Forms;
using System.Drawing.Design;
using System.Windows.Forms.Design;
namespace ThunderEditor.Editor
{
    /// <summary>  
    /// IMSOpenFileInPropertyGrid 的摘要说明。    
    /// </summary>  

    public class PropertyGridFileItem : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

            if (edSvc != null)
            {
                // 可以打开任何特定的对话框
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.AddExtension = false;
                if (dialog.ShowDialog().Equals(DialogResult.OK))
                {

                    return dialog.FileName;

                }

            }
            return value;

        }

    }

}
