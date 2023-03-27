using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;
using GoWMS.Server.Controllers;
using GoWMS.Server.Models;
using GoWMS.Server.Models.Inb;
using NpgsqlTypes;
using System.Text;

namespace GoWMS.Server.Data
{
    public class InbDAL
    {
        readonly private string connectionString = ConnGlobals.GetConnLocalDBPG();


        public IEnumerable<Inb_Goodreceipt_Go> GetAllOubGoodreceiptGo()
        {
            List<Inb_Goodreceipt_Go> lstobj = new List<Inb_Goodreceipt_Go>();
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                StringBuilder sql = new StringBuilder();

                //sql.AppendLine("select * ");
                //sql.AppendLine("from wms.inb_goodreceipt_go");
                //sql.AppendLine("where efstatus <> @efstatus");
                //sql.AppendLine("order by efidx");

                sql.AppendLine("SELECT efidx, efstatus, created, modified, innovator, device,lenum as Package_ID, lenum as Roll_ID");
                sql.AppendLine(",SUBSTRING (matnr FROM 2) as Material_Code, '-' as Material_Description");
                sql.AppendLine(",created as Receiving_Date, CAST(matqty AS DECIMAL) as  GR_Quantity");
                sql.AppendLine(",null as Unit,1.00 as GR_Quantity_KG");
                sql.AppendLine(",lgnum as  WH_Code, lgnum as Warehouse, matnr as Job, matbatch as Job_Code, typor, karor");
                sql.AppendLine("FROM api.postasrsorders");
                sql.AppendLine("WHERE typor=@typor");


                sql.AppendLine("SELECT t1.efidx , t1.efstatus, t1.created, t1.modified, t1.innovator, t1.device");
                sql.AppendLine(", t1.pono, t1.pallettag, t1.itemtag, t1.itemcode, t1.itemname, t1.itembar, t1.unit");
                sql.AppendLine(", t1.weightunit, t1.quantity, t1.weight, t1.lotno, t1.totalquantity, t1.totalweight");
                sql.AppendLine(", t1.docno, t1.docby, t1.docdate, t1.docnote, t1.grnrefer, t1.grntime, t1.grtime");
                sql.AppendLine(", t1.grtype, t1.pallteno, t1.palltmapkey, t1.storagetime, t1.storageno");
                sql.AppendLine(", t1.storagearea, t1.storagebin, t1.gnrefer, t1.allocatequantity, t1.allocateweight");
                sql.AppendLine(",t0.efidx as t0efidx, t0.efstatus ast0efstatus, t0.created as t0created, t0.modified as t0modified, t0.innovator as t0innovator, t0.device as t0device");
                sql.AppendLine(",t0.lenum as Package_ID, t0.lenum as Roll_ID");
                sql.AppendLine(",t0.matnr as Material_Code, '-' as Material_Description");
                //sql.AppendLine(",SUBSTRING (t0.matnr FROM 2) as Material_Code, '-' as Material_Description");
                sql.AppendLine(",t0.created as Receiving_Date, CAST(t0.matqty AS DECIMAL) as  GR_Quantity");
                sql.AppendLine(",null as Unit,1.00 as GR_Quantity_KG");
                sql.AppendLine(",t0.lgnum as  WH_Code, t0.lgnum as Warehouse, t0.matnr as Job, t0.matbatch as Job_Code, t0.typor, t0.karor");
                sql.AppendLine("FROM api.postasrsorders t0");
                sql.AppendLine("LEFT JOIN wms.vinv_stock_go_readypick t1");
                sql.AppendLine("ON t0.lenum = t1.pallettag");
                sql.AppendLine("WHERE t0.typor=@typor");
                sql.AppendLine("order by itemcode ASC, docdate ASC, pallettag ASC");






                NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), con)
                {
                    CommandType = CommandType.Text
                };
                cmd.Parameters.AddWithValue("@typor", "PICK");

                con.Open();
                NpgsqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Inb_Goodreceipt_Go objrd = new Inb_Goodreceipt_Go
                    {
                        //Package_ID = rdr["Package_ID"].ToString(),
                        //Roll_ID = rdr["Roll_ID"].ToString(),
                        //Material_Code = rdr["Material_Code"].ToString(),
                        //Material_Description = rdr["Material_Description"].ToString(),
                        //Receiving_Date = rdr["Receiving_Date"] == DBNull.Value ? null : (DateTime?)rdr["Receiving_Date"],
                        //GR_Quantity = rdr["GR_Quantity"] == DBNull.Value ? null : (decimal?)rdr["GR_Quantity"],
                        //Unit = rdr["Unit"].ToString(),
                        //GR_Quantity_KG = rdr["GR_Quantity_KG"] == DBNull.Value ? null : (decimal?)rdr["GR_Quantity_KG"],
                        //WH_Code = rdr["WH_Code"].ToString(),
                        //Warehouse = rdr["Warehouse"].ToString(),
                        //Location = rdr["Location"].ToString(),
                        //Document_Number = rdr["Document_Number"].ToString(),
                        //Job = rdr["Job"].ToString(),
                        //Job_Code = rdr["Job_Code"].ToString()

                        Efidx = rdr["efidx"] == DBNull.Value ? null : (Int64?)rdr["efidx"],
                        Efstatus = rdr["efstatus"] == DBNull.Value ? null : (Int32?)rdr["efstatus"],
                        Created = rdr["created"] == DBNull.Value ? null : (DateTime?)rdr["created"],
                        Modified = rdr["modified"] == DBNull.Value ? null : (DateTime?)rdr["modified"],
                        Innovator = rdr["innovator"] == DBNull.Value ? null : (Int64?)rdr["innovator"],
                        Device = rdr["device"].ToString(),

                        Pono = rdr["karor"].ToString(),
                        Pallettag = rdr["Package_ID"].ToString(),
                        Itemtag = rdr["Roll_ID"].ToString(),
                        Itemcode = rdr["Material_Code"].ToString(),
                        Itemname = rdr["Material_Description"].ToString(),
                        Itembar = rdr["Package_ID"].ToString(),
                        Unit = rdr["Unit"].ToString(),
                        Weightunit = null,
                        Quantity = rdr["GR_Quantity"] == DBNull.Value ? null : (decimal?)rdr["GR_Quantity"],
                        Weight = 1,
                        Lotno = rdr["Job_Code"].ToString(),
                        Totalquantity = rdr["GR_Quantity"] == DBNull.Value ? null : (decimal?)rdr["GR_Quantity"],
                        Totalweight = rdr["GR_Quantity_KG"] == DBNull.Value ? null : (decimal?)rdr["GR_Quantity_KG"],
                        Docno = rdr["Job_Code"].ToString(),
                        Docby = rdr["WH_Code"].ToString(),
                        Docdate = rdr["Receiving_Date"] == DBNull.Value ? null : (DateTime?)rdr["Receiving_Date"],
                        Docnote = rdr["Warehouse"].ToString(),
                        Grntype = rdr["typor"].ToString()
                    };
                    lstobj.Add(objrd);
                }
                con.Close();
            }
            return lstobj;
        }

        public IEnumerable<Inb_Goodreceipt_Go> GetAllInbGoodreceiptGo()
        {
            List<Inb_Goodreceipt_Go> lstobj = new List<Inb_Goodreceipt_Go>();
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                StringBuilder sql = new StringBuilder();

                //sql.AppendLine("select * ");
                //sql.AppendLine("from wms.inb_goodreceipt_go");
                //sql.AppendLine("where efstatus <> @efstatus");
                //sql.AppendLine("order by efidx");

                sql.AppendLine("SELECT subQ.*");
                sql.AppendLine(", CASE WHEN t3.weightnet ISNULL");
                sql.AppendLine("THEN subQ.GR_Quantity");
                sql.AppendLine("ELSE subQ.GR_Quantity * t3.weightnet");
                sql.AppendLine("END AS disgr_quantity");

                sql.AppendLine(", t3.itemname as itemdesctiption");


                sql.AppendLine("FROM (");
                sql.AppendLine("SELECT efidx, efstatus, created, modified, innovator, device,lenum as Package_ID, lenum as Roll_ID");
                sql.AppendLine(",matnr as Material_Code, '-' as Material_Description");
                //sql.AppendLine(",SUBSTRING (matnr FROM 2) as Material_Code, '-' as Material_Description");
                sql.AppendLine(",created as Receiving_Date, CAST(matqty AS DECIMAL) as  GR_Quantity");
                sql.AppendLine(",null as Unit,1.00 as GR_Quantity_KG");
                sql.AppendLine(",lgnum as  WH_Code, lgnum as Warehouse, matnr as Job, matbatch as Job_Code, typor, karor");
                sql.AppendLine("FROM api.postasrsorders");
                sql.AppendLine("WHERE typor=@typor");
                sql.AppendLine(")subQ");
                sql.AppendLine("LEFT JOIN wms.mas_item_go t3");
                sql.AppendLine("ON subQ.Material_Code=t3.itemcode");

                NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), con)
                {
                    CommandType = CommandType.Text
                };
                cmd.Parameters.AddWithValue("@typor", "PUT");

                con.Open();
                NpgsqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Inb_Goodreceipt_Go objrd = new Inb_Goodreceipt_Go
                    {
                        //Package_ID = rdr["Package_ID"].ToString(),
                        //Roll_ID = rdr["Roll_ID"].ToString(),
                        //Material_Code = rdr["Material_Code"].ToString(),
                        //Material_Description = rdr["Material_Description"].ToString(),
                        //Receiving_Date = rdr["Receiving_Date"] == DBNull.Value ? null : (DateTime?)rdr["Receiving_Date"],
                        //GR_Quantity = rdr["GR_Quantity"] == DBNull.Value ? null : (decimal?)rdr["GR_Quantity"],
                        //Unit = rdr["Unit"].ToString(),
                        //GR_Quantity_KG = rdr["GR_Quantity_KG"] == DBNull.Value ? null : (decimal?)rdr["GR_Quantity_KG"],
                        //WH_Code = rdr["WH_Code"].ToString(),
                        //Warehouse = rdr["Warehouse"].ToString(),
                        //Location = rdr["Location"].ToString(),
                        //Document_Number = rdr["Document_Number"].ToString(),
                        //Job = rdr["Job"].ToString(),
                        //Job_Code = rdr["Job_Code"].ToString()

                        Efidx = rdr["efidx"] == DBNull.Value ? null : (Int64?)rdr["efidx"],
                        Efstatus = rdr["efstatus"] == DBNull.Value ? null : (Int32?)rdr["efstatus"],
                        Created = rdr["created"] == DBNull.Value ? null : (DateTime?)rdr["created"],
                        Modified = rdr["modified"] == DBNull.Value ? null : (DateTime?)rdr["modified"],
                        Innovator = rdr["innovator"] == DBNull.Value ? null : (Int64?)rdr["innovator"],
                        Device = rdr["device"].ToString(),

                        Pono = rdr["karor"].ToString(),
                        Pallettag = rdr["Package_ID"].ToString(),
                        Itemtag = rdr["Roll_ID"].ToString(),
                        Itemcode = rdr["Material_Code"].ToString(),
                        Itemname = rdr["itemdesctiption"].ToString(),
                        Itembar = rdr["Package_ID"].ToString(),
                        Unit = rdr["Unit"].ToString(),
                        Weightunit = null,
                        Quantity = rdr["GR_Quantity"] == DBNull.Value ? null : (decimal?)rdr["GR_Quantity"],
                        Weight = 1,
                        Lotno = rdr["Job_Code"].ToString(),
                        Totalquantity = rdr["GR_Quantity"] == DBNull.Value ? null : (decimal?)rdr["GR_Quantity"],
                        Totalweight = rdr["GR_Quantity_KG"] == DBNull.Value ? null : (decimal?)rdr["GR_Quantity_KG"],
                        Docno = rdr["Job_Code"].ToString(),
                        Docby = rdr["WH_Code"].ToString(),
                        Docdate = rdr["Receiving_Date"] == DBNull.Value ? null : (DateTime?)rdr["Receiving_Date"],
                        Docnote = rdr["Warehouse"].ToString(),
                        Grntype = rdr["typor"].ToString(),
                        DisQuantity = rdr["disgr_quantity"] == DBNull.Value ? null : (decimal?)rdr["disgr_quantity"],
                        DisTotalquantity = rdr["disgr_quantity"] == DBNull.Value ? null : (decimal?)rdr["disgr_quantity"],
                    };
                    lstobj.Add(objrd);
                }
                con.Close();
            }
            return lstobj;
        }


        public async Task<long> GetCountInbGoodreceiptGo()
        {

            long lRet = 0;
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("SELECT count(subQ.palletcode) FROM (");

                sql.AppendLine("SELECT palletcode");
                sql.AppendLine("FROM api.posttaskorders");
                sql.AppendLine("WHERE tasktype=@typor");
                sql.AppendLine("AND efstatus <= @efstatus");
                sql.AppendLine("GROUP BY palletcode");
                sql.AppendLine(")subQ");



                NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), con)
                {
                    CommandType = CommandType.Text
                };

                cmd.Parameters.AddWithValue("@typor", "01");
                cmd.Parameters.AddWithValue("@efstatus", 0);

                con.Open();
                lRet = (long)(cmd.ExecuteScalar());

                con.Close();
            }

            return lRet;

        }

        public async Task<long> GetCountOutGoodreceiptGo()
        {

            long lRet = 0;
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("SELECT count(subQ.palletcode) FROM (");

                sql.AppendLine("SELECT palletcode");
                sql.AppendLine("FROM api.posttaskorders");
                sql.AppendLine("WHERE tasktype=@typor");
                sql.AppendLine("AND efstatus <= @efstatus");
                sql.AppendLine("GROUP BY palletcode");
                sql.AppendLine(")subQ");

                NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), con)
                {
                    CommandType = CommandType.Text
                };

                cmd.Parameters.AddWithValue("@typor", "05");
                cmd.Parameters.AddWithValue("@efstatus", 0);

                con.Open();
                lRet = (long)(cmd.ExecuteScalar());

                con.Close();
            }

            return lRet;

        }



        public IEnumerable<Inb_Goodreceive_Go> GetAllInbGoodreceiveGo()
        {
            List<Inb_Goodreceive_Go> lstobj = new List<Inb_Goodreceive_Go>();
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                StringBuilder sql = new StringBuilder();


                sql.AppendLine("SELECT subQ.*");
                sql.AppendLine(", CASE WHEN t3.weightnet ISNULL");
                sql.AppendLine("THEN subQ.quantity");
                sql.AppendLine("ELSE subQ.quantity * t3.weightnet");
                sql.AppendLine("END AS disquantity");
                sql.AppendLine(", CASE WHEN t3.weightnet ISNULL");
                sql.AppendLine("THEN subQ.weight");
                sql.AppendLine("ELSE subQ.weight * t3.weightnet");
                sql.AppendLine("END AS disweight");
                sql.AppendLine(", subQ.lotno");
                sql.AppendLine(", CASE WHEN t3.weightnet ISNULL");
                sql.AppendLine("THEN subQ.totalquantity");
                sql.AppendLine("ELSE subQ.totalquantity * t3.weightnet");
                sql.AppendLine("END AS Distotalquantity");
                sql.AppendLine(", CASE WHEN t3.weightnet ISNULL");
                sql.AppendLine("THEN subQ.totalweight");
                sql.AppendLine("ELSE subQ.totalweight * t3.weightnet");
                sql.AppendLine("END AS distotalweight");
                sql.AppendLine(", t3.itemname as itemdesctiption");
                sql.AppendLine("FROM (");
                sql.AppendLine("select * ");
                sql.AppendLine("from wms.inb_goodreceive_go");
                sql.AppendLine("where efstatus <> @efstatus");
                sql.AppendLine(")subQ");
                sql.AppendLine("LEFT JOIN wms.mas_item_go t3");
                sql.AppendLine("ON subQ.itemcode=t3.itemcode");
                sql.AppendLine("order by efidx");


                //sql.AppendLine("select * ");
                //sql.AppendLine("from wms.inb_goodreceive_go");
                //sql.AppendLine("where efstatus <> @efstatus");
                //sql.AppendLine("order by efidx");

                NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), con)
                {
                    CommandType = CommandType.Text
                };


                cmd.Parameters.AddWithValue("@efstatus", 3);

                con.Open();
                NpgsqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Inb_Goodreceive_Go objrd = new Inb_Goodreceive_Go
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
                        DisQuantity = rdr["disquantity"] == DBNull.Value ? null : (decimal?)rdr["disquantity"],
                        DisWeight = rdr["disweight"] == DBNull.Value ? null : (decimal?)rdr["disweight"],
                        DisTotalquantity = rdr["distotalquantity"] == DBNull.Value ? null : (decimal?)rdr["distotalquantity"],
                        DisTotalweight = rdr["distotalweight"] == DBNull.Value ? null : (decimal?)rdr["distotalweight"]

                    };
                    lstobj.Add(objrd);
                }
                con.Close();
            }
            return lstobj;
        }

        public IEnumerable<Inb_Putaway_Go> GetAllInbPutawayGo()
        {
            List<Inb_Putaway_Go> lstobj = new List<Inb_Putaway_Go>();
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("select * ");
                sql.AppendLine("from wms.inb_putaway_go");
                sql.AppendLine("order by efidx");


                NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), con)
                {
                    CommandType = CommandType.Text
                };
                con.Open();
                NpgsqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Inb_Putaway_Go objrd = new Inb_Putaway_Go
                    {
                        Efidx = rdr["efidx"] == DBNull.Value ? null : (Int64?)rdr["efidx"],
                        Efstatus = rdr["efstatus"] == DBNull.Value ? null : (Int32?)rdr["efstatus"],
                        Created = rdr["created"] == DBNull.Value ? null : (DateTime?)rdr["created"],
                        Modified = rdr["modified"] == DBNull.Value ? null : (DateTime?)rdr["modified"],
                        Innovator = rdr["innovator"] == DBNull.Value ? null : (Int64?)rdr["innovator"],
                        Device = rdr["device"].ToString(),
                        Palletmapkey = rdr["palletmapkey"].ToString(),
                        Puttype = rdr["puttype"].ToString(),
                        Palletno = rdr["palletno"].ToString(),
                        Started = rdr["started"] == DBNull.Value ? null : (DateTime?)rdr["started"],
                        Loadted = rdr["loadted"] == DBNull.Value ? null : (DateTime?)rdr["loadted"],
                        Completed = rdr["completed"] == DBNull.Value ? null : (DateTime?)rdr["completed"],
                        Storagetime = rdr["storagetime"] == DBNull.Value ? null : (DateTime?)rdr["storagetime"],
                        Storageno = rdr["storageno"].ToString(),
                        Storagearea = rdr["storagearea"].ToString(),
                        Storagebin = rdr["storagebin"].ToString()
                    };
                    lstobj.Add(objrd);
                }
                con.Close();
            }
            return lstobj;
        }


        public IEnumerable<Inb_Putaway_Go> GetAllInbPutawayGoBypallet(string pallet)
        {
            List<Inb_Putaway_Go> lstobj = new List<Inb_Putaway_Go>();
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("select * ");
                sql.AppendLine("where palletno = @palletno ");
                sql.AppendLine("order by efidx");


                NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), con)
                {
                    CommandType = CommandType.Text
                };
                con.Open();
                cmd.Parameters.AddWithValue("@palletno", NpgsqlDbType.Varchar, pallet);

                NpgsqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Inb_Putaway_Go objrd = new Inb_Putaway_Go
                    {
                        Efidx = rdr["efidx"] == DBNull.Value ? null : (Int64?)rdr["efidx"],
                        Efstatus = rdr["efstatus"] == DBNull.Value ? null : (Int32?)rdr["efstatus"],
                        Created = rdr["created"] == DBNull.Value ? null : (DateTime?)rdr["created"],
                        Modified = rdr["modified"] == DBNull.Value ? null : (DateTime?)rdr["modified"],
                        Innovator = rdr["innovator"] == DBNull.Value ? null : (Int64?)rdr["innovator"],
                        Device = rdr["device"].ToString(),
                        Palletmapkey = rdr["palletmapkey"].ToString(),
                        Puttype = rdr["puttype"].ToString(),
                        Palletno = rdr["palletno"].ToString(),
                        Started = rdr["started"] == DBNull.Value ? null : (DateTime?)rdr["started"],
                        Loadted = rdr["loadted"] == DBNull.Value ? null : (DateTime?)rdr["loadted"],
                        Completed = rdr["completed"] == DBNull.Value ? null : (DateTime?)rdr["completed"],
                        Storagetime = rdr["storagetime"] == DBNull.Value ? null : (DateTime?)rdr["storagetime"],
                        Storageno = rdr["storageno"].ToString(),
                        Storagearea = rdr["storagearea"].ToString(),
                        Storagebin = rdr["storagebin"].ToString()
                    };
                    lstobj.Add(objrd);
                }
                con.Close();
            }
            return lstobj;
        }

        public void SetStorageComplete(string pallet, string bin)
        {
            Int32? iRet = 0;
            string sRet = "Calling";
            NpgsqlConnection con = new NpgsqlConnection(connectionString);
            try
            {
                con.Open();
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("Call wms.poc_inb_storagebin(");
                sql.AppendLine("@spalletno, @sbin, @retchk, @retmsg)");
                NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), con)
                {
                    CommandType = CommandType.Text
                };

                cmd.Parameters.AddWithValue("@spalletno", pallet);
                cmd.Parameters.AddWithValue("@sbin", bin);
                cmd.Parameters.AddWithValue("@retchk", iRet);
                cmd.Parameters.AddWithValue("@retmsg", sRet);
                NpgsqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    iRet = rdr["retchk"] == DBNull.Value ? null : (Int32?)rdr["retchk"];
                    sRet = rdr["retmsg"].ToString();
                }
            }
            catch (NpgsqlException exp)
            {
                //Response.Write(exp.ToString());
            }
            finally
            {
                con.Close();
            }
        }

        public async Task<Int64> GetSumOrderAllInbGoodreceiptGo()
        {
            Int64 lRet = 0;
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                NpgsqlCommand cmd = new NpgsqlCommand("select count(*) " +
                    "from wms.inb_goodreceive_go " +
                    "where pallteno in (select palletno from wms.inb_putaway_go)", con)
                {
                    CommandType = CommandType.Text
                };
                con.Open();

                lRet= Convert.ToInt64(cmd.ExecuteScalar());
                
                con.Close();
            }

            return lRet;
        }

        public async Task<Int64> GetSumPalletAllInbGoodreceiptGo()
        {
            Int64 lRet = 0;
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("select count(*)");
                sql.AppendLine("from wms.inb_putaway_go");
                sql.AppendLine("where efstatus = @efstatus ");

                NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), con)
                {
                    CommandType = CommandType.Text
                };

                cmd.Parameters.AddWithValue("@efstatus", 0);

                con.Open();
                lRet = Convert.ToInt64(cmd.ExecuteScalar());

                con.Close();
            }

            return lRet;
        }


        public async Task<Int64> GetSumPalletAllOubGoodPickingGo()
        {
            Int64 lRet = 0;
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {

                StringBuilder sql = new StringBuilder();

                sql.AppendLine("select count(lpncode) ");
                sql.AppendLine("from wcs.tas_works");
                sql.AppendLine("where work_code=@work_code");
                sql.AppendLine("and work_status=@work_status");



                NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), con)
                {
                    CommandType = CommandType.Text
                };

                cmd.Parameters.AddWithValue("@work_code", "05");
                cmd.Parameters.AddWithValue("@work_status", "AVL");

                con.Open();
                lRet = Convert.ToInt64(cmd.ExecuteScalar());

                con.Close();
            }

            return lRet;
        }

        public async Task<Int64> GetSumOrderAllOubGoodPickingGo()
        {
            Int64 lRet = 0;
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("select count(*) ");
                sql.AppendLine("from public.sap_storeout");
                sql.AppendLine("where status < @status");


                NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), con)
                {
                    CommandType = CommandType.Text
                };
                cmd.Parameters.AddWithValue("@status", 3);

                con.Open();
                lRet = Convert.ToInt64(cmd.ExecuteScalar());

                con.Close();
            }

            return lRet;
        }


        public bool CancelReceivingOrdersBypack(string pallet, string pack)
        {
            bool bret=false;
            try
            {
                using NpgsqlConnection con = new NpgsqlConnection(connectionString);
                StringBuilder sql = new StringBuilder();

                sql.AppendLine("Delete From wms.inb_goodreceive_go");
                sql.AppendLine("Where pallettag = @Pack ");
                sql.AppendLine("and pallteno = @Pallet");
                sql.AppendLine(";");

                sql.AppendLine("Delete From public.sap_storein");
                sql.AppendLine("Where su_no = @su_no ");
                sql.AppendLine("and sap_su = @sap_su");
                sql.AppendLine(";");

                sql.AppendLine("Delete From wms.api_receivingorders_go");
                sql.AppendLine("Where package_id = @package_id");
                sql.AppendLine(";");


                NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), con)
                {
                    CommandType = CommandType.Text
                };

                cmd.Parameters.AddWithValue("@Pallet", pallet);
                cmd.Parameters.AddWithValue("@Pack", pack);
                cmd.Parameters.AddWithValue("@package_id", pack);

                cmd.Parameters.AddWithValue("@sap_su", pallet);
                cmd.Parameters.AddWithValue("@su_no", pack);
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


        public bool CancelReceivingOrdersByPallet(string pallet)
        {
            bool bret = false;
            try
            {
                using NpgsqlConnection con = new NpgsqlConnection(connectionString);
                StringBuilder sql = new StringBuilder();
                
              

                sql.AppendLine("Delete From wms.inb_goodreceive_go");
                sql.AppendLine("Where pallteno = @pallteno");
                sql.AppendLine(";");

                sql.AppendLine("Delete From public.sap_storein");
                sql.AppendLine("Where sap_su = @sap_su");
                sql.AppendLine(";");

                sql.AppendLine("Delete From wcs.tas_works");
                sql.AppendLine("Where lpncode = @lpncode ");
                sql.AppendLine(";");

                NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), con)
                {
                    CommandType = CommandType.Text
                };

                cmd.Parameters.AddWithValue("@pallteno", pallet);
                cmd.Parameters.AddWithValue("@sap_su", pallet);
                cmd.Parameters.AddWithValue("@lpncode", pallet);
     
                

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

        public bool CancelPutawayByPallet(long efidx)
        {
            bool bret = false;
            try
            {
                using NpgsqlConnection con = new NpgsqlConnection(connectionString);
                StringBuilder sql = new StringBuilder();

                sql.AppendLine("Delete From wms.inb_putaway_go");
                sql.AppendLine("Where efidx = @efidx");
                sql.AppendLine(";");


                NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), con)
                {
                    CommandType = CommandType.Text
                };

                cmd.Parameters.AddWithValue("@efidx", efidx);

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

        public bool CancelGrAPI(long idx)
        {
            bool bret = false;
            try
            {
                using NpgsqlConnection con = new NpgsqlConnection(connectionString);
                StringBuilder sql = new StringBuilder();

                sql.AppendLine("Delete From api.postasrsorders");
                sql.AppendLine("Where efidx = @efidx");
                sql.AppendLine(";");


                NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), con)
                {
                    CommandType = CommandType.Text
                };

                cmd.Parameters.AddWithValue("@efidx", idx);

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
