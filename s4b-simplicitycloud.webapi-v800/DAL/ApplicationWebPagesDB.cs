using System.Data.OleDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimplicityOnlineWebApi.BLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using SimplicityOnlineBLL.Entities;

namespace SimplicityOnlineWebApi.DAL
{
    public class ApplicationWebPagesDB: MainDB
    {

        public ApplicationWebPagesDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }
        public List<ApplicationWebPages> GetAllApplicationWebPages()
        {
            List<ApplicationWebPages> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(ApplicationWebPagesQueries.GetAllApplicationWebPages(this.DatabaseType), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<ApplicationWebPages>();
                                while (dr.Read())
                                {
                                    returnValue.Add(LoadApplicationWebPages(dr));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }
        private ApplicationWebPages LoadApplicationWebPages(OleDbDataReader dr)
        {
            ApplicationWebPages refGenericLabels = null;
            try
            {
                if (dr != null)
                {
                    refGenericLabels = new ApplicationWebPages();
                    refGenericLabels.Sequence = long.Parse(dr["sequence"].ToString());
                    refGenericLabels.PageGeneticLabel = Utilities.GetDBString(dr["page_genetic_label"]);
                    refGenericLabels.PageCustomizeLabel = Utilities.GetDBString(dr["page_customize_label"]);
                    refGenericLabels.PageUrl = Utilities.GetDBString(dr["page_url"]);
                }
            }
            catch (Exception ex)
            {
            }
            return refGenericLabels;
        }

    }
}
