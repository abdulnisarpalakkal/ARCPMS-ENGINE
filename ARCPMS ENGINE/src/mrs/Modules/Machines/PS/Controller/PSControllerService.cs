using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OPCDA.NET;
using ARCPMS_ENGINE.src.mrs.Modules.Machines.PS.Model;
using ARCPMS_ENGINE.src.mrs.Modules.Machines.PST.Model;
using ARCPMS_ENGINE.src.mrs.Modules.Machines.EES.Model;

namespace ARCPMS_ENGINE.src.mrs.Modules.Machines.PS.Controller
{
    interface PSControllerService
    {
        List<Model.PSData> GetPSList();
        Model.PSData GetPSDetailsIncludeAisle(int aisle);
        bool PSMove(PSData objPSData);
        bool PSGetFromEES(PSData objPSData, EESData objEESData);
        bool PSGetFromPST(PSData objPSData, PSTData objPSTData);
        bool PSPutToEES(PSData objPSData, EESData objEESData);
        bool PSPutToPST(PSData objPSData, PSTData objPSTData);

        bool CheckPSCommandDone(PSData objPSData, PSTData objPSTData, EESData objEESData);
        bool ClearPathForPS(PSData objPSData);
        bool ClearNearestPS(PSData objPSData);
        bool CheckPSHealthy(PSData objPSData);

        bool UpdatePSIntData(string machineCode,string opcTag,int dataValue);
        bool UpdatePSBoolData(string machineCode, string opcTag, bool dataValue);
        void FindCommandTypeAndDoneTag(PSData objPSData,out int commandType,out string doneTag);
        bool DoTriggerAction(Model.PSData objPSData,PSTData objPSTData,EESData objEESData, int commandType);
       
        bool IsPSBlockedInDB(string machineName);
        bool UpdateMachineBlockStatus(string machine_code, bool blockStatus);
        bool IsPSDisabled(string machineName);
        bool IsPSSwitchOff(string machineName);

        bool IsPalletPresentOnPS(PSData objPSData, out bool hasPSCommunication);
        int GetAisleOfPS(PSData objPSData);

       // bool AsynchReadSettingsForPS();
        void AsynchReadListenerForPS(object sender, RefreshEventArguments arg);
    }
}
