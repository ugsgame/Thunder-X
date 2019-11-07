using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thunder.GameLogic.Common
{
    public interface IRunnable
    {
        void OnUpdate(float dTime);
    }
}
