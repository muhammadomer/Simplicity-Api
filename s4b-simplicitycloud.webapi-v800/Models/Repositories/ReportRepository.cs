using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Models.Interfaces;
using System;
using System.Collections.Generic;

using SimplicityOnlineWebApi.Commons;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SimplicityOnlineWebApi.DAL;
using System.Net;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class ReportRepository : IReportRepository
    {
        
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }


        public ReportRepository()
        {
            
        }

        public ResponseModel GenerateReport(HttpRequest request,string templateData)
        {
            ResponseModel returnValue = new ResponseModel();
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    //---- Sample data
                    //string templateData = @"{'template':{'shortid':'B1XZ0C0j7'},'data':{'Sequence':4165,
                    //'JobReference':'12345668812150',
                    //'ClientRef':'24356',
                    //'ClientName':'ADAMSMITH',
                    //'FromAddress':' General Plumbing Supplies\r\n24-25 Ormond Road\r\nLondon\r\nUK\r\nN19 4ER',
                    //'ToAddress':'Not Set',
                    //'InvoiceNo':'CBS418685545',
                    //'DateRaised':'2018-10-18T00:00:00',
                    //'JobAddress':'298 BROADWAY\r\nBEXLEYHEATH KENT DA6 8AH',
                    //'TradeCode':'Asbestos',
                    //'AmountInitial':553.7,
                    //'AmountDiscount':11.07,
                    //'PcentRetention':0.0,
                    //'AmountRetention':0.0,
                    //'AmountSubTotal':548.05,
                    //'AmountVat':5.43,
                    //'AmountCis':0.0,
                    //'AmountTotal':548.05,
                    //'Footnote':' ',
                    //'OrderBillItems':[{'Sequence':3057,
                    //'JobSequence':8077,
                    //'BillSequence':4165,
                    //'ItemSequence':14391,
                    //'FlgTextLine':false,
                    //'ItemCode':'C4005065',
                    //'ItemDesc':'Cutting out defective precast reinforced concrete or timber external lintel and renewing with 65 x 225 x 1000 mm long precast pr',
                    //'PcentPayment':100.0,
                    //'AmountPayment':553.7,
                    //'FlgDiscounted':true,
                    //'PcentDiscount':2.0,
                    //'AmountDiscount':11.074,
                    //'FlgRetention':false,
                    //'PcentRetention':0.0,
                    //'AmountRetention':0.0,
                    //'AmountSubTotal':542.626,
                    //'PcentVat':1.0,
                    //'AmountVat':5.4263,
                    //'PcentCis':0.0,
                    //'AmountCis':0.0,
                    //'SageNominalCode':'',
                    //'SageTaxCode':'',
                    //'SageBankCode':'',
                    //'CostCentreId':'',
                    //'ItemType':-1,
                    //'ItemQty':5.0,
                    //'ItemUnits':'NR',
                    //'ItemAmountBalance':0.0,
                    //'ItemAmountTotal':553.7
                    //}]}}";
                    string response = CallReportServer(templateData).Result;
                    returnValue.TheObject = response;
                    returnValue.IsSucessfull = true;
                }
                else
                {
                    returnValue.Message = SimplicityConstants.MESSAGE_INVALID_REQUEST_HEADER;
                }
            }
            catch (Exception ex)
            {
                returnValue.Message = "Exception Occured while getting report. " + ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }

        public async Task<string> CallReportServer(string templateData)
        {
           string returnValue=null;
            try
            {
                JObject o = JObject.Parse(templateData);

                string url = "http://localhost:5488/api/report";
                var request = WebRequest.CreateHttp(url);
                request.Method = "POST";
                request.ContentType = "application/json";
                var buffer = Encoding.UTF8.GetBytes(o.ToString());
                request.ContentLength = buffer.Length;
                request.GetRequestStream().Write(buffer, 0, buffer.Length);
                var response = request.GetResponse();
                returnValue = (new StreamReader(response.GetResponseStream())).ReadToEnd();
                //This wil save file on disk
                //var fileName =  DateTime.Now.ToString("yyyy-MM-dd") + ".pdf";
                //using (var wstream = File.Create(fileName))
                //    response.GetResponseStream().CopyTo(wstream);
                //---------------------------------------------------------

            }
            catch (Exception ex)
            {
                Utilities.WriteLog("Exception occur in getting pdf:" + ex.Message);
            }
            return returnValue;

        }
    }
}
