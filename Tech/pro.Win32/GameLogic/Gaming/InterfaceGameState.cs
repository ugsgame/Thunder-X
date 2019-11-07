using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thunder.GameLogic.Gaming
{
    interface InterfaceGameState
    {
        void OnPause();
        void OnResume();
    }
}
