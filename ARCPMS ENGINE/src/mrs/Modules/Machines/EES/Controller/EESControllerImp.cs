﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OPC;
using ARCPMS_ENGINE.src.mrs.OPCConnection.OPCConnectionImp;
using ARCPMS_ENGINE.src.mrs.Global;
using ARCPMS_ENGINE.src.mrs.OPCOperations.OPCOperationsImp;
using ARCPMS_ENGINE.src.mrs.OPCOperations;
using System.Threading;
using ARCPMS_ENGINE.src.mrs.Modules.Machines.EES.Model;
using ARCPMS_ENGINE.src.mrs.Modules.Machines.EES.DB;
using ARCPMS_ENGINE.src.mrs.Config;
using System.Threading.Tasks;
using System.IO;
using ARCPMS_ENGINE.src.mrs.Manager.QueueManager.Controller;
using System.Net;
using System.Drawing;
using ARCPMS_ENGINE.src.mrs.Utility;
using ARCPMS_ENGINE.src.mrs.Manager.ErrorManager.DB;
using ARCPMS_ENGINE.src.mrs.Manager.ErrorManager.Controller;
using ARCPMS_ENGINE.src.mrs.Manager.ErrorManager.Model;

namespace ARCPMS_ENGINE.src.mrs.Modules.Machines.EES.Controller
{
    class EESControllerImp : CommonServicesForMachines, EESControllerService
    {

        OPCDA.NET.RefreshGroup uGrp;
        int DAUpdateRate = 1;
        Thread updateDataFromOpcListener = null;
        EESDaoService objEESDaoService = null;
        QueueControllerService objQueueControllerService = null;
        UtilityClass objUtilityClass = null;
        private ErrorDaoService objErrorDaoService;
        private ErrorControllerService objErrorControllerService;
        enum EES_STATUS
        {
            EES_READY_STATUS=103,
            EES_CAR_ALIGNED_STATUS = 106,
            EES_READY_TO_GET_STATUS = 108,
            EES_READY_FOR_ENTRY = 100,
            EES_READY_FOR_PALLET = 110
        };


        public List<Model.EESData> GetEESListInRange(int minRange, int maxRange)
        {
            if (objEESDaoService==null)  objEESDaoService = new EESDaoImp();
            return objEESDaoService.GetEESListInRange(minRange, maxRange);
        }
        public int GetEESMode(EESData objEESData)
        {
            Int32 eesMode;
            using (OpcOperationsService opcd = new OpcOperationsImp(OpcConnection.GetOPCServerConnection()))
            {
                eesMode = opcd.ReadTag<Int32>(objEESData.machineChannel, objEESData.machineCode, OpcTags.EES_Mode);
            }
            return eesMode;
        }
        public bool ChangeMode(EESData objEESData)
        {
            bool success = false;

            if (GetEESMode(objEESData) != objEESData.eesMode)
            {
                using (OpcOperationsService opcd = new OpcOperationsImp(OpcConnection.GetOPCServerConnection()))
                {
                    //success = opcd.WriteTag<bool>(objEESData.machineChannel, objEESData.machineCode, OpcTags.EES_Manual_Mode, true);
                    success = opcd.WriteTag<int>(objEESData.machineChannel, objEESData.machineCode, OpcTags.EES_Mode, objEESData.eesMode);
                   //success = success && opcd.WriteTag<bool>(objEESData.machineChannel, objEESData.machineCode, OpcTags.EES_Auto_Mode, true);
                }
            }
            else
                success = true;
           
            return success;
        }

        public bool IsEESReadyForChangeMode(EESData objEESData)
        {
            bool isEESReady = false;
            objEESDaoService = new EESDaoImp();

            isEESReady = CheckEESHealthy(objEESData);
          
            isEESReady = isEESReady && !objEESDaoService.IsEESBlockedInDBForParking(objEESData.machineCode);
            isEESReady = isEESReady && !objEESDaoService.IsEESBlockedInDBForPMS(objEESData.machineCode);
            using (OpcOperationsService opcd = new OpcOperationsImp(OpcConnection.GetOPCServerConnection()))
            {
                isEESReady = isEESReady && opcd.ReadTag<bool>(objEESData.machineChannel, objEESData.machineCode, OpcTags.EES_L2_Change_Mode_OK);
            

            //    isEESReady = isEESReady && opcd.ReadTag<bool>(objEESData.machineChannel, objEESData.machineCode, OpcTags.EES_L2_Change_Mode_OK);
            //    //isEESReady = isEESReady && !opcd.ReadTag<bool>(objEESData.machineChannel, objEESData.machineCode, OpcTags.EES_Inner_Door_Close_Con);
            //    //isEESReady = isEESReady && !opcd.ReadTag<bool>(objEESData.machineChannel, objEESData.machineCode, OpcTags.EES_Inner_Door_Open_Con);
            //    //isEESReady = isEESReady && !opcd.ReadTag<bool>(objEESData.machineChannel, objEESData.machineCode, OpcTags.EES_Outer_Door_Close_Con);
            //    //isEESReady = isEESReady && !opcd.ReadTag<bool>(objEESData.machineChannel, objEESData.machineCode, OpcTags.EES_Outer_Door_Open_Con);
            //    //isEESReady = isEESReady && !opcd.ReadTag<bool>(objEESData.machineChannel, objEESData.machineCode, OpcTags.EES_LOCKED_BY_PS);
            //    //isEESReady = isEESReady && !opcd.ReadTag<bool>(objEESData.machineChannel, objEESData.machineCode, OpcTags.EES_LOCKED_BY_REM);

            }
           
            return isEESReady;
        }

        public bool IsEESReadyForParkingChangeMode(EESData objEESData)
        {
            bool isEESReady = false;
            objEESDaoService = new EESDaoImp();

            isEESReady = CheckEESHealthy(objEESData);

            //isEESReady = isEESReady && !objEESDaoService.IsEESBlockedInDBForParking(objEESData.machineCode);
            isEESReady = isEESReady && !objEESDaoService.IsEESBlockedInDBForPMS(objEESData.machineCode);
            using (OpcOperationsService opcd = new OpcOperationsImp(OpcConnection.GetOPCServerConnection()))
            {
                isEESReady = isEESReady && opcd.ReadTag<bool>(objEESData.machineChannel, objEESData.machineCode, OpcTags.EES_L2_Change_Mode_OK);
               

                //    isEESReady = isEESReady && opcd.ReadTag<bool>(objEESData.machineChannel, objEESData.machineCode, OpcTags.EES_L2_Change_Mode_OK);
                //    //isEESReady = isEESReady && !opcd.ReadTag<bool>(objEESData.machineChannel, objEESData.machineCode, OpcTags.EES_Inner_Door_Close_Con);
                //    //isEESReady = isEESReady && !opcd.ReadTag<bool>(objEESData.machineChannel, objEESData.machineCode, OpcTags.EES_Inner_Door_Open_Con);
                //    //isEESReady = isEESReady && !opcd.ReadTag<bool>(objEESData.machineChannel, objEESData.machineCode, OpcTags.EES_Outer_Door_Close_Con);
                //    //isEESReady = isEESReady && !opcd.ReadTag<bool>(objEESData.machineChannel, objEESData.machineCode, OpcTags.EES_Outer_Door_Open_Con);
                //    //isEESReady = isEESReady && !opcd.ReadTag<bool>(objEESData.machineChannel, objEESData.machineCode, OpcTags.EES_LOCKED_BY_PS);
                //    //isEESReady = isEESReady && !opcd.ReadTag<bool>(objEESData.machineChannel, objEESData.machineCode, OpcTags.EES_LOCKED_BY_REM);

            }

            return isEESReady;
        }
        //using when changing mode back to entry in normal and morning mode
        public bool IsEESReadyForParkingChangeModeBack(EESData objEESData)
        {
            bool isEESReady = false;
            objEESDaoService = new EESDaoImp();

            isEESReady = CheckEESHealthy(objEESData);

            //isEESReady = isEESReady && !objEESDaoService.IsEESBlockedInDBForParking(objEESData.machineCode);
            //isEESReady = isEESReady && !objEESDaoService.IsEESBlockedInDBForPMS(objEESData.machineCode);
            using (OpcOperationsService opcd = new OpcOperationsImp(OpcConnection.GetOPCServerConnection()))
            {
                isEESReady = isEESReady && opcd.ReadTag<bool>(objEESData.machineChannel, objEESData.machineCode, OpcTags.EES_L2_Change_Mode_OK);
                isEESReady = isEESReady && !opcd.ReadTag<bool>(objEESData.machineChannel, objEESData.machineCode, OpcTags.EES_OutDoor_NotClosed_LS);
                
               
                //    isEESReady = isEESReady && opcd.ReadTag<bool>(objEESData.machineChannel, objEESData.machineCode, OpcTags.EES_L2_Change_Mode_OK);
                //    //isEESReady = isEESReady && !opcd.ReadTag<bool>(objEESData.machineChannel, objEESData.machineCode, OpcTags.EES_Inner_Door_Close_Con);
                //    //isEESReady = isEESReady && !opcd.ReadTag<bool>(objEESData.machineChannel, objEESData.machineCode, OpcTags.EES_Inner_Door_Open_Con);
                //    //isEESReady = isEESReady && !opcd.ReadTag<bool>(objEESData.machineChannel, objEESData.machineCode, OpcTags.EES_Outer_Door_Close_Con);
                //    //isEESReady = isEESReady && !opcd.ReadTag<bool>(objEESData.machineChannel, objEESData.machineCode, OpcTags.EES_Outer_Door_Open_Con);
                //    //isEESReady = isEESReady && !opcd.ReadTag<bool>(objEESData.machineChannel, objEESData.machineCode, OpcTags.EES_LOCKED_BY_PS);
                //    //isEESReady = isEESReady && !opcd.ReadTag<bool>(objEESData.machineChannel, objEESData.machineCode, OpcTags.EES_LOCKED_BY_REM);

            }

            return isEESReady;
        }
        public bool TakePhoto(Model.EESData objEESData)
        {
            Logger.WriteLogger(GlobalValues.PARKING_LOG, "Queue Id:" + objEESData.queueId + ":Entered TakePhoto");
            if (objEESDaoService == null)
                objEESDaoService = new EESDaoImp();
            if (objUtilityClass == null)
                objUtilityClass = new UtilityClass();
            
            try
            {
                //string northimg =null;
                //string southimg = null;
                string imagePath = GlobalValues.IMG_DIR;
                string cameraConfigXmlName = "eft_config\\CameraNodes.xml";
                string cuurDate = System.DateTime.Now.Date.Date.Day + "_" + System.DateTime.Now.Date.Date.Month + "_" + System.DateTime.Now.Date.Date.Year;
                string northimg = objEESData.queueId + "_" + objEESData.customerPkId + "_" + cuurDate + "_" + "north.jpg";
                string southimg = objEESData.queueId + "_" + objEESData.customerPkId + "_" + cuurDate + "_" + "south.jpg";
                string northFullPath = null;
                string southFullPath = null;
                if (objEESData.isEntry)
                {
                    northFullPath = imagePath + "\\" + northimg;
                    southFullPath = imagePath + "\\" + southimg;
                }
                else
                {
                    northFullPath = imagePath + "\\" + northimg;
                    southFullPath = imagePath + "\\" + southimg;
                }

                string northImgOPCMachine = objEESData.eesName + "_North";
                string southImgOPCMachine = objEESData.eesName + "_South";

                // Define a delegate that prints and returns the system tick count
                Func<object, bool> north = (object obj) =>
                {

                  //  using (OpcOperationsService opcd = new OpcOperationsImp(OpcConnection.GetCamOPCServerConnection()))
                 //   {

                        try
                        {
                            
                          //  opcd.WriteTagToCamOpc<bool>(northImgOPCMachine + "." + OpcTags.EES_Cam_GetCmd, true);
                          //  northimg = opcd.ReadTagFromCamOpc<string>(northImgOPCMachine + "." + OpcTags.EES_Cam_ImgPath);

                            if (BasicConfig.GetXmlAttributeValueOfTag(cameraConfigXmlName, "Node", "Name", northImgOPCMachine, "Active").Equals("true"))
                                objUtilityClass.CaptureImage(BasicConfig.GetXmlAttributeValueOfTag(cameraConfigXmlName, "Node", "Name", northImgOPCMachine, "IP"), northFullPath);
                            //if (!string.IsNullOrEmpty(opcd.GetCamOpcError("EES" + eesNumber + "_North.Error")))
                            //break;


                        }
                        finally
                        {
                            //opcd.WriteTagToCamOpc<bool>(northImgOPCMachine + "." + OpcTags.EES_Cam_GetCmd, true);
                        }

                   // }
                    return true;
                };

                // Define a delegate that prints and returns the system tick count
                Func<object, bool> south = (object obj) =>
                {
                    //using (OpcOperationsService opcd = new OpcOperationsImp(OpcConnection.GetCamOPCServerConnection()))
                    //{

                        try
                        {
                            
                            //opcd.WriteTagToCamOpc<bool>(southImgOPCMachine + "." + OpcTags.EES_Cam_GetCmd, true);
                            //southimg = opcd.ReadTagFromCamOpc<string>(southImgOPCMachine + "." + OpcTags.EES_Cam_ImgPath);
                            if (BasicConfig.GetXmlAttributeValueOfTag(cameraConfigXmlName, "Node", "Name", southImgOPCMachine, "Active").Equals("true"))
                                objUtilityClass.CaptureImage(BasicConfig.GetXmlAttributeValueOfTag(cameraConfigXmlName, "Node", "Name", southImgOPCMachine, "IP"), southFullPath);
                            //if (!string.IsNullOrEmpty(opcd.GetCamOpcError("EES" + eesNumber + "_North.Error")))
                            //break;


                        }
                        finally
                        {
                            //opcd.WriteTagToCamOpc<bool>(southImgOPCMachine + "." + OpcTags.EES_Cam_GetCmd, false);
                        }

                   // }
                    return true;
                };

                Task<bool>[] tasks = new Task<bool>[2];
                tasks[0] = Task<bool>.Factory.StartNew(north, 1);
                tasks[1] = Task<bool>.Factory.StartNew(south, 1);


                // Wait for all the tasks to finish.
               // Task.WaitAll(tasks);

                if (!string.IsNullOrEmpty(northimg))
                    northimg = new FileInfo(northimg).Name;

                if (!string.IsNullOrEmpty(southimg))
                    southimg = new FileInfo(southimg).Name;


                objEESDaoService.UpdatePhotoPathToCustomerTable(objEESData, southimg, northimg);

            }
            catch (Exception errMsg)
            {
                Logger.WriteLogger(GlobalValues.PARKING_LOG, "Queue Id:" + objEESData.queueId + ":Error in TakePhoto: " + errMsg.Message);
            }
            finally
            {

            }
            Logger.WriteLogger(GlobalValues.PARKING_LOG, "Queue Id:" + objEESData.queueId + ":Exitting  TakePhoto");
            return true;
        }

        public int CheckError(Model.EESData objEESData)
        {
            throw new NotImplementedException();
        }

        public bool CheckEESClear(Model.EESData objEESData)
        {
            throw new NotImplementedException();
        }
        public bool UpdateMachineBlockStatus(string machine_code, bool blockStatus)
        {
            objEESDaoService = new EESDaoImp();
            return objEESDaoService.UpdateMachineBlockStatus(machine_code,blockStatus);

        }
        public bool UpdateMachineBlockStatusForPMS(string machine_code, bool blockStatus)
        {
            objEESDaoService = new EESDaoImp();
            return objEESDaoService.UpdateMachineBlockStatusForPMS(machine_code, blockStatus);
        }

        public override bool UpdateMachineValues()
        {
            objEESDaoService = new EESDaoImp();
            List<EESData> eesList;
            bool result;
            int dataValue;
            bool dataValueInBool;



            try
            {
                eesList = objEESDaoService.GetEESList();
                using (OpcOperationsService opcd = new OpcOperationsImp(OpcConnection.GetOPCServerConnection()))
                {
                    foreach (EESData objEESData in eesList)
                    {

                        if (opcd.IsMachineHealthy(objEESData.machineChannel + "." + objEESData.machineCode + "." + OpcTags.EES_Auto_Mode) == true)
                        {
                            //dataValueInBool = false;
                            //dataValueInBool = opcd.ReadTag<bool>(objEESData.machineChannel, objEESData.machineCode, OpcTags.EES_Auto_Mode);
                           // UpdateEESBoolData(objEESData.machineCode, OpcTags.EES_Auto_Mode, dataValueInBool);

                            dataValueInBool = false;
                            dataValueInBool = opcd.ReadTag<bool>(objEESData.machineChannel, objEESData.machineCode, OpcTags.EES_Auto_Ready);
                            UpdateEESBoolData(objEESData.machineCode, OpcTags.EES_Auto_Ready, dataValueInBool);

                            dataValueInBool = false;
                            dataValueInBool = opcd.ReadTag<bool>(objEESData.machineChannel, objEESData.machineCode, OpcTags.EES_Car_Ready_At_Entry);
                            UpdateEESBoolData(objEESData.machineCode, OpcTags.EES_Car_Ready_At_Entry, dataValueInBool);

                            dataValueInBool = false;
                            dataValueInBool = opcd.ReadTag<bool>(objEESData.machineChannel, objEESData.machineCode, OpcTags.EES_Car_Ready_At_Exit);
                            UpdateEESBoolData(objEESData.machineCode, OpcTags.EES_Car_Ready_At_Exit, dataValueInBool);

                            dataValueInBool = false;
                            dataValueInBool = GetCarAtEESStatus(objEESData.machineChannel, objEESData.machineCode);
                            UpdateEESBoolData(objEESData.machineCode, OpcTags.EES_Car_At_EES, dataValueInBool);



                        }

                    }
                }
                result = true;
            }
            catch (Exception errMsg)
            {
                result = false;
                Console.WriteLine(errMsg.Message);
            }
            return result;
        }

        public bool GetCarAtEESStatus(string eesChannel, string eesCode)
        {
            bool dataValueInBool = false;
            using (OpcOperationsService opcd = new OpcOperationsImp(OpcConnection.GetOPCServerConnection()))
            {
                dataValueInBool = opcd.ReadTag<bool>(eesChannel, eesCode, OpcTags.EES_Car_At_EES);
                dataValueInBool = dataValueInBool || opcd.ReadTag<bool>(eesChannel, eesCode, OpcTags.EES_OutDoor_NotClosed_LS);
            }
            return dataValueInBool;
        }
        public void AsynchReadListenerForEES(object sender, OPCDA.NET.RefreshEventArguments arg)
        {
            OPCDA.NET.OPCItemState res = arg.items[0].OpcIRslt;

            try
            {
                if (arg.Reason == OPCDA.NET.RefreshEventReason.DataChanged)
                {            // data changes
                    if (HRESULTS.Succeeded(res.Error))
                    {

                        OPCDA.NET.ItemDef opcItemDef = (OPCDA.NET.ItemDef)arg.items.GetValue(0);

                        string[] iterateItemName = opcItemDef.OpcIDef.ItemID.Split(new Char[] { '.' });
                        string channel = "";
                        string machineName = "";
                        string tag_Name = "";
                        if (iterateItemName.Length == 3)
                        {
                            channel = iterateItemName[0].ToString();
                            machineName = iterateItemName[1].ToString();
                            tag_Name = iterateItemName[2].ToString();

                            updateDataFromOpcListener = new Thread(delegate()
                            {
                                if (tag_Name.Equals(OpcTags.EES_Car_At_EES) || tag_Name.Equals(OpcTags.EES_OutDoor_NotClosed_LS))
                                {
                                    res.DataValue=GetCarAtEESStatus(channel, machineName);
                                }
                                    UpdateMachineTagValueToDBFromListener(machineName, tag_Name, res.DataValue);
                            });
                            updateDataFromOpcListener.IsBackground = true;
                            updateDataFromOpcListener.Start();

                        }

                    }
                }
            }
            catch (Exception errMsg)
            {
            }
        }
        public override bool AsynchReadSettings()
        {
            // add a periodic data callback group and add one item to the group
            OPCDA.NET.RefreshEventHandler dch = new OPCDA.NET.RefreshEventHandler(AsynchReadListenerForEES);
            uGrp = new OPCDA.NET.RefreshGroup(OpcConnection.GetOPCServerConnection(), DAUpdateRate, dch);

            int rtc = 0;

            EESDaoService objEESDaoService = new EESDaoImp();
            List<EESData> eesList = objEESDaoService.GetEESList();


            try
            {

                foreach (EESData objEESData in eesList)
                {
                   // rtc = uGrp.Add(objEESData.machineChannel + "." + objEESData.machineCode + "." + OpcTags.EES_Auto_Mode);
                    rtc = uGrp.Add(objEESData.machineChannel + "." + objEESData.machineCode + "." + OpcTags.EES_Auto_Ready);
                    rtc = uGrp.Add(objEESData.machineChannel + "." + objEESData.machineCode + "." + OpcTags.EES_Mode);


                    rtc = uGrp.Add(objEESData.machineChannel + "." + objEESData.machineCode + "." + OpcTags.EES_Car_Ready_At_Entry);
                    rtc = uGrp.Add(objEESData.machineChannel + "." + objEESData.machineCode + "." + OpcTags.EES_Car_Ready_At_Exit);

                    rtc = uGrp.Add(objEESData.machineChannel + "." + objEESData.machineCode + "." + OpcTags.EES_Car_At_EES);
                    rtc = uGrp.Add(objEESData.machineChannel + "." + objEESData.machineCode + "." + OpcTags.EES_OutDoor_NotClosed_LS);

                    rtc = uGrp.Add(objEESData.machineChannel + "." + objEESData.machineCode + "." + OpcTags.EES_State_EES_HMI);
                    rtc = uGrp.Add(objEESData.machineChannel + "." + objEESData.machineCode + "." + OpcTags.EES_Vehicle_Detector);

                }
            }
            catch (Exception errMsg)
            {
                rtc = 0;
                Console.WriteLine(errMsg.Message);
            }
            finally
            {

            }

            return rtc == 0 ? false : true;
        }


        public override void GetDataTypeAndFieldOfTag(string opcTag, out int dataType, out string tableField, out bool isRem)
        {
            isRem = false;
            tableField = "";
            dataType = 0;//1:bool;2:number;3:string
           
            
            if (opcTag.Equals(OpcTags.EES_Auto_Mode))
            {
                tableField = "IS_AUTOMODE";
                dataType = 1;

            }
            else if (opcTag.Equals(OpcTags.EES_Auto_Ready))
            {
                tableField = "IS_AUTOMODE";
                dataType = 1;

            }
            else if (opcTag.Equals(OpcTags.EES_Car_Ready_At_Entry))
            {
                tableField = "CAR_READY_AT_ENTRY";
                dataType = 1;
            }
            else if (opcTag.Equals(OpcTags.EES_Car_Ready_At_Exit))
            {
                tableField = "CAR_READY_AT_EXIT";
                dataType = 1;
            }
            else if (opcTag.Equals(OpcTags.EES_Car_At_EES))
            {
                tableField = "CAR_AT_EES";
                dataType = 1;
            }
            else if (opcTag.Equals(OpcTags.EES_OutDoor_NotClosed_LS))
            {
                tableField = "CAR_AT_EES";
                dataType = 1;
            }
            else if (opcTag.Equals(OpcTags.EES_State_EES_HMI))
            {
                tableField = "null";
                dataType = 2;
            }
            else if (opcTag.Equals(OpcTags.EES_Vehicle_Detector))
            {
                tableField = "null";
                dataType = 1;
            }
        }


        public bool UpdateEESIntData(string machineCode, string opcTag, int dataValue)
        {
            int dataType = 0; //1:bool;2:number;3:string
            bool isRem = false;
            string field = "";
            bool boolDataValue;
            int intDataValue;
            string stringDataValue;


                GetDataTypeAndFieldOfTag(opcTag, out dataType, out field, out isRem);
           
                if (dataType == 2)
                {
                    
                    intDataValue = Convert.ToInt16(dataValue);
                    objEESDaoService.UpdateIntValueUsingMachineCode(machineCode, field, intDataValue);
                }
                else if (dataType == 1)
                {
                    boolDataValue = Convert.ToBoolean(dataValue);
                    objEESDaoService.UpdateBoolValueUsingMachineCode(machineCode, field, boolDataValue);
                }
                else if (dataType == 3)
                {
                    stringDataValue = Convert.ToString(dataValue);
                    objEESDaoService.UpdateStringValueUsingMachineCode(machineCode, field, stringDataValue);
                }
           
            return true;
        }

        public bool UpdateEESBoolData(string machineCode, string opcTag, bool dataValue)
        {
            int dataType = 0; //1:bool;2:number;3:string
            bool isRem = false;
            string field = "";
            bool boolDataValue;
            int intDataValue;
            string stringDataValue;


            GetDataTypeAndFieldOfTag(opcTag, out dataType, out field, out isRem);
           
                if (dataType == 1)
                {
                    boolDataValue = Convert.ToBoolean(dataValue);
                    objEESDaoService.UpdateBoolValueUsingMachineCode(machineCode, field, boolDataValue);
                }
                else if (dataType == 2)
                {
                    intDataValue = Convert.ToInt16(dataValue);
                    objEESDaoService.UpdateIntValueUsingMachineCode(machineCode, field, intDataValue);
                }
                else if (dataType == 3)
                {
                    stringDataValue = Convert.ToString(dataValue);
                    objEESDaoService.UpdateStringValueUsingMachineCode(machineCode, field, stringDataValue);
                }
           
            return true;
        }


        public override bool UpdateMachineTagValueToDBFromListener(string machineCode, string machineTag, object dataValue)
        {
            string field = "";
            bool boolDataValue;
            Int16 intDataValue;
            string stringDataValue;
            if (objEESDaoService == null)
                objEESDaoService = new EESDaoImp();
            int dataType = 0; //1:bool;2:number;3:string
            bool isRem = false;


            switch (machineTag)
            {
                case OpcTags.EES_State_EES_HMI:
                 
                    int dataValueInt = int.Parse(dataValue.ToString());
                    if (dataValueInt == (int)EES_STATUS.EES_READY_STATUS)
                    {
                        objEESDaoService.SetEESReadyTime(machineCode, System.DateTime.Now);
                    }
                    else if (dataValueInt == (int)EES_STATUS.EES_CAR_ALIGNED_STATUS)
                    {
                        objEESDaoService.SetCarAlignedTime(machineCode, System.DateTime.Now);
                    }
                    else if (dataValueInt == (int)EES_STATUS.EES_READY_TO_GET_STATUS)
                    {
                        objEESDaoService.SetReadyToGetTime(machineCode, System.DateTime.Now);
                    }
                    break;
                case OpcTags.EES_Vehicle_Detector:
                    if ((bool)dataValue)
                    {
                        objEESDaoService.SetCarOutsideTime(machineCode,System.DateTime.Now);
                    }
                    break;
                default:
                    GetDataTypeAndFieldOfTag(machineTag, out dataType, out field, out isRem);
                    if (dataType == 1)
                    {
                        boolDataValue = Convert.ToBoolean(dataValue);
                        objEESDaoService.UpdateBoolValueUsingMachineCode(machineCode, field, boolDataValue);
                    }
                    else if (dataType == 2)
                    {
                        intDataValue = Convert.ToInt16(dataValue);

                        objEESDaoService.UpdateIntValueUsingMachineCode(machineCode, field, intDataValue);
                    }
                    else if (dataType == 3)
                    {
                        stringDataValue = Convert.ToString(dataValue);
                        objEESDaoService.UpdateStringValueUsingMachineCode(machineCode, field, stringDataValue);
                    }
                    break;

            }
              
           
            return true;
        }
        public ARCPMS_ENGINE.src.mrs.Global.GlobalValues.CAR_TYPE GetCarType(EESData objEESData)
        {
            bool lowerSensor = false;
            bool midSensor = false;
            lowerSensor=GetLowerHeightSensorStatus(objEESData);
            midSensor=GetMidHeightSensorStatus(objEESData);
            ARCPMS_ENGINE.src.mrs.Global.GlobalValues.CAR_TYPE carType = ARCPMS_ENGINE.src.mrs.Global.GlobalValues.CAR_TYPE.high;
            if (lowerSensor && midSensor)
            {
                carType=ARCPMS_ENGINE.src.mrs.Global.GlobalValues.CAR_TYPE.low;
            }
            else if (!lowerSensor && midSensor)
            {
                carType = ARCPMS_ENGINE.src.mrs.Global.GlobalValues.CAR_TYPE.medium;
            }
            else if (!lowerSensor && !midSensor)
            {
                carType = ARCPMS_ENGINE.src.mrs.Global.GlobalValues.CAR_TYPE.high;
            }
            return carType;
        }
        public bool GetLowerHeightSensorStatus(EESData objEESData)
        {
            bool lowerSensor = false;
           
            try
            {
               
                
                using (OpcOperationsService opcd = new OpcOperationsImp(OpcConnection.GetOPCServerConnection()))
                {
                    //for low car is true, for high car its false.
                    lowerSensor = opcd.ReadTag<bool>(objEESData.machineChannel, objEESData.machineCode, OpcTags.EES_Lower_Height_Sensor_Blocked);
                }
            }
            finally
            {

            }

            return lowerSensor;
        }
      
        public bool GetMidHeightSensorStatus(EESData objEESData)
        {
            bool midSensor = false;

            try
            {


                using (OpcOperationsService opcd = new OpcOperationsImp(OpcConnection.GetOPCServerConnection()))
                {
                    //for low car is true, for high car its false.
                    midSensor = opcd.ReadTag<bool>(objEESData.machineChannel, objEESData.machineCode, OpcTags.EES_Mid_Height_Sensor_Blocked);
                }
            }
            finally
            {

            }

            return midSensor;
        }


        public bool checkCarAtEES(EESData objEESData)
        {
            bool carAtEES = false;
            using (OpcOperationsService opcd = new OpcOperationsImp(OpcConnection.GetOPCServerConnection()))
            {
                carAtEES=opcd.ReadTag<bool>(objEESData.machineChannel, objEESData.machineCode, OpcTags.EES_Car_At_EES);
            }
            return carAtEES;
        }

        public List<EESData> GetEESList()
        {
            if (objEESDaoService == null) objEESDaoService = new EESDaoImp();
            return objEESDaoService.GetEESList();
        }

        public EESData GetEESDetails(EESData objEESData)
        {
            if (objEESDaoService == null) objEESDaoService = new EESDaoImp();
            return objEESDaoService.GetEESDetails(objEESData);
        }
        public bool checkCarReadyAtEntry(EESData objEESData)
        {
            bool carReadyAtEntry = false;
            using (OpcOperationsService opcd = new OpcOperationsImp(OpcConnection.GetOPCServerConnection()))
            {
                carReadyAtEntry = opcd.ReadTag<bool>(objEESData.machineChannel, objEESData.machineCode, OpcTags.EES_Car_Ready_At_Entry);
            }
            return carReadyAtEntry;
        }
        public bool ExcecuteEESGetCar(EESData objEESData)
        {
            bool success = false;
            int printCounter = 0;
            if (objQueueControllerService == null) objQueueControllerService = new QueueControllerImp();
            using (OpcOperationsService opcd = new OpcOperationsImp(OpcConnection.GetOPCServerConnection()))
            {
                
                do
                {
                    success = opcd.WriteTag<bool>(objEESData.machineChannel, objEESData.machineCode, objEESData.command, true);
                    System.Threading.Thread.Sleep(1000);

                    printCounter += 1;
                    if (printCounter > 3)
                    {
                        //checking transaction deleted or not
                        objQueueControllerService.CancelIfRequested(objEESData.queueId);
                        Logger.WriteLogger(GlobalValues.PARKING_LOG, "Queue Id:" + objEESData.queueId + ":--Failed to excecute get car for EES" + objEESData.machineCode);
                        printCounter = 0;

                    }

                } while (!IsDoneExcecuteEESGetCar(objEESData));
            }
            return success;
        }
        public bool IsDoneExcecuteEESGetCar(EESData objEESData)
       {
            bool isDone = false;
            using (OpcOperationsService opcd = new OpcOperationsImp(OpcConnection.GetOPCServerConnection()))
            {
                isDone = opcd.ReadTag<bool>(objEESData.machineChannel, objEESData.machineCode, OpcTags.EES_InDoor_NotClosed_LS);
            }
            return isDone;
        }



        public bool ExcecutePaymentDone(EESData objEESData)
        {
            bool success = false;
            using (OpcOperationsService opcd = new OpcOperationsImp(OpcConnection.GetOPCServerConnection()))
            {
                success = opcd.WriteTag<bool>(objEESData.machineChannel, objEESData.machineCode, objEESData.command, true);
            }
            return success;
        }



        public bool CheckEESHealthy(EESData objEESData)
        {
            bool isHealthy = false;
            objEESDaoService = new EESDaoImp();

            using (OpcOperationsService opcd = new OpcOperationsImp(OpcConnection.GetOPCServerConnection()))
            {
                if (opcd.IsMachineHealthy(objEESData.machineChannel + "." + objEESData.machineCode + "." + OpcTags.EES_Auto_Ready))
                {
                    isHealthy = opcd.ReadTag<bool>(objEESData.machineChannel, objEESData.machineCode, OpcTags.EES_Auto_Ready);
                 
                    isHealthy = isHealthy && !objEESDaoService.IsEESDisabled(objEESData.machineCode);
                   // isHealthy = isHealthy && !objEESDaoService.IsEESSwitchOff(objEESData.machineCode);
                    
                    
                }

            }
            return isHealthy;
        }

        public bool IsEESEntryInCurrentModeInDB(string machineCode)
        {
            objEESDaoService = new EESDaoImp();
            return objEESDaoService.IsEESEntryInCurrentModeInDB(machineCode);
        }

        public bool IsPalletPresentOnEES(Model.EESData objEESData)
        {
            bool isPresent = false;

            using (OpcOperationsService opcd = new OpcOperationsImp(OpcConnection.GetOPCServerConnection()))
            {
                if (opcd.IsMachineHealthy(objEESData.machineChannel + "." + objEESData.machineCode + "." + OpcTags.EES_Auto_Ready))
                {
                    isPresent = opcd.ReadTag<bool>(objEESData.machineChannel, objEESData.machineCode, OpcTags.EES_Pallet_Present_Prox_NE);
                    isPresent = isPresent || opcd.ReadTag<bool>(objEESData.machineChannel, objEESData.machineCode, OpcTags.EES_Pallet_Present_Prox_SW);


                }

            }
            return isPresent;
        }

        public bool IsEESEntryInOPC(EESData objEESData)
        {

            bool isEntry = false;

            using (OpcOperationsService opcd = new OpcOperationsImp(OpcConnection.GetOPCServerConnection()))
            {
                if (opcd.IsMachineHealthy(objEESData.machineChannel + "." + objEESData.machineCode + "." + OpcTags.EES_Auto_Ready))
                {
                    isEntry = opcd.ReadTag<Int32>(objEESData.machineChannel, objEESData.machineCode, OpcTags.EES_Mode)==GlobalValues.EESEntry;
                   
                }

            }
            return isEntry;
        }
        public bool IsEESBlockedInDBForParking(string machineName)
        {
            if (objEESDaoService==null) objEESDaoService = new EESDaoImp();
            return objEESDaoService.IsEESBlockedInDBForParking(machineName);
        }
        public bool IsEESBlockedInDBForPMS(string machineName)
        {
            if (objEESDaoService == null) objEESDaoService = new EESDaoImp();
            return objEESDaoService.IsEESBlockedInDBForPMS(machineName);
        }

        public bool GetCountOfEntryAndExitRequests(List<Model.EESData> eesList,out int entryRequestCount,out int exitRequestCount)
        {
            entryRequestCount = 0;
            exitRequestCount = 0;
            bool isEESEntry = false;
            bool hasPalletOnEES = false;
            bool hasCarAtEES = false;
            foreach (EESData objEESData in eesList)
            {
                hasCarAtEES = checkCarAtEES(objEESData);
                if (!CheckEESHealthy(objEESData) || hasCarAtEES) continue;
                isEESEntry = IsEESEntryInOPC(objEESData);
                hasPalletOnEES = IsPalletPresentOnEES(objEESData);

                if (isEESEntry && !hasPalletOnEES)
                    entryRequestCount++;
                else if (!isEESEntry && hasPalletOnEES)
                    exitRequestCount++;
            }
            return true;
        }




        public bool IsEESReadyForPS(EESData objEESData)
        {
            bool isReady = false;

            using (OpcOperationsService opcd = new OpcOperationsImp(OpcConnection.GetOPCServerConnection()))
            {
                if (opcd.IsMachineHealthy(objEESData.machineChannel + "." + objEESData.machineCode + "." + OpcTags.EES_Auto_Ready))
                {
                    isReady = opcd.ReadTag<bool>(objEESData.machineChannel, objEESData.machineCode, OpcTags.EES_Ready_for_PS_Lock) ;

                }

            }
            return isReady;
        }

        public bool IsEESReadyForRem(EESData objEESData)
        {
            bool isReady = false;
             if (objEESDaoService == null) objEESDaoService = new EESDaoImp();

            using (OpcOperationsService opcd = new OpcOperationsImp(OpcConnection.GetOPCServerConnection()))
            {
                if (opcd.IsMachineHealthy(objEESData.machineChannel + "." + objEESData.machineCode + "." + OpcTags.EES_Auto_Ready))
                {
                    isReady = opcd.ReadTag<bool>(objEESData.machineChannel, objEESData.machineCode, OpcTags.EES_Ready_for_REM_Lock);
                    isReady = isReady && !opcd.ReadTag<bool>(objEESData.machineChannel, objEESData.machineCode, OpcTags.EES_Car_At_EES);
                    isReady = isReady && objEESDaoService.IsPSNotGettingFromEES(objEESData.machineCode);
                    isReady = isReady && !objEESDaoService.IsEESDisabled(objEESData.machineCode);
                }

            }
            return isReady;
        }


        public bool ConfirmEESReadyForREMLock(string machineCode)
        {
            bool isReady = false;
            EESData objEESData = new EESData();
            objEESData.machineCode = machineCode;
            if (objEESDaoService == null) objEESDaoService = new EESDaoImp();
            objEESData= objEESDaoService.GetEESDetails(objEESData);
            isReady=IsEESReadyForRem(objEESData);
            return isReady;
        }
        public int GetEESState(EESData objEESData)
        {

            int eesState = 0;
            using (OpcOperationsService opcd = new OpcOperationsImp(OpcConnection.GetOPCServerConnection()))
            {
                if (opcd.IsMachineHealthy(objEESData.machineChannel + "." + objEESData.machineCode + "." + OpcTags.EES_Auto_Ready))
                {
                    eesState = opcd.ReadTag<int>(objEESData.machineChannel, objEESData.machineCode, OpcTags.EES_State_EES_HMI);
                }
            }
            return  eesState;
        }
        public bool IsEESDisabled(string machineName)
        {
            if (objEESDaoService == null) objEESDaoService = new EESDaoImp();
            return objEESDaoService.IsEESDisabled(machineName);

        }


        public void CheckGateEntryStatus(EESData objEESData)
        {
            if (objEESDaoService == null)
                objEESDaoService = new EESDaoImp();

            try
            {
                if (objEESDaoService.IsEESDisabled(objEESData.machineCode))
                    return;
                       
                GlobalValues.GATE_ENTRY_STATUS status = GetGateEntryStatus(objEESData);
                objEESDaoService.UpdateGateEntryStatus(objEESData.machineCode, (short)status);
            }
            catch (Exception errMsg)
            {
                Console.WriteLine(errMsg.Message);
            }
            finally
            {

            }
         
        }
        public GlobalValues.GATE_ENTRY_STATUS GetGateEntryStatus(EESData objEESData)
        {
            GlobalValues.GATE_ENTRY_STATUS gateStatus = GlobalValues.GATE_ENTRY_STATUS.NOT_READY;
            if (objEESDaoService == null)
                objEESDaoService = new EESDaoImp();
            if (!GetVehicleDetectorStatus(objEESData))
            {
                if (objEESDaoService.IsPSPuttingEES(objEESData.machineCode))
                    gateStatus = GlobalValues.GATE_ENTRY_STATUS.FEEDING;
                else if (objEESDaoService.IsLCMGettingEES(objEESData.machineCode))
                    gateStatus = GlobalValues.GATE_ENTRY_STATUS.CLEARING;
                else {
                    int state=GetEESState(objEESData);
                    if (state == (int)EES_STATUS.EES_READY_FOR_PALLET)
                         gateStatus = GlobalValues.GATE_ENTRY_STATUS.CLEARING;
                    else if (state == (int)EES_STATUS.EES_READY_FOR_ENTRY)
                        gateStatus = GlobalValues.GATE_ENTRY_STATUS.READY;
                }
                
            }

            return gateStatus;

        }
        public bool IsEESReadyForCarEntry(EESData objEESData)
        {

            bool isReady = false;
            int ees_state = 0;

            ees_state = GetEESState(objEESData);
            isReady = (ees_state == 100);
            return isReady;
        }
        public bool GetVehicleDetectorStatus(Model.EESData objEESData)
        {
            bool status = false;
            using (OpcOperationsService opcd = new OpcOperationsImp(OpcConnection.GetOPCServerConnection()))
            {
                status = opcd.ReadTag<bool>(objEESData.machineChannel , objEESData.machineCode ,OpcTags.EES_Vehicle_Detector);
            }

            return status;

        }
        public bool ShowTrigger(Model.EESData objEESData)
        {
            if (objEESData == null)
                return false;
            bool needToShowTrigger = false;
            if (objErrorDaoService == null)
                objErrorDaoService = new ErrorDaoImp();
            if (objErrorControllerService == null)
                objErrorControllerService = new ErrorControllerImp();

            int error = objErrorControllerService.GetErrorCode(objEESData.machineChannel, objEESData.machineCode, OpcTags.EES_L2_ErrCode);
            if (error != 0)
            {
                needToShowTrigger=true;
                 objErrorDaoService.UpdateTriggerActiveStatus(GetTriggerData(TriggerData.triggerCategory.ERROR, error.ToString(), objEESData.machineCode));
            }
            //else if (!CheckEESHealthy(objEESData))
            //{
            //    needToShowTrigger = objErrorDaoService.UpdateTriggerActiveStatus(GetTriggerData(TriggerData.triggerCategory.TRIGGER, "", objEESData.machineCode));
            //}
            if (!needToShowTrigger)
                objErrorDaoService.RemoveTrigger(objEESData.machineCode);
            return needToShowTrigger;
        }
    }
}
