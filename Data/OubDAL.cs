using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;
using GoWMS.Server.Controllers;
using GoWMS.Server.Models.Oub;
using GoWMS.Server.Models.Public;
using NpgsqlTypes;
using System.Text;

namespace GoWMS.Server.Data
{
    public class OubDAL
    {
        readonly private string connectionString = ConnGlobals.GetConnLocalDBPG();

        public IEnumerable<Sap_Storeout> GetAllSapStoreout()
        {
            List<Sap_Storeout> lstobj = new List<Sap_Storeout>();
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                StringBuilder sql = new StringBuilder();

                sql.AppendLine("SELECT subQ.*");

                sql.AppendLine(", CASE WHEN t3.weightnet IS NULL ");
                sql.AppendLine(" THEN   subQ.request_qty");
                sql.AppendLine(" ELSE subQ.request_qty * t3.weightnet ");
                sql.AppendLine(" END AS disrequest_qty");

                sql.AppendLine(", CASE WHEN t3.weightnet IS NULL ");
                sql.AppendLine(" THEN   subQ.stock_qty");
                sql.AppendLine(" ELSE subQ.stock_qty * t3.weightnet ");
                sql.AppendLine(" END AS disstock_qty");

                sql.AppendLine(", CASE WHEN t3.weightnet IS NULL ");
                sql.AppendLine(" THEN   subQ.transfer_qty");
                sql.AppendLine(" ELSE subQ.transfer_qty * t3.weightnet ");
                sql.AppendLine(" END AS distransfer_qty");

                sql.AppendLine(", t3.itemname as itemdesctiption");

                sql.AppendLine("FROM(");
                sql.AppendLine("SELECT *");
                sql.AppendLine("FROM public.sap_storeout");
                sql.AppendLine("WHERE status < @status");
                sql.AppendLine(")subQ");

                sql.AppendLine("LEFT JOIN wms.mas_item_go t3");
                sql.AppendLine("ON subQ.item_code=t3.itemcode");

                sql.AppendLine("order by idx");

                NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), con)
                {
                    CommandType = CommandType.Text
                };
                cmd.Parameters.AddWithValue("@status", 3);


                //NpgsqlCommand cmd = new NpgsqlCommand("select * " +
                //    "from public.sap_storeout  " +
                //    "where  status <@status " +
                //    "order by idx", con)
                //{
                //    CommandType = CommandType.Text
                //};
                //cmd.Parameters.AddWithValue("@status", 3);


                con.Open();
                NpgsqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Sap_Storeout objrd = new Sap_Storeout
                    {
                        Idx = rdr["idx"] == DBNull.Value ? null : (Int64?)rdr["idx"],
                        Entity_Lock = rdr["entity_lock"] == DBNull.Value ? null : (Int32?)rdr["entity_lock"],
                        Created = rdr["created"] == DBNull.Value ? null : (DateTime?)rdr["created"],
                        Modified = rdr["modified"] == DBNull.Value ? null : (DateTime?)rdr["modified"],
                        Client_Id = rdr["client_Id"] == DBNull.Value ? null : (Int64?)rdr["client_Id"],
                        Client_Ip = rdr["client_Ip"].ToString(),
                        Order_No = rdr["order_no"].ToString(),
                        Ship_To_Code = rdr["ship_to_code"].ToString(),
                        Ship_Name = rdr["ship_name"].ToString(),
                        Delivery_Priority = rdr["delivery_priority"] == DBNull.Value ? null : (Int32?)rdr["delivery_priority"],
                        Delivery_Date = rdr["delivery_date"] == DBNull.Value ? null : (DateTime?)rdr["delivery_date"],
                        Item_Code = rdr["item_code"].ToString(),
                        Batch_Number = rdr["batch_number"].ToString(),
                        Request_Qty = rdr["request_qty"] == DBNull.Value ? null : (decimal?)rdr["request_qty"],
                        Status = rdr["status"] == DBNull.Value ? null : (Int32?)rdr["status"],
                        Error_Code = rdr["error_code"].ToString(),
                        Movement_Type = rdr["movement_type"].ToString(),
                        Movement_Reason = rdr["movement_reason"].ToString(),
                        To_No = rdr["to_no"].ToString(),
                        To_Line = rdr["to_line"].ToString(),
                        Po_Header_Txt = rdr["po_header_txt"].ToString(),
                        Requisitioner = rdr["requisitioner"].ToString(),
                        Po_No = rdr["po_no"].ToString(),
                        Remark = rdr["remark"].ToString(),
                        Doc_Ref = rdr["doc_ref"].ToString(),
                        Created_By = rdr["created_by"].ToString(),
                        Created_Date = rdr["created_date"] == DBNull.Value ? null : (DateTime?)rdr["created_date"],
                        Update_By = rdr["update_by"].ToString(),
                        Update_Date = rdr["update_date"] == DBNull.Value ? null : (DateTime?)rdr["update_date"],
                        Order_Line = rdr["order_line"].ToString(),
                        Stock_Consign = rdr["stock_consign"].ToString(),
                        Site = rdr["site"].ToString(),
                        Storage_Location = rdr["storage_location"].ToString(),
                        Warehouse = rdr["warehouse"].ToString(),
                        Item_Name = rdr["itemdesctiption"].ToString(),
                        Tracking_Number = rdr["tracking_number"].ToString(),
                        Su_No = rdr["su_no"].ToString(),
                        Pallet_No = rdr["pallet_no"].ToString(),
                        Stock_Qty = rdr["stock_qty"] == DBNull.Value ? null : (decimal?)rdr["stock_qty"],
                        Transfer_Qty = rdr["transfer_qty"] == DBNull.Value ? null : (decimal?)rdr["transfer_qty"],
                        Ref_No = rdr["ref_no"].ToString(),
                        Ref_Line = rdr["ref_line"].ToString(),
                        Unit = rdr["unit"].ToString(),
                        Vendor_Code = rdr["vendor_code"].ToString(),
                        Batch_No = rdr["batch_no"].ToString(),
                        DisRequest_Qty = rdr["disrequest_qty"] == DBNull.Value ? null : (decimal?)rdr["disrequest_qty"],
                        DisStock_Qty = rdr["disstock_qty"] == DBNull.Value ? null : (decimal?)rdr["disstock_qty"],
                        DisTransfer_Qty = rdr["distransfer_qty"] == DBNull.Value ? null : (decimal?)rdr["distransfer_qty"]
                    };
                    lstobj.Add(objrd);
                }
                con.Close();
            }
            return lstobj;
        }

        public async Task<IEnumerable<Inv_Stock_GoApiInfo>> GetStockListInfo()
        {

            List<Inv_Stock_GoApiInfo> lstobj = new List<Inv_Stock_GoApiInfo>();
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                StringBuilder Sql = new StringBuilder();
                /*
                Sql.AppendLine("select row_number() over(order by  itemcode asc) AS rn,");
                Sql.AppendLine("itemcode, itemname, quantity, pallettag, pallteno, storagearea, storagebin");
                Sql.AppendLine("from wms.inv_stock_go ");
                Sql.AppendLine("order by itemcode, ");
                */
                Sql.AppendLine("SELECT subQ.*");
                Sql.AppendLine(", CASE WHEN t3.weightnet IS NULL ");
                Sql.AppendLine(" THEN   subQ.apiGR_Quantity");
                Sql.AppendLine(" ELSE subQ.apiGR_Quantity * t3.weightnet ");
                Sql.AppendLine(" END AS disapigr_quantity");

                Sql.AppendLine(", t3.itemname as itemdesctiption");

                Sql.AppendLine("FROM (");
                Sql.AppendLine("SELECT t1.efidx , t1.efstatus, t1.created, t1.modified, t1.innovator, t1.device");
                Sql.AppendLine(", t1.pono, t1.pallettag, t1.itemtag, t1.itemcode, t1.itemname, t1.itembar, t1.unit");
                Sql.AppendLine(", t1.weightunit, t1.quantity, t1.weight, t1.lotno, t1.totalquantity, t1.totalweight");
                Sql.AppendLine(", t1.docno, t1.docby, t1.docdate, t1.docnote, t1.grnrefer, t1.grntime, t1.grtime");
                Sql.AppendLine(", t1.grtype, t1.pallteno, t1.palltmapkey, t1.storagetime, t1.storageno");
                Sql.AppendLine(", t1.storagearea, t1.storagebin, t1.gnrefer, t1.allocatequantity, t1.allocateweight");
                Sql.AppendLine(",t0.efidx as apiefidx, t0.efstatus as apiefstatus, t0.created as apicreated, t0.modified as apimodified, t0.innovator as apiinnovator, t0.device as apidevice");
                Sql.AppendLine(",t0.taskno as apiPackage_ID, t0.taskno as apiRoll_ID");

                Sql.AppendLine(",t0.itemno as apiMaterial_Code, '-' as apiMaterial_Description");
                Sql.AppendLine(",t0.created as apiReceiving_Date, CAST(t0.qty AS DECIMAL) as  apiGR_Quantity");
                Sql.AppendLine(",null as apiUnit,1.00 as apiGR_Quantity_KG");
                Sql.AppendLine(",t0.batchno as  apiWH_Code, t0.pickgate as apiWarehouse, 'P' || t0.itemno as apiJob, t0.batchno as apiJob_Code, t0.tasktype as apitypor, t0.taskno as apikaror");
                Sql.AppendLine("FROM api.posttaskorders t0");
                Sql.AppendLine("LEFT JOIN wms.vinv_stock_go_readypick t1");

                Sql.AppendLine("ON t0.palletcode = t1.pallteno");
                Sql.AppendLine("WHERE t0.tasktype=@typor");
                Sql.AppendLine("AND t0.efstatus=@efstatus");
                Sql.AppendLine(")subQ");

                Sql.AppendLine("LEFT JOIN wms.mas_item_go t3");
                Sql.AppendLine("ON subQ.apiMaterial_Code=t3.itemcode");

                Sql.AppendLine("order by itemcode ASC, docdate ASC, pallettag ASC");


                NpgsqlCommand cmd = new NpgsqlCommand(Sql.ToString(), con)
                {
                    CommandType = CommandType.Text
                };
                cmd.Parameters.AddWithValue("@typor", "05");
                cmd.Parameters.AddWithValue("@efstatus", 0);

                con.Open();
                NpgsqlDataReader rdr = cmd.ExecuteReader();
                while (await rdr.ReadAsync())
                {
                    Inv_Stock_GoApiInfo objrd = new Inv_Stock_GoApiInfo
                    {
                        Efidx = rdr["efidx"] == DBNull.Value ? null : (Int64?)rdr["efidx"],
                        Efstatus = rdr["efstatus"] == DBNull.Value ? null : (Int32?)rdr["efstatus"],
                        Created = rdr["created"] == DBNull.Value ? null : (DateTime?)rdr["created"],
                        Modified = rdr["modified"] == DBNull.Value ? null : (DateTime?)rdr["modified"],
                        Innovator = rdr["innovator"] == DBNull.Value ? null : (Int64?)rdr["innovator"],
                        Device = rdr["device"].ToString(),
                        Pono = rdr["pono"].ToString(),
                        Pallettag = rdr["pallettag"].ToString(),
                        Itemtag = rdr["itemtag"].ToString(),
                        Itemcode = rdr["itemcode"].ToString(),
                        Itemname = rdr["itemdesctiption"].ToString(),
                        Itembar = rdr["itembar"].ToString(),
                        Unit = rdr["unit"].ToString(),
                        Weightunit = rdr["weightunit"].ToString(),
                        Quantity = rdr["quantity"] == DBNull.Value ? null : (decimal?)rdr["quantity"],
                        Weight = rdr["weight"] == DBNull.Value ? null : (decimal?)rdr["weight"],
                        Lotno = rdr["lotno"].ToString(),
                        Totalquantity = rdr["totalquantity"] == DBNull.Value ? null : (decimal?)rdr["totalquantity"],
                        Totalweight = rdr["totalweight"] == DBNull.Value ? null : (decimal?)rdr["totalweight"],
                        Docno = rdr["docno"].ToString(),
                        Docby = rdr["docby"].ToString(),
                        Docdate = rdr["docdate"] == DBNull.Value ? null : (DateTime?)rdr["docdate"],
                        Docnote = rdr["docnote"].ToString(),
                        Grnrefer = rdr["grnrefer"] == DBNull.Value ? null : (Int64?)rdr["grnrefer"],
                        Grntime = rdr["grntime"] == DBNull.Value ? null : (DateTime?)rdr["grntime"],
                        Grtime = rdr["grtime"] == DBNull.Value ? null : (DateTime?)rdr["grtime"],
                        Grtype = rdr["grtype"].ToString(),
                        Pallteno = rdr["pallteno"].ToString(),
                        Palltmapkey = rdr["palltmapkey"].ToString(),
                        Storagetime = rdr["storagetime"] == DBNull.Value ? null : (DateTime?)rdr["storagetime"],
                        Storageno = rdr["storageno"].ToString(),
                        Storagearea = rdr["storagearea"].ToString(),
                        Storagebin = rdr["storagebin"].ToString(),
                        Gnrefer = rdr["gnrefer"] == DBNull.Value ? null : (Int64?)rdr["gnrefer"],
                        Allocatequantity = rdr["allocatequantity"] == DBNull.Value ? null : (decimal?)rdr["allocatequantity"],
                        Allocateweight = rdr["allocateweight"] == DBNull.Value ? null : (decimal?)rdr["allocateweight"],
                        Apiefidx = rdr["apiefidx"] == DBNull.Value ? null : (Int64?)rdr["apiefidx"],
                        Apiefstatus = rdr["apiefstatus"] == DBNull.Value ? null : (Int32?)rdr["apiefstatus"],
                        Apicreated = rdr["apicreated"] == DBNull.Value ? null : (DateTime?)rdr["apicreated"],
                        Apimodified = rdr["apimodified"] == DBNull.Value ? null : (DateTime?)rdr["apimodified"],
                        Apiinnovator = rdr["apiinnovator"] == DBNull.Value ? null : (Int64?)rdr["apiinnovator"],
                        Apidevice = rdr["apidevice"].ToString(),
                        ApipackageID = rdr["apiPackage_ID"].ToString(),
                        ApirollID = rdr["apiRoll_ID"].ToString(),
                        ApimaterialCode = rdr["apiMaterial_Code"].ToString(),
                        ApimaterialDescription = rdr["apiMaterial_Description"].ToString(),
                        ApireceivingDate = rdr["apiReceiving_Date"] == DBNull.Value ? null : (DateTime?)rdr["apiReceiving_Date"],
                        ApigrQuantity = rdr["apiGR_Quantity"] == DBNull.Value ? null : (decimal?)rdr["apiGR_Quantity"],
                        ApiUnit = rdr["apiUnit"].ToString(),
                        apigrQuantityKG = rdr["apiGR_Quantity_KG"] == DBNull.Value ? null : (decimal?)rdr["apiGR_Quantity_KG"],
                        ApiwhCode = rdr["apiWH_Code"].ToString(),
                        Apiwarehouse = rdr["apiWarehouse"].ToString(),
                        Apijob = rdr["apiJob"].ToString(),
                        ApijobCode = rdr["apiJob_Code"].ToString(),
                        Apitypor = rdr["apitypor"].ToString(),
                        Apikaror = rdr["apikaror"].ToString(),
                        DisApigrQuantity = rdr["disapigr_quantity"] == DBNull.Value ? null : (decimal?)rdr["disapigr_quantity"],
                    };
                    lstobj.Add(objrd);
                }
                con.Close();
            }
            return lstobj;
        }


        public IEnumerable<Sap_Storeout> GetSapStoreoutSetBatch()
        {
            List<Sap_Storeout> lstobj = new List<Sap_Storeout>();
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {

                StringBuilder sql = new StringBuilder();

                sql.AppendLine("SELECT subQ.*");

                sql.AppendLine(", CASE WHEN t3.weightnet IS NULL ");
                sql.AppendLine(" THEN   subQ.request_qty");
                sql.AppendLine(" ELSE subQ.request_qty * t3.weightnet ");
                sql.AppendLine(" END AS disrequest_qty");

                sql.AppendLine(", CASE WHEN t3.weightnet IS NULL ");
                sql.AppendLine(" THEN   subQ.stock_qty");
                sql.AppendLine(" ELSE subQ.stock_qty * t3.weightnet ");
                sql.AppendLine(" END AS disstock_qty");

                sql.AppendLine(", CASE WHEN t3.weightnet IS NULL ");
                sql.AppendLine(" THEN   subQ.transfer_qty");
                sql.AppendLine(" ELSE subQ.transfer_qty * t3.weightnet ");
                sql.AppendLine(" END AS distransfer_qty");

                sql.AppendLine(", t3.itemname as itemdesctiption");

                sql.AppendLine("FROM(");
                sql.AppendLine("SELECT *");
                sql.AppendLine("FROM public.sap_storeout");
                sql.AppendLine("WHERE status = @status");
                sql.AppendLine(")subQ");

                sql.AppendLine("LEFT JOIN wms.mas_item_go t3");
                sql.AppendLine("ON subQ.item_code=t3.itemcode");

                sql.AppendLine("order by idx");

                NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), con)
                {
                    CommandType = CommandType.Text
                };
                cmd.Parameters.AddWithValue("@status", 0);


                //NpgsqlCommand cmd = new NpgsqlCommand("select * " +
                //    "from public.sap_storeout  " +
                //    "where (1=1)  " +
                //    "and status =0 " +
                //    "order by idx", con)
                //{
                //    CommandType = CommandType.Text
                //};
                con.Open();
                NpgsqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Sap_Storeout objrd = new Sap_Storeout
                    {
                        Idx = rdr["idx"] == DBNull.Value ? null : (Int64?)rdr["idx"],
                        Entity_Lock = rdr["entity_lock"] == DBNull.Value ? null : (Int32?)rdr["entity_lock"],
                        Created = rdr["created"] == DBNull.Value ? null : (DateTime?)rdr["created"],
                        Modified = rdr["modified"] == DBNull.Value ? null : (DateTime?)rdr["modified"],
                        Client_Id = rdr["client_id"] == DBNull.Value ? null : (Int64?)rdr["client_Id"],
                        Client_Ip = rdr["client_ip"].ToString(),
                        Order_No = rdr["order_no"].ToString(),
                        Ship_To_Code = rdr["ship_to_code"].ToString(),
                        Ship_Name = rdr["ship_name"].ToString(),
                        Delivery_Priority = rdr["delivery_priority"] == DBNull.Value ? null : (Int32?)rdr["delivery_priority"],
                        Delivery_Date = rdr["delivery_date"] == DBNull.Value ? null : (DateTime?)rdr["delivery_date"],
                        Item_Code = rdr["item_code"].ToString(),
                        Batch_Number = rdr["batch_number"].ToString(),
                        Request_Qty = rdr["request_qty"] == DBNull.Value ? null : (decimal?)rdr["request_qty"],
                        Status = rdr["status"] == DBNull.Value ? null : (Int32?)rdr["status"],
                        Error_Code = rdr["error_code"].ToString(),
                        Movement_Type = rdr["movement_type"].ToString(),
                        Movement_Reason = rdr["movement_reason"].ToString(),
                        To_No = rdr["to_no"].ToString(),
                        To_Line = rdr["to_line"].ToString(),
                        Po_Header_Txt = rdr["po_header_txt"].ToString(),
                        Requisitioner = rdr["requisitioner"].ToString(),
                        Po_No = rdr["po_no"].ToString(),
                        Remark = rdr["remark"].ToString(),
                        Doc_Ref = rdr["doc_ref"].ToString(),
                        Created_By = rdr["created_by"].ToString(),
                        Created_Date = rdr["created_date"] == DBNull.Value ? null : (DateTime?)rdr["created_date"],
                        Update_By = rdr["update_by"].ToString(),
                        Update_Date = rdr["update_date"] == DBNull.Value ? null : (DateTime?)rdr["update_date"],
                        Order_Line = rdr["order_line"].ToString(),
                        Stock_Consign = rdr["stock_consign"].ToString(),
                        Site = rdr["site"].ToString(),
                        Storage_Location = rdr["storage_location"].ToString(),
                        Warehouse = rdr["warehouse"].ToString(),
                        Item_Name = rdr["itemdesctiption"].ToString(),
                        Tracking_Number = rdr["tracking_number"].ToString(),
                        Su_No = rdr["su_no"].ToString(),
                        Pallet_No = rdr["pallet_no"].ToString(),
                        Stock_Qty = rdr["stock_qty"] == DBNull.Value ? null : (decimal?)rdr["stock_qty"],
                        Transfer_Qty = rdr["transfer_qty"] == DBNull.Value ? null : (decimal?)rdr["transfer_qty"],
                        Ref_No = rdr["ref_no"].ToString(),
                        Ref_Line = rdr["ref_line"].ToString(),
                        Unit = rdr["unit"].ToString(),
                        Vendor_Code = rdr["vendor_code"].ToString(),
                        Batch_No = rdr["batch_no"].ToString(),
                        DisRequest_Qty = rdr["disrequest_qty"] == DBNull.Value ? null : (decimal?)rdr["disrequest_qty"],
                        DisStock_Qty = rdr["disstock_qty"] == DBNull.Value ? null : (decimal?)rdr["disstock_qty"],
                        DisTransfer_Qty = rdr["distransfer_qty"] == DBNull.Value ? null : (decimal?)rdr["distransfer_qty"]
                    };
                    lstobj.Add(objrd);
                }
                con.Close();
            }
            return lstobj;
        }

        public string GetRunnning(string sSeq, Int32 iPad)
        {
            string sRunning = "";
            Int32? iRet =0 ;
            
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                StringBuilder sqlQurey = new StringBuilder();
                sqlQurey.AppendLine("select _retchk, _retrunning from public.fuc_create_running(:sSeq , :iPad);");
                NpgsqlCommand cmd = new NpgsqlCommand(sqlQurey.ToString(), con)
                {
                    CommandType = CommandType.Text
                };
                cmd.Parameters.AddWithValue(":sSeq", sSeq);
                cmd.Parameters.AddWithValue(":iPad", iPad);

                con.Open();
                NpgsqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    iRet = rdr["_retchk"] == DBNull.Value ? null : (Int32?)rdr["_retchk"];
                    sRunning = rdr["_retrunning"].ToString();
                }
                con.Close();
            }
            return sRunning;
        }

        public Boolean CreateBatchOrder(DateTime deliverydate,Int32 deliveryprio,string orderno, string shiptocode, string sSeq)
        {
            Boolean bRet = false;
            string sRet = "";
            Int32? iRet = 0;
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                StringBuilder sqlQurey = new StringBuilder();
                sqlQurey.AppendLine("select _retchk, _retmsg from public.fuc_create_batchorder(:deliverydate , :deliveryprio, :orderno, :shiptocode, :sSeq);");
                NpgsqlCommand cmd = new NpgsqlCommand(sqlQurey.ToString(), con)
                {
                    CommandType = CommandType.Text
                };
                cmd.Parameters.AddWithValue(":deliverydate", NpgsqlDbType.Timestamp, deliverydate);
                cmd.Parameters.AddWithValue(":deliveryprio", NpgsqlDbType.Integer,  deliveryprio);
                cmd.Parameters.AddWithValue(":orderno", NpgsqlDbType.Varchar,orderno);
                cmd.Parameters.AddWithValue(":shiptocode", NpgsqlDbType.Varchar, shiptocode);
                cmd.Parameters.AddWithValue(":sSeq", NpgsqlDbType.Varchar, sSeq);

                con.Open();
                try
                {
                    NpgsqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        iRet = rdr["_retchk"] == DBNull.Value ? null : (Int32?)rdr["_retchk"];
                        sRet = rdr["_retmsg"].ToString();
                    }
                    con.Close();
                    if (iRet == 1)
                    {
                        bRet = true;
                    }
                }
                catch (Exception e)
                {
                    string str = e.Message.ToString(); 
                }
                   
                    
               
            }
            return bRet;
        }

        public Boolean CreateBatchSetting (string sSeq, Int32 istation)
        {
            Boolean bRet = false;
            string sRet = "";
            Int32? iRet = 0;
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                StringBuilder sqlQurey = new StringBuilder();
                sqlQurey.AppendLine("select _retchk, _retmsg from public.fuc_create_batchsetting(:sSeq , :istation);");
                NpgsqlCommand cmd = new NpgsqlCommand(sqlQurey.ToString(), con)
                {
                    CommandType = CommandType.Text
                };
                cmd.Parameters.AddWithValue(":sSeq", NpgsqlDbType.Varchar, sSeq);
                cmd.Parameters.AddWithValue(":istation", NpgsqlDbType.Integer, istation);

                con.Open();
                NpgsqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    iRet = rdr["_retchk"] == DBNull.Value ? null : (Int32?)rdr["_retchk"];
                    sRet = rdr["_retmsg"].ToString();
                }
                con.Close();
                if (iRet == 1)
                {
                    bRet = true;
                }
            }
            return bRet;
        }

        public Boolean StartBatchsetting(string sSeq, Int32 istation)
        {
            Boolean bRet = false;
            string sRet = "";
            Int32? iRet = 0;
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                StringBuilder sqlQurey = new StringBuilder();
                sqlQurey.AppendLine("select _retchk, _retmsg from public.fuc_start_batchsetting(:sSeq , :istation);");
                NpgsqlCommand cmd = new NpgsqlCommand(sqlQurey.ToString(), con)
                {
                    CommandType = CommandType.Text
                };
                cmd.Parameters.AddWithValue(":sSeq", NpgsqlDbType.Varchar, sSeq);
                cmd.Parameters.AddWithValue(":istation", NpgsqlDbType.Integer, istation);

                con.Open();
                NpgsqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    iRet = rdr["_retchk"] == DBNull.Value ? null : (Int32?)rdr["_retchk"];
                    sRet = rdr["_retmsg"].ToString();
                }
                con.Close();
                if (iRet == 1)
                {
                    bRet = true;
                }
            }
            return bRet;
        }

        public Boolean RollbackBatch(string sSeq)
        {
            Boolean bRet = false;
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                StringBuilder sqlQurey = new StringBuilder();
                sqlQurey.AppendLine("delete from public.sap_batchsetting where batch_no = :batch_no1 ;");
                sqlQurey.AppendLine("delete from public.sap_batchorder where batch_no = :batch_no2 ;");

                NpgsqlCommand cmd = new NpgsqlCommand(sqlQurey.ToString(), con)
                {
                    CommandType = CommandType.Text
                };
                cmd.Parameters.AddWithValue(":batch_no1", NpgsqlDbType.Varchar, sSeq);
                cmd.Parameters.AddWithValue(":batch_no2", NpgsqlDbType.Varchar, sSeq);

                con.Open();
                cmd.ExecuteNonQuery();
                bRet = true;
                con.Close();
            }
            return bRet;
        }

        public IEnumerable<Oub_CustomerReturn> GetAllCustomerReturn()
        {
            List<Oub_CustomerReturn> lstobj = new List<Oub_CustomerReturn>();
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                StringBuilder sql = new StringBuilder();

                sql.AppendLine("SELECT  c.package_id");
                sql.AppendLine(",c.material_code");
                sql.AppendLine(", c.material_description");
                sql.AppendLine(", c.unit");
                sql.AppendLine(", c.customer_code");
                sql.AppendLine(", 1 as quantity");
                sql.AppendLine(", c.matelement as dnno");
                sql.AppendLine(", c.finished_product as lotno");
                sql.AppendLine(", c.finished_product_description  as batchno");
                sql.AppendLine(", c.created::date as created");
                sql.AppendLine(", c.job");
                sql.AppendLine(", c.pallet_no");

                sql.AppendLine("FROM wms.oub_deliveryorder_go c");
                /*
                sql.AppendLine("WHERE NOT EXISTS");
                sql.AppendLine("(SELECT 1");
                sql.AppendLine("FROM wms.inv_stock_go p");
                //sql.AppendLine("WHERE p.pallettag = c.package_id)");
                sql.AppendLine("WHERE p.pono = c.matelement AND p.pallettag = c.package_id AND p.docno =c.finished_product AND p.docnote = c.finished_product_description )");
                sql.AppendLine("AND c.efstatus < 5");
                */
                sql.AppendLine("WHERE c.efstatus < 5");
                sql.AppendLine("GROUP BY");
                sql.AppendLine("package_id, material_code, material_description, unit,  customer_code, dnno, lotno, batchno, created, job, pallet_no");
                sql.AppendLine(";");

                NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), con)
                {
                    CommandType = CommandType.Text
                };
                con.Open();
                NpgsqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Oub_CustomerReturn objrd = new Oub_CustomerReturn
                    {
                        Customer_code= rdr["customer_code"].ToString(),
                        Package_id = rdr["package_id"].ToString(),
                        Material_code = rdr["material_code"].ToString(),
                        Material_description = rdr["material_description"].ToString(),
                        Unit = rdr["unit"].ToString(),
                        Dnno= rdr["dnno"].ToString(),
                        Lotno = rdr["lotno"].ToString(),
                        Batchno = rdr["batchno"].ToString(),
                        Sono = rdr["job"].ToString(),
                        Quantity= rdr["quantity"] == DBNull.Value ? null : (int?)rdr["quantity"],
                        Palletno = rdr["pallet_no"].ToString(),
                        Sodate  = rdr["created"] == DBNull.Value ? null : (DateTime?)rdr["created"]

                };
                    lstobj.Add(objrd);
                }
                con.Close();
            }
            return lstobj;
        }


        public Boolean UpdateCustomerReturn(string Spallet)
        {
            Boolean bRet = false;
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                StringBuilder sqlQurey = new StringBuilder();
                sqlQurey.AppendLine("UPDATE wms.oub_deliveryorder_go");
                sqlQurey.AppendLine("SET efstatus = 5");
                sqlQurey.AppendLine("WHERE pallteno = @pallteno");
                sqlQurey.AppendLine("AND efstatus < 5");
                NpgsqlCommand cmd = new NpgsqlCommand(sqlQurey.ToString(), con)
                {
                    CommandType = CommandType.Text
                };
                cmd.Parameters.AddWithValue("@pallteno", NpgsqlDbType.Varchar, Spallet);
                con.Open();
                cmd.ExecuteNonQuery();
                bRet = true;
                con.Close();
            }
            return bRet;
        }


        public IEnumerable<Set_Workstation> GetAllWorkstations()
        {
            List< Set_Workstation> lstobj = new List<Set_Workstation>();
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                NpgsqlCommand cmd = new NpgsqlCommand("select idx, created, entity_lock, modified, client_id, client_ip, stcode, stdesc, stref " +
                    "FROM public.set_workstation  " +
                    "order by idx", con)
                {
                    CommandType = CommandType.Text
                };
                con.Open();
                NpgsqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Set_Workstation objrd = new Set_Workstation
                    {
                        Idx = rdr["idx"] == DBNull.Value ? null : (Int64?)rdr["idx"],
                        Entity_Lock = rdr["entity_lock"] == DBNull.Value ? null : (Int32?)rdr["entity_lock"],
                        Created = rdr["created"] == DBNull.Value ? null : (DateTime?)rdr["created"],
                        Modified = rdr["modified"] == DBNull.Value ? null : (DateTime?)rdr["modified"],
                        Client_Id = rdr["client_Id"] == DBNull.Value ? null : (Int64?)rdr["client_Id"],
                        Client_Ip = rdr["client_Ip"].ToString(),
                        Stcode = rdr["stcode"].ToString(),
                        Stdesc = rdr["stdesc"].ToString(),
                        Stref = rdr["stref"].ToString()
                    };
                    lstobj.Add(objrd);
                }
                con.Close();
            }
            return lstobj;
        }

        public bool CancelOrderPicking(string suno, string orderno)
        {
            bool bret = false;
            try
            {
                using NpgsqlConnection con = new NpgsqlConnection(connectionString);
                StringBuilder sql = new StringBuilder();

                sql.AppendLine("Update api.postasrsorders");
                sql.AppendLine("Set typor ='PICK' ,efstatus =0");
                sql.AppendLine("Where lenum = @lenum");
                sql.AppendLine("And karor = @karor");
                sql.AppendLine(";");

                sql.AppendLine("update wms.inv_stock_go");
                sql.AppendLine("set allocatequantity = 0.0");
                sql.AppendLine("Where pallettag = @pallettag");
                sql.AppendLine(";");

                sql.AppendLine("Update public.sap_stock");
                sql.AppendLine("Set total_qty = qty");
                sql.AppendLine("Where su_no = @su_no ");
                sql.AppendLine(";");

                sql.AppendLine("Delete from public.sap_storeout");
                sql.AppendLine("Where su_no = @su_noout");
                sql.AppendLine("And order_no = @order_no");
                sql.AppendLine(";");

                sql.AppendLine("Delete FROM wms.oub_deliveryorder_go");
                sql.AppendLine("Where package_id = @package_id");
                sql.AppendLine("And job = @job");
                sql.AppendLine(";");

                NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), con)
                {
                    CommandType = CommandType.Text
                };

                cmd.Parameters.AddWithValue("@lenum", suno);
                cmd.Parameters.AddWithValue("@karor", orderno);
                cmd.Parameters.AddWithValue("@pallettag", suno);
                cmd.Parameters.AddWithValue("@su_no", suno);
                cmd.Parameters.AddWithValue("@su_noout", suno);
                cmd.Parameters.AddWithValue("@order_no", orderno);
                cmd.Parameters.AddWithValue("@package_id", suno);
                cmd.Parameters.AddWithValue("@job", orderno);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                bret = true;
            }
            catch
            {

            }
            return bret;
        }


    }
}
