using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ARCPMS_ENGINE.src.mrs.Manager.CarWashManager.DB
{
    interface CarWashDaoService
    {
        /// <summary>
        /// insert queue for new request
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        int InsertQueueForWashing(int job);
        /// <summary>
        /// get first part of path
        /// </summary>
        /// <param name="washQId"></param>
        /// <returns></returns>
        bool FindCarWashingPathFirst(int washQId);
        /// <summary>
        /// get second part of path
        /// </summary>
        /// <param name="washQId"></param>
        /// <returns></returns>
        int FindCarWashingPathSecond(int washQId);
        /// <summary>
        /// update db once car reached there inside the car wash slot
        /// </summary>
        /// <param name="washQId"></param>
        /// <returns></returns>
        bool updateBeforeWashing(int washQId);
        /// <summary>
        /// update db once car get from the car wash slot
        /// </summary>
        /// <param name="washQId"></param>
        /// <returns></returns>
        bool updateAfterWashing(int washQId);
        /// <summary>
        /// update db once car reached back to the slot
        /// </summary>
        /// <param name="washQId"></param>
        /// <returns></returns>
        bool updateAfterReachedInSlot(int washQId);
        /// <summary>
        /// update if car wash slot ready
        /// </summary>
        /// <param name="isReady"></param>
        void UpdateIsWashReady(bool isReady);
        /// <summary>
        /// update flag for enabling car wash finish trigger
        /// </summary>
        /// <param name="isCarWashFinish"></param>
        void UpdateCarWashFinishTrigger(int isCarWashFinish);
        /// <summary>
        /// get status of user clicked the car wash finish trigger
        /// </summary>
        /// <returns></returns>
        bool IsCarWashFinishTriggeredByUser();
        /// <summary>
        /// update after car wash transaction finished
        /// </summary>
        /// <param name="washQId"></param>
        /// <returns></returns>
        bool updateAfterProcessing(int washQId);
    }
}
