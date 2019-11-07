using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace ThunderEditor.Editor
{
    /// <summary>
    /// 编辑器预加载的资源
    /// TODO:要整理一下
    /// </summary>
    public class EditorRes
    {
        public static EditorRes Instance = new EditorRes();

        public BitmapImage Img_Pointer_Hand;

        public EditorRes()
        {
            Img_Pointer_Hand = new BitmapImage(new Uri(@"pack://application:,,,/Res/EditorIcons/hand.png", UriKind.RelativeOrAbsolute));
        }
    }
}
