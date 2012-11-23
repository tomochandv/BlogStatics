using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TomochanClass;
using System.Data;

namespace ReportBlog.Dac
{
    public class Member
    {
        public int idx { get; set; }
        public string email { get; set; }
        public string pwd { get; set; }
        public DateTime intdate { get; set; }

        public int InsertMember(Member member)
        {
            int result = 0;

            string qry = string.Format("Insert into Member values('{0}','{1}',getdate(),'Y')", member.email, member.pwd);
            result = TomochanData.ExecuteNonQuery(qry);

            return result;
        }

        public int DeleteMember(Member member)
        {
            int result = 0;

            string qry = string.Format("DELETE FROM Member WHERE IDX= {0}", member.idx);
            result = TomochanData.ExecuteNonQuery(qry);

            return result;
        }

        public DataTable LoginMember(Member member)
        {
            string qry = string.Format("SELECT * FROM Member WHERE EMAIL ='{0}' AND PWD = '{1}'", member.email, member.pwd);
            return TomochanData.GetDataTable(qry);
        }
        public DataTable ViewMember(Member member)
        {
            string qry = string.Format("SELECT * FROM Member WHERE EMAIL ='{0}'", member.email);
            return TomochanData.GetDataTable(qry);
        }

        public DataTable ListMember()
        {
            string qry = string.Format("SELECT * FROM Member ORDER BY INDATE DESC");
            return TomochanData.GetDataTable(qry);
        }
    }
}
