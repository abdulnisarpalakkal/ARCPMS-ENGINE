using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ARCPMS_ENGINE.src.mrs.Global
{
    public static class OpcTags //For EFT
    {
        //For CM
        public  const string CM_ClearError                                     ="ClearError";
        public  const string CM_AT_NORTH                                       ="CM_AT_NORTH";
        public  const string CM_AT_SOUTH                                       ="CM_AT_SOUTH";
        public  const string CM_Locked                                         ="CM_Locked";
                
        public  const string CM_Position_for_L2                                = "CM_Position_for_L2";
        public  const string CM_EastCMOff                                      = "EastCMOff";
        public  const string CM_WestCMOff                                      = "WestCMOff";
                
                
        public  const string CM_L2_Block_CM                                    = "L2_Block_CM";
        public  const string CM_L2_CMD_DONE                                    = "L2_CMD_DONE";
        public  const string CM_L2_Destination_Row                             = "L2_Destination_Row";
        public  const string CM_L2_Error_Data_Register                         = "L2_Error_Data_Register";
                
        public  const string CM_L2_EStop                                       = "L2_EStop";
        public  const string CM_L2_Get_Cmd                                     = "L2_Get_Cmd";
        public  const string CM_L2_Max_Window_Limit                            = "L2_Max_Window_Limit";
        public  const string CM_L2_Min_Window_Limit                            = "L2_Min_Window_Limit";
                
        public  const string CM_L2_Move_Cmd                                    = "L2_Move_Cmd";
        public  const string CM_L2_Put_Cmd                                     = "L2_Put_Cmd";
        public  const string CM_Manual_Mode                                    = "Manual_Mode";
        public  const string CM_Moving                                         = "Moving";
                
        public  const string CM_N0_of_hours                                    = "N0_of_hours";
        public  const string CM_L2_Destination_Aisle                           = "L2_Destination_Aisle";
        public  const string CM_Auto_Mode                                      = "Auto_Mode";
        public  const string CM_Pallet_Present_on_REM                          = "Pallet_Present_on_REM";
        public const string  CM_L2_CM_IN_BUSY                                  = "L2_CM_IN_BUSY";
        public const string CM_REM_In_Home_Position                            = "REM_IN_HOME_LOCAL";

        public const string CM_Pallet_Present                                  = "Pallet_Present";
        public const string CM_Pallet_Not_Present                              = "Pallet_Not_Present";
        public const string CM_Progress_State                                  = "Progress_State";
             
        //For REM
        public const string REM_Pallet_Present                                 = "PP_Half_L2";

        //For U 
        public  const string UCM_L2_AUTO_READY                                  = "L2_AUTO_READY";
        //For L 
                
        public  const string LCM_Clear_Error_CM                                = "Clear_Error_CM";
        public  const string LCM_L2_ROT_FALSE_ALARM                            = "L2_ROT_FALSE_ALARM";
        public  const string LCM_CM_At_Aisle                                   = "CM_At_Aisle";
                
        public  const string LCM_L2_Command_Position_Done                      = "L2_Command_Position_Done";
        public  const string LCM_L2_TT_ROT                                     = "L2_TT_ROT";
        public  const string LCM_L2_ROTATE_DONE                                = "L2_ROTATE_DONE";
                
        public  const string LCM_L2_AUTO_READY                                 = "Auto_Ready";
        public  const string LCM_L2_CM_IN_ROTATION                             = "L2_CM_IN_ROTATION";
                
                
        //VLC   
         public const string VLC_At_Floor                                     = "At_Floor";
         public const string VLC_Auto_Mode                                    = "Auto_Mode";
         public const string VLC_Auto_Ready                                   = "Auto_Ready";
         public const string VLC_Axis_Error_Code                              = "Axis_Error_Code";
               
         public const string VLC_CP_Done                                      = "CP_Done";
         public const string VLC_CP_Get_Start                                 = "CP_Get_Start";
         public const string VLC_CP_Put_Start                                 = "CP_Put_Start";
         public const string VLC_CP_Start                                     = "CP_Start";
               
         public const string VLC_DestFloor                                    = "DestFloor";
         public const string VLC_Get_Put_Done                                 = "Get_Put_Done";
         public const string VLC_L2_ErrCode                                   = "L2_ErrCode";
         public const string VLC_Landed_Position_OK                           = "Landed_Position_OK";
               
         public const string VLC_Manual_Mode                                  = "Manual_Mode";
         public const string VLC_No_Current_Error                             = "No_Current_Error";
         public const string VLC_DECK_PalletPres                              = "PalletPresent";
               
        //EES  
         public const string EES_Status02                                     = "Status[02]";//rem locked
         public const string EES_Ready_for_REM_Lock                           = "Status[01]";
         public const string EES_Stat2PSs02                                   = "Stat2PSs[02]"; //ps locked
         public const string EES_Ready_for_PS_Lock                            = "Stat2PSs[01]";
               
         public const string EES_Payment_Is_Done                              = "Payment_Is_Done";
         public const string EES_Pallet_Present_Prox_SW                       = "Pallet_Present_Prox_SW";
         public const string EES_Pallet_Present_Prox_NE                       = "Pallet_Present_Prox_NE";
         public const string EES_Outer_Door_Open_Con                          = "Outer_Door_Open_Con";
               
         public const string EES_Outer_Door_Close_Con                         = "Outer_Door_Close_Con";
         public const string EES_Outer_Door_Block_Sensor                      = "Outer_Door_Block_Sensor";
         public const string EES_OutDoor_NotOpen_LS                           = "OutDoor_NotOpen_LS";
         public const string EES_OutDoor_NotClosed_LS                         = "OutDoor_NotClosed_LS";
               
         public const string EES_No_Current_Error                             = "No_Current_Error";
         public const string EES_Manual_Mode                                  = "Manual_Mode";
         public const string EES_L2_ErrCode                                   = "L2_ErrCode";
         public const string EES_Inner_Door_Open_Con                          = "Inner_Door_Open_Con";
               
         public const string EES_Inner_Door_Close_Con                         = "Inner_Door_Close_Con";
         public const string EES_InDoor_NotOpen_LS                            = "InDoor_NotOpen_LS";
         public const string EES_InDoor_NotClosed_LS                          = "InDoor_NotClosed_LS";
         public const string EES_Get_Car                                      = "Get_Car";
                
        public  const string EES_Sensors_Clear                                 = "EES_Sensors_Clear";
        public  const string EES_Mode                                          = "EES_Mode";
        public  const string EES_Car_Simulaton                                 = "Car_Simulaton";
        public  const string EES_Car_Ready_To_Get                              = "Car_Ready_To_Get";
                
        public  const string EES_Car_Ready_At_Exit                             = "Car_Ready_At_Exit";
        public  const string EES_Car_Ready_At_Entry                            = "Car_Ready_At_Entry";
        public  const string EES_Car_At_EES                                    = "Car_At_EES";
        public  const string EES_CAR_SENSE_ALARM                               = "CAR_SENSE_ALARM";
                
        public  const string EES_CAR_AT_EES_ALARM                              = "CAR_AT_EES_ALARM";
        public  const string EES_Auto_Ready                                    = "Auto_Ready";
        public  const string EES_Auto_Mode                                     = "Auto_Mode";
        public  const string EES_Lower_Height_Sensor_Blocked                   = "Lower_Height_Sensor_Blocked";
        public const string EES_Mid_Height_Sensor_Blocked                      = "Mid_Height_Sensor_Blocked";
        public const string EES_L2_Change_Mode_OK                              = "L2_MODE_CHANGE_READY";
        public const string EES_State_EES_HMI                                  = "State_EES_HMI";
        public const string EES_Vehicle_Detector                               = "Vehicle_Detector";
                
                
        //PVL   
        public  const string PVL_StatusPvL02                                   = "Status[02]"; //rem locked
        public  const string PVL_StatusPvl01                                   = "Status[01]";
        public  const string PVL_Request_Floor                                 = "Request_Floor";
        public  const string PVL_Put_PB_Done                                   = "Put_PB_Done";
                
        public  const string PVL_Put_PB                                        = "Put_PB";
        public  const string PVL_Pvl_Manual_Mode                               = "Manual_Mode";
        public  const string PVL_L2_ErrCode                                    = "L2_ErrCode";
        public  const string PVL_Get_Put_Done                                  = "Get_Put_Done";
                
        public  const string PVL_Get_PB_Done                                   = "Get_PB_Done";
        public  const string PVL_Get_PB                                        = "Get_PB";
        public  const string PVL_Current_Floor                                 = "Current_Floor";
        public  const string PVL_CP_Start                                      = "CP_Start";
                
        public  const string PVL_CP_Put_Start                                  = "CP_Put_Start";
        public  const string PVL_CP_Get_Start                                  = "CP_Get_Start";
        public  const string PVL_CP_Done                                       = "CP_Done";
        public  const string PVL_Auto_Ready                                    = "Auto_Ready";
                
        public  const string PVL_Auto_Mode                                     = "Auto_Mode";
        public const string  PVL_Deck_Pallet_Present                           = "Deck_Pallet_Present";
        public const string PVL_West_Pallet_Present                            = "West_Pallet_Present_Prox";
        public const string PVL_East_Pallet_Present                            = "East_Pallet_Present_Prox";
        
                
        //PST   
        public  const string PST_South_Pallet_Sensor_4                         = "South_Pallet_Sensor_4";
        public  const string PST_South_Pallet_Sensor_3                         = "South_Pallet_Sensor_3";
        public  const string PST_South_Pallet_Sensor_2                         = "South_Pallet_Sensor_2";
        public  const string PST_South_Pallet_Sensor_1                         = "South_Pallet_Sensor_1";
                
        public  const string PST_North_Pallet_Sensor_4                         = "North_Pallet_Sensor_4";
        public  const string PST_North_Pallet_Sensor_3                         = "North_Pallet_Sensor_3";
        public  const string PST_North_Pallet_Sensor_2                         = "North_Pallet_Sensor_2";
        public  const string PST_North_Pallet_Sensor_1                         = "North_Pallet_Sensor_1";

        public const string PST100_Pallet_Sensor_1_Bottom                      = "B_Line_Pallet_Sensor_1_Bottom";
        public const string PST100_Pallet_Sensor_2                             = "B_Line_Pallet_Sensor_2";
        public const string PST100_Pallet_Sensor_3                             = "B_Line_Pallet_Sensor_3";
        public const string PST100_Pallet_Sensor_4_Top                         = "B_Line_Pallet_Sensor_4_Top";

        public  const string PST_L2_ErrCode                                    = "L2_ErrCode";
        public  const string PST_Auto_Ready                                    = "Auto_Ready";
        public  const string PST_Auto_Mode                                     = "Auto_Mode";
        public  const string PST_StackFull                                     = "StackFull";
                
        public  const string PST_StackEmpty                                    = "StackEmpty";
        public const string PST_Pallet_Count                                   = "Pallet_Count";

                
        //PS    
        public  const string PS_West_Pallet_Present_Prox                       = "West_Pallet_Present_Prox";
        public  const string PS_East_Pallet_Present_Prox                       = "East_Pallet_Present_Prox";
        public  const string PS_Shuttle_Aisle_Position_for_L2                  = "Shuttle_Aisle_Position_for_L2";
        public  const string PS_Req_Unlock                                     = "Req_Unlock";
                
        public  const string PS_Req_Put                                        = "Req_Put";
        public  const string PS_Req_Lockout                                    = "Req_Lockout";
        public  const string PS_Req_Get                                        = "Req_Get";
        public  const string PS_PalletPresent                                  = "PalletPresent";
                
        public  const string PS_NextPSOff                                      = "NextPSOff";
        public  const string PS_Manual_Mode                                    = "Manual_Mode";
        public  const string PS_LockConfirmed                                  = "LockConfirmed";
        public  const string PS_L2_PutCmd                                      = "L2_PutCmd";
                
        public  const string PS_L2_PST_PutCmd                                  = "L2_PST_PutCmd";
        public  const string PS_L2_PST_GetCmd                                  = "L2_PST_GetCmd";
        public  const string PS_L2_MoveCmd                                     = "L2_MoveCmd";
        public  const string PS_L2_Min_Window_Limit                            = "L2_Min_Window_Limit";
                
        public  const string PS_L2_Max_Window_Limit                            = "L2_Max_Window_Limit";
        public  const string PS_L2_GetCmd                                      = "L2_GetCmd";
        public  const string PS_L2_Error_Data_Register                         = "L2_Error_Data_Register";
        public  const string PS_L2_EStop                                       = "L2_EStop";
                
        public  const string PS_L2_EES_PutCmd                                  = "L2_EES_PutCmd";
        public  const string PS_L2_EES_GetCmd                                  = "L2_EES_GetCmd";
        public  const string PS_L2_Destination_Aisle                           = "L2_Destination_Aisle";
        public  const string PS_L2_CMD_DONE_ALM                                = "L2_CMD_DONE_ALM";
                
        public  const string PS_L2_CMD_DONE                                    = "L2_CMD_DONE";
        public  const string PS_L2_Block_PS                                    = "L2_Block_PS";
        public  const string PS_L2_Auto_Ready_Bit                              = "L2_Auto_Ready_Bit";
        public  const string PS_Auto_Mode                                      = "Auto_Mode";

        //For Car Wash
        public const string WASH_CarWash_HighCar                               = "CarWash_HighCar";
        public const string WASH_CarWash_Ready                                 = "CarWash_Ready";
        public const string WASH_CarWash_Car_Present                           = "CarWash_Car_Present";
        public const string WASH_CW_Fin                                        = "CW_Fin";
        public const string WASH_CarWash_Start_Cycle                           = "CarWash_Start_Cycle";
        
        //Camera OPC
        public const string EES_Cam_ImgPath                                    = "ImgPath";
        public const string EES_Cam_GetCmd                                     = "GetCmd";
        


    }
}
