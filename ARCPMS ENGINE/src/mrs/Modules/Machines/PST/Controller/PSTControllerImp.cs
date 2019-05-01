using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ARCPMS_ENGINE.src.mrs.Modules.Machines.PST.DB;
using ARCPMS_ENGINE.src.mrs.OPCOperations;
using ARCPMS_ENGINE.src.mrs.Global;
using OPC;
using ARCPMS_ENGINE.src.mrs.OPCConnection.OPCConnectionImp;
using ARCPMS_ENGINE.src.mrs.OPCOperations.OPCOperationsImp;
using ARCPMS_ENGINE.src.mrs.Manager.QueueManager.Model;
using ARCPMS_ENGINE.src.mrs.Manager.ParkingManager.Controller;
using ARCPMS_ENGINE.src.mrs.Manager.ParkingManager.Model;
using ARCPMS_ENGINE.src.mrs.Manager.ErrorManager.Controller;
using ARCPMS_ENGINE.src.mrs.Manager.ErrorManager.DB;
using ARCPMS_ENGINE.src.mrs.Manager.ErrorManager.Model;

namespace ARCPMS_ENGINE.src.mrs.Modules.Machines.PST.Controller
{
    class PSTControllerImp : CommonServicesForMachines,PSTControllerService
    {
        PSTDaoService objPSTDaoService = null;
        ParkingControllerService objParkingControllerService = null;
        ErrorControllerService objErrorControllerService = null;
        ErrorDaoService objErrorDaoService = null;
        

        public List<Model.PSTData> GetPSTList()
        {
            if (objPSTDaoService == null) objPSTDaoService = new PSTDaoImp();
            return objPSTDaoService.GetPSTList();
        }
        public Model.PSTData GetPSTDetails(Model.PSTData objPSTData)
        {
            if (objPSTDaoService == null) objPSTDaoService = new PSTDaoImp();
            return objPSTDaoService.GetPSTDetails(objPSTData);
        }
        public Model.PSTData GetPSTDetailsInRange(int minAisle, int maxAisle)
        {
            if (objPSTDaoService == null) objPSTDaoService = new PSTDaoImp();
            return objPSTDaoService.GetPSTDetailsInRange(minAisle,maxAisle);
        }
       

        public bool UpdateMachineValues()
        {
            throw new NotImplementedException();
        }


        public bool AsynchReadSettings()
        {
            throw new NotImplementedException();
        }

        //public int GetCountOfPallet(Model.PSTData objPSTData)
        //{
        //    int palletCount = -1;
        //    using (OpcOperationsService opcd = new OpcOperationsImp(OpcConnection.GetOPCServerConnection()))
        //    {
        //        palletCount=opcd.ReadTag<Int32>(objPSTData.machineChannel, objPSTData.machineCode, OpcTags.PST_Pallet_Count);
        //    }
        //    return palletCount;
        //}
        public int GetCountOfPallet(Model.PSTData objPSTData)
        {
            int palletCount = -1;
            bool isPalletOnPST = true;
            
        using (OpcOperationsService opcd = new OpcOperationsImp(OpcConnection.GetOPCServerConnection()))
            {
                palletCount = opcd.ReadTag<int>(objPSTData.machineChannel, objPSTData.machineCode, OpcTags.PST_Pallet_Count);
             /*   if (!objPSTData.machineCode.Contains("PST_FLR4_01"))
            {
                isPalletOnPST = opcd.ReadTag<bool>(objPSTData.machineChannel, objPSTData.machineCode, OpcTags.PST_North_Pallet_Sensor_1);
                isPalletOnPST = isPalletOnPST || opcd.ReadTag<bool>(objPSTData.machineChannel, objPSTData.machineCode, OpcTags.PST_South_Pallet_Sensor_1);
                palletCount = isPalletOnPST == true ? 1 : 0;

                isPalletOnPST = opcd.ReadTag<bool>(objPSTData.machineChannel, objPSTData.machineCode, OpcTags.PST_North_Pallet_Sensor_2);
                isPalletOnPST = isPalletOnPST || opcd.ReadTag<bool>(objPSTData.machineChannel, objPSTData.machineCode, OpcTags.PST_South_Pallet_Sensor_2);
                palletCount += isPalletOnPST == true ? 1 : 0;

                isPalletOnPST = opcd.ReadTag<bool>(objPSTData.machineChannel, objPSTData.machineCode, OpcTags.PST_North_Pallet_Sensor_3);
                isPalletOnPST = isPalletOnPST || opcd.ReadTag<bool>(objPSTData.machineChannel, objPSTData.machineCode, OpcTags.PST_South_Pallet_Sensor_3);
                palletCount += isPalletOnPST == true ? 1 : 0;

                isPalletOnPST = opcd.ReadTag<bool>(objPSTData.machineChannel, objPSTData.machineCode, OpcTags.PST_North_Pallet_Sensor_4);
                isPalletOnPST = isPalletOnPST || opcd.ReadTag<bool>(objPSTData.machineChannel, objPSTData.machineCode, OpcTags.PST_South_Pallet_Sensor_4);
                palletCount += isPalletOnPST == true ? 1 : 0;
            }
                else if (objPSTData.machineCode.Contains("PST_FLR4_01"))
            {
                isPalletOnPST = opcd.ReadTag<bool>(objPSTData.machineChannel, objPSTData.machineCode, OpcTags.PST100_Pallet_Sensor_1_Bottom);
                palletCount = isPalletOnPST == true ? 1 : 0;

                isPalletOnPST = opcd.ReadTag<bool>(objPSTData.machineChannel, objPSTData.machineCode, OpcTags.PST100_Pallet_Sensor_2);
                palletCount += isPalletOnPST == true ? 1 : 0;

                isPalletOnPST = opcd.ReadTag<bool>(objPSTData.machineChannel, objPSTData.machineCode, OpcTags.PST100_Pallet_Sensor_3);
                palletCount += isPalletOnPST == true ? 1 : 0;

                isPalletOnPST = opcd.ReadTag<bool>(objPSTData.machineChannel, objPSTData.machineCode, OpcTags.PST100_Pallet_Sensor_4_Top);
                palletCount += isPalletOnPST == true ? 1 : 0;
            }*/

            }
        return palletCount;
        }
        
        public bool CheckPSTHealthy(Model.PSTData objPSTData)
        {
            bool isHealthy = false;
            if (objPSTDaoService==null ) objPSTDaoService = new PSTDaoImp();

            using (OpcOperationsService opcd = new OpcOperationsImp(OpcConnection.GetOPCServerConnection()))
            {
                if (opcd.IsMachineHealthy(objPSTData.machineChannel+"."+objPSTData.machineCode+"."+OpcTags.PST_Auto_Ready))
                {
                    isHealthy = opcd.ReadTag<bool>(objPSTData.machineChannel, objPSTData.machineCode, OpcTags.PST_Auto_Ready);
                    isHealthy = isHealthy && !objPSTDaoService.IsPSTDisabled(objPSTData.machineCode);
                    isHealthy = isHealthy && !objPSTDaoService.IsPSTSwitchOff(objPSTData.machineCode);

                }

            }
            return isHealthy;

        }

        public void AsynchReadListenerForPST(object sender, OPCDA.NET.RefreshEventArguments arg)
        {
            throw new NotImplementedException();
        }


        public bool UpdateMachineTagValueToDBFromListener(string machineCode, string machineTag, object dataValue)
        {
            throw new NotImplementedException();
        }

        public void GetDataTypeAndFieldOfTag(string opcTag, out int dataType, out string tableField, out bool isRem)
        {
            throw new NotImplementedException();
        }


        public bool IsPSTBlockedInDB(string machineName)
        {
            if (objPSTDaoService == null) objPSTDaoService = new PSTDaoImp();
            return objPSTDaoService.IsPSTBlockedInDB(machineName);

        }

        public bool UpdateMachineBlockStatus(string machine_code, bool blockStatus)
        {
            if (objPSTDaoService == null) objPSTDaoService = new PSTDaoImp();
            return objPSTDaoService.UpdateMachineBlockStatus(machine_code, blockStatus);
        }

        public bool IsPSTDisabled(string machineName)
        {
            if (objPSTDaoService == null) objPSTDaoService = new PSTDaoImp();
            return objPSTDaoService.IsPSTDisabled(machineName);
        }

        public bool IsPSTSwitchOff(string machineName)
        {
            if (objPSTDaoService == null) objPSTDaoService = new PSTDaoImp();
            return objPSTDaoService.IsPSTSwitchOff(machineName);
        }
        public bool ProcessPST100Request(bool isStore)
        {
           // Logger.WriteLogger(GlobalValues.PMS_LOG, "Entered ProcessPVLRequest : PVL=" + objPVLData.machineCode + ", isStore=" + objPVLData.isStore);
            int pathId = 0;
            if (objPSTDaoService == null) objPSTDaoService = new PSTDaoImp();

            QueueData objQueueData = new QueueData();
            if (objParkingControllerService == null) objParkingControllerService = new ParkingControllerImp();
            pathId = objPSTDaoService.GetPST100PathId(isStore);
            bool processStatus = false;
            if (pathId != 0)
            {

                objQueueData.pathPkId =pathId;
                objParkingControllerService.ExcecuteCommandsForPMS(objQueueData);
                objPSTDaoService.UpdateAfterPST100Task(pathId);
                processStatus = true;
            }
            //Logger.WriteLogger(GlobalValues.PMS_LOG, "Exitting ProcessPVLRequest : PVL=" + objPVLData.machineCode + ", isStore=" + objPVLData.isStore
              //  + ", processStatus=" + processStatus + ", pathId=" + pathId);
            return processStatus;

        }
        
        public bool ShowTrigger(Model.PSTData objPSTData)
        {
            if (objPSTData == null)
                return false;
            if (objErrorDaoService == null)
                objErrorDaoService = new ErrorDaoImp();
            if (objErrorControllerService == null)
                objErrorControllerService = new ErrorControllerImp();

            int error = objErrorControllerService.GetErrorCode(objPSTData.machineChannel, objPSTData.machineCode, OpcTags.PST_L2_ErrCode);
            if (error != 0)
                return false;
            TriggerData objTriggerData = new TriggerData();
            objTriggerData.MachineCode = objPSTData.machineCode;
            objTriggerData.category = TriggerData.triggerCategory.ERROR;
            objTriggerData.ErrorCode = error.ToString();
            objTriggerData.TriggerEnabled = true;
            objErrorDaoService.UpdateTriggerActiveStatus(objTriggerData);
            return true;
        }
    }
}
