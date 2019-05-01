using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ARCPMS_ENGINE.src.mrs.Manager.ParkingManager.Model;
using ARCPMS_ENGINE.src.mrs.Manager.ParkingManager.Controller;
using ARCPMS_ENGINE.src.mrs.Manager.CarWashManager.DB;
using ARCPMS_ENGINE.src.mrs.Manager.QueueManager.Model;
using System.Threading;
using ARCPMS_ENGINE.src.mrs.OPCOperations;
using ARCPMS_ENGINE.src.mrs.OPCOperations.OPCOperationsImp;
using ARCPMS_ENGINE.src.mrs.OPCConnection.OPCConnectionImp;
using ARCPMS_ENGINE.src.mrs.Modules.Machines.VLC.Model;
using ARCPMS_ENGINE.src.mrs.Global;
using ARCPMS_ENGINE.src.mrs.Manager.QueueManager.Controller;
using ARCPMS_ENGINE.src.mrs.Config;

namespace ARCPMS_ENGINE.src.mrs.Manager.CarWashManager.Controller
{
    class CarWashControllerImp: CarWashControllerService
    {
        ParkingControllerService objParkingControllerService = null;
        CarWashDaoService objCarWashDaoService = null;
        QueueControllerService objQueueControllerService = null;

        public int InsertQueueForWashing(int job)
        {
            if (objCarWashDaoService == null) objCarWashDaoService = new CarWashDaoImp();

            return objCarWashDaoService.InsertQueueForWashing(job);
        }

        public List<PathDetailsData> FindCarWashingPathFirst(int washQId)
        {
            if (objCarWashDaoService == null) objCarWashDaoService = new CarWashDaoImp();
            if (objParkingControllerService == null) objParkingControllerService = new ParkingControllerImp();
            objCarWashDaoService.FindCarWashingPathFirst(washQId);
            return objParkingControllerService.GetPathDetails(washQId);
        }

        public List<PathDetailsData> FindCarWashingPathSecond(int washQId)
        {
            if (objCarWashDaoService == null) objCarWashDaoService = new CarWashDaoImp();
            if (objParkingControllerService == null) objParkingControllerService = new ParkingControllerImp();
            int pathId = 0;
            pathId = objCarWashDaoService.FindCarWashingPathSecond(washQId);
            return pathId>0?objParkingControllerService.GetPathDetails(washQId, pathId):null;
        }

        public bool ProcessWashPath(QueueData objQueueData)
        {
            if (objParkingControllerService == null) objParkingControllerService = new ParkingControllerImp();
            if (objQueueControllerService == null) objQueueControllerService = new QueueControllerImp();

            List<PathDetailsData> lstPathDetails = null;
           // QueueData objQueueData = new QueueData();
            try
            {

                lstPathDetails = new List<PathDetailsData>();
                do
                {

                    lstPathDetails = FindCarWashingPathFirst(objQueueData.queuePkId);
                    if (lstPathDetails == null) Thread.Sleep(1500);

                    /**checking transaction deleted or not****/
                    objQueueControllerService.CancelIfRequested(objQueueData.queuePkId);
                    /******/
                } while (lstPathDetails == null);
                //objQueueData.queuePkId = washQId;
                objParkingControllerService.ExcecuteCommands(objQueueData);

                if (!IsSameFloorTravel(lstPathDetails))
                {
                    lstPathDetails = new List<PathDetailsData>();

                    do
                    {
                        lstPathDetails = FindCarWashingPathSecond(objQueueData.queuePkId);
                        if (lstPathDetails == null) Thread.Sleep(1500);

                        /**checking transaction deleted or not****/
                        objQueueControllerService.CancelIfRequested(objQueueData.queuePkId);
                        /******/
                    } while (lstPathDetails == null);

                  //  objQueueData.queuePkId = washQId;
                    objParkingControllerService.ExcecuteCommands(objQueueData);
                }
                objCarWashDaoService.updateAfterProcessing(objQueueData.queuePkId);
            }
            catch (OperationCanceledException errMsg)
            {
                Logger.WriteLogger(GlobalValues.PARKING_LOG, "Queue Id:" + objQueueData.queuePkId + " --TaskCanceledException 'ProcessWashPath':: " + errMsg.Message);

            }
            catch (Exception errMsg)
            {
                Logger.WriteLogger(GlobalValues.PARKING_LOG, "Queue Id:" + objQueueData.queuePkId + ":--Exception 'ProcessWashPath':: " + errMsg.Message);

            }
            finally
            {

            }
            return true;
        }
        public bool IsSameFloorTravel(List<PathDetailsData> lstPathDetails)
        {
            bool isSame=true;
            foreach (PathDetailsData pathDetails in lstPathDetails)
            {
                if (pathDetails.machineName.Contains("VLC"))
                {
                    isSame = false;
                    break;
                }

            }
            return isSame;
        }
        public void UpdateIsWashReady(bool isReady)
        {
            if (objCarWashDaoService == null) objCarWashDaoService = new CarWashDaoImp();

            objCarWashDaoService.UpdateIsWashReady(isReady);
           
        }
        public void UpdateCarWashFinishTrigger(int isCarWashFinish)
        {
            if (objCarWashDaoService == null) objCarWashDaoService = new CarWashDaoImp();

            objCarWashDaoService.UpdateCarWashFinishTrigger(isCarWashFinish);
           
        }
        public bool IsCarWashFinishTriggeredByUser()
        {
            if (objCarWashDaoService == null) objCarWashDaoService = new CarWashDaoImp();

            return objCarWashDaoService.IsCarWashFinishTriggeredByUser();
           
        }




        public bool SetHighCarBitForCarWash(VLCData objVLCData, bool bit)
        {
            bool success = false;
            using (OpcOperationsService opcd = new OpcOperationsImp(OpcConnection.GetOPCServerConnection()))
            {
                if (opcd.IsMachineHealthy(objVLCData.machineChannel + "." + objVLCData.machineCode + "." + OpcTags.WASH_CarWash_HighCar))
                {
                    success = opcd.WriteTag<bool>(objVLCData.machineChannel, objVLCData.machineCode, OpcTags.WASH_CarWash_HighCar, bit);
                }
            }
            return success;
        }

        public bool StartCarWash(Modules.Machines.VLC.Model.VLCData objVLCData)
        {
            bool success = false;
            using (OpcOperationsService opcd = new OpcOperationsImp(OpcConnection.GetOPCServerConnection()))
            {
                if (opcd.IsMachineHealthy(objVLCData.machineChannel + "." + objVLCData.machineCode + "." + OpcTags.WASH_CarWash_Start_Cycle))
                {
                    success = opcd.WriteTag<bool>(objVLCData.machineChannel, objVLCData.machineCode, objVLCData.command, true);
                    Thread.Sleep(2000);
                    success = opcd.WriteTag<bool>(objVLCData.machineChannel, objVLCData.machineCode, objVLCData.command, false);
                }
            }
            return success;
        }
    }
}
