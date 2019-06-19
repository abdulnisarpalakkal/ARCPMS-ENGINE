using ARCPMS_ENGINE.src.mrs.Manager.ErrorManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ARCPMS_ENGINE.src.mrs.Modules.Machines
{
    abstract class CommonServicesForMachines
    {
        public abstract bool UpdateMachineValues();
        public abstract bool AsynchReadSettings();


        public abstract bool UpdateMachineTagValueToDBFromListener(string machineCode, string machineTag, Object dataValue);
        public abstract void GetDataTypeAndFieldOfTag(string opcTag, out int dataType, out string tableField, out bool isRem);
        //public abstract TriggerData GetTriggerData(Manager.ErrorManager.Model.TriggerData.triggerCategory triggerCategory, int error, string machineCode);
        public TriggerData GetTriggerData(TriggerData.triggerCategory triggerCategory, string error, string machineCode)
        {
            TriggerData objTriggerData = new TriggerData();
            objTriggerData.MachineCode = machineCode;
            objTriggerData.category = triggerCategory;
            objTriggerData.ErrorCode = error.ToString();
            objTriggerData.TriggerEnabled = true;
            return objTriggerData;
        }
    }
}
