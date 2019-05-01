using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ARCPMS_ENGINE.src.mrs.Manager.ParkingManager.Model;
using ARCPMS_ENGINE.src.mrs.Modules.Machines.VLC.Model;
using ARCPMS_ENGINE.src.mrs.Manager.QueueManager.Model;

namespace ARCPMS_ENGINE.src.mrs.Manager.CarWashManager.Controller
{
    interface CarWashControllerService
    {
        /// <summary>
        /// Insert new car wash request into the queue
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        int InsertQueueForWashing(int job);
        /// <summary>
        /// find first part path of car wash
        /// </summary>
        /// <param name="washQId"></param>
        /// <returns></returns>
        List<PathDetailsData> FindCarWashingPathFirst(int washQId);
        /// <summary>
        /// find second part path of car wash
        /// </summary>
        /// <param name="washQId"></param>
        /// <returns></returns>
        List<PathDetailsData> FindCarWashingPathSecond(int washQId);
        /// <summary>
        /// Control car wash transaction
        /// </summary>
        /// <param name="washQId"></param>
        /// <returns></returns>
        bool ProcessWashPath(QueueData objQueueData);
        /// <summary>
        /// update flag into the table if car wash slot is ready
        /// </summary>
        /// <param name="isReady"></param>
        void UpdateIsWashReady(bool isReady);
        /// <summary>
        /// update flag for enabling trigger button once car wash finished
        /// </summary>
        /// <param name="isCarWashFinish"></param>
        void UpdateCarWashFinishTrigger(int isCarWashFinish);
        /// <summary>
        /// check whether user clicked the car wash trigger or not
        /// </summary>
        /// <returns></returns>
        bool IsCarWashFinishTriggeredByUser();
        /// <summary>
        /// set bit in OPC if car type is high
        /// </summary>
        /// <param name="objVLCData"></param>
        /// <param name="bit"></param>
        /// <returns></returns>
        bool SetHighCarBitForCarWash(VLCData objVLCData,bool bit);
        /// <summary>
        /// start car wash once car reached there inside the car wash slot
        /// </summary>
        /// <param name="objVLCData"></param>
        /// <returns></returns>
        bool StartCarWash(VLCData objVLCData);
    }
}
