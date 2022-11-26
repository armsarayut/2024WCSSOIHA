using System;

namespace GoWMS.Server.Models.Api
{
    public class ApiKardexconfirmStatus
    {
        public string karor { get; set; }
        public string matnr { get; set; }
        public string lgnum { get; set; }
        public string lenum { get; set; }
        public string typor { get; set; }
        public string kxbin { get; set; }
        public string kquit2 { get; set; }
        public string msgid { get; set; }
        public string msgno { get; set; }
        public string msgty { get; set; }
        public string text { get; set; }
        public string failed_read { get; set; }
        public string failed_conf { get; set; }
        public string kardexconfirm_status { get; set; }
        public DateTime? created { get; set; }

        public DateTime? send_to_kardex { get; set; }
        public string backcolor { get; set; }
        public string focecolor { get; set; }

    }
}
