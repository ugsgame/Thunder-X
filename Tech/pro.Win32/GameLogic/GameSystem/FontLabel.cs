
namespace Thunder.GameLogic.GameSystem
{





    /// <summary>
    ///* 对话文本的xml的格式是单边标签属性格式，本协议LABEL起头的成员字段表示标签，<br>
    ///* XML起头是拼合的标签，LABEL为标签标记，ATT起头的为标签属性，VALUE为值属性，标签属性可以任意写在标签内，但程序不一定完全支持<br>
    ///* xml使用事例：
    ///* <p>
    ///* xml标签：<br>
    ///* //<font fsize=25 fcolor=ffffff>表达文字1<font fcolor=15ff75>表达文字2<ani id=15><br>
    ///* 显示结果：<br>
    ///* 表达文字1表达文字2动画<br>
    ///* </p>
    ///*
    /// </summary>
    public static class FontLabel
    {

        /// <summary>
        ///* 字体标签
        /// </summary>
        public const string LABEL_FONT = "font";

        /// <summary>
        ///* 动画标签
        /// </summary>
        public const string LABEL_ANI = "ani";

        /// <summary>
        ///* 换行标签
        /// </summary>
        public const string LABEL_BR = "br";

        /// <summary>
        ///* 元件标签
        /// </summary>
        public const string LABEL_CELL = "cell";

        /// <summary>
        ///* 图片标签
        /// </summary>
        public const string LABEL_PNG = "png";

        /// <summary>
        ///* id定义，可以用于字体和cell等标签
        /// </summary>
        public const string LABEL_Align = "align";

        /// <summary>
        ///* 字体大小
        /// </summary>
        public const string ATT_FONT_fsize = "fsize";

        /// <summary>
        ///* 字体颜色
        /// </summary>
        public const string ATT_FONT_fcolor = "fcolor";

        /// <summary>
        ///* 透明度
        /// </summary>
        public const string ATT_FONT_alpha = "alpha";

        /// <summary>
        ///* 背影颜色
        /// </summary>
        public const string ATT_FONT_bcolor = "bcolor";

        /// <summary>
        ///* 粗体
        /// </summary>
        public const string ATT_FONT_born = "born";

        /// <summary>
        ///* 下划线颜色
        /// </summary>
        public const string ATT_FONT_line = "line";

        /// <summary>
        ///* 边框颜色
        /// </summary>
        public const string ATT_FONT_border = "border";

        /// <summary>
        ///* id定义，可以用于字体和cell等标签
        /// </summary>
        public const string ATT_id = "id";

        /// <summary>
        ///* 元件等宽
        /// </summary>
        public const string ATT_width = "width";

        /// <summary>
        ///* 元件等高
        /// </summary>
        public const string ATT_height = "height";
        
        /// <summary>
        ///* 元件等缩放比例
        /// </summary>
        public const string ATT_scale = "scale";

        /// <summary>
        ///* 用户任意数据定义，用于定义各种自由格式的数据
        /// </summary>
        public const string ATT_userdate = "userdate";

        /// <summary>
        ///* 角色ID属性的标签
        /// </summary>
        public const string ATT_roleID = "roleID";

        /// <summary>
        ///* 角色名字属性的标签
        /// </summary>
        public const string ATT_playerName = "playerName";

        /// <summary>
        ///* 物品ID属性的标签
        /// </summary>
        public const string ATT_itemID = "itemID";

        /// <summary>
        ///* 物品名字属性的标签
        /// </summary>
        public const string ATT_templetID = "templetID";

        /// <summary>
        ///* 对齐方式属性的标签
        /// </summary>
        public const string ATT_Type = "type";

        /// <summary>
        ///* 对齐方式左对齐的值
        /// </summary>
        public const int Value_Align_Left = 0;

        /// <summary>
        ///* 对齐方式中间对齐的值
        /// </summary>
        public const int Value_Align_Center = 1;

        /// <summary>
        ///* 对齐方式右对齐的值
        /// </summary>
        public const int Value_Align_Right = 2;

        /// <summary>
        ///* 默认值，如果设置其他颜色为默认颜色，将按照文字颜色为准
        /// </summary>
        public const string VALUE_default = "default";

    }

}

