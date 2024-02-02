using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using NpgsqlTypes;
using GoWMS.Server.Controllers;
using GoWMS.Server.Models;
using GoWMS.Server.Models.Wcs;
using GoWMS.Server.Models.Hagv;
using GoWMS.Server.DataAccess;
using Serilog;


namespace GoWMS.Server.Data
{
    public class AgvDAL
    {
        readonly private string connectionString = ConnGlobals.GetConnLocalDBPG();

        public IEnumerable<Tas_Agvworks> GetAllTaskAgv()
        {
            List<Tas_Agvworks> lstobj = new List<Tas_Agvworks>();
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();

                    sql.AppendLine("SELECT t1.idx, t1.created, t1.entity_lock, t1.modified, t1.client_id, t1.client_ip");
                    sql.AppendLine(", t1.lpncode, t1.work_code, t1.work_status, t1.work_id");
                    sql.AppendLine(", t1.agv_name, t1.gate_source, t1.gate_dest, t1.ctime, t1.stime, t1.etime, t1.work_priority, t1.task_code");
                    sql.AppendLine(", 0 as gate_status, '-' as gate_status_desc");
                    sql.AppendLine("FROM wcs.tas_agvworks t1");
                    //sql.AppendLine("left join cira.vagvgate_status t2");
                    //sql.AppendLine("on t1.gate_dest=t2.gate_name");
                    sql.AppendLine("order by t1.created");
                    sql.AppendLine(";");

                    NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };
                    con.Open();

                    NpgsqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Tas_Agvworks objrd = new Tas_Agvworks
                        {
                            Idx = rdr["idx"] == DBNull.Value ? null : (Int64?)rdr["idx"],
                            Entity_Lock = rdr["entity_lock"] == DBNull.Value ? null : (int?)rdr["entity_lock"],
                            Created = rdr["created"] == DBNull.Value ? null : (DateTime?)rdr["created"],
                            Modified = rdr["modified"] == DBNull.Value ? null : (DateTime?)rdr["modified"],
                            Client_Id = rdr["client_id"] == DBNull.Value ? null : (long?)rdr["client_id"],
                            Client_Ip = rdr["client_ip"].ToString(),
                            Lpncode = rdr["lpncode"].ToString(),
                            Work_Code = rdr["work_code"].ToString(),
                            Work_Status = rdr["work_status"] == DBNull.Value ? null : (Int32?)rdr["work_status"],
                            Work_Id = rdr["work_id"] == DBNull.Value ? null : (Int64?)rdr["work_id"],
                            Agv_Name = rdr["agv_name"].ToString(),
                            Gate_Source = rdr["gate_source"].ToString(),
                            Gate_Dest = rdr["gate_dest"].ToString(),
                            Ctime = rdr["ctime"] == DBNull.Value ? null : (DateTime?)rdr["ctime"],
                            Stime = rdr["stime"] == DBNull.Value ? null : (DateTime?)rdr["stime"],
                            Etime = rdr["etime"] == DBNull.Value ? null : (DateTime?)rdr["etime"],
                            Work_Priority = rdr["work_priority"] == DBNull.Value ? null : (Int32?)rdr["work_priority"],
                            Task_code = rdr["task_code"].ToString(),
                            DestStatus = rdr["gate_status"] == DBNull.Value ? null : (Int32?)rdr["gate_status"],
                            DestMsg = rdr["gate_status_desc"].ToString()
                        };
                        lstobj.Add(objrd);
                    }
                }
                catch (NpgsqlException ex)
                {
                    Log.Error(ex.ToString());
                }
                finally
                {
                    con.Close();
                }


            }
            return lstobj;
        }


        public IEnumerable<Functionreturn> CancleTaskAGV(string pallet)
        {
            List<Functionreturn> lstobj = new List<Functionreturn>();
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine("SELECT _retchk, _retmsg FROM");
                    sql.AppendLine("wcs.fuc_cancel_avgtask(");
                    sql.AppendLine("@_lpncode)");

                    NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };

                    con.Open();

                    cmd.Parameters.AddWithValue("@_lpncode", pallet);

                    NpgsqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Functionreturn objrd = new Functionreturn
                        {

                            Retchk = rdr["_retchk"] == DBNull.Value ? null : (Int32?)rdr["_retchk"],
                            Retmsg = rdr["_retmsg"].ToString()
                        };
                        lstobj.Add(objrd);
                    }
                }
                catch (NpgsqlException ex)
                {
                    Log.Error(ex.ToString());
                }
                finally
                {
                    con.Close();
                }
            }
            return lstobj;
        }

        public List<Agv_Status> GetAllAgvStatusDesc()
        {
            List<Agv_Status> lstobj = new List<Agv_Status>();
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();

                    sql.AppendLine("SELECT status, description");
                    sql.AppendLine("FROM hagv.agv_status");
                    sql.AppendLine(";");

                    DataSet _ds = new DataSet();
                    DataTable _dt = new DataTable();
                    using (NpgsqlCommand vCmd = new NpgsqlCommand())
                    {
                        con.Open();
                        vCmd.Connection = con;
                        vCmd.CommandType = CommandType.Text;
                        vCmd.CommandText = sql.ToString();
                        NpgsqlDataAdapter da = new NpgsqlDataAdapter(vCmd);
                        da.Fill(_ds);
                        _dt = _ds.Tables[0];
                        for (int i = 0; i < _dt.Rows.Count; i++)
                        {
                            lstobj.Add(new Agv_Status() { Status = Convert.ToInt16(_dt.Rows[i]["status"].ToString()), Description = _dt.Rows[i]["description"].ToString() });
                        }
                    }
                }
                catch (NpgsqlException ex)
                {
                    Log.Error(ex.ToString());
                }
                finally
                {
                    con.Close();
                }
            }
            return lstobj;
        }


        public DataTable GetQueryAgvStatusApiName()
        {
            DataTable vApiName = new DataTable();

            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();

                    sql.AppendLine("SELECT base_url::text || api_name::text AS full_api_name, map_short_name");
                    sql.AppendLine("FROM hagv.set_agv_api");
                    sql.AppendLine("WHERE api_name=@api_name");
                    sql.AppendLine(";");

                    DataSet _ds = new DataSet();
                    DataTable _dt = new DataTable();
                    using (NpgsqlCommand vCmd = new NpgsqlCommand())
                    {
                        con.Open();
                        vCmd.Connection = con;
                        vCmd.CommandType = CommandType.Text;
                        vCmd.CommandText = sql.ToString();
                        vCmd.Parameters.AddWithValue("@api_name", "queryAgvStatus");
                        NpgsqlDataAdapter da = new NpgsqlDataAdapter(vCmd);
                        da.Fill(_ds);
                        _dt = _ds.Tables[0];
                        vApiName = _dt;
                    }
                }
                catch (NpgsqlException ex)
                {
                    Log.Error(ex.ToString());
                }
                finally
                {
                    con.Close();
                }

            }
            return vApiName;
        }

        public IEnumerable<Vrptqueueloadtimeagv> GetAllReportTaskAgv()
        {
            List<Vrptqueueloadtimeagv> lstobj = new List<Vrptqueueloadtimeagv>();
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();

                    sql.AppendLine("SELECT * ");
                    sql.AppendLine("FROM wcs.vrptqueueloadtimeagv");
                    sql.AppendLine(";");

                    NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };
                    con.Open();

                    NpgsqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Vrptqueueloadtimeagv objrd = new Vrptqueueloadtimeagv
                        {
                            Lpncode = rdr["lpncode"].ToString(),
                            Work_code = rdr["work_code"].ToString(),
                            Work_status = rdr["work_status"] == DBNull.Value ? null : (Int32?)rdr["work_status"],
                            Agv_name = rdr["agv_name"].ToString(),
                            Gate_source = rdr["gate_source"].ToString(),
                            Gate_dest = rdr["gate_dest"].ToString(),
                            Ctime = rdr["ctime"] == DBNull.Value ? null : (DateTime?)rdr["ctime"],
                            Stime = rdr["stime"] == DBNull.Value ? null : (DateTime?)rdr["stime"],
                            Etime = rdr["etime"] == DBNull.Value ? null : (DateTime?)rdr["etime"],
                            Loadtime = rdr["loadtime"].ToString()
                        };
                        lstobj.Add(objrd);
                    }
                }
                catch (NpgsqlException ex)
                {
                    Log.Error(ex.ToString());
                }
                finally
                {
                    con.Close();
                }
            }
            return lstobj;
        }

        public IEnumerable<RptTaskHourCount> GetAllReportTaskPerHourAgv()
        {
            List<RptTaskHourCount> lstobj = new List<RptTaskHourCount>();
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();

                    sql.AppendLine("SELECT * ");
                    sql.AppendLine("FROM wcs.vrptqueueperhouragv");
                    sql.AppendLine(";");

                    NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };
                    con.Open();

                    NpgsqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        RptTaskHourCount objrd = new RptTaskHourCount
                        {
                            W_hour = rdr["w_hour"] == DBNull.Value ? null : (DateTime?)rdr["w_hour"],
                            W_count = rdr["w_count"] == DBNull.Value ? null : (Int64?)rdr["w_count"]
                        };
                        lstobj.Add(objrd);
                    }
                }
                catch (NpgsqlException ex)
                {
                    Log.Error(ex.ToString());
                }
                finally
                {
                    con.Close();
                }
            }
            return lstobj;
        }


    }
}
