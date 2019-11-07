using MatrixEngine.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thunder.GameLogic.Gaming
{
    public class ColorChange
    {
        Color32 fromCol;
        Color32 toCol;
        Color32 aimCol;

        public ColorChange()
        {
            fromCol = Color32.White;
            toCol = Color32.White;
        }

        public ColorChange(Color32 from, Color32 to)
        {
            fromCol = from;
            toCol = to;
        }

        public Color32 FromColor
        {
            set { fromCol = value; }
        }
        public Color32 ToColor
        {
            set { toCol = value; }
        }
        public Color32 Color
        {
            get { return aimCol; }
        }

        public virtual void OnUpdate(float percent)
        {
            if (percent >= 1.0f)
            {
                aimCol = toCol;
            }
            else
            {
                aimCol.R = (byte)(fromCol.R + ((toCol.R - fromCol.R) * percent));
                aimCol.G = (byte)(fromCol.G + ((toCol.G - fromCol.G) * percent));
                aimCol.B = (byte)(fromCol.B + ((toCol.B - fromCol.B) * percent));
            }
        }
    }
}
