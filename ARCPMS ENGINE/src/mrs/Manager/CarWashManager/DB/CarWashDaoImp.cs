using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ARCPMS_ENGINE.src.mrs.DBCon;
using Oracle.DataAccess.Client;

namespace ARCPMS_ENGINE.src.mrs.Manager.CarWashManager.DB
{
    class CarWashDaoImp: CarWashDaoService
    {
        public int InsertQueueForWashing(int job)
        {
            int washingQueueId = 0;
            try
            {
                //while (washingQueueId == 0)
                // {
                using (OracleConnection con = new DBConnection().getDBConnection()) // new DA.Connection().getDBConnection())
                {
                    using (OracleCommand command = con.CreateCommand())
                    {
                        if (con.State == ConnectionState.Closed) con.Open();
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "CAR_WASHING_PACKAGE.insert_queue_for_washing";
                        command.Parameters.Add("job_param", OracleDbType.Int32, job, ParameterDirection.Input);
                        command.Parameters.Add("wash_queue_id", OracleDbType.Int32, washingQueueId, ParameterDirection.Output);
                        command.ExecuteNonQuery();
                        Int32.TryParse(command.Parameters["wash_queue_id"].Value.ToString(), out washingQueueId);
                    }
                }
                // }
            }
            catch (Exception errMsg)
            {
                Console.WriteLine(errMsg.Message);
            }
            finally
            {

            }

            return washingQueueId;
        }

        public bool FindCarWashingPathFirst(int washQId)
        {
            bool result = true;
            try
            {

                using (OracleConnection con = new DBConnection().getDBConnection()) // new DA.Connection().getDBConnection())
                {
                    using (OracleCommand command = con.CreateCommand())
                    {
                        if (con.State == ConnectionState.Closed) con.Open();
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "CAR_WASHING_PACKAGE.find_car_washing_path_first";
                        command.Parameters.Add("wash_queue_id", OracleDbType.Int32, washQId, ParameterDirection.Input);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception errMsg)
            {
                result = false;
                Console.WriteLine(errMsg.Message);
            }
            finally
            {

            }
            return result;
        }

        public int FindCarWashingPathSecond(int washQId)
        {
            int pathId = 0;
            try
            {


                using (OracleConnection con = new DBConnection().getDBConnection()) 
                {
                    using (OracleCommand command = con.CreateCommand())
                    {
                        if (con.State == ConnectionState.Closed) con.Open();
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "CAR_WASHING_PACKAGE.find_car_washing_path_second";
                        command.Parameters.Add("wash_queue_id", OracleDbType.Int32, washQId, ParameterDirection.Input);
                        command.Parameters.Add("wash_path_id", OracleDbType.Int64, pathId, ParameterDirection.Output);

                        command.ExecuteNonQuery();
                        int.TryParse(command.Parameters["wash_path_id"].Value.ToString(), out pathId);
                    }
                }
            }
            catch (Exception errMsg)
            {
                Console.WriteLine(errMsg.Message);
            }
            finally
            {

            }
            return pathId;
        }

        public bool updateBeforeWashing(int washQId)
        {
            bool res = true;
            try
            {
                using (OracleConnection con = new DBConnection().getDBConnection())
                {
                    using (OracleCommand command = con.CreateCommand())
                    {
                        if (con.State == ConnectionState.Closed) con.Open();
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "CAR_WASHING_PACKAGE.update_before_washing";
                        command.Parameters.Add("arg_queue_id", OracleDbType.Int32, washQId, ParameterDirection.Input);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception errMsg)
            {
                res = false;
                Console.WriteLine(errMsg.Message);
            }
            finally
            {

            }
            return res;
        }

        public bool updateAfterWashing(int washQId)
        {
            bool res = true;
            try
            {
                using (OracleConnection con = new DBConnection().getDBConnection())
                {
                    using (OracleCommand command = con.CreateCommand())
                    {
                        if (con.State == ConnectionState.Closed) con.Open();
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "CAR_WASHING_PACKAGE.update_after_washing";
                        command.Parameters.Add("arg_queue_id", OracleDbType.Int32, washQId, ParameterDirection.Input);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception errMsg)
            {
                res = false;
                Console.WriteLine(errMsg.Message);
            }
            finally
            {

            }
            return res;
        }

        public bool updateAfterReachedInSlot(int washQId)
        {
            bool res = true;
            try
            {
                using (OracleConnection con = new DBConnection().getDBConnection())
                {
                    using (OracleCommand command = con.CreateCommand())
                    {
                        if (con.State == ConnectionState.Closed) con.Open();
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "CAR_WASHING_PACKAGE.update_after_reached_in_slot";
                        command.Parameters.Add("arg_queue_id", OracleDbType.Int32, washQId, ParameterDirection.Input);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception errMsg)
            {
                res = false;
                Console.WriteLine(errMsg.Message);
            }
            finally
            {

            }
            return res;
        }
        public void UpdateIsWashReady(bool isReady)
        {
            try
            {
                using (OracleConnection con = new DBConnection().getDBConnection())
                {
                    using (OracleCommand command = con.CreateCommand())
                    {
                        if (con.State == ConnectionState.Closed) con.Open();
                        command.CommandType = CommandType.Text;
                        command.CommandText = "UPDATE L2_CARWASH_MASTER SET IS_WASH_READY = " + (isReady ? 1 : 0) + " where MASTER_ID = 1";
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception errMsg)
            {
                Console.WriteLine("UpdateIsWashReady, " + errMsg.Message);
            }
            finally
            {

            }
        }
        public void UpdateCarWashFinishTrigger(int isCarWashFinish)
        {
            try
            {
                using (OracleConnection con = new DBConnection().getDBConnection())
                {
                    if (con.State == ConnectionState.Closed) con.Open();
                    OracleCommand command = con.CreateCommand();

                    string sql = "update L2_CONFIG_MASTER set VALUE = '" + Convert.ToString(isCarWashFinish) + "'  where MODULE_NAME ='CARWASH' " +
                        " and ITEM_NAME = 'FINISH' and PROPERTY_NAME = 'IsTriggered'";
                    command.CommandText = sql;
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
            }
            finally
            { }
        }
        public bool IsCarWashFinishTriggeredByUser()
        {
            bool isTriggered = false;
            try
            {
                using (OracleConnection con = new DBConnection().getDBConnection())
                {
                    if (con.State == ConnectionState.Closed) con.Open();
                    OracleCommand command = con.CreateCommand();

                    string sql = "select VALUE from L2_CONFIG_MASTER where MODULE_NAME ='CARWASH' " +
                        " and ITEM_NAME = 'FINISH' and PROPERTY_NAME = 'IsTriggered'";
                    command.CommandText = sql;
                    command.CommandType = CommandType.Text;
                    int intTrigger = 0;
                    int.TryParse(Convert.ToString(command.ExecuteScalar()), out intTrigger);
                    isTriggered = (intTrigger == 2); //2 = user triggered button to get car from car slot.
                }
            }
            finally
            { }
            return isTriggered;
        }
        public bool updateAfterProcessing(int washQId)
        {
            bool res = true;
            try
            {
                using (OracleConnection con = new DBConnection().getDBConnection())
                {
                    using (OracleCommand command = con.CreateCommand())
                    {
                        if (con.State == ConnectionState.Closed) con.Open();
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "CAR_WASHING_PACKAGE.update_after_processing";
                        command.Parameters.Add("arg_queue_id", OracleDbType.Int32, washQId, ParameterDirection.Input);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception errMsg)
            {
                res = false;
                Console.WriteLine(errMsg.Message);
            }
            finally
            {

            }
            return res;
        }
    }
}
