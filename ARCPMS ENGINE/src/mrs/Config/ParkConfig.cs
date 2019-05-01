using ARCPMS_ENGINE.src.mrs.Global;
using ARCPMS_ENGINE.src.mrs.Manager.QueueManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ARCPMS_ENGINE.src.mrs.Config
{
    class ParkConfig
    {
        public static bool IsAutoRefreshActive(int requestType)
        {
            bool isActive = false;
            isActive =  GlobalValues.AUTO_REFRESH;
            isActive = isActive
                && (requestType == 1 || requestType == 0 || requestType == 5 || requestType == 6);
            return isActive;

        }
    }
}
