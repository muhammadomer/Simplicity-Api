using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.ClientInvoice.Entities;
using SimplicityOnlineWebApi.ClientInvoice.Models.RepositoryQuery.Queries;
using SimplicityOnlineWebApi.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;

namespace SimplicityOnlineWebApi.ClientInvoice.Models.RepositoryQuery
{
    public class ClientInvoiceDB : MainDB
    {
        public ClientInvoiceDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        internal static string JOB_CLIENT_REF_NULL = "Not Set";
        internal static string JOB_TRADE_CODE_NULL = "Not Set";
        internal static string SHOW_DROP_BOX_ITEM = "Invoices";

        internal BankDetailsCompanyNoVatNoResponse BankDetailForClient(int sequnceId, string invoiceNo)
        {
            Utilities.WriteLog("Method: BankDetailForClient Of ClientInvoiceDB List start : " + DateTime.Now);            
            BankDetailsCompanyNoVatNoResponse returnValue = null;

            Utilities.WriteLog("Request for DB Connection On:" + DateTime.Now);
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    Utilities.WriteLog("Get DB Connection On :" + DateTime.Now);
                    OleDbDataAdapter da = new OleDbDataAdapter();
                    
                    Utilities.WriteLog("Get Bank detail data : " + DateTime.Now);
                    using (OleDbCommand objCmdSelect = new OleDbCommand(ClientInvoiceQueries.GetBankDetailsQuery(sequnceId, invoiceNo), conn))
                    {
                        Utilities.WriteLog("Set Data Adapter For Selecting records :" + DateTime.Now);
                        da.SelectCommand = objCmdSelect;
                        DataTable dt = new DataTable();

                        Utilities.WriteLog("Request to Fill records on :" + DateTime.Now);
                        da.Fill(dt);

                        Utilities.WriteLog("Fill records on:" + DateTime.Now);
                        DataRow row = null;
                        if (dt.Rows != null && dt.Rows.Count > 0)
                        {
                            Utilities.WriteLog("Enter in loop to process rows on:" + DateTime.Now);
                            row = dt.Rows[0];
                            Utilities.WriteLog("Complete loop: " + DateTime.Now);
                        }
                        returnValue = ProcessForBankDetailData(row);
                    }
                    Utilities.WriteLog("Complete Get Bank detail data : " + DateTime.Now);
                }
                Utilities.WriteLog("End DB Connection On :" + DateTime.Now);
            }
            catch (Exception ex)
            {
                ErrorMessage = "Exception Occured While Getting BankDetailForClient " + ex.Message + " " + ex.InnerException;
                Utilities.WriteLog("Exception Occured While Getting BankDetailForClient " + ex.Message + " " + ex.InnerException);
            }
            return returnValue;
        }

        internal BankDetailsCompanyNoVatNoResponse ProcessForBankDetailData(DataRow row)
        {
            BankDetailsCompanyNoVatNoResponse model = new BankDetailsCompanyNoVatNoResponse();
            model.bankName = DBUtil.GetStringValue(row, "BankName") == null ? "" : DBUtil.GetStringValue(row, "BankName");
            model.bankSortCode = DBUtil.GetStringValue(row, "BankSortCode") == null ? "" : DBUtil.GetStringValue(row, "BankSortCode");
            model.bankAccountNo = DBUtil.GetStringValue(row, "BankAccountNo") == null ? "" : DBUtil.GetStringValue(row, "BankAccountNo");
            model.bankAccountName = DBUtil.GetStringValue(row, "BankAccountName") == null ? "" : DBUtil.GetStringValue(row, "BankAccountName");
            model.companyRegistrationNo = DBUtil.GetStringValue(row, "CompanyRegistrationNo") == null ? "" : DBUtil.GetStringValue(row, "CompanyRegistrationNo");
            model.vatNo = DBUtil.GetStringValue(row, "VatNo") == null ? "" : DBUtil.GetStringValue(row, "VatNo");
            return model;
        }

        #region Aged Dabator Section 

        internal List<CompanyAgedDabtorResponse> AgedDabatorList(ClientPageListRequest listRequest, out int count, bool isCountRequired, bool apsConfig)
        {
            Utilities.WriteLog("Method: AgedDabatorList of ClientInvoiceDB start : " + DateTime.Now);
            List<CompanyAgedDabtorResponse> returnValue = null;
            count = 0;
            try
            {
                Utilities.WriteLog("Get DB Connection On Of AgedDabatorList :" + DateTime.Now);
                using (OleDbConnection conn = this.getDbConnection())
                {
                    Utilities.WriteLog("Fetching AgedDabatorList itemcount start :" + DateTime.Now);
                    if (isCountRequired)
                    {
                        Utilities.WriteLog("Create Data Adapter For Couting Records Of AgedDabatorList start:" + DateTime.Now);
                        using (OleDbCommand objCmdSelect = new OleDbCommand(ClientInvoiceQueries.GetCountOfAgedDabatorList(this.DatabaseType), conn))
                        {
                            OleDbDataReader dr = objCmdSelect.ExecuteReader();
                            if (dr.HasRows)
                            {
                                Utilities.WriteLog("Fill count records Of AgedDabatorList start:" + DateTime.Now);
                                while (dr.Read())
                                {
                                    count = Convert.ToInt32(dr[0].ToString());
                                    Utilities.WriteLog("Total Return records Of AgedDabatorList are :" + count);
                                }
                                Utilities.WriteLog("Fill count records Of AgedDabatorList end:" + DateTime.Now);
                            }
                        }
                        Utilities.WriteLog("Create Data Adapter For Couting Records Of AgedDabatorList end:" + DateTime.Now);
                    }
                    Utilities.WriteLog("Fetching AgedDabatorList itemcount end :" + DateTime.Now);

                    Utilities.WriteLog("Fetching AgedDabatorList items start :" + DateTime.Now);
                    using (OleDbCommand objCmdSelect = new OleDbCommand(ClientInvoiceQueries.GetAgedDabatorList(this.DatabaseType), conn))
                    {
                        OleDbDataAdapter da = new OleDbDataAdapter();

                        Utilities.WriteLog("Set Data Adapter For Selecting records Of AgedDabatorList start  :" + DateTime.Now);
                        da.SelectCommand = objCmdSelect;
                        Utilities.WriteLog("Set Data Adapter For Selecting records Of AgedDabatorList end  :" + DateTime.Now);

                        Utilities.WriteLog("Request to Fill records Of AgedDabatorList start :" + DateTime.Now);
                        DataTable dt = new DataTable();
                        da.Fill((listRequest.size * listRequest.page), listRequest.size, dt);
                        Utilities.WriteLog("Request to Fill records Of AgedDabatorList end :" + DateTime.Now);

                        Utilities.WriteLog("Fill records Of AgedDabatorList start:" + DateTime.Now);
                        if (dt.Rows != null && dt.Rows.Count > 0)
                        {
                            returnValue = new List<CompanyAgedDabtorResponse>();
                            Utilities.WriteLog("Enter in loop to process rows Of AgedDabatorList start:" + DateTime.Now);
                            foreach (DataRow row in dt.Rows)
                            {
                                returnValue.Add(LoadAgedDabatorDataFromDataRow(row, apsConfig));
                            }
                            Utilities.WriteLog("Exit loop Of AgedDabatorList end:" + DateTime.Now);
                        }
                    }
                    Utilities.WriteLog("Fetching AgedDabatorList items end :" + DateTime.Now);
                }
                Utilities.WriteLog("Get DB Connection Of AgedDabatorList end :" + DateTime.Now);
            }
            catch (Exception ex)
            {
                ErrorMessage = "Exception Occured While Getting AgedDabatorList List. " + ex.Message + " " + ex.InnerException;
                Utilities.WriteLog("Exception Occured While Getting AgedDabatorList List. " + ex.Message + " " + ex.InnerException);
            }
            Utilities.WriteLog("Method: AgedDabatorList of ClientInvoiceDB end : " + DateTime.Now);
            return returnValue;
        }

        internal CompanyAgedDabtorResponse LoadAgedDabatorDataFromDataRow(DataRow row, bool apsConfig)
        {
            CompanyAgedDabtorResponse returnValue = null;
            try
            {
                if (row != null)
                {
                    returnValue = new CompanyAgedDabtorResponse();
                    returnValue.entityId = DBUtil.GetLongValue(row, "entityId");
                    returnValue.companyName = DBUtil.GetStringValue(row, "companyName");
                    returnValue.forename = DBUtil.GetStringValue(row, "Forename");
                    returnValue.surname = DBUtil.GetStringValue(row, "Surname");
                    returnValue.telephone = DBUtil.GetStringValue(row, "Telephone");
                    returnValue.telMobile = DBUtil.GetStringValue(row, "TelMobile");
                    returnValue.email = DBUtil.GetStringValue(row, "Email");
                    returnValue.total = DBUtil.GetDecimalValue(row, "Total");
                    returnValue.days30Amount = DBUtil.GetDecimalValue(row, "Days30Amount");
                    returnValue.days60Amount = DBUtil.GetDecimalValue(row, "Days60Amount");
                    returnValue.days90Amount = DBUtil.GetDecimalValue(row, "Days90Amount");
                    returnValue.olderAmount = DBUtil.GetDecimalValue(row, "OlderAmount");
                    returnValue.foreSurname = DBUtil.GetStringValue(row, "Forename") + " " + DBUtil.GetStringValue(row, "Surname");
                    Utilities.WriteLog("Add item which item id is " + returnValue.entityId + " at : " + DateTime.Now);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteLog("Exception Occured While Getting LoadAgedDabatorDataFromDataRow. " + ex.Message + " " + ex.InnerException + " " + DateTime.Now);
            }
            return returnValue;
        }

        #endregion

        #region Statement Data Load

        internal StatementListResponse StatementList(Parameters parameters, bool apsConfig)
        {
            Utilities.WriteLog("Method: StatementList Of ClientInvoiceDB List start : " + DateTime.Now);
            List<StatementDBResponse> allStatementData = null;
            StatementListResponse returnValue = null;

            try
            {
                Utilities.WriteLog("Get DB Connection of StatementList start :" + DateTime.Now);
                using (OleDbConnection conn = this.getDbConnection())
                {
                    Utilities.WriteLog("Fetching AgedDabatorList items start :" + DateTime.Now);
                    OleDbDataAdapter da = new OleDbDataAdapter();
                    using (OleDbCommand objCmdSelect = new OleDbCommand(ClientInvoiceQueries.GetStatementList(this.DatabaseType, parameters), conn))
                    {
                        Utilities.WriteLog("Set Data Adapter For Selecting records Of StatementList start :" + DateTime.Now);
                        da.SelectCommand = objCmdSelect;
                        Utilities.WriteLog("Set Data Adapter For Selecting records Of StatementList end :" + DateTime.Now);

                        Utilities.WriteLog("Request to Fill records Of StatementList start :" + DateTime.Now);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        if (dt.Rows != null && dt.Rows.Count > 0)
                        {
                            allStatementData = new List<StatementDBResponse>();
                            returnValue = new StatementListResponse();

                            Utilities.WriteLog("Enter in loop to process rows Of StatementList on:" + DateTime.Now);
                            foreach (DataRow row in dt.Rows)
                            {
                                allStatementData.Add(LoadStatementDataFromDataRow(row, apsConfig));
                            }
                            Utilities.WriteLog("Exit loop Of StatementList :" + DateTime.Now);
                        }
                        Utilities.WriteLog("Request to Fill records Of StatementList end :" + DateTime.Now);

                        Utilities.WriteLog("Statementdata convert into response data start : " + DateTime.Now);
                        PrepareStatementDataAsPerBalanceCalculation(allStatementData, parameters, returnValue);
                        Utilities.WriteLog("Statementdata convert into response data end : " + DateTime.Now);
                    }
                    Utilities.WriteLog("Fetching AgedDabatorList items end :" + DateTime.Now);
                }
                Utilities.WriteLog("Get DB Connection of StatementList end :" + DateTime.Now);
            }
            catch (Exception ex)
            {
                Utilities.WriteLog("Exception Occured While Getting StatementList. " + ex.Message + " " + ex.InnerException);
            }

            Utilities.WriteLog("Method: StatementList Of ClientInvoiceDB List end : " + DateTime.Now);
            return returnValue;
        }

        internal StatementDBResponse LoadStatementDataFromDataRow(DataRow row, bool apsConfig)
        {
            StatementDBResponse returnValue = null;
            try
            {
                if (row != null)
                {
                    returnValue = new StatementDBResponse();
                    returnValue.jobRef = DBUtil.GetStringValue(row, "JobRef");
                    returnValue.jobAddress = DBUtil.GetStringValue(row, "JobAddress");
                    returnValue.client = DBUtil.GetStringValue(row, "Client");
                    returnValue.invoicenoOrItemRef = DBUtil.GetStringValue(row, "InvoicenoOrItemRef");
                    returnValue.entryType = DBUtil.GetStringValue(row, "EntryType");
                    returnValue.entryDate = DBUtil.GetDateTimeValue(row, "EntryDate");
                    returnValue.entryAmtTotal = DBUtil.GetDecimalValue(row, "EntryAmtTotal");
                    returnValue.entryDetails = DBUtil.GetStringValue(row, "EntryDetails");
                    returnValue.sequence = DBUtil.GetStringValue(row, "Sequence");
                    Utilities.WriteLog("StatementDbresponse of entryDate" + returnValue.entryDate + " at :" + DateTime.Now);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteLog("Exception Occured While Getting LoadStatementDataFromDataRow . " + ex.Message + " " + ex.InnerException + " " + DateTime.Now);
            }
            return returnValue;
        }

        internal StatementListResponse PrepareStatementDataAsPerBalanceCalculation(List<StatementDBResponse> statementData, Parameters parameters, StatementListResponse returnResponse)
        {
            if (statementData == null || statementData.Count <= 0)
            {
                returnResponse.data = new List<StatementResponse>();
                returnResponse.openingBalance = 0;
                returnResponse.closingBalance = 0;
            }

            Utilities.WriteLog("Converting data as per Balance Calculation : Start " + DateTime.Now);

            List<StatementDBResponse> openingBalanceData = statementData.Where(x => x.entryDate.Value < ((DateTime)parameters.startDate)).ToList();
            List<StatementDBResponse> statementBalanceData = statementData.Where(x => x.entryDate.Value >= ((DateTime)parameters.startDate)).ToList();

            returnResponse.openingBalance = GetOpeningBalance(openingBalanceData, parameters.startDate);
            returnResponse = GetStatementData(returnResponse, statementBalanceData, parameters.startDate);

            Utilities.WriteLog("Converting data as per Balance Calculation : End " + DateTime.Now);
            return returnResponse;
        }

        internal decimal GetOpeningBalance(List<StatementDBResponse> response, DateTime? startDate)
        {
            decimal openingBalance = 0;
            try
            {
                if (response != null)
                {
                    foreach (StatementDBResponse item in response)
                    {
                        decimal amount = 0;
                        if (item.entryType == "SI")
                            amount = Convert.ToDecimal(item.entryAmtTotal);
                        else
                            amount = (Convert.ToDecimal(item.entryAmtTotal) * -1);

                        openingBalance = (openingBalance + amount);
                        Utilities.WriteLog("Add opening balance of " + item.entryDate + " at : " + DateTime.Now);
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteLog("Exception Occured While Getting GetOpeningBalance. " + ex.Message + " " + ex.InnerException + " " + DateTime.Now);
            }
            Utilities.WriteLog("Statement opening balance is end : " + DateTime.Now);
            return openingBalance;
        }

        internal StatementListResponse GetStatementData(StatementListResponse responsemodel, List<StatementDBResponse> statementData, DateTime? startDate)
        {
            try
            {
                List<StatementResponse> data = new List<StatementResponse>();
                int srNo = 0;
                decimal? closingBalance = responsemodel.openingBalance;
                foreach (StatementDBResponse item in statementData)
                {
                    srNo = srNo + 1;
                    decimal amount = 0;
                    if (item.entryType == "SI")
                        amount = Convert.ToDecimal(item.entryAmtTotal);
                    else
                        amount = (Convert.ToDecimal(item.entryAmtTotal) * -1);

                    closingBalance = (closingBalance + amount);
                    Utilities.WriteLog("Add closing balance of " + item.entryDate + " at : " + DateTime.Now);

                    data.Add(MakeStatementData(srNo, item, closingBalance, amount));
                }
                responsemodel.closingBalance = closingBalance;
                responsemodel.data = data;
            }
            catch (Exception ex)
            {
                Utilities.WriteLog("Exception Occured While Getting GetStatementData. " + ex.Message + " " + ex.InnerException + " " + DateTime.Now);
            }
            return responsemodel;
        }

        internal StatementResponse MakeStatementData(int srno, StatementDBResponse item, decimal? balance, decimal? amount)
        {
            StatementResponse model = new StatementResponse();
            try
            {
                model.srNo = srno;
                model.jobRef = item.jobRef;
                model.jobAddress = item.jobAddress;
                model.client = item.client;
                model.invoicenoOrItemRef = item.invoicenoOrItemRef;
                model.entryType = item.entryType;
                model.sequence = item.sequence;
                model.date = item.entryDate != null ? item.entryDate.Value.ToString("dd/MM/yyyy") : null;

                if (item.entryType == "SI")
                    model.refText = item.invoicenoOrItemRef;
                else
                {
                    if (item.entryDetails.Replace("Not Set", "").Length > 1)
                        model.refText = item.invoicenoOrItemRef + "-" + item.entryDetails;
                    else
                        model.refText = item.invoicenoOrItemRef;
                }

                if (amount < 0)
                    model.firstBalance = (amount * -1);
                if (amount >= 0)
                    model.secondBalance = amount;

                model.balance = balance;
            }
            catch (Exception ex)
            {
                Utilities.WriteLog("Exception Occured While Getting MakeStatementData. " + ex.Message + " " + ex.InnerException + " " + DateTime.Now);
            }
            return model;
        }

        #endregion

        #region All Items & Unpaid Items Section

        internal InvoiceListResponse<InvoiceResponse> AllAndUnPaidItemsList(string basePath, ClientPageListRequest listRequest, out int count, bool isCountRequired, bool apsConfig, Parameters parameters, string projectId)
        {
            string methodCallFor = (parameters.status == "open" ? "Unpaid " : " All ");
            Utilities.WriteLog("Method: AllAndUnPaidItemsList of ClientInvoiceDB for " + methodCallFor + "items start : " + DateTime.Now);

            InvoiceListResponse<InvoiceResponse> returnValue = null;
            List<InvoiceResponse> data = null;
            count = 0;
            try
            {
                Utilities.WriteLog("Get DB Connection of " + methodCallFor + "items start :" + DateTime.Now);
                using (OleDbConnection conn = this.getDbConnection())
                {
                    Utilities.WriteLog("Fetching " + methodCallFor + "item's itemcount start :" + DateTime.Now);
                    if (isCountRequired)
                    {
                        Utilities.WriteLog("Create Data Adapter For Couting Records Of " + methodCallFor + "item's itemcount start :" + DateTime.Now);
                        using (OleDbCommand objCmdSelect = new OleDbCommand(ClientInvoiceQueries.GetInvoiceList(this.DatabaseType, parameters, true), conn))
                        {
                            OleDbDataReader dr = objCmdSelect.ExecuteReader();
                            if (dr.HasRows)
                            {
                                Utilities.WriteLog("Fill count records Of " + methodCallFor + "item's start:" + DateTime.Now);
                                while (dr.Read())
                                {
                                    count = Convert.ToInt32(dr[0].ToString());
                                    Utilities.WriteLog("Total Return records of " + methodCallFor + "item's are : " + count);
                                }
                                Utilities.WriteLog("Fill count records Of " + methodCallFor + "item's end:" + DateTime.Now);
                            }
                        }
                        Utilities.WriteLog("Create Data Adapter For Couting Records Of " + methodCallFor + "item's itemcount end :" + DateTime.Now);
                    }
                    Utilities.WriteLog("Fetching " + methodCallFor + "item's itemcount end :" + DateTime.Now);

                    Utilities.WriteLog("Fetching " + methodCallFor + "items start :" + DateTime.Now);
                    using (OleDbCommand objCmdSelect = new OleDbCommand(ClientInvoiceQueries.GetInvoiceList(this.DatabaseType, parameters, false), conn))
                    {
                        Utilities.WriteLog("Set Data Adapter For Selecting records Of " + methodCallFor + "items start  :" + DateTime.Now);
                        OleDbDataAdapter da = new OleDbDataAdapter();
                        da.SelectCommand = objCmdSelect;
                        Utilities.WriteLog("Set Data Adapter For Selecting records Of " + methodCallFor + "items end  :" + DateTime.Now);

                        Utilities.WriteLog("Request to Fill records Of " + methodCallFor + "items start :" + DateTime.Now);
                        DataTable dt = new DataTable();
                        da.Fill((listRequest.size * listRequest.page), listRequest.size, dt);
                        Utilities.WriteLog("Request to Fill records Of " + methodCallFor + "items end :" + DateTime.Now);

                        Utilities.WriteLog("Fill records Of " + methodCallFor + "items start:" + DateTime.Now);
                        if (dt.Rows != null && dt.Rows.Count > 0)
                        {
                            data = new List<InvoiceResponse>();
                            Utilities.WriteLog("Enter in loop to process rows Of " + methodCallFor + "items start:" + DateTime.Now);
                            foreach (DataRow row in dt.Rows)
                            {
                                data.Add(LoadInvoiceItemsFromDataRow(basePath, row, apsConfig, projectId));
                            }
                            Utilities.WriteLog("Exit loop Of " + methodCallFor + "items end:" + DateTime.Now);
                        }
                        Utilities.WriteLog("Fill records Of " + methodCallFor + "items end:" + DateTime.Now);

                        Utilities.WriteLog("Fill records Of " + methodCallFor + "items Page respone start:" + DateTime.Now);
                        returnValue = ProcessForPageListResponse(data, count, listRequest);
                        Utilities.WriteLog("Fill records Of " + methodCallFor + "items Page respone end:" + DateTime.Now);
                    }
                    Utilities.WriteLog("Fetching " + methodCallFor + "items end :" + DateTime.Now);
                }
                Utilities.WriteLog("Get DB Connection of " + methodCallFor + "items end :" + DateTime.Now);
            }
            catch (Exception ex)
            {
                ErrorMessage = "Exception Occured While Getting " + methodCallFor + "items List. " + ex.Message + " " + ex.InnerException;
                Utilities.WriteLog("Exception Occured While Getting " + methodCallFor + "items List. " + ex.Message + " " + ex.InnerException);
            }

            Utilities.WriteLog("Method: AllAndUnPaidItemsList of ClientInvoiceDB for " + methodCallFor + "items end : " + DateTime.Now);
            return returnValue;
        }

        internal InvoiceResponse LoadInvoiceItemsFromDataRow(string basePath, DataRow row, bool apsConfig, string projectId)
        {
            InvoiceResponse returnValue = null;
            try
            {
                if (row != null)
                {
                    returnValue = new InvoiceResponse();
                    returnValue.entityId = DBUtil.GetLongValue(row, "entityId");
                    returnValue.clientName = DBUtil.GetStringValue(row, "ClientName");
                    returnValue.jobRef = DBUtil.GetStringValue(row, "JobRef");
                    returnValue.jobAddress = DBUtil.GetStringValue(row, "JobAddress");
                    returnValue.sequence = DBUtil.GetLongValue(row, "Sequence");
                    returnValue.invoiceNo = DBUtil.GetStringValue(row, "InvoiceNo");
                    returnValue.invoiceDate = DBUtil.GetDateTimeValueStr(row, "InvoiceDate");
                    returnValue.amountSubTotal = DBUtil.GetDecimalValue(row, "AmountSubTotal");
                    returnValue.amountTotal = DBUtil.GetDecimalValue(row, "AmountTotal");
                    returnValue.amountVat = DBUtil.GetDecimalValue(row, "AmountVat");
                    returnValue.entryAmtTotal = DBUtil.GetDecimalValue(row, "EntryAmtTotal");
                    returnValue.entryAmtAllocated = DBUtil.GetDecimalValue(row, "EntryAmtAllocated");
                    returnValue.amtOutstanding = DBUtil.GetDecimalValue(row, "AmtOutstanding");
                    returnValue.inv = DBUtil.GetStringValue(row, "Inv");
                    try
                    {
                        if (returnValue.inv == "0")
                        {
                            //ConfigModel config = GetCompanyConfiguration(projectId, basePath);
                            returnValue.invStatus = "APC";
                        }
                        else if (Convert.ToDouble(returnValue.amtOutstanding) > 0.01)
                            returnValue.invStatus = "O/S";
                        else
                            returnValue.invStatus = "SETTLED";
                    }
                    catch
                    {
                        returnValue.invStatus = "";
                    }

                    returnValue.flgSettled = Convert.ToBoolean(row["FlgSettled"]);
                    Utilities.WriteLog("Method LoadInvoiceItemsFromDataRow of " + returnValue.sequence + " at :" + DateTime.Now);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteLog("Exception Occured While Getting LoadInvoiceItemsFromDataRow. " + ex.Message + " " + ex.InnerException + " " + DateTime.Now);
            }
            return returnValue;
        }

        internal InvoiceListResponse<InvoiceResponse> ProcessForPageListResponse(List<InvoiceResponse> invoiceData, int itemCount, ClientPageListRequest listRequest)
        {
            InvoiceListResponse<InvoiceResponse> invoiceListResponse = new InvoiceListResponse<InvoiceResponse>();
            try
            {
                if (invoiceData != null)
                {
                    if (invoiceData.Count > 0)
                    {
                        InvoiceTotalResponse pageTotal = new InvoiceTotalResponse();
                        pageTotal.netTotal = invoiceData.ToList().Sum(x => x.amountSubTotal);//amount_sub_total
                        pageTotal.vatTotal = invoiceData.ToList().Sum(x => x.amountVat); // amount_vat
                        pageTotal.grossTotal = invoiceData.ToList().Sum(x => x.amountTotal); //amount_total
                        pageTotal.osTotal = invoiceData.ToList().Sum(x => x.amtOutstanding); //amt_outstanding

                        InvoiceTotalResponse allPageTotal = new InvoiceTotalResponse();
                        allPageTotal.netTotal = invoiceData.ToList().Sum(x => x.amountSubTotal);//amount_sub_total
                        allPageTotal.vatTotal = invoiceData.ToList().Sum(x => x.amountVat); // amount_vat
                        allPageTotal.grossTotal = invoiceData.ToList().Sum(x => x.amountTotal); //amount_total
                        allPageTotal.osTotal = invoiceData.ToList().Sum(x => x.amtOutstanding); //amt_outstanding

                        invoiceListResponse.data = invoiceData.ToArray();
                        invoiceListResponse.summary = new SummaryTotalResponse(pageTotal, allPageTotal);
                        invoiceListResponse.totalRecords = itemCount;
                        invoiceListResponse.currenPage = listRequest.page;
                        invoiceListResponse.pageSize = listRequest.size;
                        invoiceListResponse.currenPageRecords = invoiceData.Count();

                        return invoiceListResponse;
                    }
                    else
                        return BlankResponse(invoiceListResponse, listRequest);
                }
                else
                    return BlankResponse(invoiceListResponse, listRequest);
            }
            catch (Exception ex)
            {
                Utilities.WriteLog("Exception Occured While Getting ProcessForPageListResponse. " + ex.Message + " " + ex.InnerException + " " + DateTime.Now);
            }
            return BlankResponse(invoiceListResponse, listRequest);
        }

        internal InvoiceListResponse<InvoiceResponse> BlankResponse(InvoiceListResponse<InvoiceResponse> invoiceListResponse, ClientPageListRequest listRequest)
        {
            invoiceListResponse.data = new List<InvoiceResponse>().ToArray();
            invoiceListResponse.summary = new SummaryTotalResponse();
            invoiceListResponse.totalRecords = 0;
            invoiceListResponse.currenPage = listRequest.page;
            invoiceListResponse.pageSize = listRequest.size;
            invoiceListResponse.currenPageRecords = 0;
            return invoiceListResponse;
        }

        #endregion

        #region Get Invoice Detail

        internal InvoiceDetailResponse GetInvoiceDetail(string projectId, string basePath, int sequenceId, string invoiceNo)
        {
            InvoiceDetailResponse returnValue = null;
            Utilities.WriteLog("Request for DB Connection On:" + DateTime.Now);
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    Utilities.WriteLog("Get DB Connection On :" + DateTime.Now);
                    OleDbDataAdapter da = new OleDbDataAdapter();

                    #region Common Data
                    Utilities.WriteLog("Get Invoice Detail Common Data : " + DateTime.Now);
                    using (OleDbCommand objCmdSelect = new OleDbCommand(ClientInvoiceQueries.GetInvoiceDetailCommanData(sequenceId, invoiceNo), conn))
                    {
                        Utilities.WriteLog("Set Data Adapter For Selecting records :" + DateTime.Now);
                        da.SelectCommand = objCmdSelect;
                        DataTable dt = new DataTable();

                        Utilities.WriteLog("Request to Fill records on :" + DateTime.Now);
                        da.Fill(dt);

                        Utilities.WriteLog("Fill records on:" + DateTime.Now);
                        DataRow commonRow = null;
                        if (dt.Rows != null && dt.Rows.Count > 0)
                        {
                            Utilities.WriteLog("Enter in loop to process rows on:" + DateTime.Now);
                            commonRow = dt.Rows[0];
                            Utilities.WriteLog("Complete Invoice Detail Common Data : " + DateTime.Now);
                        }
                        returnValue = ProcessCommonData(commonRow, projectId, basePath);
                    }
                    Utilities.WriteLog("Complete Get Invoice Detail Items : " + DateTime.Now);

                    #endregion

                    if (returnValue.invoiceNo != null && returnValue.invoiceNo != "")
                    {
                        #region Invoice Detail Items

                        Utilities.WriteLog("Get Invoice Detail Items : " + DateTime.Now);
                        List<InvoiceDetailItemsResponse> invoiceItems = null;
                        using (OleDbCommand objCmdSelect = new OleDbCommand(ClientInvoiceQueries.GetInvoiceDetailItems(sequenceId, invoiceNo), conn))
                        {
                            Utilities.WriteLog("Set Data Adapter For Selecting records :" + DateTime.Now);
                            da.SelectCommand = objCmdSelect;
                            DataTable dt = new DataTable();

                            Utilities.WriteLog("Request to Fill records on :" + DateTime.Now);
                            da.Fill(dt);

                            Utilities.WriteLog("Fill records on:" + DateTime.Now);
                            if (dt.Rows != null && dt.Rows.Count > 0)
                            {
                                invoiceItems = new List<InvoiceDetailItemsResponse>();

                                Utilities.WriteLog("Enter in loop to process rows on:" + DateTime.Now);
                                foreach (DataRow row in dt.Rows)
                                {
                                    invoiceItems.Add(ProcessOfntoInvoiceItems(row));
                                }
                                Utilities.WriteLog("Exit loop on:" + DateTime.Now);

                                returnValue.invoiceItems = invoiceItems;
                            }
                            else
                            {
                                ErrorMessage = SimplicityConstants.MESSAGE_NO_RECORD_FOUND;
                            }
                        }
                        Utilities.WriteLog("Complete Invoice Detail Items : " + DateTime.Now);

                        #endregion

                        #region Invoice Payment Items

                        Utilities.WriteLog("Get Invoice Detail Payment Items : " + DateTime.Now);
                        List<InvoiceDetailPaymentResponse> payments = null;
                        using (OleDbCommand objCmdSelect = new OleDbCommand(ClientInvoiceQueries.GetInvoiceNoWisePayment(returnValue.invoiceNo), conn))
                        {
                            Utilities.WriteLog("Set Data Adapter For Selecting records :" + DateTime.Now);
                            da.SelectCommand = objCmdSelect;
                            DataTable dt = new DataTable();

                            Utilities.WriteLog("Request to Fill records on :" + DateTime.Now);
                            da.Fill(dt);

                            Utilities.WriteLog("Fill records on:" + DateTime.Now);
                            if (dt.Rows != null && dt.Rows.Count > 0)
                            {
                                payments = new List<InvoiceDetailPaymentResponse>();

                                Utilities.WriteLog("Enter in loop to process rows on:" + DateTime.Now);
                                foreach (DataRow row in dt.Rows)
                                {
                                    payments.Add(ProcessOfInvoicePaymentItems(row));
                                }
                                Utilities.WriteLog("Exit loop on:" + DateTime.Now);

                                if (payments != null && payments.Count > 0)
                                {
                                    returnValue.invoicePayments = payments;
                                    UpdatePaymentStatus(returnValue, payments);
                                }
                            }
                            else
                            {
                                ErrorMessage = SimplicityConstants.MESSAGE_NO_RECORD_FOUND;
                            }
                        }
                        Utilities.WriteLog("Complete Invoice Detail Payment Items : " + DateTime.Now);

                        #endregion
                    }
                    else
                    {
                        returnValue.invoiceItems = new List<InvoiceDetailItemsResponse>();
                        returnValue.invoicePayments = new List<InvoiceDetailPaymentResponse>();
                    }
                }
                Utilities.WriteLog("End DB Connection On :" + DateTime.Now);
            }
            catch (Exception ex)
            {
                ErrorMessage = "Exception Occured While Getting GetAllAgedDabutors List. " + ex.Message + " " + ex.InnerException;
                Utilities.WriteLog("Exception Occured While Getting GetAllAgedDabutors List. " + ex.Message + " " + ex.InnerException);
            }
            return returnValue;
        }

        internal string GetCompanyAddress()
        {
            string companyAddress = "";
            Utilities.WriteLog("Request for DB Connection On:" + DateTime.Now);
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    Utilities.WriteLog("Get DB Connection On :" + DateTime.Now);
                    OleDbDataAdapter da = new OleDbDataAdapter();

                    #region Common Data
                    Utilities.WriteLog("Get Company Address Value : " + DateTime.Now);
                    using (OleDbCommand objCmdSelect = new OleDbCommand(ClientInvoiceQueries.GetCompanyAddressQuery(), conn))
                    {
                        Utilities.WriteLog("Set Data Adapter For Selecting records :" + DateTime.Now);
                        da.SelectCommand = objCmdSelect;
                        DataTable dt = new DataTable();

                        Utilities.WriteLog("Request to Fill records on :" + DateTime.Now);
                        da.Fill(dt);

                        Utilities.WriteLog("Fill records on:" + DateTime.Now);
                        if (dt.Rows != null && dt.Rows.Count > 0)
                        {
                            if (DBUtil.GetStringValue(dt.Rows[0], "AddressFull") != null)
                                companyAddress = "<p>" + DBUtil.GetStringValue(dt.Rows[0], "AddressFull").Replace("\r\n", "<br/>") + "</p>";
                            else
                                companyAddress = DBUtil.GetStringValue(dt.Rows[0], "AddressFull");
                        }
                    }
                    Utilities.WriteLog("Complete Company Address Value : " + DateTime.Now);

                    #endregion
                }
            }
            catch
            { }
            return companyAddress;
        }

        internal CompanyAddressWithDetailResponse GetCompanyAddressInDetail()
        {
            CompanyAddressWithDetailResponse companyAddressDetail = new CompanyAddressWithDetailResponse();
            Utilities.WriteLog("Request for DB Connection On:" + DateTime.Now);
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    Utilities.WriteLog("Get DB Connection On :" + DateTime.Now);
                    OleDbDataAdapter da = new OleDbDataAdapter();

                    #region Common Data
                    Utilities.WriteLog("Get Company Address Details Value : " + DateTime.Now);
                    using (OleDbCommand objCmdSelect = new OleDbCommand(ClientInvoiceQueries.GetCompanyAddressInDetailQuery(), conn))
                    {
                        Utilities.WriteLog("Set Data Adapter For Selecting records :" + DateTime.Now);
                        da.SelectCommand = objCmdSelect;
                        DataTable dt = new DataTable();

                        Utilities.WriteLog("Request to Fill records on :" + DateTime.Now);
                        da.Fill(dt);

                        Utilities.WriteLog("Fill records on:" + DateTime.Now);
                        if (dt.Rows != null && dt.Rows.Count > 0)
                        {
                            companyAddressDetail.AddressNo = DBUtil.GetStringValue(dt.Rows[0], "AddressNo");
                            companyAddressDetail.AddressLine1 = DBUtil.GetStringValue(dt.Rows[0], "AddressLine1");
                            companyAddressDetail.AddressLine2 = DBUtil.GetStringValue(dt.Rows[0], "AddressLine2");
                            companyAddressDetail.AddressLine3 = DBUtil.GetStringValue(dt.Rows[0], "AddressLine3");
                            companyAddressDetail.AddressLine4 = DBUtil.GetStringValue(dt.Rows[0], "AddressLine4");
                            companyAddressDetail.AddressLine5 = DBUtil.GetStringValue(dt.Rows[0], "AddressLine5");
                            companyAddressDetail.AddressFull = DBUtil.GetStringValue(dt.Rows[0], "AddressFull");
                            if (companyAddressDetail.AddressFull != null)
                            {
                                companyAddressDetail.AddressFull = "<p>" + companyAddressDetail.AddressFull.Replace("\r\n", "<br/>") + "</p>";
                            }
                        }
                    }
                    Utilities.WriteLog("Complete Company Address Details Value : " + DateTime.Now);

                    #endregion
                }
            }
            catch
            { }
            return companyAddressDetail;
        }

        internal InvoiceDetailResponse ProcessCommonData(DataRow row, string projectId, string basePath)
        {
            InvoiceDetailResponse model = new InvoiceDetailResponse();

            model.invoiceType = DBUtil.GetStringValue(row, "invoiceType") == null ? "" : DBUtil.GetStringValue(row, "invoiceType");
            model.jobRef = DBUtil.GetStringValue(row, "JobRef") == null ? "" : DBUtil.GetStringValue(row, "JobRef");
            model.jobClientRef = (DBUtil.GetStringValue(row, "JobClientRef") == null) ? JOB_CLIENT_REF_NULL : DBUtil.GetStringValue(row, "JobClientRef");
            model.jobTradeCode = (DBUtil.GetStringValue(row, "JobTradeCode") == null) ? JOB_TRADE_CODE_NULL : DBUtil.GetStringValue(row, "JobTradeCode");
            model.jobDateStart = DBUtil.GetStringValue(row, "JobDateStart") == null ? "" : DBUtil.GetStringValue(row, "JobDateStart");
            model.jobDateFinish = DBUtil.GetStringValue(row, "JobDateFinish") == null ? "" : DBUtil.GetStringValue(row, "JobDateFinish");
            model.invoiceNo = DBUtil.GetStringValue(row, "InvoiceNo") == null ? "" : DBUtil.GetStringValue(row, "InvoiceNo");
            model.invoiceDate = DBUtil.GetDateTimeValueStr(row, "InvoiceDate") == null ? "" : DBUtil.GetDateTimeValueStr(row, "InvoiceDate");

            string maillingAddress = DBUtil.GetStringValue(row, "MaillingAddress") == null ? "" : DBUtil.GetStringValue(row, "maillingAddress");
            if (!string.IsNullOrEmpty(maillingAddress))
                model.maillingAddress = maillingAddress.Replace("\r\n", "<br/>");            
            
            model.subtotalScheduledItems = DBUtil.GetDecimalValue(row, "SubtotalScheduledItems");
            model.invoiceAmount = DBUtil.GetDecimalValue(row, "InvoiceAmount");
            model.discount = DBUtil.GetDecimalValue(row, "Discount");
            model.retentionTotal = DBUtil.GetDecimalValue(row, "RetentionTotal");
            model.subTotal = DBUtil.GetDecimalValue(row, "SubTotal");
            model.vat = DBUtil.GetDecimalValue(row, "Vat");
            model.invoiceTotal = DBUtil.GetDecimalValue(row, "InvoiceTotal");
            
            model.nameLong = DBUtil.GetStringValue(row, "NameLong") == null ? "" : DBUtil.GetStringValue(row, "NameLong");
            model.jobAddress = DBUtil.GetStringValue(row, "JobAddress") == null ? "" : DBUtil.GetStringValue(row, "JobAddress");
            model.footNote = DBUtil.GetStringValue(row, "FootNote") == null ? "" : DBUtil.GetStringValue(row, "FootNote").Trim();
            model.flgSetToInvoice = DBUtil.GetBooleanValue(row, "FlgSetToInvoice");

            if (projectId != null && projectId.Length > 0)
            {
                try
                {
                    //ConfigModel config = GetCompanyConfiguration(projectId, basePath);
                    //model.Invaddr = config.invaddr;
                    CompanyAddressWithDetailResponse companyDetails = GetCompanyAddressInDetail();
                    model.invaddr = (companyDetails != null && companyDetails.AddressFull != null) ? companyDetails.AddressFull : GetCompanyAddress();
                    model.companyAddressInDetail = GetCompanyAddressInDetail();
                    if (model.flgSetToInvoice != true)
                    {
                        //model.invoiceDetailMainTitlePretext = config.appn;
                        model.invoiceDetailMainTitlePretext = "Application for Payment";
                    }
                    else
                        model.invoiceDetailMainTitlePretext = SHOW_DROP_BOX_ITEM;
                }
                catch
                {
                    model.invaddr = "";
                    model.invoiceDetailMainTitlePretext = "";
                }
            }
            return model;
        }

        internal InvoiceDetailItemsResponse ProcessOfntoInvoiceItems(DataRow row)
        {
            InvoiceDetailItemsResponse returnValue = null;
            if (row != null)
            {
                returnValue = new InvoiceDetailItemsResponse();

                returnValue.itemQuantity = DBUtil.GetStringValue(row, "ItemQuantity") == null ? "0" : DBUtil.GetStringValue(row, "ItemQuantity");

                returnValue.itemCode = DBUtil.GetStringValue(row, "ItemCode");
                returnValue.itemDesc = DBUtil.GetStringValue(row, "ItemDesc");

                
                returnValue.itemUnits = DBUtil.GetStringValue(row, "ItemUnits");
                returnValue.amountPayment = DBUtil.GetDecimalValue(row, "AmountPayment");

                returnValue.amountVat = DBUtil.GetDecimalValue(row, "AmountVat");
                returnValue.amountDiscount = DBUtil.GetDecimalValue(row, "AmountDiscount");
                returnValue.amountSubTotal = DBUtil.GetDecimalValue(row, "AmountSubTotal");
                returnValue.amountRetention = DBUtil.GetDecimalValue(row, "AmountRetention");

                returnValue.flgRowIsText = DBUtil.GetBooleanValue(row, "FlgRowIsText");
            }
            return returnValue;
        }

        internal InvoiceDetailPaymentResponse ProcessOfInvoicePaymentItems(DataRow row)
        {
            InvoiceDetailPaymentResponse returnValue = null;
            if (row != null)
            {
                returnValue = new InvoiceDetailPaymentResponse();
                returnValue.entryDate = DBUtil.GetDateTimeValueStr(row, "EntryDate");
                returnValue.entryType = DBUtil.GetStringValue(row, "EntryType");
                returnValue.invoiceNo = DBUtil.GetStringValue(row, "InvoiceNo");
                returnValue.invoicenoOrItemRef = DBUtil.GetStringValue(row, "InvoicenoOrItemRef");
                returnValue.entryAmtAllocated = DBUtil.GetDecimalValue(row, "EntryAmtAllocated");
            }

            return returnValue;
        }

        internal void UpdatePaymentStatus(InvoiceDetailResponse returnValue, List<InvoiceDetailPaymentResponse> payments)
        {
            decimal pt = 0;
            foreach (InvoiceDetailPaymentResponse item in payments)
            {
                pt = pt + Convert.ToDecimal(item.entryAmtAllocated);
            }
            if (pt >= Convert.ToDecimal(returnValue.invoiceTotal))
                returnValue.paymentStatus = "PAID";
            
            returnValue.paidToDate = pt;
        }

        #endregion

        #region Client Configuration 

        internal List<JobRefAutoCompleteResponse> GetJobRefAutoComplete(string projectId, string searchText, int itemCount)
        {
            List<JobRefAutoCompleteResponse> returnValue = null;
            Utilities.WriteLog("Request for DB Connection On:" + DateTime.Now);
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    Utilities.WriteLog("Get DB Connection On :" + DateTime.Now);
                    OleDbDataAdapter da = new OleDbDataAdapter();

                    Utilities.WriteLog("Get Job Ref Data : " + DateTime.Now);
                    using (OleDbCommand objCmdSelect = new OleDbCommand(ClientInvoiceQueries.GetJobRefAutoCompleteQuery(this.DatabaseType, searchText, itemCount), conn))
                    {
                        Utilities.WriteLog("Set Data Adapter For Selecting records :" + DateTime.Now);
                        da.SelectCommand = objCmdSelect;
                        DataTable dt = new DataTable();

                        Utilities.WriteLog("Request to Fill records on :" + DateTime.Now);
                        da.Fill(dt);

                        Utilities.WriteLog("Fill records on:" + DateTime.Now);
                        if (dt.Rows != null && dt.Rows.Count > 0)
                        {
                            Utilities.WriteLog("Enter in loop to process rows on:" + DateTime.Now);
                            returnValue = new List<JobRefAutoCompleteResponse>();
                            foreach (DataRow row in dt.Rows)
                            {
                                returnValue.Add(LoadJobRefData(row));
                            }
                            Utilities.WriteLog("Fill Records Comeplete On  : " + DateTime.Now);
                        }
                        else
                        {
                            ErrorMessage = SimplicityConstants.MESSAGE_NO_RECORD_FOUND;
                        }
                    }
                }
                Utilities.WriteLog("End DB Connection On :" + DateTime.Now);
            }
            catch (Exception ex)
            {
                ErrorMessage = "Exception Occured While Getting GetJobRefAutoComplete List. " + ex.Message + " " + ex.InnerException;
                Utilities.WriteLog("Exception Occured While Getting GetJobRefAutoComplete List. " + ex.Message + " " + ex.InnerException);
            }
            return returnValue;
        }

        internal JobRefAutoCompleteResponse LoadJobRefData(DataRow row)
        {
            JobRefAutoCompleteResponse model = new JobRefAutoCompleteResponse();
            model.jobRef = DBUtil.GetStringValue(row, "JobRef");
            model.jobAddress = DBUtil.GetStringValue(row, "JobAddress").Replace("\r\n", " ");
            return model;
        }

        internal List<NamedModel> GetShowTypesData(string projectId, string basePath)
        {
            List<NamedModel> returnValue = null;
            Utilities.WriteLog("Request for DB Connection On:" + DateTime.Now);
            try
            {
                if (projectId != null && projectId.Length > 0)
                {
                    //ConfigModel config = GetCompanyConfiguration(projectId, basePath);
                    returnValue = new List<NamedModel>();
                    returnValue.Add(new NamedModel("Invoices", 0));
                    //returnValue.Add(new NamedModel(config.appnpl, 1));
                    returnValue.Add(new NamedModel("Applications for Payment", 1));
                }

                Utilities.WriteLog("End DB Connection On :" + DateTime.Now);
            }
            catch (Exception ex)
            {
                ErrorMessage = "Exception Occured While Getting GetJobRefAutoComplete List. " + ex.Message + " " + ex.InnerException;
                Utilities.WriteLog("Exception Occured While Getting GetJobRefAutoComplete List. " + ex.Message + " " + ex.InnerException);
            }
            return returnValue;
        }

        internal List<NamedModel> GetCompanyAutoComplete(string projectId, string searchText, int itemCount)
        {
            List<NamedModel> returnValue = null;
            Utilities.WriteLog("Request for DB Connection On:" + DateTime.Now);
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    Utilities.WriteLog("Get DB Connection On :" + DateTime.Now);
                    OleDbDataAdapter da = new OleDbDataAdapter();

                    Utilities.WriteLog("Get Company Data : " + DateTime.Now);
                    using (OleDbCommand objCmdSelect = new OleDbCommand(ClientInvoiceQueries.GetCompanyAutoComplete(this.DatabaseType, searchText, itemCount), conn))
                    {
                        Utilities.WriteLog("Set Data Adapter For Selecting records :" + DateTime.Now);
                        da.SelectCommand = objCmdSelect;
                        DataTable dt = new DataTable();

                        Utilities.WriteLog("Request to Fill records on :" + DateTime.Now);
                        da.Fill(dt);

                        Utilities.WriteLog("Fill records on:" + DateTime.Now);
                        if (dt.Rows != null && dt.Rows.Count > 0)
                        {
                            Utilities.WriteLog("Enter in loop to process rows on:" + DateTime.Now);
                            returnValue = new List<NamedModel>();
                            foreach (DataRow row in dt.Rows)
                            {
                                returnValue.Add(LoadCompanyAutoCompleteData(row));
                            }
                            Utilities.WriteLog("Fill Records Comeplete On  : " + DateTime.Now);
                        }
                        else
                        {
                            ErrorMessage = SimplicityConstants.MESSAGE_NO_RECORD_FOUND;
                        }
                    }
                }
                Utilities.WriteLog("End DB Connection On :" + DateTime.Now);
            }
            catch (Exception ex)
            {
                ErrorMessage = "Exception Occured While Getting GetCompanyAutoComplete List. " + ex.Message + " " + ex.InnerException;
                Utilities.WriteLog("Exception Occured While Getting GetCompanyAutoComplete List. " + ex.Message + " " + ex.InnerException);
            }
            return returnValue;
        }

        internal NamedModel LoadCompanyAutoCompleteData(DataRow row)
        {
            NamedModel model = new NamedModel();
            model.id = DBUtil.GetIntValue(row, "id");
            model.name = DBUtil.GetStringValue(row, "name");
            return model;
        }
        #endregion
    }
}
