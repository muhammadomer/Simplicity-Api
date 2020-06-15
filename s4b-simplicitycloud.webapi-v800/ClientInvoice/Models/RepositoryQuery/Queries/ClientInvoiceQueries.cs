using SimplicityOnlineWebApi.ClientInvoice.Entities;
using SimplicityOnlineWebApi.ClientInvoice.Utills;
using SimplicityOnlineWebApi.Commons;
using System.Text;

namespace SimplicityOnlineWebApi.ClientInvoice.Models.RepositoryQuery.Queries
{
    public static class ClientInvoiceQueries
    {
        #region Bank Detail Query 
        internal static string GetBankDetailsQuery(int sequnceId, string invoiceNo)
        {
            string entityIdQuery = "Select distinct client_id from un_orders_bills where sequence = " + sequnceId;
            if (sequnceId > 0)
                entityIdQuery = "Select distinct client_id from un_orders_bills where sequence = " + sequnceId;
            if (invoiceNo != null && invoiceNo.Length > 0)
                entityIdQuery = "Select distinct client_id from un_orders_bills where invoice_no = '" + sequnceId + "'";
            
            StringBuilder sb = new StringBuilder();
            sb.Append("select BankName, BankSortCode, BankAccountNo, BankAccountName, CompanyRegistrationNo,VatNo ");
            sb.Append("\r\n");
            sb.Append("from ");
            sb.Append("\r\n");
            sb.Append("( ");
            sb.Append("\r\n");
            sb.Append("	SELECT data as columnValue, 'BankName' as columnTitle  FROM un_entity_details_supplementary WHERE entity_id in (" + entityIdQuery + ") AND data_type = '039' ");
            sb.Append("\r\n");
            sb.Append("	UNION ALL ");
            sb.Append("\r\n");
            sb.Append("	SELECT data as columnValue, 'BankSortCode' as columnTitle  FROM un_entity_details_supplementary WHERE entity_id in (" + entityIdQuery + ") AND data_type = '040' ");
            sb.Append("\r\n");
            sb.Append("	UNION ALL ");
            sb.Append("\r\n");
            sb.Append("	SELECT data as columnValue, 'BankAccountNo' as columnTitle  FROM un_entity_details_supplementary WHERE entity_id in (" + entityIdQuery + ") AND data_type = '041' ");
            sb.Append("\r\n");
            sb.Append("	UNION ALL ");
            sb.Append("\r\n");
            sb.Append("	SELECT data as columnValue, 'BankAccountName' as columnTitle  FROM un_entity_details_supplementary WHERE entity_id in (" + entityIdQuery + ") AND data_type = '042' ");
            sb.Append("\r\n");
            sb.Append("	UNION ALL ");
            sb.Append("\r\n");
            sb.Append("	SELECT data as columnValue, 'CompanyRegistrationNo' as columnTitle  FROM un_entity_details_supplementary WHERE entity_id in (" + entityIdQuery + ") AND data_type = '050' ");
            sb.Append("\r\n");
            sb.Append("	UNION ALL ");
            sb.Append("\r\n");
            sb.Append("	SELECT data as columnValue, 'VatNo' as columnTitle   FROM un_entity_details_supplementary WHERE entity_id in (" + entityIdQuery + ") AND data_type = '008' ");
            sb.Append("\r\n");
            sb.Append(") d ");
            sb.Append("\r\n");
            sb.Append("pivot ");
            sb.Append("\r\n");
            sb.Append("( ");
            sb.Append("\r\n");
            sb.Append("  max(columnValue) ");
            sb.Append("\r\n");
            sb.Append("  for columnTitle in (BankName, BankSortCode, BankAccountNo, BankAccountName, CompanyRegistrationNo,VatNo) ");
            sb.Append("\r\n");
            sb.Append(") piv; ");
            sb.Append("\r\n");
            return sb.ToString();
        }

        #endregion

        #region Get Company Address Query

        internal static string GetCompanyAddressQuery()
        {
            return "select Top 1 address_full as AddressFull from un_entity_details_core order by entity_id asc";
        }

        internal static string GetCompanyAddressInDetailQuery()
        {
            return "select Top 1 address_no as AddressNo,address_line1 as AddressLine1,address_line2 as AddressLine2,address_line3 as AddressLine3,address_line4 as AddressLine4,address_line5 as AddressLine5,address_full as AddressFull from un_entity_details_core order by entity_id asc";
        }

        #endregion

        #region Aged Dabator Section

        internal static string getAgedDabatorListColumns()
        {
            return " entity_id as entityId, name_long as companyName, Forename, Surname, Telephone, TelMobile, Email, SUM(OutstandingAmount) as Total, SUM(d30Days) as Days30Amount,SUM(d60Days) as Days60Amount, SUM(d90Days) as Days90Amount, Sum(Older) as OlderAmount ";
        }

        internal static string GetCountOfAgedDabatorList(string databaseType)
        {
            string DATE_DIFF_DAY_RANGE = DB_TAG_UTILLS.GetTagAsPerDbType(databaseType, "DATE_DIFF_DAY_RANGE");
            string CURRENT_DATE_TAG = DB_TAG_UTILLS.GetTagAsPerDbType(databaseType, "CURRENT_DATE_TAG");
            string BOOLEAN_TRUE_VALUE_STR = DB_TAG_UTILLS.GetTagAsPerDbType(databaseType, "BOOLEAN_TRUE_VALUE");
            string BOOLEAN_FALSE_VALUE_STR = DB_TAG_UTILLS.GetTagAsPerDbType(databaseType, "BOOLEAN_FALSE_VALUE");

            StringBuilder sb = new StringBuilder();
            sb.Append("Select Count(entity_id) as itemCount ");
            sb.Append("\r\n");
            sb.Append("from  ");
            sb.Append("\r\n");
            sb.Append("(  ");
            sb.Append("\r\n");
            sb.Append("	SELECT Distinct entity_id  ");
            sb.Append("\r\n");
            sb.Append("	FROM  ");
            sb.Append("\r\n");
            sb.Append("	( ");
            sb.Append("\r\n");
            sb.Append("		SELECT edc.entity_id, edc.name_short, edc.name_long, edc.name_forename as forename, edc.name_surname as surname, edc.telephone, edc.tel_mobile as telMobile, edc.email, ");
            sb.Append("\r\n");
            sb.Append("			(SUM(ie_i.entry_amt_total) - SUM((ie_i.entry_amt_allocated +  ie_i.entry_amt_allocated_labour))) AS OutstandingAmount, ");
            sb.Append("\r\n");
            sb.Append("			SUM(iif(datediff(" + DATE_DIFF_DAY_RANGE + ", ie_i.entry_date, " + CURRENT_DATE_TAG + ") <= 30,(ie_i.entry_amt_total - (ie_i.entry_amt_allocated +  ie_i.entry_amt_allocated_labour)), 0.00)) AS d30Days, ");
            sb.Append("\r\n");
            sb.Append("			SUM(iif(datediff(" + DATE_DIFF_DAY_RANGE + ", ie_i.entry_date, " + CURRENT_DATE_TAG + ") BETWEEN 31 AND 60, (ie_i.entry_amt_total - (ie_i.entry_amt_allocated +  ie_i.entry_amt_allocated_labour)), 0.00)) AS d60Days, ");
            sb.Append("\r\n");
            sb.Append("			SUM(iif(datediff(" + DATE_DIFF_DAY_RANGE + ", ie_i.entry_date, " + CURRENT_DATE_TAG + ") BETWEEN 61 AND 90, (ie_i.entry_amt_total - (ie_i.entry_amt_allocated +  ie_i.entry_amt_allocated_labour)), 0.00)) AS d90Days, ");
            sb.Append("\r\n");
            sb.Append("			SUM(iif(datediff(" + DATE_DIFF_DAY_RANGE + ", ie_i.entry_date, " + CURRENT_DATE_TAG + ") > 90, (ie_i.entry_amt_total - (ie_i.entry_amt_allocated +  ie_i.entry_amt_allocated_labour)), 0.00)) AS Older, ");
            sb.Append("\r\n");
            sb.Append("			" + BOOLEAN_TRUE_VALUE_STR + " AS si_line ");
            sb.Append("\r\n");
            sb.Append("		FROM un_invoice_entries_new AS ie_i, un_entity_details_core AS edc, un_entity_details_join AS edj ");
            sb.Append("\r\n");
            sb.Append("		WHERE edj.entity_id = edc.entity_id ");
            sb.Append("\r\n");
            sb.Append("		   AND ie_i.contact_id = edj.entity_id ");
            sb.Append("\r\n");
            sb.Append("		   AND edj.trans_type = 'B' ");
            sb.Append("\r\n");
            sb.Append("		   AND ie_i.trans_type = edj.trans_type ");
            sb.Append("\r\n");
            sb.Append("		   AND ie_i.entry_type = 'SI' ");
            sb.Append("\r\n");
            sb.Append("		GROUP BY edc.entity_id, edc.name_short, edc.name_long,edc.name_forename,edc.name_surname, edc.telephone, edc.tel_mobile, edc.email ");
            sb.Append("\r\n");
            sb.Append("		UNION  ");
            sb.Append("\r\n");
            sb.Append("		SELECT edc.entity_id, edc.name_short, edc.name_long, edc.name_forename as forename, edc.name_surname as surname, edc.telephone, edc.tel_mobile as telMobile, edc.email, ");
            sb.Append("\r\n");
            sb.Append("			(SUM(ie_i.entry_amt_total) - SUM((ie_i.entry_amt_allocated +  ie_i.entry_amt_allocated_labour))) * -1 AS OutstandingAmount, ");
            sb.Append("\r\n");
            sb.Append("			SUM(iif(datediff(" + DATE_DIFF_DAY_RANGE + ", ie_i.entry_date," + CURRENT_DATE_TAG + ") <= 30,(ie_i.entry_amt_total - (ie_i.entry_amt_allocated +  ie_i.entry_amt_allocated_labour)), 0.00)) * -1 AS d30Days, ");
            sb.Append("\r\n");
            sb.Append("			SUM(iif(datediff(" + DATE_DIFF_DAY_RANGE + ", ie_i.entry_date," + CURRENT_DATE_TAG + ") BETWEEN 31 AND 60, (ie_i.entry_amt_total - (ie_i.entry_amt_allocated +  ie_i.entry_amt_allocated_labour)), 0.00)) * -1 AS d60Days, ");
            sb.Append("\r\n");
            sb.Append("			SUM(iif(datediff(" + DATE_DIFF_DAY_RANGE + ", ie_i.entry_date," + CURRENT_DATE_TAG + ") BETWEEN 61 AND 90, (ie_i.entry_amt_total - (ie_i.entry_amt_allocated +  ie_i.entry_amt_allocated_labour)), 0.00)) * -1 AS d90Days, ");
            sb.Append("\r\n");
            sb.Append("			SUM(iif(datediff(" + DATE_DIFF_DAY_RANGE + ", ie_i.entry_date," + CURRENT_DATE_TAG + ") > 90, (ie_i.entry_amt_total - (ie_i.entry_amt_allocated +  ie_i.entry_amt_allocated_labour)), 0.00)) * -1 AS Older, ");
            sb.Append("\r\n");
            sb.Append("			" + BOOLEAN_FALSE_VALUE_STR + " AS si_line ");
            sb.Append("\r\n");
            sb.Append("		FROM un_invoice_entries_new AS ie_i, un_entity_details_core AS edc, un_entity_details_join AS edj ");
            sb.Append("\r\n");
            sb.Append("		WHERE edj.entity_id = edc.entity_id ");
            sb.Append("\r\n");
            sb.Append("		   AND ie_i.contact_id = edj.entity_id ");
            sb.Append("\r\n");
            sb.Append("		   AND edj.trans_type = 'B' ");
            sb.Append("\r\n");
            sb.Append("		   AND ie_i.trans_type = edj.trans_type ");
            sb.Append("\r\n");
            sb.Append("		   AND (ie_i.entry_type = 'SC' OR ie_i.entry_type = 'SA') ");
            sb.Append("\r\n");
            sb.Append("		GROUP BY edc.entity_id, edc.name_short, edc.name_long,edc.name_forename,edc.name_surname, edc.telephone, edc.tel_mobile, edc.email ");
            sb.Append("\r\n");
            sb.Append("		HAVING (SUM(ie_i.entry_amt_total) <> SUM((ie_i.entry_amt_allocated +  ie_i.entry_amt_allocated_labour))) ");
            sb.Append("\r\n");
            if (databaseType == "MSACCESS")
                sb.Append("	ORDER BY edc.name_long, si_line");
            else if (databaseType == "SQLSERVER")
                sb.Append("");
            else
                sb.Append("");
            sb.Append("	) as A ");
            sb.Append("\r\n");
            sb.Append(")as B ");
            sb.Append("\r\n");

            return sb.ToString();
        }

        internal static string GetAgedDabatorList(string databaseType)
        {
            string DATE_DIFF_DAY_RANGE = DB_TAG_UTILLS.GetTagAsPerDbType(databaseType, "DATE_DIFF_DAY_RANGE");
            string CURRENT_DATE_TAG = DB_TAG_UTILLS.GetTagAsPerDbType(databaseType, "CURRENT_DATE_TAG");
            string BOOLEAN_TRUE_VALUE_STR = DB_TAG_UTILLS.GetTagAsPerDbType(databaseType, "BOOLEAN_TRUE_VALUE");
            string BOOLEAN_FALSE_VALUE_STR = DB_TAG_UTILLS.GetTagAsPerDbType(databaseType, "BOOLEAN_FALSE_VALUE");

            StringBuilder sb = new StringBuilder();
            sb.Append("Select " + getAgedDabatorListColumns());
            sb.Append("\r\n");
            sb.Append("FROM ");
            sb.Append("\r\n");
            sb.Append("(");
            sb.Append("\r\n");
            sb.Append("	SELECT edc.entity_id, edc.name_short, edc.name_long, edc.name_forename as forename, edc.name_surname as surname, edc.telephone, edc.tel_mobile as telMobile, edc.email,");
            sb.Append("\r\n");
            sb.Append("		(SUM(ie_i.entry_amt_total) - SUM((ie_i.entry_amt_allocated +  ie_i.entry_amt_allocated_labour))) AS OutstandingAmount,");
            sb.Append("\r\n");
            sb.Append("		SUM(iif(datediff(" + DATE_DIFF_DAY_RANGE + ", ie_i.entry_date, " + CURRENT_DATE_TAG + ") <= 30,(ie_i.entry_amt_total - (ie_i.entry_amt_allocated +  ie_i.entry_amt_allocated_labour)), 0.00)) AS d30Days,");
            sb.Append("\r\n");
            sb.Append("		SUM(iif(datediff(" + DATE_DIFF_DAY_RANGE + ", ie_i.entry_date, " + CURRENT_DATE_TAG + ") BETWEEN 31 AND 60, (ie_i.entry_amt_total - (ie_i.entry_amt_allocated +  ie_i.entry_amt_allocated_labour)), 0.00)) AS d60Days,");
            sb.Append("\r\n");
            sb.Append("		SUM(iif(datediff(" + DATE_DIFF_DAY_RANGE + ", ie_i.entry_date, " + CURRENT_DATE_TAG + ") BETWEEN 61 AND 90, (ie_i.entry_amt_total - (ie_i.entry_amt_allocated +  ie_i.entry_amt_allocated_labour)), 0.00)) AS d90Days,");
            sb.Append("\r\n");
            sb.Append("		SUM(iif(datediff(" + DATE_DIFF_DAY_RANGE + ", ie_i.entry_date, " + CURRENT_DATE_TAG + ") > 90, (ie_i.entry_amt_total - (ie_i.entry_amt_allocated +  ie_i.entry_amt_allocated_labour)), 0.00)) AS Older,");
            sb.Append("\r\n");
            sb.Append("		" + BOOLEAN_TRUE_VALUE_STR + " AS si_line");
            sb.Append("\r\n");
            sb.Append("	FROM un_invoice_entries_new AS ie_i, un_entity_details_core AS edc, un_entity_details_join AS edj");
            sb.Append("\r\n");
            sb.Append("	WHERE edj.entity_id = edc.entity_id");
            sb.Append("\r\n");
            sb.Append("	   AND ie_i.contact_id = edj.entity_id");
            sb.Append("\r\n");
            sb.Append("	   AND edj.trans_type = 'B'");
            sb.Append("\r\n");
            sb.Append("	   AND ie_i.trans_type = edj.trans_type");
            sb.Append("\r\n");
            sb.Append("	   AND ie_i.entry_type = 'SI'");
            sb.Append("\r\n");
            sb.Append("	GROUP BY edc.entity_id, edc.name_short, edc.name_long,edc.name_forename,edc.name_surname, edc.telephone, edc.tel_mobile, edc.email");
            sb.Append("\r\n");
            sb.Append("	UNION ");
            sb.Append("\r\n");
            sb.Append("	SELECT edc.entity_id, edc.name_short, edc.name_long, edc.name_forename as forename, edc.name_surname as surname, edc.telephone, edc.tel_mobile as telMobile, edc.email,");
            sb.Append("\r\n");
            sb.Append("		(SUM(ie_i.entry_amt_total) - SUM((ie_i.entry_amt_allocated +  ie_i.entry_amt_allocated_labour))) * -1 AS OutstandingAmount,");
            sb.Append("\r\n");
            sb.Append("		SUM(iif(datediff(" + DATE_DIFF_DAY_RANGE + ", ie_i.entry_date, " + CURRENT_DATE_TAG + ") <= 30,(ie_i.entry_amt_total - (ie_i.entry_amt_allocated +  ie_i.entry_amt_allocated_labour)), 0.00)) * -1 AS d30Days,");
            sb.Append("\r\n");
            sb.Append("		SUM(iif(datediff(" + DATE_DIFF_DAY_RANGE + ", ie_i.entry_date," + CURRENT_DATE_TAG + ") BETWEEN 31 AND 60, (ie_i.entry_amt_total - (ie_i.entry_amt_allocated +  ie_i.entry_amt_allocated_labour)), 0.00)) * -1 AS d60Days,");
            sb.Append("\r\n");
            sb.Append("		SUM(iif(datediff(" + DATE_DIFF_DAY_RANGE + ", ie_i.entry_date, " + CURRENT_DATE_TAG + ") BETWEEN 61 AND 90, (ie_i.entry_amt_total - (ie_i.entry_amt_allocated +  ie_i.entry_amt_allocated_labour)), 0.00)) * -1 AS d90Days,");
            sb.Append("\r\n");
            sb.Append("		SUM(iif(datediff(" + DATE_DIFF_DAY_RANGE + ", ie_i.entry_date, " + CURRENT_DATE_TAG + ") > 90, (ie_i.entry_amt_total - (ie_i.entry_amt_allocated +  ie_i.entry_amt_allocated_labour)), 0.00)) * -1 AS Older,");
            sb.Append("\r\n");
            sb.Append("		" + BOOLEAN_FALSE_VALUE_STR + " AS si_line");
            sb.Append("\r\n");
            sb.Append("	FROM un_invoice_entries_new AS ie_i, un_entity_details_core AS edc, un_entity_details_join AS edj");
            sb.Append("\r\n");
            sb.Append("	WHERE edj.entity_id = edc.entity_id");
            sb.Append("\r\n");
            sb.Append("	   AND ie_i.contact_id = edj.entity_id");
            sb.Append("\r\n");
            sb.Append("	   AND edj.trans_type = 'B'");
            sb.Append("\r\n");
            sb.Append("	   AND ie_i.trans_type = edj.trans_type");
            sb.Append("\r\n");
            sb.Append("	   AND (ie_i.entry_type = 'SC' OR ie_i.entry_type = 'SA')");
            sb.Append("\r\n");
            sb.Append("	GROUP BY edc.entity_id, edc.name_short, edc.name_long,edc.name_forename,edc.name_surname, edc.telephone, edc.tel_mobile, edc.email");
            sb.Append("\r\n");
            sb.Append("	HAVING (SUM(ie_i.entry_amt_total) <> SUM((ie_i.entry_amt_allocated +  ie_i.entry_amt_allocated_labour)))");
            sb.Append("\r\n");
            if (databaseType == "MSACCESS")
                sb.Append("	ORDER BY edc.name_long, si_line");
            else if (databaseType == "SQLSERVER")
                sb.Append("");
            else
                sb.Append("");
            sb.Append("\r\n");
            sb.Append(") as A");
            sb.Append("\r\n");
            sb.Append("group by entity_id,name_long,forename,surname,telephone,telMobile,email");
            sb.Append("\r\n");
            sb.Append("Order by name_long ASC");
            sb.Append("\r\n");
            return sb.ToString();
        }

        #endregion

        #region Statement Section

        internal static string GetStatementList(string databaseType, Parameters parameters)
        {
            string endDateSTR = "";
            if (parameters.endDate != null)
                endDateSTR = Utilities.GetDateValueForDML(databaseType, parameters.endDate);

            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT ord.job_ref AS JobRef, ord.job_address AS JobAddress, edc.name_long AS Client, ");
            sb.Append("\r\n");
            sb.Append("    ie.entry_type AS EntryType, ie.entry_date AS EntryDate, ie.entry_amt_total AS EntryAmtTotal, ");
            sb.Append("\r\n");
            sb.Append("    ie.entry_details AS EntryDetails, ie.invoiceno_or_itemref AS InvoiceNoOrItemRef, ob.sequence AS sequence ");
            sb.Append("\r\n");
            sb.Append("FROM  ");
            sb.Append("\r\n");
            sb.Append("	( ");
            sb.Append("\r\n");
            sb.Append("		( ");
            sb.Append("\r\n");
            sb.Append("			un_invoice_entries_new AS ie INNER JOIN un_orders AS ord ON ie.job_sequence = ord.sequence ");
            sb.Append("\r\n");
            sb.Append("		) ");
            sb.Append("\r\n");
            sb.Append("		INNER JOIN  ");
            sb.Append("\r\n");
            sb.Append("				( ");
            sb.Append("\r\n");
            sb.Append("					un_web_viewer_assign_to AS wvat INNER JOIN un_entity_details_core AS edc ");
            sb.Append("\r\n");
            sb.Append("					ON wvat.entity_id = edc.entity_id ");
            sb.Append("\r\n");
            sb.Append("				) ");
            sb.Append("\r\n");
            sb.Append("				ON ie.contact_id = edc.entity_id ");
            sb.Append("\r\n");
            sb.Append("	) ");
            sb.Append("\r\n");
            sb.Append("	INNER JOIN un_orders_bills AS ob ON (ob.invoice_no = ie.invoiceno_or_itemref)  ");
            sb.Append("\r\n");
            sb.Append("			AND (ie.contact_id = ob.entity_join_id) AND (ie.job_sequence = ob.job_sequence) ");
            sb.Append("\r\n");
            sb.Append("WHERE ie.entry_type = 'SI' ");
            sb.Append("\r\n");
            sb.Append("	AND ie.trans_type = 'B' ");
            sb.Append("\r\n");
            sb.Append("	AND ie.flg_cancelled <> 1 ");
            sb.Append("\r\n");
            sb.Append("	AND wvat.flg_deleted <> 1 ");
            sb.Append("\r\n");
            sb.AppendFormat("	AND ie.entry_date <= {0} ", endDateSTR);
            sb.Append("\r\n");
            sb.Append("UNION ");
            sb.Append("\r\n");
            sb.Append("SELECT '' AS JobRef, '' AS JobAddress, edc.name_long AS Client, ");
            sb.Append("\r\n");
            sb.Append("    ie.entry_type AS EntryType, ie.entry_date AS EntryDate, ie.entry_amt_total AS EntryAmtTotal, ");
            sb.Append("\r\n");
            sb.Append("    ie.entry_details AS EntryDetails, ie.invoiceno_or_itemref AS InvoiceNoOrItemRef, ie.sequence ");
            sb.Append("\r\n");
            sb.Append("FROM un_invoice_entries_new AS ie ");
            sb.Append("\r\n");
            sb.Append("	INNER JOIN  ");
            sb.Append("\r\n");
            sb.Append("			( ");
            sb.Append("\r\n");
            sb.Append("				un_web_viewer_assign_to AS wvat INNER JOIN un_entity_details_core AS edc ON wvat.entity_id = edc.entity_id ");
            sb.Append("\r\n");
            sb.Append("			) ");
            sb.Append("\r\n");
            sb.Append("			ON ie.contact_id = edc.entity_id ");
            sb.Append("\r\n");
            sb.Append("WHERE ie.entry_type <> 'NA' ");
            sb.Append("\r\n");
            sb.Append("	AND ie.entry_type <> 'SI' ");
            sb.Append("\r\n");
            sb.Append("	AND ie.entry_type <> 'RR' ");
            sb.Append("\r\n");
            sb.Append("	AND ie.trans_type = 'B' ");
            sb.Append("\r\n");
            sb.Append("	AND ie.flg_cancelled <> 1 ");
            sb.Append("\r\n");
            sb.Append("	AND wvat.flg_deleted <> 1 ");
            sb.Append("\r\n");
            sb.AppendFormat("	AND ie.entry_date <= {0} ", endDateSTR);
            sb.Append("\r\n");
            sb.Append("UNION ");
            sb.Append("\r\n");
            sb.Append("SELECT ord.job_ref AS JobRef, ord.job_address AS JobAddress,edc.name_long AS Client, ");
            sb.Append("\r\n");
            sb.Append("    'AFP' AS EntryType, ob.invoice_date AS EntryDate, ob.amount_total AS EntryAmtTotal, ");
            sb.Append("\r\n");
            sb.Append("    'Not Set' AS EntryDetails, ob.invoice_no AS InvoiceNoOrItemRef, ob.sequence AS sequence ");
            sb.Append("\r\n");
            sb.Append("FROM  ");
            sb.Append("\r\n");
            sb.Append("	( ");
            sb.Append("\r\n");
            sb.Append("		un_orders_bills AS ob INNER JOIN un_orders AS ord ON ob.job_sequence = ord.sequence ");
            sb.Append("\r\n");
            sb.Append("	) ");
            sb.Append("\r\n");
            sb.Append("	INNER JOIN ( ");
            sb.Append("\r\n");
            sb.Append("					un_entity_details_core AS edc INNER JOIN un_web_viewer_assign_to AS wvat ON edc.entity_id = wvat.entity_id ");
            sb.Append("\r\n");
            sb.Append("			   ) ");
            sb.Append("\r\n");
            sb.Append("			   ON ob.entity_join_id = edc.entity_id ");
            sb.Append("\r\n");
            sb.Append("WHERE ob.flg_set_to_proforma = 1 ");
            sb.Append("\r\n");
            sb.Append("	AND ob.flg_set_to_invoice <> 1 ");
            sb.Append("\r\n");
            sb.Append("	AND wvat.flg_deleted <> 1 ");
            sb.Append("\r\n");
            sb.AppendFormat("	AND ob.invoice_date  <= {0} ", endDateSTR);
            sb.Append("\r\n");
            sb.Append("ORDER BY EntryDate  ");
            sb.Append("\r\n");
            return sb.ToString();
        }

        #endregion

        #region All Items & Unpaid Items Section

        internal static string GetInvoiceListColumnName(bool isGetCountQuery)
        {
            if (isGetCountQuery)
                return " count(Sequence) as itemCount ";
            else
                return " EntityId, ClientName, JobRef, JobAddress, Sequence, InvoiceNo, InvoiceDate, AmountSubTotal, AmountVat, AmountTotal, FlgSettled, EntryAmtTotal, EntryAmtAllocated, AmtOutstanding, Inv  ";
        }

        internal static string GetDateQuery(Parameters parameters, string databaseType, string fieldName)
        {
            StringBuilder dateQuery = new StringBuilder();
            string startDateSTR, endDateSTR;
            if (parameters.startDate != null)
                startDateSTR = "		AND " + fieldName + " >= " + Utilities.GetDateValueForDML(databaseType, parameters.startDate);
            else
                startDateSTR = "";

            if (parameters.endDate != null)
                endDateSTR = "		AND " + fieldName + " <= " + Utilities.GetDateValueForDML(databaseType, parameters.endDate);
            else
                endDateSTR = "";

            dateQuery.Append(startDateSTR);
            dateQuery.Append(endDateSTR);

            return dateQuery.ToString();
        }

        internal static string GetInvoiceList(string databaseType, Parameters parameters, bool isGetCountQuery)
        {
            string BOOLEAN_TRUE_VALUE_STR = DB_TAG_UTILLS.GetTagAsPerDbType(databaseType, "BOOLEAN_TRUE_VALUE");
            string BOOLEAN_FALSE_VALUE_STR = DB_TAG_UTILLS.GetTagAsPerDbType(databaseType, "BOOLEAN_FALSE_VALUE");

            StringBuilder sb = new StringBuilder();
            sb.Append("Select " + GetInvoiceListColumnName(isGetCountQuery));
            sb.Append("\r\n");
            sb.Append("from ");
            sb.Append("\r\n");
            sb.Append("(");
            sb.Append("\r\n");

            #region Comman Query             
            if (parameters.type == null || parameters.type == "" || parameters.type == "0")
            {
                sb.Append("	SELECT edc.entity_id as EntityId, edc.name_long as ClientName,ord.job_ref as JobRef, ord.job_address as JobAddress, ob.sequence as Sequence, ob.invoice_no as InvoiceNo, ob.invoice_date as InvoiceDate, ob.amount_sub_total as AmountSubTotal, ob.amount_vat as AmountVat, ob.amount_total as AmountTotal, ");
                sb.Append("\r\n");
                sb.Append("		ie.flg_settled as FlgSettled, ie.entry_amt_total as EntryAmtTotal, ie.entry_amt_allocated as EntryAmtAllocated,(ie.entry_amt_total - ie.entry_amt_allocated) AS AmtOutstanding, 1 AS Inv  ");
                sb.Append("\r\n");
                sb.Append("	FROM  ");
                sb.Append("\r\n");
                sb.Append("		( ");
                sb.Append("\r\n");
                sb.Append("			( ");
                sb.Append("\r\n");
                sb.Append("				un_entity_details_core AS edc ");
                sb.Append("\r\n");
                sb.Append("					INNER JOIN un_orders_bills AS ob ON edc.entity_id = ob.client_id ");
                sb.Append("\r\n");
                sb.Append("			) ");
                sb.Append("\r\n");
                sb.Append("			INNER JOIN un_orders AS ord ON ob.job_sequence = ord.sequence ");
                sb.Append("\r\n");
                sb.Append("		) ");
                sb.Append("\r\n");
                sb.Append("		INNER JOIN un_invoice_entries_new AS ie ON ob.invoice_no = ie.invoiceno_or_itemref ");
                sb.Append("\r\n");
                sb.AppendFormat("	WHERE ob.flg_set_to_invoice = {0}", BOOLEAN_TRUE_VALUE_STR);
                sb.Append("\r\n");
                if (parameters.entityId > 0)
                {
                    sb.AppendFormat("		AND edc.entity_id = {0} ", parameters.entityId);
                    sb.Append("\r\n");
                }
                sb.Append("		AND ie.entry_type = 'SI'");
                sb.Append("\r\n");
                sb.Append("		AND ie.trans_type = 'B'");
                sb.Append("\r\n");

                if (parameters.job != null)
                {
                    sb.AppendFormat("		AND ord.job_ref = '{0}' ", parameters.job);
                    sb.Append("\r\n");
                }

                sb.Append(GetDateQuery(parameters, databaseType, "ob.invoice_date"));

                if (parameters.status != null)
                {
                    if (parameters.status == "open")
                        sb.Append("		AND ie.entry_amt_total - ie.entry_amt_allocated > 0.01 ");
                    else
                        sb.Append("		AND ie.entry_amt_total - ie.entry_amt_allocated < 0.01 ");
                }
            }

            #region Retentions
            sb.Append("\r\n");
            sb.Append("UNION");
            sb.Append("\r\n");
            sb.Append("	SELECT edc.entity_id as EntityId, edc.name_long as ClientName, ord.job_ref AS JobRef, ord.job_address AS JobAddress, -1 AS Sequence, ie.invoiceno_or_itemref AS InvoiceNo,");
            sb.Append("\r\n");
            sb.Append("		ie.entry_date AS InvoiceDate,  ie.entry_amt_subtotal AS AmountSubTotal, ie.entry_amt_vat AS AmountVat, ie.entry_amt_total AS AmountTotal,");
            sb.Append("\r\n");
            sb.Append("		ie.flg_settled AS FlgSettled, ie.entry_amt_total AS EntryAmtTotal, ie.entry_amt_allocated AS EntryAmtAllocated,");
            sb.Append("\r\n");
            sb.Append("		(ie.entry_amt_total-ie.entry_amt_allocated) AS AmtOutstanding, 1 AS Inv ");
            sb.Append("\r\n");
            sb.Append("	FROM  ");
            sb.Append("\r\n");
            sb.Append("		( ");
            sb.Append("\r\n");
            sb.Append("			un_invoice_retentions AS ir INNER JOIN un_orders AS ord ON ir.job_sequence = ord.sequence ");
            sb.Append("\r\n");
            sb.Append("		) ");
            sb.Append("\r\n");
            sb.Append("		INNER JOIN  ");
            sb.Append("\r\n");
            sb.Append("			( ");
            sb.Append("\r\n");
            sb.Append("				un_invoice_entries_new AS ie  ");
            sb.Append("\r\n");
            sb.Append("					INNER JOIN un_entity_details_core AS edc ON ie.contact_id = edc.entity_id ");
            sb.Append("\r\n");
            sb.Append("			) ");
            sb.Append("\r\n");
            sb.Append("			ON ir.retention_invoice_no = ie.invoiceno_or_itemref ");
            sb.Append("\r\n");
            sb.Append("	 WHERE ie.entry_type = 'SI' ");
            sb.Append("\r\n");
            sb.Append("		AND ie.trans_type ='B' ");
            sb.Append("\r\n");

            if (parameters.entityId > 0)
                sb.AppendFormat("		AND ie.contact_id = {0} ", parameters.entityId);
            else
                sb.Append("		AND ie.contact_id <= 0 ");

            sb.Append("\r\n");

            sb.AppendFormat("		AND ir.flg_retention_cleared = {0} ", BOOLEAN_TRUE_VALUE_STR);
            sb.Append("\r\n");

            if (parameters.job != null)
            {
                sb.AppendFormat("		AND ord.job_ref = '{0}' ", parameters.job);
                sb.Append("\r\n");
            }

            sb.Append(GetDateQuery(parameters, databaseType, "ie.entry_date"));

            if (parameters.status != null)
            {
                if (parameters.status == "open")
                    sb.Append("		AND ie.entry_amt_total - ie.entry_amt_allocated > 0.01 ");
                else
                    sb.Append("		AND ie.entry_amt_total - ie.entry_amt_allocated < 0.01 ");
            }
            #endregion

            if (parameters.type == null || parameters.type == "")
            {
                sb.Append("\r\n");
                sb.Append("UNION ");
                sb.Append("\r\n");
            }

            if (parameters.type == null || parameters.type == "" || parameters.type == "1")
            {
                sb.Append("	SELECT edc.entity_id as entityId, edc.name_long as ClientName, ord.job_ref as JobRef,ord.job_address as JobAddress, ob.sequence, ob.invoice_no as InvoiceNo, ");
                sb.Append("\r\n");
                sb.Append("		ob.invoice_date as InvoiceDate, ob.amount_sub_total as AmountSubTotal, ob.amount_vat as AmountVat, ob.amount_total as AmountTotal, ");
                sb.Append("\r\n");
                sb.Append("		" + BOOLEAN_FALSE_VALUE_STR + " AS FlgSettled, ob.amount_total AS EntryAmtTotal, 0 AS EntryAmtAllocated, ");
                sb.Append("\r\n");
                sb.Append("		ob.amount_total AS AmtOutstanding, 0 AS Inv ");
                sb.Append("\r\n");
                sb.Append("	FROM  ");
                sb.Append("\r\n");
                sb.Append("		( ");
                sb.Append("\r\n");
                sb.Append("			un_entity_details_core AS edc INNER JOIN un_orders_bills AS ob ON edc.entity_id = ob.client_id ");
                sb.Append("\r\n");
                sb.Append("		)  ");
                sb.Append("\r\n");
                sb.Append("		INNER JOIN un_orders AS ord ON ob.job_sequence = ord.sequence ");
                sb.Append("\r\n");
                sb.AppendFormat("	WHERE ob.flg_set_to_proforma = {0}  AND ob.flg_set_to_invoice<> {0} ", BOOLEAN_TRUE_VALUE_STR);
                sb.Append("\r\n");

                if (parameters.entityId > 0)
                    sb.AppendFormat("		AND edc.entity_id = {0} ", parameters.entityId);
                else
                    sb.Append("		AND  ob.entity_join_id <= 0 ");
                sb.Append("\r\n");

                if (parameters.job != null)
                {
                    sb.AppendFormat("		AND ord.job_ref = '{0}' ", parameters.job);
                    sb.Append("\r\n");
                }
                sb.Append(GetDateQuery(parameters, databaseType, "ob.invoice_date"));
            }
            #endregion

            sb.Append(") as temp");
            sb.Append("\r\n");

            if (!isGetCountQuery)
                sb.Append("ORDER BY InvoiceDate DESC ");

            return sb.ToString();
        }

        #endregion

        #region Get Invoice Queries 

        internal static string GetInvoiceDetailCommanData(int sequnceId, string invoiceNo)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Select Top 1 * from ( ");
            sb.Append("\r\n");
            sb.Append("SELECT unb.sequence, (Select top 1 entry_details from un_invoice_entries_new where  invoiceno_or_itemref = unb.invoice_no) as FootNote, ");
            sb.Append("\r\n");
            sb.Append("	(Select top 1 entry_type from un_invoice_entries_new where  invoiceno_or_itemref = unb.invoice_no) as invoiceType, ");
            sb.Append("\r\n");
            sb.Append("	unb.invoice_no as InvoiceNo,unb.invoice_date as InvoiceDate,un_edc.name_long as NameLong,unor.job_ref as JobRef, ");
            sb.Append("\r\n");
            sb.Append("	unor.job_address as JobAddress,unor.job_date_start as JobDateStart,unor.job_date_finish as JobDateFinish, ");
            sb.Append("\r\n");
            sb.Append("	unor.job_client_ref as JobClientRef,unor.job_trade_code as JobTradeCode, unb.flg_set_to_invoice as FlgSetToInvoice, ");
            sb.Append("\r\n");
            sb.Append("	unb.mailling_address as MaillingAddress, un_oi.row_index as RowIndex,  ");
            sb.Append("\r\n");
            sb.Append("	unb.amount_initial as SubtotalScheduledItems, unb.amount_initial as InvoiceAmount,unb.amount_discount as Discount,   ");
            sb.Append("\r\n");
            sb.Append("	unb.amount_retention as RetentionTotal, unb.amount_sub_total as SubTotal, unb.amount_vat AS Vat, unb.amount_total as InvoiceTotal 	 ");
            sb.Append("\r\n");
            sb.Append("FROM  ");
            sb.Append("\r\n");
            sb.Append("	( ");
            sb.Append("\r\n");
            sb.Append("		( ");
            sb.Append("\r\n");
            sb.Append("			(un_entity_details_core un_edc INNER JOIN un_orders_bills unb ON un_edc.entity_id=unb.client_id)  ");
            sb.Append("\r\n");
            sb.Append("			LEFT OUTER JOIN un_orders_bill_items un_obi ON unb.sequence=un_obi.bill_sequence  ");
            sb.Append("\r\n");
            sb.Append("		)  ");
            sb.Append("\r\n");
            sb.Append("		LEFT OUTER JOIN un_orders unor ON unb.job_sequence = unor.sequence  ");
            sb.Append("\r\n");
            sb.Append("	) ");
            sb.Append("\r\n");
            sb.Append("	LEFT OUTER JOIN un_order_items un_oi ON (un_obi.job_sequence = un_oi.job_sequence) AND (un_obi.item_sequence = un_oi.sequence)  ");
            sb.Append("\r\n");
            sb.Append("WHERE  ");
            sb.Append("\r\n");
            sb.Append("	(	 ");
            sb.Append("\r\n");
            sb.Append("		un_oi.job_sequence IS NULL  ");
            sb.Append("\r\n");
            sb.Append("		AND un_oi.sequence IS NULL  ");
            sb.Append("\r\n");
            sb.Append("		OR un_oi.job_sequence IS NOT NULL  ");
            sb.Append("\r\n");
            sb.Append("		AND un_oi.sequence IS NOT NULL  ");
            sb.Append("\r\n");
            sb.Append("		AND un_oi.job_sequence = un_obi.job_sequence  ");
            sb.Append("\r\n");
            sb.Append("		AND un_oi.sequence=un_obi.item_sequence  ");
            sb.Append("\r\n");
            sb.Append("	)  ");
            sb.Append("\r\n");

            if (sequnceId > 0)
                sb.AppendFormat(" 	AND unb.sequence = {0} ", sequnceId);
            if (invoiceNo != null && invoiceNo.Length > 0)
                sb.AppendFormat(" 	AND unb.invoice_no = '{0}'", invoiceNo);

            sb.Append("\r\n");
            sb.Append("UNION ALL ");
            sb.Append("\r\n");
            sb.Append("SELECT ob.sequence,(Select top 1 entry_details from un_invoice_entries_new where invoiceno_or_itemref =  ob.invoice_no) as FootNote, ");
            sb.Append("\r\n");
            sb.Append("	(Select top 1 entry_type from un_invoice_entries_new where  invoiceno_or_itemref = ob.invoice_no) as invoiceType, ");
            sb.Append("\r\n");
            sb.Append("	ob.invoice_no as InvoiceNo,ob.invoice_date as InvoiceDate,edc.name_long as NameLong, ord.job_ref as JobRef,  ");
            sb.Append("\r\n");
            sb.Append("	ord.job_address as JobAddress,ord.job_date_start as JobDateStart,ord.job_date_finish as JobDateFinish,  ");
            sb.Append("\r\n");
            sb.Append("	ord.job_client_ref as JobClientRef,ord.job_trade_code as JobTradeCode,ob.flg_set_to_invoice as FlgSetToInvoice,  ");
            sb.Append("\r\n");
            sb.Append("	ob.mailling_address as MaillingAddress, ordi.row_index as RowIndex,  ");
            sb.Append("\r\n");
            sb.Append("	ob.amount_initial as SubtotalScheduledItems, ob.amount_initial as InvoiceAmount, ob.amount_discount as Discount,   ");
            sb.Append("\r\n");
            sb.Append("	ob.amount_retention as RetentionTotal, ob.amount_sub_total as SubTotal, ob.amount_vat AS Vat, ob.amount_total as InvoiceTotal ");
            sb.Append("\r\n");
            sb.Append("FROM ");
            sb.Append("\r\n");
            sb.Append("	( ");
            sb.Append("\r\n");
            sb.Append("		( ");
            sb.Append("\r\n");
            sb.Append("			un_entity_details_core AS edc INNER JOIN un_orders_bills AS ob ON edc.entity_id = ob.client_id  ");
            sb.Append("\r\n");
            sb.Append("		) ");
            sb.Append("\r\n");
            sb.Append("		LEFT OUTER JOIN un_orders AS ord ON ob.job_sequence = ord.sequence  ");
            sb.Append("\r\n");
            sb.Append("	) ");
            sb.Append("\r\n");
            sb.Append("	LEFT OUTER JOIN un_order_items AS ordi ON ord.sequence = ordi.job_sequence 	     ");
            sb.Append("\r\n");
            sb.Append("WHERE ordi.flg_row_is_text = 1  ");
            sb.Append("\r\n");

            if (sequnceId > 0)
                sb.AppendFormat(" 	AND ob.sequence = {0}", sequnceId);
            if (invoiceNo != null && invoiceNo.Length > 0)
                sb.AppendFormat(" 	AND ob.invoice_no = '{0}'", invoiceNo);

            sb.Append("\r\n");
            sb.Append(") as Temp ORDER BY RowIndex,Sequence ");
            sb.Append("\r\n");
            return sb.ToString();
        }

        internal static string GetInvoiceDetailItemsColumn()
        {
            return " FlgRowIsText, ItemDesc, ItemCode, ItemUnits, ItemQuantity, AmountPayment, AmountVat, AmountDiscount, AmountSubTotal, AmountRetention, RowIndex, Sequence ";
        }

        internal static string GetInvoiceDetailItems(int sequnceId, string invoiceNo)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Select " + GetInvoiceDetailItemsColumn());
            sb.Append("from ( ");
            sb.Append("\r\n");
            sb.Append("	SELECT un_oi.flg_row_is_text as FlgRowIsText,un_oi.item_desc as ItemDesc,un_oi.item_code as ItemCode, ");
            sb.Append("\r\n");
            sb.Append("		un_oi.item_units as ItemUnits,un_oi.item_quantity as ItemQuantity,un_obi.amount_payment as AmountPayment, ");
            sb.Append("\r\n");
            sb.Append("		un_obi.amount_vat as AmountVat,un_obi.amount_discount as AmountDiscount,un_obi.amount_sub_total as AmountSubTotal,un_obi.amount_retention as AmountRetention, ");
            sb.Append("\r\n");
            sb.Append("		un_oi.row_index as RowIndex,un_oi.Sequence ");
            sb.Append("\r\n");
            sb.Append("	FROM  ");
            sb.Append("\r\n");
            sb.Append("		( ");
            sb.Append("\r\n");
            sb.Append("			( ");
            sb.Append("\r\n");
            sb.Append("				( ");
            sb.Append("\r\n");
            sb.Append("					un_entity_details_core un_edc INNER JOIN un_orders_bills unb ON un_edc.entity_id=unb.client_id  ");
            sb.Append("\r\n");
            sb.Append("				)  ");
            sb.Append("\r\n");
            sb.Append("				INNER JOIN un_orders_bill_items un_obi ON unb.sequence=un_obi.bill_sequence  ");
            sb.Append("\r\n");
            sb.Append("			)  ");
            sb.Append("\r\n");
            sb.Append("			INNER JOIN un_orders unor ON unb.job_sequence = unor.sequence  ");
            sb.Append("\r\n");
            sb.Append("		) ");
            sb.Append("\r\n");
            sb.Append("		LEFT OUTER JOIN un_order_items un_oi ON (un_obi.job_sequence = un_oi.job_sequence)  ");
            sb.Append("\r\n");
            sb.Append("			AND (un_obi.item_sequence = un_oi.sequence)  ");
            sb.Append("\r\n");
            sb.Append("	WHERE  ");
            sb.Append("\r\n");
            sb.Append("		( ");
            sb.Append("\r\n");
            sb.Append("			un_oi.job_sequence IS NULL  ");
            sb.Append("\r\n");
            sb.Append("			AND un_oi.sequence IS NULL  ");
            sb.Append("\r\n");
            sb.Append("			OR un_oi.job_sequence IS NOT NULL  ");
            sb.Append("\r\n");
            sb.Append("			AND un_oi.sequence IS NOT NULL  ");
            sb.Append("\r\n");
            sb.Append("			AND un_oi.job_sequence=un_obi.job_sequence  ");
            sb.Append("\r\n");
            sb.Append("			AND un_oi.sequence=un_obi.item_sequence  ");
            sb.Append("\r\n");
            sb.Append("		)  ");
            sb.Append("\r\n");
            sb.Append("		AND (un_oi.item_quantity > 0 OR  un_obi.amount_payment > 0)  ");
            sb.Append("\r\n");
            
            if (sequnceId > 0)
                sb.AppendFormat(" 	AND unb.sequence = {0}", sequnceId);
            if (invoiceNo != null && invoiceNo.Length > 0)
                sb.AppendFormat(" 	AND unb.invoice_no = '{0}'", invoiceNo);

            sb.Append("\r\n");
            sb.Append("UNION ALL ");
            sb.Append("\r\n");
            sb.Append("	SELECT ordi.flg_row_is_text as FlgRowIsText,ordi.item_desc as ItemDesc,ordi.item_code as ItemCode, ");
            sb.Append("\r\n");
            sb.Append("		ordi.item_units as ItemUnits,ordi.item_quantity as ItemQuantity,0 AS AmountPayment,0 as AmountVat,0 as AmountDiscount,0 as AmountSubTotal,0 as AmountRetention, ");
            sb.Append("\r\n");
            sb.Append("		ordi.row_index as RowIndex,ordi.Sequence ");
            sb.Append("\r\n");
            sb.Append("	FROM ");
            sb.Append("\r\n");
            sb.Append("		( ");
            sb.Append("\r\n");
            sb.Append("			( ");
            sb.Append("\r\n");
            sb.Append("				un_entity_details_core AS edc INNER JOIN un_orders_bills AS ob ON edc.entity_id = ob.client_id  ");
            sb.Append("\r\n");
            sb.Append("			) ");
            sb.Append("\r\n");
            sb.Append("			INNER JOIN un_orders AS ord ON ob.job_sequence = ord.sequence ");
            sb.Append("\r\n");
            sb.Append("		) ");
            sb.Append("\r\n");
            sb.Append("		INNER JOIN un_order_items AS ordi ON ord.sequence = ordi.job_sequence ");
            sb.Append("\r\n");
            sb.Append("	WHERE ordi.flg_row_is_text = 1  ");
            sb.Append("\r\n");
            sb.Append("		AND (ordi.item_quantity > 0)  ");
            sb.Append("\r\n");
            if (invoiceNo == null || invoiceNo.Length > 0)
                sb.AppendFormat(" 	AND ob.sequence = {0}", sequnceId);
            else
                sb.AppendFormat(" 	AND ob.invoice_no = '{0}'", invoiceNo);
            sb.Append("\r\n");
            sb.Append(") as Temp ");
            sb.Append("\r\n");
            sb.Append("ORDER BY RowIndex, Sequence ");
            sb.Append("\r\n");
            return sb.ToString();
        }

        internal static string GetInvoiceNoWisePayment(string invoiceNo)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT p.entry_type as EntryType, ");
            sb.Append("\r\n");
            sb.Append("	p.invoiceno_or_itemref AS InvoicenoOrItemRef, ");
            sb.Append("\r\n");
            sb.Append("	p.entry_date as EntryDate, ");
            sb.Append("\r\n");
            sb.Append("	c.entry_amt_allocated as EntryAmtAllocated,");
            sb.Append("\r\n");
            sb.Append("'" + invoiceNo + "'	as InvoiceNo");
            sb.Append("\r\n");
            sb.Append("FROM un_invoice_entries_new p ");
            sb.Append("\r\n");
            sb.Append("	INNER JOIN un_invoice_entries_new c ON p.sequence = c.sub_entry_join_cr_sequence ");
            sb.Append("\r\n");
            sb.Append("WHERE c.sub_entry_join_dr_sequence IN ");
            sb.Append("\r\n");
            sb.Append("	(SELECT sequence FROM un_invoice_entries_new ");
            sb.Append("\r\n");
            sb.AppendFormat("	 WHERE invoiceno_or_itemref = '{0}' AND trans_type = 'B'", invoiceNo);
            sb.Append("\r\n");
            sb.Append("	 ) ");
            sb.Append("\r\n");
            sb.Append("	 AND p.trans_type = 'B' ");
            sb.Append("\r\n");
            sb.Append("ORDER BY p.entry_date ASC");
            sb.Append("\r\n");

            return sb.ToString();
        }

        #endregion

        #region Get Auto Complete query

        internal static string GetJobRefAutoCompleteQuery(string databaseType, string searchText, int itemCount)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT Top " + itemCount + " job_ref as JobRef, job_address as JobAddress");
            sb.Append("\r\n");
            sb.Append("FROM un_orders ");
            sb.Append("\r\n");
            sb.AppendFormat("WHERE flg_cancelled = 0 ");
            sb.Append("\r\n");
            if (searchText != null)
            {
                sb.Append(" and " + DB_TAG_UTILLS.GetLikeQuery(databaseType, "job_ref", searchText));
                sb.Append("\r\n");
            }
            sb.Append(" ORDER BY job_ref ASC ");
            sb.Append("\r\n");

            return sb.ToString();
        }

        internal static string GetCompanyAutoComplete(string databaseType, string searchText, int itemCount)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT distinct Top " + itemCount + " edc.entity_id as id, edc.name_long as name ");
            sb.Append("\r\n");
            sb.Append("FROM un_invoice_entries_new AS ie_i, un_entity_details_core AS edc, un_entity_details_join AS edj");
            sb.Append("\r\n");
            sb.Append("WHERE edj.entity_id = edc.entity_id");
            sb.Append("\r\n");
            sb.Append("	AND ie_i.contact_id = edj.entity_id");
            sb.Append("\r\n");
            sb.Append("	AND edj.trans_type = 'B'");
            sb.Append("\r\n");
            sb.Append("	AND ie_i.trans_type = edj.trans_type");
            sb.Append("\r\n");
            sb.Append("	AND (ie_i.entry_type = 'SI' OR (ie_i.entry_type = 'SC' OR ie_i.entry_type = 'SA'))	");
            sb.Append("\r\n");
            if (searchText != null)
            {
                sb.Append(" AND " + DB_TAG_UTILLS.GetLikeQuery(databaseType, "edc.name_long", searchText));
                sb.Append("\r\n");
            }

            sb.Append("Order by edc.name_long ASC");
            sb.Append("\r\n");
            return sb.ToString();
        }

        #endregion
    }
}
