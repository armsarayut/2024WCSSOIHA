using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoWMS.Server.Data;
using GoWMS.Server.Models;
using GoWMS.Server.Models.Wcs;
using GoWMS.Server.Models.Hagv;

namespace GoWMS.Server.Controllers
{
    public class AgvService
    {
        readonly AgvDAL objDAL = new AgvDAL();

        public List<Tas_Agvworks> GetAllTaskAgv()
        {
            List<Tas_Agvworks> retlist = objDAL.GetAllTaskAgv().ToList();
            return retlist;
        }

        public List<Functionreturn> CancleTaskAGV(string pallet)
        {
            List<Functionreturn> retlist = objDAL.CancleTaskAGV(pallet).ToList();
            return retlist;
        }

        public List<Agv_Status> GetAllAgvStatusDesc()
        {
            return objDAL.GetAllAgvStatusDesc();
        }

        public System.Data.DataTable GetQueryAgvStatusApiName()
        {
            return objDAL.GetQueryAgvStatusApiName();
        }

        public List<Vrptqueueloadtimeagv> GetAllReportTaskAgv()
        {
            List<Vrptqueueloadtimeagv> retlist = objDAL.GetAllReportTaskAgv().ToList();
            return retlist;
        }

        public List<RptTaskHourCount> GetAllReportTaskPerHourAgv()
        {
            List<RptTaskHourCount> retlist = objDAL.GetAllReportTaskPerHourAgv().ToList();
            return retlist;
        }

    }
}
