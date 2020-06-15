using Microsoft.AspNetCore.Http;


using Newtonsoft.Json;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.BLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using SimplicityOnlineWebApi.DAL;
using Microsoft.VisualBasic;
using Microsoft.Extensions.Logging;

namespace SimplicityOnlineWebApi.Models.Repositories
{
   public class S4BFormsRepository : IS4BFormsRepository
   {
      
		private ILogger<S4BFormsRepository> Logger;
		public string Message { get; set; }
      public bool IsSecondaryDatabase { get; set; }
      public string SecondaryDatabaseId { get; set; }

      /// <summary>
      /// Hold down the Map of S4BFormsControl
      /// FieldName is the Key
      /// </summary>
      private Dictionary<string, S4BFormsControl> _s4BFormsControlMap = new Dictionary<string, S4BFormsControl>();
      public Dictionary<string, S4BFormsControl> S4BFormsControlMap
      {
         get
         {
            return _s4BFormsControlMap;
         }
      }

      public S4BFormsRepository()
      {
         
      }

		public ResponseModel ProcessSubmittedForm(HttpRequest request)
		{
			const string METHOD_NAME = "S4BFormsRepository.ProcessSubmittedForm()";
			ResponseModel returnValue = new ResponseModel();
			try
			{
				ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
				if (settings != null)
				{
					S4BFormRequest s4bFormRequest = JsonConvert.DeserializeObject<S4BFormRequest>(request.Form["S4BFormData"]);
					if (s4bFormRequest != null)
					{
						IFormFile submittedZipForm = request.Form.Files.GetFile(s4bFormRequest.FormName);
						Utilities.WriteLog("Get submitted zip form name");
						if (submittedZipForm == null || submittedZipForm.Length <= 0)
						{
							returnValue.Message = "Unable to retrive zip file from request. Please verify the request should contain the zip file '" + s4bFormRequest.FormName + "'";
							return returnValue;
						}
						Utilities.WriteLog("Get GetRecodsById return refS4bForm");
						RefS4bForms refS4bForm = new RefS4bFormsRepository().GetRecodsById(s4bFormRequest.FormSequence ?? 0, request);
						Utilities.WriteLog("Generate s4bFormSubmission");
						S4bFormSubmissions s4bFormSubmission = generateS4BFormSubmission(refS4bForm, s4bFormRequest, request);
						s4bFormSubmission.ContentPath = extractSubmittedZipForm(submittedZipForm, settings, request, s4bFormSubmission, s4bFormRequest);
						Utilities.WriteLog("start ProcessS4BForm");
						if (ProcessS4BForm(request, s4bFormSubmission, s4bFormRequest))
						{
							s4bFormRequest.SubmissionId = s4bFormSubmission.S4bSubmitNo;
							//---Update Falg Completed
							new S4bFormSubmissionRepository().UpdateFlgCompletedFormSubmission(s4bFormSubmission, request);
							returnValue.IsSucessfull = true;
							returnValue.TheObject = s4bFormRequest;
						}
						else
						{
							returnValue.Message = "Unable to Process Submitted S4B Form. Reason: " + Message;
						}
					}
					else
					{
						returnValue.Message = SimplicityConstants.MESSAGE_INVALID_REQUEST + " S4BFormData : " + (Convert.ToString(request.Form["S4BFormData"]) == null ? "null" : request.Form["S4BFormData"].ToString());
					}
				}
				else
				{
					returnValue.Message = SimplicityConstants.MESSAGE_INVALID_REQUEST_HEADER;
				}
			}
			catch (Exception ex)
			{
				returnValue.Message = Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error Occured while Processing S4BForm.", ex);
			}
			return returnValue;
		}

		public ResponseModel ProcessSubmittedFormVideoFile(HttpRequest request)
		{
			const string METHOD_NAME = "S4BFormsRepository.ProcessSubmittedFormVideo()";
			ResponseModel returnValue = new ResponseModel();
			List<SubmittedVideoFile> submittedFiles = new List<SubmittedVideoFile>();

			try
			{
				ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
				if (settings != null)
				{
					S4BFormRequest s4bFormRequest = JsonConvert.DeserializeObject<S4BFormRequest>(request.Form["S4BFormData"]);
					if (s4bFormRequest != null)
					{
						IFormFile submittedZipForm = request.Form.Files.GetFile(s4bFormRequest.FormName);
						if (submittedZipForm == null || submittedZipForm.Length <= 0)
						{
							returnValue.Message = "Unable to retrive zip file from request. Please verify the request should contain the zip file '" + s4bFormRequest.FormName + "'";
							return returnValue;
						}
						RefS4bForms refS4bForm = new RefS4bFormsRepository().GetRecodsById(s4bFormRequest.FormSequence ?? 0, request);
						//---inorder to get jobSequence, Get FormSubmission from submit No 
						S4bFormSubmissionRepository s4bFormSubmissionRepo = new S4bFormSubmissionRepository();
						S4bFormSubmissions s4bFormSubmission = s4bFormSubmissionRepo.getFormSubmissionBySubmitNo(request, s4bFormRequest.S4bSubmitNo);
						//--- Get JobRef by jobSequence
						OrdersRepository orderRepository = new OrdersRepository(IsSecondaryDatabase, SecondaryDatabaseId);
						Orders order = orderRepository.GetOrderDetailsBySequence(s4bFormSubmission.Orders.Sequence ?? 0, request);
						string zipFileName = Utilities.GenerateS4BeFormVideoZipFileName(s4bFormRequest.FormSequence ?? 0, s4bFormRequest.FormName, 1);
						string ContentPath = extractSubmittedVideoZipFile(submittedZipForm, settings, request, s4bFormRequest);
						//---Upload File to drive
						string tempFolderPath = s4bFormRequest.FormSequence.ToString();
						if (Directory.Exists(tempFolderPath))
						{
							DirectoryInfo directory = new DirectoryInfo(tempFolderPath);
							directory.EmptyDirectory();
						}
						else
						{
							Directory.CreateDirectory(tempFolderPath);
						}
						string[] fileEntries = Directory.GetFiles(ContentPath);
						foreach (string filePath in fileEntries)
						{

							string fileName = Path.GetFileName(filePath);
							string videoFilePath = Path.Combine(ContentPath, fileName);
							string videoFileCabId = UploadToDrive(request, videoFilePath, order.JobRef + "," + settings.FilingCabinetS4BFormsFolder, fileName);
							SubmittedVideoFile zipfileCabId = new SubmittedVideoFile();
							zipfileCabId.FileCabId = videoFileCabId;
							zipfileCabId.FileName = fileName;
							submittedFiles.Add(zipfileCabId);
						}

						if (submittedFiles.Count > 0)
						{
							returnValue.IsSucessfull = true;
							returnValue.TheObject = submittedFiles;
						}
						else
						{
							returnValue.Message = "Unable to Process Submitted S4B Video File. Reason: " + Message;
						}
					}
					else
					{
						returnValue.Message = SimplicityConstants.MESSAGE_INVALID_REQUEST + " S4BFormData : " + (Convert.ToString(request.Form["S4BFormData"]) ==null ? "null" : request.Form["S4BFormData"].ToString());
					}
				}
				else
				{
					returnValue.Message = SimplicityConstants.MESSAGE_INVALID_REQUEST_HEADER;
				}
			}
			catch (Exception ex)
			{
				returnValue.Message = Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error Occured while Processing S4BForm video file.", ex);
			}
			return returnValue;
		}

		private S4bFormSubmissions generateS4BFormSubmission(RefS4bForms refS4BForm, S4BFormRequest s4bFormRequest, HttpRequest request)
		{
			S4bFormSubmissions s4bFormSubmission = null;
			S4bFormSubmissionRepository s4bFormSubmissionRepo = new S4bFormSubmissionRepository();
			if (string.IsNullOrEmpty(s4bFormRequest.SubmissionId))
			{
				s4bFormSubmission = new S4bFormSubmissions();
				s4bFormSubmission.S4bSubmitTs = DateTime.Now.ToString("yyyyMMddHHss");
				s4bFormSubmission.DateSubmit = DateTime.Now;
				s4bFormSubmission.FormSequence = refS4BForm.FormSequence ?? 0;
				s4bFormSubmission.TemplateName = refS4BForm.FormDesc;
				s4bFormSubmission.SubmitDetails = s4bFormRequest.FormName;
				s4bFormSubmission.CreatedBy = long.Parse(request.Headers["UserId"]);
				s4bFormSubmission.DateCreated = DateTime.Now;
				s4bFormSubmission.LastAmendedBy = long.Parse(request.Headers["UserId"]);
				s4bFormSubmission.DateLastAmended = DateTime.Now;
				s4bFormSubmission.FileCabId = "";
				s4bFormSubmission = s4bFormSubmissionRepo.InsertFormSubmission(s4bFormSubmission, request);
				s4bFormSubmission.RefNatForms = refS4BForm;
			}
			else
			{
				s4bFormSubmission = s4bFormSubmissionRepo.getFormSubmissionBySubmitNo(request, s4bFormRequest.SubmissionId);
			}
			return s4bFormSubmission;
		}

		private bool ProcessS4BForm(HttpRequest request, S4bFormSubmissions s4bFormSubmission, S4BFormRequest s4bFormRequest)
		{
			const string METHOD_NAME = "S4BFormsRepository.ProcessS4BForm()";
			bool returnValue = false;
			try
			{
				Utilities.WriteLog("Get json File Path");
				string jsonFilePath = Path.Combine(s4bFormSubmission.ContentPath, SimplicityConstants.S4BFormJsonFileName);
				if (!File.Exists(jsonFilePath))
				{
					Message = jsonFilePath + " does not exist in the submission.";
					Utilities.WriteLog(Message);
					return false;
				}
				Utilities.WriteLog("Deserialize jsonFilePath");
				RootObject s4bFormRootObject = JsonConvert.DeserializeObject<RootObject>(File.ReadAllText(jsonFilePath));
				Utilities.WriteLog("Load S4B FormsControls");
				Dictionary<String, S4BFormsControl> s4bFormsControls = LoadS4BFormsControls(s4bFormRootObject);
				Utilities.WriteLog("Get Order From S4BForms Control");
				Orders order = Utilities.GetOrderFromS4BFormsControl(request, s4bFormSubmission, s4bFormsControls);
				if (order != null)
				{
					s4bFormSubmission.Orders = new Orders { JobAddress = order.JobAddress, JobRef = order.JobRef, Sequence = order.Sequence ?? 0 };
					s4bFormSubmission.JobSequence = s4bFormSubmission.Orders.Sequence ?? 0;
					Utilities.WriteLog("Job sequence:" + s4bFormSubmission.JobSequence);
				}
				if (Utilities.IsS4BFormTemplateSecondary(s4bFormSubmission.RefNatForms.FormId))
				{
					Utilities.WriteLog("IsS4BFormTemplateSecondary");
					bool flgInvalidOrder = false;
					Utilities.WriteLog("GetSecondaryOrderByJobSequence");
					s4bFormSubmission.OrderSecondary = Utilities.GetSecondaryOrderByJobSequence(request, s4bFormSubmission.Orders.Sequence ?? 0, Utilities.GetSecondaryIdByS4BFormId(s4bFormSubmission.RefNatForms.FormId), out flgInvalidOrder);
					s4bFormSubmission.IsInvalidOrderSecondary = flgInvalidOrder;
				}
				Utilities.WriteLog("AddS4BFormToFilingCabinet");
				s4bFormSubmission.FileCabId = AddS4BFormToFilingCabinet(request, s4bFormRequest, s4bFormSubmission, s4bFormsControls);
				Utilities.WriteLog("Filing Cabinet Id:" + s4bFormSubmission.FileCabId);
				if (!new S4bFormSubmissionRepository().UpdateFormSubmission(s4bFormSubmission, request))
				{
					Utilities.WriteLog("Unable to Update Submission");
					Utilities.GenerateAndLogMessage(METHOD_NAME, "Unable to Update Submission.", null);
				}
				if (!emailSubmission(request, s4bFormRequest, s4bFormSubmission, s4bFormsControls))
				{
					Message = "Unable to Email the Submitted document(s)";
					Utilities.WriteLog(Message);
				}

				//------------Save Template Data in table
				if (!s4bFormRequest.IsFormResubmitted)
				{
					if (SaveTemplateData(request, s4bFormRequest, s4bFormSubmission, s4bFormRootObject, s4bFormsControls))
					{
						returnValue = true;
					}
					else
					{
						Message = Message;
					}
				}


				switch (s4bFormSubmission.RefNatForms.FormId)
				{

					case "1023744743": // Franklin Hodge Site Inspection
						if (!s4bFormRequest.IsFormResubmitted)
						{
							if (ProcessFHSiteInspection(request, s4bFormRequest, s4bFormSubmission, s4bFormRootObject, s4bFormsControls))
							{
								returnValue = true;
							}
							else
							{
								Message = Message;
							}
						}
						break;

					case "470969094": // // Franklin Hodge Site Inspection V10 "" 
						if (!s4bFormRequest.IsFormResubmitted)
						{
							if (ProcessFHSiteInspectionV08(request, s4bFormRequest, s4bFormSubmission, s4bFormRootObject, s4bFormsControls))
							{
								returnValue = true;
							}
							else
							{
								Message = Message;
							}
						}
						break;

					case "357000015": // CEE Water Sampling Report V04
						if (!emailSubmissionToJobManagerAndOrderDistributionList(request, s4bFormRequest, s4bFormSubmission, s4bFormsControls))
						{
							Message = "Unable to Email the Submitted document to Job Manager or Order Distribution List";
							Utilities.WriteLog(Message);
						}
						if (ProcessCEEWaterSampling(request, s4bFormRequest, s4bFormSubmission, s4bFormRootObject, s4bFormsControls))
						{
							returnValue = true;
						}
						else
						{
							Message = Message;
						}
						break;

					case "357000014": // CEE Technical Report V04
						if (!emailSubmissionToJobManagerAndOrderDistributionList(request, s4bFormRequest, s4bFormSubmission, s4bFormsControls))
						{
							Message = "Unable to Email the Submitted document to Job Manager or Order Distribution List";
							Utilities.WriteLog(Message);
						}
						if (ProcessCEETechnicalReport(request, s4bFormRequest, s4bFormSubmission, s4bFormRootObject, s4bFormsControls))
						{
							returnValue = true;
						}
						else
						{
							Message = Message;
						}
						break;

					case "357000013": // CEE Disinfection Report
						if (!emailSubmissionToJobManagerAndOrderDistributionList(request, s4bFormRequest, s4bFormSubmission, s4bFormsControls))
						{
							Message = "Unable to Email the Submitted document to Job Manager or Order Distribution List";
							Utilities.WriteLog(Message);
						}
						if (ProcessCEEDisinfectionReport(request, s4bFormRequest, s4bFormSubmission, s4bFormRootObject, s4bFormsControls))
						{
							returnValue = true;
						}
						else
						{
							Message = Message;
						}
						break;

					case "357000012":
						if (!emailSubmissionToJobManagerAndOrderDistributionList(request, s4bFormRequest, s4bFormSubmission, s4bFormsControls))
						{
							Message = "Unable to Email the Submitted document to Job Manager or Order Distribution List";
							Utilities.WriteLog(Message);
						}
						if (ProcessCEEChemicalAnalysis(request, s4bFormRequest, s4bFormSubmission, s4bFormRootObject, s4bFormsControls))
						{
							returnValue = true;
						}
						else
						{
							Message = Message;
						}
						break;

					case "1008617976": // Woodvale Quality Control V07
						if (ProcessWoodvaleQualityControl(request, s4bFormRequest, s4bFormSubmission, s4bFormRootObject, s4bFormsControls))
						{
							returnValue = true;
						}
						else
						{
							Message = Message;
						}
						break;

					case "355855623": // Woodvale Job Ticket V01
						if (ProcessWoodvaleJobTicket(request, s4bFormRequest, s4bFormSubmission, s4bFormRootObject, s4bFormsControls))
						{
							returnValue = true;
						}
						else
						{
							Message = Message;
						}
						break;

					case "1026189291": // Avon Ruby - Worksheet 1 Avon Ruby V01
					case "919187445": // Avon Ruby - Active Response Worksheet 1 V16
						if (ProcessAvonRubyWorkshee1(request, s4bFormRequest, s4bFormSubmission, s4bFormRootObject, s4bFormsControls))
						{
							returnValue = true;
						}
						else
						{
							Message = Message;
						}
						break;

					case "1299383635": // Avon Ruby - Roofing V02
						if (ProcessAvonRubyRoofing(request, s4bFormRequest, s4bFormSubmission, s4bFormRootObject, s4bFormsControls))
						{
							returnValue = true;
						}
						else
						{
							Message = Message;
						}
						break;

					case "984083642": // Avon Ruby - Estimate V03
						if (ProcessAvonRubyEstimateV03(request, s4bFormRequest, s4bFormSubmission, s4bFormRootObject, s4bFormsControls))
						{
							returnValue = true;
						}
						else
						{
							Message = Message;
						}
						break;

					case "735722830": // Avon Ruby - Estimate V06// it is renamed to v09
						if (ProcessAvonRubyEstimateV06(request, s4bFormRequest, s4bFormSubmission, s4bFormRootObject, s4bFormsControls))
						{
							returnValue = true;
						}
						else
						{
							Message = Message;
						}
						break;

					case "360000110": // Active Response - Worksheet 1 V16 "360000110"
					case "360000145": // Active Response - Worksheet 1 Avon Ruby V01 "360000145"
					case "357000021": // Active Response - Worksheet Plumber V16 "357000021"
						if (ProcessActiveResponseWorkshee1(request, s4bFormRequest, s4bFormSubmission, s4bFormRootObject, s4bFormsControls))
						{
							returnValue = true;
						}
						else
						{
							Message = Message;
						}
						break;
					case "560072524": // Active Response - New Worksheet 1 V18 (560072524)
						if (ProcessActiveResponseWorksheeV18(request, s4bFormRequest, s4bFormSubmission, s4bFormRootObject, s4bFormsControls))
						{
							returnValue = true;
						}
						else
						{
							Message = Message;
						}
						break;
					//case "360000168": // Capel Group - KS Plant Weekly Plant Sheet V01 - Processing of this template has been merged with Recruitment Timesheet.
					//    if (ProcessCapelKSPlantWeeklyPlant(request, s4bFormRequest, s4bFormSubmission, s4bFormRootObject, s4bFormsControls))
					//    {
					//        returnValue = true;
					//    }
					//    else
					//    {
					//        Message = Message;
					//    }
					//    break;

					case "360000167": // Capel Group - Capel Recruitment Weekly Timesheet 360000167
					case "360000168": // Capel Group - KS Plant Weekly Plant Sheet V01 - 360000168
						if (ProcessCapelTimesheet(request, s4bFormRequest, s4bFormSubmission, s4bFormRootObject, s4bFormsControls))
						{
							returnValue = true;
						}
						else
						{
							Message = Message;
						}
						break;

					case "1114043282": // Capel Group - Capel Plant Vehcile And Driver Log - 1114043282
						if (ProcessCapelVehicleAndDriverLog(request, s4bFormRequest, s4bFormSubmission, s4bFormRootObject, s4bFormsControls))
						{
							returnValue = true;
						}
						else
						{
							Message = Message;
						}
						break;
					case "1673956019"://Capel KS Plant -Small Tools 13 Week Maintenance Check Sheet V01 
						if (ProcessCapelSmallToolWeekMaintenance(request, s4bFormRequest, s4bFormSubmission, s4bFormRootObject, s4bFormsControls))
						{
							returnValue = true;
						}
						else
						{
							returnValue = false;
						}
						break;
					case "1623983715"://Capel KS Plant -Vehicle Test V02
						if (ProcessCapelKSPlantVehicleTestV02(request, s4bFormRequest, s4bFormSubmission, s4bFormRootObject, s4bFormsControls))
						{
							returnValue = true;
						}
						else
						{
							returnValue = false;
						}
						break;
					case "59626082": // CBS - Job Sheet and Risk Assessment  V09 59626082
					case "984204447": // CBS - Job Sheet and Risk Assessment Blank V09 984204447
						if (ProcessCBSJobSheetAndRiskAssessmentV009(request, s4bFormRequest, s4bFormSubmission, s4bFormRootObject, s4bFormsControls))
						{
							returnValue = true;
						}
						else
						{
							Message = Message;
						}
						break;

					case "1587685603": // CBS - Job Sheet and Risk Assessment Blank V01 1587685603
					case "357000009": // CBS - Job Sheet and Risk Assessment V07 357000009
						if (ProcessCBSJobSheetAndRiskAssessment(request, s4bFormRequest, s4bFormSubmission, s4bFormRootObject, s4bFormsControls))
						{
							returnValue = true;
						}
						else
						{
							Message = Message;
						}
						break;

					case "2093570712": // Lowry - Purchase Order Blank V02 2093570712 - formType = 0
						if (ProcessLowryPO(request, s4bFormRequest, s4bFormSubmission, s4bFormRootObject, s4bFormsControls, 0))
						{
							returnValue = true;
						}
						break;

					case "905560356": // Lowry - Purchase Order Blank Contractor V02 2093570712 - formType = 0
						if (ProcessLowryPOContractor(request, s4bFormRequest, s4bFormSubmission, s4bFormRootObject, s4bFormsControls, 0))
						{
							returnValue = true;
						}
						break;

					case "1788502524": // UNC_Client - Template S4B New Client Sales Meeting V01 - 1788502524
						if (ProcessUNCNewClientSalesMeeting(request, s4bFormRequest, s4bFormSubmission, s4bFormRootObject, s4bFormsControls))
						{
							returnValue = true;
						}
						break;

					case "851061305000":
					case "851061305":
					case "1396300134":
					case "1396300134000":
						if (ProcessFiveEnvJobTicket(request, s4bFormRequest, s4bFormSubmission, s4bFormRootObject, s4bFormsControls))
						{
							returnValue = true;
						}
						break;

					default:
						returnValue = true;
						break;
				}

			}
			catch (Exception ex)
			{
				Message = Utilities.GenerateAndLogMessage("", "Unable to Process S4BForm. ", ex);
			}
			return returnValue;
		}

		private bool emailSubmission(HttpRequest request, S4BFormRequest s4bFormRequest, S4bFormSubmissions s4bFormSubmission, Dictionary<string, S4BFormsControl> s4bFormsControls)
      {
         bool returnValue = false;
         try
         {
            if (s4bFormSubmission.RefNatForms != null)
            {
               ProjectSettings settings = Configs.settings[request.Headers["ProjectId"]];
               string pdfFilePath = Path.Combine(s4bFormSubmission.ContentPath, SimplicityConstants.S4BFormSubmittedTemplateName);
               //--- Rename file to send in attachment
               string pdfFileName = Utilities.GenerateS4BeFormPdfFileName(s4bFormSubmission, 1);
               string newFileName = Path.Combine(s4bFormSubmission.ContentPath, pdfFileName);
               System.IO.File.Move(pdfFilePath, newFileName);
               CldSettingsRepository cldSettingsRepository = new CldSettingsRepository();
               CldSettings emailSubjectSetting = null;
               CldSettings emailContentSetting = null;
               CldSettings highSecurityEmailAddressSetting = null;
               bool isFormHighRisk = Utilities.IsS4BFormSubmissionHighRisk(s4bFormsControls);
               if (isFormHighRisk)
               {
                  emailSubjectSetting = cldSettingsRepository.GetCldSettingsBySettingName(request, SimplicityConstants.CldSettingS4BFormSubmissionEmailSubjectHighRisk);
                  emailContentSetting = cldSettingsRepository.GetCldSettingsBySettingName(request, SimplicityConstants.CldSettingS4BFormSubmissionEmailContentHighRisk);
                  highSecurityEmailAddressSetting = cldSettingsRepository.GetCldSettingsBySettingName(request, SimplicityConstants.CldSettingS4BFormDefaultDistributionEmailAddressHighRisk);
               }
               else
               {
                  emailSubjectSetting = cldSettingsRepository.GetCldSettingsBySettingName(request, SimplicityConstants.CldSettingS4BFormSubmissionEmailSubject);
                  emailContentSetting = cldSettingsRepository.GetCldSettingsBySettingName(request, SimplicityConstants.CldSettingS4BFormSubmissionEmailContent);
               }
               RefS4bForms s4bForm = s4bFormSubmission.RefNatForms;
               string emailSubject = emailSubjectSetting.SettingValue;
               emailSubject = Utilities.replaceS4BFormsKeywords(emailSubject, s4bFormRequest, s4bFormSubmission, s4bFormsControls);
               string emailContent = emailContentSetting.SettingValue;
               emailContent = Utilities.replaceS4BFormsKeywords(emailContent, s4bFormRequest, s4bFormSubmission, s4bFormsControls);
               List<string> fileAttachments = new List<string>();
               fileAttachments.Add(newFileName);
               EmailContact fromContact = new EmailContact();
               fromContact.EmailAddress = settings.AdminEmailAddress;

               string emailTo = Utilities.ExtractEmailAddressesFromEmailFields(request, s4bForm.EmailTo, s4bFormsControls);
               string emailCC = Utilities.ExtractEmailAddressesFromEmailFields(request, s4bForm.CCEMailAddress, s4bFormsControls);
               string emailBCC = Utilities.ExtractEmailAddressesFromEmailFields(request, s4bForm.BCCEmailAddess, s4bFormsControls);
               if (isFormHighRisk && !string.IsNullOrEmpty(highSecurityEmailAddressSetting.SettingValue))
               {
                  emailTo = emailTo + highSecurityEmailAddressSetting.SettingValue + SimplicityConstants.EmailAddressSeparator;
               }

               if (Utilities.SendMail(fromContact, Utilities.GetEmailContactsFromEmailAddresses(emailTo),
                                      Utilities.GetEmailContactsFromEmailAddresses(emailCC),
                                      Utilities.GetEmailContactsFromEmailAddresses(emailBCC),
                                      emailSubject, emailContent, fileAttachments, "", ""))
               {
                  returnValue = true;
               }
               else
               {
                  //TODO: Need to report an error in log file as well as an email to be sent to administrator
               }
            }
         }
         catch (Exception ex)
         {
         }
         return returnValue;
      }

      /// <summary>
      /// This Method will email the S4bForm Submission to Job Manager and Order Distributtion List.
      /// It was designed for CEE but can be used for other clients.
      /// </summary>
      /// <param name="request"></param>
      /// <param name="s4bFormRequest"></param>
      /// <param name="s4bFormSubmission"></param>
      /// <param name="s4bFormContentsPath"></param>
      /// <param name="s4bFormsControls"></param>
      /// <returns></returns>
      private bool emailSubmissionToJobManagerAndOrderDistributionList(HttpRequest request, S4BFormRequest s4bFormRequest, S4bFormSubmissions s4bFormSubmission, Dictionary<string, S4BFormsControl> s4bFormsControls)
      {
         bool returnValue = false;
         try
         {
            string emailAddresses = "";
            Orders order = new OrdersRepository(null).GetOrderDetailsBySequence(s4bFormSubmission.JobSequence ?? 0, request);
            if (s4bFormSubmission.JobSequence > 0)
            {
               if (order.JobManagerId > 0)
               {
                  EntityDetailsCore jobManager = new EntityDetailsCoreRepository().GetEntityByEntityId(request, order.JobManagerId);
                  emailAddresses = jobManager.Email;
               }
               OrdersDistributionListRepository ordersDistributionListRepository = new OrdersDistributionListRepository();
               List<OrdersDistributionList> ordersDistributionList = ordersDistributionListRepository.GetByJobSequence(request, s4bFormSubmission.JobSequence ?? 0);
               if (ordersDistributionList != null)
               {
                  foreach (OrdersDistributionList orderDistribution in ordersDistributionList)
                  {
                     if (!string.IsNullOrEmpty(orderDistribution.EmailAddress))
                     {
                        if (!string.IsNullOrEmpty(emailAddresses))
                        {
                           emailAddresses = emailAddresses + SimplicityConstants.EmailAddressSeparator;
                        }
                        emailAddresses = emailAddresses + orderDistribution.EmailAddress.Trim();
                     }
                  }
               }
               if (string.IsNullOrEmpty(emailAddresses))
               {
                  emailAddresses = new CldSettingsRepository().GetDefaultEmailForDistribution(request);
               }
               if (!string.IsNullOrEmpty(emailAddresses))
               {
                  string pdfFilePath = Path.Combine(s4bFormSubmission.ContentPath, SimplicityConstants.S4BFormSubmittedTemplateName);
                  //--- Rename file to send in attachment
                  string pdfFileName = Utilities.GenerateS4BeFormPdfFileName(s4bFormSubmission, 1);
                  string newFileName = Path.Combine(s4bFormSubmission.ContentPath, pdfFileName);
                  if (System.IO.File.Exists(newFileName) == false) //Move file if it does not exists
                  {
                     System.IO.File.Move(pdfFilePath, newFileName);
                  }

                  CldSettingsRepository cldSettingsRepository = new CldSettingsRepository();
                  CldSettings emailSubjectSetting = cldSettingsRepository.GetCldSettingsBySettingName(request, SimplicityConstants.CldSettingS4BFormSubmissionEmailSubject);
                  CldSettings emailContentSetting = cldSettingsRepository.GetCldSettingsBySettingName(request, SimplicityConstants.CldSettingS4BFormSubmissionEmailContent);
                  ProjectSettings settings = Configs.settings[request.Headers["ProjectId"]];
                  RefS4bForms s4bForm = s4bFormSubmission.RefNatForms;
                  string emailSubject = emailSubjectSetting.SettingValue;
                  emailSubject = Utilities.replaceS4BFormsKeywords(emailSubject, s4bFormRequest, s4bFormSubmission, s4bFormsControls);
                  string emailContent = emailContentSetting.SettingValue;
                  emailContent = Utilities.replaceS4BFormsKeywords(emailContent, s4bFormRequest, s4bFormSubmission, s4bFormsControls);
                  List<string> fileAttachments = new List<string>();
                  fileAttachments.Add(newFileName);
                  EmailContact fromContact = new EmailContact();
                  fromContact.EmailAddress = settings.AdminEmailAddress;

                  if (Utilities.SendMail(fromContact, Utilities.GetEmailContactsFromEmailAddresses(emailAddresses),
                                         null,
                                         null,
                                         emailSubject, emailContent, fileAttachments, "", ""))
                  {
                     returnValue = true;
                  }
                  else
                  {
                     //TODO: Need to report an error in log file as well as an email to be sent to administrator
                  }
               }
            }
         }
         catch (Exception ex)
         {
         }
         return returnValue;
      }
      //
      // General Template Saving
      private bool SaveTemplateData(HttpRequest request, S4BFormRequest s4bFormRequest, S4bFormSubmissions s4bFormSubmission,
                                           RootObject root, Dictionary<string, S4BFormsControl> s4bFormsControls)
      {
         bool returnValue = false;
         DateTime? resultDate = DateTime.MinValue;
         string jobRef = "";

         Dictionary<string, string> imageValues = new Dictionary<string, string>();
         try
         {
            //----Insert a row for hiding cost
            // Prepare Data for Insertion
            S4BSubmissionsData2 rowdata = new S4BSubmissionsData2();
            rowdata.JoinSequence = Convert.ToInt32(s4bFormSubmission.S4bSubmitNo);
            rowdata.FieldName = "flgShowPriceToClient";
            rowdata.FieldData = "False";
            rowdata.FieldPosition = 3;
            rowdata.FieldType = "checkbox";
            rowdata.PageNumber = 4;

            rowdata.CreatedBy = Int32.Parse(request.Headers["UserId"].ToString());
            rowdata.DateCreated = s4bFormSubmission.DateSubmit;
            SiteInspectionRepository siteInspectionRepository = new SiteInspectionRepository();
            siteInspectionRepository.Insert(rowdata, request, null);

            foreach (KeyValuePair<string, S4BFormsControl> s4bFormControlEntry in s4bFormsControls)
            {
               S4BFormsControl s4bFormControl = s4bFormControlEntry.Value;
               // Prepare Data for Insertion
               S4BSubmissionsData2 data = new S4BSubmissionsData2();
               data.JoinSequence = Convert.ToInt32(s4bFormSubmission.S4bSubmitNo);
               data.FieldName = s4bFormControlEntry.Key;
               data.FieldData = s4bFormControl.fieldValue == null ? " " : s4bFormControl.fieldValue;
               data.FieldPosition = s4bFormControl.sequenceNum;
               data.FieldType = s4bFormControl.type;
               data.PageNumber = s4bFormControl.pageNo;

               data.CreatedBy = Int32.Parse(request.Headers["UserId"].ToString());
               data.DateCreated = s4bFormSubmission.DateSubmit;
               siteInspectionRepository.Insert(data, request, null);
            }
            //Check and Create Job Ref
            if (!string.IsNullOrEmpty(jobRef))
            {
               jobRef = Utilities.FormatJobRefForCreateOrder(jobRef);
               OrdersRepository orderRepostory = new OrdersRepository(null);
               Orders order = orderRepostory.CreateOrderByJobRef(jobRef, false, request, null);
            }
            returnValue = true;
         }
         catch (Exception ex)
         {
            Message = "Unable to Process Template. Exception: " + ex.Message;
         }
         return returnValue;
      }
		// New S4B Quotation V3 (393353803) eForm Submission Processing
		private bool ProcessS4BQuotationV3(HttpRequest request, S4BFormRequest s4bFormRequest, S4bFormSubmissions s4bFormSubmission,
											 RootObject root, Dictionary<string, S4BFormsControl> s4bFormsControls)
		{
			bool returnValue = false;
			double resultDouble = 0;
			DateTime? resultDate = DateTime.MinValue;
			DateTime? orderDate = DateTime.MinValue;
			DateTime? jobEstDate = DateTime.MinValue;
			string jobRef = "", jobSequence = "", jobAddressId = "", siteAddress = "", clientId = "", clientRef = "", clientName = "", userId = "", diarySequence = "", p1InspectedBy = "", p2Location = "";
			string emailAdd = "";
			double subTotal1 = 0, subTotal2 = 0, subTotal3 = 0;
			const int P1_NO_ROWS = 12, P2_NO_ROWS = 31;
			string[] p1Unit = new string[P1_NO_ROWS];
			double[] p1Qty = new double[P1_NO_ROWS];
			double[] p1Amount = new double[P1_NO_ROWS];
			double[] p1Total = new double[P1_NO_ROWS];
			string[] p1Desc = new string[P1_NO_ROWS];
			string[] p2Unit = new string[P2_NO_ROWS];
			double[] p2Qty = new double[P2_NO_ROWS];
			double[] p2Amount = new double[P2_NO_ROWS];
			double[] p2Total = new double[P2_NO_ROWS];
			string[] p2Desc = new string[P2_NO_ROWS];
			double[] subTotal = new double[P2_NO_ROWS];
			string[] descPart2 = new string[P2_NO_ROWS];
			string[] inc = new string[P2_NO_ROWS];
			int index = -1;
			//--- Reference data
			p1Desc[0] = "Implementation and Setup for Simplicity EBS and Cloud database for Client Site incl. Generic Labels (Task completed online)";
			p1Desc[1] = "Start-up training day for all office staff. Understanding the business process from a system perspective and a business requirement.";
			p1Desc[2] = "On Site Training for office staff and managers – if any further days are required, they are charged at £750 a day";
			p1Desc[3] = "Create eForms - Simplicity eForms, branding and publish to users – Starter package, eForm templates which includes two Simplicity branded reports 1. Invoice 2. Estimate.";
			p1Desc[4] = "Client Invoicing & Quotes Portal - Clients portal branding and linking to client’s website.";
			p1Desc[5] = "Setup Simplicity Time-track (Data from Sage Payroll to Simplicity Time-Track)";
			p1Desc[6] = "Analysing Day(s) Within the Analysing Day, Jean will get an understanding of any company or personal excel spreadsheets, applications or databases that are been used within the company. The data collected will be analysed and then used within the Simplicity system for the creation of the:-CRM, jobs / contracts etc. (Any data uncovered will be costed separately once analysed).";
			p1Desc[8] = "Client Data. Transfer clients & supplier data from Sage to Simplicity.";
			p1Desc[9] = "Create:- Branded emails for team, from Simplicity EBS & Cloud";
			p1Desc[10] = "All out of pocket expenses for Simplicity for Business Ltd staff to be charged at cost.All costs to be agree with Simplicity for Business before travel.";
			p1Desc[11] = "Total Price of Installation and Training for Simplicity Cloud, eForms & TimeTrack";
			p2Desc[0] = "Licenses for Simplicity EBS & Cloud - Admin";
			p2Desc[2] = "Simplicity Company Cloud Filing Cabinet Only - No Simplicity EBS users";
			p2Desc[3] = "Licenses eForms";
			p2Desc[5] = "Licenses Invoicing Portal";
			p2Desc[7] = "Simplicity Web Client Appointment Booking system";
			p2Desc[8] = "Simplicity Cloud Client Document portal. Licenced per client";
			p2Desc[9] = "Simplicity Sub/Contactors web portal setup";
			p2Desc[10] = "Support & Maintenance for Simplicity EBS, Cloud & eForms";
			p2Desc[11] = "Extra storage charged monthly for Filing Cabinet";
			p2Desc[14] = "** Sage Accounts 50c version 25";
			p2Desc[15] = "** Sage Payroll 2019/2020";
			p2Desc[16] = "Sage account and payroll are charged per company, per user";
			p2Desc[17] = "Simplicity Van Stock";
			p2Desc[18] = "PO & Supplier Chain";
			p2Desc[19] = "Hands-Live H&S system";
			p2Desc[20] = "CIS Manager";
			p2Desc[21] = "Simplicity Hire Module";
			p2Desc[22] = "Asset Manager – Simplicity has a full Asset system for • Company & Client asset analysing";
			p2Desc[23] = "Simplicity Company Filing Cabinet system setup";
			p2Desc[24] = "Simplicity Client Cloud Document portal setup. Licences per client";
			p2Desc[25] = "Simplicity Web Client Appointment Booking system setup";
			p2Desc[26] = "Simplicity Sub – Contactors web portal setup";
			p2Desc[27] = "Simplicity Client Invoicing System setup";
			p1Unit[0] = "DAY";
			p1Unit[1] = "DAY";
			p1Unit[2] = "DAY";
			p1Unit[3] = "EF";
			p1Unit[4] = "NO";
			p1Unit[5] = "NO";
			p1Unit[6] = "OPT";
			p1Unit[8] = "NO";
			p1Unit[9] = "NO";
			p1Unit[10] = "OPT";
			p1Unit[11] = "NO";
			for (int rowNo = 14; rowNo < 27; rowNo++)
			{
				p2Unit[rowNo] = "M";
			}
			try
			{
				foreach (KeyValuePair<string, S4BFormsControl> s4bFormControlEntry in s4bFormsControls)
				{
					S4BFormsControl s4bFormControl = s4bFormControlEntry.Value;
					if (s4bFormControl.fieldValue != null)
					{	
						switch (s4bFormControlEntry.Key)
						{
							case "VARIABLE_PG1_JOB_SEQUENCE":
								jobSequence = s4bFormControl.fieldValue.Trim();
								break;
							case "VARIABLE_PG1_USER_ID":
								userId = s4bFormControl.fieldValue.Trim();
								break;
							case "VAR_PG1_JOB_REF":
								jobRef = s4bFormControl.fieldValue.Trim();
								break;
							case "VARIABLE_PG1_JOB_ADDRESS_ID":
								jobAddressId = s4bFormControl.fieldValue.Trim();
								break;
							case "VAR_PG1_JOB_ADDRESS_V":
								siteAddress = s4bFormControl.fieldValue.Trim();
								break;
							case "VARIABLE_PG1_JOB_CLIENT_ID":
								clientId = s4bFormControl.fieldValue.Trim();
								break;
							case "VAR_PG1_CLIENT_LONG_NAME":
								clientName = s4bFormControl.fieldValue.Trim();
								break;
							case "VAR_PG1_JOB_CLIENT_REF":
								clientRef = s4bFormControl.fieldValue.Trim();
								break;
							case "VARIABLE_PG1_DIARY_ENTRY_ID":
								diarySequence = s4bFormControl.fieldValue.Trim();
								break;
							case "VAR_PG1_JOB_EST_DATE":
								resultDate = DateTime.MinValue;
								try
								{
									resultDate = DateTime.ParseExact(s4bFormControl.fieldValue.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
								}
								catch (Exception ex)
								{ }
								jobEstDate = resultDate;
								break;
							case "FIELD_PG1_ORDER_DATE":
								resultDate = DateTime.MinValue;
								try
								{
									resultDate = DateTime.ParseExact(s4bFormControl.fieldValue.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
								}
								catch (Exception ex)
								{ }
								orderDate = resultDate;
								break;
							case "FIELD_PG1_ROW01_AMOUNT":
							case "FIELD_PG1_ROW02_AMOUNT":
							case "FIELD_PG1_ROW03_AMOUNT":
							case "FIELD_PG1_ROW04_AMOUNT":
							case "FIELD_PG1_ROW05_AMOUNT":
							case "FIELD_PG1_ROW06_AMOUNT":
							case "FIELD_PG1_ROW07_AMOUNT":
							case "FIELD_PG1_ROW08_AMOUNT":
							case "FIELD_PG1_ROW09_AMOUNT":
							case "FIELD_PG1_ROW10_AMOUNT":
							case "FIELD_PG1_ROW11_AMOUNT":
							case "FIELD_PG1_ROW12_AMOUNT":
							case "FIELD_PG2_ROW01_AMOUNT":
							case "FIELD_PG2_ROW02_AMOUNT":
							case "FIELD_PG2_ROW03_AMOUNT":
							case "FIELD_PG2_ROW04_AMOUNT":
							case "FIELD_PG2_ROW05_AMOUNT":
							case "FIELD_PG2_ROW06_AMOUNT":
							case "FIELD_PG2_ROW07_AMOUNT":
							case "FIELD_PG2_ROW08_AMOUNT":
							case "FIELD_PG2_ROW09_AMOUNT":
							case "FIELD_PG2_ROW10_AMOUNT":
							case "FIELD_PG2_ROW12_AMOUNT":
							case "FIELD_PG2_ROW13_AMOUNT":
							case "FIELD_PG2_ROW14_AMOUNT":
							case "FIELD_PG2_ROW15_AMOUNT":
							case "FIELD_PG2_ROW16_AMOUNT":
							case "FIELD_PG2_ROW17_AMOUNT":
							case "FIELD_PG2_ROW18_AMOUNT":
							case "FIELD_PG2_ROW19_AMOUNT":
							case "FIELD_PG2_ROW20_AMOUNT":
							case "FIELD_PG2_ROW21_AMOUNT":
							case "FIELD_PG2_ROW22_AMOUNT":
							case "FIELD_PG2_ROW23_AMOUNT":
							case "FIELD_PG2_ROW24_AMOUNT":
							case "FIELD_PG2_ROW25_AMOUNT":
							case "FIELD_PG2_ROW26_AMOUNT":
							case "FIELD_PG2_ROW27_AMOUNT":
							case "FIELD_PG2_ROW28_AMOUNT":
							case "FIELD_PG2_ROW29_AMOUNT":
								index = -1;
								resultDouble = 0;
								int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
								double.TryParse(s4bFormControl.fieldValue.Trim(), out resultDouble);
								if (s4bFormControlEntry.Key.Substring(8, 1) == "1")
									p1Amount[index - 1] = resultDouble;
								else
									p2Amount[index - 1] = resultDouble;
								break;
							case "VAR_PG2_ROW11_ALL_P2_TOTAL_ROWS":
								resultDouble = 0;
								double.TryParse(s4bFormControl.fieldValue.Trim(), out resultDouble);
								p2Amount[11] = resultDouble;
								break;
							case "FIELD_PG1_ROW01_QTY":
							case "FIELD_PG1_ROW02_QTY":
							case "FIELD_PG1_ROW03_QTY":
							case "FIELD_PG1_ROW04_QTY":
							case "FIELD_PG1_ROW05_QTY":
							case "FIELD_PG1_ROW06_QTY":
							case "FIELD_PG1_ROW07_QTY":
							case "FIELD_PG1_ROW08_QTY":
							case "FIELD_PG1_ROW09_QTY":
							case "FIELD_PG1_ROW10_QTY":
							case "FIELD_PG1_ROW11_QTY":
							case "FIELD_PG1_ROW12_QTY":
							case "FIELD_PG2_ROW01_QTY":
							case "FIELD_PG2_ROW02_QTY":
							case "FIELD_PG2_ROW03_QTY":
							case "FIELD_PG2_ROW04_QTY":
							case "FIELD_PG2_ROW06_QTY":
							case "FIELD_PG2_ROW07_QTY":
							case "FIELD_PG2_ROW08_QTY":
							case "FIELD_PG2_ROW09_QTY":
							case "FIELD_PG2_ROW10_QTY":
							case "FIELD_PG2_ROW11_QTY":
							case "FIELD_PG2_ROW17_QTY":
								index = -1;
								resultDouble = 0;
								int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
								double.TryParse(s4bFormControl.fieldValue.Trim(), out resultDouble);
								if (s4bFormControlEntry.Key.Substring(8, 1) == "1")
									p1Qty[index - 1] = resultDouble;
								else
									p2Qty[index - 1] = resultDouble;
								break;
							case "VAR_PG1_ROW01_TOTAL":
							case "VAR_PG1_ROW02_TOTAL":
							case "VAR_PG1_ROW03_TOTAL":
							case "VAR_PG1_ROW04_TOTAL":
							case "VAR_PG1_ROW05_TOTAL":
							case "VAR_PG1_ROW06_TOTAL":
							case "VAR_PG1_ROW07_TOTAL":
							case "VAR_PG1_ROW08_TOTAL":
							case "VAR_PG1_ROW09_TOTAL":
							case "VAR_PG1_ROW10_TOTAL":
							case "VAR_PG1_ROW11_TOTAL":
							case "VAR_PG1_ROW12_TOTAL":
							case "VAR_PG2_ROW01_TOTAL":
							case "VAR_PG2_ROW02_TOTAL":
							case "VAR_PG2_ROW03_TOTAL":
							case "VAR_PG2_ROW04_TOTAL":
							case "VAR_PG2_ROW05_TOTAL":
							case "VAR_PG2_ROW06_TOTAL":
							case "VAR_PG2_ROW07_TOTAL":
							case "VAR_PG2_ROW08_TOTAL":
							case "VAR_PG2_ROW09_TOTAL":
							case "VAR_PG2_ROW10_TOTAL":
							case "VAR_PG2_ROW11_TOTAL":
							case "VAR_PG2_ROW12_TOTAL":
							case "VAR_PG2_ROW13_TOTAL":
							case "VAR_PG2_ROW14_TOTAL":
							case "VAR_PG2_ROW15_TOTAL":
							case "VAR_PG2_ROW16_TOTAL":
							case "VAR_PG2_ROW17_TOTAL":
							case "VAR_PG2_ROW18_TOTAL":
							case "VAR_PG2_ROW19_TOTAL":
							case "VAR_PG2_ROW20_TOTAL":
							case "VAR_PG2_ROW21_TOTAL":
							case "VAR_PG2_ROW22_TOTAL":
							case "VAR_PG2_ROW23_TOTAL":
							case "VAR_PG2_ROW24_TOTAL":
							case "VAR_PG2_ROW25_TOTAL":
							case "VAR_PG2_ROW26_TOTAL":
							case "VAR_PG2_ROW27_TOTAL":
							case "VAR_PG2_ROW28_TOTAL":
							case "VAR_PG2_ROW29_TOTAL":
							case "VAR_PG2_ROW31_TOTAL":
								index = -1;
								resultDouble = 0;
								int.TryParse(s4bFormControlEntry.Key.Substring(11, 2), out index);
								double.TryParse(s4bFormControl.fieldValue.Trim(), out resultDouble);
								if (s4bFormControlEntry.Key.Substring(6, 1) == "1")
									p1Total[index - 1] = resultDouble;
								else
									p2Total[index - 1] = resultDouble;
								break;
							case "VAR_PG1_SUBTOTAL_1":
								resultDouble = 0;
								double.TryParse(s4bFormControl.fieldValue.Trim(), out resultDouble);
								subTotal1 = resultDouble;
								break;
							case "VAR_PG1_SUBTOTAL_2":
								resultDouble = 0;
								double.TryParse(s4bFormControl.fieldValue.Trim(), out resultDouble);
								subTotal2 = resultDouble;
								break;
							case "VAR_PG1_SUBTOTAL_3":
								resultDouble = 0;
								double.TryParse(s4bFormControl.fieldValue.Trim(), out resultDouble);
								subTotal3 = resultDouble;
								break;
							case "VAR_PG2_ROW05_QTY_TOTAL":
								resultDouble = 0;
								double.TryParse(s4bFormControl.fieldValue.Trim(), out resultDouble);
								p2Qty[4] = resultDouble;
								break;
							case "VAR_PG2_ROW14_SUBTOTAL":
							case "VAR_PG2_ROW30_SUBTOTAL":
								index = -1;
								resultDouble = 0;
								int.TryParse(s4bFormControlEntry.Key.Substring(11, 2), out index);
								double.TryParse(s4bFormControl.fieldValue.Trim(), out resultDouble);
								subTotal[index - 1] = resultDouble;
								break;
							case "FIELD_PG1_ROW08_DESC":
							case "FIELD_PG2_ROW07_DESC":
							case "FIELD_PG2_ROW13_DESC":
							case "FIELD_PG2_ROW29_DESC":
								index = -1;
								int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
								if (s4bFormControlEntry.Key.Substring(8, 1) == "1")
									p1Desc[index - 1] = s4bFormControl.fieldValue.Trim();
								else
									p2Desc[index - 1] = s4bFormControl.fieldName.Trim();
								break;
							case "FIELD_PG1_ROW08_UNIT":
							case "FIELD_PG2_ROW01_UNIT":
							case "FIELD_PG2_ROW02_UNIT":
							case "FIELD_PG2_ROW03_UNIT":
							case "FIELD_PG2_ROW04_UNIT":
							case "FIELD_PG2_ROW05_UNIT":
							case "FIELD_PG2_ROW06_UNIT":
							case "FIELD_PG2_ROW07_UNIT":
							case "FIELD_PG2_ROW08_UNIT":
							case "FIELD_PG2_ROW09_UNIT":
							case "FIELD_PG2_ROW10_UNIT":
							case "FIELD_PG2_ROW13_UNIT":
								index = -1;
								int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
								if (s4bFormControlEntry.Key.Substring(8, 1) == "1")
									p1Unit[index - 1] = s4bFormControl.fieldValue.Trim();
								else
									p2Unit[index - 1] = s4bFormControl.fieldValue.Trim();
								break;
							case "FIELD_PG1_ROW10_EMAIL_ADD":
								emailAdd = s4bFormControl.fieldValue.Trim();
								break;
							case "FIELD_PG2_ROW02_DESC_PART2":
							case "FIELD_PG2_ROW12_DESC_PART2":
								index = -1;
								int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
								descPart2[index - 1] = s4bFormControl.fieldValue.Trim();
								break;
							case "FIELD_PG2_ROW15_INC": //boolean
							case "FIELD_PG2_ROW16_INC":
							case "FIELD_PG2_ROW18_INC":
							case "FIELD_PG2_ROW19_INC":
							case "FIELD_PG2_ROW20_INC":
							case "FIELD_PG2_ROW21_INC":
							case "FIELD_PG2_ROW22_INC":
							case "FIELD_PG2_ROW23_INC":
							case "FIELD_PG2_ROW24_INC":
							case "FIELD_PG2_ROW25_INC":
							case "FIELD_PG2_ROW26_INC":
							case "FIELD_PG2_ROW27_INC":
							case "FIELD_PG2_ROW28_INC":
							case "FIELD_PG2_ROW29_INC":
								index = -1;
								int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
								inc[index - 1] = s4bFormControl.fieldValue.Trim();
								break;
						}
					}
				}
				//Get join sequence
				OrdersMeHeaderRepository objHR = new OrdersMeHeaderRepository();
				OrdersMeHeader objOrdHeader = objHR.GetOrdersMeHeaderByJobSequence(Convert.ToInt64(jobSequence), request);
				// Insert Header Record
				OrdersMeSchHeader objH = new OrdersMeSchHeader();
				objH.JobSequence = Convert.ToInt64(jobSequence);
				objH.joinSequence = objOrdHeader.Sequence;
				objH.FlgFinalised = false;
				objH.MeVersionNo = s4bFormSubmission.S4bSubmitTs;
				objH.DateMeVersionNo = DateTime.Now;
				objH.CreatedBy = Int32.Parse(request.Headers["UserId"].ToString());
				objH.DateCreated = DateTime.Now;
				OrdersMeSchHeaderRepository objHRepository = new OrdersMeSchHeaderRepository();
				OrdersMeSchHeader retHeader = objHRepository.Insert(objH, request);
				OrdersMeSchItemsRepository objItemsRepository = new OrdersMeSchItemsRepository();
				if (retHeader != null) // if header is inserted
				{
					for (int rowNo = 0; rowNo < P1_NO_ROWS; rowNo++)
					{
						if (p1Qty[rowNo] != 0)
						{
							OrdersMeSchItems objItems = new OrdersMeSchItems();
							objItems.joinSequence = retHeader.Sequence;
							objItems.JobSequence = Convert.ToInt64(jobSequence);
							objItems.ClonedFromSeq = -1;
							objItems.RowIndex = rowNo + 1;
							objItems.FlgRowLocked = false;
							objItems.FlgRowSelected = false;
							objItems.FlgRowIsText = false;
							objItems.ItemType = 0;
							objItems.ItemDesc = string.IsNullOrEmpty(p1Desc[rowNo]) ? "" : p1Desc[rowNo];
							objItems.ItemUnits = string.IsNullOrEmpty(p1Unit[rowNo]) ? "" : p1Unit[rowNo];
							objItems.ItemQuantity = p1Qty[rowNo];
							objItems.AmountLabour = p1Amount[rowNo];
							objItems.AmountValue = p1Total[rowNo];
							objItems.AmountTotal = p1Total[rowNo];
							objItems.TransType = "B";
							objItems.ItemCode = "";
							objItems.CreatedBy = Int32.Parse(request.Headers["UserId"]);
							objItems.DateCreated = DateTime.Now;
							objItems.LastAmendedBy = -1;
							objItemsRepository.Insert(objItems, request);
						}
					}
					for (int rowNo = 0; rowNo < P2_NO_ROWS; rowNo++)
					{
						if (p2Qty[rowNo] != 0)
						{
							OrdersMeSchItems objItems = new OrdersMeSchItems();
							objItems.joinSequence = retHeader.Sequence;
							objItems.JobSequence = Convert.ToInt64(jobSequence);
							objItems.ClonedFromSeq = -1;
							objItems.RowIndex = rowNo + 1;
							objItems.FlgRowLocked = false;
							objItems.FlgRowSelected = false;
							objItems.FlgRowIsText = false;
							objItems.ItemType = 0;
							objItems.ItemDesc = string.IsNullOrEmpty(p2Desc[rowNo]) ? "" : p2Desc[rowNo];
							objItems.ItemUnits = string.IsNullOrEmpty(p2Unit[rowNo]) ? "" : p2Unit[rowNo];
							objItems.ItemQuantity = p2Qty[rowNo];
							objItems.AmountLabour = p2Amount[rowNo];
							objItems.AmountValue = p2Total[rowNo];
							objItems.AmountTotal = p2Total[rowNo];
							objItems.TransType = "B";
							objItems.ItemCode = "";
							objItems.CreatedBy = Int32.Parse(request.Headers["UserId"]);
							objItems.DateCreated = DateTime.Now;
							objItems.LastAmendedBy = -1;
							objItemsRepository.Insert(objItems, request);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Message = "Unable to Process UNC Client Quotation. Exception: " + ex.Message;
				Utilities.WriteLog(Message);
			}
			return returnValue;
		}

		// Franklin Hodge Site Inspection V06 "1023744743" 
		private bool ProcessFHSiteInspection(HttpRequest request, S4BFormRequest s4bFormRequest, S4bFormSubmissions s4bFormSubmission,
                                           RootObject root, Dictionary<string, S4BFormsControl> s4bFormsControls)
      {
         bool returnValue = false;
         double resultDouble = 0;
         DateTime? resultDate = DateTime.MinValue;
         string jobRef = "", p1SiteAddress = "", p1ClientName = "", p1TankType = "", p1TankSize = "", p1InspectedBy = "", p2Location = "";
         string p2ContactName = "", p2ContactTelNo = "", p2ContactExt = "", p2ContactMobile = "", p2Lcontacts = "", p2VisitStatus = "";
         string p2ManufacturerDetails = "", p2VisitPurpose = "", p2VisitPurpose2 = "", p3TankStatus = "", p3TankDiameter = "";
         string p3TankShape = "", p3TankSize = "", p3TankManufacturer = "", p3PanelDimensions = "", p3TankType = "";
         string p3TopPanelDimensions = "", p3HozBoltSeam = "", p3TankHeight = "", p3ActualCapacity = "", p3TankDetails = "";
         string p3TankShellDetails = "", p3TankShellDetails2 = "", p4TestReturn = "", p4DrainValve = "", p4Suction = "";
         string p4OverFlows = "", p4InletValve = "", p4ImmersionHeater = "", p4LowLevelManway = "", p6AncillaryItems = "";
         string p6ExternalLadder = "", p6InletValveHousing = "", p7ConclusionsAndRecomm = "", p7SignatureImageFile = "";
         const int P5_NO_ROWS = 11;
         const int P6_NO_ROWS = 8;
         const int P7_NO_OBSERVATIONS = 10;
         string[] p5Size = new string[P5_NO_ROWS];
         double[] p5Qty = new double[P5_NO_ROWS];
         string[] p5Condition = new string[P5_NO_ROWS];
         string[] p5Comments = new string[P5_NO_ROWS];
         double[] p6Qty = new double[P6_NO_ROWS];
         string[] p6Condition = new string[P6_NO_ROWS];
         string[] p6Comments = new string[P6_NO_ROWS];
         string[] p7Observations = new string[P7_NO_OBSERVATIONS];
         Dictionary<string, string> imageValues = new Dictionary<string, string>();
         int index = -1;
         DateTime? p1ReportDate = DateTime.MinValue, p2InstallationDate = DateTime.MinValue, p3TankInstallationDate = DateTime.MinValue;
         try
         {
            foreach (KeyValuePair<string, S4BFormsControl> s4bFormControlEntry in s4bFormsControls)
            {
               S4BFormsControl s4bFormControl = s4bFormControlEntry.Value;
               if (s4bFormControl.fieldValue != null)
               {
                  switch (s4bFormControlEntry.Key)
                  {
                     case "FIELD_VAR_JOB_REFERENCE":
                        jobRef = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG1_SITE_ADDRESS":
                        p1SiteAddress = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG1_CLIENT_NAME":
                        p1ClientName = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG1_TANK_TYPE":
                        p1TankType = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG1_TANK_SIZE":
                        p1TankSize = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG1_REPORT_DATE":
                        resultDate = DateTime.MinValue;
                        try
                        {
                           resultDate = DateTime.ParseExact(s4bFormControl.fieldValue.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        }
                        catch (Exception ex)
                        { }
                        p1ReportDate = resultDate;
                        break;
                     case "FIELD_PG1_INSPECTION_BY":
                        p1InspectedBy = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG2_LOCATION":
                        p2Location = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG2_INSTALLATION_DATE":
                        resultDate = DateTime.MinValue;
                        try
                        {
                           resultDate = DateTime.ParseExact(s4bFormControl.fieldValue.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        }
                        catch (Exception ex)
                        { }
                        p2InstallationDate = resultDate;
                        break;
                     case "FIELD_PG2_CONTACT_NAME":
                        p2ContactName = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG2_CONTACT_TEL_NO":
                        p2ContactTelNo = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG2_CONTACT_EXT":
                        p2ContactExt = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG2_CONTACT_MOBILE_NO":
                        p2ContactMobile = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG2_CONTACTS":
                        p2Lcontacts = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG2_VISIT_STATUS":
                        p2VisitStatus = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG2_MANUFACTURER_DETAILS":
                        p2ManufacturerDetails = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG2_VISIT_PURPOSE":
                        p2VisitPurpose = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG2_VISIT_PURPOSE2":
                        p2VisitPurpose2 = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG3_TANK_STATUS":
                        p3TankStatus = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG3_TANK_DIAMETER":
                        p3TankDiameter = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG3_TANK_SHAPE":
                        p3TankShape = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG3_TANK_SIZE":
                        p3TankSize = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG3_TANK_MANUFACRURER":
                        p3TankManufacturer = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG3_PANEL_DIMENSIONS":
                        p3PanelDimensions = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG3_TANK_TYPE":
                        p3TankType = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG3_TOP_PANEL_DIMENSIONS":
                        p3TopPanelDimensions = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG3_TANK_INSTALLATION_DATE":
                        resultDate = DateTime.MinValue;
                        try
                        {
                           resultDate = DateTime.ParseExact(s4bFormControl.fieldValue.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        }
                        catch (Exception ex)
                        { }
                        p3TankInstallationDate = resultDate;
                        break;
                     case "FIELD_PG3_HOZ_BOLT_SEAM":
                        p3HozBoltSeam = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG3_TANK_HEIGHT":
                        p3TankHeight = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG3_ACTUAL_CAPACITY":
                        p3ActualCapacity = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG3_TANK_DETAILS":
                        p3TankDetails = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG3_TANK_SHELL_EXTERNAL":
                        p3TankShellDetails = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG3_TANK_SHELL_EXTERNAL2":
                        p3TankShellDetails2 = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG4_TEST_RETURN":
                        p4TestReturn = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG4_DRAIN_VALVE":
                        p4DrainValve = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG4_SUCTION":
                        p4Suction = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG4_OVERFLOWS":
                        p4OverFlows = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG4_INLET_FLOAT_VALVE":
                        p4InletValve = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG4_IMMERSION_HEATER":
                        p4ImmersionHeater = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG4_LOW_LEVEL_MANWAY":
                        p4LowLevelManway = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG5_ROW01_SIZE":
                     case "FIELD_PG5_ROW02_SIZE":
                     case "FIELD_PG5_ROW03_SIZE":
                     case "FIELD_PG5_ROW04_SIZE":
                     case "FIELD_PG5_ROW05_SIZE":
                     case "FIELD_PG5_ROW06_SIZE":
                     case "FIELD_PG5_ROW07_SIZE":
                     case "FIELD_PG5_ROW08_SIZE":
                     case "FIELD_PG5_ROW09_SIZE":
                     case "FIELD_PG5_ROW10_SIZE":
                     case "FIELD_PG5_ROW11_SIZE":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        p5Size[index - 1] = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG5_ROW01_QTY":
                     case "FIELD_PG5_ROW02_QTY":
                     case "FIELD_PG5_ROW03_QTY":
                     case "FIELD_PG5_ROW04_QTY":
                     case "FIELD_PG5_ROW05_QTY":
                     case "FIELD_PG5_ROW06_QTY":
                     case "FIELD_PG5_ROW07_QTY":
                     case "FIELD_PG5_ROW08_QTY":
                     case "FIELD_PG5_ROW09_QTY":
                     case "FIELD_PG5_ROW10_QTY":
                     case "FIELD_PG5_ROW11_QTY":
                        index = -1;
                        resultDouble = 0;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        double.TryParse(s4bFormControl.fieldValue.Trim(), out resultDouble);
                        p5Qty[index - 1] = resultDouble;
                        break;
                     case "FIELD_PG5_ROW01_CONDITION":
                     case "FIELD_PG5_ROW02_CONDITION":
                     case "FIELD_PG5_ROW03_CONDITION":
                     case "FIELD_PG5_ROW04_CONDITION":
                     case "FIELD_PG5_ROW05_CONDITION":
                     case "FIELD_PG5_ROW06_CONDITION":
                     case "FIELD_PG5_ROW07_CONDITION":
                     case "FIELD_PG5_ROW08_CONDITION":
                     case "FIELD_PG5_ROW09_CONDITION":
                     case "FIELD_PG5_ROW10_CONDITION":
                     case "FIELD_PG5_ROW11_CONDITION":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        p5Condition[index - 1] = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG5_ROW01_COMMENTS":
                     case "FIELD_PG5_ROW02_COMMENTS":
                     case "FIELD_PG5_ROW03_COMMENTS":
                     case "FIELD_PG5_ROW04_COMMENTS":
                     case "FIELD_PG5_ROW05_COMMENTS":
                     case "FIELD_PG5_ROW06_COMMENTS":
                     case "FIELD_PG5_ROW07_COMMENTS":
                     case "FIELD_PG5_ROW08_COMMENTS":
                     case "FIELD_PG5_ROW09_COMMENTS":
                     case "FIELD_PG5_ROW10_COMMENTS":
                     case "FIELD_PG5_ROW11_COMMENTS":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        p5Comments[index - 1] = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG6_ANCILLARY_ITEMS":
                        p6AncillaryItems = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG6_EXTERNAL_LADDER":
                        p6ExternalLadder = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG6_INLET_VALVE_HOUSING":
                        p6InletValveHousing = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG6_ROW01_QTY":
                     case "FIELD_PG6_ROW02_QTY":
                     case "FIELD_PG6_ROW03_QTY":
                     case "FIELD_PG6_ROW04_QTY":
                     case "FIELD_PG6_ROW05_QTY":
                     case "FIELD_PG6_ROW06_QTY":
                     case "FIELD_PG6_ROW07_QTY":
                     case "FIELD_PG6_ROW08_QTY":
                        index = -1;
                        resultDouble = 0;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        double.TryParse(s4bFormControl.fieldValue.Trim(), out resultDouble);
                        p6Qty[index - 1] = resultDouble;
                        break;
                     case "FIELD_PG6_ROW01_CONDITION":
                     case "FIELD_PG6_ROW02_CONDITION":
                     case "FIELD_PG6_ROW03_CONDITION":
                     case "FIELD_PG6_ROW04_CONDITION":
                     case "FIELD_PG6_ROW05_CONDITION":
                     case "FIELD_PG6_ROW06_CONDITION":
                     case "FIELD_PG6_ROW07_CONDITION":
                     case "FIELD_PG6_ROW08_CONDITION":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        p6Condition[index - 1] = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG6_ROW01_COMMENTS":
                     case "FIELD_PG6_ROW02_COMMENTS":
                     case "FIELD_PG6_ROW03_COMMENTS":
                     case "FIELD_PG6_ROW04_COMMENTS":
                     case "FIELD_PG6_ROW05_COMMENTS":
                     case "FIELD_PG6_ROW06_COMMENTS":
                     case "FIELD_PG6_ROW07_COMMENTS":
                     case "FIELD_PG6_ROW08_COMMENTS":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        p6Comments[index - 1] = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG7_ROW01_OBSERVATIONS":
                     case "FIELD_PG7_ROW02_OBSERVATIONS":
                     case "FIELD_PG7_ROW03_OBSERVATIONS":
                     case "FIELD_PG7_ROW04_OBSERVATIONS":
                     case "FIELD_PG7_ROW05_OBSERVATIONS":
                     case "FIELD_PG7_ROW06_OBSERVATIONS":
                     case "FIELD_PG7_ROW07_OBSERVATIONS":
                     case "FIELD_PG7_ROW08_OBSERVATIONS":
                     case "FIELD_PG7_ROW09_OBSERVATIONS":
                     case "FIELD_PG7_ROW10_OBSERVATIONS":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        p7Observations[index - 1] = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG7_RECOMMENDATIONS":
                        p7ConclusionsAndRecomm = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG7_SIGNATURE":
                     case "FIELD_PG2_VISIT_PURPOSE_IMAGE":
                     case "FIELD_PG2_VISIT_PURPOSE2_IMAGE2":
                     case "FIELD_PG3_TANK_SHELL_EXTERNAL_IMAGE":
                     case "FIELD_PG3_TANK_SHELL_EXTERNAL2_IMAGE":
                     case "FIELD_PG4_TEST_RETURN_IMAGE":
                     case "FIELD_PG4_DRAIN_VALVE_IMAGE":
                     case "FIELD_PG4_SUCTION_IMAGE":
                     case "FIELD_PG4_OVERFLOWS_IMAGE":
                     case "FIELD_PG4_INLET_FLOAT_VALVE_IMAGE":
                     case "FIELD_PG4_IMMERSION_HEATER_IMAGE":
                     case "FIELD_PG4_LOW_LEVEL_MANWAY_IMAGE":
                     case "FIELD_PG6_EXTERNAL_LADDER_IMAGE":
                     case "FIELD_PG6_INLET_VALVE_HOUSING_IMAGE":
                        imageValues.Add(s4bFormControlEntry.Key, s4bFormControl.fieldValue.Trim());
                        break;
                  }
               }
            }
            //Check and Create Job Ref
            if (!string.IsNullOrEmpty(jobRef))
            {
               jobRef = Utilities.FormatJobRefForCreateOrder(jobRef);
               OrdersRepository orderRepostory = new OrdersRepository(null);
               Orders order = orderRepostory.CreateOrderByJobRef(jobRef, false, request, null);
            }
            // Insert Header Record
            // Prepare Data for Insertion
            SubmissionsDataFh siteInspection = new SubmissionsDataFh();
            siteInspection.P1ContractNo = jobRef;
            siteInspection.SubmitNo = s4bFormSubmission.S4bSubmitNo;
            siteInspection.SubmitTs = s4bFormSubmission.S4bSubmitTs;
            siteInspection.DateSubmit = s4bFormSubmission.DateSubmit;
            siteInspection.FileCabId = s4bFormSubmission.FileCabId;
            siteInspection.FlgAmended = false;
            siteInspection.FlgResubmission = false;
            siteInspection.CreatedPDFCount = 1;
            siteInspection.P1ContractNo = jobRef;
            siteInspection.P1SiteAddress = p1SiteAddress;
            siteInspection.P1ClientName = p1ClientName;
            siteInspection.P1TankType = p1TankType;
            siteInspection.P1TankSize = p1TankSize;
            siteInspection.P1ReportDate = p1ReportDate;
            siteInspection.P1InspectedBy = p1InspectedBy;
            siteInspection.P2Location = p2Location;
            siteInspection.P2InstallationDate = p2InstallationDate;
            siteInspection.P2ContactName = p2ContactName;
            siteInspection.P2ContactTelNo = p2ContactTelNo;
            siteInspection.P2ContacText = p2ContactExt;
            siteInspection.P2ContactMobile = p2ContactMobile;
            siteInspection.P2LContacts = p2Lcontacts;
            siteInspection.P2VisitStatus = p2VisitStatus;
            siteInspection.P2ManufacturerDetails = p2ManufacturerDetails;
            siteInspection.P2VisitPurpose = p2VisitPurpose;
            siteInspection.P2VisitPurpose2 = p2VisitPurpose2;
            siteInspection.P3TankStatus = p3TankStatus;
            siteInspection.P3TankDiameter = p3TankDiameter;
            siteInspection.P3TankShape = p3TankShape;
            siteInspection.P3Tanksize = p3TankSize;
            siteInspection.P3TankManufacturer = p3TankManufacturer;
            siteInspection.P3PanelDimensions = p3PanelDimensions;
            siteInspection.P3TankType = p3TankType;
            siteInspection.P3ToppanelDimensions = p3TopPanelDimensions;
            siteInspection.P3TankInstallationDate = p3TankInstallationDate;
            siteInspection.P3Hozboltseam = p3HozBoltSeam;
            siteInspection.P3TankHeight = p3TankHeight;
            siteInspection.P3ActualCapacity = p3ActualCapacity;
            siteInspection.P3TankDetails = p3TankDetails;
            siteInspection.P3TankShellDetails = p3TankShellDetails;
            siteInspection.P3TankShellDetails2 = p3TankShellDetails2;
            siteInspection.P4TestReturn = p4TestReturn;
            siteInspection.P4DrainValve = p4DrainValve;
            siteInspection.P4Suction = p4Suction;
            siteInspection.P4OverFlows = p4OverFlows;
            siteInspection.P4InletValve = p4InletValve;
            siteInspection.P4ImmersionHeater = p4ImmersionHeater;
            siteInspection.P4LowLevelManWay = p4LowLevelManway;
            siteInspection.P6AncillaryItems = p6AncillaryItems;
            siteInspection.P6ExternalLadder = p6ExternalLadder;
            siteInspection.P6InletValveHousing = p6InletValveHousing;
            siteInspection.P7Observations01 = p7Observations[0];
            siteInspection.P7Observations02 = p7Observations[1];
            siteInspection.P7Observations03 = p7Observations[2];
            siteInspection.P7Observations04 = p7Observations[3];
            siteInspection.P7Observations05 = p7Observations[4];
            siteInspection.P7Observations06 = p7Observations[5];
            siteInspection.P7Observations07 = p7Observations[6];
            siteInspection.P7Observations08 = p7Observations[7];
            siteInspection.P7Observations09 = p7Observations[8];
            siteInspection.P7Observations10 = p7Observations[9];
            siteInspection.P7Conclusions = p7ConclusionsAndRecomm;
            siteInspection.P7SignatureImageFile = p7SignatureImageFile;
            siteInspection.CreatedBy = Int32.Parse(request.Headers["UserId"].ToString());
            siteInspection.DateCreated = siteInspection.DateSubmit;
            for (int rowNo = 0; rowNo < P5_NO_ROWS; rowNo++)
            {
               if (p5Qty[rowNo] != 0 || !string.IsNullOrEmpty(p5Size[rowNo]) ||
                   !string.IsNullOrEmpty(p5Comments[rowNo]) || !string.IsNullOrEmpty(p5Condition[rowNo]))
               {
                  SubmissionsDataFhi tankConnection = new SubmissionsDataFhi();
                  tankConnection.PageNo = 5;
                  tankConnection.RowNo = rowNo + 1;
                  tankConnection.RowSize = p5Size[rowNo];
                  tankConnection.RowQty = p5Qty[rowNo];
                  tankConnection.RowCondition = p5Condition[rowNo];
                  tankConnection.RowComments = p5Comments[rowNo];
                  tankConnection.CreatedBy = Int32.Parse(request.Headers["UserId"]);
                  tankConnection.DateCreated = DateTime.Now;
                  tankConnection.LastAmendedBy = -1;
                  tankConnection.DateLastAmended = DateTime.Now;
                  if (siteInspection.TankConnections == null)
                  {
                     siteInspection.TankConnections = new List<SubmissionsDataFhi>();
                  }
                  siteInspection.TankConnections.Add(tankConnection);
               }
            }
            for (int rowNo = 0; rowNo < P6_NO_ROWS; rowNo++)
            {
               if (p6Qty[rowNo] != 0 ||
                   !string.IsNullOrEmpty(p6Comments[rowNo]) || !string.IsNullOrEmpty(p6Condition[rowNo]))
               {
                  SubmissionsDataFhi ancillaryItem = new SubmissionsDataFhi();
                  ancillaryItem.PageNo = 6;
                  ancillaryItem.RowNo = rowNo + 1;
                  ancillaryItem.RowSize = "";
                  ancillaryItem.RowQty = p5Qty[rowNo];
                  ancillaryItem.RowCondition = p5Condition[rowNo];
                  ancillaryItem.RowComments = p5Comments[rowNo];
                  ancillaryItem.CreatedBy = Int32.Parse(request.Headers["UserId"]);
                  ancillaryItem.DateCreated = DateTime.Now;
                  ancillaryItem.LastAmendedBy = -1;
                  ancillaryItem.DateLastAmended = DateTime.Now;
                  if (siteInspection.AncillaryItems == null)
                  {
                     siteInspection.AncillaryItems = new List<SubmissionsDataFhi>();
                  }
                  siteInspection.AncillaryItems.Add(ancillaryItem);
               }
            }
            int pageNo;
            foreach (KeyValuePair<string, string> imageData in imageValues)
            {
               if (!string.IsNullOrEmpty(imageData.Value))
               {
                  if (siteInspection.SiteInspectionImages == null)
                  {
                     siteInspection.SiteInspectionImages = new List<SubmissionsImagesFh>();
                  }
                  pageNo = 1;
                  int.TryParse(imageData.Key.Substring(8, 1), out pageNo);
                  SubmissionsImagesFh submissionImageFh = new SubmissionsImagesFh();
                  submissionImageFh.FixedImage = true;
                  submissionImageFh.PageNo = pageNo;
                  submissionImageFh.FieldId = imageData.Key;
                  submissionImageFh.FileDisplayName = "";
                  submissionImageFh.FilePath = imageData.Value;
                  submissionImageFh.CreatedBy = Int32.Parse(request.Headers["UserId"]);
                  submissionImageFh.DateCreated = DateTime.Now;
                  submissionImageFh.CreatedBy = -1;
                  submissionImageFh.DateLastAmended = DateTime.Now;
                  siteInspection.SiteInspectionImages.Add(submissionImageFh);
               }
            }
            SiteInspectionRepository siteInspectionRepository = new SiteInspectionRepository();
            if (siteInspectionRepository.Insert(siteInspection, request, null))
            {
               returnValue = true;
            }
            else
            {
               Message = siteInspectionRepository.Message;
            }

         }
         catch (Exception ex)
         {
            Message = "Unable to Process Franklin Hodge Site Inspection. Exception: " + ex.Message;
         }
         return returnValue;
      }

      // Franklin Hodge Site Inspection V08 "470969094" 
      private bool ProcessFHSiteInspectionV08(HttpRequest request, S4BFormRequest s4bFormRequest, S4bFormSubmissions s4bFormSubmission,
                                              RootObject root, Dictionary<string, S4BFormsControl> s4bFormsControls)
      {
         bool returnValue = false;
         double resultDouble = 0;
         DateTime? resultDate = DateTime.MinValue;
         string jobRef = "", p1SiteAddress = "", p1ClientName = "", p1TankType = "", p1TankSize = "", p1InspectedBy = "", p2Location = "";
         string p2ContactName = "", p2ContactTelNo = "", p2ContactExt = "", p2ContactMobile = "", p2Lcontacts = "", p2VisitStatus = "";
         string p2InstallationDateUnknown = "", p2ManufacturerDetails = "", p2VisitPurpose = "", p3TankStatus = "", p3TankDiameter = "";
         string p3TankShape = "", p3TankSize = "", p3TankManufacturer = "", p3PanelDimensions = "", p3TankType = "";
         string p3TopPanelDimensions = "", p3HozBoltSeam = "", p3TankHeight = "", p3ActualCapacity = "", p3TankDetails = "";
         string p3TankShellDetails = "", p3TankShellDetails2 = "", p4RoofTankShellDetails = "", p4TestReturn = "", p5DrainValve = "", p5Suction = "";
         string p6OverFlows = "", p6InletValve = "", p7ImmersionHeater = "", p7LowLevelManway = "", P8ContentsGauge = "", p8LevelSwitches = "";
         string p10AncillaryItems = "";
         string p10ExternalLadder = "", p10InletValveHousing = "", p12ConclusionsAndRecomm = "", p12SignatureImageFile = "";
         const int P9_NO_ROWS = 12;
         const int P11_NO_ROWS = 8;
         const int P7_NO_OBSERVATIONS = 10;
         string[] p9Size = new string[P9_NO_ROWS];
         double[] p9Qty = new double[P9_NO_ROWS];
         string[] p9Condition = new string[P9_NO_ROWS];
         string[] p9Comments = new string[P9_NO_ROWS];
         double[] p11Qty = new double[P11_NO_ROWS];
         string[] p11Condition = new string[P11_NO_ROWS];
         string[] p11Comments = new string[P11_NO_ROWS];
         string[] p12Observations = new string[P7_NO_OBSERVATIONS];
         Dictionary<string, string> imageValues = new Dictionary<string, string>();
         int index = -1;
         DateTime? p1ReportDate = DateTime.MinValue, p2InstallationDate = DateTime.MinValue, p3TankInstallationDate = DateTime.MinValue;
         try
         {
            foreach (KeyValuePair<string, S4BFormsControl> s4bFormControlEntry in s4bFormsControls)
            {
               S4BFormsControl s4bFormControl = s4bFormControlEntry.Value;
               if (s4bFormControl.fieldValue != null)
               {
                  switch (s4bFormControlEntry.Key)
                  {
                     case "FIELD_VAR_JOB_REFERENCE":
                        jobRef = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG1_SITE_ADDRESS":
                        p1SiteAddress = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG1_CLIENT_NAME":
                        p1ClientName = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG1_TANK_TYPE":
                        p1TankType = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG1_TANK_SIZE":
                        p1TankSize = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG1_REPORT_DATE":
                        resultDate = DateTime.MinValue;
                        try
                        {
                           resultDate = DateTime.ParseExact(s4bFormControl.fieldValue.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        }
                        catch (Exception ex)
                        { }
                        p1ReportDate = resultDate;
                        break;
                     case "FIELD_PG1_INSPECTION_BY":
                        p1InspectedBy = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG2_LOCATION":
                        p2Location = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG2_INSTALLATION_DATE":
                        resultDate = DateTime.MinValue;
                        try
                        {
                           resultDate = DateTime.ParseExact(s4bFormControl.fieldValue.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        }
                        catch (Exception ex)
                        { }
                        p2InstallationDate = resultDate;
                        break;
                     case "FIELD_PG2_CONTACT_NAME":
                        p2ContactName = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG2_CONTACT_TEL_NO":
                        p2ContactTelNo = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG2_CONTACT_EXT":
                        p2ContactExt = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG2_CONTACT_MOBILE_NO":
                        p2ContactMobile = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG2_CONTACTS":
                        p2Lcontacts = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG2_VISIT_STATUS":
                        p2VisitStatus = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG2_MANUFACTURER_DETAILS":
                        p2ManufacturerDetails = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG2_INSTALLATION_DATE_UNKNOWN":
                        p2InstallationDateUnknown = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG2_VISIT_PURPOSE":
                        p2VisitPurpose = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG3_TANK_STATUS":
                        p3TankStatus = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG3_TANK_DIAMETER":
                        p3TankDiameter = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG3_TANK_SHAPE":
                        p3TankShape = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG3_TANK_SIZE":
                        p3TankSize = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG3_TANK_MANUFACRURER":
                        p3TankManufacturer = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG3_PANEL_DIMENSIONS":
                        p3PanelDimensions = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG3_TANK_TYPE":
                        p3TankType = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG3_TOP_PANEL_DIMENSIONS":
                        p3TopPanelDimensions = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG3_TANK_INSTALLATION_DATE":
                        resultDate = DateTime.MinValue;
                        try
                        {
                           resultDate = DateTime.ParseExact(s4bFormControl.fieldValue.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        }
                        catch (Exception ex)
                        { }
                        p3TankInstallationDate = resultDate;
                        break;
                     case "FIELD_PG3_HOZ_BOLT_SEAM":
                        p3HozBoltSeam = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG3_TANK_HEIGHT":
                        p3TankHeight = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG3_ACTUAL_CAPACITY":
                        p3ActualCapacity = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG3_TANK_DETAILS":
                        p3TankDetails = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG3_TANK_SHELL_DETAILS":
                        p3TankShellDetails = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG3_TANK_SHELL_EXTERNAL2":
                        p3TankShellDetails2 = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG4_ROOF_TANK_SHELL_DETAILS":
                        p4RoofTankShellDetails = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG4_TEST_RETURN":
                        p4TestReturn = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG5_DRAIN_VALVE":
                        p5DrainValve = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG5_SUCTION":
                        p5Suction = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG6_OVERFLOWS":
                        p6OverFlows = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG6_INLET_FLOAT_VALVE":
                        p6InletValve = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG7_IMMERSION_HEATER":
                        p7ImmersionHeater = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG7_LOW_LEVEL_MANWAY":
                        p7LowLevelManway = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG8_CONTENTS_GAUGE":
                        P8ContentsGauge = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG8_LEVEL_SWITCHES":
                        p8LevelSwitches = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG9_ROW01_SIZE":
                     case "FIELD_PG9_ROW02_SIZE":
                     case "FIELD_PG9_ROW03_SIZE":
                     case "FIELD_PG9_ROW04_SIZE":
                     case "FIELD_PG9_ROW05_SIZE":
                     case "FIELD_PG9_ROW06_SIZE":
                     case "FIELD_PG9_ROW07_SIZE":
                     case "FIELD_PG9_ROW08_SIZE":
                     case "FIELD_PG9_ROW09_SIZE":
                     case "FIELD_PG9_ROW10_SIZE":
                     case "FIELD_PG9_ROW11_SIZE":
                     case "FIELD_PG9_ROW12_SIZE":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        p9Size[index - 1] = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG9_ROW01_QTY":
                     case "FIELD_PG9_ROW02_QTY":
                     case "FIELD_PG9_ROW03_QTY":
                     case "FIELD_PG9_ROW04_QTY":
                     case "FIELD_PG9_ROW05_QTY":
                     case "FIELD_PG9_ROW06_QTY":
                     case "FIELD_PG9_ROW07_QTY":
                     case "FIELD_PG9_ROW08_QTY":
                     case "FIELD_PG9_ROW09_QTY":
                     case "FIELD_PG9_ROW10_QTY":
                     case "FIELD_PG9_ROW11_QTY":
                     case "FIELD_PG9_ROW12_QTY":
                        index = -1;
                        resultDouble = 0;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        double.TryParse(s4bFormControl.fieldValue.Trim(), out resultDouble);
                        p9Qty[index - 1] = resultDouble;
                        break;
                     case "FIELD_PG9_ROW01_CONDITION":
                     case "FIELD_PG9_ROW02_CONDITION":
                     case "FIELD_PG9_ROW03_CONDITION":
                     case "FIELD_PG9_ROW04_CONDITION":
                     case "FIELD_PG9_ROW05_CONDITION":
                     case "FIELD_PG9_ROW06_CONDITION":
                     case "FIELD_PG9_ROW07_CONDITION":
                     case "FIELD_PG9_ROW08_CONDITION":
                     case "FIELD_PG9_ROW09_CONDITION":
                     case "FIELD_PG9_ROW10_CONDITION":
                     case "FIELD_PG9_ROW11_CONDITION":
                     case "FIELD_PG9_ROW12_CONDITION":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        p9Condition[index - 1] = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG9_ROW01_COMMENTS":
                     case "FIELD_PG9_ROW02_COMMENTS":
                     case "FIELD_PG9_ROW03_COMMENTS":
                     case "FIELD_PG9_ROW04_COMMENTS":
                     case "FIELD_PG9_ROW05_COMMENTS":
                     case "FIELD_PG9_ROW06_COMMENTS":
                     case "FIELD_PG9_ROW07_COMMENTS":
                     case "FIELD_PG9_ROW08_COMMENTS":
                     case "FIELD_PG9_ROW09_COMMENTS":
                     case "FIELD_PG9_ROW10_COMMENTS":
                     case "FIELD_PG9_ROW11_COMMENTS":
                     case "FIELD_PG9_ROW12_COMMENTS":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        p9Comments[index - 1] = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG10_ANCILLARY_ITEMS":
                        p10AncillaryItems = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG10_EXTERNAL_LADDER":
                        p10ExternalLadder = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG10_INLET_VALVE_HOUSING":
                        p10InletValveHousing = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG11_ROW01_QTY":
                     case "FIELD_PG11_ROW02_QTY":
                     case "FIELD_PG11_ROW03_QTY":
                     case "FIELD_PG11_ROW04_QTY":
                     case "FIELD_PG11_ROW05_QTY":
                     case "FIELD_PG11_ROW06_QTY":
                     case "FIELD_PG11_ROW07_QTY":
                     case "FIELD_PG11_ROW08_QTY":
                        index = -1;
                        resultDouble = 0;
                        int.TryParse(s4bFormControlEntry.Key.Substring(14, 2), out index);
                        double.TryParse(s4bFormControl.fieldValue.Trim(), out resultDouble);
                        p11Qty[index - 1] = resultDouble;
                        break;
                     case "FIELD_PG11_ROW01_CONDITION":
                     case "FIELD_PG11_ROW02_CONDITION":
                     case "FIELD_PG11_ROW03_CONDITION":
                     case "FIELD_PG11_ROW04_CONDITION":
                     case "FIELD_PG11_ROW05_CONDITION":
                     case "FIELD_PG11_ROW06_CONDITION":
                     case "FIELD_PG11_ROW07_CONDITION":
                     case "FIELD_PG11_ROW08_CONDITION":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(14, 2), out index);
                        p11Condition[index - 1] = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG11_ROW01_COMMENTS":
                     case "FIELD_PG11_ROW02_COMMENTS":
                     case "FIELD_PG11_ROW03_COMMENTS":
                     case "FIELD_PG11_ROW04_COMMENTS":
                     case "FIELD_PG11_ROW05_COMMENTS":
                     case "FIELD_PG11_ROW06_COMMENTS":
                     case "FIELD_PG11_ROW07_COMMENTS":
                     case "FIELD_PG11_ROW08_COMMENTS":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(14, 2), out index);
                        p11Comments[index - 1] = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG8_ROW01_OBSERVATIONS": //Issue in json. These shuold be PG12
                     case "FIELD_PG8_ROW02_OBSERVATIONS":
                     case "FIELD_PG8_ROW03_OBSERVATIONS":
                     case "FIELD_PG8_ROW04_OBSERVATIONS":
                     case "FIELD_PG8_ROW05_OBSERVATIONS":
                     case "FIELD_PG8_ROW06_OBSERVATIONS":
                     case "FIELD_PG8_ROW07_OBSERVATIONS":
                     case "FIELD_PG8_ROW08_OBSERVATIONS":
                     case "FIELD_PG8_ROW09_OBSERVATIONS":
                     case "FIELD_PG8_ROW10_OBSERVATIONS":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        p12Observations[index - 1] = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG8_RECOMMENDATIONS":
                        p12ConclusionsAndRecomm = s4bFormControl.fieldValue.Trim();
                        break;
                     case "FIELD_PG8_SIGNATURE":
                     case "CAM_PG2_CAMERA_01":
                     case "CAM_PG4_CAMERA_02":
                     case "CAM_PG3_CAMERA_01B":
                     case "CAM_PG3_CAMERA_01C":
                     case "CAM_PG4_CAMERA_03":
                     case "CAM_PG4_CAMERA_04":
                     case "CAM_PG4_CAMERA_05":
                     case "CAM_PG5_CAMERA_06":
                     case "CAM_PG5_CAMERA_07":
                     case "CAM_PG5_CAMERA_08":
                     case "CAM_PG5_CAMERA_09":
                     case "CAM_PG6_CAMERA_10":
                     case "CAM_PG6_CAMERA_11":
                     case "CAM_PG6_CAMERA_12":
                     case "CAM_PG6_CAMERA_13":
                     case "CAM_PG7_CAMERA_14":
                     case "CAM_PG7_CAMERA_15":
                     case "CAM_PG7_CAMERA_16":
                     case "CAM_PG7_CAMERA_17":
                     case "CAM_PG8_CAMERA_18":
                     case "CAM_PG8_CAMERA_19":
                     case "CAM_PG8_CAMERA_20":
                     case "CAM_PG8_CAMERA_21":
                     case "CAM_PG10_CAMERA_22":
                     case "CAM_PG10_CAMERA_23":
                        imageValues.Add(s4bFormControlEntry.Key, s4bFormControl.fieldValue.Trim());
                        break;
                  }
               }
            }
            //Check and Create Job Ref
            if (!string.IsNullOrEmpty(jobRef))
            {
               jobRef = jobRef.ToUpper();
               OrdersRepository orderRepostory = new OrdersRepository(null);
               Orders order = orderRepostory.CreateOrderByJobRef(jobRef, false, request, null);
            }
            // Insert Header Record
            // Prepare Data for Insertion
            SubmissionsDataFh siteInspection = new SubmissionsDataFh();
            siteInspection.P1ContractNo = jobRef;
            siteInspection.SubmitNo = s4bFormSubmission.S4bSubmitNo;
            siteInspection.SubmitTs = s4bFormSubmission.S4bSubmitTs;
            siteInspection.DateSubmit = s4bFormSubmission.DateSubmit;
            siteInspection.FileCabId = s4bFormSubmission.FileCabId;
            siteInspection.FlgAmended = false;
            siteInspection.FlgResubmission = false;
            siteInspection.CreatedPDFCount = 1;
            siteInspection.P1ContractNo = jobRef;
            siteInspection.P1SiteAddress = p1SiteAddress;
            siteInspection.P1ClientName = p1ClientName;
            siteInspection.P1TankType = p1TankType;
            siteInspection.P1TankSize = p1TankSize;
            siteInspection.P1ReportDate = p1ReportDate;
            siteInspection.P1InspectedBy = p1InspectedBy;
            siteInspection.P2Location = p2Location;
            siteInspection.P2InstallationDate = p2InstallationDate;
            siteInspection.P2ContactName = p2ContactName;
            siteInspection.P2ContactTelNo = p2ContactTelNo;
            siteInspection.P2ContacText = p2ContactExt;
            siteInspection.P2ContactMobile = p2ContactMobile;
            siteInspection.P2LContacts = p2Lcontacts;
            siteInspection.P2VisitStatus = p2VisitStatus;
            siteInspection.P2ManufacturerDetails = p2ManufacturerDetails;
            siteInspection.P2InstallationDateUnknown = p2InstallationDateUnknown;
            siteInspection.P2VisitPurpose = p2VisitPurpose;
            siteInspection.P2VisitPurpose2 = "";
            siteInspection.P3TankStatus = p3TankStatus;
            siteInspection.P3TankDiameter = p3TankDiameter;
            siteInspection.P3TankShape = p3TankShape;
            siteInspection.P3Tanksize = p3TankSize;
            siteInspection.P3TankManufacturer = p3TankManufacturer;
            siteInspection.P3PanelDimensions = p3PanelDimensions;
            siteInspection.P3TankType = p3TankType;
            siteInspection.P3ToppanelDimensions = p3TopPanelDimensions;
            siteInspection.P3TankInstallationDate = p3TankInstallationDate;
            siteInspection.P3Hozboltseam = p3HozBoltSeam;
            siteInspection.P3TankHeight = p3TankHeight;
            siteInspection.P3ActualCapacity = p3ActualCapacity;
            siteInspection.P3TankDetails = p3TankDetails;
            siteInspection.P3TankShellDetails = p3TankShellDetails;
            siteInspection.P3TankShellDetails2 = p3TankShellDetails2;
            siteInspection.P4RoofTankShellDetails = p4RoofTankShellDetails;
            siteInspection.P4TestReturn = p4TestReturn;
            siteInspection.P4DrainValve = p5DrainValve;
            siteInspection.P4Suction = p5Suction;
            siteInspection.P4OverFlows = p6OverFlows;
            siteInspection.P4InletValve = p6InletValve;
            siteInspection.P4ImmersionHeater = p7ImmersionHeater;
            siteInspection.P4LowLevelManWay = p7LowLevelManway;
            siteInspection.P6AncillaryItems = p10AncillaryItems;
            siteInspection.P6ExternalLadder = p10ExternalLadder;
            siteInspection.P6InletValveHousing = p10InletValveHousing;
            siteInspection.P7Observations01 = p12Observations[0];
            siteInspection.P7Observations02 = p12Observations[1];
            siteInspection.P7Observations03 = p12Observations[2];
            siteInspection.P7Observations04 = p12Observations[3];
            siteInspection.P7Observations05 = p12Observations[4];
            siteInspection.P7Observations06 = p12Observations[5];
            siteInspection.P7Observations07 = p12Observations[6];
            siteInspection.P7Observations08 = p12Observations[7];
            siteInspection.P7Observations09 = p12Observations[8];
            siteInspection.P7Observations10 = p12Observations[9];
            siteInspection.P7Conclusions = p12ConclusionsAndRecomm;
            siteInspection.P7SignatureImageFile = p12SignatureImageFile;
            siteInspection.P8LevelSwitches = p8LevelSwitches;
            siteInspection.P8ContentsGauge = P8ContentsGauge;
            siteInspection.CreatedBy = Int32.Parse(request.Headers["UserId"].ToString());
            siteInspection.DateCreated = siteInspection.DateSubmit;
            for (int rowNo = 0; rowNo < P9_NO_ROWS; rowNo++)
            {
               if (p9Qty[rowNo] != 0 || !string.IsNullOrEmpty(p9Size[rowNo]) ||
                   !string.IsNullOrEmpty(p9Comments[rowNo]) || !string.IsNullOrEmpty(p9Condition[rowNo]))
               {
                  SubmissionsDataFhi tankConnection = new SubmissionsDataFhi();
                  tankConnection.PageNo = 9;
                  tankConnection.RowNo = rowNo + 1;
                  tankConnection.RowSize = p9Size[rowNo];
                  tankConnection.RowQty = p9Qty[rowNo];
                  tankConnection.RowCondition = p9Condition[rowNo];
                  tankConnection.RowComments = p9Comments[rowNo];
                  tankConnection.CreatedBy = Int32.Parse(request.Headers["UserId"]);
                  tankConnection.DateCreated = DateTime.Now;
                  tankConnection.LastAmendedBy = -1;
                  tankConnection.DateLastAmended = DateTime.Now;
                  if (siteInspection.TankConnections == null)
                  {
                     siteInspection.TankConnections = new List<SubmissionsDataFhi>();
                  }
                  siteInspection.TankConnections.Add(tankConnection);
               }
            }
            for (int rowNo = 0; rowNo < P11_NO_ROWS; rowNo++)
            {
               if (p11Qty[rowNo] != 0 ||
                   !string.IsNullOrEmpty(p11Comments[rowNo]) || !string.IsNullOrEmpty(p11Condition[rowNo]))
               {
                  SubmissionsDataFhi ancillaryItem = new SubmissionsDataFhi();
                  ancillaryItem.PageNo = 11;
                  ancillaryItem.RowNo = rowNo + 1;
                  ancillaryItem.RowSize = "";
                  ancillaryItem.RowQty = p11Qty[rowNo];
                  ancillaryItem.RowCondition = p11Condition[rowNo];
                  ancillaryItem.RowComments = p11Comments[rowNo];
                  ancillaryItem.CreatedBy = Int32.Parse(request.Headers["UserId"]);
                  ancillaryItem.DateCreated = DateTime.Now;
                  ancillaryItem.LastAmendedBy = -1;
                  ancillaryItem.DateLastAmended = DateTime.Now;
                  if (siteInspection.AncillaryItems == null)
                  {
                     siteInspection.AncillaryItems = new List<SubmissionsDataFhi>();
                  }
                  siteInspection.AncillaryItems.Add(ancillaryItem);
               }
            }
            int pageNo;
            foreach (KeyValuePair<string, string> imageData in imageValues)
            {
               if (!string.IsNullOrEmpty(imageData.Value))
               {
                  string[] splittedImageIds = imageData.Key.Split('_');
                  if (splittedImageIds.Length > 1)
                  {
                     if (siteInspection.SiteInspectionImages == null)
                     {
                        siteInspection.SiteInspectionImages = new List<SubmissionsImagesFh>();
                     }
                     pageNo = 1;
                     int.TryParse(splittedImageIds[1].Replace("PG", ""), out pageNo);
                     SubmissionsImagesFh submissionImageFh = new SubmissionsImagesFh();
                     submissionImageFh.FixedImage = true;
                     submissionImageFh.PageNo = pageNo;
                     submissionImageFh.FieldId = imageData.Key;
                     submissionImageFh.FileDisplayName = "";
                     submissionImageFh.FilePath = imageData.Value;
                     submissionImageFh.CreatedBy = Int32.Parse(request.Headers["UserId"]);
                     submissionImageFh.DateCreated = DateTime.Now;
                     submissionImageFh.CreatedBy = -1;
                     submissionImageFh.DateLastAmended = DateTime.Now;
                     siteInspection.SiteInspectionImages.Add(submissionImageFh);
                  }
               }
            }
            SiteInspectionRepository siteInspectionRepository = new SiteInspectionRepository();
            if (siteInspectionRepository.Insert(siteInspection, request, null))
            {
               returnValue = true;
            }
            else
            {
               Message = siteInspectionRepository.Message;
            }

         }
         catch (Exception ex)
         {
            Message = "Unable to Process Franklin Hodge Site Inspection. Exception: " + ex.Message;
         }
         return returnValue;
      }

      // CEE Water Sampling Report V04 "357000015" 
      private bool ProcessCEEWaterSampling(HttpRequest request, S4BFormRequest s4bFormRequest, S4bFormSubmissions s4bFormSubmission,
                                           RootObject root, Dictionary<string, S4BFormsControl> s4bFormsControls)
      {
         bool returnValue = false;
         int jobAddressId = -1, jobClientId = -1, userId = -1, assignedTo = -1, appSequence = -1;
         long jobSequence = -1;
         string userName = "", jobRef = "";
         const int TOTAL_NO_ROWS = 10;
         string[] sampleDesc = new string[TOTAL_NO_ROWS];
         string[] refNo = new string[TOTAL_NO_ROWS];
         DateTime[] sampleDate = new DateTime[TOTAL_NO_ROWS];
         DateTime? timeStamp = DateTime.Now;
         bool flgNonCompliant = false;
         int index = -1;
         Dictionary<string, string> imageValues = new Dictionary<string, string>();
         DateTime? p1ReportDate = DateTime.MinValue, p2InstallationDate = DateTime.MinValue, p3TankInstallationDate = DateTime.MinValue;
         try
         {
            foreach (KeyValuePair<string, S4BFormsControl> s4bFormControlEntry in s4bFormsControls)
            {
               S4BFormsControl s4bFormControl = s4bFormControlEntry.Value;
               if (s4bFormControl.fieldValue != null)
               {
                  switch (s4bFormControlEntry.Key)
                  {
                     case "VARIABLE_PG1_JOB_SEQUENCE":
                        if (long.TryParse(s4bFormControl.fieldValue.Trim(), out jobSequence))
                        {
                        };
                        break;
                     case "VARIABLE_PG1_JOB_CLIENT_ID":
                        if (int.TryParse(s4bFormControl.fieldValue.Trim(), out jobClientId))
                        {
                        };
                        break;
                     case "VARIABLE_PG1_JOB_ADDRESS_ID":
                        if (int.TryParse(s4bFormControl.fieldValue.Trim(), out jobAddressId))
                        {
                        };
                        break;
                     case "VARIABLE_PG1_USER_ID":
                        if (int.TryParse(s4bFormControl.fieldValue.Trim(), out userId))
                        {
                        };
                        break;

                     case "VARIABLE_PG1_DIARY_RESOURCE":
                        userName = s4bFormControl.fieldValue.Trim();
                        break;

                     case "VARIABLE_PG1_DIARY_ENTRY_ID":
                        int.TryParse(s4bFormControl.fieldValue.Trim(), out appSequence);
                        break;

                     case "FIELD_PG1_JOB_REF":
                        jobRef = s4bFormControl.fieldValue.Trim();
                        break;

                     case "FIELD_PG1_ROW01_SAMPLE_DESCRIPTION":
                     case "FIELD_PG1_ROW02_SAMPLE_DESCRIPTION":
                     case "FIELD_PG1_ROW03_SAMPLE_DESCRIPTION":
                     case "FIELD_PG1_ROW04_SAMPLE_DESCRIPTION":
                     case "FIELD_PG1_ROW05_SAMPLE_DESCRIPTION":
                     case "FIELD_PG1_ROW06_SAMPLE_DESCRIPTION":
                     case "FIELD_PG1_ROW07_SAMPLE_DESCRIPTION":
                     case "FIELD_PG1_ROW08_SAMPLE_DESCRIPTION":
                     case "FIELD_PG1_ROW09_SAMPLE_DESCRIPTION":
                     case "FIELD_PG1_ROW10_SAMPLE_DESCRIPTION":
                        index = -1;
                        int.TryParse(s4bFormControl.fieldName.Substring(13, 2), out index);
                        if (index > 0)
                        {
                           sampleDesc[index - 1] = s4bFormControl.fieldValue.Trim();
                        }
                        break;

                     case "FIELD_PG1_ROW01_REFERENCE_NO":
                     case "FIELD_PG1_ROW02_REFERENCE_NO":
                     case "FIELD_PG1_ROW03_REFERENCE_NO":
                     case "FIELD_PG1_ROW04_REFERENCE_NO":
                     case "FIELD_PG1_ROW05_REFERENCE_NO":
                     case "FIELD_PG1_ROW06_REFERENCE_NO":
                     case "FIELD_PG1_ROW07_REFERENCE_NO":
                     case "FIELD_PG1_ROW08_REFERENCE_NO":
                     case "FIELD_PG1_ROW09_REFERENCE_NO":
                     case "FIELD_PG1_ROW10_REFERENCE_NO":
                        index = -1;
                        int.TryParse(s4bFormControl.fieldName.Substring(13, 2), out index);
                        if (index > 0)
                        {
                           refNo[index - 1] = s4bFormControl.fieldValue.Trim();
                        }
                        break;

                     case "FIELD_PG1_ROW01_SAMPLE_DATE":
                     case "FIELD_PG1_ROW02_SAMPLE_DATE":
                     case "FIELD_PG1_ROW03_SAMPLE_DATE":
                     case "FIELD_PG1_ROW04_SAMPLE_DATE":
                     case "FIELD_PG1_ROW05_SAMPLE_DATE":
                     case "FIELD_PG1_ROW06_SAMPLE_DATE":
                     case "FIELD_PG1_ROW07_SAMPLE_DATE":
                     case "FIELD_PG1_ROW08_SAMPLE_DATE":
                     case "FIELD_PG1_ROW09_SAMPLE_DATE":
                     case "FIELD_PG1_ROW10_SAMPLE_DATE":
                        index = -1;
                        int.TryParse(s4bFormControl.fieldName.Substring(13, 2), out index);
                        if (index > 0)
                        {
                           DateTime result = DateTime.MinValue;
                           DateTime.TryParse(s4bFormControl.fieldValue.Trim(), out result);
                           sampleDate[index - 1] = result;
                        }
                        break;

                     case "FIELD_PG2_NON_COMPLIANT":
                        bool.TryParse(s4bFormControl.fieldValue.Trim(), out flgNonCompliant);
                        break;

                  }
               }
            }
            if (jobSequence <= 0)
            {
               if (jobRef != "")
               {
                  OrdersRepository orderRepos = new OrdersRepository(null);
                  Orders order = orderRepos.GetOrderByJobRef(jobRef, request);
               }
            }

            /// Water Sampling Record Data For Insert
            /// 
            for (int counter = 0; counter < TOTAL_NO_ROWS; counter++)
            {
               if ((sampleDesc[counter] != null && sampleDesc[counter].Trim() != "") ||
                   (refNo[counter] != null && refNo[counter].Trim() != "") ||
                   (sampleDate[counter] != null && sampleDate[counter] != DateTime.MinValue))
               {
                  EformsOrdCeeWsrRepository eformsOrdCeeWsrRepository = new EformsOrdCeeWsrRepository();
                  EformsOrdCeeWsr eformsOrdCeeWsr = new EformsOrdCeeWsr();
                  eformsOrdCeeWsr.JobSequence = jobSequence;
                  eformsOrdCeeWsr.FormId = s4bFormSubmission.RefNatForms.FormId;
                  eformsOrdCeeWsr.FormSubmissionId = s4bFormSubmission.S4bSubmitNo;
                  eformsOrdCeeWsr.FormTimeStamp = ((DateTime)timeStamp).ToString("dd/MM/yyyy HH:mm:ss");
                  eformsOrdCeeWsr.RowNo = counter + 1;
                  eformsOrdCeeWsr.RowDesc = sampleDesc[counter];
                  eformsOrdCeeWsr.RowRefNo = refNo[counter];
                  eformsOrdCeeWsr.DateRowSampleDate = sampleDate[counter];
                  eformsOrdCeeWsr.CreatedBy = userId;
                  eformsOrdCeeWsr.DateCreated = DateTime.Now;
                  eformsOrdCeeWsr.LastAmendedBy = -1;
                  eformsOrdCeeWsr.DateLastAmended = DateTime.Now;
                  eformsOrdCeeWsr = eformsOrdCeeWsrRepository.Insert(eformsOrdCeeWsr, request);
               }
            }
            returnValue = true;
         }
         catch (Exception ex)
         {
            Message = "Unable to Process Water Sampling V04 for CEE. Exception: " + ex.Message;
         }
         return returnValue;
      }

      // CEE Technical Report V04 "357000014" 
      private bool ProcessCEETechnicalReport(HttpRequest request, S4BFormRequest s4bFormRequest, S4bFormSubmissions s4bFormSubmission,
                                             RootObject root, Dictionary<string, S4BFormsControl> s4bFormsControls)
      {
         bool returnValue = false;
         int jobAddressId = -1, jobClientId = -1, userId = -1, assignedTo = -1, appSequence = -1;
         long jobSequence = -1;
         string fieldName = "", userName = "", jobRef = "", workCompleted = "", comments = "";
         bool flgNonCompliant = false;
         DateTime? datRegistered = DateTime.MinValue;
         string nonConformance = "";
         string personResponsible = "";
         DateTime? timeStamp = s4bFormSubmission.DateCreated;
         int index = -1;
         Dictionary<string, string> imageValues = new Dictionary<string, string>();
         DateTime? p1ReportDate = DateTime.MinValue, p2InstallationDate = DateTime.MinValue, p3TankInstallationDate = DateTime.MinValue;
         try
         {
            foreach (KeyValuePair<string, S4BFormsControl> s4bFormControlEntry in s4bFormsControls)
            {
               S4BFormsControl s4bFormControl = s4bFormControlEntry.Value;
               if (s4bFormControl.fieldValue != null)
               {
                  switch (s4bFormControlEntry.Key)
                  {
                     case "VARIABLE_PG1_JOB_SEQUENCE":
                        if (long.TryParse(s4bFormControl.fieldValue.Trim(), out jobSequence))
                        {
                        };
                        break;
                     case "VARIABLE_PG1_JOB_CLIENT_ID":
                        if (int.TryParse(s4bFormControl.fieldValue.Trim(), out jobClientId))
                        {
                        };
                        break;
                     case "VARIABLE_PG1_JOB_ADDRESS_ID":
                        if (int.TryParse(s4bFormControl.fieldValue.Trim(), out jobAddressId))
                        {
                        };
                        break;
                     case "VARIABLE_PG1_USER_ID":
                        if (int.TryParse(s4bFormControl.fieldValue.Trim(), out userId))
                        {
                        };
                        break;

                     case "VARIABLE_PG1_DIARY_RESOURCE":
                        userName = s4bFormControl.fieldValue.Trim();
                        break;

                     case "VARIABLE_PG1_DIARY_ENTRY_ID":
                        int.TryParse(s4bFormControl.fieldValue.Trim(), out appSequence);
                        break;

                     case "FIELD_PG1_JOB_REF":
                        jobRef = s4bFormControl.fieldValue.Trim();
                        break;

                     case "FIELD_PG1_WORK_COMPLETED":
                        workCompleted = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG1_COMMENTS":
                        comments = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG2_NON_COMPLIANT":
                        bool.TryParse(s4bFormControl.fieldValue, out flgNonCompliant);
                        break;

                     case "FIELD_PG2_DATE_REGISTERED":
                        DateTime result = DateTime.MinValue;
                        DateTime.TryParse(s4bFormControl.fieldValue, out result);
                        datRegistered = result;
                        break;
                     case "FIELD_PG2_PERSON_RESPONSIBLE":
                        personResponsible = s4bFormControl.fieldValue;
                        break;
                     case "FIELD_PG2_NON_CONFORMANCE":
                        nonConformance = s4bFormControl.fieldValue;
                        break;

                  }
               }
            }
            if (jobSequence <= 0)
            {
               if (jobRef != "")
               {
                  OrdersRepository orderRepos = new OrdersRepository(null);
                  Orders order = orderRepos.GetOrderByJobRef(jobRef, request);
               }
            }

            /// Technical Report Data For Insert
            ///
            OrderItemsRepository orderItemsRepository = new OrderItemsRepository();
            if ((workCompleted != null && workCompleted.Trim() != ""))
            {
               string itemDesc = "TECHNICAL REPORT – WORK COMPLETED – " + workCompleted;
               OrderItems oi = new OrderItems();
               oi.JobSequence = jobSequence;
               oi.FlgRowIsText = true;
               oi.ItemType = 0;
               oi.ItemDesc = itemDesc;
               oi.ItemUnits = "";
               oi.ItemQuantity = 0;
               oi.AmountLabour = 0;
               oi.AmountMaterials = 0;
               oi.AmountPlant = 0;
               oi.AmountValue = 0;
               oi.AmountTotal = 0;
               oi.AssignedTo = assignedTo;
               oi.FlgCompleted = true;
               oi.FlgDocsRecd = true;
               oi.CreatedBy = userId;
               oi.DateCreated = timeStamp;
               oi.LastAmendedBy = userId;
               oi.DateLastAmended = timeStamp;
               OrderItems oiUpdated = orderItemsRepository.CreateOrderItems(oi, request);
            }
            if (comments != null && comments.Trim() != "")
            {
               string itemDesc = "TECHNICAL REPORT – COMMENTS – " + comments;
               OrderItems oi = new OrderItems();
               oi.JobSequence = jobSequence;
               oi.FlgRowIsText = true;
               oi.ItemType = 0;
               oi.ItemDesc = itemDesc;
               oi.ItemUnits = "";
               oi.ItemQuantity = 0;
               oi.AmountLabour = 0;
               oi.AmountMaterials = 0;
               oi.AmountPlant = 0;
               oi.AmountValue = 0;
               oi.AmountTotal = 0;
               oi.AssignedTo = assignedTo;
               oi.FlgCompleted = true;
               oi.FlgDocsRecd = true;
               oi.CreatedBy = userId;
               oi.DateCreated = timeStamp;
               oi.LastAmendedBy = userId;
               oi.DateLastAmended = timeStamp;
               OrderItems oiUpdated = orderItemsRepository.CreateOrderItems(oi, request);
            }
            if (flgNonCompliant && !string.IsNullOrEmpty(s4bFormSubmission.S4bSubmitNo))
            {
               S4BSubmissionsDataH nFSubmissionsDataH = new S4BSubmissionsDataH();
               nFSubmissionsDataH.JoinSequence = s4bFormSubmission.Sequence;
               nFSubmissionsDataH.FlgYesOrNo1 = true;
               nFSubmissionsDataH.FlgYesOrNo2 = false;
               nFSubmissionsDataH.DateUser1 = datRegistered;
               nFSubmissionsDataH.DateUser2 = DateTime.MinValue;
               nFSubmissionsDataH.UserAmt1 = 0;
               nFSubmissionsDataH.UserAmt2 = 0;
               nFSubmissionsDataH.UserQty1 = 0;
               nFSubmissionsDataH.UserQty2 = 0;
               nFSubmissionsDataH.UserText1 = personResponsible;
               nFSubmissionsDataH.UserText2 = "";
               nFSubmissionsDataH.UserMemo1 = nonConformance;
               nFSubmissionsDataH.UserMemo2 = "";
               nFSubmissionsDataH.CreatedBy = userId;
               nFSubmissionsDataH.DateCreated = DateTime.Now;
               nFSubmissionsDataH.LastAmendedBy = -1;
               nFSubmissionsDataH.DateLastAmended = DateTime.Now;
               S4BSubmissionsDataHRepository s4BSubmissionsDataHRepository = new S4BSubmissionsDataHRepository();
               nFSubmissionsDataH = s4BSubmissionsDataHRepository.Insert(nFSubmissionsDataH, request);
            }
            returnValue = true;
         }
         catch (Exception ex)
         {
            Message = "Unable to Process Water Sampling V04 for CEE. Exception: " + ex.Message;
         }
         return returnValue;
      }

      // CEE Disinfection Report V04 "357000013" 
      private bool ProcessCEEDisinfectionReport(HttpRequest request, S4BFormRequest s4bFormRequest, S4bFormSubmissions s4bFormSubmission,
                                             RootObject root, Dictionary<string, S4BFormsControl> s4bFormsControls)
      {
         bool returnValue = false;
         int jobAddressId = -1, jobClientId = -1, userId = -1, assignedTo = -1, appSequence = -1;
         long jobSequence = -1;
         string fieldName = "", userName = "", jobRef = "", workCompleted = "", comments = "";
         bool flgNonCompliant = false;
         DateTime? datRegistered = DateTime.MinValue;
         string nonConformance = "";
         string personResponsible = "";
         DateTime? timeStamp = s4bFormSubmission.DateCreated;
         int index = -1;
         Dictionary<string, string> imageValues = new Dictionary<string, string>();
         DateTime? p1ReportDate = DateTime.MinValue, p2InstallationDate = DateTime.MinValue, p3TankInstallationDate = DateTime.MinValue;
         try
         {
            foreach (KeyValuePair<string, S4BFormsControl> s4bFormControlEntry in s4bFormsControls)
            {
               S4BFormsControl s4bFormControl = s4bFormControlEntry.Value;
               if (s4bFormControl.fieldValue != null)
               {
                  switch (s4bFormControlEntry.Key)
                  {
                     case "FIELD_PG2_NON_COMPLIANT":
                        bool.TryParse(s4bFormControl.fieldValue, out flgNonCompliant);
                        break;

                     case "FIELD_PG2_DATE_REGISTERED":
                        DateTime result = DateTime.MinValue;
                        DateTime.TryParse(s4bFormControl.fieldValue, out result);
                        datRegistered = result;
                        break;
                     case "FIELD_PG2_PERSON_RESPONSIBLE":
                        personResponsible = s4bFormControl.fieldValue;
                        break;
                     case "FIELD_PG2_NON_CONFORMANCE":
                        nonConformance = s4bFormControl.fieldValue;
                        break;
                  }
               }
            }
            if (flgNonCompliant && !string.IsNullOrEmpty(s4bFormSubmission.S4bSubmitNo))
            {
               S4BSubmissionsDataH nFSubmissionsDataH = new S4BSubmissionsDataH();
               nFSubmissionsDataH.JoinSequence = s4bFormSubmission.Sequence;
               nFSubmissionsDataH.FlgYesOrNo1 = true;
               nFSubmissionsDataH.FlgYesOrNo2 = false;
               nFSubmissionsDataH.DateUser1 = datRegistered;
               nFSubmissionsDataH.DateUser2 = DateTime.MinValue;
               nFSubmissionsDataH.UserAmt1 = 0;
               nFSubmissionsDataH.UserAmt2 = 0;
               nFSubmissionsDataH.UserQty1 = 0;
               nFSubmissionsDataH.UserQty2 = 0;
               nFSubmissionsDataH.UserText1 = personResponsible;
               nFSubmissionsDataH.UserText2 = "";
               nFSubmissionsDataH.UserMemo1 = nonConformance;
               nFSubmissionsDataH.UserMemo2 = "";
               nFSubmissionsDataH.CreatedBy = userId;
               nFSubmissionsDataH.DateCreated = DateTime.Now;
               nFSubmissionsDataH.LastAmendedBy = -1;
               nFSubmissionsDataH.DateLastAmended = DateTime.Now;
               S4BSubmissionsDataHRepository s4BSubmissionsDataHRepository = new S4BSubmissionsDataHRepository();
               nFSubmissionsDataH = s4BSubmissionsDataHRepository.Insert(nFSubmissionsDataH, request);
            }
            returnValue = true;
         }
         catch (Exception ex)
         {
            Message = "Unable to Process Water Sampling V04 for CEE. Exception: " + ex.Message;
         }
         return returnValue;
      }

      // CEE Chemical Analysis Report V04 "357000012" 
      private bool ProcessCEEChemicalAnalysis(HttpRequest request, S4BFormRequest s4bFormRequest, S4bFormSubmissions s4bFormSubmission,
                                              RootObject root, Dictionary<string, S4BFormsControl> s4bFormsControls)
      {
         bool returnValue = false;
         bool flgNonCompliant = false;
         DateTime? datRegistered = DateTime.MinValue;
         long userId = long.Parse(request.Headers["UserId"].ToString());
         string nonConformance = "";
         string personResponsible = "";
         DateTime? timeStamp = s4bFormSubmission.DateCreated;
         Dictionary<string, string> imageValues = new Dictionary<string, string>();
         DateTime? p1ReportDate = DateTime.MinValue, p2InstallationDate = DateTime.MinValue, p3TankInstallationDate = DateTime.MinValue;
         try
         {
            foreach (KeyValuePair<string, S4BFormsControl> s4bFormControlEntry in s4bFormsControls)
            {
               S4BFormsControl s4bFormControl = s4bFormControlEntry.Value;
               if (s4bFormControl.fieldValue != null)
               {
                  switch (s4bFormControlEntry.Key)
                  {
                     case "FIELD_PG2_NON_COMPLIANT":
                        bool.TryParse(s4bFormControl.fieldValue, out flgNonCompliant);
                        break;

                     case "FIELD_PG2_DATE_REGISTERED":
                        DateTime result = DateTime.MinValue;
                        DateTime.TryParseExact(s4bFormControl.fieldValue,
                                               new string[] { "dd/MM/yyyy", "dd-MM-yyyy" },
                                               new CultureInfo("en-GB"), DateTimeStyles.None, out result);
                        datRegistered = result;
                        break;
                     case "FIELD_PG2_PERSON_RESPONSIBLE":
                        personResponsible = s4bFormControl.fieldValue;
                        break;
                     case "FIELD_PG2_NON_CONFORMANCE":
                        nonConformance = s4bFormControl.fieldValue;
                        break;
                  }
               }
            }
            if (flgNonCompliant && !string.IsNullOrEmpty(s4bFormSubmission.S4bSubmitNo))
            {
               S4BSubmissionsDataH nFSubmissionsDataH = new S4BSubmissionsDataH();
               nFSubmissionsDataH.JoinSequence = s4bFormSubmission.Sequence;
               nFSubmissionsDataH.FlgYesOrNo1 = true;
               nFSubmissionsDataH.FlgYesOrNo2 = false;
               nFSubmissionsDataH.DateUser1 = datRegistered;
               nFSubmissionsDataH.DateUser2 = DateTime.MinValue;
               nFSubmissionsDataH.UserAmt1 = 0;
               nFSubmissionsDataH.UserAmt2 = 0;
               nFSubmissionsDataH.UserQty1 = 0;
               nFSubmissionsDataH.UserQty2 = 0;
               nFSubmissionsDataH.UserText1 = personResponsible;
               nFSubmissionsDataH.UserText2 = "";
               nFSubmissionsDataH.UserMemo1 = nonConformance;
               nFSubmissionsDataH.UserMemo2 = "";
               nFSubmissionsDataH.CreatedBy = userId;
               nFSubmissionsDataH.DateCreated = DateTime.Now;
               nFSubmissionsDataH.LastAmendedBy = -1;
               nFSubmissionsDataH.DateLastAmended = DateTime.Now;
               S4BSubmissionsDataHRepository s4BSubmissionsDataHRepository = new S4BSubmissionsDataHRepository();
               nFSubmissionsDataH = s4BSubmissionsDataHRepository.Insert(nFSubmissionsDataH, request);
            }
            returnValue = true;
         }
         catch (Exception ex)
         {
            Message = "Unable to Process Chemical Analysis for CEE. Exception: " + ex.Message;
         }
         return returnValue;
      }

      private string AddS4BFormToFilingCabinet(HttpRequest request, S4BFormRequest s4bFormRequest, S4bFormSubmissions s4bFormSubmission, Dictionary<string, S4BFormsControl> s4bFormsControls)
      {
         const string METHOD_NAME = "S4BFormsRepository.AddS4BFormToFilingCabinet()";
         string returnValue = "";
         try
         {
            ProjectSettings settings = Configs.settings[request.Headers["ProjectId"]];
            string zipFileName = Utilities.GenerateS4BeFormZipFileName(s4bFormSubmission, 1);
            string[] excludeFiles = { SimplicityConstants.S4BFormSubmittedTemplateName, SimplicityConstants.S4BFormJsonFileName };
            s4bFormSubmission.ZipFilePath = Utilities.zipFiles(s4bFormSubmission.ContentPath, s4bFormSubmission.ContentPath, zipFileName, excludeFiles,
                                                    Path.Combine(settings.TempUploadFolderPath, request.Headers["ProjectId"], s4bFormSubmission.S4bSubmitNo));
            string zipFileCabId = UploadToDrive(request, s4bFormSubmission.ZipFilePath, s4bFormSubmission.Orders.JobRef + "," + settings.FilingCabinetS4BFormsFolder, zipFileName);
            string pdfFileName = Utilities.GenerateS4BeFormPdfFileName(s4bFormSubmission, 1);
            s4bFormSubmission.PdfFilePath = Path.Combine(s4bFormSubmission.ContentPath, SimplicityConstants.S4BFormSubmittedTemplateName);
            returnValue = UploadToDrive(request, s4bFormSubmission.PdfFilePath, s4bFormSubmission.Orders.JobRef + "," + settings.FilingCabinetS4BFormsFolder, pdfFileName);
            if (s4bFormSubmission.RefNatForms != null &&
                Utilities.IsS4BFormTemplateSecondary(s4bFormSubmission.RefNatForms.FormId) &&
                s4bFormSubmission.OrderSecondary != null)
            {
               if (settings.SecondaryS4BFormsSubmissionsExportFolder != null && settings.SecondaryS4BFormsSubmissionsExportFolder[Utilities.GetSecondaryIdByS4BFormId(s4bFormSubmission.RefNatForms.FormId)] != null)
               {
                  string sourcePdfFilePath = Path.Combine(s4bFormSubmission.ContentPath, SimplicityConstants.S4BFormSubmittedTemplateName);
                  string sourceZipFilePath = Path.Combine(s4bFormSubmission.ZipFilePath);
                  string destinationFolder = Path.Combine(settings.SecondaryS4BFormsSubmissionsExportFolder[Utilities.GetSecondaryIdByS4BFormId(s4bFormSubmission.RefNatForms.FormId)], s4bFormSubmission.OrderSecondary.JobRef, settings.FilingCabinetS4BFormsFolder);
                  string destinationPdfFilePath = Path.Combine(destinationFolder, pdfFileName);
                  string destinationZipFilePath = Path.Combine(destinationFolder, zipFileName);
                  string doneFilePath = Path.Combine(destinationFolder, SimplicityConstants.DONE_FILE_NAME);
                  if (!Directory.Exists(destinationFolder))
                  {
                     Directory.CreateDirectory(destinationFolder);
                  }
                  File.Copy(sourcePdfFilePath, destinationPdfFilePath, true);
                  File.Copy(sourceZipFilePath, destinationZipFilePath, true);
                  using (var fs = File.Create(doneFilePath))
                  {
                     fs.Close();
                  }
               }
            }
         }
         catch (Exception ex)
         {
            Utilities.GenerateAndLogMessage(METHOD_NAME, "Error Occured While Adding AddS4BFormToFilingCabinet.", ex);
         }
         return returnValue;
      }

      public static string UploadToDrive(HttpRequest request, string filePath, string parentFolderName, string imageName)
      {
         string returnValue = "";

         Cld_Ord_Labels_Files oiFireProtectionIImages = new Cld_Ord_Labels_Files();
         oiFireProtectionIImages.FlgIsBase64Img = true;
         oiFireProtectionIImages.Base64Img = Convert.ToBase64String(File.ReadAllBytes(filePath));
         oiFireProtectionIImages.ImageName = imageName;

         DriveRequest driveRequest = new DriveRequest
         {
            Name = oiFireProtectionIImages.ImageName,
            ParentFolderNames = parentFolderName,
            FireProtectionImages = oiFireProtectionIImages
         };

         AttachmentFilesFolderRepository attachmentFileRepos = new AttachmentFilesFolderRepository();
         AttachmentFiles file = attachmentFileRepos.AddFileInSpecificFolder(driveRequest, request, null);
         if (file != null)
         {
            returnValue = file.Id;
         }
         return returnValue;
      }

      public Dictionary<string, S4BFormsControl> LoadS4BFormsControls(RootObject root)
      {
         Dictionary<string, S4BFormsControl> _s4BFormsControlMap = null;
         try
         {
            if (root != null && root.view != null && root.view.pages != null && root.view.pages.Count > 0)
            {
               _s4BFormsControlMap = new Dictionary<string, S4BFormsControl>(StringComparer.InvariantCultureIgnoreCase);
               foreach (Page page in root.view.pages)
               {
                  if (page.controls != null && page.controls.Count > 0)
                  {
                     foreach (S4BFormsControl control in page.controls)
                     {
                        control.pageNo = page.pageNum;
                        if (control != null && !_s4BFormsControlMap.ContainsKey(control.fieldName.ToUpper()))
                        {
                           _s4BFormsControlMap.Add(control.fieldName.ToUpper(), control);
                        }
                     }
                  }
               }
            }
         }
         catch (Exception ex)
         {
            Logger.LogError(ex.Message);
         }
         return _s4BFormsControlMap;
      }

      public Dictionary<int, Page> LoadS4BPages(RootObject root)
      {
         Dictionary<int, Page> _s4BFormPagesMap = null;
         try
         {
            if (root != null && root.view != null && root.view.pages != null && root.view.pages.Count > 0)
            {
               _s4BFormPagesMap = new Dictionary<int, Page>();
               foreach (Page page in root.view.pages)
               {
                  if (page != null && !_s4BFormPagesMap.ContainsKey(page.pageNum))
                  {
                     _s4BFormPagesMap.Add(page.pageNum, page);
                  }
               }
            }
         }
         catch (Exception ex)
         {
            Logger.LogError(ex.Message);
         }
         return _s4BFormPagesMap;
      }

      private string extractSubmittedZipForm(IFormFile submittedZipForm, ProjectSettings settings, HttpRequest Request,
                                             S4bFormSubmissions s4bFormSubmission, S4BFormRequest s4bFormRequest)
      {
         string projectIdTempPath = Path.Combine(settings.TempUploadFolderPath, Request.Headers["ProjectId"]);
         string zipFileName = s4bFormSubmission.S4bSubmitNo + ".zip";
         string zipFileFullPath = Path.Combine(projectIdTempPath, zipFileName);
         if (!Directory.Exists(projectIdTempPath))
         {
            Directory.CreateDirectory(projectIdTempPath);
         }
            FileStream objFileStream = new FileInfo(zipFileFullPath).Create(); // File.Open(zipFileFullPath, FileMode.Open, FileAccess.ReadWrite);
            submittedZipForm.CopyTo(objFileStream);
            objFileStream.Close();
            objFileStream.Dispose();
            //submittedZipForm.SaveAs(zipFileFullPath);
         string zipFileExtractedPath = Path.Combine(settings.S4BFormsSubmissionRootFolderPath, s4bFormSubmission.S4bSubmitNo);
         if (Directory.Exists(zipFileExtractedPath))
         {
            Directory.Delete(zipFileExtractedPath, true);
         }
         Directory.CreateDirectory(zipFileExtractedPath);
         System.IO.Compression.ZipFile.ExtractToDirectory(zipFileFullPath, zipFileExtractedPath);
         return zipFileExtractedPath;
      }

      private string extractSubmittedVideoZipFile(IFormFile submittedZipForm, ProjectSettings settings, HttpRequest Request,
                                            S4BFormRequest s4bFormRequest)
      {
         string projectIdTempPath = Path.Combine(settings.TempUploadFolderPath, Request.Headers["ProjectId"]);
         string zipFileName = s4bFormRequest.FormSequence + "_video.zip";
         string zipFileFullPath = Path.Combine(projectIdTempPath, zipFileName);
         if (!Directory.Exists(projectIdTempPath))
         {
            Directory.CreateDirectory(projectIdTempPath);
         }
            FileStream objFileStream = new FileInfo(zipFileFullPath).Create(); // File.Open(zipFileFullPath, FileMode.Open, FileAccess.ReadWrite);
            submittedZipForm.CopyTo(objFileStream);
            objFileStream.Close();
            objFileStream.Dispose();

            //submittedZipForm.SaveAs(zipFileFullPath);
         string zipFileExtractedPath = Path.Combine(settings.S4BFormsSubmissionRootFolderPath, s4bFormRequest.FormSequence.ToString());
         if (Directory.Exists(zipFileExtractedPath))
         {
            Directory.Delete(zipFileExtractedPath, true);
         }
         Directory.CreateDirectory(zipFileExtractedPath);
         System.IO.Compression.ZipFile.ExtractToDirectory(zipFileFullPath, zipFileExtractedPath);
         return zipFileExtractedPath;
      }

      public ResponseModel GetTemplateURL(NaturalFormRequest naturalFormRequest, HttpRequest Request)
      {
         ResponseModel response = null;
         try
         {
            response = new ResponseModel();
            response.IsSucessfull = false;

            string projectId = Request.Headers["ProjectId"];
            if (naturalFormRequest == null)
            {
               response.Message = "Invalid Request. The Request object is null";
               return response;
            }
            if (naturalFormRequest.Form == null)
            {
               response.Message = "Invalid Request. The Form object is null";
               return response;
            }
            if (!string.IsNullOrWhiteSpace(projectId))
            {
               ProjectSettings settings = Configs.settings[projectId];
               if (settings != null)
               {
                  if (naturalFormRequest.IsThirdParty)
                  {
                     DiaryAppsWebAssignDB daWebAssignDB = new DiaryAppsWebAssignDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                     string templateURL = daWebAssignDB.GetThirdPartyTemplateURL(naturalFormRequest, settings);
                     if (string.IsNullOrEmpty(templateURL))
                     {
                        response.Message = "Unable to get Template URL. Details: " + daWebAssignDB.ErrorMessage;
                     }
                     else
                     {
                        response.IsSucessfull = true;
                        response.Message = templateURL;
                     }
                  }
                  else
                  {
                     response.Message = "Request is not implemented for Non Third Party Templates.";
                     return response;
                  }
               }
               else
               {
                  response.Message = "No Appointment found for sequence. ";
               }
            }
            else
            {
               response.Message = "Unable to load Project Settings from User session.";
            }
         }
         catch (Exception ex)
         {
         }
         return response;
      }

      public S4BFormPrepopulationDataModel GetPrepopulationData(S4BFormPrepopulationDataRequest s4BFormPrepopulationDataRequest, HttpRequest request)
      {
         S4BFormPrepopulationDataModel response = null;

         ProjectSettings settings = Configs.settings[request.Headers["ProjectId"]];
         RefS4bFormsDB refS4bFormsDB = new RefS4bFormsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));

         response = new S4BFormPrepopulationDataModel();
         response.Appointments = new List<Appointment>();
         RefS4bFormsRepository refS4bFormsRepository = new RefS4bFormsRepository();
         RefS4bForms refS4bForms = null;
         string sql = string.Empty;
         Appointment rtnAppointment = null;
         Template rtnTemplate = null;
         s4BFormPrepopulationDataRequest.Appointments.ForEach(appointment =>
         {
            rtnAppointment = new Appointment();
            rtnAppointment.Sequence = appointment.Sequence;
            rtnAppointment.Templates = new List<Template>();
            //---- Add Template if no template is assigned to appointment
            if (appointment.Templates.Count == 0)
            {
               List<RefS4bForms> refS4bFormTemplate = new List<RefS4bForms>();
               refS4bFormTemplate = refS4bFormsRepository.GetFormList(request);
               refS4bFormTemplate.ForEach(refS4bForm =>
               {
                  Template t = new Template();
                  t.Sequence = refS4bForm.FormSequence;
                  appointment.Templates.Add(t);
               });
            }
            appointment.Templates.ForEach(template =>
            {
               rtnTemplate = new Template();
               rtnTemplate.Sequence = template.Sequence;
               refS4bForms = null;
               sql = string.Empty;
               refS4bForms = refS4bFormsRepository.GetRecodsById(template.Sequence ?? 0, request);
               if (refS4bForms != null)
               {
                  if (String.IsNullOrWhiteSpace(refS4bForms.PrePopulationSql) == false)
                  {
                     sql = refS4bForms.PrePopulationSql + " " + appointment.Sequence;
                     rtnTemplate.Variables = refS4bFormsDB.executePrepopulationQuery(request, sql, template.Sequence ?? 0);
                  }
               }
               rtnAppointment.Templates.Add(rtnTemplate);
            });

            response.Appointments.Add(rtnAppointment);
         });
         response.IsSucessfull = true;
         return response;
      }

      // Woodvale Maintenance Quality Control "1008617976" 
      private bool ProcessWoodvaleQualityControl(HttpRequest request, S4BFormRequest s4bFormRequest, S4bFormSubmissions s4bFormSubmission,
                                                 RootObject root, Dictionary<string, S4BFormsControl> s4bFormsControls)
      {
         bool returnValue = false;
         DateTime? datRegistered = DateTime.MinValue;
         int userId = Int32.Parse(request.Headers["UserId"].ToString());
         int jobAddressId = -1, jobClientId = -1, appSequence = -1;
         long jobSequence = -1;
         string userName = "", jobRef = "";
         const int TOTAL_NO_ROWS = 7;
         string operativeSignedInOut = "", diffOperative = "", schoolComm = "", schoolCommOpt = "", siteManagerComm = "", notes = "";
         string siteManagerCommOpt = "", submit = "", signedName = "", time = "";
         string[] ques = new string[TOTAL_NO_ROWS];
         string[] quesComment = new string[TOTAL_NO_ROWS];
         string[] drawingNotes = new string[TOTAL_NO_ROWS - 1];
         DateTime? datUser2 = DateTime.MinValue;
         int index = -1;
         DateTime? timeStamp = s4bFormSubmission.DateCreated;
         Dictionary<string, string> imageValues = new Dictionary<string, string>();
         try
         {
            foreach (KeyValuePair<string, S4BFormsControl> s4bFormControlEntry in s4bFormsControls)
            {
               S4BFormsControl s4bFormControl = s4bFormControlEntry.Value;
               if (s4bFormControl.fieldValue != null)
               {
                  switch (s4bFormControlEntry.Key)
                  {
                     case "VARIABLE_PG1_JOB_SEQUENCE":
                        if (long.TryParse(s4bFormControl.fieldValue, out jobSequence))
                        {
                        };
                        break;
                     case "VARIABLE_PG1_JOB_CLIENT_ID":
                        if (int.TryParse(s4bFormControl.fieldValue, out jobClientId))
                        {
                        };
                        break;
                     case "VARIABLE_PG1_JOB_ADDRESS_ID":
                        if (int.TryParse(s4bFormControl.fieldValue, out jobAddressId))
                        {
                        };
                        break;
                     case "VARIABLE_PG1_USER_ID":
                        if (int.TryParse(s4bFormControl.fieldValue, out userId))
                        {
                        };
                        break;

                     case "VARIABLE_PG1_DIARY_RESOURCE":
                        userName = s4bFormControl.fieldValue;
                        break;

                     case "VARIABLE_PG1_DIARY_ENTRY_ID":
                        int.TryParse(s4bFormControl.fieldValue, out appSequence);
                        break;

                     case "FIELD_PG1_JOB_REF":
                        jobRef = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG2_DATE":
                        DateTime result = DateTime.MinValue;
                        DateTime.TryParseExact(s4bFormControl.fieldValue,
                                        new string[] { "dd/MM/yyyy", "dd-MM-yyyy" },
                                        new CultureInfo("en-GB"), DateTimeStyles.None, out result);
                        datUser2 = result;
                        break;

                     case "FIELD_PG1_SIGNED_IN_OUT":
                        operativeSignedInOut = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG1_DIFFERENT_OPERATIVE":
                        diffOperative = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG2_QUESTION1_1TO10":
                     case "FIELD_PG2_QUESTION2_1TO10":
                     case "FIELD_PG2_QUESTION3_1TO10":
                     case "FIELD_PG2_QUESTION4_1TO10":
                     case "FIELD_PG2_QUESTION5_1TO10":
                     case "FIELD_PG2_QUESTION6_1TO10":
                     case "FIELD_PG2_QUESTION7_1TO10":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(18, 1), out index);
                        ques[index - 1] = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG2_QUESTION1_COMM":
                     case "FIELD_PG2_QUESTION2_COMM":
                     case "FIELD_PG2_QUESTION3_COMM":
                     case "FIELD_PG2_QUESTION4_COMM":
                     case "FIELD_PG2_QUESTION5_COMM":
                     case "FIELD_PG2_QUESTION6_COMM":
                     case "FIELD_PG2_QUESTION7_COMM":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(18, 1), out index);
                        quesComment[index - 1] = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG2_COMM_SCHOOL":
                        schoolComm = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG2_COMM_SCHOOL_OPT":
                        schoolCommOpt = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG2_COMM_SITE_MANAGER":
                        siteManagerComm = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG2_COMM_SITE_MANAGER_OPT":
                        siteManagerCommOpt = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG3_NOTES":
                        notes = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG3_DRAWING1_NOTES":
                     case "FIELD_PG3_DRAWING2_NOTES":
                     case "FIELD_PG3_DRAWING3_NOTES":
                     case "FIELD_PG3_DRAWING4_NOTES":
                     case "FIELD_PG3_DRAWING5_NOTES":
                     case "FIELD_PG3_DRAWING6_NOTES":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(17, 1), out index);
                        drawingNotes[index - 1] = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG3_SUBMIT":
                        submit = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG2_NAME":
                        signedName = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG2_TIME":
                        time = s4bFormControl.fieldValue;
                        break;
                  }
               }
            }
            if (jobSequence <= 0)
            {
               if (jobRef != "")
               {
                  OrdersRepository orderRepos = new OrdersRepository(null);
                  Orders order = orderRepos.GetOrderByJobRef(jobRef, request);
               }
            }
            if (time == "")
            {
               time = "00:00";
            }
            try
            {
               datUser2 = DateTime.ParseExact(((DateTime)datUser2).ToString("dd/MM/yyyy") + " " + time, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
            }
            OrdersRepository orderRepository = new OrdersRepository(null);
            if (!orderRepository.UpdateUserFlag2AndUserDate2ByJobSequence(request, jobSequence, true, datUser2))
            {
               //TODO: Report Error
               //Utilities.ReportError("Unable to Update Order Cancel flag for the Job Sequence " + jobSequence + " and Imp Ref '" + edwFormInstance.ImpRef + "' for Decor Rofing Quote." + Utilities.Message, method_name, true, system, edwFormInstance);
            }
            if (!orderRepository.updateFlgJobFinishAndJobDateFinishByJobSequence(request, jobSequence, true, datUser2))
            {
               //TODO: Report Error
               //Utilities.ReportError("Unable to Update Order Date Finish for the Job Sequence " + jobSequence + " and Imp Ref '" + edwFormInstance.ImpRef + "' for Decor Rofing Quote." + Utilities.Message, method_name, true, system, edwFormInstance);
            }
            /// Data Insert for Job Internal Notes
            /// 
            string formattedNotes = "Quality Control\n\n";
            formattedNotes = formattedNotes + "Different Operative: " + diffOperative + "\n\n";
            formattedNotes = formattedNotes + "Gas Site Operative Signed in/out: " + operativeSignedInOut + "\n\n";
            for (int counter = 0; counter < TOTAL_NO_ROWS; counter++)
            {
               formattedNotes = formattedNotes + (counter + 1) + ". [" + ques[counter] + "] - " + quesComment[counter] + "\n\n";
            }
            formattedNotes = formattedNotes + "Comments from School\n" + schoolComm + "\n";
            formattedNotes = formattedNotes + schoolCommOpt + "\n\n";
            formattedNotes = formattedNotes + "Comments from Site Manager\n" + siteManagerComm + "\n";
            formattedNotes = formattedNotes + siteManagerCommOpt + "\n\n";
            //formattedNotes = formattedNotes + "Signed Name: " + signedName + " Signed Date: " + signedDate + "\n\n";
            formattedNotes = formattedNotes + "Notes\n" + notes + "\n\n";
            formattedNotes = formattedNotes + "Drawings Notes\n";
            for (int counter = 0; counter < TOTAL_NO_ROWS - 1; counter++)
            {
               formattedNotes = formattedNotes + "Drawing " + (counter + 1) + ". " + drawingNotes[counter] + "\n";
            }
            OrdersNotesRepository ordersNotesRepository = new OrdersNotesRepository();
            OrdersNotes ordersNote = new OrdersNotes();
            ordersNote.JobSequence = jobSequence;
            ordersNote.OrderNotes = formattedNotes;
            ordersNote.CreatedBy = userId;
            ordersNote.DateCreated = timeStamp;
            ordersNote.LastAmendedBy = userId;
            ordersNote.DateLastAmended = timeStamp;
            ordersNote = ordersNotesRepository.Insert(request, ordersNote);
            if (ordersNote == null)
            {
               //Utilities.ReportError("Unable to Add Order Internal Notes for 'Woodvale's Quality Control'" + Utilities.Message, method_name, true, system, edwFormInstance);
               //TODO: Report Error
            }
            formattedNotes = formattedNotes + "\nSubmit:" + submit;
            //log.Info("Finished Processing Data for Woodvale Maintenance QC Report with Template Id '" + edwFormInstance.TemplateId + "' + and Imp Ref" + edwFormInstance.ImpRef);
            //TODO: Log Info
            returnValue = true;
         }
         catch (Exception ex)
         {
            Message = "Unable to Process Quality Control for Woodvale. Exception: " + ex.Message;
         }
         return returnValue;
      }

      // Woodvale Maintenance Job Ticket V01 "355855623" 
      private bool ProcessWoodvaleJobTicket(HttpRequest request, S4BFormRequest s4bFormRequest, S4bFormSubmissions s4bFormSubmission,
                                            RootObject root, Dictionary<string, S4BFormsControl> s4bFormsControls)
      {
         bool returnValue = false;
         const int TOTAL_NO_QUESTION = 7, TOTAL_NO_ROWS = 15, TOTAL_NO_ROWS_DELAY = 4;
         bool formComplete = false;
         int jobAddressId = -1, jobClientId = -1, userId = -1, assignedTo = -1, appSequence = -1;
         long jobSequence = -1;
         string userName = "", jobRef = "", visitStatus = "", reason = "", eawrName = "";
         string[] works = new string[TOTAL_NO_ROWS];
         bool[] ques = new bool[TOTAL_NO_QUESTION];
         double[] qty = new double[TOTAL_NO_ROWS];
         string[] delayHours = new string[TOTAL_NO_ROWS_DELAY];
         string[] delayDesc = new string[TOTAL_NO_ROWS_DELAY];
         string startDate = "", startTime = "", finishDate = "", finishTime = "";
         DateTime? startDateTime = DateTime.MinValue, finishDateTime = DateTime.MinValue;
         int index = -1;
         DateTime? timeStamp = s4bFormSubmission.DateCreated;
         Dictionary<string, string> imageValues = new Dictionary<string, string>();
         try
         {
            foreach (KeyValuePair<string, S4BFormsControl> s4bFormControlEntry in s4bFormsControls)
            {
               S4BFormsControl s4bFormControl = s4bFormControlEntry.Value;
               if (s4bFormControl.fieldValue != null)
               {
                  switch (s4bFormControlEntry.Key)
                  {
                     case "VARIABLE_PG1_JOB_SEQUENCE":
                        if (long.TryParse(s4bFormControl.fieldValue, out jobSequence))
                        {
                        };
                        break;
                     case "VARIABLE_PG1_JOB_CLIENT_ID":
                        if (int.TryParse(s4bFormControl.fieldValue, out jobClientId))
                        {
                        };
                        break;
                     case "VARIABLE_PG1_JOB_ADDRESS_ID":
                        if (int.TryParse(s4bFormControl.fieldValue, out jobAddressId))
                        {
                        };
                        break;
                     case "VARIABLE_PG1_USER_ID":
                        if (int.TryParse(s4bFormControl.fieldValue, out userId))
                        {
                        };
                        break;

                     case "VARIABLE_PG1_DIARY_RESOURCE":
                        userName = s4bFormControl.fieldValue;
                        break;

                     case "VARIABLE_PG1_DIARY_ENTRY_ID":
                        int.TryParse(s4bFormControl.fieldValue, out appSequence);
                        break;

                     case "FIELD_PG1_JOB_REF":
                        jobRef = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG1_VISIT_STSTUS":
                        visitStatus = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG1_EAWR_NAME":
                        eawrName = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG1_START_DATE":
                        startDate = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG1_START_TIME":
                        startTime = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG1_Q_1":
                     case "FIELD_PG1_Q_2":
                     case "FIELD_PG1_Q_3":
                     case "FIELD_PG1_Q_4":
                     case "FIELD_PG1_Q_5":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(12, 1), out index);
                        bool valueQ = false;
                        Boolean.TryParse(s4bFormControl.fieldValue, out valueQ);
                        ques[index - 1] = valueQ;
                        break;

                     case "FIELD_PG1_REASON":
                        reason = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG2_ROW01_QTY":
                     case "FIELD_PG2_ROW02_QTY":
                     case "FIELD_PG2_ROW03_QTY":
                     case "FIELD_PG2_ROW04_QTY":
                     case "FIELD_PG2_ROW05_QTY":
                     case "FIELD_PG2_ROW06_QTY":
                     case "FIELD_PG2_ROW07_QTY":
                     case "FIELD_PG2_ROW08_QTY":
                     case "FIELD_PG2_ROW09_QTY":
                     case "FIELD_PG2_ROW10_QTY":
                     case "FIELD_PG2_ROW11_QTY":
                     case "FIELD_PG2_ROW12_QTY":
                     case "FIELD_PG2_ROW13_QTY":
                     case "FIELD_PG2_ROW14_QTY":
                     case "FIELD_PG2_ROW15_QTY":
                        index = -1;
                        double result = 0;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        double.TryParse(s4bFormControl.fieldValue, out result);
                        qty[index - 1] = result;
                        break;

                     case "FIELD_PG2_ROW01_WORKS":
                     case "FIELD_PG2_ROW02_WORKS":
                     case "FIELD_PG2_ROW03_WORKS":
                     case "FIELD_PG2_ROW04_WORKS":
                     case "FIELD_PG2_ROW05_WORKS":
                     case "FIELD_PG2_ROW06_WORKS":
                     case "FIELD_PG2_ROW07_WORKS":
                     case "FIELD_PG2_ROW08_WORKS":
                     case "FIELD_PG2_ROW09_WORKS":
                     case "FIELD_PG2_ROW10_WORKS":
                     case "FIELD_PG2_ROW11_WORKS":
                     case "FIELD_PG2_ROW12_WORKS":
                     case "FIELD_PG2_ROW13_WORKS":
                     case "FIELD_PG2_ROW14_WORKS":
                     case "FIELD_PG2_ROW15_WORKS":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        works[index - 1] = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG2_ROW01_HOURS":
                     case "FIELD_PG2_ROW02_HOURS":
                     case "FIELD_PG2_ROW03_HOURS":
                     case "FIELD_PG2_ROW04_HOURS":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        delayHours[index - 1] = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG2_ROW01_DELAY":
                     case "FIELD_PG2_ROW02_DELAY":
                     case "FIELD_PG2_ROW03_DELAY":
                     case "FIELD_PG2_ROW04_DELAY":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        delayDesc[index - 1] = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG3_FINISH_DATE":
                        finishDate = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG3_FINISH_TIME":
                        finishTime = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG3_FORM_COMPLETE":
                        bool.TryParse(s4bFormControl.fieldValue, out formComplete);
                        break;
                  }
               }
            }
            if (jobSequence <= 0)
            {
               if (jobRef != "")
               {
                  OrdersRepository orderRepos = new OrdersRepository(null);
                  Orders order = orderRepos.GetOrderByJobRef(jobRef, request);
               }
            }
            // Start Date & Start Time 
            string finalStartDateTime = startDate == "" ? DateTime.MinValue.ToString("dd/MM/yyyy") : startDate;
            finalStartDateTime = finalStartDateTime + " " + (startTime == "" ? "00:00" : startTime);
            startDateTime = DateTime.ParseExact(finalStartDateTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            int jobStatusId = Utilities.GetWoodvaleStatusByStatusDesc(visitStatus);
            if (formComplete) //Page 3 Status Complete
            {
               OrdersRepository ordersRepository = new OrdersRepository(null);
               if (!ordersRepository.boolUpdateOrderStatusSucceeded(request, true, jobSequence, 2, startDateTime, "Complete", userId, timeStamp))
               {
                  //TODO:
                  //Utilities.ReportError("For Woodvale Maint Imp Ref: " + edwFormInstance.ImpRef + ", unable to Add Order Status Audit. " + Utilities.Message, method_name, true, system, edwFormInstance);
               }

               /////
               //TODO:
               /////////
               OrdersBillsRepository ordersBillsRepository = new OrdersBillsRepository();
               if (!ordersBillsRepository.CreateApplicationForPayment(request, jobSequence, "", timeStamp))
               {
                  //    //TODO:
                  //    //Utilities.ReportError("Unable to Create Application for Payment for the Job Sequence " + jobSequence + " and Imp Ref '" + edwFormInstance.ImpRef + "' for Decor Rofing Quote." + Utilities.Message, method_name, true, system, edwFormInstance);
               }
            }
            else ///Page 1 Status Update
            {
               string statusDesc = "EAWR Helpdesk Name: " + eawrName + "\n" +
                                   "Start Date & Time: " + startDate + " " + startTime;
               if (ques[1] || ques[2] || ques[3])
               {
                  statusDesc = statusDesc + "\nReason " + reason;
               }
               if (jobStatusId >= 0)
               {
                  OrdersRepository ordersRepository = new OrdersRepository( null);
                  if (!ordersRepository.boolUpdateOrderStatusSucceeded(request, true, jobSequence, jobStatusId, startDateTime, statusDesc, userId, timeStamp))
                  {
                     //TODO:
                     //Utilities.ReportError("For Woodvale Maint Imp Ref: " + edwFormInstance.ImpRef + ", unable to Add Order Status Audit. " + Utilities.Message, method_name, true, system, edwFormInstance);
                  }
               }
            }

            if (startDateTime != DateTime.MinValue)
            {
               OrdersRepository ordersRepository = new OrdersRepository(null);
               if (ordersRepository.updateFlgJobStartAndJobDateStartByJobSequence(request, jobSequence, true, startDateTime))
               {
                  if (!ordersRepository.UpdateFlgUser1AndDateUser1ByJobSequence(request, jobSequence, true, startDateTime))
                  {
                     //TODO:
                     //Utilities.ReportError("Unable to Update Order Flag User 1 for the Job Sequence " + jobSequence + " and Imp Ref '" + edwFormInstance.ImpRef + "' for Decor Rofing Quote." + Utilities.Message, method_name, true, system, edwFormInstance);
                  }
               }
               else
               {
                  //TODO:
                  //Utilities.ReportError("Unable to Update Order Start Date Time for the Job Sequence " + jobSequence + " and Imp Ref '" + edwFormInstance.ImpRef + "' for Decor Rofing Quote." + Utilities.Message, method_name, true, system, edwFormInstance);
               }
            }
            /// Data Insert for Items Completed
            /// 
            bool dataFound = false;
            string textRowDesc = "Items Completed";
            for (int counter = 0; counter < TOTAL_NO_ROWS; counter++)
            {
               if (qty[counter] != 0 || (works[counter] != null && works[counter] != ""))
               { dataFound = true; }
            }
            if (dataFound)
            {
               OINFSubmissionDataRepository oINFSubmissionDataRepository = new OINFSubmissionDataRepository();
               OINFSubmissionData oINFSubmissionData = new OINFSubmissionData();
               oINFSubmissionData.JobSequence = jobSequence;
               oINFSubmissionData.DeSequence = appSequence;
               oINFSubmissionData.DatDe = startDateTime;
               oINFSubmissionData.NfsSubmitNo = s4bFormSubmission.S4bSubmitNo;
               oINFSubmissionData.NfsSubmitTs = s4bFormSubmission.S4bSubmitTs;
               oINFSubmissionData.FlgRowIsText = true;
               oINFSubmissionData.ItemQuantity = 0;
               oINFSubmissionData.ItemCode = "";
               oINFSubmissionData.ItemDesc = textRowDesc;
               oINFSubmissionData.ItemUnits = "";
               oINFSubmissionData.AmountUnit = 0;
               oINFSubmissionData.AmountTotal = 0;
               oINFSubmissionData.FlgThirdParty = false;
               oINFSubmissionData.IdThirdParty = -1;
               oINFSubmissionData.CreatedBy = userId;
               oINFSubmissionData.DateCreated = DateTime.Now;
               oINFSubmissionData.LastAmendedBy = userId;
               oINFSubmissionData.DateLastAmended = DateTime.Now;
               oINFSubmissionData = oINFSubmissionDataRepository.insert(request, oINFSubmissionData);
               if (oINFSubmissionData == null)
               {
                  //TODO:
                  //Utilities.ReportError("Unable to Add OI NF Submission Data Text Line for Job Sequence " + jobSequence + " and Imp Ref '" + edwFormInstance.ImpRef + "' for Woodvale Maint Job Ticket." + Utilities.Message, method_name, true, system, edwFormInstance);
               }
               string formattedNotes = "Job Ticket Submission for Job Ref: " + jobRef + "\r\n";
               formattedNotes += "Date: " + ((DateTime)startDateTime).ToString("dd/MM/yyyy HH:mm") + "\r\n";
               for (int counter = 0; counter < TOTAL_NO_ROWS; counter++)
               {
                  if (qty[counter] != 0 || (works[counter] != null && works[counter] != ""))
                  {
                     oINFSubmissionData = new OINFSubmissionData();
                     oINFSubmissionData.JobSequence = jobSequence;
                     oINFSubmissionData.DeSequence = appSequence;
                     oINFSubmissionData.DatDe = startDateTime;
                     oINFSubmissionData.NfsSubmitNo = s4bFormSubmission.S4bSubmitNo;
                     oINFSubmissionData.NfsSubmitTs = s4bFormSubmission.S4bSubmitTs;
                     oINFSubmissionData.FlgRowIsText = false;
                     oINFSubmissionData.ItemQuantity = qty[counter];
                     oINFSubmissionData.ItemCode = "";
                     oINFSubmissionData.ItemDesc = works[counter];
                     oINFSubmissionData.ItemUnits = "";
                     oINFSubmissionData.AmountUnit = 0;
                     oINFSubmissionData.AmountTotal = 0;
                     oINFSubmissionData.FlgThirdParty = false;
                     oINFSubmissionData.IdThirdParty = -1;
                     oINFSubmissionData.CreatedBy = userId;
                     oINFSubmissionData.DateCreated = DateTime.Now;
                     oINFSubmissionData.LastAmendedBy = userId;
                     oINFSubmissionData.DateLastAmended = DateTime.Now;
                     oINFSubmissionData = oINFSubmissionDataRepository.insert(request, oINFSubmissionData);
                     if (oINFSubmissionData == null)
                     {
                        //TODO:
                        //Utilities.ReportError("Unable to Add OI NF Submission Data Text Line for Job Sequence " + jobSequence + " and Imp Ref '" + edwFormInstance.ImpRef + "' for Woodvale Maint Job Ticket." + Utilities.Message, method_name, true, system, edwFormInstance);
                     }
                     formattedNotes += "Item Description:" + works[counter] + "\r\n";
                     formattedNotes += "Item Quanity:" + qty[counter] + "\r\n\r\n";
                     //Code Comment out as Per Tino's request dated: 17/11/2016
                     //OrderItemsRepository orderItemsRepository = new OrderItemsRepository(null);
                     //OrderItems oi = new OrderItems();
                     //oi.JobSequence = jobSequence;
                     //oi.FlgRowIsText = false;
                     //oi.ItemType = 0;
                     //oi.RowIndex = 9999;
                     //oi.TransType = SimplicityConstants.ClientTransType;
                     //oi.ItemCode = "";
                     //oi.ItemDesc = works[counter];
                     //oi.ItemUnits = "";
                     //oi.ItemQuantity = qty[counter];
                     //oi.AmountLabour = oi.AmountMaterials = oi.AmountPlant = oi.AmountValue = oi.AmountTotal = 0;
                     //oi.AssignedTo = -1;
                     //oi.FlgCompleted = false;
                     //oi.FlgDocsRecd = false;
                     //oi.AdjCode = SimplicityConstants.NotAvailable;
                     //oi.VamCostSequence = -1;
                     //oi.AssetSequence = -1;
                     //oi.GrpOrdTi = -1;
                     //oi.MeOiSequence = -1;
                     //oi.SupplierId = -1;
                     //oi.RciInvNo = SimplicityConstants.NotSet;
                     //oi.CreatedBy = userId;
                     //oi.DateCreated = DateTime.Now;
                     //oi = orderItemsRepository.CreateOrderItems(oi, request);
                     //if (oi == null)
                     //{
                     //    //TODO:
                     //    //Utilities.ReportError("Unable to Add Order Item for Job Sequence " + jobSequence + " and Imp Ref '" + edwFormInstance.ImpRef + "' for Woodvale Maint Job Ticket." + Utilities.Message, method_name, true, system, edwFormInstance);
                     //}
                  }
               }
               OrdersNotesRepository ordersNotesRepository = new OrdersNotesRepository();
               OrdersNotes ordersNote = new OrdersNotes();
               ordersNote.JobSequence = jobSequence;
               ordersNote.OrderNotes = formattedNotes;
               ordersNote.CreatedBy = userId;
               ordersNote.DateCreated = timeStamp;
               ordersNote.LastAmendedBy = userId;
               ordersNote.DateLastAmended = timeStamp;
               ordersNote = ordersNotesRepository.Insert(request, ordersNote);
               if (ordersNote == null)
               {
                  //Utilities.ReportError("Unable to Add Order Internal Notes for 'Woodvale's Quality Control'" + Utilities.Message, method_name, true, system, edwFormInstance);
                  //TODO: Report Error
               }
            }
            /// Data Insert for Delay
            /// 
            dataFound = false;
            textRowDesc = "Delay";
            for (int counter = 0; counter < TOTAL_NO_ROWS_DELAY; counter++)
            {
               if ((delayHours[counter] != null && delayHours[counter] != "") || (delayDesc[counter] != null && delayDesc[counter] != ""))
               { dataFound = true; }
            }
            if (dataFound)
            {
               OINFSubmissionDataRepository oINFSubmissionDataRepository = new OINFSubmissionDataRepository();
               OINFSubmissionData oINFSubmissionData = new OINFSubmissionData();
               oINFSubmissionData.JobSequence = jobSequence;
               oINFSubmissionData.DeSequence = appSequence;
               oINFSubmissionData.DatDe = startDateTime;
               oINFSubmissionData.NfsSubmitNo = s4bFormSubmission.S4bSubmitNo;
               oINFSubmissionData.NfsSubmitTs = s4bFormSubmission.S4bSubmitTs;
               oINFSubmissionData.FlgRowIsText = true;
               oINFSubmissionData.ItemQuantity = 0;
               oINFSubmissionData.ItemCode = "";
               oINFSubmissionData.ItemDesc = textRowDesc;
               oINFSubmissionData.ItemUnits = "";
               oINFSubmissionData.AmountUnit = 0;
               oINFSubmissionData.AmountTotal = 0;
               oINFSubmissionData.FlgThirdParty = false;
               oINFSubmissionData.IdThirdParty = -1;
               oINFSubmissionData.CreatedBy = userId;
               oINFSubmissionData.DateCreated = DateTime.Now;
               oINFSubmissionData.LastAmendedBy = userId;
               oINFSubmissionData.DateLastAmended = DateTime.Now;
               oINFSubmissionData = oINFSubmissionDataRepository.insert(request, oINFSubmissionData);
               if (oINFSubmissionData == null)
               {
                  //TODO:
                  //Utilities.ReportError("Unable to Add OI NF Submission Data Text Line for Job Sequence " + jobSequence + " and Imp Ref '" + edwFormInstance.ImpRef + "' for Woodvale Maint Job Ticket." + Utilities.Message, method_name, true, system, edwFormInstance);
               }
               for (int counter = 0; counter < TOTAL_NO_ROWS_DELAY; counter++)
               {
                  if ((delayHours[counter] != null && delayHours[counter] != "") || (delayDesc[counter] != null && delayDesc[counter] != ""))
                  {
                     oINFSubmissionData = new OINFSubmissionData();
                     oINFSubmissionData.JobSequence = jobSequence;
                     oINFSubmissionData.DeSequence = appSequence;
                     oINFSubmissionData.DatDe = startDateTime;
                     oINFSubmissionData.NfsSubmitNo = s4bFormSubmission.S4bSubmitNo;
                     oINFSubmissionData.NfsSubmitTs = s4bFormSubmission.S4bSubmitTs;
                     oINFSubmissionData.FlgRowIsText = false;
                     oINFSubmissionData.ItemQuantity = Utilities.GetQuantityFromDelayHours(delayHours[counter]);
                     oINFSubmissionData.ItemCode = "";
                     oINFSubmissionData.ItemDesc = delayDesc[counter];
                     oINFSubmissionData.ItemUnits = "HRS";
                     oINFSubmissionData.AmountUnit = 0;
                     oINFSubmissionData.AmountTotal = 0;
                     oINFSubmissionData.FlgThirdParty = false;
                     oINFSubmissionData.IdThirdParty = -1;
                     oINFSubmissionData.CreatedBy = userId;
                     oINFSubmissionData.DateCreated = DateTime.Now;
                     oINFSubmissionData.LastAmendedBy = userId;
                     oINFSubmissionData.DateLastAmended = DateTime.Now;
                     oINFSubmissionData = oINFSubmissionDataRepository.insert(request, oINFSubmissionData);
                     if (oINFSubmissionData == null)
                     {
                        //TODO:
                        //Utilities.ReportError("Unable to Add OI NF Submission Data Text Line for Job Sequence " + jobSequence + " and Imp Ref '" + edwFormInstance.ImpRef + "' for Woodvale Maint Job Ticket." + Utilities.Message, method_name, true, system, edwFormInstance);
                     }
                  }
               }
            }
            /// Data Insert for Finish Date
            string finalFinishDateTime = finishDate == "" ? DateTime.MinValue.ToString("dd/MM/yyyy") : finishDate;
            finalFinishDateTime = finalFinishDateTime + " " + (finishTime == "" ? "00:00" : finishTime);
            finishDateTime = DateTime.ParseExact(finalFinishDateTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            if (finishDateTime != DateTime.MinValue)
            {
               OrdersRepository ordersRepository = new OrdersRepository(null);
               if (ordersRepository.updateFlgJobFinishAndJobDateFinishByJobSequence(request, jobSequence, true, finishDateTime))
               {
                  if (!ordersRepository.UpdateFlgJobSLATimerStopAndDateJobSLATimerStopByJobSequence(request, jobSequence, true, finishDateTime))
                  {
                     //TODO:
                     //Utilities.ReportError("Unable to Update Order SLA Flag for the Job Sequence " + jobSequence + " and Imp Ref '" + edwFormInstance.ImpRef + "' for Decor Rofing Quote." + Utilities.Message, method_name, true, system, edwFormInstance);
                  }
               }
               else
               {
                  //TODO:
                  //Utilities.ReportError("Unable to Update Order Finish Date Time for the Job Sequence " + jobSequence + " and Imp Ref '" + edwFormInstance.ImpRef + "' for Decor Rofing Quote." + Utilities.Message, method_name, true, system, edwFormInstance);
               }
               if (!ordersRepository.UpdateFlgSetToJTAndDateSetToJTByJobSequence(request, jobSequence, true, finishDateTime))
               {
                  //TODO:
                  //Utilities.ReportError("Unable to Update Job Ticket Flag for the Job Sequence " + jobSequence + " and Imp Ref '" + edwFormInstance.ImpRef + "' for Decor Rofing Quote." + Utilities.Message, method_name, true, system, edwFormInstance);
               }
            }

            //log.Info("Finished Processing Data for Woodvale Maintenance QC Report with Template Id '" + edwFormInstance.TemplateId + "' + and Imp Ref" + edwFormInstance.ImpRef);
            //TODO: Log Info
            returnValue = true;
         }
         catch (Exception ex)
         {
            Message = "Unable to Process Job Ticket for Woodvale. Exception: " + ex.Message;
         }
         return returnValue;
      }

      // Avon Ruby - Worksheet 1 Avon Ruby V01 "1026189291" 
      private bool ProcessAvonRubyWorkshee1(HttpRequest request, S4BFormRequest s4bFormRequest, S4bFormSubmissions s4bFormSubmission,
                                            RootObject root, Dictionary<string, S4BFormsControl> s4bFormsControls)
      {
         const string METHOD_NAME = "S4BFormsRepository.ProcessAvonRubyWorkshee1()";
         bool returnValue = false;
         int userId = Utilities.GetUserIdFromRequest(request);
         long jobSequence = -1, jobAddressId = -1, jobClientId = -1;
         DiaryResources diaryResource = null;
         int visitStatusId = -1;
         long appSequence = -1;
         string userName = "";
         double[] descOfWork_amtValue = new double[10];
         string[] descOfWork_itemDesc = new string[10];
         string[] furtherInfo = new string[10];
         string location = "", serialNo = "", make = "", gcNo = "", model = "", type = "", condition = "";
         string diaryResourceName = "", timeStart = "", timeEnd = "", vat = "", subTotal = "", total = "";
         DateTime? installDate = DateTime.MinValue, lastServiceDate = DateTime.MinValue;
         string[] recommendations = new string[4];
         double[] matUsed_amtTotal = new double[8];
         string[] matUsed_itemDesc = new string[8];
         double[] matUsed_itemQty = new double[8];
         string[] necessaryRemedial = new string[4];
         string[] defectFound = new string[16];
         bool flgPymtCash = false, flgPymtCheque = false, flgPymtCard = false, flgPymtInvoice = false;
         int index = -1;
         DateTime? timeStamp = s4bFormSubmission.DateCreated;
         DateTime? appDate = DateTime.Now;
         DateTime? resultDate = DateTime.MinValue;
         Dictionary<string, string> imageValues = new Dictionary<string, string>();
         try
         {
            foreach (KeyValuePair<string, S4BFormsControl> s4bFormControlEntry in s4bFormsControls)
            {
               S4BFormsControl s4bFormControl = s4bFormControlEntry.Value;
               if (s4bFormControl.fieldValue != null)
               {
                  switch (s4bFormControlEntry.Key)
                  {
                     case "VARIABLE_PG1_JOB_SEQUENCE":
                        if (long.TryParse(s4bFormControl.fieldValue, out jobSequence))
                        {
                        };
                        break;
                     case "VARIABLE_PG1_DIARY_ENTRY_ID":
                     case "VARIABLE_PG1_DA_SEQUENCE":
                        long.TryParse(s4bFormControl.fieldValue, out appSequence);
                        break;
                     case "VARIABLE_PG1_JOB_CLIENT_ID":
                        if (long.TryParse(s4bFormControl.fieldValue, out jobClientId))
                        {
                        };
                        break;
                     case "VARIABLE_PG1_JOB_ADDRESS_ID":
                        if (long.TryParse(s4bFormControl.fieldValue, out jobAddressId))
                        {
                        };
                        break;
                     case "VARIABLE_PG1_USER_ID":
                        if (int.TryParse(s4bFormControl.fieldValue, out userId))
                        {
                        };
                        break;
                     case "VARIABLE_PG1_DIARY_RESOURCE":
                     case "VARIABLE_PG2_DIARY_RESOURCE":
                        userName = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG6_VISIT_STAUTS":
                        visitStatusId = Utilities.GetVisitStatusIdByStatusDesc(request, s4bFormControl.fieldValue);
                        break;

                     case "VARIABLE_PG6_DIARY_ENTRY_DATE":
                        resultDate = DateTime.MinValue;
                        try
                        {
                           resultDate = DateTime.ParseExact(s4bFormControl.fieldValue.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        }
                        catch (Exception ex)
                        { }
                        appDate = resultDate;
                        break;

                     case "FIELD_PG7_ROW01_DESC":
                     case "FIELD_PG7_ROW02_DESC":
                     case "FIELD_PG7_ROW03_DESC":
                     case "FIELD_PG7_ROW04_DESC":
                     case "FIELD_PG7_ROW05_DESC":
                     case "FIELD_PG7_ROW06_DESC":
                     case "FIELD_PG7_ROW07_DESC":
                     case "FIELD_PG7_ROW08_DESC":
                     case "FIELD_PG7_ROW09_DESC":
                     case "FIELD_PG7_ROW10_DESC":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        if (index > 0)
                        {
                           descOfWork_itemDesc[index - 1] = s4bFormControl.fieldValue;
                        }
                        break;

                     case "VARIABLE_PG7_ROW01_AMT":
                     case "VARIABLE_PG7_ROW02_AMT":
                     case "VARIABLE_PG7_ROW03_AMT":
                     case "VARIABLE_PG7_ROW04_AMT":
                     case "VARIABLE_PG7_ROW05_AMT":
                     case "VARIABLE_PG7_ROW06_AMT":
                     case "VARIABLE_PG7_ROW07_AMT":
                     case "VARIABLE_PG7_ROW08_AMT":
                     case "VARIABLE_PG7_ROW09_AMT":
                     case "VARIABLE_PG7_ROW10_AMT":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        if (index > 0)
                        {
                           Double.TryParse(s4bFormControl.fieldValue, out descOfWork_amtValue[index - 1]);
                        }
                        break;

                     case "FIELD_PG2_ROW01_FURTHER_INFO":
                     case "FIELD_PG2_ROW02_FURTHER_INFO":
                     case "FIELD_PG2_ROW03_FURTHER_INFO":
                     case "FIELD_PG2_ROW04_FURTHER_INFO":
                     case "FIELD_PG2_ROW05_FURTHER_INFO":
                     case "FIELD_PG2_ROW06_FURTHER_INFO":
                     case "FIELD_PG2_ROW07_FURTHER_INFO":
                     case "FIELD_PG2_ROW08_FURTHER_INFO":
                     case "FIELD_PG2_ROW09_FURTHER_INFO":
                     case "FIELD_PG2_ROW10_FURTHER_INFO":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        if (index > 0)
                        {
                           furtherInfo[index - 1] = s4bFormControl.fieldValue;
                        }
                        break;

                     case "FIELD_PG4_LAST_SERVICE_DATE":
                        resultDate = DateTime.MinValue;
                        try
                        {
                           resultDate = DateTime.ParseExact(s4bFormControl.fieldValue.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        }
                        catch (Exception ex)
                        { }
                        lastServiceDate = resultDate;
                        break;

                     case "FIELD_PG4_CONDITION":
                        condition = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG4_LOCATION":
                        location = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG4_MAKE":
                        make = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG4_MODEL":
                        model = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG4_TYPE":
                        type = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG4_INSTALLATION_DATE":
                        resultDate = DateTime.MinValue;
                        try
                        {
                           resultDate = DateTime.ParseExact(s4bFormControl.fieldValue.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        }
                        catch (Exception ex)
                        { }
                        installDate = resultDate;
                        break;

                     case "FIELD_PG4_SERIAL_NO":
                        serialNo = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG4_GC_NO":
                        gcNo = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG5_ROW01_WORKS_REQUIRED":
                     case "FIELD_PG5_ROW02_WORKS_REQUIRED":
                     case "FIELD_PG5_ROW03_WORKS_REQUIRED":
                     case "FIELD_PG5_ROW04_WORKS_REQUIRED":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        if (index > 0)
                        {
                           necessaryRemedial[index - 1] = s4bFormControl.fieldValue;
                        }
                        break;

                     case "FIELD_PG7_FLG_PYMT_CASH":
                        Boolean.TryParse(s4bFormControl.fieldValue, out flgPymtCash);
                        break;

                     case "FIELD_PG7_FLG_PYMT_CHEQUE":
                        Boolean.TryParse(s4bFormControl.fieldValue, out flgPymtCheque);
                        break;

                     case "FIELD_PG7_FLG_PYMT_Card":
                        Boolean.TryParse(s4bFormControl.fieldValue, out flgPymtCard);
                        break;

                     case "FIELD_PG7_FLG_PYMT_INVOICE":
                        Boolean.TryParse(s4bFormControl.fieldValue, out flgPymtInvoice);
                        break;

                     case "FIELD_PG8_ROW01_MAT_DETAILS":
                     case "FIELD_PG8_ROW02_MAT_DETAILS":
                     case "FIELD_PG8_ROW03_MAT_DETAILS":
                     case "FIELD_PG8_ROW04_MAT_DETAILS":
                     case "FIELD_PG8_ROW05_MAT_DETAILS":
                     case "FIELD_PG8_ROW06_MAT_DETAILS":
                     case "FIELD_PG8_ROW07_MAT_DETAILS":
                     case "FIELD_PG8_ROW08_MAT_DETAILS":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        if (index > 0)
                        {
                           matUsed_itemDesc[index - 1] = s4bFormControl.fieldValue;
                        }
                        break;

                     case "FIELD_PG8_ROW01_QTY":
                     case "FIELD_PG8_ROW02_QTY":
                     case "FIELD_PG8_ROW03_QTY":
                     case "FIELD_PG8_ROW04_QTY":
                     case "FIELD_PG8_ROW05_QTY":
                     case "FIELD_PG8_ROW06_QTY":
                     case "FIELD_PG8_ROW07_QTY":
                     case "FIELD_PG8_ROW08_QTY":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        if (index > 0)
                        {
                           Double.TryParse(s4bFormControl.fieldValue, out matUsed_itemQty[index - 1]);
                        }
                        break;

                     case "VARIABLE_PG8_ROW01_AMT":
                     case "VARIABLE_PG8_ROW02_AMT":
                     case "VARIABLE_PG8_ROW03_AMT":
                     case "VARIABLE_PG8_ROW04_AMT":
                     case "VARIABLE_PG8_ROW05_AMT":
                     case "VARIABLE_PG8_ROW06_AMT":
                     case "VARIABLE_PG8_ROW07_AMT":
                     case "VARIABLE_PG8_ROW08_AMT":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        if (index > 0)
                        {
                           Double.TryParse(s4bFormControl.fieldValue, out matUsed_amtTotal[index - 1]);
                        }
                        break;

                     case "FIELD_PG8_ROW01_RECOMMENDATIONS":
                     case "FIELD_PG8_ROW02_RECOMMENDATIONS":
                     case "FIELD_PG8_ROW03_RECOMMENDATIONS":
                     case "FIELD_PG8_ROW04_RECOMMENDATIONS":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        if (index > 0)
                        {
                           recommendations[index - 1] = s4bFormControl.fieldValue;
                        }
                        break;

                     case "VARIABLE_PG3_DIARY_RESOURCE":
                        diaryResourceName = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG6_TIME_START":
                        timeStart = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG6_TIME_END":
                        timeEnd = s4bFormControl.fieldValue;
                        break;

                     case "VARIABLE_PG7_AMT_SUBTOTAL":
                        subTotal = s4bFormControl.fieldValue;
                        break;

                     case "VARIABLE_PG7_AMT_VAT":
                        vat = s4bFormControl.fieldValue;
                        break;

                     case "VARIABLE_PG7_AMT_TOTAL":
                        total = s4bFormControl.fieldValue;
                        break;
                  }
               }
            }
            if (userId <= 0)
            {
               userId = 1;
            }
            diaryResource = Utilities.GetDiaryResourceByUserId(request, userId);
            OrdersNotesRepository ordersNotesRepository = new OrdersNotesRepository();
            OrdersNotes ordersNote = new OrdersNotes();

            /// 1. Further Information - Order Internal Notes
            string formattedNotes = "";
            for (int counter = 0; counter < 10; counter++)
            {
               if (furtherInfo[counter] != null && furtherInfo[counter].Trim() != "")
               {
                  if (formattedNotes == "")
                  {
                     formattedNotes = "Further information or any other hazards" + "\r\n";
                  }
                  formattedNotes = formattedNotes + "\r\n" + furtherInfo[counter];
               }
            }
            if (formattedNotes != "")
            {
               ordersNote.JobSequence = jobSequence;
               ordersNote.OrderNotes = Utilities.replaceSpecialCharsForInsert(formattedNotes);
               ordersNote.CreatedBy = userId;
               ordersNote.DateCreated = timeStamp;
               ordersNote.LastAmendedBy = userId;
               ordersNote.DateLastAmended = timeStamp;
               OrdersNotes ordNoteNew = ordersNotesRepository.Insert(request, ordersNote);
               if (ordersNote == null)
               {
                  Message += Utilities.GenerateAndLogMessage(METHOD_NAME, "Unable to Add Order Notes for Further Information while processing '" + s4bFormSubmission.RefNatForms.FormDesc + "'. Reason: " + ordersNotesRepository.Message, null);
               }
            }

            /// 2. Appliance Details
            if ((type != "" && type != null) ||
                (serialNo != "" && serialNo != null) ||
                (location != "" && location != null) ||
                (make != "" && make != null) ||
                (model != "" && model != null) ||
                (gcNo != "" && gcNo != null) ||
                (condition != "" && condition != null))
            {
               AssetRegister assetRegister = new AssetRegister();
               assetRegister.FlgDeleted = false;
               assetRegister.TransType = SimplicityConstants.ClientTransType;
               assetRegister.EntityId = -1;
               assetRegister.ItemJoinDept = -1;
               assetRegister.ItemJoinCategory = -1;
               assetRegister.ItemJoinSupplementary = -1;
               assetRegister.ItemManufacturer = make;
               assetRegister.ItemModel = model;
               assetRegister.ItemSerialRef = serialNo;
               assetRegister.ItemExtraInfo = type;
               assetRegister.ItemUserField1 = gcNo;
               assetRegister.ItemUserField2 = SimplicityConstants.NotSet;
               assetRegister.ItemUserField3 = SimplicityConstants.NotSet;
               assetRegister.ItemQuantity = 1;
               assetRegister.DateInstalled = installDate;
               assetRegister.DateAcquired = DateTime.MinValue;
               assetRegister.DateDisposed = DateTime.MinValue;
               assetRegister.ItemValueBook = 0;
               assetRegister.ItemValueDepreciation = 0;
               assetRegister.ItemValueDisposal = 0;
               assetRegister.ItemDesc = "";
               assetRegister.ItemAddress = "";
               assetRegister.FlgUseAddressId = false;
               assetRegister.ItemAddressId = -1;
               assetRegister.ItemLocationJoinId = -1;
               assetRegister.ItemLocation = location;
               assetRegister.FlgItemChargeable = false;
               assetRegister.ItemCostMaterialRate = 0;
               assetRegister.ItemCostLabourRate = 0;
               assetRegister.CreatedBy = userId;
               assetRegister.DateCreated = timeStamp;
               assetRegister.LastAmendedBy = userId;
               assetRegister.DateLastAmended = timeStamp;
               AssetRegisterRepository assetRegisterRepository = new AssetRegisterRepository();
               AssetRegister existAssetRegister = assetRegisterRepository.getAssetRegisterByLocationMakeModelTypeSearialNo(request, location, make, model, type, serialNo);
               AssetRegister assetRegisterUpdated = new AssetRegister();
               if (existAssetRegister == null)
               {
                  assetRegisterUpdated = assetRegisterRepository.insert(request, assetRegister);
               }
               else
               {
                  assetRegister.Sequence = existAssetRegister.Sequence;
                  assetRegisterUpdated = assetRegisterRepository.Update(request, assetRegister);
               }
               if (assetRegisterUpdated.Sequence <= 0)
               {
                  Message += Utilities.GenerateAndLogMessage(METHOD_NAME, "Unable to Update Existing Asset Register while processing '" + s4bFormSubmission.RefNatForms.FormDesc + "'. Reason: " + assetRegisterRepository.Message, null);
               }
               else
               {
                  /// Check if it is boiler then insert for supplementary
                  /// 
                  if (type.ToUpper().Equals("COMBI") || type.ToUpper().Equals("BACK BOILER"))
                  {
                     AssetRegisterSuppGas AssetRegisterSuppGasObj = new AssetRegisterSuppGas();
                     AssetRegisterSuppGasObj.Sequence = -1;
                     AssetRegisterSuppGasObj.JoinSequence = assetRegisterUpdated.Sequence;
                     AssetRegisterSuppGasObj.EntityId = -1;
                     AssetRegisterSuppGasObj.AssetGasType = "";
                     AssetRegisterSuppGasObj.FlgGasFixing = false;
                     AssetRegisterSuppGasObj.GasFixing = "";
                     AssetRegisterSuppGasObj.FlgGasType = false;
                     AssetRegisterSuppGasObj.GasType = "";
                     AssetRegisterSuppGasObj.FlgGasFuel = false;
                     AssetRegisterSuppGasObj.GasFuel = "";
                     AssetRegisterSuppGasObj.FlgGasEfficiency = false;
                     AssetRegisterSuppGasObj.GasEfficiency = "";
                     AssetRegisterSuppGasObj.FlgGasFlueType = false;
                     AssetRegisterSuppGasObj.GasFlueType = "";
                     AssetRegisterSuppGasObj.FlgGasFlueing = false;
                     AssetRegisterSuppGasObj.GasFlueing = "";
                     AssetRegisterSuppGasObj.FlgGasOvUvSs = false;
                     AssetRegisterSuppGasObj.GasOvUvSs = "";
                     AssetRegisterSuppGasObj.FlgGasExpansionVessel = false;
                     AssetRegisterSuppGasObj.FlgGasExpansion = false;
                     AssetRegisterSuppGasObj.GasExpansion = "";
                     AssetRegisterSuppGasObj.FlgGasImmersion = false;
                     AssetRegisterSuppGasObj.GasImmersion = "";
                     AssetRegisterSuppGasObj.LastAmendedBy = userId;
                     AssetRegisterSuppGasObj.DateLastAmended = timeStamp;
                     AssetRegisterSuppGasRepository AssetRegisterSuppGasRepos = new AssetRegisterSuppGasRepository();
                     AssetRegisterSuppGas AssetRegisterSuppGasNew = AssetRegisterSuppGasRepos.insert(request, AssetRegisterSuppGasObj);
                     if (AssetRegisterSuppGasNew == null)
                     {
                        Message += Utilities.GenerateAndLogMessage(METHOD_NAME, "Unable to Add Asset Register Supp Gas while processing '" + s4bFormSubmission.RefNatForms.FormDesc + "'. Reason: " + ordersNotesRepository.Message, null);
                     }
                  }
                  AssetRegisterService assetRegisterServiceObj = new AssetRegisterService();
                  assetRegisterServiceObj.Sequence = -1;
                  assetRegisterServiceObj.FlgDeleted = false;
                  assetRegisterServiceObj.AssetSequence = assetRegisterUpdated.Sequence;
                  assetRegisterServiceObj.JobSequence = jobSequence;
                  assetRegisterServiceObj.DaSequence = appSequence;
                  assetRegisterServiceObj.ServiceInitials = "";
                  assetRegisterServiceObj.ServiceNotes = "";
                  assetRegisterServiceObj.ConditionSequence = -1;
                  assetRegisterServiceObj.ServiceBy = userId;
                  assetRegisterServiceObj.FlgNewJobCreated = false;
                  assetRegisterServiceObj.FlgNewApp = false;
                  assetRegisterServiceObj.FlgValidated = false;
                  assetRegisterServiceObj.ValidatedBy = -1;
                  assetRegisterServiceObj.DateValidated = DateTime.MinValue;
                  assetRegisterServiceObj.ItemCostLabourRate = 0;
                  assetRegisterServiceObj.CreatedBy = userId;
                  assetRegisterServiceObj.DateCreated = timeStamp;
                  AssetRegisterService assetRegisterServiceNew = null;
                  AssetRegisterServiceRepository assetRegisterServiceRepos = new AssetRegisterServiceRepository();
                  if (lastServiceDate != null && lastServiceDate != DateTime.MinValue)
                  {
                     assetRegisterServiceObj.FlgNotActive = true;
                     assetRegisterServiceObj.DaAppType = -1;
                     assetRegisterServiceObj.DateDaStart = lastServiceDate;
                     assetRegisterServiceObj.DateService = lastServiceDate;
                     assetRegisterServiceNew = assetRegisterServiceRepos.insert(request, assetRegisterServiceObj);
                     if (assetRegisterServiceNew == null)
                     {
                        Message += Utilities.GenerateAndLogMessage(METHOD_NAME, "Unable to Add Asset Register Service for Further Information while processing '" + s4bFormSubmission.RefNatForms.FormDesc + "'. Reason: " + assetRegisterServiceRepos.Message, null);
                     }
                  }
                  assetRegisterServiceObj.FlgNotActive = false;
                  assetRegisterServiceObj.DaAppType = 1;
                  assetRegisterServiceObj.DateDaStart = appDate;
                  assetRegisterServiceObj.DateService = appDate;
                  assetRegisterServiceNew = assetRegisterServiceRepos.insert(request, assetRegisterServiceObj);
                  if (assetRegisterServiceNew == null)
                  {
                     Message += Utilities.GenerateAndLogMessage(METHOD_NAME, "Unable to Add Asset Register Service for Further Information while processing '" + s4bFormSubmission.RefNatForms.FormDesc + "'. Reason: " + assetRegisterServiceRepos.Message, null);
                  }
               }
            }

            /// 3. NECESSARY REMEDIAL WORK REQUIRED
            formattedNotes = "";
            for (int counter = 0; counter < 4; counter++)
            {
               if (necessaryRemedial[counter] != null && necessaryRemedial[counter].Trim() != "")
               {
                  if (formattedNotes == "")
                  {
                     formattedNotes = "Necessary Remedial Work Required" + "\r\n";
                  }
                  formattedNotes = formattedNotes + "\r\n" + necessaryRemedial[counter];
               }
            }
            if (formattedNotes != "")
            {
               ordersNote = new OrdersNotes();
               ordersNote.JobSequence = jobSequence;
               ordersNote.OrderNotes = Utilities.replaceSpecialCharsForInsert(formattedNotes);
               ordersNote.CreatedBy = userId;
               ordersNote.DateCreated = timeStamp;
               ordersNote.LastAmendedBy = userId;
               ordersNote.DateLastAmended = timeStamp;
               ordersNote = ordersNotesRepository.Insert(request, ordersNote);
               if (ordersNote == null)
               {
                  Message += Utilities.GenerateAndLogMessage(METHOD_NAME, "Unable to Add Order Notes for Further Information while processing '" + s4bFormSubmission.RefNatForms.FormDesc + "'. Reason: " + ordersNotesRepository.Message, null);
               }
            }

            /// 4. Visit Status Update
            if (appSequence > 0)
            {
               DiaryAppsRepository diaryAppsRepository = new DiaryAppsRepository();
               if (visitStatusId >= 0)
               {
                  if (!diaryAppsRepository.updateVisitStatusAndFlgCompletedBySequence(request, appSequence, visitStatusId, true, userId, timeStamp))
                  {
                     Message += Utilities.GenerateAndLogMessage(METHOD_NAME, "Unable to Update Visit Status and Completed Flag while processing '" + s4bFormSubmission.RefNatForms.FormDesc + "'. Reason: " + diaryAppsRepository.Message, null);
                  }
               }
            }

            /// 5. Description of Works
            string descOfWorks = "Description Of Works\r\n";
            for (int counter = 0; counter < 10; counter++)
            {
               if (descOfWork_amtValue[counter] > 0 ||
                   (descOfWork_itemDesc[counter] != "" && descOfWork_itemDesc[counter] != null))
               {
                  descOfWorks = descOfWorks + descOfWork_itemDesc[counter];
                  if (descOfWork_amtValue[counter] != 0)
                  {
                     descOfWorks = descOfWorks + " - Value " + descOfWork_amtValue[counter];
                  }
                  descOfWorks = descOfWorks + "\r\n";
               }
            }

            /// 7. Materials Used / Description
            string materialsUsed = "Materials Used/Description\r\n";
            for (int counter = 0; counter < 8; counter++)
            {
               if (matUsed_amtTotal[counter] > 0 ||
                   matUsed_itemQty[counter] > 0 ||
                   (matUsed_itemDesc[counter] != "" && matUsed_itemDesc[counter] != null))
               {
                  double matAmt = 0;
                  if (matUsed_itemQty[counter] != 0)
                  {
                     matAmt = Math.Round(matUsed_amtTotal[counter] / matUsed_itemQty[counter], 2); ;
                  }
                  materialsUsed = materialsUsed + matUsed_itemDesc[counter];
                  if (matUsed_amtTotal[counter] != 0)
                  {
                     materialsUsed = materialsUsed + " Value: " + matUsed_amtTotal[counter];
                  }
                  materialsUsed = materialsUsed + "\r\n";
               }
            }

            /// 8. Recommendations For Further Work
            formattedNotes = "";
            String recomms = "Recommendations\r\n";
            for (int counter = 0; counter < 4; counter++)
            {
               if (recommendations[counter] != null && recommendations[counter].Trim() != "")
               {
                  if (formattedNotes == "")
                  {
                     formattedNotes = "RECOMMENDATIONS FOR FURTHER WORK" + "\r\n";
                  }
                  formattedNotes = formattedNotes + "\r\n" + recommendations[counter];
                  recomms = recomms + recommendations[counter] + "\r\n";
               }
            }
            if (formattedNotes != "")
            {
               ordersNote = new OrdersNotes();
               ordersNote.JobSequence = jobSequence;
               ordersNote.OrderNotes = Utilities.replaceSpecialCharsForInsert(formattedNotes);
               ordersNote.CreatedBy = userId;
               ordersNote.DateCreated = timeStamp;
               ordersNote.LastAmendedBy = userId;
               ordersNote.DateLastAmended = timeStamp;
               ordersNote = ordersNotesRepository.Insert(request, ordersNote);
               if (ordersNote == null)
               {
                  Message += Utilities.GenerateAndLogMessage(METHOD_NAME, "Unable to Add Order Notes For Recommendations while processing '" + s4bFormSubmission.RefNatForms.FormDesc + "'. Reason: " + ordersNotesRepository.Message, null);
               }
            }

            /// 9. Payment Type Additions into Schedule Items
            if (flgPymtCheque || flgPymtCard || flgPymtCash || flgPymtInvoice)
            {
               string paymentTypes = "";
               if (flgPymtCheque)
               {
                  if (paymentTypes == "")
                  {
                     paymentTypes = "Payment = Cheque";
                  }
                  else
                  {
                     paymentTypes = paymentTypes + " - Cheque";
                  }
               }
               if (flgPymtCard)
               {
                  if (paymentTypes == "")
                  {
                     paymentTypes = "Payment = Card";
                  }
                  else
                  {
                     paymentTypes = paymentTypes + " - Card";
                  }
               }
               if (flgPymtCash)
               {
                  if (paymentTypes == "")
                  {
                     paymentTypes = "Payment = Cash";
                  }
                  else
                  {
                     paymentTypes = paymentTypes + " - Cash";
                  }
               }
               if (flgPymtInvoice)
               {
                  if (paymentTypes == "")
                  {
                     paymentTypes = "Payment = Invoice";
                  }
                  else
                  {
                     paymentTypes = paymentTypes + " - Invoice";
                  }
               }
            }

            // Order Internal Notes Entry
            string appDateString = (appDate == null || appDate == DateTime.MinValue) ? "" : ((DateTime)appDate).ToString("dd/MM/yyyy");
            string installDateString = (installDate == null || installDate == DateTime.MinValue) ? "" : ((DateTime)installDate).ToString("dd/MM/yyyy");
            string lastServiceDateString = lastServiceDate == DateTime.MinValue ? "" : ((DateTime)lastServiceDate).ToString("dd/MM/yyyy");
            string orderInternalNotes = "W.S-" + appDateString + " " + diaryResource.ResourceName + "\r\n\r\n";
            orderInternalNotes += diaryResource.ResourceName + " attended " + appDateString + " between " + timeStart + " - " + timeEnd + "\r\n\r\n";
            orderInternalNotes += "Appliance Details\r\n";
            orderInternalNotes += "Location: " + location + "\r\n";
            orderInternalNotes += "Make: " + make + "\r\n";
            orderInternalNotes += "Model: " + model + "\r\n";
            orderInternalNotes += "Type: " + type + "\r\n";
            orderInternalNotes += "Condition: " + condition + "\r\n";
            orderInternalNotes += "Sreial no: " + serialNo + "\r\n";
            orderInternalNotes += "GC No: " + gcNo + "\r\n";
            orderInternalNotes += "Install Date: " + installDateString + "\r\n";
            orderInternalNotes += "Last Service Date: " + lastServiceDateString + "\r\n\r\n";
            orderInternalNotes += descOfWorks + "\r\n";
            orderInternalNotes += materialsUsed + "\r\n";
            orderInternalNotes += recomms + "\r\n";
            orderInternalNotes += "Subtotal " + subTotal + "\r\n";
            orderInternalNotes += "VAT " + vat + "\r\n";
            orderInternalNotes += "Total " + total + "\r\n\r\n";
            string paymentType = "";
            if (flgPymtCash)
            {
               paymentType = "Cash";
            }
            else if (flgPymtCard)
            {
               paymentType = "Card";
            }
            else if (flgPymtCheque)
            {
               paymentType = "Cheque";
            }
            else if (flgPymtInvoice)
            {
               paymentType = "Invoice";
            }
            orderInternalNotes += "Payment Type: " + paymentType;
            ordersNote = new OrdersNotes();
            ordersNote.JobSequence = jobSequence;
            ordersNote.OrderNotes = Utilities.replaceSpecialCharsForInsert(orderInternalNotes);
            ordersNote.CreatedBy = userId;
            ordersNote.DateCreated = timeStamp;
            ordersNote.LastAmendedBy = userId;
            ordersNote.DateLastAmended = timeStamp;
            ordersNote = ordersNotesRepository.Insert(request, ordersNote);
            if (ordersNote == null)
            {
               Message += Utilities.GenerateAndLogMessage(METHOD_NAME, "Unable to Add Order Notes Summary while processing '" + s4bFormSubmission.RefNatForms.FormDesc + "'. Reason: " + ordersNotesRepository.Message, null);
            }

            returnValue = true;
         }
         catch (Exception ex)
         {
            Message += Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured while processing '" + s4bFormSubmission.RefNatForms.FormDesc + "'.", ex);
         }
         return returnValue;
      }

      // Avon Ruby - Roofing V02 "1299383635" 
      private bool ProcessAvonRubyRoofing(HttpRequest request, S4BFormRequest s4bFormRequest, S4bFormSubmissions s4bFormSubmission,
                                          RootObject root, Dictionary<string, S4BFormsControl> s4bFormsControls)
      {
         bool returnValue = false;
         int userId = Utilities.GetUserIdFromRequest(request);

         int visitStatusId = -1;
         long jobSequence = -1, jobAddressId = -1, jobClientId = -1;
         long appSequence = -1;
         string userName = "";
         DiaryResources diaryResource = null;
         string diaryResourceName = "", timeStart = "", timeEnd = "", vat = "", subTotal = "", total = "";
         string typeOfAttendance = "";
         double subTotalMaterials = 0, subTotalLabour = 0, subtotalDiscount = 0;
         string reasonForAttendance = "", briefDescOfWorks = "";
         string[] descOfWork_itemDesc = new string[14];
         double[] descOfWork_amtValue = new double[14];
         string[] materials_itemDesc = new string[14];
         double[] materials_amtValue = new double[14];
         double[] materials_Qty = new double[14];
         string recommsForWorksRequired = "";
         string[] drawingRoof = new string[2];
         string[] drawingMeasurements = new string[2];
         string[] drawingNotes = new string[2];
         bool flgPymtCash = false, flgPymtCheque = false, flgPymtCard = false, flgPymtInvoice = false;
         int index = -1;
         DateTime? timeStamp = s4bFormSubmission.DateCreated;
         DateTime appDate = DateTime.Now;
         Dictionary<string, string> imageValues = new Dictionary<string, string>();
         try
         {
            foreach (KeyValuePair<string, S4BFormsControl> s4bFormControlEntry in s4bFormsControls)
            {
               S4BFormsControl s4bFormControl = s4bFormControlEntry.Value;
               if (s4bFormControl.fieldValue != null)
               {
                  switch (s4bFormControlEntry.Key)
                  {
                     case "VARIABLE_PG1_JOB_SEQUENCE":
                        if (long.TryParse(s4bFormControl.fieldValue, out jobSequence))
                        {
                        };
                        break;
                     case "VARIABLE_PG1_DIARY_ENTRY_ID":
                        long.TryParse(s4bFormControl.fieldValue, out appSequence);
                        break;
                     case "VARIABLE_PG1_JOB_CLIENT_ID":
                        if (long.TryParse(s4bFormControl.fieldValue, out jobClientId))
                        {
                        };
                        break;
                     case "VARIABLE_PG1_JOB_ADDRESS_ID":
                        if (long.TryParse(s4bFormControl.fieldValue, out jobAddressId))
                        {
                        };
                        break;
                     case "VARIABLE_PG1_USER_ID":
                        if (int.TryParse(s4bFormControl.fieldValue, out userId))
                        {
                        };
                        break;
                     case "VARIABLE_PG1_DIARY_RESOURCE":
                        userName = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG2_VISIT_STATUS":
                        visitStatusId = Utilities.GetVisitStatusIdByStatusDesc(request, s4bFormControl.fieldValue);
                        break;

                     case "VARIABLE_PG6_DIARY_ENTRY_DATE":
                        if (DateTime.TryParse(s4bFormControl.fieldValue, out appDate))
                        {
                        };
                        break;

                     case "FIELD_PG2_TYPE_OF_ATTENDANCE":
                        typeOfAttendance = s4bFormControl.fieldValue;
                        break;

                     case "VAR_PG4_MATERIAL_SUB_TOTAL":
                        Double.TryParse(s4bFormControl.fieldValue, out subTotalMaterials);
                        break;

                     case "VAR_PG3_SUB_TOTAL":
                        Double.TryParse(s4bFormControl.fieldValue, out subTotalLabour);
                        break;

                     case "VAR_PG4_DISCOUNT":
                        Double.TryParse(s4bFormControl.fieldValue, out subtotalDiscount);
                        break;

                     case "VAR_PG2_DIARY_ENTRY_NOTES":
                        reasonForAttendance = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG3_ROW01_WORKS_REQUIRED":
                        briefDescOfWorks = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG3_ROW01_DESC":
                     case "FIELD_PG3_ROW02_DESC":
                     case "FIELD_PG3_ROW03_DESC":
                     case "FIELD_PG3_ROW04_DESC":
                     case "FIELD_PG3_ROW05_DESC":
                     case "FIELD_PG3_ROW06_DESC":
                     case "FIELD_PG3_ROW07_DESC":
                     case "FIELD_PG3_ROW08_DESC":
                     case "FIELD_PG3_ROW09_DESC":
                     case "FIELD_PG3_ROW10_DESC":
                     case "FIELD_PG3_ROW11_DESC":
                     case "FIELD_PG3_ROW12_DESC":
                     case "FIELD_PG3_ROW13_DESC":
                     case "FIELD_PG3_ROW14_DESC":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        if (index > 0)
                        {
                           descOfWork_itemDesc[index - 1] = s4bFormControl.fieldValue;
                        }
                        break;

                     case "FIELD_PG3_ROW01_AMOUNT":
                     case "FIELD_PG3_ROW02_AMOUNT":
                     case "FIELD_PG3_ROW03_AMOUNT":
                     case "FIELD_PG3_ROW04_AMOUNT":
                     case "FIELD_PG3_ROW05_AMOUNT":
                     case "FIELD_PG3_ROW06_AMOUNT":
                     case "FIELD_PG3_ROW07_AMOUNT":
                     case "FIELD_PG3_ROW08_AMOUNT":
                     case "FIELD_PG3_ROW09_AMOUNT":
                     case "FIELD_PG3_ROW10_AMOUNT":
                     case "FIELD_PG3_ROW11_AMOUNT":
                     case "FIELD_PG3_ROW12_AMOUNT":
                     case "FIELD_PG3_ROW13_AMOUNT":
                     case "FIELD_PG3_ROW14_AMOUNT":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        if (index > 0)
                        {
                           Double.TryParse(s4bFormControl.fieldValue, out descOfWork_amtValue[index - 1]);
                        }
                        break;

                     case "FIELD_PG4_ROW01_MATERIALS":
                     case "FIELD_PG4_ROW02_MATERIALS":
                     case "FIELD_PG4_ROW03_MATERIALS":
                     case "FIELD_PG4_ROW04_MATERIALS":
                     case "FIELD_PG4_ROW05_MATERIALS":
                     case "FIELD_PG4_ROW06_MATERIALS":
                     case "FIELD_PG4_ROW07_MATERIALS":
                     case "FIELD_PG4_ROW08_MATERIALS":
                     case "FIELD_PG4_ROW09_MATERIALS":
                     case "FIELD_PG4_ROW10_MATERIALS":
                     case "FIELD_PG4_ROW11_MATERIALS":
                     case "FIELD_PG4_ROW12_MATERIALS":
                     case "FIELD_PG4_ROW13_MATERIALS":
                     case "FIELD_PG4_ROW14_MATERIALS":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        if (index > 0)
                        {
                           materials_itemDesc[index - 1] = s4bFormControl.fieldValue;
                        }
                        break;

                     case "FIELD_PG4_ROW01_AMOUNT":
                     case "FIELD_PG4_ROW02_AMOUNT":
                     case "FIELD_PG4_ROW03_AMOUNT":
                     case "FIELD_PG4_ROW04_AMOUNT":
                     case "FIELD_PG4_ROW05_AMOUNT":
                     case "FIELD_PG4_ROW06_AMOUNT":
                     case "FIELD_PG4_ROW07_AMOUNT":
                     case "FIELD_PG4_ROW08_AMOUNT":
                     case "FIELD_PG4_ROW09_AMOUNT":
                     case "FIELD_PG4_ROW10_AMOUNT":
                     case "FIELD_PG4_ROW11_AMOUNT":
                     case "FIELD_PG4_ROW12_AMOUNT":
                     case "FIELD_PG4_ROW13_AMOUNT":
                     case "FIELD_PG4_ROW14_AMOUNT":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        if (index > 0)
                        {
                           Double.TryParse(s4bFormControl.fieldValue, out materials_amtValue[index - 1]);
                        }
                        break;

                     case "FIELD_PG4_ROW01_QTY":
                     case "FIELD_PG4_ROW02_QTY":
                     case "FIELD_PG4_ROW03_QTY":
                     case "FIELD_PG4_ROW04_QTY":
                     case "FIELD_PG4_ROW05_QTY":
                     case "FIELD_PG4_ROW06_QTY":
                     case "FIELD_PG4_ROW07_QTY":
                     case "FIELD_PG4_ROW08_QTY":
                     case "FIELD_PG4_ROW09_QTY":
                     case "FIELD_PG4_ROW10_QTY":
                     case "FIELD_PG4_ROW11_QTY":
                     case "FIELD_PG4_ROW12_QTY":
                     case "FIELD_PG4_ROW13_QTY":
                     case "FIELD_PG4_ROW14_QTY":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        if (index > 0)
                        {
                           Double.TryParse(s4bFormControl.fieldValue, out materials_Qty[index - 1]);
                        }
                        break;

                     case "VAR_PG4_SUB_TOTAL":
                        subTotal = s4bFormControl.fieldValue;
                        break;

                     case "VAR_PG4_VAT":
                        vat = s4bFormControl.fieldValue;
                        break;

                     case "VAR_PG4_TOTAL":
                        total = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG5_WORKS_REQUIRED":
                        recommsForWorksRequired = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG6_ROW01_ROOF":
                     case "FIELD_PG6_ROW02_ROOF":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        if (index > 0)
                        {
                           drawingRoof[index - 1] = s4bFormControl.fieldValue;
                        }
                        break;

                     case "FIELD_PG6_ROW01_MEASUREMENTS":
                     case "FIELD_PG6_ROW02_MEASUREMENTS":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        if (index > 0)
                        {
                           drawingMeasurements[index - 1] = s4bFormControl.fieldValue;
                        }
                        break;

                     case "FIELD_PG6_ROW01_NOTES":
                     case "FIELD_PG6_ROW02_NOTES":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        if (index > 0)
                        {
                           drawingNotes[index - 1] = s4bFormControl.fieldValue;
                        }
                        break;

                     case "FIELD_PG7_FLG_PYMT_CASH":
                        Boolean.TryParse(s4bFormControl.fieldValue, out flgPymtCash);
                        break;

                     case "FIELD_PG7_FLG_PYMT_CHEQUE":
                        Boolean.TryParse(s4bFormControl.fieldValue, out flgPymtCheque);
                        break;

                     case "FIELD_PG7_FLG_PYMT_Card":
                        Boolean.TryParse(s4bFormControl.fieldValue, out flgPymtCard);
                        break;

                     case "FIELD_PG7_FLG_PYMT_INVOICE":
                        Boolean.TryParse(s4bFormControl.fieldValue, out flgPymtInvoice);
                        break;

                     case "VARIABLE_PG1_DAIRY_RESOURCE":
                        diaryResourceName = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG2_START_TIME":
                        timeStart = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG2_FINISH_TIME":
                        timeEnd = s4bFormControl.fieldValue;
                        break;
                  }
               }
            }
            if (userId <= 0)
            {
               userId = 1;
            }
            diaryResource = Utilities.GetDiaryResourceByUserId(request, userId);
            OrderItemsRepository orderItemsRepository = new OrderItemsRepository();
            OrdersNotesRepository ordersNotesRepository = new OrdersNotesRepository();
            OrdersNotes ordersNote = new OrdersNotes();
            OrderItems orderItemObj = null;

            /// 1. Schedule Items Rows Added for Mat, Lab and Discount
            /// 
            if (typeOfAttendance.Equals("Emergency Call Out", StringComparison.InvariantCultureIgnoreCase))
            {
               if (subTotalMaterials > 0)
               {
                  orderItemObj = new OrderItems();
                  orderItemObj.JobSequence = jobSequence;
                  orderItemObj.FlgRowIsText = false;
                  orderItemObj.ItemType = 0;
                  orderItemObj.ItemCode = "";
                  orderItemObj.ItemDesc = "Misc. Materials";
                  orderItemObj.ItemUnits = "";
                  orderItemObj.ItemQuantity = 1;
                  orderItemObj.AmountLabour = 0;
                  orderItemObj.AmountMaterials = subTotalMaterials;
                  orderItemObj.AmountPlant = 0;
                  orderItemObj.AmountValue = subTotalMaterials;
                  orderItemObj.AmountTotal = subTotalMaterials;
                  if (diaryResource != null)
                  {
                     orderItemObj.AssignedTo = diaryResource.JoinResource;
                  }
                  orderItemObj.FlgCompleted = true;
                  orderItemObj.FlgDocsRecd = true;
                  orderItemObj.CreatedBy = userId;
                  orderItemObj.DateCreated = timeStamp;
                  orderItemObj.LastAmendedBy = userId;
                  orderItemObj.DateLastAmended = timeStamp;
                  OrderItems orderItemsNew = orderItemsRepository.CreateOrderItems(orderItemObj, request);
                  if (orderItemsNew == null)
                  {
                     //TODO: Log and Report Error
                     //Utilities.ReportError("Unable to Add Material Line for Order Items for Avon Ruby's Roofing V2." + Utilities.Message, method_name, true, system, edwFormInstance);
                  }
               }
               if (subTotalLabour > 0)
               {
                  orderItemObj = new OrderItems();
                  orderItemObj.JobSequence = jobSequence;
                  orderItemObj.FlgRowIsText = false;
                  orderItemObj.ItemType = 0;
                  orderItemObj.ItemCode = "";
                  orderItemObj.ItemDesc = "Labour";
                  orderItemObj.ItemUnits = "";
                  orderItemObj.ItemQuantity = 1;
                  orderItemObj.AmountLabour = 0;
                  orderItemObj.AmountMaterials = subTotalLabour;
                  orderItemObj.AmountPlant = 0;
                  orderItemObj.AmountValue = subTotalLabour;
                  orderItemObj.AmountTotal = subTotalLabour;
                  if (diaryResource != null)
                  {
                     orderItemObj.AssignedTo = diaryResource.JoinResource;
                  }
                  orderItemObj.FlgCompleted = true;
                  orderItemObj.FlgDocsRecd = true;
                  orderItemObj.CreatedBy = userId;
                  orderItemObj.DateCreated = timeStamp;
                  orderItemObj.LastAmendedBy = userId;
                  orderItemObj.DateLastAmended = timeStamp;
                  OrderItems orderItemsNew = orderItemsRepository.CreateOrderItems(orderItemObj, request);
                  if (orderItemsNew == null)
                  {
                     //TODO: Log and Report Error
                     //Utilities.ReportError("Unable to Add Labour Line for Order Items for Avon Ruby's Roofing V2." + Utilities.Message, method_name, true, system, edwFormInstance);
                  }
               }
               if (subtotalDiscount > 0)
               {
                  orderItemObj = new OrderItems();
                  orderItemObj.JobSequence = jobSequence;
                  orderItemObj.FlgRowIsText = false;
                  orderItemObj.ItemType = 0;
                  orderItemObj.ItemCode = "";
                  orderItemObj.ItemDesc = "Labour Discount";
                  orderItemObj.ItemUnits = "";
                  orderItemObj.ItemQuantity = 1;
                  orderItemObj.AmountLabour = 0;
                  orderItemObj.AmountMaterials = 1 * subtotalDiscount;
                  orderItemObj.AmountPlant = 0;
                  orderItemObj.AmountValue = 1 * subtotalDiscount;
                  orderItemObj.AmountTotal = 1 * subtotalDiscount;
                  if (diaryResource != null)
                  {
                     orderItemObj.AssignedTo = diaryResource.JoinResource;
                  }
                  orderItemObj.FlgCompleted = true;
                  orderItemObj.FlgDocsRecd = true;
                  orderItemObj.CreatedBy = userId;
                  orderItemObj.DateCreated = timeStamp;
                  orderItemObj.LastAmendedBy = userId;
                  orderItemObj.DateLastAmended = timeStamp;
                  OrderItems orderItemsNew = orderItemsRepository.CreateOrderItems(orderItemObj, request);
                  if (orderItemsNew == null)
                  {
                     //TODO: Log and Report Error
                     //Utilities.ReportError("Unable to Add Labour Discount for Order Items for Avon Ruby's Roofing V2." + Utilities.Message, method_name, true, system, edwFormInstance);
                  }
               }
            }

            /// 2. Visit Status Update
            if (appSequence > 0)
            {
               DiaryAppsRepository diaryAppsRepository = new DiaryAppsRepository();
               if (visitStatusId >= 0)
               {
                  if (!diaryAppsRepository.updateVisitStatusAndFlgCompletedBySequence(request, appSequence, visitStatusId, true, userId, timeStamp))
                  {
                     //TODO: Log and Report Error
                     //    //Utilities.ReportError("Unable to Update Diary Visit Status" + Utilities.Message, method_name, true, system, edwFormInstance);
                  }
               }
            }

            /// Description of Works
            string descOfWorks = "Description Of Works\r\n";
            for (int counter = 0; counter < 14; counter++)
            {
               if (descOfWork_amtValue[counter] > 0 ||
                   (descOfWork_itemDesc[counter] != "" && descOfWork_itemDesc[counter] != null))
               {
                  descOfWorks = descOfWorks + descOfWork_itemDesc[counter];
                  if (descOfWork_amtValue[counter] != 0)
                  {
                     descOfWorks = descOfWorks + " - Value " + descOfWork_amtValue[counter];
                  }
                  if (descOfWork_amtValue[counter] != 0)
                  {
                     descOfWorks = descOfWorks + " - Value " + descOfWork_amtValue[counter];
                  }
                  descOfWorks = descOfWorks + "\r\n";
               }
            }

            /// Materials Used / Description
            string materialsUsed = "Materials Used/Description\r\n";
            for (int counter = 0; counter < 14; counter++)
            {
               if (materials_amtValue[counter] > 0 ||
                   materials_Qty[counter] > 0 ||
                   (materials_itemDesc[counter] != null && materials_itemDesc[counter] != ""))
               {
                  materialsUsed = materialsUsed + materials_itemDesc[counter];
                  if (materials_amtValue[counter] != 0)
                  {
                     materialsUsed = materialsUsed + " Value: " + materials_amtValue[counter];
                  }
                  materialsUsed = materialsUsed + "\r\n";
               }
            }

            /// Recommendations For Further Work
            if (recommsForWorksRequired != null && recommsForWorksRequired != "")
            {
               if (recommsForWorksRequired == "")
               {
                  recommsForWorksRequired = "RECOMMENDATIONS FOR FURTHER WORK" + "\r\n";
               }
               recommsForWorksRequired = recommsForWorksRequired + "\r\n" + recommsForWorksRequired + "\r\n";
            }

            /// Drawing Section
            string drawingSection = "";
            for (int counter = 0; counter < 2; counter++)
            {
               drawingSection = drawingSection + "\r\nRoof: ";
               if (drawingRoof[counter] != null && drawingRoof[counter] != "")
               {
                  drawingSection = drawingSection + drawingRoof[counter] + "";
               }
               drawingSection = drawingSection + "\r\n";
               drawingSection = drawingSection + "Measurements: ";
               if (drawingMeasurements[counter] != null && drawingMeasurements[counter] != "")
               {
                  drawingSection = drawingSection + drawingMeasurements[counter] + "";
               }
               drawingSection = drawingSection + "\r\n";
               drawingSection = drawingSection + "Notes: ";
               if (drawingNotes[counter] != null && drawingNotes[counter] != "")
               {
                  drawingSection = drawingSection + drawingNotes[counter] + "";
               }
               drawingSection = drawingSection + "\r\n";
            }

            // Order Internal Notes Entry
            string appDateString = appDate == DateTime.MinValue ? "" : appDate.ToString("dd/MM/yyyy");
            string orderInternalNotes = "ROOFING-" + appDateString + " " + diaryResource.ResourceName + "\r\n\r\n";
            orderInternalNotes += diaryResource.ResourceName + " attended " + appDateString + " between " + timeStart + " - " + timeEnd + "\r\n\r\n";
            orderInternalNotes += "Reason for Attendance: " + reasonForAttendance + "\r\n\r\n";
            orderInternalNotes += "Brief Description of Works required: " + briefDescOfWorks + "\r\n\r\n";
            orderInternalNotes += descOfWorks + "\r\n";
            orderInternalNotes += materialsUsed + "\r\n";
            orderInternalNotes += recommsForWorksRequired + "\r\n";
            orderInternalNotes += "Drawing Area Section: " + "\r\n" + drawingSection + "\r\n";
            orderInternalNotes += "Sub total of Labour: " + subTotalLabour + "\r\n";
            orderInternalNotes += "Sub total of Materials: " + subTotalMaterials + "\r\n";
            orderInternalNotes += "Preferential client status and Discount applied to Labour only: " + subtotalDiscount + "\r\n\r\n";
            orderInternalNotes += "Sub Total " + subTotal + "\r\n";
            orderInternalNotes += "VAT " + vat + "\r\n";
            orderInternalNotes += "Total " + total + "\r\n\r\n";
            string paymentType = "";
            if (flgPymtCash)
            {
               paymentType = "Cash";
            }
            else if (flgPymtCard)
            {
               paymentType = "Card";
            }
            else if (flgPymtCheque)
            {
               paymentType = "Cheque";
            }
            else if (flgPymtInvoice)
            {
               paymentType = "Invoice";
            }
            orderInternalNotes += "Payment Type Selected: " + paymentType;

            ordersNote = new OrdersNotes();
            ordersNote.JobSequence = jobSequence;
            ordersNote.OrderNotes = orderInternalNotes;
            ordersNote.CreatedBy = userId;
            ordersNote.DateCreated = timeStamp;
            ordersNote.LastAmendedBy = userId;
            ordersNote.DateLastAmended = timeStamp;
            ordersNote = ordersNotesRepository.Insert(request, ordersNote);
            if (ordersNote == null)
            {
               //TODO: Log and Report Error
               //Utilities.ReportError("Unable to Add Order Internal Notes for Avon Ruby's Roofing V2 " + Utilities.Message, method_name, true, system, edwFormInstance);
            }
            //TODO: Log 
            //log.Info("Finished Processing Data for AvonRuby's Roofing V2 with Template Id '" + edwFormInstance.TemplateId + "'");
            returnValue = true;
         }
         catch (Exception ex)
         {
            Message = "Unable to Process Avon Ruby Worksheet 1. Exception: " + ex.Message;
         }
         return returnValue;
      }

		// Avon Ruby - Estimate V06 "735722830"
		private bool ProcessAvonRubyEstimateV06(HttpRequest request, S4BFormRequest s4bFormRequest, S4bFormSubmissions s4bFormSubmission,
											  RootObject root, Dictionary<string, S4BFormsControl> s4bFormsControls)
		{
			bool returnValue = false;
			int userId = Utilities.GetUserIdFromRequest(request);
			int index = -1, pageindex = -1;

			long jobSequence = -1;
			long appSequence = -1;
			string userName = "";
			const int NO_OF_MATERIALS_ROWS = 75;
			const int NO_OF_DRAWING_ROWS = 4;
			string[] materials_desc = new string[NO_OF_MATERIALS_ROWS];
			string[] materials_unit = new string[NO_OF_MATERIALS_ROWS];
			double[] materials_price = new double[NO_OF_MATERIALS_ROWS];
			double[] materials_qty = new double[NO_OF_MATERIALS_ROWS];
			string[] drawing_room = new string[NO_OF_DRAWING_ROWS];
			string[] drawing_floor = new string[NO_OF_DRAWING_ROWS];
			string[] drawing_notes = new string[NO_OF_DRAWING_ROWS];
			string furtherInfo = "";
			DateTime? timeStamp = s4bFormSubmission.DateCreated;
			DateTime appDate = DateTime.Now;
			Dictionary<string, string> imageValues = new Dictionary<string, string>();
			try
			{
				foreach (KeyValuePair<string, S4BFormsControl> s4bFormControlEntry in s4bFormsControls)
				{
					S4BFormsControl s4bFormControl = s4bFormControlEntry.Value;
					if (s4bFormControl.fieldValue != null)
					{
						switch (s4bFormControlEntry.Key)
						{
							case "VARIABLE_PG1_JOB_SEQUENCE":
								if (long.TryParse(s4bFormControl.fieldValue, out jobSequence))
								{
								};
								break;
							case "VARIABLE_PG1_DIARY_ENTRY_ID":
								long.TryParse(s4bFormControl.fieldValue, out appSequence);
								break;
							case "VARIABLE_PG1_USER_ID":
								if (int.TryParse(s4bFormControl.fieldValue, out userId))
								{
								};
								break;
							case "VARIABLE_PG1_DIARY_RESOURCE":
								userName = s4bFormControl.fieldValue;
								break;

							case "FIELD_PG1_FURTHER_INFORMATION":
								furtherInfo = s4bFormControl.fieldValue;
								break;
							case "FIELD_PG2_WORKS_REQUIRED":
								//worksRequiredPg2 = s4bFormControl.fieldValue;
								break;
							case "FIELD_PG3_WORKS_REQUIRED":
								//worksRequiredPg3 = s4bFormControl.fieldValue;
								break;
							case "FIELD_PG4_WORKS_REQUIRED":
								//worksRequiredPg4 = s4bFormControl.fieldValue;
								break;

							case "FIELD_PG5_ROW01_MATERIALS":
							case "FIELD_PG5_ROW02_MATERIALS":
							case "FIELD_PG5_ROW03_MATERIALS":
							case "FIELD_PG5_ROW04_MATERIALS":
							case "FIELD_PG5_ROW05_MATERIALS":
							case "FIELD_PG5_ROW06_MATERIALS":
							case "FIELD_PG5_ROW07_MATERIALS":
							case "FIELD_PG5_ROW08_MATERIALS":
							case "FIELD_PG5_ROW09_MATERIALS":
							case "FIELD_PG5_ROW10_MATERIALS":
							case "FIELD_PG5_ROW11_MATERIALS":
							case "FIELD_PG5_ROW12_MATERIALS":
							case "FIELD_PG5_ROW13_MATERIALS":
							case "FIELD_PG5_ROW14_MATERIALS":
							case "FIELD_PG5_ROW15_MATERIALS":
							case "FIELD_PG5_ROW16_MATERIALS":
							case "FIELD_PG5_ROW17_MATERIALS":
							case "FIELD_PG5_ROW18_MATERIALS":
							case "FIELD_PG5_ROW19_MATERIALS":
							case "FIELD_PG5_ROW20_MATERIALS":
							case "FIELD_PG5_ROW21_MATERIALS":
							case "FIELD_PG5_ROW22_MATERIALS":
							case "FIELD_PG5_ROW23_MATERIALS":
							case "FIELD_PG5_ROW24_MATERIALS":
							case "FIELD_PG6_ROW01_MATERIALS":
							case "FIELD_PG6_ROW02_MATERIALS":
							case "FIELD_PG6_ROW03_MATERIALS":
							case "FIELD_PG6_ROW04_MATERIALS":
							case "FIELD_PG6_ROW05_MATERIALS":
							case "FIELD_PG6_ROW06_MATERIALS":
							case "FIELD_PG6_ROW07_MATERIALS":
							case "FIELD_PG6_ROW08_MATERIALS":
							case "FIELD_PG6_ROW09_MATERIALS":
							case "FIELD_PG6_ROW10_MATERIALS":
							case "FIELD_PG6_ROW11_MATERIALS":
							case "FIELD_PG6_ROW12_MATERIALS":
							case "FIELD_PG6_ROW13_MATERIALS":
							case "FIELD_PG6_ROW14_MATERIALS":
							case "FIELD_PG6_ROW15_MATERIALS":
							case "FIELD_PG6_ROW16_MATERIALS":
							case "FIELD_PG6_ROW17_MATERIALS":
							case "FIELD_PG6_ROW18_MATERIALS":
							case "FIELD_PG6_ROW19_MATERIALS":
							case "FIELD_PG6_ROW20_MATERIALS":
							case "FIELD_PG6_ROW21_MATERIALS":
							case "FIELD_PG6_ROW22_MATERIALS":
							case "FIELD_PG6_ROW23_MATERIALS":
							case "FIELD_PG6_ROW24_MATERIALS":
							case "FIELD_PG6_ROW25_MATERIALS":
							case "FIELD_PG7_ROW01_MATERIALS":
							case "FIELD_PG7_ROW02_MATERIALS":
							case "FIELD_PG7_ROW03_MATERIALS":
							case "FIELD_PG7_ROW04_MATERIALS":
							case "FIELD_PG7_ROW05_MATERIALS":
							case "FIELD_PG7_ROW06_MATERIALS":
							case "FIELD_PG7_ROW07_MATERIALS":
							case "FIELD_PG7_ROW08_MATERIALS":
							case "FIELD_PG7_ROW09_MATERIALS":
							case "FIELD_PG7_ROW10_MATERIALS":
							case "FIELD_PG7_ROW11_MATERIALS":
							case "FIELD_PG7_ROW12_MATERIALS":
							case "FIELD_PG7_ROW13_MATERIALS":
							case "FIELD_PG7_ROW14_MATERIALS":
							case "FIELD_PG7_ROW15_MATERIALS":
							case "FIELD_PG7_ROW16_MATERIALS":
							case "FIELD_PG7_ROW17_MATERIALS":
							case "FIELD_PG7_ROW18_MATERIALS":
							case "FIELD_PG7_ROW19_MATERIALS":
							case "FIELD_PG7_ROW20_MATERIALS":
							case "FIELD_PG7_ROW21_MATERIALS":
							case "FIELD_PG7_ROW22_MATERIALS":
							case "FIELD_PG7_ROW23_MATERIALS":
							case "FIELD_PG7_ROW24_MATERIALS":
							case "FIELD_PG7_ROW25_MATERIALS":
								pageindex = 5;
								index = -1;
								int.TryParse(s4bFormControlEntry.Key.Substring(8, 1), out pageindex);
								int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
								if (pageindex == 6)
								{
									index = index + 24;
								}
								else if (pageindex == 7)
								{
									index = index + 48;
								}
								if (index > 0)
								{
									materials_desc[index - 1] = s4bFormControl.fieldValue;
								}
								break;
							//case "FIELD_PG5_ROW01_UNITS":
							//case "FIELD_PG5_ROW02_UNITS":
							//case "FIELD_PG5_ROW03_UNITS":
							//case "FIELD_PG5_ROW04_UNITS":
							//case "FIELD_PG5_ROW05_UNITS":
							//case "FIELD_PG5_ROW06_UNITS":
							//case "FIELD_PG5_ROW07_UNITS":
							//case "FIELD_PG5_ROW08_UNITS":
							//case "FIELD_PG5_ROW09_UNITS":
							//case "FIELD_PG5_ROW10_UNITS":
							//case "FIELD_PG5_ROW11_UNITS":
							//case "FIELD_PG5_ROW12_UNITS":
							//case "FIELD_PG5_ROW13_UNITS":
							//case "FIELD_PG5_ROW14_UNITS":
							//case "FIELD_PG5_ROW15_UNITS":
							//case "FIELD_PG5_ROW16_UNITS":
							//case "FIELD_PG5_ROW17_UNITS":
							//case "FIELD_PG5_ROW18_UNITS":
							//case "FIELD_PG5_ROW19_UNITS":
							//case "FIELD_PG5_ROW20_UNITS":
							//case "FIELD_PG5_ROW21_UNITS":
							//case "FIELD_PG5_ROW22_UNITS":
							//case "FIELD_PG5_ROW23_UNITS":
							//case "FIELD_PG5_ROW24_UNITS":
							//case "FIELD_PG6_ROW01_UNITS":
							//case "FIELD_PG6_ROW02_UNITS":
							//case "FIELD_PG6_ROW03_UNITS":
							//case "FIELD_PG6_ROW04_UNITS":
							//case "FIELD_PG6_ROW05_UNITS":
							//case "FIELD_PG6_ROW06_UNITS":
							//case "FIELD_PG6_ROW07_UNITS":
							//case "FIELD_PG6_ROW08_UNITS":
							//case "FIELD_PG6_ROW09_UNITS":
							//case "FIELD_PG6_ROW10_UNITS":
							//case "FIELD_PG6_ROW11_UNITS":
							//case "FIELD_PG6_ROW12_UNITS":
							//case "FIELD_PG6_ROW13_UNITS":
							//case "FIELD_PG6_ROW14_UNITS":
							//case "FIELD_PG6_ROW15_UNITS":
							//case "FIELD_PG6_ROW16_UNITS":
							//case "FIELD_PG6_ROW17_UNITS":
							//case "FIELD_PG6_ROW18_UNITS":
							//case "FIELD_PG6_ROW19_UNITS":
							//case "FIELD_PG6_ROW20_UNITS":
							//case "FIELD_PG6_ROW21_UNITS":
							//case "FIELD_PG6_ROW22_UNITS":
							//case "FIELD_PG6_ROW23_UNITS":
							//case "FIELD_PG6_ROW24_UNITS":
							//case "FIELD_PG6_ROW25_UNITS":
							//case "FIELD_PG7_ROW01_UNITS":
							//case "FIELD_PG7_ROW02_UNITS":
							//case "FIELD_PG7_ROW03_UNITS":
							//case "FIELD_PG7_ROW04_UNITS":
							//case "FIELD_PG7_ROW05_UNITS":
							//case "FIELD_PG7_ROW06_UNITS":
							//case "FIELD_PG7_ROW07_UNITS":
							//case "FIELD_PG7_ROW08_UNITS":
							//case "FIELD_PG7_ROW09_UNITS":
							//case "FIELD_PG7_ROW10_UNITS":
							//case "FIELD_PG7_ROW11_UNITS":
							//case "FIELD_PG7_ROW12_UNITS":
							//case "FIELD_PG7_ROW13_UNITS":
							//case "FIELD_PG7_ROW14_UNITS":
							//case "FIELD_PG7_ROW15_UNITS":
							//case "FIELD_PG7_ROW16_UNITS":
							//case "FIELD_PG7_ROW17_UNITS":
							//case "FIELD_PG7_ROW18_UNITS":
							//case "FIELD_PG7_ROW19_UNITS":
							//case "FIELD_PG7_ROW20_UNITS":
							//case "FIELD_PG7_ROW21_UNITS":
							//case "FIELD_PG7_ROW22_UNITS":
							//case "FIELD_PG7_ROW23_UNITS":
							//case "FIELD_PG7_ROW24_UNITS":
							//case "FIELD_PG7_ROW25_UNITS":
							//    pageindex = 5;
							//    index = -1;
							//    int.TryParse(s4bFormControlEntry.Key.Substring(8, 1), out pageindex);
							//    int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
							//    if (pageindex == 6)
							//    {
							//        index = index + 25;
							//    }
							//    else if (pageindex == 7)
							//    {
							//        index = index + 50;
							//    }
							//    if (index > 0)
							//    {
							//        materials_unit[index - 1] = s4bFormControl.fieldValue;
							//    }
							//    break;
							//case "FIELD_PG5_ROW01_QUANTITY":
							//case "FIELD_PG5_ROW02_QUANTITY":
							//case "FIELD_PG5_ROW03_QUANTITY":
							//case "FIELD_PG5_ROW04_QUANTITY":
							//case "FIELD_PG5_ROW05_QUANTITY":
							//case "FIELD_PG5_ROW06_QUANTITY":
							//case "FIELD_PG5_ROW07_QUANTITY":
							//case "FIELD_PG5_ROW08_QUANTITY":
							//case "FIELD_PG5_ROW09_QUANTITY":
							//case "FIELD_PG5_ROW10_QUANTITY":
							//case "FIELD_PG5_ROW11_QUANTITY":
							//case "FIELD_PG5_ROW12_QUANTITY":
							//case "FIELD_PG5_ROW13_QUANTITY":
							//case "FIELD_PG5_ROW14_QUANTITY":
							//case "FIELD_PG5_ROW15_QUANTITY":
							//case "FIELD_PG5_ROW16_QUANTITY":
							//case "FIELD_PG5_ROW17_QUANTITY":
							//case "FIELD_PG5_ROW18_QUANTITY":
							//case "FIELD_PG5_ROW19_QUANTITY":
							//case "FIELD_PG5_ROW20_QUANTITY":
							//case "FIELD_PG5_ROW21_QUANTITY":
							//case "FIELD_PG5_ROW22_QUANTITY":
							//case "FIELD_PG5_ROW23_QUANTITY":
							//case "FIELD_PG5_ROW24_QUANTITY":
							//case "FIELD_PG6_ROW01_QUANTITY":
							//case "FIELD_PG6_ROW02_QUANTITY":
							//case "FIELD_PG6_ROW03_QUANTITY":
							//case "FIELD_PG6_ROW04_QUANTITY":
							//case "FIELD_PG6_ROW05_QUANTITY":
							//case "FIELD_PG6_ROW06_QUANTITY":
							//case "FIELD_PG6_ROW07_QUANTITY":
							//case "FIELD_PG6_ROW08_QUANTITY":
							//case "FIELD_PG6_ROW09_QUANTITY":
							//case "FIELD_PG6_ROW10_QUANTITY":
							//case "FIELD_PG6_ROW11_QUANTITY":
							//case "FIELD_PG6_ROW12_QUANTITY":
							//case "FIELD_PG6_ROW13_QUANTITY":
							//case "FIELD_PG6_ROW14_QUANTITY":
							//case "FIELD_PG6_ROW15_QUANTITY":
							//case "FIELD_PG6_ROW16_QUANTITY":
							//case "FIELD_PG6_ROW17_QUANTITY":
							//case "FIELD_PG6_ROW18_QUANTITY":
							//case "FIELD_PG6_ROW19_QUANTITY":
							//case "FIELD_PG6_ROW20_QUANTITY":
							//case "FIELD_PG6_ROW21_QUANTITY":
							//case "FIELD_PG6_ROW22_QUANTITY":
							//case "FIELD_PG6_ROW23_QUANTITY":
							//case "FIELD_PG6_ROW24_QUANTITY":
							//case "FIELD_PG6_ROW25_QUANTITY":
							//case "FIELD_PG7_ROW01_QUANTITY":
							//case "FIELD_PG7_ROW02_QUANTITY":
							//case "FIELD_PG7_ROW03_QUANTITY":
							//case "FIELD_PG7_ROW04_QUANTITY":
							//case "FIELD_PG7_ROW05_QUANTITY":
							//case "FIELD_PG7_ROW06_QUANTITY":
							//case "FIELD_PG7_ROW07_QUANTITY":
							//case "FIELD_PG7_ROW08_QUANTITY":
							//case "FIELD_PG7_ROW09_QUANTITY":
							//case "FIELD_PG7_ROW10_QUANTITY":
							//case "FIELD_PG7_ROW11_QUANTITY":
							//case "FIELD_PG7_ROW12_QUANTITY":
							//case "FIELD_PG7_ROW13_QUANTITY":
							//case "FIELD_PG7_ROW14_QUANTITY":
							//case "FIELD_PG7_ROW15_QUANTITY":
							//case "FIELD_PG7_ROW16_QUANTITY":
							//case "FIELD_PG7_ROW17_QUANTITY":
							//case "FIELD_PG7_ROW18_QUANTITY":
							//case "FIELD_PG7_ROW19_QUANTITY":
							//case "FIELD_PG7_ROW20_QUANTITY":
							//case "FIELD_PG7_ROW21_QUANTITY":
							//case "FIELD_PG7_ROW22_QUANTITY":
							//case "FIELD_PG7_ROW23_QUANTITY":
							//case "FIELD_PG7_ROW24_QUANTITY":
							//case "FIELD_PG7_ROW25_QUANTITY":
							//    pageindex = 5;
							//    index = -1;
							//    int.TryParse(s4bFormControlEntry.Key.Substring(8, 1), out pageindex);
							//    int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
							//    if (pageindex == 6)
							//    {
							//        index = index + 25;
							//    }
							//    else if (pageindex == 7)
							//    {
							//        index = index + 50;
							//    }
							//    if (index > 0)
							//    {
							//        Double.TryParse(s4bFormControl.fieldValue, out materials_qty[index - 1]);
							//    }
							//    break;
							case "FIELD_PG5_ROW01_PRICE":
							case "FIELD_PG5_ROW02_PRICE":
							case "FIELD_PG5_ROW03_PRICE":
							case "FIELD_PG5_ROW04_PRICE":
							case "FIELD_PG5_ROW05_PRICE":
							case "FIELD_PG5_ROW06_PRICE":
							case "FIELD_PG5_ROW07_PRICE":
							case "FIELD_PG5_ROW08_PRICE":
							case "FIELD_PG5_ROW09_PRICE":
							case "FIELD_PG5_ROW10_PRICE":
							case "FIELD_PG5_ROW11_PRICE":
							case "FIELD_PG5_ROW12_PRICE":
							case "FIELD_PG5_ROW13_PRICE":
							case "FIELD_PG5_ROW14_PRICE":
							case "FIELD_PG5_ROW15_PRICE":
							case "FIELD_PG5_ROW16_PRICE":
							case "FIELD_PG5_ROW17_PRICE":
							case "FIELD_PG5_ROW18_PRICE":
							case "FIELD_PG5_ROW19_PRICE":
							case "FIELD_PG5_ROW20_PRICE":
							case "FIELD_PG5_ROW21_PRICE":
							case "FIELD_PG5_ROW22_PRICE":
							case "FIELD_PG5_ROW23_PRICE":
							case "FIELD_PG5_ROW24_PRICE":
							case "FIELD_PG6_ROW01_PRICE":
							case "FIELD_PG6_ROW02_PRICE":
							case "FIELD_PG6_ROW03_PRICE":
							case "FIELD_PG6_ROW04_PRICE":
							case "FIELD_PG6_ROW05_PRICE":
							case "FIELD_PG6_ROW06_PRICE":
							case "FIELD_PG6_ROW07_PRICE":
							case "FIELD_PG6_ROW08_PRICE":
							case "FIELD_PG6_ROW09_PRICE":
							case "FIELD_PG6_ROW10_PRICE":
							case "FIELD_PG6_ROW11_PRICE":
							case "FIELD_PG6_ROW12_PRICE":
							case "FIELD_PG6_ROW13_PRICE":
							case "FIELD_PG6_ROW14_PRICE":
							case "FIELD_PG6_ROW15_PRICE":
							case "FIELD_PG6_ROW16_PRICE":
							case "FIELD_PG6_ROW17_PRICE":
							case "FIELD_PG6_ROW18_PRICE":
							case "FIELD_PG6_ROW19_PRICE":
							case "FIELD_PG6_ROW20_PRICE":
							case "FIELD_PG6_ROW21_PRICE":
							case "FIELD_PG6_ROW22_PRICE":
							case "FIELD_PG6_ROW23_PRICE":
							case "FIELD_PG6_ROW24_PRICE":
							case "FIELD_PG6_ROW25_PRICE":
							case "FIELD_PG7_ROW01_PRICE":
							case "FIELD_PG7_ROW02_PRICE":
							case "FIELD_PG7_ROW03_PRICE":
							case "FIELD_PG7_ROW04_PRICE":
							case "FIELD_PG7_ROW05_PRICE":
							case "FIELD_PG7_ROW06_PRICE":
							case "FIELD_PG7_ROW07_PRICE":
							case "FIELD_PG7_ROW08_PRICE":
							case "FIELD_PG7_ROW09_PRICE":
							case "FIELD_PG7_ROW10_PRICE":
							case "FIELD_PG7_ROW11_PRICE":
							case "FIELD_PG7_ROW12_PRICE":
							case "FIELD_PG7_ROW13_PRICE":
							case "FIELD_PG7_ROW14_PRICE":
							case "FIELD_PG7_ROW15_PRICE":
							case "FIELD_PG7_ROW16_PRICE":
							case "FIELD_PG7_ROW17_PRICE":
							case "FIELD_PG7_ROW18_PRICE":
							case "FIELD_PG7_ROW19_PRICE":
							case "FIELD_PG7_ROW20_PRICE":
							case "FIELD_PG7_ROW21_PRICE":
							case "FIELD_PG7_ROW22_PRICE":
							case "FIELD_PG7_ROW23_PRICE":
							case "FIELD_PG7_ROW24_PRICE":
								pageindex = 5;
								index = -1;
								int.TryParse(s4bFormControlEntry.Key.Substring(8, 1), out pageindex);
								int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
								if (pageindex == 6)
								{
									index = index + 24;
								}
								else if (pageindex == 7)
								{
									index = index + 48;
								}
								if (index > 0)
								{
									materials_price[index - 1] = s4bFormControl.fieldValue != "" ? Convert.ToDouble(s4bFormControl.fieldValue != "") : 0;
								}
								break;
							case "FIELD_PG8_ROW01_COL01_ROOM":
							case "FIELD_PG8_ROW01_COL02_ROOM":
							case "FIELD_PG8_ROW02_COL01_ROOM":
							case "FIELD_PG8_ROW02_COL02_ROOM":
								index = getIndexFromAvonRubyEstimateV06PageNoRowNoColNo(s4bFormControlEntry.Key);
								drawing_room[index - 1] = s4bFormControl.fieldValue;
								break;

							case "FIELD_PG8_ROW01_COL01_FLOOR":
							case "FIELD_PG8_ROW01_COL02_FLOOR":
							case "FIELD_PG8_ROW02_COL01_FLOOR":
							case "FIELD_PG8_ROW02_COL02_FLOOR":
								index = getIndexFromAvonRubyEstimateV06PageNoRowNoColNo(s4bFormControlEntry.Key);
								drawing_floor[index - 1] = s4bFormControl.fieldValue;
								break;

							case "FIELD_PG8_ROW01_COL01_NOTES":
							case "FIELD_PG8_ROW01_COL02_NOTES":
							case "FIELD_PG8_ROW02_COL01_NOTES":
							case "FIELD_PG8_ROW02_COL02_NOTES":
								index = getIndexFromAvonRubyEstimateV06PageNoRowNoColNo(s4bFormControlEntry.Key);
								drawing_notes[index - 1] = s4bFormControl.fieldValue;
								break;
						}
					}
				}
				if (userId <= 0)
				{
					userId = 1;
				}
				DiaryResources diaryResource = Utilities.GetDiaryResourceByUserId(request, userId);
				DiaryApps diaryApp = null;
				/// Visit Status Update
				if (appSequence > 0)
				{
					DiaryAppsRepository diaryAppsRepository = new DiaryAppsRepository();
					diaryApp = diaryAppsRepository.GetDiaryAppsBySequence(appSequence, request, null);
					if (!diaryAppsRepository.updateVisitStatusAndFlgCompletedBySequence(request, appSequence, -1, true, userId, timeStamp))
					{
						//TODO: Log and Report Error
						//Utilities.ReportError("Unable to Update Diary Visit Status" + Utilities.Message, method_name, true, system, edwFormInstance);
					}
				}


				/// Brief description of Works Required
				//string descriptionOfWorks = "Brief description of Works Required:";
				//if (!string.IsNullOrEmpty(worksRequiredPg2))
				//{
				//    descriptionOfWorks = descriptionOfWorks + "\r\n" + worksRequiredPg2;
				//}
				//if (!string.IsNullOrEmpty(worksRequiredPg3))
				//{
				//    descriptionOfWorks = descriptionOfWorks + "\r\n" + worksRequiredPg3;
				//}
				//if (!string.IsNullOrEmpty(worksRequiredPg4))
				//{
				//    descriptionOfWorks = descriptionOfWorks + "\r\n" + worksRequiredPg4;
				//}

				//Materials Required
				string materialsRequired = "Materials Required:";
				for (int counter = 0; counter < NO_OF_MATERIALS_ROWS; counter++)
				{
					if (materials_qty[counter] > 0 ||
						(materials_desc[counter] != null && materials_desc[counter] != "") ||
						(materials_unit[counter] != null && materials_unit[counter] != ""))
					{
						materialsRequired = materialsRequired + "\r\n" + materials_desc[counter];
						//materialsRequired = materialsRequired + " Unit: " + materials_unit[counter];
						//if (materials_qty[counter] != 0)
						//{
						//    materialsRequired = materialsRequired + " Qty: " + materials_qty[counter];
						//}
					}
				}

				/// Drawing Section
				string drawingSection = "Drawing Area:";
				for (int counter = 0; counter < NO_OF_DRAWING_ROWS; counter++)
				{
					if ((drawing_notes[counter] != null && drawing_notes[counter] != "") ||
						(drawing_floor[counter] != null && drawing_floor[counter] != "") ||
						(drawing_room[counter] != null && drawing_room[counter] != ""))
					{
						drawingSection = drawingSection + "\r\n";
						drawingSection = drawingSection + "Floor: " + (string.IsNullOrEmpty(drawing_floor[counter]) ? "" : drawing_floor[counter]);
						drawingSection = drawingSection + " Room: " + (string.IsNullOrEmpty(drawing_room[counter]) ? "" : drawing_room[counter]);
						drawingSection = drawingSection + " Notes: " + (string.IsNullOrEmpty(drawing_notes[counter]) ? "" : drawing_notes[counter]);
					}
				}

				// Order Internal Notes Entry
				string appDateString = appDate == DateTime.MinValue ? "" : appDate.ToString("dd/MM/yyyy");
				string timeStart = diaryApp.DateAppStart.Value.ToString("HH:mm");
				string timeEnd = diaryApp.DateAppEnd.Value.ToString("HH:mm");
				string orderInternalNotes = "ESTIMATE-" + appDateString + " " + diaryResource.ResourceName + "\r\n\r\n";
				orderInternalNotes += diaryResource.ResourceName + " attended " + appDateString + " between " + timeStart + " - " + timeEnd + "\r\n\r\n";
				orderInternalNotes += "Further information or any other hazards:\r\n" + furtherInfo + "\r\n\r\n";
				//orderInternalNotes += descriptionOfWorks + "\r\n\r\n";
				orderInternalNotes += materialsRequired + "\r\n\r\n";
				orderInternalNotes += drawingSection;
				OrdersNotes ordersNote = new OrdersNotes();
				ordersNote.JobSequence = jobSequence;
				ordersNote.OrderNotes = orderInternalNotes;
				ordersNote.CreatedBy = userId;
				ordersNote.DateCreated = timeStamp;
				ordersNote.LastAmendedBy = userId;
				ordersNote.DateLastAmended = timeStamp;
				OrdersNotesRepository ordersNotesRepository = new OrdersNotesRepository();
				ordersNote = ordersNotesRepository.Insert(request, ordersNote);
				if (ordersNote == null)
				{
					//ordersNotesRepository.Message
					//TODO: Log and Report Error
					//Utilities.ReportError("Unable to Add Order Internal Notes for Avon Ruby's Roofing V2 " + Utilities.Message, method_name, true, system, edwFormInstance);
				}



				/// 1. Schedule Items Rows Added for Materials
				/// ///
				OrderItemsRepository orderItemsRepository = new OrderItemsRepository();
				OrderItems orderItemObj = null;
				for (int counter = 0; counter < NO_OF_MATERIALS_ROWS; counter++)
				{
					if (materials_price[counter] > 0 ||
						(materials_desc[counter] != null && materials_desc[counter] != ""))
					{
						orderItemObj = new OrderItems();
						orderItemObj.JobSequence = jobSequence;
						orderItemObj.FlgRowIsText = false;
						orderItemObj.TransType = "B";
						orderItemObj.ItemType = 0;
						orderItemObj.ItemCode = "";
						orderItemObj.ItemDesc = materials_desc[counter];
						orderItemObj.ItemUnits = "";
						orderItemObj.ItemQuantity = 1;
						orderItemObj.AmountLabour = 0;
						orderItemObj.AmountMaterials = materials_price[counter];
						orderItemObj.AmountPlant = 0;
						orderItemObj.AmountValue = materials_price[counter];
						orderItemObj.AmountTotal = materials_price[counter];
						if (diaryResource != null)
						{
							orderItemObj.AssignedTo = diaryResource.JoinResource;
						}
						orderItemObj.FlgCompleted = true;
						orderItemObj.FlgDocsRecd = true;
						orderItemObj.CreatedBy = userId;
						orderItemObj.DateCreated = timeStamp;
						orderItemObj.LastAmendedBy = userId;
						orderItemObj.DateLastAmended = timeStamp;
						OrderItems orderItemsNew = orderItemsRepository.CreateOrderItems(orderItemObj, request);
						if (orderItemsNew == null)
						{
							//TODO: Log and Report Error
							Utilities.WriteLog("Unable to Add Material Line for Order Items for Avon Ruby's Roofing Job Sheet with Template V09." + Utilities.Message);
						}
					}
				}
				//TODO: Log 
				//log.Info("Finished Processing Data for AvonRuby Job Sheet with Template Id '" + edwFormInstance.TemplateId + "'");
				Utilities.WriteLog("Finished Processing Data for AvonRuby Job Sheet with Template V09");
				returnValue = true;
			}
			catch (Exception ex)
			{
				Message = "Unable to Process Avon Ruby Estimate V09. Exception: " + ex.Message;
				Utilities.WriteLog(Message);
			}
			return returnValue;
		}

		// Avon Ruby - Estimate V03 "984083642" 
		private bool ProcessAvonRubyEstimateV03(HttpRequest request, S4BFormRequest s4bFormRequest, S4bFormSubmissions s4bFormSubmission,
											  RootObject root, Dictionary<string, S4BFormsControl> s4bFormsControls)
		{
			bool returnValue = false;
			int userId = Utilities.GetUserIdFromRequest(request);
			int index = -1;

			long jobSequence = -1;
			long appSequence = -1;
			string userName = "", worksRequiredPg2 = "", worksRequiredPg3 = "", worksRequiredPg4 = "";
			const int NO_OF_MATERIALS_ROWS = 19;
			const int NO_OF_DRAWING_ROWS = 12;
			string[] materials_desc = new string[NO_OF_MATERIALS_ROWS];
			string[] materials_unit = new string[NO_OF_MATERIALS_ROWS];
			double[] materials_qty = new double[NO_OF_MATERIALS_ROWS];
			string[] drawing_room = new string[NO_OF_DRAWING_ROWS];
			string[] drawing_floor = new string[NO_OF_DRAWING_ROWS];
			string[] drawing_notes = new string[NO_OF_DRAWING_ROWS];
			string furtherInfo = "";
			DateTime? timeStamp = s4bFormSubmission.DateCreated;
			DateTime appDate = DateTime.Now;
			Dictionary<string, string> imageValues = new Dictionary<string, string>();
			Utilities.WriteLog("Enter in Avon Ruby - Estimate V03 984083642");
			try
			{
				foreach (KeyValuePair<string, S4BFormsControl> s4bFormControlEntry in s4bFormsControls)
				{
					S4BFormsControl s4bFormControl = s4bFormControlEntry.Value;
					if (s4bFormControl.fieldValue != null)
					{
						switch (s4bFormControlEntry.Key)
						{
							case "VARIABLE_PG1_JOB_SEQUENCE":
								if (long.TryParse(s4bFormControl.fieldValue, out jobSequence))
								{
								};
								break;
							case "VARIABLE_PG1_DIARY_ENTRY_ID":
								long.TryParse(s4bFormControl.fieldValue, out appSequence);
								break;
							case "VARIABLE_PG1_USER_ID":
								if (int.TryParse(s4bFormControl.fieldValue, out userId))
								{
								};
								break;
							case "VARIABLE_PG1_DIARY_RESOURCE":
								userName = s4bFormControl.fieldValue;
								break;

							case "FIELD_PG1_FURTHER_INFORMATION":
								furtherInfo = s4bFormControl.fieldValue;
								break;
							case "FIELD_PG2_WORKS_REQUIRED":
								//worksRequiredPg2 = s4bFormControl.fieldValue;
								break;
							case "FIELD_PG3_WORKS_REQUIRED":
								//worksRequiredPg3 = s4bFormControl.fieldValue;
								break;
							case "FIELD_PG4_WORKS_REQUIRED":
								//worksRequiredPg4 = s4bFormControl.fieldValue;
								break;

							case "FIELD_PG5_ROW01_MATERIALS":
							case "FIELD_PG5_ROW02_MATERIALS":
							case "FIELD_PG5_ROW03_MATERIALS":
							case "FIELD_PG5_ROW04_MATERIALS":
							case "FIELD_PG5_ROW05_MATERIALS":
							case "FIELD_PG5_ROW06_MATERIALS":
							case "FIELD_PG5_ROW07_MATERIALS":
							case "FIELD_PG5_ROW08_MATERIALS":
							case "FIELD_PG5_ROW09_MATERIALS":
							case "FIELD_PG5_ROW10_MATERIALS":
							case "FIELD_PG5_ROW11_MATERIALS":
							case "FIELD_PG5_ROW12_MATERIALS":
							case "FIELD_PG5_ROW13_MATERIALS":
							case "FIELD_PG5_ROW14_MATERIALS":
							case "FIELD_PG5_ROW15_MATERIALS":
							case "FIELD_PG5_ROW16_MATERIALS":
							case "FIELD_PG5_ROW17_MATERIALS":
							case "FIELD_PG5_ROW18_MATERIALS":
							case "FIELD_PG5_ROW19_MATERIALS":
								index = -1;
								int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
								if (index > 0)
								{
									materials_desc[index - 1] = s4bFormControl.fieldValue;
								}
								break;
							case "FIELD_PG5_ROW01_UNITS":
							case "FIELD_PG5_ROW02_UNITS":
							case "FIELD_PG5_ROW03_UNITS":
							case "FIELD_PG5_ROW04_UNITS":
							case "FIELD_PG5_ROW05_UNITS":
							case "FIELD_PG5_ROW06_UNITS":
							case "FIELD_PG5_ROW07_UNITS":
							case "FIELD_PG5_ROW08_UNITS":
							case "FIELD_PG5_ROW09_UNITS":
							case "FIELD_PG5_ROW10_UNITS":
							case "FIELD_PG5_ROW11_UNITS":
							case "FIELD_PG5_ROW12_UNITS":
							case "FIELD_PG5_ROW13_UNITS":
							case "FIELD_PG5_ROW14_UNITS":
							case "FIELD_PG5_ROW15_UNITS":
							case "FIELD_PG5_ROW16_UNITS":
							case "FIELD_PG5_ROW17_UNITS":
							case "FIELD_PG5_ROW18_UNITS":
							case "FIELD_PG5_ROW19_UNITS":
								index = -1;
								int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
								if (index > 0)
								{
									materials_unit[index - 1] = s4bFormControl.fieldValue;
								}
								break;
							case "FIELD_PG5_ROW01_QUANTITY":
							case "FIELD_PG5_ROW02_QUANTITY":
							case "FIELD_PG5_ROW03_QUANTITY":
							case "FIELD_PG5_ROW04_QUANTITY":
							case "FIELD_PG5_ROW05_QUANTITY":
							case "FIELD_PG5_ROW06_QUANTITY":
							case "FIELD_PG5_ROW07_QUANTITY":
							case "FIELD_PG5_ROW08_QUANTITY":
							case "FIELD_PG5_ROW09_QUANTITY":
							case "FIELD_PG5_ROW10_QUANTITY":
							case "FIELD_PG5_ROW11_QUANTITY":
							case "FIELD_PG5_ROW12_QUANTITY":
							case "FIELD_PG5_ROW13_QUANTITY":
							case "FIELD_PG5_ROW14_QUANTITY":
							case "FIELD_PG5_ROW15_QUANTITY":
							case "FIELD_PG5_ROW16_QUANTITY":
							case "FIELD_PG5_ROW17_QUANTITY":
							case "FIELD_PG5_ROW18_QUANTITY":
							case "FIELD_PG5_ROW19_QUANTITY":
								index = -1;
								int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
								if (index > 0)
								{
									Double.TryParse(s4bFormControl.fieldValue, out materials_qty[index - 1]);
								}
								break;

							case "FIELD_PG6_ROW01_COL01_ROOM":
							case "FIELD_PG6_ROW01_COL02_ROOM":
							case "FIELD_PG6_ROW02_COL01_ROOM":
							case "FIELD_PG6_ROW02_COL02_ROOM":
							case "FIELD_PG7_ROW01_COL01_ROOM":
							case "FIELD_PG7_ROW01_COL02_ROOM":
							case "FIELD_PG7_ROW02_COL01_ROOM":
							case "FIELD_PG7_ROW02_COL02_ROOM":
							case "FIELD_PG8_ROW01_COL01_ROOM":
							case "FIELD_PG8_ROW01_COL02_ROOM":
							case "FIELD_PG8_ROW02_COL01_ROOM":
							case "FIELD_PG8_ROW02_COL02_ROOM":
								index = getIndexFromCapelEstimatePageNoRowNoColNo(s4bFormControlEntry.Key);
								drawing_room[index - 1] = s4bFormControl.fieldValue;
								break;

							case "FIELD_PG6_ROW01_COL01_FLOOR":
							case "FIELD_PG6_ROW01_COL02_FLOOR":
							case "FIELD_PG6_ROW02_COL01_FLOOR":
							case "FIELD_PG6_ROW02_COL02_FLOOR":
							case "FIELD_PG7_ROW01_COL01_FLOOR":
							case "FIELD_PG7_ROW01_COL02_FLOOR":
							case "FIELD_PG7_ROW02_COL01_FLOOR":
							case "FIELD_PG7_ROW02_COL02_FLOOR":
							case "FIELD_PG8_ROW01_COL01_FLOOR":
							case "FIELD_PG8_ROW01_COL02_FLOOR":
							case "FIELD_PG8_ROW02_COL01_FLOOR":
							case "FIELD_PG8_ROW02_COL02_FLOOR":
								index = getIndexFromCapelEstimatePageNoRowNoColNo(s4bFormControlEntry.Key);
								drawing_floor[index - 1] = s4bFormControl.fieldValue;
								break;

							case "FIELD_PG6_ROW01_COL01_NOTES":
							case "FIELD_PG6_ROW01_COL02_NOTES":
							case "FIELD_PG6_ROW02_COL01_NOTES":
							case "FIELD_PG6_ROW02_COL02_NOTES":
							case "FIELD_PG7_ROW01_COL01_NOTES":
							case "FIELD_PG7_ROW01_COL02_NOTES":
							case "FIELD_PG7_ROW02_COL01_NOTES":
							case "FIELD_PG7_ROW02_COL02_NOTES":
							case "FIELD_PG8_ROW01_COL01_NOTES":
							case "FIELD_PG8_ROW01_COL02_NOTES":
							case "FIELD_PG8_ROW02_COL01_NOTES":
							case "FIELD_PG8_ROW02_COL02_NOTES":
								index = getIndexFromCapelEstimatePageNoRowNoColNo(s4bFormControlEntry.Key);
								drawing_notes[index - 1] = s4bFormControl.fieldValue;
								break;
						}
					}
				}
				if (userId <= 0)
				{
					userId = 1;
				}
				DiaryResources diaryResource = Utilities.GetDiaryResourceByUserId(request, userId);
				DiaryApps diaryApp = null;
				/// Visit Status Update
				if (appSequence > 0)
				{
					DiaryAppsRepository diaryAppsRepository = new DiaryAppsRepository();
					diaryApp = diaryAppsRepository.GetDiaryAppsBySequence(appSequence, request, null);
					Utilities.WriteLog("Update Diary App status and flg Completed:" + diaryApp.Sequence);
					if (!diaryAppsRepository.updateVisitStatusAndFlgCompletedBySequence(request, appSequence, -1, true, userId, timeStamp))
					{
						//TODO: Log and Report Error
						//Utilities.ReportError("Unable to Update Diary Visit Status" + Utilities.Message, method_name, true, system, edwFormInstance);
						Utilities.WriteLog("Unable to Update Diary Visit Status:" + Utilities.Message);
					}
				}


				/// Brief description of Works Required
				//string descriptionOfWorks = "Brief description of Works Required:";
				//if (!string.IsNullOrEmpty(worksRequiredPg2))
				//{
				//    descriptionOfWorks = descriptionOfWorks + "\r\n" + worksRequiredPg2;
				//}
				//if (!string.IsNullOrEmpty(worksRequiredPg3))
				//{
				//    descriptionOfWorks = descriptionOfWorks + "\r\n" + worksRequiredPg3;
				//}
				//if (!string.IsNullOrEmpty(worksRequiredPg4))
				//{
				//    descriptionOfWorks = descriptionOfWorks + "\r\n" + worksRequiredPg4;
				//}

				//Materials Required
				string materialsRequired = "Materials Required:";
				for (int counter = 0; counter < NO_OF_MATERIALS_ROWS; counter++)
				{
					if (materials_qty[counter] > 0 ||
						(materials_desc[counter] != null && materials_desc[counter] != "") ||
						(materials_unit[counter] != null && materials_unit[counter] != ""))
					{
						materialsRequired = materialsRequired + "\r\n" + materials_desc[counter];
						materialsRequired = materialsRequired + " Unit: " + materials_unit[counter];
						if (materials_qty[counter] != 0)
						{
							materialsRequired = materialsRequired + " Qty: " + materials_qty[counter];
						}
					}
				}

				/// Drawing Section
				string drawingSection = "Drawing Area:";
				for (int counter = 0; counter < NO_OF_DRAWING_ROWS; counter++)
				{
					if ((drawing_notes[counter] != null && drawing_notes[counter] != "") ||
						(drawing_floor[counter] != null && drawing_floor[counter] != "") ||
						(drawing_room[counter] != null && drawing_room[counter] != ""))
					{
						drawingSection = drawingSection + "\r\n";
						drawingSection = drawingSection + "Floor: " + (string.IsNullOrEmpty(drawing_floor[counter]) ? "" : drawing_floor[counter]);
						drawingSection = drawingSection + " Room: " + (string.IsNullOrEmpty(drawing_room[counter]) ? "" : drawing_room[counter]);
						drawingSection = drawingSection + " Notes: " + (string.IsNullOrEmpty(drawing_notes[counter]) ? "" : drawing_notes[counter]);
					}
				}

				// Order Internal Notes Entry
				string appDateString = appDate == DateTime.MinValue ? "" : appDate.ToString("dd/MM/yyyy");
				string timeStart = diaryApp.DateAppStart.Value.ToString("HH:mm");
				string timeEnd = diaryApp.DateAppEnd.Value.ToString("HH:mm");
				string orderInternalNotes = "ESTIMATE-" + appDateString + " " + diaryResource.ResourceName + "\r\n\r\n";
				orderInternalNotes += diaryResource.ResourceName + " attended " + appDateString + " between " + timeStart + " - " + timeEnd + "\r\n\r\n";
				orderInternalNotes += "Further information or any other hazards:\r\n" + furtherInfo + "\r\n\r\n";
				Utilities.WriteLog("Order Internal Notes:" + orderInternalNotes);
				//orderInternalNotes += descriptionOfWorks + "\r\n\r\n";
				orderInternalNotes += materialsRequired + "\r\n\r\n";
				orderInternalNotes += drawingSection;
				OrdersNotes ordersNote = new OrdersNotes();
				ordersNote.JobSequence = jobSequence;
				ordersNote.OrderNotes = orderInternalNotes;
				ordersNote.CreatedBy = userId;
				ordersNote.DateCreated = timeStamp;
				ordersNote.LastAmendedBy = userId;
				ordersNote.DateLastAmended = timeStamp;
				OrdersNotesRepository ordersNotesRepository = new OrdersNotesRepository();
				ordersNote = ordersNotesRepository.Insert(request, ordersNote);
				if (ordersNote == null)
				{
					//ordersNotesRepository.Message
					//TODO: Log and Report Error
					//Utilities.ReportError("Unable to Add Order Internal Notes for Avon Ruby's Roofing V2 " + Utilities.Message, method_name, true, system, edwFormInstance);
					Utilities.WriteLog("Unable to Add Order Internal Notes for Avon Ruby Estimate V03" + Utilities.Message);
				}
				//TODO: Log 
				//log.Info("Finished Processing Data for AvonRuby Job Sheet with Template Id '" + edwFormInstance.TemplateId + "'");
				Utilities.WriteLog("Finished Processing Data for AvonRuby Estimate V03 ");

				returnValue = true;
			}
			catch (Exception ex)
			{
				Message = "Unable to Process Avon Ruby Estimate V03. Exception: " + ex.Message;
				Utilities.WriteLog(Message);
			}
			return returnValue;
		}

		private int getIndexFromCapelEstimatePageNoRowNoColNo(string key)
      {
         int returnValue = -1;
         int pageindex = -1;
         int rowNo = -1;
         int colNo = -1;
         int.TryParse(key.Substring(8, 1), out pageindex);
         int.TryParse(key.Substring(13, 2), out rowNo);
         int.TryParse(key.Substring(19, 2), out colNo);
         if (pageindex > 0 && rowNo > 0 && colNo > 0)
         {
            returnValue = 0;
            if (pageindex == 7)
            {
               returnValue = 4;
            }
            else if (pageindex == 8)
            {
               returnValue = 8;
            }
            if (rowNo == 2)
            {
               returnValue++;
            }
            returnValue = returnValue + colNo;
         }
         return returnValue;
      }

      private int getIndexFromAvonRubyEstimateV06PageNoRowNoColNo(string key)
      {
         int returnValue = -1;
         int pageindex = -1;
         int rowNo = -1;
         int colNo = -1;
         int.TryParse(key.Substring(8, 1), out pageindex);
         int.TryParse(key.Substring(13, 2), out rowNo);
         int.TryParse(key.Substring(19, 2), out colNo);
         if (pageindex > 0 && rowNo > 0 && colNo > 0)
         {
            returnValue = 1;
            if (rowNo == 1 && colNo == 2)
            {
               returnValue = 2;
            }
            else if (rowNo == 2 && colNo == 1)
            {
               returnValue = 3;
            }
            else if (rowNo == 2 && colNo == 2)
            {
               returnValue = 4;
            }
         }
         return returnValue;
      }

      // Active Response - Worksheet 1 V16 "360000110"
      // Active Response - Worksheet 1 Avon Ruby V01 "360000145"
      // Active Response - Worksheet Plumber V16 "357000021"
      private bool ProcessActiveResponseWorkshee1(HttpRequest request, S4BFormRequest s4bFormRequest, S4bFormSubmissions s4bFormSubmission,
                                                  RootObject root, Dictionary<string, S4BFormsControl> s4bFormsControls)
      {
         bool returnValue = false;
         int userId = Utilities.GetUserIdFromRequest(request);

         long jobSequence = -1, jobAddressId = -1, jobClientId = -1;
         DiaryResources diaryResource = null;
         int visitStatusId = -1;
         long appSequence = -1;
         string userName = "";
         double[] descOfWork_amtValue = new double[10];
         string[] descOfWork_itemDesc = new string[10];
         string[] furtherInfo = new string[10];
         string location = "", serialNo = "", make = "", gcNo = "", model = "", type = "", condition = "";
         string gpsLocationStartTimeStamp = "", gpsLocationFinishTimeStamp = "", emailAddress = "";
         string diaryResourceName = "", timeStart = "", timeEnd = "", vat = "", subTotal = "", total = "";
         DateTime installDate = DateTime.MinValue, lastServiceDate = DateTime.MinValue;
         string[] recommendations = new string[4];
         double[] matUsed_amtTotal = new double[8];
         string[] matUsed_itemDesc = new string[8];
         double[] matUsed_itemQty = new double[8];
         string[] necessaryRemedial = new string[4];
         string[] defectFound = new string[16];
         bool flgPymtCash = false, flgPymtCheque = false, flgPymtCard = false, flgPymtInvoice = false;
         int index = -1;
         DateTime? timeStamp = s4bFormSubmission.DateCreated;
         DateTime appDate = DateTime.Now;
         Dictionary<string, string> imageValues = new Dictionary<string, string>();
         try
         {
            foreach (KeyValuePair<string, S4BFormsControl> s4bFormControlEntry in s4bFormsControls)
            {
               S4BFormsControl s4bFormControl = s4bFormControlEntry.Value;
               if (s4bFormControl.fieldValue != null)
               {
                  switch (s4bFormControlEntry.Key)
                  {
                     case "VARIABLE_PG1_JOB_SEQUENCE":
                        if (long.TryParse(s4bFormControl.fieldValue, out jobSequence))
                        {
                        };
                        break;
                     case "VARIABLE_PG1_DIARY_ENTRY_ID":
                     case "VARIABLE_PG1_DA_SEQUENCE":
                        long.TryParse(s4bFormControl.fieldValue, out appSequence);
                        break;
                     case "VARIABLE_PG1_JOB_CLIENT_ID":
                        if (long.TryParse(s4bFormControl.fieldValue, out jobClientId))
                        {
                        };
                        break;
                     case "VARIABLE_PG1_JOB_ADDRESS_ID":
                        if (long.TryParse(s4bFormControl.fieldValue, out jobAddressId))
                        {
                        };
                        break;
                     case "VARIABLE_PG1_USER_ID":
                        if (int.TryParse(s4bFormControl.fieldValue, out userId))
                        {
                        };
                        break;
                     case "VARIABLE_PG1_DIARY_RESOURCE":
                     case "VARIABLE_PG2_DIARY_RESOURCE":
                        userName = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG6_VISIT_STAUTS":
                        visitStatusId = Utilities.GetVisitStatusIdByStatusDesc(request, s4bFormControl.fieldValue);
                        break;

                     case "VARIABLE_PG6_DIARY_ENTRY_DATE":
                        if (DateTime.TryParse(s4bFormControl.fieldValue, out appDate))
                        {
                        };
                        break;

                     case "FIELD_PG7_ROW01_DESC":
                     case "FIELD_PG7_ROW02_DESC":
                     case "FIELD_PG7_ROW03_DESC":
                     case "FIELD_PG7_ROW04_DESC":
                     case "FIELD_PG7_ROW05_DESC":
                     case "FIELD_PG7_ROW06_DESC":
                     case "FIELD_PG7_ROW07_DESC":
                     case "FIELD_PG7_ROW08_DESC":
                     case "FIELD_PG7_ROW09_DESC":
                     case "FIELD_PG7_ROW10_DESC":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        if (index > 0)
                        {
                           descOfWork_itemDesc[index - 1] = s4bFormControl.fieldValue;
                        }
                        break;

                     case "VARIABLE_PG7_ROW01_AMT":
                     case "VARIABLE_PG7_ROW02_AMT":
                     case "VARIABLE_PG7_ROW03_AMT":
                     case "VARIABLE_PG7_ROW04_AMT":
                     case "VARIABLE_PG7_ROW05_AMT":
                     case "VARIABLE_PG7_ROW06_AMT":
                     case "VARIABLE_PG7_ROW07_AMT":
                     case "VARIABLE_PG7_ROW08_AMT":
                     case "VARIABLE_PG7_ROW09_AMT":
                     case "VARIABLE_PG7_ROW10_AMT":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        if (index > 0)
                        {
                           Double.TryParse(s4bFormControl.fieldValue, out descOfWork_amtValue[index - 1]);
                        }
                        break;

                     case "FIELD_PG2_ROW01_FURTHER_INFO":
                     case "FIELD_PG2_ROW02_FURTHER_INFO":
                     case "FIELD_PG2_ROW03_FURTHER_INFO":
                     case "FIELD_PG2_ROW04_FURTHER_INFO":
                     case "FIELD_PG2_ROW05_FURTHER_INFO":
                     case "FIELD_PG2_ROW06_FURTHER_INFO":
                     case "FIELD_PG2_ROW07_FURTHER_INFO":
                     case "FIELD_PG2_ROW08_FURTHER_INFO":
                     case "FIELD_PG2_ROW09_FURTHER_INFO":
                     case "FIELD_PG2_ROW10_FURTHER_INFO":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        if (index > 0)
                        {
                           furtherInfo[index - 1] = s4bFormControl.fieldValue;
                        }
                        break;

                     case "FIELD_PG4_LAST_SERVICE_DATE":
                        DateTime.TryParse(s4bFormControl.fieldValue, out lastServiceDate);
                        break;

                     case "FIELD_PG4_CONDITION":
                        condition = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG4_LOCATION":
                        location = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG4_MAKE":
                        make = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG4_MODEL":
                        model = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG4_TYPE":
                        type = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG4_INSTALLATION_DATE":
                        DateTime.TryParse(s4bFormControl.fieldValue, out installDate);
                        break;

                     case "FIELD_PG4_SERIAL_NO":
                        serialNo = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG4_GC_NO":
                        gcNo = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG5_ROW01_WORKS_REQUIRED":
                     case "FIELD_PG5_ROW02_WORKS_REQUIRED":
                     case "FIELD_PG5_ROW03_WORKS_REQUIRED":
                     case "FIELD_PG5_ROW04_WORKS_REQUIRED":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        if (index > 0)
                        {
                           necessaryRemedial[index - 1] = s4bFormControl.fieldValue;
                        }
                        break;

                     case "FIELD_PG7_FLG_PYMT_CASH":
                        Boolean.TryParse(s4bFormControl.fieldValue, out flgPymtCash);
                        break;

                     case "FIELD_PG7_FLG_PYMT_CHEQUE":
                        Boolean.TryParse(s4bFormControl.fieldValue, out flgPymtCheque);
                        break;

                     case "FIELD_PG7_FLG_PYMT_Card":
                        Boolean.TryParse(s4bFormControl.fieldValue, out flgPymtCard);
                        break;

                     case "FIELD_PG7_FLG_PYMT_INVOICE":
                        Boolean.TryParse(s4bFormControl.fieldValue, out flgPymtInvoice);
                        break;

                     case "FIELD_PG8_ROW01_MAT_DETAILS":
                     case "FIELD_PG8_ROW02_MAT_DETAILS":
                     case "FIELD_PG8_ROW03_MAT_DETAILS":
                     case "FIELD_PG8_ROW04_MAT_DETAILS":
                     case "FIELD_PG8_ROW05_MAT_DETAILS":
                     case "FIELD_PG8_ROW06_MAT_DETAILS":
                     case "FIELD_PG8_ROW07_MAT_DETAILS":
                     case "FIELD_PG8_ROW08_MAT_DETAILS":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        if (index > 0)
                        {
                           matUsed_itemDesc[index - 1] = s4bFormControl.fieldValue;
                        }
                        break;

                     case "FIELD_PG8_ROW01_QTY":
                     case "FIELD_PG8_ROW02_QTY":
                     case "FIELD_PG8_ROW03_QTY":
                     case "FIELD_PG8_ROW04_QTY":
                     case "FIELD_PG8_ROW05_QTY":
                     case "FIELD_PG8_ROW06_QTY":
                     case "FIELD_PG8_ROW07_QTY":
                     case "FIELD_PG8_ROW08_QTY":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        if (index > 0)
                        {
                           Double.TryParse(s4bFormControl.fieldValue, out matUsed_itemQty[index - 1]);
                        }
                        break;

                     case "VARIABLE_PG8_ROW01_AMT":
                     case "VARIABLE_PG8_ROW02_AMT":
                     case "VARIABLE_PG8_ROW03_AMT":
                     case "VARIABLE_PG8_ROW04_AMT":
                     case "VARIABLE_PG8_ROW05_AMT":
                     case "VARIABLE_PG8_ROW06_AMT":
                     case "VARIABLE_PG8_ROW07_AMT":
                     case "VARIABLE_PG8_ROW08_AMT":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        if (index > 0)
                        {
                           Double.TryParse(s4bFormControl.fieldValue, out matUsed_amtTotal[index - 1]);
                        }
                        break;

                     case "FIELD_PG8_ROW01_RECOMMENDATIONS":
                     case "FIELD_PG8_ROW02_RECOMMENDATIONS":
                     case "FIELD_PG8_ROW03_RECOMMENDATIONS":
                     case "FIELD_PG8_ROW04_RECOMMENDATIONS":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        if (index > 0)
                        {
                           recommendations[index - 1] = s4bFormControl.fieldValue;
                        }
                        break;

                     case "FIELD_PG1_ROW_01_RA_YES_NO_NA":
                        gpsLocationStartTimeStamp = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG9_FLG_SUBMIT":
                        gpsLocationFinishTimeStamp = s4bFormControl.fieldValue;
                        break;

                     case "VARIABLE_PG3_DIARY_RESOURCE":
                        diaryResourceName = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG6_TIME_START":
                        timeStart = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG6_TIME_END":
                        timeEnd = s4bFormControl.fieldValue;
                        break;

                     case "VARIABLE_PG7_AMT_SUBTOTAL":
                        subTotal = s4bFormControl.fieldValue;
                        break;

                     case "VARIABLE_PG7_AMT_VAT":
                        vat = s4bFormControl.fieldValue;
                        break;

                     case "VARIABLE_PG7_AMT_TOTAL":
                        total = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG9_CLIENT_EMAIL":
                        emailAddress = s4bFormControl.fieldValue;
                        break;

                  }
               }
            }
            if (userId <= 0)
            {
               userId = 1;
            }
            diaryResource = Utilities.GetDiaryResourceByUserId(request, userId);
            OrdersNotesRepository ordersNotesRepository = new OrdersNotesRepository();
            OrdersNotes ordersNote = new OrdersNotes();

            /// 1. Further Information - Order Internal Notes
            string formattedNotes = "";
            for (int counter = 0; counter < 10; counter++)
            {
               if (furtherInfo[counter] != null && furtherInfo[counter].Trim() != "")
               {
                  if (formattedNotes == "")
                  {
                     //formattedNotes = jobSequence + " - " + userName + " - " + timeStamp;
                     formattedNotes = "Further information or any other hazards" + "\r\n";
                  }
                  formattedNotes = formattedNotes + "\r\n" + furtherInfo[counter];
               }
            }
            if (formattedNotes != "")
            {
               ordersNote.JobSequence = jobSequence;
               ordersNote.OrderNotes = formattedNotes;
               ordersNote.CreatedBy = userId;
               ordersNote.DateCreated = timeStamp;
               ordersNote.LastAmendedBy = userId;
               ordersNote.DateLastAmended = timeStamp;
               OrdersNotes ordNoteNew = ordersNotesRepository.Insert(request, ordersNote);
            }

            /// 2. Appliance Details
            if ((type != "" && type != null) ||
                (serialNo != "" && serialNo != null) ||
                (location != "" && location != null) ||
                (make != "" && make != null) ||
                (model != "" && model != null) ||
                (gcNo != "" && gcNo != null) ||
                (condition != "" && condition != null))
            {
               AssetRegister assetRegister = new AssetRegister();
               assetRegister.FlgDeleted = false;
               assetRegister.TransType = SimplicityConstants.ClientTransType;
               assetRegister.EntityId = -1;
               assetRegister.ItemJoinDept = -1;
               assetRegister.ItemJoinCategory = -1;
               assetRegister.ItemJoinSupplementary = -1;
               assetRegister.ItemManufacturer = make;
               assetRegister.ItemModel = model;
               assetRegister.ItemSerialRef = serialNo;
               assetRegister.ItemExtraInfo = type;
               assetRegister.ItemUserField1 = gcNo;
               assetRegister.ItemUserField2 = SimplicityConstants.NotSet;
               assetRegister.ItemUserField3 = SimplicityConstants.NotSet;
               assetRegister.ItemQuantity = 1;
               assetRegister.DateInstalled = installDate;
               assetRegister.DateAcquired = DateTime.MinValue;
               assetRegister.DateDisposed = DateTime.MinValue;
               assetRegister.ItemValueBook = 0;
               assetRegister.ItemValueDepreciation = 0;
               assetRegister.ItemValueDisposal = 0;
               assetRegister.ItemDesc = "";
               assetRegister.ItemAddress = "";
               assetRegister.FlgUseAddressId = false;
               assetRegister.ItemAddressId = -1;
               assetRegister.ItemLocationJoinId = -1;
               assetRegister.ItemLocation = location;
               assetRegister.FlgItemChargeable = false;
               assetRegister.ItemCostMaterialRate = 0;
               assetRegister.ItemCostLabourRate = 0;
               assetRegister.CreatedBy = userId;
               assetRegister.DateCreated = timeStamp;
               assetRegister.LastAmendedBy = userId;
               assetRegister.DateLastAmended = timeStamp;
               AssetRegisterRepository assetRegisterRepository = new AssetRegisterRepository();
               AssetRegister existAssetRegister = assetRegisterRepository.getAssetRegisterByLocationMakeModelTypeSearialNo(request, location, make, model, type, serialNo);
               AssetRegister assetRegisterUpdated = new AssetRegister();
               if (existAssetRegister == null)
               {
                  assetRegisterUpdated = assetRegisterRepository.insert(request, assetRegister);
               }
               else
               {
                  assetRegister.Sequence = existAssetRegister.Sequence;
                  assetRegisterUpdated = assetRegisterRepository.Update(request, assetRegister);
               }
               if (assetRegisterUpdated.Sequence <= 0)
               {
                  //Utilities.ReportError("Unable to Insert/Update Asset Register Record for Import Id " + edwFormInstance.ImpRef + ". Error Message is " + Utilities.Message, method_name, true, system, edwFormInstance);
                  //TODO: Log and Report Error
               }
               else
               {
                  /// Check if it is boiler then insert for supplementary
                  /// 
                  if (type.ToUpper().Equals("COMBI") || type.ToUpper().Equals("BACK BOILER"))
                  {
                     AssetRegisterSuppGas AssetRegisterSuppGasObj = new AssetRegisterSuppGas();
                     AssetRegisterSuppGasObj.Sequence = -1;
                     AssetRegisterSuppGasObj.JoinSequence = assetRegisterUpdated.Sequence;
                     AssetRegisterSuppGasObj.EntityId = -1;
                     AssetRegisterSuppGasObj.AssetGasType = "";
                     AssetRegisterSuppGasObj.FlgGasFixing = false;
                     AssetRegisterSuppGasObj.GasFixing = "";
                     AssetRegisterSuppGasObj.FlgGasType = false;
                     AssetRegisterSuppGasObj.GasType = "";
                     AssetRegisterSuppGasObj.FlgGasFuel = false;
                     AssetRegisterSuppGasObj.GasFuel = "";
                     AssetRegisterSuppGasObj.FlgGasEfficiency = false;
                     AssetRegisterSuppGasObj.GasEfficiency = "";
                     AssetRegisterSuppGasObj.FlgGasFlueType = false;
                     AssetRegisterSuppGasObj.GasFlueType = "";
                     AssetRegisterSuppGasObj.FlgGasFlueing = false;
                     AssetRegisterSuppGasObj.GasFlueing = "";
                     AssetRegisterSuppGasObj.FlgGasOvUvSs = false;
                     AssetRegisterSuppGasObj.GasOvUvSs = "";
                     AssetRegisterSuppGasObj.FlgGasExpansionVessel = false;
                     AssetRegisterSuppGasObj.FlgGasExpansion = false;
                     AssetRegisterSuppGasObj.GasExpansion = "";
                     AssetRegisterSuppGasObj.FlgGasImmersion = false;
                     AssetRegisterSuppGasObj.GasImmersion = "";
                     AssetRegisterSuppGasObj.LastAmendedBy = userId;
                     AssetRegisterSuppGasObj.DateLastAmended = timeStamp;
                     AssetRegisterSuppGasRepository AssetRegisterSuppGasRepos = new AssetRegisterSuppGasRepository();
                     AssetRegisterSuppGas AssetRegisterSuppGasNew = AssetRegisterSuppGasRepos.insert(request, AssetRegisterSuppGasObj);
                     if (AssetRegisterSuppGasNew == null)
                     {
                        //Utilities.ReportError("Unable to Insert/Update Asset Register Supp Record for Import Id " + edwFormInstance.ImpRef + ". Error Message is " + Utilities.Message, method_name, true, system, edwFormInstance);
                        //TODO: Log and Report Error
                     }
                  }
                  AssetRegisterService assetRegisterServiceObj = new AssetRegisterService();
                  assetRegisterServiceObj.Sequence = -1;
                  assetRegisterServiceObj.FlgDeleted = false;
                  assetRegisterServiceObj.AssetSequence = assetRegisterUpdated.Sequence;
                  assetRegisterServiceObj.JobSequence = jobSequence;
                  assetRegisterServiceObj.DaSequence = appSequence;
                  assetRegisterServiceObj.ServiceInitials = "";
                  assetRegisterServiceObj.ServiceNotes = "";
                  assetRegisterServiceObj.ConditionSequence = -1;
                  assetRegisterServiceObj.ServiceBy = userId;
                  assetRegisterServiceObj.FlgNewJobCreated = false;
                  assetRegisterServiceObj.FlgNewApp = false;
                  assetRegisterServiceObj.FlgValidated = false;
                  assetRegisterServiceObj.ValidatedBy = -1;
                  assetRegisterServiceObj.DateValidated = DateTime.MinValue;
                  assetRegisterServiceObj.ItemCostLabourRate = 0;
                  assetRegisterServiceObj.CreatedBy = userId;
                  assetRegisterServiceObj.DateCreated = timeStamp;
                  AssetRegisterService assetRegisterServiceNew = null;
                  AssetRegisterServiceRepository assetRegisterServiceRepos = new AssetRegisterServiceRepository();
                  if (lastServiceDate != null && lastServiceDate != DateTime.MinValue)
                  {
                     assetRegisterServiceObj.FlgNotActive = true;
                     assetRegisterServiceObj.DaAppType = -1;
                     assetRegisterServiceObj.DateDaStart = lastServiceDate;
                     assetRegisterServiceObj.DateService = lastServiceDate;
                     assetRegisterServiceNew = assetRegisterServiceRepos.insert(request, assetRegisterServiceObj);
                  }
                  assetRegisterServiceObj.FlgNotActive = false;
                  assetRegisterServiceObj.DaAppType = 1;
                  assetRegisterServiceObj.DateDaStart = appDate;
                  assetRegisterServiceObj.DateService = appDate;
                  assetRegisterServiceNew = assetRegisterServiceRepos.insert(request, assetRegisterServiceObj);
               }
            }

            /// 3. NECESSARY REMEDIAL WORK REQUIRED
            formattedNotes = "";
            for (int counter = 0; counter < 4; counter++)
            {
               if (necessaryRemedial[counter] != null && necessaryRemedial[counter].Trim() != "")
               {
                  if (formattedNotes == "")
                  {
                     formattedNotes = "Necessary Remedial Work Required" + "\r\n";
                  }
                  formattedNotes = formattedNotes + "\r\n" + necessaryRemedial[counter];
               }
            }
            if (formattedNotes != "")
            {
               ordersNote = new OrdersNotes();
               ordersNote.JobSequence = jobSequence;
               ordersNote.OrderNotes = formattedNotes;
               ordersNote.CreatedBy = userId;
               ordersNote.DateCreated = timeStamp;
               ordersNote.LastAmendedBy = userId;
               ordersNote.DateLastAmended = timeStamp;
               ordersNote = ordersNotesRepository.Insert(request, ordersNote);
               if (ordersNote == null)
               {
                  //    Utilities.ReportError("Unable to Add Order Internal Notes for 'Necessary Remedial Work Required'" + Utilities.Message, method_name, true, system, edwFormInstance);
                  //TODO: Log and Report Error
               }
            }

            /// 4. Visit Status Update
            if (appSequence > 0)
            {
               DiaryAppsRepository diaryAppsRepositoryVisit = new DiaryAppsRepository();
               if (visitStatusId >= 0)
               {
                  if (!diaryAppsRepositoryVisit.updateVisitStatusAndFlgCompletedBySequence(request, appSequence, visitStatusId, true, userId, timeStamp))
                  {
                     //TODO: Log and Report Error
                     //    //Utilities.ReportError("Unable to Update Diary Visit Status" + Utilities.Message, method_name, true, system, edwFormInstance);
                  }
               }
            }

            /// 5. Description of Works
            string descOfWorks = "Description Of Works\r\n";
            bool itemExists = false;
            for (int counter = 0; counter < 10; counter++)
            {
               if (descOfWork_amtValue[counter] > 0 ||
                   (descOfWork_itemDesc[counter] != "" && descOfWork_itemDesc[counter] != null))
               {
                  itemExists = true;
                  descOfWorks = descOfWorks + descOfWork_itemDesc[counter];
                  if (descOfWork_amtValue[counter] != 0)
                  {
                     descOfWorks = descOfWorks + " - Value " + descOfWork_amtValue[counter];
                  }
                  descOfWorks = descOfWorks + "\r\n";
               }
            }
            if (itemExists)
            {
               ordersNote = new OrdersNotes();
               ordersNote.JobSequence = jobSequence;
               ordersNote.OrderNotes = descOfWorks;
               ordersNote.CreatedBy = userId;
               ordersNote.DateCreated = timeStamp;
               ordersNote.LastAmendedBy = userId;
               ordersNote.DateLastAmended = timeStamp;
               ordersNote = ordersNotesRepository.Insert(request, ordersNote);
            }

            /// 7. Materials Used / Description
            string materialsUsed = "Materials Used/Description\r\n";
            itemExists = false;
            for (int counter = 0; counter < 8; counter++)
            {
               if (matUsed_amtTotal[counter] > 0 ||
                   matUsed_itemQty[counter] > 0 ||
                   (matUsed_itemDesc[counter] != "" && matUsed_itemDesc[counter] != null))
               {
                  itemExists = true;
                  double matAmt = 0;
                  if (matUsed_itemQty[counter] != 0)
                  {
                     matAmt = Math.Round(matUsed_amtTotal[counter] / matUsed_itemQty[counter], 2); ;
                  }
                  materialsUsed = materialsUsed + matUsed_itemDesc[counter];
                  if (matUsed_amtTotal[counter] != 0)
                  {
                     materialsUsed = materialsUsed + " Value: " + matUsed_amtTotal[counter];
                  }
                  materialsUsed = materialsUsed + "\r\n";
               }
            }
            if (itemExists)
            {
               ordersNote = new OrdersNotes();
               ordersNote.JobSequence = jobSequence;
               ordersNote.OrderNotes = materialsUsed;
               ordersNote.CreatedBy = userId;
               ordersNote.DateCreated = timeStamp;
               ordersNote.LastAmendedBy = userId;
               ordersNote.DateLastAmended = timeStamp;
               ordersNote = ordersNotesRepository.Insert(request, ordersNote);
            }

            /// 8. Recommendations For Further Work
            formattedNotes = "";
            itemExists = false;
            String recomms = "Recommendations\r\n";
            for (int counter = 0; counter < 4; counter++)
            {
               if (recommendations[counter] != null && recommendations[counter].Trim() != "")
               {
                  itemExists = true;
                  if (formattedNotes == "")
                  {
                     formattedNotes = "RECOMMENDATIONS FOR FURTHER WORK" + "\r\n";
                  }
                  formattedNotes = formattedNotes + "\r\n" + recommendations[counter];
                  recomms = recomms + recommendations[counter] + "\r\n";
               }
            }
            if (formattedNotes != "")
            {
               ordersNote = new OrdersNotes();
               ordersNote.JobSequence = jobSequence;
               ordersNote.OrderNotes = recomms;
               ordersNote.CreatedBy = userId;
               ordersNote.DateCreated = timeStamp;
               ordersNote.LastAmendedBy = userId;
               ordersNote.DateLastAmended = timeStamp;
               ordersNote = ordersNotesRepository.Insert(request, ordersNote);
               if (ordersNote == null)
               {
                  //TODO: Report and Log Error
                  //    Utilities.ReportError("Unable to Add Order Internal Notes for 'Further information or any other hazards'" + Utilities.Message, method_name, true, system, edwFormInstance);
               }
            }

            /// 9. Payment Type Additions into Schedule Items
            if (flgPymtCheque || flgPymtCard || flgPymtCash || flgPymtInvoice)
            {
               string paymentTypes = "";
               if (flgPymtCheque)
               {
                  if (paymentTypes == "")
                  {
                     paymentTypes = "Payment = Cheque";
                  }
                  else
                  {
                     paymentTypes = paymentTypes + " - Cheque";
                  }
               }
               if (flgPymtCard)
               {
                  if (paymentTypes == "")
                  {
                     paymentTypes = "Payment = Card";
                  }
                  else
                  {
                     paymentTypes = paymentTypes + " - Card";
                  }
               }
               if (flgPymtCash)
               {
                  if (paymentTypes == "")
                  {
                     paymentTypes = "Payment = Cash";
                  }
                  else
                  {
                     paymentTypes = paymentTypes + " - Cash";
                  }
               }
               if (flgPymtInvoice)
               {
                  if (paymentTypes == "")
                  {
                     paymentTypes = "Payment = Invoice";
                  }
                  else
                  {
                     paymentTypes = paymentTypes + " - Invoice";
                  }
               }
               ordersNote = new OrdersNotes();
               ordersNote.JobSequence = jobSequence;
               ordersNote.OrderNotes = paymentTypes;
               ordersNote.CreatedBy = userId;
               ordersNote.DateCreated = timeStamp;
               ordersNote.LastAmendedBy = userId;
               ordersNote.DateLastAmended = timeStamp;
               ordersNote = ordersNotesRepository.Insert(request, ordersNote);
               if (ordersNote == null)
               {
                  //TODO: Report and Log Error
                  //    Utilities.ReportError("Unable to Add Order Internal Notes for 'Further information or any other hazards'" + Utilities.Message, method_name, true, system, edwFormInstance);
               }
            }

            //10. Adding order internal notes
            DiaryAppsRepository diaryAppsRepository = new DiaryAppsRepository();
            DiaryApps diaryApp = diaryAppsRepository.GetDiaryAppsBySequence(appSequence, request, null);

            string appDateString = appDate == DateTime.MinValue ? "" : appDate.ToString("dd/MM/yyyy");
            string timeAppStart = diaryApp.DateAppStart.Value.ToString("HH:mm");
            string timeAppEnd = diaryApp.DateAppEnd.Value.ToString("HH:mm");
            string orderInternalNotes = "W.S-" + appDateString + " " + diaryResource.ResourceName + "\r\n\r\n";
            orderInternalNotes += "GPS timestamp start " + gpsLocationStartTimeStamp + " finish " + gpsLocationFinishTimeStamp + "\r\n\r\n";
            orderInternalNotes += diaryResource.ResourceName + " attended " + appDateString + " between " + timeAppStart + " - " + timeAppEnd + "\r\n\r\n";
            orderInternalNotes += "Appliance Details\r\n";
            orderInternalNotes += "Location: " + location + "\r\n";
            orderInternalNotes += "Make: " + make + "\r\n";
            orderInternalNotes += "Model: " + model + "\r\n";
            orderInternalNotes += "Type: " + type + "\r\n";
            orderInternalNotes += "Condition: " + condition + "\r\n";
            orderInternalNotes += "Sreial no: " + serialNo + "\r\n";
            orderInternalNotes += "GC No: " + gcNo + "\r\n";
            orderInternalNotes += "Install Date: " + (installDate == null || installDate.ToString() == "01/01/0001 00:00:00" ? "" : ((DateTime)installDate).ToShortDateString()) + "\r\n";
            orderInternalNotes += "Last Service Date: " + (lastServiceDate.ToString() == "01/01/0001 00:00:00" ? "" : ((DateTime)lastServiceDate).ToShortDateString()) + "\r\n\r\n";
            orderInternalNotes += descOfWorks + "\r\n";
            orderInternalNotes += materialsUsed + "\r\n";
            orderInternalNotes += recomms + "\r\n";
            orderInternalNotes += "Subtotal " + subTotal + "\r\n";
            orderInternalNotes += "VAT " + vat + "\r\n";
            orderInternalNotes += "Total " + total + "\r\n\r\n";
            string paymentType = "";
            if (flgPymtCash)
            {
               paymentType = "Cash";
            }
            else if (flgPymtCard)
            {
               paymentType = "Card";
            }
            else if (flgPymtCheque)
            {
               paymentType = "Cheque";
            }
            else if (flgPymtInvoice)
            {
               paymentType = "Invoice";
            }
            orderInternalNotes += "Payment Type: " + paymentType;
            ordersNote = new OrdersNotes();
            ordersNote.JobSequence = jobSequence;
            ordersNote.OrderNotes = orderInternalNotes;
            ordersNote.CreatedBy = userId;
            ordersNote.DateCreated = timeStamp;
            ordersNote.LastAmendedBy = userId;
            ordersNote.DateLastAmended = timeStamp;
            ordersNote = ordersNotesRepository.Insert(request, ordersNote);
            if (ordersNote == null)
            {
               //TODO: Log and Report Error
               //Utilities.ReportError("Unable to Add Order Internal Notes for 'Necessary Remedial Work Required'" + Utilities.Message, method_name, true, system, edwFormInstance);
            }
            emailAddress = emailAddress.Trim();
            if (emailAddress != "")
            {
               if (Utilities.IsValidEmailId(emailAddress))
               {
                  if (jobSequence > 0)
                  {
                     OrdersRepository ordersRepository = new OrdersRepository( null);
                     Orders order = new Orders();
                     order.OccupierEmail = emailAddress;
                     order.Sequence = jobSequence;
                     order.LastAmendedBy = userId;
                     order.LastAmendedDate = timeStamp;
                     if (!ordersRepository.UpdateOrderInfo(order, SimplicityConstants.DB_FIELD_ORDERS_OCCUPIER_EMAIL, request))
                     {
                        //TODO: Log and Report Error
                        //Utilities.ReportError("Unable to Update Order's Owner Email for the Job Sequence " + jobSequence + " and Imp Ref '" + edwFormInstance.ImpRef + "' for Active Response." + Utilities.Message, method_name, true, system, edwFormInstance);
                     }

                  }
                  if (jobAddressId > 0)
                  {
                     EntityDetailsCore edc = new EntityDetailsCore();
                     edc.EntityId = jobAddressId;
                     edc.Email = emailAddress;
                     edc.LastAmendedBy = userId;
                     edc.DateLastAmended = timeStamp;
                     EntityDetailsCoreRepository entityDetailsCoreRepository = new EntityDetailsCoreRepository();
                     if (!entityDetailsCoreRepository.UpdateEntityDetailsCoreInfo(request, edc, SimplicityConstants.DB_FIELD_EDC_EMAIL))
                        if (!entityDetailsCoreRepository.UpdateEntityDetailsCoreInfo(request, edc, SimplicityConstants.DB_FIELD_EDC_EMAIL))
                        {
                           //TODO: Log and Report Error
                           //Utilities.ReportError("Unable to Update Order Address Email for the Job Sequence " + jobSequence + " and Imp Ref '" + edwFormInstance.ImpRef + "' for Active Response." + Utilities.Message, method_name, true, system, edwFormInstance);
                        }

                  }
               }
            }

            //TODO: Log 
            //log.Info("Finished Processing Data for AvonRuby Job Sheet with Template Id '" + edwFormInstance.TemplateId + "'");

            returnValue = true;
         }
         catch (Exception ex)
         {
            Message = "Unable to Process Avon Ruby Worksheet 1. Exception: " + ex.Message;
         }
         return returnValue;
      }
		//Active Response - New Worksheet 1 V18 (560072524)
		private bool ProcessActiveResponseWorksheeV18(HttpRequest request, S4BFormRequest s4bFormRequest, S4bFormSubmissions s4bFormSubmission,
													RootObject root, Dictionary<string, S4BFormsControl> s4bFormsControls)
		{
			bool returnValue = false;
			int userId = Utilities.GetUserIdFromRequest(request);

			long jobSequence = -1, jobAddressId = -1, jobClientId = -1;
			DiaryResources diaryResource = null;
			int visitStatusId = -1;
			long appSequence = -1;
			string userName = "";
			double[] descOfWork_amtValue = new double[10];
			string[] descOfWork_itemDesc = new string[10];
			string[] furtherInfo = new string[10];
			string location = "", serialNo = "", make = "", gcNo = "", model = "", type = "", condition = "";
			string gpsLocationStartTimeStamp = "", gpsLocationFinishTimeStamp = "", emailAddress = "";
			string diaryResourceName = "", timeStart = "", timeEnd = "", vat = "", subTotal = "", total = "";
			DateTime installDate = DateTime.MinValue, lastServiceDate = DateTime.MinValue;
			string[] recommendations = new string[4];
			double[] matUsed_amtTotal = new double[20];
			string[] matUsed_itemDesc = new string[20];
			double[] matUsed_itemQty = new double[20];
			string[] necessaryRemedial = new string[4];
			string[] defectFound = new string[16];
			bool flgPymtCash = false, flgPymtCheque = false, flgPymtCard = false, flgPymtInvoice = false;
			int index = -1;
			DateTime? timeStamp = s4bFormSubmission.DateCreated;
			DateTime appDate = DateTime.Now;
			Dictionary<string, string> imageValues = new Dictionary<string, string>();
			try
			{
				foreach (KeyValuePair<string, S4BFormsControl> s4bFormControlEntry in s4bFormsControls)
				{
					S4BFormsControl s4bFormControl = s4bFormControlEntry.Value;
					if (s4bFormControl.fieldValue != null)
					{
						System.Diagnostics.Debug.Print(s4bFormControlEntry.Key);
						Console.WriteLine(s4bFormControlEntry.Key);
						switch (s4bFormControlEntry.Key)
						{
							case "VARIABLE_PG1_JOB_SEQUENCE":
								if (long.TryParse(s4bFormControl.fieldValue, out jobSequence))
								{
								};
								break;
							case "VARIABLE_PG1_DIARY_ENTRY_ID":
								long.TryParse(s4bFormControl.fieldValue, out appSequence);
								break;
							case "VARIABLE_PG1_JOB_CLIENT_ID":
								if (long.TryParse(s4bFormControl.fieldValue, out jobClientId))
								{
								};
								break;
							case "VARIABLE_PG1_JOB_ADDRESS_ID":
								if (long.TryParse(s4bFormControl.fieldValue, out jobAddressId))
								{
								};
								break;
							case "VARIABLE_PG1_USER_ID":
								if (int.TryParse(s4bFormControl.fieldValue, out userId))
								{
								};
								break;
							case "VAR_PG1_DIARY_RESOURCE":
								userName = s4bFormControl.fieldValue;
								break;

							case "FIELD_PG4_VISIT_STAUTS":
								visitStatusId = Utilities.GetVisitStatusIdByStatusDesc(request, s4bFormControl.fieldValue);
								break;

							case "VAR_PG4_DIARY_ENTRY_DATE":
								if (DateTime.TryParse(s4bFormControl.fieldValue, out appDate))
								{
								};
								break;

							case "FIELD_PG4_ROW01_DESC":
							case "FIELD_PG4_ROW02_DESC":
							case "FIELD_PG4_ROW03_DESC":
							case "FIELD_PG4_ROW04_DESC":
							case "FIELD_PG4_ROW05_DESC":
							case "FIELD_PG4_ROW06_DESC":
							case "FIELD_PG4_ROW07_DESC":
							case "FIELD_PG4_ROW08_DESC":
							case "FIELD_PG4_ROW09_DESC":
							case "FIELD_PG4_ROW10_DESC":
							case "FIELD_PG4_ROW11_DESC":
							case "FIELD_PG4_ROW12_DESC":
								index = -1;
								int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
								if (index > 0)
								{
									descOfWork_itemDesc[index - 1] = s4bFormControl.fieldValue;
								}
								break;

							case "VARIABLE_PG4_ROW01_AMOUNT":
							case "VARIABLE_PG4_ROW02_AMOUNT":
							case "VARIABLE_PG4_ROW03_AMOUNT":
							case "VARIABLE_PG4_ROW04_AMOUNT":
							case "VARIABLE_PG4_ROW05_AMOUNT":
							case "VARIABLE_PG4_ROW06_AMOUNT":
							case "VARIABLE_PG4_ROW07_AMOUNT":
							case "VARIABLE_PG4_ROW08_AMOUNT":
							case "VARIABLE_PG4_ROW09_AMOUNT":
							case "VARIABLE_PG4_ROW10_AMOUNT":
							case "VARIABLE_PG4_ROW11_AMOUNT":
							case "VARIABLE_PG4_ROW12_AMOUNT":
								index = -1;
								int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
								if (index > 0)
								{
									Double.TryParse(s4bFormControl.fieldValue, out descOfWork_amtValue[index - 1]);
								}
								break;

							case "FIELD_PG1_FURTHER_INFORMATION":
								index = -1;
								int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
								if (index > 0)
								{
									furtherInfo[index - 1] = s4bFormControl.fieldValue;
								}
								break;

							case "FIELD_PG2_LAST_SERVICE_DATE":
								DateTime.TryParse(s4bFormControl.fieldValue, out lastServiceDate);
								break;

							case "FIELD_PG2_CONDITION":
								condition = s4bFormControl.fieldValue;
								break;

							case "FIELD_PG2_LOCATION":
								location = s4bFormControl.fieldValue;
								break;

							case "FIELD_PG2_MAKE":
								make = s4bFormControl.fieldValue;
								break;

							case "FIELD_PG2_MODEL":
								model = s4bFormControl.fieldValue;
								break;

							case "FIELD_PG2_TYPE":
								type = s4bFormControl.fieldValue;
								break;

							case "FIELD_PG2_INSTALLATION_DATE":
								DateTime.TryParse(s4bFormControl.fieldValue, out installDate);
								break;

							case "FIELD_PG2_SERIAL_NO":
								serialNo = s4bFormControl.fieldValue;
								break;

							case "FIELD_PG2_GC_NO":
								gcNo = s4bFormControl.fieldValue;
								break;

							case "FIELD_PG3_WORKS_REQUIRED":
								index = -1;
								int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
								if (index > 0)
								{
									necessaryRemedial[index - 1] = s4bFormControl.fieldValue;
								}
								break;

							case "FIELD_PG5_PYMT_TYPE_CASH":
								Boolean.TryParse(s4bFormControl.fieldValue, out flgPymtCash);
								break;

							case "FIELD_PG5_PYMT_TYPE_CHEQUE":
								Boolean.TryParse(s4bFormControl.fieldValue, out flgPymtCheque);
								break;

							case "FIELD_PG5_PYMT_TYPE_CARD":
								Boolean.TryParse(s4bFormControl.fieldValue, out flgPymtCard);
								break;

							case "FIELD_PG5_PYMT_TYPE_INVOICE":
								Boolean.TryParse(s4bFormControl.fieldValue, out flgPymtInvoice);
								break;

							case "FIELD_PG4_ROW13_DESC":
							case "FIELD_PG4_ROW14_DESC":
							case "FIELD_PG4_ROW15_DESC":
							case "FIELD_PG4_ROW16_DESC":
							case "FIELD_PG4_ROW17_DESC":
							case "FIELD_PG4_ROW18_DESC":
							case "FIELD_PG4_ROW19_DESC":
							case "FIELD_PG4_ROW20_DESC":
								index = -1;
								int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
								if (index > 0)
								{
									matUsed_itemDesc[index - 1] = s4bFormControl.fieldValue;
								}
								break;

							case "FIELD_PG4_ROW13_QTY":
							case "FIELD_PG4_ROW14_QTY":
							case "FIELD_PG4_ROW15_QTY":
							case "FIELD_PG4_ROW16_QTY":
							case "FIELD_PG4_ROW17_QTY":
							case "FIELD_PG4_ROW18_QTY":
							case "FIELD_PG4_ROW19_QTY":
							case "FIELD_PG4_ROW20_QTY":
								index = -1;
								int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
								if (index > 0)
								{
									Double.TryParse(s4bFormControl.fieldValue, out matUsed_itemQty[index - 1]);
								}
								break;

							case "FIELD_PG4_ROW13_AMOUNT":
							case "FIELD_PG4_ROW14_AMOUNT":
							case "FIELD_PG4_ROW15_AMOUNT":
							case "FIELD_PG4_ROW16_AMOUNT":
							case "FIELD_PG4_ROW17_AMOUNT":
							case "FIELD_PG4_ROW18_AMOUNT":
							case "FIELD_PG4_ROW19_AMOUNT":
							case "FIELD_PG4_ROW20_AMOUNT":
								index = -1;
								int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
								if (index > 0)
								{
									Double.TryParse(s4bFormControl.fieldValue, out matUsed_amtTotal[index - 1]);
								}
								break;

							case "FIELD_PG5_RECOMMENDATIONS":
								index = -1;
								int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
								if (index > 0)
								{
									recommendations[index - 1] = s4bFormControl.fieldValue;
								}
								break;

							case "FIELD_PG1_RA01":
								gpsLocationStartTimeStamp = s4bFormControl.fieldValue;
								break;

							case "FIELD_PG5_SUBMIT":
								gpsLocationFinishTimeStamp = s4bFormControl.fieldValue;
								break;

							case "VARIABLE_PG1_DIARY_RESOURCE":
								diaryResourceName = s4bFormControl.fieldValue;
								break;

							case "FIELD_PG4_TIME_START":
								timeStart = s4bFormControl.fieldValue;
								break;

							case "FIELD_PG4_TIME_END":
								timeEnd = s4bFormControl.fieldValue;
								break;

							case "VAR_PG5_SUB_TOTAL":
								subTotal = s4bFormControl.fieldValue;
								break;

							case "VAR_PG5_VAT":
								vat = s4bFormControl.fieldValue;
								break;

							case "VAR_PG5_TOTAL":
								total = s4bFormControl.fieldValue;
								break;

							case "FIELD_PG5_CLIENT_EMAIL":
								emailAddress = s4bFormControl.fieldValue;
								break;

						}
					}
				}
				if (userId <= 0)
				{
					userId = 1;
				}
				diaryResource = Utilities.GetDiaryResourceByUserId(request, userId);
				OrdersNotesRepository ordersNotesRepository = new OrdersNotesRepository();
				OrdersNotes ordersNote = new OrdersNotes();

				/// 1. Further Information - Order Internal Notes
				string formattedNotes = "";
				for (int counter = 0; counter < 10; counter++)
				{
					if (furtherInfo[counter] != null && furtherInfo[counter].Trim() != "")
					{
						if (formattedNotes == "")
						{
							//formattedNotes = jobSequence + " - " + userName + " - " + timeStamp;
							formattedNotes = "Further information or any other hazards" + "\r\n";
						}
						formattedNotes = formattedNotes + "\r\n" + furtherInfo[counter];
					}
				}
				if (formattedNotes != "")
				{
					ordersNote.JobSequence = jobSequence;
					ordersNote.OrderNotes = formattedNotes;
					ordersNote.CreatedBy = userId;
					ordersNote.DateCreated = timeStamp;
					ordersNote.LastAmendedBy = userId;
					ordersNote.DateLastAmended = timeStamp;
					OrdersNotes ordNoteNew = ordersNotesRepository.Insert(request, ordersNote);
				}

				/// 2. Appliance Details
				if ((type != "" && type != null) ||
					(serialNo != "" && serialNo != null) ||
					(location != "" && location != null) ||
					(make != "" && make != null) ||
					(model != "" && model != null) ||
					(gcNo != "" && gcNo != null) ||
					(condition != "" && condition != null))
				{
					AssetRegister assetRegister = new AssetRegister();
					assetRegister.FlgDeleted = false;
					assetRegister.TransType = SimplicityConstants.ClientTransType;
					assetRegister.EntityId = -1;
					assetRegister.ItemJoinDept = -1;
					assetRegister.ItemJoinCategory = -1;
					assetRegister.ItemJoinSupplementary = -1;
					assetRegister.ItemManufacturer = make;
					assetRegister.ItemModel = model;
					assetRegister.ItemSerialRef = serialNo;
					assetRegister.ItemExtraInfo = type;
					assetRegister.ItemUserField1 = gcNo;
					assetRegister.ItemUserField2 = SimplicityConstants.NotSet;
					assetRegister.ItemUserField3 = SimplicityConstants.NotSet;
					assetRegister.ItemQuantity = 1;
					assetRegister.DateInstalled = installDate;
					assetRegister.DateAcquired = DateTime.MinValue;
					assetRegister.DateDisposed = DateTime.MinValue;
					assetRegister.ItemValueBook = 0;
					assetRegister.ItemValueDepreciation = 0;
					assetRegister.ItemValueDisposal = 0;
					assetRegister.ItemDesc = "";
					assetRegister.ItemAddress = "";
					assetRegister.FlgUseAddressId = false;
					assetRegister.ItemAddressId = -1;
					assetRegister.ItemLocationJoinId = -1;
					assetRegister.ItemLocation = location;
					assetRegister.FlgItemChargeable = false;
					assetRegister.ItemCostMaterialRate = 0;
					assetRegister.ItemCostLabourRate = 0;
					assetRegister.CreatedBy = userId;
					assetRegister.DateCreated = timeStamp;
					assetRegister.LastAmendedBy = userId;
					assetRegister.DateLastAmended = timeStamp;
					AssetRegisterRepository assetRegisterRepository = new AssetRegisterRepository();
					AssetRegister existAssetRegister = assetRegisterRepository.getAssetRegisterByLocationMakeModelTypeSearialNo(request, location, make, model, type, serialNo);
					AssetRegister assetRegisterUpdated = new AssetRegister();
					if (existAssetRegister == null)
					{
						assetRegisterUpdated = assetRegisterRepository.insert(request, assetRegister);
					}
					else
					{
						assetRegister.Sequence = existAssetRegister.Sequence;
						assetRegisterUpdated = assetRegisterRepository.Update(request, assetRegister);
					}
					if (assetRegisterUpdated.Sequence <= 0)
					{
						//Utilities.ReportError("Unable to Insert/Update Asset Register Record for Import Id " + edwFormInstance.ImpRef + ". Error Message is " + Utilities.Message, method_name, true, system, edwFormInstance);
						//TODO: Log and Report Error
					}
					else
					{
						/// Check if it is boiler then insert for supplementary
						/// 
						if (type.ToUpper().Equals("COMBI") || type.ToUpper().Equals("BACK BOILER"))
						{
							AssetRegisterSuppGas AssetRegisterSuppGasObj = new AssetRegisterSuppGas();
							AssetRegisterSuppGasObj.Sequence = -1;
							AssetRegisterSuppGasObj.JoinSequence = assetRegisterUpdated.Sequence;
							AssetRegisterSuppGasObj.EntityId = -1;
							AssetRegisterSuppGasObj.AssetGasType = "";
							AssetRegisterSuppGasObj.FlgGasFixing = false;
							AssetRegisterSuppGasObj.GasFixing = "";
							AssetRegisterSuppGasObj.FlgGasType = false;
							AssetRegisterSuppGasObj.GasType = "";
							AssetRegisterSuppGasObj.FlgGasFuel = false;
							AssetRegisterSuppGasObj.GasFuel = "";
							AssetRegisterSuppGasObj.FlgGasEfficiency = false;
							AssetRegisterSuppGasObj.GasEfficiency = "";
							AssetRegisterSuppGasObj.FlgGasFlueType = false;
							AssetRegisterSuppGasObj.GasFlueType = "";
							AssetRegisterSuppGasObj.FlgGasFlueing = false;
							AssetRegisterSuppGasObj.GasFlueing = "";
							AssetRegisterSuppGasObj.FlgGasOvUvSs = false;
							AssetRegisterSuppGasObj.GasOvUvSs = "";
							AssetRegisterSuppGasObj.FlgGasExpansionVessel = false;
							AssetRegisterSuppGasObj.FlgGasExpansion = false;
							AssetRegisterSuppGasObj.GasExpansion = "";
							AssetRegisterSuppGasObj.FlgGasImmersion = false;
							AssetRegisterSuppGasObj.GasImmersion = "";
							AssetRegisterSuppGasObj.LastAmendedBy = userId;
							AssetRegisterSuppGasObj.DateLastAmended = timeStamp;
							AssetRegisterSuppGasRepository AssetRegisterSuppGasRepos = new AssetRegisterSuppGasRepository();
							AssetRegisterSuppGas AssetRegisterSuppGasNew = AssetRegisterSuppGasRepos.insert(request, AssetRegisterSuppGasObj);
							if (AssetRegisterSuppGasNew == null)
							{
								//Utilities.ReportError("Unable to Insert/Update Asset Register Supp Record for Import Id " + edwFormInstance.ImpRef + ". Error Message is " + Utilities.Message, method_name, true, system, edwFormInstance);
								//TODO: Log and Report Error
							}
						}
						AssetRegisterService assetRegisterServiceObj = new AssetRegisterService();
						assetRegisterServiceObj.Sequence = -1;
						assetRegisterServiceObj.FlgDeleted = false;
						assetRegisterServiceObj.AssetSequence = assetRegisterUpdated.Sequence;
						assetRegisterServiceObj.JobSequence = jobSequence;
						assetRegisterServiceObj.DaSequence = appSequence;
						assetRegisterServiceObj.ServiceInitials = "";
						assetRegisterServiceObj.ServiceNotes = "";
						assetRegisterServiceObj.ConditionSequence = -1;
						assetRegisterServiceObj.ServiceBy = userId;
						assetRegisterServiceObj.FlgNewJobCreated = false;
						assetRegisterServiceObj.FlgNewApp = false;
						assetRegisterServiceObj.FlgValidated = false;
						assetRegisterServiceObj.ValidatedBy = -1;
						assetRegisterServiceObj.DateValidated = DateTime.MinValue;
						assetRegisterServiceObj.ItemCostLabourRate = 0;
						assetRegisterServiceObj.CreatedBy = userId;
						assetRegisterServiceObj.DateCreated = timeStamp;
						AssetRegisterService assetRegisterServiceNew = null;
						AssetRegisterServiceRepository assetRegisterServiceRepos = new AssetRegisterServiceRepository();
						if (lastServiceDate != null && lastServiceDate != DateTime.MinValue)
						{
							assetRegisterServiceObj.FlgNotActive = true;
							assetRegisterServiceObj.DaAppType = -1;
							assetRegisterServiceObj.DateDaStart = lastServiceDate;
							assetRegisterServiceObj.DateService = lastServiceDate;
							assetRegisterServiceNew = assetRegisterServiceRepos.insert(request, assetRegisterServiceObj);
						}
						assetRegisterServiceObj.FlgNotActive = false;
						assetRegisterServiceObj.DaAppType = 1;
						assetRegisterServiceObj.DateDaStart = appDate;
						assetRegisterServiceObj.DateService = appDate;
						assetRegisterServiceNew = assetRegisterServiceRepos.insert(request, assetRegisterServiceObj);
					}
				}

				/// 3. NECESSARY REMEDIAL WORK REQUIRED
				formattedNotes = "";
				for (int counter = 0; counter < 4; counter++)
				{
					if (necessaryRemedial[counter] != null && necessaryRemedial[counter].Trim() != "")
					{
						if (formattedNotes == "")
						{
							formattedNotes = "Necessary Remedial Work Required" + "\r\n";
						}
						formattedNotes = formattedNotes + "\r\n" + necessaryRemedial[counter];
					}
				}
				if (formattedNotes != "")
				{
					ordersNote = new OrdersNotes();
					ordersNote.JobSequence = jobSequence;
					ordersNote.OrderNotes = formattedNotes;
					ordersNote.CreatedBy = userId;
					ordersNote.DateCreated = timeStamp;
					ordersNote.LastAmendedBy = userId;
					ordersNote.DateLastAmended = timeStamp;
					ordersNote = ordersNotesRepository.Insert(request, ordersNote);
					if (ordersNote == null)
					{
						//    Utilities.ReportError("Unable to Add Order Internal Notes for 'Necessary Remedial Work Required'" + Utilities.Message, method_name, true, system, edwFormInstance);
						//TODO: Log and Report Error
					}
				}

				/// 4. Visit Status Update
				if (appSequence > 0)
				{
					DiaryAppsRepository diaryAppsRepositoryVisit = new DiaryAppsRepository();
					if (visitStatusId >= 0)
					{
						if (!diaryAppsRepositoryVisit.updateVisitStatusAndFlgCompletedBySequence(request, appSequence, visitStatusId, true, userId, timeStamp))
						{
							//TODO: Log and Report Error
							//    //Utilities.ReportError("Unable to Update Diary Visit Status" + Utilities.Message, method_name, true, system, edwFormInstance);
						}
					}
				}

				/// 5. Description of Works
				string descOfWorks = "Description Of Works\r\n";
				bool itemExists = false;
				for (int counter = 0; counter < 10; counter++)
				{
					if (descOfWork_amtValue[counter] > 0 ||
						(descOfWork_itemDesc[counter] != "" && descOfWork_itemDesc[counter] != null))
					{
						itemExists = true;
						descOfWorks = descOfWorks + descOfWork_itemDesc[counter];
						if (descOfWork_amtValue[counter] != 0)
						{
							descOfWorks = descOfWorks + " - Value " + descOfWork_amtValue[counter];
						}
						descOfWorks = descOfWorks + "\r\n";
					}
				}
				if (itemExists)
				{
					ordersNote = new OrdersNotes();
					ordersNote.JobSequence = jobSequence;
					ordersNote.OrderNotes = descOfWorks;
					ordersNote.CreatedBy = userId;
					ordersNote.DateCreated = timeStamp;
					ordersNote.LastAmendedBy = userId;
					ordersNote.DateLastAmended = timeStamp;
					ordersNote = ordersNotesRepository.Insert(request, ordersNote);
				}

				/// 7. Materials Used / Description
				string materialsUsed = "Materials Used/Description\r\n";
				itemExists = false;
				for (int counter = 0; counter < 8; counter++)
				{
					if (matUsed_amtTotal[counter] > 0 ||
						matUsed_itemQty[counter] > 0 ||
						(matUsed_itemDesc[counter] != "" && matUsed_itemDesc[counter] != null))
					{
						itemExists = true;
						double matAmt = 0;
						if (matUsed_itemQty[counter] != 0)
						{
							matAmt = Math.Round(matUsed_amtTotal[counter] / matUsed_itemQty[counter], 2); ;
						}
						materialsUsed = materialsUsed + matUsed_itemDesc[counter];
						if (matUsed_amtTotal[counter] != 0)
						{
							materialsUsed = materialsUsed + " Value: " + matUsed_amtTotal[counter];
						}
						materialsUsed = materialsUsed + "\r\n";
					}
				}
				if (itemExists)
				{
					ordersNote = new OrdersNotes();
					ordersNote.JobSequence = jobSequence;
					ordersNote.OrderNotes = materialsUsed;
					ordersNote.CreatedBy = userId;
					ordersNote.DateCreated = timeStamp;
					ordersNote.LastAmendedBy = userId;
					ordersNote.DateLastAmended = timeStamp;
					ordersNote = ordersNotesRepository.Insert(request, ordersNote);
				}

				/// 8. Recommendations For Further Work
				formattedNotes = "";
				itemExists = false;
				String recomms = "Recommendations\r\n";
				for (int counter = 0; counter < 4; counter++)
				{
					if (recommendations[counter] != null && recommendations[counter].Trim() != "")
					{
						itemExists = true;
						if (formattedNotes == "")
						{
							formattedNotes = "RECOMMENDATIONS FOR FURTHER WORK" + "\r\n";
						}
						formattedNotes = formattedNotes + "\r\n" + recommendations[counter];
						recomms = recomms + recommendations[counter] + "\r\n";
					}
				}
				if (formattedNotes != "")
				{
					ordersNote = new OrdersNotes();
					ordersNote.JobSequence = jobSequence;
					ordersNote.OrderNotes = recomms;
					ordersNote.CreatedBy = userId;
					ordersNote.DateCreated = timeStamp;
					ordersNote.LastAmendedBy = userId;
					ordersNote.DateLastAmended = timeStamp;
					ordersNote = ordersNotesRepository.Insert(request, ordersNote);
					if (ordersNote == null)
					{
						//TODO: Report and Log Error
						//    Utilities.ReportError("Unable to Add Order Internal Notes for 'Further information or any other hazards'" + Utilities.Message, method_name, true, system, edwFormInstance);
					}
				}

				/// 9. Payment Type Additions into Schedule Items
				if (flgPymtCheque || flgPymtCard || flgPymtCash || flgPymtInvoice)
				{
					string paymentTypes = "";
					if (flgPymtCheque)
					{
						if (paymentTypes == "")
						{
							paymentTypes = "Payment = Cheque";
						}
						else
						{
							paymentTypes = paymentTypes + " - Cheque";
						}
					}
					if (flgPymtCard)
					{
						if (paymentTypes == "")
						{
							paymentTypes = "Payment = Card";
						}
						else
						{
							paymentTypes = paymentTypes + " - Card";
						}
					}
					if (flgPymtCash)
					{
						if (paymentTypes == "")
						{
							paymentTypes = "Payment = Cash";
						}
						else
						{
							paymentTypes = paymentTypes + " - Cash";
						}
					}
					if (flgPymtInvoice)
					{
						if (paymentTypes == "")
						{
							paymentTypes = "Payment = Invoice";
						}
						else
						{
							paymentTypes = paymentTypes + " - Invoice";
						}
					}
					ordersNote = new OrdersNotes();
					ordersNote.JobSequence = jobSequence;
					ordersNote.OrderNotes = paymentTypes;
					ordersNote.CreatedBy = userId;
					ordersNote.DateCreated = timeStamp;
					ordersNote.LastAmendedBy = userId;
					ordersNote.DateLastAmended = timeStamp;
					ordersNote = ordersNotesRepository.Insert(request, ordersNote);
					if (ordersNote == null)
					{
						//TODO: Report and Log Error
						//    Utilities.ReportError("Unable to Add Order Internal Notes for 'Further information or any other hazards'" + Utilities.Message, method_name, true, system, edwFormInstance);
					}
				}

				//10. Adding order internal notes
				DiaryAppsRepository diaryAppsRepository = new DiaryAppsRepository();
				DiaryApps diaryApp = diaryAppsRepository.GetDiaryAppsBySequence(appSequence, request, null);

				string appDateString = appDate == DateTime.MinValue ? "" : appDate.ToString("dd/MM/yyyy");
				string timeAppStart = diaryApp.DateAppStart.Value.ToString("HH:mm");
				string timeAppEnd = diaryApp.DateAppEnd.Value.ToString("HH:mm");
				string orderInternalNotes = "W.S-" + appDateString + " " + diaryResource.ResourceName + "\r\n\r\n";
				orderInternalNotes += "GPS timestamp start " + gpsLocationStartTimeStamp + " finish " + gpsLocationFinishTimeStamp + "\r\n\r\n";
				orderInternalNotes += diaryResource.ResourceName + " attended " + appDateString + " between " + timeAppStart + " - " + timeAppEnd + "\r\n\r\n";
				orderInternalNotes += "Appliance Details\r\n";
				orderInternalNotes += "Location: " + location + "\r\n";
				orderInternalNotes += "Make: " + make + "\r\n";
				orderInternalNotes += "Model: " + model + "\r\n";
				orderInternalNotes += "Type: " + type + "\r\n";
				orderInternalNotes += "Condition: " + condition + "\r\n";
				orderInternalNotes += "Sreial no: " + serialNo + "\r\n";
				orderInternalNotes += "GC No: " + gcNo + "\r\n";
				orderInternalNotes += "Install Date: " + (installDate == null || installDate.ToString() == "01/01/0001 00:00:00" ? "" : ((DateTime)installDate).ToShortDateString()) + "\r\n";
				orderInternalNotes += "Last Service Date: " + (lastServiceDate.ToString() == "01/01/0001 00:00:00" ? "" : ((DateTime)lastServiceDate).ToShortDateString()) + "\r\n\r\n";
				orderInternalNotes += descOfWorks + "\r\n";
				orderInternalNotes += materialsUsed + "\r\n";
				orderInternalNotes += recomms + "\r\n";
				orderInternalNotes += "Subtotal " + subTotal + "\r\n";
				orderInternalNotes += "VAT " + vat + "\r\n";
				orderInternalNotes += "Total " + total + "\r\n\r\n";
				string paymentType = "";
				if (flgPymtCash)
				{
					paymentType = "Cash";
				}
				else if (flgPymtCard)
				{
					paymentType = "Card";
				}
				else if (flgPymtCheque)
				{
					paymentType = "Cheque";
				}
				else if (flgPymtInvoice)
				{
					paymentType = "Invoice";
				}
				orderInternalNotes += "Payment Type: " + paymentType;
				ordersNote = new OrdersNotes();
				ordersNote.JobSequence = jobSequence;
				ordersNote.OrderNotes = orderInternalNotes;
				ordersNote.CreatedBy = userId;
				ordersNote.DateCreated = timeStamp;
				ordersNote.LastAmendedBy = userId;
				ordersNote.DateLastAmended = timeStamp;
				ordersNote = ordersNotesRepository.Insert(request, ordersNote);
				if (ordersNote == null)
				{
					//TODO: Log and Report Error
					//Utilities.ReportError("Unable to Add Order Internal Notes for 'Necessary Remedial Work Required'" + Utilities.Message, method_name, true, system, edwFormInstance);
				}
				emailAddress = emailAddress.Trim();
				if (emailAddress != "")
				{
					if (Utilities.IsValidEmailId(emailAddress))
					{
						if (jobSequence > 0)
						{
							OrdersRepository ordersRepository = new OrdersRepository(null);
							Orders order = new Orders();
							order.OccupierEmail = emailAddress;
							order.Sequence = jobSequence;
							order.LastAmendedBy = userId;
							order.LastAmendedDate = timeStamp;
							if (!ordersRepository.UpdateOrderInfo(order, SimplicityConstants.DB_FIELD_ORDERS_OCCUPIER_EMAIL, request))
							{
								//TODO: Log and Report Error
								//Utilities.ReportError("Unable to Update Order's Owner Email for the Job Sequence " + jobSequence + " and Imp Ref '" + edwFormInstance.ImpRef + "' for Active Response." + Utilities.Message, method_name, true, system, edwFormInstance);
							}

						}
						if (jobAddressId > 0)
						{
							EntityDetailsCore edc = new EntityDetailsCore();
							edc.EntityId = jobAddressId;
							edc.Email = emailAddress;
							edc.LastAmendedBy = userId;
							edc.DateLastAmended = timeStamp;
							EntityDetailsCoreRepository entityDetailsCoreRepository = new EntityDetailsCoreRepository();
							if (!entityDetailsCoreRepository.UpdateEntityDetailsCoreInfo(request, edc, SimplicityConstants.DB_FIELD_EDC_EMAIL))
								if (!entityDetailsCoreRepository.UpdateEntityDetailsCoreInfo(request, edc, SimplicityConstants.DB_FIELD_EDC_EMAIL))
								{
									//TODO: Log and Report Error
									//Utilities.ReportError("Unable to Update Order Address Email for the Job Sequence " + jobSequence + " and Imp Ref '" + edwFormInstance.ImpRef + "' for Active Response." + Utilities.Message, method_name, true, system, edwFormInstance);
								}

						}
					}
				}

				//TODO: Log 
				//log.Info("Finished Processing Data for AvonRuby Job Sheet with Template Id '" + edwFormInstance.TemplateId + "'");

				returnValue = true;
			}
			catch (Exception ex)
			{
				Message = "Unable to ProcessActive Response Worksheet 18. Exception: " + ex.Message;
			}
			return returnValue;
		}



		// Capel Group - Capel KS Plant Weekly Plant 360000168
		private bool ProcessCapelKSPlantWeeklyPlant(HttpRequest request, S4BFormRequest s4bFormRequest, S4bFormSubmissions s4bFormSubmission,
                                                  RootObject root, Dictionary<string, S4BFormsControl> s4bFormsControls)
      {
         const string METHOD_NAME = "S4BFormsRepository.ProcessCapelKSPlantWeeklyPlant()";
         bool returnValue = false;
         int userId = Utilities.GetUserIdFromRequest(request);
         int index = -1;

         const int NO_ROWS = 11;
         long jobSequence = -1, jobAddressId = -1, jobClientId = -1, assignedTo = -1, appSequence = -1;
         string userName = "";
         string[] itemCode = new string[NO_ROWS];
         string[] itemDesc = new string[NO_ROWS];
         string[] itemDescPart2 = new string[NO_ROWS];
         string[] vehiclePlantDescFull = new string[NO_ROWS];
         string[] ud = new string[NO_ROWS];
         string[] workCompleted = new string[NO_ROWS];
         string[] percentage = new string[NO_ROWS];
         string[] timeFrom = new string[NO_ROWS];
         string[] timeTo = new string[NO_ROWS];
         string mileage = "";
         string[] pymtTypeMultiplyDesc = new string[NO_ROWS];
         double[] pymtTypeMultiplier = new double[NO_ROWS];
         string[] notes = new string[NO_ROWS];
         double[] itemQty = new double[NO_ROWS];
         bool[] createRow = new bool[NO_ROWS];

         double[] amtValue = new double[NO_ROWS];
         string[] itemUnit = new string[NO_ROWS];
         DateTime? timeStamp = s4bFormSubmission.DateCreated;
         DateTime appDate = DateTime.Now;
         Dictionary<string, string> imageValues = new Dictionary<string, string>();
         try
         {
            foreach (KeyValuePair<string, S4BFormsControl> s4bFormControlEntry in s4bFormsControls)
            {
               S4BFormsControl s4bFormControl = s4bFormControlEntry.Value;
               if (s4bFormControl.fieldValue != null)
               {
                  switch (s4bFormControlEntry.Key)
                  {
                     case "VARIABLE_PG1_JOB_SEQUENCE":
                        if (long.TryParse(s4bFormControl.fieldValue, out jobSequence))
                        {
                        };
                        break;
                     case "VARIABLE_PG1_DIARY_ENTRY_ID":
                     case "VARIABLE_PG1_DA_SEQUENCE":
                     case "VARIABLE_PG1_DE_SEQUENCE":
                        long.TryParse(s4bFormControl.fieldValue, out appSequence);
                        break;
                     case "VARIABLE_PG1_JOB_CLIENT_ID":
                        if (long.TryParse(s4bFormControl.fieldValue, out jobClientId))
                        {
                        };
                        break;
                     case "VARIABLE_PG1_JOB_ADDRESS_ID":
                        if (long.TryParse(s4bFormControl.fieldValue, out jobAddressId))
                        {
                        };
                        break;
                     case "VARIABLE_PG1_USER_ID":
                        if (int.TryParse(s4bFormControl.fieldValue, out userId))
                        {
                        };
                        break;
                     case "VARIABLE_PG1_DIARY_RESOURCE":
                        userName = s4bFormControl.fieldValue;
                        break;
                     case "VARIABLE_PG6_DIARY_ENTRY_DATE":
                        if (DateTime.TryParse(s4bFormControl.fieldValue, out appDate))
                        {
                        };
                        break;

                     case "FIELD_PG1_ROW_01_VEHICLE_PLANT":
                     case "FIELD_PG1_ROW_02_VEHICLE_PLANT":
                     case "FIELD_PG1_ROW_03_VEHICLE_PLANT":
                     case "FIELD_PG1_ROW_04_VEHICLE_PLANT":
                     case "FIELD_PG1_ROW_05_VEHICLE_PLANT":
                     case "FIELD_PG1_ROW_06_VEHICLE_PLANT":
                     case "FIELD_PG1_ROW_07_VEHICLE_PLANT":
                     case "FIELD_PG1_ROW_08_VEHICLE_PLANT":
                     case "FIELD_PG1_ROW_09_VEHICLE_PLANT":
                     case "FIELD_PG1_ROW_10_VEHICLE_PLANT":
                     case "FIELD_PG1_ROW_11_VEHICLE_PLANT":
                        index = -1;
                        int.TryParse(s4bFormControl.fieldName.Substring(14, 2), out index);
                        if (index > 0)
                        {
                           vehiclePlantDescFull[index] = s4bFormControl.fieldValue;
                           itemCode[index] = "";
                           itemDesc[index] = "";
                           if (vehiclePlantDescFull[index].Trim() != "")
                           {
                              string[] splittedValue = vehiclePlantDescFull[index].Split('-');
                              if (splittedValue.Length > 0)
                              {
                                 itemCode[index - 1] = splittedValue[0];
                                 if (splittedValue.Length > 1)
                                 {
                                    itemDesc[index - 1] = splittedValue[1];
                                 }
                              }
                           }
                        }
                        break;

                     case "FIELD_PG1_ROW_01_UD":
                     case "FIELD_PG1_ROW_02_UD":
                     case "FIELD_PG1_ROW_03_UD":
                     case "FIELD_PG1_ROW_04_UD":
                     case "FIELD_PG1_ROW_05_UD":
                     case "FIELD_PG1_ROW_06_UD":
                     case "FIELD_PG1_ROW_07_UD":
                     case "FIELD_PG1_ROW_08_UD":
                     case "FIELD_PG1_ROW_09_UD":
                     case "FIELD_PG1_ROW_10_UD":
                     case "FIELD_PG1_ROW_11_UD":
                        index = -1;
                        int.TryParse(s4bFormControl.fieldName.Substring(14, 2), out index);
                        if (index > 0)
                        {
                           ud[index - 1] = s4bFormControl.fieldValue;
                        }
                        break;

                     case "FIELD_PG1_ROW_01_WORK_COMPLETED":
                     case "FIELD_PG1_ROW_02_WORK_COMPLETED":
                     case "FIELD_PG1_ROW_03_WORK_COMPLETED":
                     case "FIELD_PG1_ROW_04_WORK_COMPLETED":
                     case "FIELD_PG1_ROW_05_WORK_COMPLETED":
                     case "FIELD_PG1_ROW_06_WORK_COMPLETED":
                     case "FIELD_PG1_ROW_07_WORK_COMPLETED":
                     case "FIELD_PG1_ROW_08_WORK_COMPLETED":
                     case "FIELD_PG1_ROW_09_WORK_COMPLETED":
                     case "FIELD_PG1_ROW_10_WORK_COMPLETED":
                     case "FIELD_PG1_ROW_11_WORK_COMPLETED":
                        index = -1;
                        int.TryParse(s4bFormControl.fieldName.Substring(14, 2), out index);
                        if (index > 0)
                        {
                           workCompleted[index - 1] = s4bFormControl.fieldValue;
                        }
                        break;

                     case "FIELD_PG1_ROW_01_PERCENTAGE":
                     case "FIELD_PG1_ROW_02_PERCENTAGE":
                     case "FIELD_PG1_ROW_03_PERCENTAGE":
                     case "FIELD_PG1_ROW_04_PERCENTAGE":
                     case "FIELD_PG1_ROW_05_PERCENTAGE":
                     case "FIELD_PG1_ROW_06_PERCENTAGE":
                     case "FIELD_PG1_ROW_07_PERCENTAGE":
                     case "FIELD_PG1_ROW_08_PERCENTAGE":
                     case "FIELD_PG1_ROW_09_PERCENTAGE":
                     case "FIELD_PG1_ROW_10_PERCENTAGE":
                     case "FIELD_PG1_ROW_11_PERCENTAGE":
                        index = -1;
                        int.TryParse(s4bFormControl.fieldName.Substring(14, 2), out index);
                        if (index > 0)
                        {
                           percentage[index - 1] = s4bFormControl.fieldValue;
                        }
                        break;

                     case "FIELD_PG1_ROW_01_TIME_FROM":
                     case "FIELD_PG1_ROW_02_TIME_FROM":
                     case "FIELD_PG1_ROW_03_TIME_FROM":
                     case "FIELD_PG1_ROW_04_TIME_FROM":
                     case "FIELD_PG1_ROW_05_TIME_FROM":
                     case "FIELD_PG1_ROW_06_TIME_FROM":
                     case "FIELD_PG1_ROW_07_TIME_FROM":
                     case "FIELD_PG1_ROW_08_TIME_FROM":
                     case "FIELD_PG1_ROW_09_TIME_FROM":
                     case "FIELD_PG1_ROW_10_TIME_FROM":
                     case "FIELD_PG1_ROW_11_TIME_FROM":
                        index = -1;
                        int.TryParse(s4bFormControl.fieldName.Substring(14, 2), out index);
                        if (index > 0)
                        {
                           timeFrom[index - 1] = s4bFormControl.fieldValue;
                        }
                        break;

                     case "FIELD_PG1_ROW_01_TIME_TO":
                     case "FIELD_PG1_ROW_02_TIME_TO":
                     case "FIELD_PG1_ROW_03_TIME_TO":
                     case "FIELD_PG1_ROW_04_TIME_TO":
                     case "FIELD_PG1_ROW_05_TIME_TO":
                     case "FIELD_PG1_ROW_06_TIME_TO":
                     case "FIELD_PG1_ROW_07_TIME_TO":
                     case "FIELD_PG1_ROW_08_TIME_TO":
                     case "FIELD_PG1_ROW_09_TIME_TO":
                     case "FIELD_PG1_ROW_10_TIME_TO":
                     case "FIELD_PG1_ROW_11_TIME_TO":
                        index = -1;
                        int.TryParse(s4bFormControl.fieldName.Substring(14, 2), out index);
                        if (index > 0)
                        {
                           timeTo[index - 1] = s4bFormControl.fieldValue;
                        }
                        break;

                     case "FIELD_PG1_MILEAGE":
                        mileage = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG1_ROW_01_PAYMENT_TYPE_MULTIPLY":
                     case "FIELD_PG1_ROW_02_PAYMENT_TYPE_MULTIPLY":
                     case "FIELD_PG1_ROW_03_PAYMENT_TYPE_MULTIPLY":
                     case "FIELD_PG1_ROW_04_PAYMENT_TYPE_MULTIPLY":
                     case "FIELD_PG1_ROW_05_PAYMENT_TYPE_MULTIPLY":
                     case "FIELD_PG1_ROW_06_PAYMENT_TYPE_MULTIPLY":
                     case "FIELD_PG1_ROW_07_PAYMENT_TYPE_MULTIPLY":
                     case "FIELD_PG1_ROW_08_PAYMENT_TYPE_MULTIPLY":
                     case "FIELD_PG1_ROW_09_PAYMENT_TYPE_MULTIPLY":
                     case "FIELD_PG1_ROW_10_PAYMENT_TYPE_MULTIPLY":
                     case "FIELD_PG1_ROW_11_PAYMENT_TYPE_MULTIPLY":
                        index = -1;
                        int.TryParse(s4bFormControl.fieldName.Substring(14, 2), out index);
                        if (index > 0)
                        {
                           pymtTypeMultiplyDesc[index - 1] = s4bFormControl.fieldValue;
                        }
                        break;

                     case "FIELD_PG1_ROW_01_NOTES":
                     case "FIELD_PG1_ROW_02_NOTES":
                     case "FIELD_PG1_ROW_03_NOTES":
                     case "FIELD_PG1_ROW_04_NOTES":
                     case "FIELD_PG1_ROW_05_NOTES":
                     case "FIELD_PG1_ROW_06_NOTES":
                     case "FIELD_PG1_ROW_07_NOTES":
                     case "FIELD_PG1_ROW_08_NOTES":
                     case "FIELD_PG1_ROW_09_NOTES":
                     case "FIELD_PG1_ROW_10_NOTES":
                     case "FIELD_PG1_ROW_11_NOTES":
                        index = -1;
                        int.TryParse(s4bFormControl.fieldName.Substring(14, 2), out index);
                        if (index > 0)
                        {
                           notes[index - 1] = s4bFormControl.fieldValue;
                        }
                        break;

                     case "FIELD_PG1_ROW_01_TOTAL":
                     case "FIELD_PG1_ROW_02_TOTAL":
                     case "FIELD_PG1_ROW_03_TOTAL":
                     case "FIELD_PG1_ROW_04_TOTAL":
                     case "FIELD_PG1_ROW_05_TOTAL":
                     case "FIELD_PG1_ROW_06_TOTAL":
                     case "FIELD_PG1_ROW_07_TOTAL":
                     case "FIELD_PG1_ROW_08_TOTAL":
                     case "FIELD_PG1_ROW_09_TOTAL":
                     case "FIELD_PG1_ROW_10_TOTAL":
                     case "FIELD_PG1_ROW_11_TOTAL":
                        index = -1;
                        int.TryParse(s4bFormControl.fieldName.Substring(14, 2), out index);
                        if (index > 0)
                        {
                           double itemQtyTemp = 0;
                           Double.TryParse(s4bFormControl.fieldValue, out itemQtyTemp);
                           itemQty[index - 1] = itemQtyTemp;
                        }
                        break;

                  }
               }
            }
            if (userId <= 0)
            {
               userId = 1;
            }
            DiaryResources diaryResource = Utilities.GetDiaryResourceByUserId(request, userId);
            OrdersMin orderSecondary = s4bFormSubmission.OrderSecondary;
            // Construction of itemdesc part 2
            for (int counter = 0; counter < NO_ROWS; counter++)
            {
               if (!string.IsNullOrWhiteSpace(ud[counter]))
               {
                  createRow[counter] = true;
                  itemDescPart2[counter] += " " + ud[counter];
               }
               if (!string.IsNullOrWhiteSpace(workCompleted[counter]))
               {
                  createRow[counter] = true;
                  itemDescPart2[counter] += " " + workCompleted[counter];
               }
               if (!string.IsNullOrWhiteSpace(percentage[counter]))
               {
                  createRow[counter] = true;
                  itemDescPart2[counter] += " " + percentage[counter];
               }
               if (!string.IsNullOrWhiteSpace(timeFrom[counter]))
               {
                  createRow[counter] = true;
                  itemDescPart2[counter] += " " + timeFrom[counter];
               }
               if (!string.IsNullOrWhiteSpace(timeTo[counter]))
               {
                  createRow[counter] = true;
                  itemDescPart2[counter] += " " + timeTo[counter];
               }
               if (!string.IsNullOrWhiteSpace(mileage))
               {
                  itemDescPart2[counter] += " " + mileage;
               }
               if (!string.IsNullOrWhiteSpace(pymtTypeMultiplyDesc[counter]))
               {
                  createRow[counter] = true;
                  itemDescPart2[counter] += " " + pymtTypeMultiplyDesc[counter];
               }
               if (!string.IsNullOrWhiteSpace(notes[counter]))
               {
                  createRow[counter] = true;
                  itemDescPart2[counter] += " " + notes[counter];
               }
            }

            // Construction of Payment Type Multiplier
            for (int counter = 0; counter < 11; counter++)
            {
               pymtTypeMultiplier[counter] = 1;
               if (!string.IsNullOrWhiteSpace(pymtTypeMultiplyDesc[counter]))
               {
                  if (pymtTypeMultiplyDesc[counter] == "Double")
                  {
                     pymtTypeMultiplier[counter] = 2;
                  }
                  else if (pymtTypeMultiplyDesc[counter] == "Time & A Half")
                  {
                     pymtTypeMultiplier[counter] = 1.5;
                  }
               }
            }

            /// OI Data Insert For 
            /// 
            for (int counter = 0; counter < 11; counter++)
            {
               double amtLabour = 0;
               double amtMaterial = 0;
               double amtPlant = 0;
               double amtValueNew = 0;
               string itemDescFull = "";
               if (createRow[counter])
               {
                  itemDescFull = appDate.ToString("dd/MM/yyyy hh:mm") + " " + itemDesc[counter] + " " + itemDescPart2[counter]; ;
                  if (itemCode[counter] == null)
                  {
                     itemCode[counter] = "";
                  }
                  itemCode[counter] = itemCode[counter].Trim();
                  ProductListRepository productListRepo = new ProductListRepository();
                  productListRepo.IsSecondaryDatabase = true;
                  productListRepo.SecondaryDatabaseId = SimplicityConstants.DB_CONNECTIONSTRINGS_CAPELPLANT_ID;
                  ProductList productList = productListRepo.getProductListByCode(request, itemCode[counter]);
                  if (productList != null)
                  {
                     amtLabour = productList.AmountLabour * pymtTypeMultiplier[counter];
                     amtPlant = productList.AmountPlant * pymtTypeMultiplier[counter];
                     amtMaterial = productList.AmountMaterials * pymtTypeMultiplier[counter];
                     amtValueNew = amtLabour + amtPlant + amtMaterial;
                  }
                  OrderItems oi = new OrderItems();
                  oi.JobSequence = (orderSecondary == null ? -1 : orderSecondary.Sequence);
                  oi.RowIndex = 9999;
                  oi.FlgRowIsText = false;
                  oi.AssetSequence = -1;
                  oi.GroupId = 1;
                  oi.ItemType = 0;
                  oi.TransType = SimplicityConstants.ClientTransType;
                  oi.ItemCode = itemCode[counter];
                  oi.ItemDesc = itemDescFull;
                  oi.ItemUnits = "";
                  oi.ItemQuantity = itemQty[counter];
                  oi.AmountLabour = amtLabour;
                  oi.AmountMaterials = amtMaterial;
                  oi.AmountPlant = amtPlant;
                  oi.AmountValue = amtValueNew;
                  oi.AmountTotal = itemQty[counter] * amtValueNew;
                  oi.AssignedTo = -1;
                  if (diaryResource != null)
                  {
                     //oi.AssignedTo = diaryResource.JoinResource; // TODO: We need to get this from Secondary Databases
                  }
                  oi.FlgCompleted = true;
                  oi.FlgDocsRecd = true;
                  oi.CreatedBy = 1; // TODO: We need to get this from Secondary Databases
                  oi.DateCreated = timeStamp;
                  oi.LastAmendedBy = 1; // TODO: We need to get this from Secondary Databases
                  oi.DateLastAmended = timeStamp;
                  OrderItemsRepository orderItemsRepository = new OrderItemsRepository();
                  orderItemsRepository.IsSecondaryDatabase = true;
                  orderItemsRepository.SecondaryDatabaseId = SimplicityConstants.DB_CONNECTIONSTRINGS_CAPELPLANT_ID;
                  OrderItems oiNew = orderItemsRepository.CreateOrderItems(oi, request);
                  if (oiNew == null)
                  {
                     Message = Utilities.GenerateAndLogMessage(METHOD_NAME, orderItemsRepository.Message, null);
                  }
               }
            }
            returnValue = true;
         }
         catch (Exception ex)
         {
            Message = "Unable to Process Capel Group - Capel Recruitment Weekly Plant. Exception: " + ex.Message;
         }
         return returnValue;
      }

      // Capel Group - Capel Recruitment Weekly Timesheet 360000167
      // Capel Group - Capel Plant Sheet 360000168
      private bool ProcessCapelTimesheet(HttpRequest request, S4BFormRequest s4bFormRequest, S4bFormSubmissions s4bFormSubmission,
                                         RootObject root, Dictionary<string, S4BFormsControl> s4bFormsControls)
      {
         const string METHOD_NAME = "S4BFormsRepository.ProcessCapelTimesheet()";
         bool returnValue = false;
         int userId = Utilities.GetUserIdFromRequest(request);
         int index = -1;
         long jobSequence = -1, jobAddressId = -1, jobClientId = -1, appSequence = -1;
         string userName = "", jobRef = "";
         OrdersMin orderSecondary = s4bFormSubmission.OrderSecondary;
         string secondaryDatabaseId = Utilities.GetSecondaryIdByS4BFormId(s4bFormSubmission.RefNatForms.FormId);
         int TOTAL_NO_ROWS = secondaryDatabaseId.Equals(SimplicityConstants.DB_CONNECTIONSTRINGS_CAPELREC_ID, StringComparison.InvariantCultureIgnoreCase) ? 6 : 8;
         string[] employeeName = new string[TOTAL_NO_ROWS];
         DateTime[] timeFrom = new DateTime[TOTAL_NO_ROWS];
         DateTime[] timeTo = new DateTime[TOTAL_NO_ROWS];
         string[] pymtTypeDesc = new string[TOTAL_NO_ROWS];
         double[] pymtTypeMultiplier = new double[TOTAL_NO_ROWS];
         string[] notes = new string[TOTAL_NO_ROWS];
         double[] itemQty = new double[TOTAL_NO_ROWS];
         string[] itemDesc = new string[TOTAL_NO_ROWS];
         string[] minBreak = new string[TOTAL_NO_ROWS];
         string[] rowAssetName = new string[TOTAL_NO_ROWS];

         DateTime diaryDate = DateTime.Now;
         DateTime? timeStamp = s4bFormSubmission.DateCreated;
         Dictionary<string, string> imageValues = new Dictionary<string, string>();
         try
         {
            foreach (KeyValuePair<string, S4BFormsControl> s4bFormControlEntry in s4bFormsControls)
            {
               S4BFormsControl s4bFormControl = s4bFormControlEntry.Value;
               if (s4bFormControl.fieldValue != null)
               {
                  switch (s4bFormControlEntry.Key)
                  {
                     case "VARIABLE_PG1_JOB_SEQUENCE":
                        if (long.TryParse(s4bFormControl.fieldValue, out jobSequence))
                        {
                        };
                        break;
                     case "VARIABLE_PG1_JOB_CLIENT_ID":
                        if (long.TryParse(s4bFormControl.fieldValue, out jobClientId))
                        {
                        };
                        break;
                     case "VARIABLE_PG1_JOB_ADDRESS_ID":
                        if (long.TryParse(s4bFormControl.fieldValue, out jobAddressId))
                        {
                        };
                        break;
                     case "VARIABLE_PG1_USER_ID":
                        if (int.TryParse(s4bFormControl.fieldValue, out userId))
                        {
                        };
                        break;

                     case "VARIABLE_PG1_DIARY_RESOURCE":
                        userName = s4bFormControl.fieldValue;
                        break;

                     case "VAR_PG1_DIARY_DATE":
                     case "VAR_PG1_DIARY_ENTRY_DATE":
                        DateTime resultDate = DateTime.MinValue;
                        try
                        {
                           resultDate = DateTime.ParseExact(s4bFormControl.fieldValue.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                           diaryDate = resultDate;
                        }
                        catch (Exception ex)
                        { }
                        break;

                     case "VARIABLE_PG1_DIARY_ENTRY_ID":
                     case "VARIABLE_PG1_DA_SEQUENCE":
                     case "VARIABLE_PG1_DE_SEQUENCE":
                        long.TryParse(s4bFormControl.fieldValue, out appSequence);
                        break;

                     case "FIELD_PG1_JOB_REF":
                        jobRef = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG1_ROW_01_EMPLOYEE_NAME":
                     case "FIELD_PG1_ROW_02_EMPLOYEE_NAME":
                     case "FIELD_PG1_ROW_03_EMPLOYEE_NAME":
                     case "FIELD_PG1_ROW_04_EMPLOYEE_NAME":
                     case "FIELD_PG1_ROW_05_EMPLOYEE_NAME":
                     case "FIELD_PG1_ROW_06_EMPLOYEE_NAME":
                     case "FIELD_PG1_ROW_07_EMPLOYEE_NAME":
                     case "FIELD_PG1_ROW_08_EMPLOYEE_NAME":
                        index = -1;
                        int.TryParse(s4bFormControl.fieldName.Substring(14, 2), out index);
                        if (index > 0)
                        {
                           employeeName[index - 1] = s4bFormControl.fieldValue;
                        }
                        break;

                     case "VAR_PG1_ROW_01_TIME_FROM":
                     case "VAR_PG1_ROW_02_TIME_FROM":
                     case "VAR_PG1_ROW_03_TIME_FROM":
                     case "VAR_PG1_ROW_04_TIME_FROM":
                     case "VAR_PG1_ROW_05_TIME_FROM":
                     case "VAR_PG1_ROW_06_TIME_FROM":
                     case "VAR_PG1_ROW_07_TIME_FROM":
                     case "VAR_PG1_ROW_08_TIME_FROM":
                        index = -1;
                        int.TryParse(s4bFormControl.fieldName.Substring(12, 2), out index);
                        if (index > 0)
                        {
                           DateTime result = DateTime.MinValue;
                           DateTime.TryParse(s4bFormControl.fieldValue, out result);
                           timeFrom[index - 1] = result;
                        }
                        break;

                     case "VAR_PG1_ROW_01_TIME_TO":
                     case "VAR_PG1_ROW_02_TIME_TO":
                     case "VAR_PG1_ROW_03_TIME_TO":
                     case "VAR_PG1_ROW_04_TIME_TO":
                     case "VAR_PG1_ROW_05_TIME_TO":
                     case "VAR_PG1_ROW_06_TIME_TO":
                     case "VAR_PG1_ROW_07_TIME_TO":
                     case "VAR_PG1_ROW_08_TIME_TO":
                        index = -1;
                        int.TryParse(s4bFormControl.fieldName.Substring(12, 2), out index);
                        if (index > 0)
                        {
                           DateTime result = DateTime.MinValue;
                           DateTime.TryParse(s4bFormControl.fieldValue, out result);
                           timeTo[index - 1] = result;
                        }
                        break;

                     case "FIELD_PG1_ROW_01_30_MIN_BREAK":
                        minBreak[0] = s4bFormControl.fieldValue;
                        break;
                     case "FIELD_PG1_ROW_02_30_MIN_BREAK":
                        minBreak[1] = s4bFormControl.fieldValue;
                        break;
                     case "FIELD_PG1_ROW_03_30_MIN_BREAK":
                        minBreak[2] = s4bFormControl.fieldValue;
                        break;
                     case "FIELD_PG1_ROW_04_30_MIN_BREAK":
                        minBreak[3] = s4bFormControl.fieldValue;
                        break;
                     case "FIELD_PG1_ROW_05_30_MIN_BREAK":
                        minBreak[4] = s4bFormControl.fieldValue;
                        break;
                     case "FIELD_PG1_ROW_06_30_MIN_BREAK":
                        minBreak[5] = s4bFormControl.fieldValue;
                        break;
                     case "FIELD_PG1_ROW_07_30_MIN_BREAK":
                        minBreak[6] = s4bFormControl.fieldValue;
                        break;
                     case "FIELD_PG1_ROW_08_30_MIN_BREAK":
                        minBreak[7] = s4bFormControl.fieldValue;
                        break;

                     case "VAR_PG1_ROW01_TOTAL":
                     case "VAR_PG1_ROW02_TOTAL":
                     case "VAR_PG1_ROW03_TOTAL":
                     case "VAR_PG1_ROW04_TOTAL":
                     case "VAR_PG1_ROW05_TOTAL":
                     case "VAR_PG1_ROW06_TOTAL":
                     case "VAR_PG1_ROW07_TOTAL":
                     case "VAR_PG1_ROW08_TOTAL":
                        index = -1;
                        int.TryParse(s4bFormControl.fieldName.Substring(11, 2), out index);
                        if (index > 0)
                        {
                           double itemQtyTemp = 0;
                           Double.TryParse(s4bFormControl.fieldValue, out itemQtyTemp);
                           itemQty[index - 1] = itemQtyTemp;
                        }
                        break;

                     case "FIELD_PG1_ROW_01_PAYMENT_TYPE":
                     case "FIELD_PG1_ROW_02_PAYMENT_TYPE":
                     case "FIELD_PG1_ROW_03_PAYMENT_TYPE":
                     case "FIELD_PG1_ROW_04_PAYMENT_TYPE":
                     case "FIELD_PG1_ROW_05_PAYMENT_TYPE":
                     case "FIELD_PG1_ROW_06_PAYMENT_TYPE":
                     case "FIELD_PG1_ROW_07_PAYMENT_TYPE":
                     case "FIELD_PG1_ROW_08_PAYMENT_TYPE":
                        index = -1;
                        int.TryParse(s4bFormControl.fieldName.Substring(14, 2), out index);
                        if (index > 0)
                        {
                           pymtTypeDesc[index - 1] = s4bFormControl.fieldValue;
                        }
                        break;

                     case "FIELD_PG1_ROW_01_NOTES":
                     case "FIELD_PG1_ROW_02_NOTES":
                     case "FIELD_PG1_ROW_03_NOTES":
                     case "FIELD_PG1_ROW_04_NOTES":
                     case "FIELD_PG1_ROW_05_NOTES":
                     case "FIELD_PG1_ROW_06_NOTES":
                     case "FIELD_PG1_ROW_07_NOTES":
                     case "FIELD_PG1_ROW_08_NOTES":
                        index = -1;
                        int.TryParse(s4bFormControl.fieldName.Substring(14, 2), out index);
                        if (index > 0)
                        {
                           notes[index - 1] = s4bFormControl.fieldValue;
                        }
                        break;

                     case "FIELD_PG1_ROW_01_VEHICLE_PLANT":
                     case "FIELD_PG1_ROW_02_VEHICLE_PLANT":
                     case "FIELD_PG1_ROW_03_VEHICLE_PLANT":
                     case "FIELD_PG1_ROW_04_VEHICLE_PLANT":
                     case "FIELD_PG1_ROW_05_VEHICLE_PLANT":
                     case "FIELD_PG1_ROW_06_VEHICLE_PLANT":
                     case "FIELD_PG1_ROW_07_VEHICLE_PLANT":
                     case "FIELD_PG1_ROW_08_VEHICLE_PLANT":
                        index = -1;
                        int.TryParse(s4bFormControl.fieldName.Substring(14, 2), out index);
                        if (index > 0)
                        {
                           rowAssetName[index - 1] = s4bFormControl.fieldValue;
                        }
                        break;
                  }
               }
            }
            if (userId <= 0)
            {
               userId = 1;
            }
            for (int counter = 0; counter < TOTAL_NO_ROWS; counter++)
            {
               if (!string.IsNullOrWhiteSpace(employeeName[counter]) || !string.IsNullOrWhiteSpace(rowAssetName[counter]))
               {
                  TmpTimesheet tmpTimesheet = new TmpTimesheet();
                  tmpTimesheet.ImpRef = s4bFormSubmission.S4bSubmitNo;
                  tmpTimesheet.DataStatus = 0;
                  tmpTimesheet.UncWebSessionId = "";
                  tmpTimesheet.RowEmployeeName = employeeName[counter];
                  tmpTimesheet.DateRowStartTime = timeFrom[counter];
                  tmpTimesheet.DateRowFinishTime = timeTo[counter];
                  tmpTimesheet.RowTimeTotal = itemQty[counter];
                  tmpTimesheet.RowPymtType = pymtTypeDesc[counter];
                  tmpTimesheet.RowNotes = notes[counter];
                  tmpTimesheet.DateRowDate = diaryDate;
                  tmpTimesheet.RowJobRef = (orderSecondary == null ? "" : orderSecondary.JobRef);
                  tmpTimesheet.FlgJobRefValid = !s4bFormSubmission.IsInvalidOrderSecondary;
                  tmpTimesheet.JobSequence = (orderSecondary == null ? -1 : orderSecondary.Sequence);
                  tmpTimesheet.FlgPayrollEntry = false;
                  tmpTimesheet.FlgLessBreakTime = (!string.IsNullOrWhiteSpace(minBreak[counter]) && minBreak[counter].Equals("yes", StringComparison.InvariantCultureIgnoreCase)) ? true : false;
                  tmpTimesheet.RowAssetName = rowAssetName[counter];
                  tmpTimesheet.EntityId = -1;
                  tmpTimesheet.CreatedBy = 1;
                  tmpTimesheet.DateCreated = timeStamp;
                  tmpTimesheet.LastAmendedBy = 1;
                  tmpTimesheet.DateLastAmended = timeStamp;
                  TmpTimesheetRepository tmpTimesheetRepositorySecondary = new TmpTimesheetRepository();
                  tmpTimesheetRepositorySecondary.IsSecondaryDatabase = true;
                  tmpTimesheetRepositorySecondary.SecondaryDatabaseId = secondaryDatabaseId;
                  TmpTimesheet tmpTimesheetNew = tmpTimesheetRepositorySecondary.insert(request, tmpTimesheet);
                  if (tmpTimesheetNew == null)
                  {
                     Message = "Unable to Insert Row in Tmp Timesheet. Error: " + tmpTimesheetRepositorySecondary.Message;
                  }
               }
            }
            returnValue = true;
         }
         catch (Exception ex)
         {
            Message = "Unable to Process Timesheet for Capel Group - '" + s4bFormSubmission.RefNatForms.FormDesc + "'. Exception: " + ex.Message;
         }
         return returnValue;
      }

      // Capel Group - Capel Plant Vehicle And Driver Log Sheet V03 - 1114043282
      private bool ProcessCapelVehicleAndDriverLog(HttpRequest request, S4BFormRequest s4bFormRequest, S4bFormSubmissions s4bFormSubmission,
                                                   RootObject root, Dictionary<string, S4BFormsControl> s4bFormsControls)
      {
         const string METHOD_NAME = "S4BFormsRepository.ProcessCapelVehicleAndDriverLog()";
         bool returnValue = false;
         int userId = Utilities.GetUserIdFromRequest(request);
         int index = -1;
         long jobSequence = -1, jobAddressId = -1, jobClientId = -1, appSequence = -1;
         string userName = "", jobRef = "";
         OrdersMin orderSecondary = s4bFormSubmission.OrderSecondary;
         string secondaryDatabaseId = Utilities.GetSecondaryIdByS4BFormId(s4bFormSubmission.RefNatForms.FormId);
         const int TOTAL_NO_ROWS_COL1 = 11;
         const int TOTAL_NO_ROWS_COL2 = 10;
         const string DEFECT_VALUE = "DEFECT";
         bool defectFound = false;
         string[] checksCol1 = new string[TOTAL_NO_ROWS_COL1];
         string[] checksCol2 = new string[TOTAL_NO_ROWS_COL2];
         string vehicleReg = "";

         DateTime diaryDate = DateTime.Now;
         DateTime? timeStamp = s4bFormSubmission.DateCreated;
         Dictionary<string, string> imageValues = new Dictionary<string, string>();
         try
         {
            foreach (KeyValuePair<string, S4BFormsControl> s4bFormControlEntry in s4bFormsControls)
            {
               S4BFormsControl s4bFormControl = s4bFormControlEntry.Value;
               if (s4bFormControl.fieldValue != null)
               {
                  switch (s4bFormControlEntry.Key)
                  {
                     case "VARIABLE_PG1_JOB_SEQUENCE":
                        if (long.TryParse(s4bFormControl.fieldValue, out jobSequence))
                        {
                        };
                        break;
                     case "VARIABLE_PG1_JOB_CLIENT_ID":
                        if (long.TryParse(s4bFormControl.fieldValue, out jobClientId))
                        {
                        };
                        break;
                     case "VARIABLE_PG1_JOB_ADDRESS_ID":
                        if (long.TryParse(s4bFormControl.fieldValue, out jobAddressId))
                        {
                        };
                        break;
                     case "VARIABLE_PG1_USER_ID":
                        if (int.TryParse(s4bFormControl.fieldValue, out userId))
                        {
                        };
                        break;

                     case "VARIABLE_PG1_DIARY_RESOURCE":
                        userName = s4bFormControl.fieldValue;
                        break;

                     case "VAR_PG1_DIARY_DATE":
                        DateTime resultDate = DateTime.MinValue;
                        try
                        {
                           resultDate = DateTime.ParseExact(s4bFormControl.fieldValue.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                           diaryDate = resultDate;
                        }
                        catch (Exception ex)
                        { }
                        break;

                     case "VARIABLE_PG1_DIARY_ENTRY_ID":
                     case "VARIABLE_PG1_DA_SEQUENCE":
                     case "VARIABLE_PG1_DE_SEQUENCE":
                        long.TryParse(s4bFormControl.fieldValue, out appSequence);
                        break;

                     case "FIELD_PG1_JOB_REF":
                        jobRef = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG2_ROW01_COL01":
                     case "FIELD_PG2_ROW02_COL01":
                     case "FIELD_PG2_ROW03_COL01":
                     case "FIELD_PG2_ROW04_COL01":
                     case "FIELD_PG2_ROW05_COL01":
                     case "FIELD_PG2_ROW06_COL01":
                     case "FIELD_PG2_ROW07_COL01":
                     case "FIELD_PG2_ROW08_COL01":
                     case "FIELD_PG2_ROW09_COL01":
                     case "FIELD_PG2_ROW10_COL01":
                     case "FIELD_PG2_ROW11_COL01":
                        index = -1;
                        int.TryParse(s4bFormControl.fieldName.Substring(13, 2), out index);
                        if (index > 0)
                        {
                           checksCol1[index - 1] = s4bFormControl.fieldValue;
                        }
                        break;

                     case "FIELD_PG2_ROW01_COL02":
                     case "FIELD_PG2_ROW02_COL02":
                     case "FIELD_PG2_ROW03_COL02":
                     case "FIELD_PG2_ROW04_COL02":
                     case "FIELD_PG2_ROW05_COL02":
                     case "FIELD_PG2_ROW06_COL02":
                     case "FIELD_PG2_ROW07_COL02":
                     case "FIELD_PG2_ROW08_COL02":
                     case "FIELD_PG2_ROW09_COL02":
                     case "FIELD_PG2_ROW10_COL02":
                        index = -1;
                        int.TryParse(s4bFormControl.fieldName.Substring(13, 2), out index);
                        if (index > 0)
                        {
                           checksCol2[index - 1] = s4bFormControl.fieldValue;
                        }
                        break;

                     case "FIELD_PG1_VEHICLE_REGISTRATION":
                        vehicleReg = s4bFormControl.fieldValue;
                        break;
                  }
               }
            }
            if (userId <= 0)
            {
               userId = 1;
            }
            ProjectSettings settings = Configs.settings[request.Headers["ProjectId"]];
            if (!string.IsNullOrWhiteSpace(vehicleReg))
            {
               bool validJobFound = false;
               bool validDefaultVehcileRefNotFound = false;

               string jobRefVehcileReg = vehicleReg.Trim();
               OrdersRepository ordersRepository = new OrdersRepository(true, secondaryDatabaseId);
               Orders orders = ordersRepository.GetOrderByJobRef(jobRefVehcileReg, request);
               if (orders == null)
               {
                  validDefaultVehcileRefNotFound = true;
                  if (jobRefVehcileReg.Contains(" "))
                  {
                     jobRefVehcileReg = jobRefVehcileReg.Replace(" ", "");
                     orders = ordersRepository.GetOrderByJobRef(jobRefVehcileReg, request);
                     if (orders != null)
                     {
                        validJobFound = true;
                     }
                  }
               }
               else
               {
                  validJobFound = true;
               }
               if (validJobFound)
               {
                  string sourcePdfFilePath = Path.Combine(s4bFormSubmission.ContentPath, SimplicityConstants.S4BFormSubmittedTemplateName);
                  string sourceZipFilePath = Path.Combine(s4bFormSubmission.ZipFilePath);
                  string destinationFolder = Path.Combine(settings.SecondaryS4BFormsSubmissionsExportFolder[secondaryDatabaseId], jobRefVehcileReg, settings.FilingCabinetS4BFormsFolder);
                  string destinationPdfFilePath = Path.Combine(destinationFolder, Utilities.GenerateS4BeFormPdfFileName(s4bFormSubmission, 1));
                  string destinationZipFilePath = Path.Combine(destinationFolder, Utilities.GenerateS4BeFormZipFileName(s4bFormSubmission, 1));
                  string doneFilePath = Path.Combine(destinationFolder, SimplicityConstants.DONE_FILE_NAME);
                  if (!Directory.Exists(destinationFolder))
                  {
                     Directory.CreateDirectory(destinationFolder);
                  }
                  File.Copy(sourcePdfFilePath, destinationPdfFilePath, true);
                  File.Copy(sourceZipFilePath, destinationZipFilePath, true);
                  using (var fs = File.Create(doneFilePath))
                  {
                     fs.Close();
                  }
               }
               if (validDefaultVehcileRefNotFound)
               {
                  CldSettingsRepository cldSettingsRepository = new CldSettingsRepository();
                  CldSettings emailToSetting = cldSettingsRepository.GetCldSettingsBySettingName(request, SimplicityConstants.CldSettingS4BFormDefaultDistributionEmailAddress);
                  if (emailToSetting != null && !string.IsNullOrWhiteSpace(emailToSetting.SettingValue))
                  {
                     CldSettings emailSubjectSetting = cldSettingsRepository.GetCldSettingsBySettingName(request, (!validJobFound ? SimplicityConstants.CldSettingS4BFormSubmissionEmailSubjectCustom2 : SimplicityConstants.CldSettingS4BFormSubmissionEmailSubjectCustom3));
                     CldSettings emailContentSetting = cldSettingsRepository.GetCldSettingsBySettingName(request, (!validJobFound ? SimplicityConstants.CldSettingS4BFormSubmissionEmailContentCustom2 : SimplicityConstants.CldSettingS4BFormSubmissionEmailContentCustom3));
                     RefS4bForms s4bForm = s4bFormSubmission.RefNatForms;
                     string emailSubject = emailSubjectSetting.SettingValue;
                     emailSubject = Utilities.replaceS4BFormsKeywords(emailSubject, s4bFormRequest, s4bFormSubmission, s4bFormsControls);
                     string emailContent = emailContentSetting.SettingValue;
                     emailContent = Utilities.replaceS4BFormsKeywords(emailContent, s4bFormRequest, s4bFormSubmission, s4bFormsControls);
                     EmailContact fromContact = new EmailContact();
                     fromContact.EmailAddress = settings.AdminEmailAddress;
                     List<string> fileAttachments = new List<string>();
                     fileAttachments.Add(s4bFormSubmission.PdfFilePath);
                     if (!Utilities.SendMail(fromContact, Utilities.GetEmailContactsFromEmailAddresses(emailToSetting.SettingValue),
                                            null, null, emailSubject, emailContent, fileAttachments, "", ""))
                     {
                        Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error Occured While Sending Match Not Found Notification. " + Utilities.Message, null);
                     }
                  }
               }
            }
            for (int counter = 0; counter < TOTAL_NO_ROWS_COL1; counter++)
            {
               if (checksCol1[counter] != null && checksCol1[counter].Equals(DEFECT_VALUE, StringComparison.InvariantCultureIgnoreCase))
               {
                  defectFound = true;
               }
            }
            if (!defectFound)
            {
               for (int counter = 0; counter < TOTAL_NO_ROWS_COL2; counter++)
               {
                  if (checksCol2[counter] != null && checksCol2[counter].Equals(DEFECT_VALUE, StringComparison.InvariantCultureIgnoreCase))
                  {
                     defectFound = true;
                  }
               }
            }
            if (defectFound)
            {
               CldSettingsRepository cldSettingsRepository = new CldSettingsRepository();
               CldSettings emailToSetting = cldSettingsRepository.GetCldSettingsBySettingName(request, SimplicityConstants.CldSettingS4BFormDefaultDistributionEmailAddress);
               if (emailToSetting != null && !string.IsNullOrWhiteSpace(emailToSetting.SettingValue))
               {
                  CldSettings emailSubjectSetting = cldSettingsRepository.GetCldSettingsBySettingName(request, SimplicityConstants.CldSettingS4BFormSubmissionEmailSubjectCustom1);
                  CldSettings emailContentSetting = cldSettingsRepository.GetCldSettingsBySettingName(request, SimplicityConstants.CldSettingS4BFormSubmissionEmailContentCustom1);
                  RefS4bForms s4bForm = s4bFormSubmission.RefNatForms;
                  string emailSubject = emailSubjectSetting.SettingValue;
                  emailSubject = Utilities.replaceS4BFormsKeywords(emailSubject, s4bFormRequest, s4bFormSubmission, s4bFormsControls);
                  string emailContent = emailContentSetting.SettingValue;
                  emailContent = Utilities.replaceS4BFormsKeywords(emailContent, s4bFormRequest, s4bFormSubmission, s4bFormsControls);
                  List<string> fileAttachments = new List<string>();
                  fileAttachments.Add(s4bFormSubmission.PdfFilePath);
                  EmailContact fromContact = new EmailContact();
                  fromContact.EmailAddress = settings.AdminEmailAddress;
                  if (!Utilities.SendMail(fromContact, Utilities.GetEmailContactsFromEmailAddresses(emailToSetting.SettingValue),
                                         null, null, emailSubject, emailContent, fileAttachments, "", ""))
                  {
                     Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error Occured While Sending Defect Report. " + Utilities.Message, null);
                  }
               }
            }
            returnValue = true;
         }
         catch (Exception ex)
         {
            Message = "Unable to Process Vehcile and Drive Log for Capel Group - '" + s4bFormSubmission.RefNatForms.FormDesc + "'. Exception: " + ex.Message;
         }
         return returnValue;
      }
		// Capel Group - Small Tools 13 Week Maintenance Check Sheet V01 (1673956019)
		private bool ProcessCapelSmallToolWeekMaintenance(HttpRequest request, S4BFormRequest s4bFormRequest, S4bFormSubmissions s4bFormSubmission,
													 RootObject root, Dictionary<string, S4BFormsControl> s4bFormsControls)
		{
			const string METHOD_NAME = "S4BFormsRepository.ProcessCapelSmallToolWeekMaintenance()";
			bool returnValue = false;
			int userId = Utilities.GetUserIdFromRequest(request);
			int checksCol1index = 0, checksCol2Index = 0, rowNo = -1, test_item_id = -1;
			long jobSequence = -1, jobAddressId = -1, jobClientId = -1, appSequence = -1, hours = -1;
			string assetSequence = "";
			string userName = "", jobRef = "", jobClientRef = "", location = "", signature = "", submit = "";
			OrdersMin orderSecondary = s4bFormSubmission.OrderSecondary;
			string secondaryDatabaseId = Utilities.GetSecondaryIdByS4BFormId(s4bFormSubmission.RefNatForms.FormId);
			const int TOTAL_NO_ROWS_COL1 = 29;
			const int TOTAL_NO_ROWS_COL2 = 11;
			const int TOTAL_NO_ROWS_Page3 = 9;
			string[] checksCol1 = new string[TOTAL_NO_ROWS_COL1];
			List<int> testItemIds = new List<int>();
			List<string> inputComments = new List<string>();
			string[] checksCol2 = new string[TOTAL_NO_ROWS_COL2];
			string[] checksCol3 = new string[TOTAL_NO_ROWS_Page3];

			DateTime diaryDate = DateTime.Now;
			DateTime resultDate = DateTime.MinValue;
			DateTime? timeStamp = s4bFormSubmission.DateCreated;
			Dictionary<string, string> imageValues = new Dictionary<string, string>();
			try
			{
				foreach (KeyValuePair<string, S4BFormsControl> s4bFormControlEntry in s4bFormsControls)
				{
					S4BFormsControl s4bFormControl = s4bFormControlEntry.Value;
					if (s4bFormControl.fieldValue != null)
					{
						switch (s4bFormControlEntry.Key)
						{
							case "VARIABLE_PG1_JOB_SEQUENCE":
								if (long.TryParse(s4bFormControl.fieldValue, out jobSequence))
								{
								};
								break;
							case "VARIABLE_PG1_JOB_CLIENT_ID":
								if (long.TryParse(s4bFormControl.fieldValue, out jobClientId))
								{
								};
								break;
							case "VARIABLE_PG1_JOB_ADDRESS_ID":
								if (long.TryParse(s4bFormControl.fieldValue, out jobAddressId))
								{
								};
								break;
							case "VARIABLE_PG1_USER_ID":
								if (int.TryParse(s4bFormControl.fieldValue, out userId))
								{
								};
								break;

							case "VAR_PG1_DIARY_RESOURCE":
								userName = s4bFormControl.fieldValue;
								break;

							case "FIELD_PG1_DATE":

								try
								{
									resultDate = DateTime.ParseExact(s4bFormControl.fieldValue.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
									diaryDate = resultDate;
								}
								catch (Exception ex)
								{ }
								break;

							case "VARIABLE_PG1_DIARY_ENTRY_ID":
							case "VARIABLE_PG1_DA_SEQUENCE":
							case "VARIABLE_PG1_DE_SEQUENCE":
								long.TryParse(s4bFormControl.fieldValue, out appSequence);
								break;
							case "VAR_PG1_JOB_REF":
								jobRef = s4bFormControl.fieldValue;
								break;
							case "VAR_PG1_JOB_CLIENT_REF":
								jobClientRef = s4bFormControl.fieldValue;
								break;
							case "FIELD_PG1_ASSET_ID":
								assetSequence = s4bFormControl.fieldValue;
								break;
							case "FIELD_PG1_HOURS":
								long.TryParse(s4bFormControl.fieldValue, out hours);
								break;
							case "FIELD_PG1_LOCATION":
								location = s4bFormControl.fieldValue;
								break;
							case "FIELD_PG1_T_12_ID_247":
							case "FIELD_PG1_T_12_ID_248":
							case "FIELD_PG1_T_12_ID_249":
							case "FIELD_PG1_T_12_ID_250":
							case "FIELD_PG1_T_12_ID_251":
							case "FIELD_PG1_T_12_ID_252":
							case "FIELD_PG1_T_12_ID_253":
							case "FIELD_PG1_T_12_ID_254":
							case "FIELD_PG1_T_12_ID_256":
							case "FIELD_PG1_T_12_ID_257":
							case "FIELD_PG1_T_12_ID_258":
							case "FIELD_PG1_T_12_ID_259":
							case "FIELD_PG1_T_12_ID_260":
							case "FIELD_PG1_T_12_ID_261":
							case "FIELD_PG1_T_12_ID_262":
							case "FIELD_PG1_T_12_ID_264":
							case "FIELD_PG1_T_12_ID_265":
							case "FIELD_PG1_T_12_ID_266":
							case "FIELD_PG1_T_12_ID_692":
							case "FIELD_PG1_T_12_ID_267":
							case "FIELD_PG1_T_12_ID_268":
							case "FIELD_PG1_T_12_ID_269":
							case "FIELD_PG1_T_12_ID_270":
							case "FIELD_PG1_T_12_ID_697":
							case "FIELD_PG1_T_12_ID_272":
							case "FIELD_PG1_T_12_ID_273":
							case "FIELD_PG1_T_12_ID_274":
							case "FIELD_PG1_T_12_ID_275":
							case "FIELD_PG1_T_12_ID_276":
								if (checksCol1index >= 0)
								{
									checksCol1[checksCol1index] = s4bFormControl.fieldValue;
									checksCol1index += 1;
								}
								test_item_id = -1;
								int.TryParse(s4bFormControlEntry.Key.Substring(18, 3), out test_item_id);
								if (test_item_id > 0)
								{
									testItemIds.Add(test_item_id);
									inputComments.Add(s4bFormControl.fieldValue);
								}
								break;

							case "FIELD_PG2_T_12_ID_278":
							case "FIELD_PG2_T_12_ID_279":
							case "FIELD_PG2_T_12_ID_280":
							case "FIELD_PG2_T_12_ID_281":
							case "FIELD_PG2_T_12_ID_282":
							case "FIELD_PG2_T_12_ID_283":
							case "FIELD_PG2_T_12_ID_284":
							case "FIELD_PG2_T_12_ID_285":
							case "FIELD_PG2_T_12_ID_287":
							case "FIELD_PG2_T_12_ID_288":
							case "FIELD_PG2_T_12_ID_696":
								if (checksCol2Index > 0)
								{
									checksCol2[checksCol2Index] = s4bFormControl.fieldValue;
									checksCol2Index += 1;
								}
								test_item_id = -1;
								int.TryParse(s4bFormControlEntry.Key.Substring(18, 3), out test_item_id);
								if (test_item_id > 0)
								{
									testItemIds.Add(test_item_id);
									inputComments.Add(s4bFormControl.fieldValue);
								}
								break;
							case "Field_pg2_signature":
								signature = s4bFormControl.fieldValue;
								break;
							case "FIELD_PG2_SUBMIT":
								submit = s4bFormControl.fieldValue;
								break;
							case "FIELD_PG3_ROW01_ITEM":
							case "FIELD_PG3_ROW02_ITEM":
							case "FIELD_PG3_ROW03_ITEM":
							case "FIELD_PG3_ROW04_ITEM":
							case "FIELD_PG3_ROW05_ITEM":
							case "FIELD_PG3_ROW06_ITEM":
							case "FIELD_PG3_ROW07_ITEM":
							case "FIELD_PG3_ROW08_ITEM":
							case "FIELD_PG3_ROW09_ITEM":
								rowNo = -1;
								int.TryParse(s4bFormControl.fieldName.Substring(13, 2), out rowNo);
								if (rowNo > 0)
								{
									checksCol3[rowNo - 1] = s4bFormControl.fieldValue;
								}
								break;
							case "FIELD_PG3_ROW01_DETAILS":
							case "FIELD_PG3_ROW02_DETAILS":
							case "FIELD_PG3_ROW03_DETAILS":
							case "FIELD_PG3_ROW04_DETAILS":
							case "FIELD_PG3_ROW05_DETAILS":
							case "FIELD_PG3_ROW06_DETAILS":
							case "FIELD_PG3_ROW07_DETAILS":
							case "FIELD_PG3_ROW08_DETAILS":
							case "FIELD_PG3_ROW09_DETAILS":
								rowNo = -1;
								int.TryParse(s4bFormControl.fieldName.Substring(13, 2), out rowNo);
								if (rowNo > 0)
								{
									checksCol3[rowNo - 1] = s4bFormControl.fieldValue;
								}
								break;
							case "FIELD_PG3_ROW01_ACTION":
							case "FIELD_PG3_ROW02_ACTION":
							case "FIELD_PG3_ROW03_ACTION":
							case "FIELD_PG3_ROW04_ACTION":
							case "FIELD_PG3_ROW05_ACTION":
							case "FIELD_PG3_ROW06_ACTION":
							case "FIELD_PG3_ROW07_ACTION":
							case "FIELD_PG3_ROW08_ACTION":
							case "FIELD_PG3_ROW09_ACTION":
								rowNo = -1;
								int.TryParse(s4bFormControl.fieldName.Substring(13, 2), out rowNo);
								if (rowNo > 0)
								{
									checksCol3[rowNo - 1] = s4bFormControl.fieldValue;
								}
								break;
							case "FIELD_PG3_ROW01_DATE":
							case "FIELD_PG3_ROW02_DATE":
							case "FIELD_PG3_ROW03_DATE":
							case "FIELD_PG3_ROW04_DATE":
							case "FIELD_PG3_ROW05_DATE":
							case "FIELD_PG3_ROW06_DATE":
							case "FIELD_PG3_ROW07_DATE":
							case "FIELD_PG3_ROW08_DATE":
							case "FIELD_PG3_ROW09_DATE":
								rowNo = -1;
								int.TryParse(s4bFormControl.fieldName.Substring(13, 2), out rowNo);
								if (rowNo > 0)
								{
									checksCol3[rowNo - 1] = s4bFormControl.fieldValue;
								}
								break;
							case "FIELD_PG3_ROW01_TIME_START":
							case "FIELD_PG3_ROW02_TIME_START":
							case "FIELD_PG3_ROW03_TIME_START":
							case "FIELD_PG3_ROW04_TIME_START":
							case "FIELD_PG3_ROW05_TIME_START":
							case "FIELD_PG3_ROW06_TIME_START":
							case "FIELD_PG3_ROW07_TIME_START":
							case "FIELD_PG3_ROW08_TIME_START":
							case "FIELD_PG3_ROW09_TIME_START":
								rowNo = -1;
								int.TryParse(s4bFormControl.fieldName.Substring(13, 2), out rowNo);
								if (rowNo > 0)
								{
									checksCol3[rowNo - 1] = s4bFormControl.fieldValue;
								}
								break;
							case "FIELD_PG3_ROW01_TIME_END":
							case "FIELD_PG3_ROW02_TIME_END":
							case "FIELD_PG3_ROW03_TIME_END":
							case "FIELD_PG3_ROW04_TIME_END":
							case "FIELD_PG3_ROW05_TIME_END":
							case "FIELD_PG3_ROW06_TIME_END":
							case "FIELD_PG3_ROW07_TIME_END":
							case "FIELD_PG3_ROW08_TIME_END":
							case "FIELD_PG3_ROW09_TIME_END":
								rowNo = -1;
								int.TryParse(s4bFormControl.fieldName.Substring(13, 2), out rowNo);
								if (rowNo > 0)
								{
									checksCol3[rowNo - 1] = s4bFormControl.fieldValue;
								}
								break;
							case "VAR_PG3_ROW01_TOTAL_HOURS":
							case "VAR_PG3_ROW02_TOTAL_HOURS":
							case "VAR_PG3_ROW03_TOTAL_HOURS":
							case "VAR_PG3_ROW04_TOTAL_HOURS":
							case "VAR_PG3_ROW05_TOTAL_HOURS":
							case "VAR_PG3_ROW06_TOTAL_HOURS":
							case "VAR_PG3_ROW07_TOTAL_HOURS":
							case "VAR_PG3_ROW08_TOTAL_HOURS":
							case "VAR_PG3_ROW09_TOTAL_HOURS":
								rowNo = -1;
								int.TryParse(s4bFormControl.fieldName.Substring(13, 2), out rowNo);
								if (rowNo > 0)
								{
									checksCol3[rowNo - 1] = s4bFormControl.fieldValue;
								}
								break;
							case "FIELD_PG3_SIGNATURE":
								break;
						}
					}
				}
				if (userId <= 0)
				{
					userId = 1;
				}
				ProjectSettings settings = Configs.settings[request.Headers["ProjectId"]];
				//Get Diary Resource
				DiaryApps diaryApp = null;
				DiaryAppsRepository diaryAppsRepository = new DiaryAppsRepository();
				long entityId = diaryAppsRepository.GetDiaryResourceSequenceByUserId(userId, request, null);

				AssetTestHRepository assetTestHRepos = new AssetTestHRepository();
				//------- Get Asset Id
				Utilities.WriteLog("Asset Sequence = :" + assetSequence);
				long assetId = assetTestHRepos.GetAssetId(assetSequence, request);
				Utilities.WriteLog("Find Asset Id=" + assetId);
				//---Lock All previous asset records
				assetTestHRepos.UpdateLocked(assetId, request);
				//----Insert into assetTestH
				AssetTestH assetTestH = new AssetTestH();
				assetTestH.AssetSequence = assetId;
				assetTestH.TypeSequence = 12;
				assetTestH.DateCheck = resultDate;
				assetTestH.EntityId = entityId;// (diaryApp != null ? diaryApp.ResourceSequence : -1);
				Utilities.WriteLog("entity Id = :" + entityId);
				assetTestH.FlgLocked = false;
				assetTestH.FlgComplete = true;
				assetTestH.CheckLocation = location;
				assetTestH.EngineHours = Convert.ToInt64(hours);
				ResponseModel retAssetTestH = assetTestHRepos.Insert(assetTestH, request);
				if (retAssetTestH.IsSucessfull == true)
				{
					assetTestH = (AssetTestH)retAssetTestH.TheObject;
					long assetTestHSequence = assetTestH.Sequence ?? 0;
					int count = 0;

					//----Insert into assetTestI
					foreach (int id in testItemIds)
					{
						AssetTestI assetTestI = new AssetTestI();
						assetTestI.JoinSequence = assetTestHSequence;
						assetTestI.AssetSequence = assetId;
						assetTestI.TestItemId = id;
						assetTestI.InputComments = inputComments[count].ToString();
						assetTestI.CreatedBy = userId;
						assetTestI.DateCreated = DateTime.Now;
						DatabaseInfo dbInfo = Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId);
						AssetTestIDB assetTestIDB = new AssetTestIDB(dbInfo);
						long sequence = -1;
						assetTestIDB.insert(out sequence, assetTestI, 12);
						count += 1;
					}
					//---Insert Extra rows For Type Sequence=12
					if (assetTestH.TypeSequence == 12)
					{
						DatabaseInfo dbInfo = Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId);
						AssetTestIDB assetTestIDB = new AssetTestIDB(dbInfo);
						assetTestIDB.insertMissingTestItem(assetTestHSequence, assetId, 12, userId);
					}

				}
				else
				{
					Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "Unable to Insert Asset Test Header Row for S4BForm '" + s4bFormSubmission.RefNatForms.FormDesc + "' and Template id " + s4bFormSubmission.RefNatForms.FormId + " Reason:" + assetTestHRepos.Message, null);
				}
				returnValue = true;
			}
			catch (Exception ex)
			{
				Message = "Unable to Process Small Tools 13 Week Maintenance Check Sheet V01 for Capel Group - '" + s4bFormSubmission.RefNatForms.FormDesc + "'. Exception: " + ex.Message;
			}
			return returnValue;
		}

		// Capel Group - VehicleTestV01 (437868189) //following is depreceated and will not use
		private bool ProcessCapelKSPlantVehicleTestV01(HttpRequest request, S4BFormRequest s4bFormRequest, S4bFormSubmissions s4bFormSubmission,
													 RootObject root, Dictionary<string, S4BFormsControl> s4bFormsControls)
		{
			const string METHOD_NAME = "S4BFormsRepository.ProcessCapelKSPlantVehicleTestV0()";
			bool returnValue = false;
			int userId = Utilities.GetUserIdFromRequest(request);
			int rowId = -1;
			long jobSequence = -1, jobAddressId = -1, jobClientId = -1, appSequence = -1, diaryResourceId = -1;
			string assetSequence = "";
			string userName = "", jobRef = "", location = "", signature = "", submit = "", capelNo = "", vehicleReg = "", mileage = "";

			bool flgTapleyTest = false, flgRollerBrakeTest = false;
			int ladenType = 0, roadConditionType = 0, brakeTestSpeedType = 0, brakeTest = 0;
			double brakeTestSpeed = 0;
			OrdersMin orderSecondary = s4bFormSubmission.OrderSecondary;
			string secondaryDatabaseId = Utilities.GetSecondaryIdByS4BFormId(s4bFormSubmission.RefNatForms.FormId);
			const int TOTAL_NO_ROWS_COL1 = 31;
			const int TOTAL_NO_ROWS_COL2 = 33;
			const int TOTAL_NO_ROWS_Page3 = 18;
			string[] checksCol1 = new string[TOTAL_NO_ROWS_COL1];
			string[] checksCol2 = new string[TOTAL_NO_ROWS_COL2];
			string[] checksCol3 = new string[TOTAL_NO_ROWS_Page3];
			List<int> sectionIds = new List<int>();
			List<int> rowIds = new List<int>();
			List<int> inputData = new List<int>();
			List<string> imNumber = new List<string>();
			List<string> faultDesc = new List<string>();

			DateTime diaryDate = DateTime.Now;
			DateTime testDate = DateTime.MinValue;
			DateTime row01Date = DateTime.Now;
			DateTime? timeStamp = s4bFormSubmission.DateCreated;
			Dictionary<string, string> imageValues = new Dictionary<string, string>();
			try
			{
				foreach (KeyValuePair<string, S4BFormsControl> s4bFormControlEntry in s4bFormsControls)
				{
					S4BFormsControl s4bFormControl = s4bFormControlEntry.Value;
					if (s4bFormControl.fieldValue != null)
					{
						switch (s4bFormControlEntry.Key)
						{
							case "VARIABLE_PG1_JOB_SEQUENCE":
								if (long.TryParse(s4bFormControl.fieldValue, out jobSequence))
								{
								};
								break;
							case "VARIABLE_PG1_JOB_CLIENT_ID":
								if (long.TryParse(s4bFormControl.fieldValue, out jobClientId))
								{
								};
								break;
							case "VARIABLE_PG1_JOB_ADDRESS_ID":
								if (long.TryParse(s4bFormControl.fieldValue, out jobAddressId))
								{
								};
								break;
							case "VARIABLE_PG1_USER_ID":
								if (int.TryParse(s4bFormControl.fieldValue, out userId))
								{
								};
								break;
							case "VAR_PG1_DIARY_RESOURCE":
								userName = s4bFormControl.fieldValue;
								break;
							case "FIELD_PG1_DATE":
								try
								{
									testDate = DateTime.ParseExact(s4bFormControl.fieldValue.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
								}
								catch (Exception ex)
								{ }
								break;
							case "VARIABLE_PG1_DIARY_ENTRY_ID":
								long.TryParse(s4bFormControl.fieldValue, out appSequence);
								break;
							case "VAR_PG1_JOB_REF":
								jobRef = s4bFormControl.fieldValue;
								break;
							case "VAR_PG1_CAPEL_NO":
								capelNo = s4bFormControl.fieldValue;
								break;
							case "FIELD_PG1_VEHICLE_REG":
								vehicleReg = s4bFormControl.fieldValue;
								assetSequence = s4bFormControl.fieldValue;
								break;
							case "FIELD_PG1_LOCATION":
								location = s4bFormControl.fieldValue;
								break;
							case "FIELD_PG1_MILEAGE":
								mileage = s4bFormControl.fieldName;
								break;
							case "FIELD_PG1_T_01_ID_123":
							case "FIELD_PG1_T_01_ID_001":
							case "FIELD_PG1_T_01_ID_002":
							case "FIELD_PG1_T_01_ID_003":
							case "FIELD_PG1_T_01_ID_004":
							case "FIELD_PG1_T_01_ID_005":
							case "FIELD_PG1_T_01_ID_006":
							case "FIELD_PG1_T_01_ID_007":
							case "FIELD_PG1_T_01_ID_008":
							case "FIELD_PG1_T_01_ID_009":
							case "FIELD_PG1_T_01_ID_010":
							case "FIELD_PG1_T_01_ID_011":
							case "FIELD_PG1_T_01_ID_012":
							case "FIELD_PG1_T_01_ID_013":
							case "FIELD_PG1_T_01_ID_014":
							case "FIELD_PG1_T_01_ID_015":
							case "FIELD_PG1_T_01_ID_016":
							case "FIELD_PG1_T_01_ID_017":
							case "FIELD_PG1_T_01_ID_018":
							case "FIELD_PG1_T_01_ID_019":
							case "FIELD_PG1_T_01_ID_020":
							case "FIELD_PG1_T_01_ID_021":
							case "FIELD_PG1_T_01_ID_022":
							case "FIELD_PG1_T_01_ID_023":
							case "FIELD_PG1_T_01_ID_024":
							case "FIELD_PG1_T_01_ID_025":
							case "FIELD_PG1_T_01_ID_026":
							case "FIELD_PG1_T_01_ID_027":
							case "FIELD_PG1_T_01_ID_028":
							case "FIELD_PG1_T_01_ID_029":
							case "FIELD_PG2_T_01_ID_030":
							case "FIELD_PG2_T_01_ID_031":
							case "FIELD_PG2_T_01_ID_032":
							case "FIELD_PG2_T_01_ID_033":
							case "FIELD_PG2_T_01_ID_034":
							case "FIELD_PG2_T_01_ID_035":
							case "FIELD_PG2_T_01_ID_036":
							case "FIELD_PG2_T_01_ID_037":
							case "FIELD_PG2_T_01_ID_038":
							case "FIELD_PG2_T_01_ID_039":
							case "FIELD_PG2_T_01_ID_040":
							case "FIELD_PG2_T_01_ID_041":
							case "FIELD_PG2_T_01_ID_042":
							case "FIELD_PG2_T_01_ID_043":
							case "FIELD_PG2_T_01_ID_045":
							case "FIELD_PG2_T_01_ID_046":
							case "FIELD_PG2_T_01_ID_047":
							case "FIELD_PG2_T_01_ID_051":
							case "FIELD_PG2_T_01_ID_052":
							case "FIELD_PG2_T_01_ID_053":
							case "FIELD_PG2_T_01_ID_055":
							case "FIELD_PG2_T_01_ID_056":
							case "FIELD_PG2_T_01_ID_057":
							case "FIELD_PG2_T_01_ID_058":
							case "FIELD_PG2_T_01_ID_059":
							case "FIELD_PG2_T_01_ID_060":
							case "FIELD_PG2_T_01_ID_061":
							case "FIELD_PG2_T_01_ID_062":
							case "FIELD_PG2_T_01_ID_063":
							case "FIELD_PG2_T_01_ID_064":
							case "FIELD_PG2_T_01_ID_065":
							case "FIELD_PG2_T_01_ID_066":
							case "FIELD_PG2_T_01_ID_067":
							case "FIELD_PG3_T_01_ID_068":
							case "FIELD_PG3_T_01_ID_069":
							case "FIELD_PG3_T_01_ID_070":
							case "FIELD_PG3_T_01_ID_071":
							case "FIELD_PG3_T_01_ID_072":
							case "FIELD_PG2_T_12_ID_278":
							case "FIELD_PG2_T_12_ID_279":
							case "FIELD_PG2_T_12_ID_280":
							case "FIELD_PG2_T_12_ID_281":
							case "FIELD_PG2_T_12_ID_282":
							case "FIELD_PG2_T_12_ID_283":
							case "FIELD_PG2_T_12_ID_284":
							case "FIELD_PG2_T_12_ID_285":
							case "FIELD_PG2_T_12_ID_287":
							case "FIELD_PG2_T_12_ID_288":
							case "FIELD_PG2_T_12_ID_696":
							case "FIELD_PG3_T_01_ID_126":
							case "FIELD_PG3_T_01_ID_076":
							case "FIELD_PG3_T_01_ID_077":
							case "FIELD_PG3_T_01_ID_078":
							case "FIELD_PG3_T_01_ID_080":
							case "FIELD_PG3_T_01_ID_081":
							case "FIELD_PG3_T_01_ID_083":
							case "FIELD_PG3_T_01_ID_087":
							case "FIELD_PG3_T_01_ID_127":
							case "FIELD_PG3_T_01_ID_125":
								//if (checksCol1index >= 0)
								//{
								//    checksCol1[checksCol1index] = s4bFormControl.fieldValue;
								//    checksCol1index += 1;
								//}
								rowId = -1;
								int.TryParse(s4bFormControlEntry.Key.Substring(18, 3), out rowId);
								if (rowId > 0)
								{
									rowIds.Add(rowId);
									if (rowId >= 1 && rowId <= 21) // between row 21 to 22
										sectionIds.Add(1);
									else if (rowId >= 22 && rowId <= 29 || rowId == 123 || rowId == 124)
										sectionIds.Add(2);
									else if (rowId >= 30 && rowId <= 36) // between row 30 to 36
										sectionIds.Add(3);
									else if (rowId >= 37 && rowId <= 72 || (rowId >= 129 && rowId <= 131))
										sectionIds.Add(4);
									else if (rowId >= 76 && rowId <= 78 || (rowId >= 125 && rowId <= 128) || rowId == 81 || rowId == 83 || rowId == 87)
										sectionIds.Add(6);
									else if (rowId >= 89 && rowId <= 91)
										sectionIds.Add(7);
									else if (rowId >= 92 && rowId <= 99 || (rowId >= 118 && rowId <= 122))
										sectionIds.Add(8);
									else
										sectionIds.Add(0);
									if (s4bFormControl.fieldValue.ToUpper() == "OK")
										inputData.Add(1);
									else if (s4bFormControl.fieldValue.ToUpper() == "X")
										inputData.Add(2);
									else if (s4bFormControl.fieldValue.ToUpper() == "NC")
										inputData.Add(3);
									else if (s4bFormControl.fieldValue.ToUpper() == "N/A")
										inputData.Add(4);
									else if (s4bFormControl.fieldValue.ToUpper() == "M")
										inputData.Add(5);
									else
										inputData.Add(0);
								}
								//string pageNo = s4bFormControlEntry.Key.Substring(6, 3);
								//if (pageNo == "PG2")
								//{
								//    imNumber.Add(sectionId.ToString());
								//    faultDesc.Add(s4bFormControl.fieldValue);
								//}
								break;
							case "FIELD_PG3_ROAD_CONDITIONS":
								if (s4bFormControl.fieldValue.ToUpper() == "NOT SET")
								{
									roadConditionType = 0;
								}
								else if (s4bFormControl.fieldValue.ToUpper() == "WET")
								{
									roadConditionType = 1;
								}
								else if (s4bFormControl.fieldValue.ToUpper() == "DRY")
								{
									roadConditionType = 2;
								}
								break;
							case "FIELD_PG3_BRAKE_TEST_TYPE":

								if (s4bFormControl.fieldValue.ToUpper() == "NOT SET")
								{
									brakeTestSpeedType = 0;
								}
								else if (s4bFormControl.fieldValue.ToUpper() == "KPH")
								{
									brakeTestSpeedType = 1;
								}
								else if (s4bFormControl.fieldValue.ToUpper() == "MPH")
								{
									brakeTestSpeedType = 2;
								}
								break;

							case "FIELD_PG3_BRAKE_TEST_SPEED":
								brakeTestSpeed = double.Parse(s4bFormControl.fieldValue);
								break;
							case "FIELD_PG3_SIGNATURE":
								signature = s4bFormControl.fieldValue;
								break;
							case "FIELD_PG3_SUBMIT":
								submit = s4bFormControl.fieldValue;
								break;

							case "FIELD_PG4_ROW01_DATE":
							case "FIELD_PG4_ROW02_DATE":
								try
								{
									row01Date = DateTime.ParseExact(s4bFormControl.fieldValue.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
								}
								catch (Exception ex)
								{ }
								break;
							case "FIELD_PG3_VEHICLE_LADEN":
								if (s4bFormControl.fieldValue.ToUpper() == "NOT SET")
								{
									ladenType = 0;
								}
								else if (s4bFormControl.fieldValue.ToUpper() == "LADEN")
								{
									ladenType = 1;
								}
								else if (s4bFormControl.fieldValue.ToUpper() == "UNLADEN")
								{
									ladenType = 2;
								}
								break;
							case "FIELD_PG4_MAKE":
								break;
							case "FIELD_PG4_MODEL":
								break;
							case "FIELD_PG4_ROW01_DECTIVE_ITEM":
							case "FIELD_PG4_ROW02_DECTIVE_ITEM":
							case "FIELD_PG4_ROW03_DECTIVE_ITEM":
							case "FIELD_PG4_ROW04_DECTIVE_ITEM":
							case "FIELD_PG4_ROW05_DECTIVE_ITEM":
							case "FIELD_PG4_ROW06_DECTIVE_ITEM":
							case "FIELD_PG4_ROW07_DECTIVE_ITEM":
							case "FIELD_PG4_ROW08_DECTIVE_ITEM":
							case "FIELD_PG4_ROW09_DECTIVE_ITEM":
							case "FIELD_PG4_ROW10_DECTIVE_ITEM":
								faultDesc.Add(s4bFormControl.fieldValue);
								break;
							case "FIELD_PG4_ROW01_ACTION_TAKEN":
							case "FIELD_PG4_ROW02_ACTION_TAKEN":
							case "FIELD_PG4_ROW03_ACTION_TAKEN":
							case "FIELD_PG4_ROW04_ACTION_TAKEN":
							case "FIELD_PG4_ROW05_ACTION_TAKEN":
							case "FIELD_PG4_ROW06_ACTION_TAKEN":
							case "FIELD_PG4_ROW07_ACTION_TAKEN":
							case "FIELD_PG4_ROW08_ACTION_TAKEN":
							case "FIELD_PG4_ROW09_ACTION_TAKEN":
							case "FIELD_PG4_ROW10_ACTION_TAKEN":
								rowId = -1;
								int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out rowId);
								if (rowId > 0)
								{
									string desc = faultDesc[rowId - 1];
									faultDesc[rowId - 1] = desc + "-" + s4bFormControl.fieldValue;
								}
								break;
							case "FIELD_PG4_ROW01_ITEM_NO":
							case "FIELD_PG4_ROW02_ITEM_NO":
							case "FIELD_PG4_ROW03_ITEM_NO":
							case "FIELD_PG4_ROW04_ITEM_NO":
							case "FIELD_PG4_ROW05_ITEM_NO":
							case "FIELD_PG4_ROW06_ITEM_NO":
							case "FIELD_PG4_ROW07_ITEM_NO":
							case "FIELD_PG4_ROW08_ITEM_NO":
							case "FIELD_PG4_ROW09_ITEM_NO":
							case "FIELD_PG4_ROW10_ITEM_NO":
								imNumber.Add(s4bFormControl.fieldValue);
								break;
							case "FIELD_PG4_ROW01_TIME_START":
								break;
							case "FIELD_PG4_ROW01_TIME_FINISH":
								break;
						}
					}
				}
				if (userId <= 0)
				{
					userId = 1;
				}
				ProjectSettings settings = Configs.settings[request.Headers["ProjectId"]];
				//Get Diary Resource
				//DiaryApps diaryApp = null;
				//DiaryAppsRepository diaryAppsRepository = new DiaryAppsRepository(null);
				//long entityId = diaryAppsRepository.GetDiaryResourceSequenceByUserId(diaryResourceId, request, null);

				AssetTestHRepository assetTestHRepos = new AssetTestHRepository();
				//------- Get Asset Id
				Utilities.WriteLog("Asset Sequence = :" + assetSequence);
				long assetId = assetTestHRepos.GetAssetId(assetSequence, request);
				Utilities.WriteLog("Find Asset Id=" + assetId);
				//---Lock All previous vehicle test records
				VehicleTestHRepository vchTestHRepos = new VehicleTestHRepository();
				vchTestHRepos.UpdateLocked(assetId, request);
				//----Insert into vehicleTestH
				VehicleTestH vchTestH = new VehicleTestH();
				vchTestH.FlgDeleted = false;
				vchTestH.AssetSequence = assetId;
				vchTestH.TypeSequence = 1;
				vchTestH.DateTest = testDate;
				vchTestH.FlgLocked = false;
				vchTestH.FlgComplete = true;
				vchTestH.TestPassOrFail = 0;
				vchTestH.CreatedBy = userId;
				Utilities.WriteLog("Insert Vehicle Test H rows");
				ResponseModel retVchTestH = vchTestHRepos.Insert(vchTestH, request);
				if (retVchTestH.IsSucessfull == true)
				{
					vchTestH = (VehicleTestH)retVchTestH.TheObject;
					long vchTestHSequence = vchTestH.Sequence ?? 0;
					int count = 0;
					DatabaseInfo dbInfo = Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId);
					VehicleTestIDB vchTestIDB = new VehicleTestIDB(dbInfo);
					//----Insert into VehicleTestI
					Utilities.WriteLog("Insert Vehicle Test I rows:" + rowIds.Count);
					foreach (int id in rowIds)
					{
						VehicleTestI vchTestI = new VehicleTestI();
						vchTestI.JoinSequence = vchTestHSequence;
						vchTestI.AssetSequence = assetId;
						vchTestI.RowId = id;
						vchTestI.SectionId = sectionIds[count];
						vchTestI.InputData = inputData[count];
						vchTestI.CreatedBy = userId;
						vchTestI.DateCreated = DateTime.Now;
						//-----------------------
						long testIsequence = -1;
						vchTestIDB.insert(out testIsequence, vchTestI);
						count += 1;
					}
					//---Insert Missing Rows of Vehicle Test I
					Utilities.WriteLog("Insert Missing Vehicle Test I rows");
					vchTestIDB.insertMissingRows(vchTestHSequence, assetId, userId);

					//----Insert into VehicleTestI2
					Utilities.WriteLog("Insert Vehicle Test I_2 rows:" + imNumber.Count);
					for (int idx = 0; idx < imNumber.Count; idx++)
					{
						VehicleTestI2 vchTestI2 = new VehicleTestI2();
						vchTestI2.JoinSequence = vchTestHSequence;
						vchTestI2.AssetSequence = assetId;
						vchTestI2.RowIndex = idx;
						vchTestI2.IMNumber = imNumber[idx];
						if (faultDesc.Count > idx)
						{
							vchTestI2.FaultDesc = faultDesc[idx].ToString();
						}
						vchTestI2.DoneBy = userName;
						vchTestI2.CreatedBy = userId;
						vchTestI2.DateCreated = DateTime.Now;
						VehicleTestI2DB vchTestI2DB = new VehicleTestI2DB(dbInfo);
						long testI2sequence = -1;
						vchTestI2DB.insert(out testI2sequence, vchTestI2);
					}
					//----Insert into vehicleTestI_3
					Utilities.WriteLog("Insert Vehicle Test I3 row");
					VehicleTestI3 vchTestI3 = new VehicleTestI3();
					vchTestI3.JoinSequence = vchTestHSequence;
					vchTestI3.AssetSequence = assetId;
					vchTestI3.BrakeTest = brakeTest;
					vchTestI3.LadenType = ladenType;
					vchTestI3.RoadCoditionType = roadConditionType;
					vchTestI3.BreakTestSpeed = brakeTestSpeed;
					vchTestI3.BreakTestSpeedType = brakeTestSpeedType;
					vchTestI3.CreatedBy = userId;
					vchTestI3.DateCreated = DateTime.Now;
					VehicleTestI3DB vchTestI3DB = new VehicleTestI3DB(dbInfo);
					long testI3sequence = -1;
					vchTestI3DB.insert(out testI3sequence, vchTestI3);
				}
				else
				{
					Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "Unable to Insert Asset Test Header Row for S4BForm '" + s4bFormSubmission.RefNatForms.FormDesc + "' and Template id " + s4bFormSubmission.RefNatForms.FormId + " Reason:" + assetTestHRepos.Message, null);
				}
				returnValue = true;
			}
			catch (Exception ex)
			{
				Message = "Unable to Process Small Tools 13 Week Maintenance Check Sheet V01 for Capel Group - '" + s4bFormSubmission.RefNatForms.FormDesc + "'. Exception: " + ex.Message;
			}
			return returnValue;
		}
		//Capel Group - VehicleTestV02 (1623983715)
		private bool ProcessCapelKSPlantVehicleTestV02(HttpRequest request, S4BFormRequest s4bFormRequest, S4bFormSubmissions s4bFormSubmission,
													RootObject root, Dictionary<string, S4BFormsControl> s4bFormsControls)
		{
			const string METHOD_NAME = "S4BFormsRepository.ProcessCapelKSPlantVehicleTestV0()";
			bool returnValue = false;
			int userId = Utilities.GetUserIdFromRequest(request);
			int rowId = -1, sectionId = -1;
			long jobSequence = -1, jobAddressId = -1, jobClientId = -1, appSequence = -1, diaryResourceId = -1;
			string assetSequence = "";
			string userName = "", jobRef = "", jobClientRef = "", location = "", signature = "", submit = "", capelNo = "", vehicleReg = "";
			long mileage = 0;
			int ladenType = 0, roadConditionType = 0, brakeTestSpeedType = 0, brakeTest = 0;
			double brakeTestMain = 0, brakeTestSpeed = 0, brakeTestSecondary = 0, brakeTestParking = 0;
			bool flgTapleyTest = false, flgRollerBrakeTest = false;
			OrdersMin orderSecondary = s4bFormSubmission.OrderSecondary;
			string secondaryDatabaseId = Utilities.GetSecondaryIdByS4BFormId(s4bFormSubmission.RefNatForms.FormId);
			const int TOTAL_NO_ROWS_COL1 = 31;
			const int TOTAL_NO_ROWS_COL2 = 33;
			const int TOTAL_NO_ROWS_Page3 = 18;
			string[] checksCol1 = new string[TOTAL_NO_ROWS_COL1];
			string[] checksCol2 = new string[TOTAL_NO_ROWS_COL2];
			string[] checksCol3 = new string[TOTAL_NO_ROWS_Page3];
			List<int> sectionIds = new List<int>();
			List<int> rowIds = new List<int>();
			List<int> inputData = new List<int>();
			List<string> imNumber = new List<string>();
			List<string> faultDesc = new List<string>();

			DateTime diaryDate = DateTime.Now;
			DateTime testDate = DateTime.MinValue;
			DateTime row01Date = DateTime.Now;
			DateTime? timeStamp = s4bFormSubmission.DateCreated;
			Dictionary<string, string> imageValues = new Dictionary<string, string>();
			try
			{
				foreach (KeyValuePair<string, S4BFormsControl> s4bFormControlEntry in s4bFormsControls)
				{
					S4BFormsControl s4bFormControl = s4bFormControlEntry.Value;
					if (s4bFormControl.fieldValue != null)
					{
						switch (s4bFormControlEntry.Key)
						{
							case "VARIABLE_PG1_JOB_SEQUENCE":
								if (long.TryParse(s4bFormControl.fieldValue, out jobSequence))
								{
								};
								break;
							case "VARIABLE_PG1_JOB_CLIENT_ID":
								if (long.TryParse(s4bFormControl.fieldValue, out jobClientId))
								{
								};
								break;
							case "VARIABLE_PG1_JOB_ADDRESS_ID":
								if (long.TryParse(s4bFormControl.fieldValue, out jobAddressId))
								{
								};
								break;
							case "VARIABLE_PG1_USER_ID":
								if (int.TryParse(s4bFormControl.fieldValue, out userId))
								{
								};
								break;
							case "VAR_PG1_DIARY_RESOURCE":
								userName = s4bFormControl.fieldValue;
								break;
							case "FIELD_PG1_DATE":
								try
								{
									testDate = DateTime.ParseExact(s4bFormControl.fieldValue.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
								}
								catch (Exception ex)
								{ }
								break;
							case "VARIABLE_PG1_DIARY_ENTRY_ID":
								long.TryParse(s4bFormControl.fieldValue, out appSequence);
								break;
							case "VAR_PG1_JOB_REF":
								jobRef = s4bFormControl.fieldValue;
								break;
							case "VAR_PG1_CAPEL_NO":
								capelNo = s4bFormControl.fieldValue;
								break;
							case "FIELD_PG1_VEHICLE_REG":
								vehicleReg = s4bFormControl.fieldValue;
								assetSequence = s4bFormControl.fieldValue;
								break;
							case "FIELD_PG1_LOCATION":
								location = s4bFormControl.fieldValue;
								break;
							case "FIELD_PG1_MILEAGE":
								mileage = long.Parse(s4bFormControl.fieldValue);
								break;
							case "FIELD_PG1_T_01_ID_123":
							case "FIELD_PG1_T_01_ID_001":
							case "FIELD_PG1_T_01_ID_002":
							case "FIELD_PG1_T_01_ID_003":
							case "FIELD_PG1_T_01_ID_004":
							case "FIELD_PG1_T_01_ID_005":
							case "FIELD_PG1_T_01_ID_006":
							case "FIELD_PG1_T_01_ID_007":
							case "FIELD_PG1_T_01_ID_008":
							case "FIELD_PG1_T_01_ID_009":
							case "FIELD_PG1_T_01_ID_010":
							case "FIELD_PG1_T_01_ID_011":
							case "FIELD_PG1_T_01_ID_012":
							case "FIELD_PG1_T_01_ID_013":
							case "FIELD_PG1_T_01_ID_014":
							case "FIELD_PG1_T_01_ID_015":
							case "FIELD_PG1_T_01_ID_016":
							case "FIELD_PG1_T_01_ID_017":
							case "FIELD_PG1_T_01_ID_018":
							case "FIELD_PG1_T_01_ID_019":
							case "FIELD_PG1_T_01_ID_020":
							case "FIELD_PG1_T_01_ID_021":
							case "FIELD_PG1_T_01_ID_022":
							case "FIELD_PG1_T_01_ID_023":
							case "FIELD_PG1_T_01_ID_024":
							case "FIELD_PG1_T_01_ID_025":
							case "FIELD_PG1_T_01_ID_026":
							case "FIELD_PG1_T_01_ID_027":
							case "FIELD_PG1_T_01_ID_028":
							case "FIELD_PG1_T_01_ID_029":
							case "FIELD_PG1_T_01_ID_105":
							case "FIELD_PG1_T_01_ID_106":
							case "FIELD_PG2_T_01_ID_110":
							case "FIELD_PG2_T_01_ID_111":
							case "FIELD_PG2_T_01_ID_112":
							case "FIELD_PG2_T_01_ID_030":
							case "FIELD_PG2_T_01_ID_031":
							case "FIELD_PG2_T_01_ID_032":
							case "FIELD_PG2_T_01_ID_033":
							case "FIELD_PG2_T_01_ID_034":
							case "FIELD_PG2_T_01_ID_035":
							case "FIELD_PG2_T_01_ID_036":
							case "FIELD_PG2_T_01_ID_037":
							case "FIELD_PG2_T_01_ID_038":
							case "FIELD_PG2_T_01_ID_039":
							case "FIELD_PG2_T_01_ID_040":
							case "FIELD_PG2_T_01_ID_041":
							case "FIELD_PG2_T_01_ID_042":
							case "FIELD_PG2_T_01_ID_043":
							case "FIELD_PG2_T_01_ID_045":
							case "FIELD_PG2_T_01_ID_046":
							case "FIELD_PG2_T_01_ID_047":
							case "FIELD_PG2_T_01_ID_051":
							case "FIELD_PG2_T_01_ID_052":
							case "FIELD_PG2_T_01_ID_053":
							case "FIELD_PG2_T_01_ID_055":
							case "FIELD_PG2_T_01_ID_056":
							case "FIELD_PG2_T_01_ID_057":
							case "FIELD_PG2_T_01_ID_058":
							case "FIELD_PG2_T_01_ID_059":
							case "FIELD_PG2_T_01_ID_060":
							case "FIELD_PG2_T_01_ID_061":
							case "FIELD_PG2_T_01_ID_062":
							case "FIELD_PG2_T_01_ID_063":
							case "FIELD_PG2_T_01_ID_064":
							case "FIELD_PG2_T_01_ID_065":
							case "FIELD_PG2_T_01_ID_066":
							case "FIELD_PG2_T_01_ID_067":
							case "FIELD_PG3_T_01_ID_068":
							case "FIELD_PG3_T_01_ID_069":
							case "FIELD_PG3_T_01_ID_070":
							case "FIELD_PG3_T_01_ID_071":
							case "FIELD_PG3_T_01_ID_072":
							case "FIELD_PG3_T_01_ID_126":
							case "FIELD_PG3_T_01_ID_076":
							case "FIELD_PG3_T_01_ID_077":
							case "FIELD_PG3_T_01_ID_078":
							case "FIELD_PG3_T_01_ID_080":
							case "FIELD_PG3_T_01_ID_081":
							case "FIELD_PG3_T_01_ID_083":
							case "FIELD_PG3_T_01_ID_087":
							case "FIELD_PG3_T_01_ID_107":
							case "FIELD_PG3_T_01_ID_108":
							case "FIELD_PG3_T_01_ID_109":
								//if (checksCol1index >= 0)
								//{
								//    checksCol1[checksCol1index] = s4bFormControl.fieldValue;
								//    checksCol1index += 1;
								//}
								rowId = -1;
								int.TryParse(s4bFormControlEntry.Key.Substring(18, 3), out rowId);
								if (rowId > 0)
								{
									rowIds.Add(rowId);
									if (rowId >= 1 && rowId <= 21) // between row 21 to 22
										sectionIds.Add(1);
									else if (rowId >= 22 && rowId <= 29 || rowId == 123 || rowId == 124 || rowId == 105 || rowId == 106)
										sectionIds.Add(2);
									else if (rowId >= 30 && rowId <= 36) // between row 30 to 36
										sectionIds.Add(3);
									else if (rowId >= 37 && rowId <= 72 || (rowId >= 129 && rowId <= 131) || (rowId >= 110 && rowId <= 112))
										sectionIds.Add(4);
									else if (rowId >= 76 && rowId <= 78 || (rowId >= 125 && rowId <= 128) || (rowId >= 107 && rowId <= 109) || rowId == 80 || rowId == 81 || rowId == 83 || rowId == 87)
										sectionIds.Add(6);
									else if (rowId >= 89 && rowId <= 91)
										sectionIds.Add(7);
									else if (rowId >= 92 && rowId <= 99 || (rowId >= 118 && rowId <= 122))
										sectionIds.Add(8);
									else
										sectionIds.Add(0);
									if (s4bFormControl.fieldValue.ToUpper() == "OK")
										inputData.Add(1);
									else if (s4bFormControl.fieldValue.ToUpper() == "X")
										inputData.Add(2);
									else if (s4bFormControl.fieldValue.ToUpper() == "NC")
										inputData.Add(3);
									else if (s4bFormControl.fieldValue.ToUpper() == "N/A")
										inputData.Add(4);
									else if (s4bFormControl.fieldValue.ToUpper() == "M")
										inputData.Add(5);
									else
										inputData.Add(0);
								}
								//string pageNo = s4bFormControlEntry.Key.Substring(6, 3);
								//if (pageNo == "PG2")
								//{
								//    imNumber.Add(sectionId.ToString());
								//    faultDesc.Add(s4bFormControl.fieldValue);
								//}
								break;
							case "FIELD_PG3_ROAD_CONDITIONS":
								if (s4bFormControl.fieldValue.ToUpper() == "NOT SET")
								{
									roadConditionType = 0;
								}
								else if (s4bFormControl.fieldValue.ToUpper() == "WET")
								{
									roadConditionType = 1;
								}
								else if (s4bFormControl.fieldValue.ToUpper() == "DRY")
								{
									roadConditionType = 2;
								}
								break;
							case "FIELD_PG3_BRAKE_TEST_TYPE":
								if (s4bFormControl.fieldValue.ToUpper() == "DECELEROMETER")
								{
									flgTapleyTest = true;
								}
								if (s4bFormControl.fieldValue.ToUpper() == "ROLLER")
								{
									flgRollerBrakeTest = true;
								}

								break;
							case "FIELD_PG3_BRAKE_TEST_SPEED_TYPE":

								if (s4bFormControl.fieldValue.ToUpper() == "NOT SET" || s4bFormControl.fieldValue.ToUpper() == "")
								{
									brakeTestSpeedType = 0;
								}
								else if (s4bFormControl.fieldValue.ToUpper() == "KPH")
								{
									brakeTestSpeedType = 1;
								}
								else if (s4bFormControl.fieldValue.ToUpper() == "MPH")
								{
									brakeTestSpeedType = 2;
								}
								break;

							case "FIELD_PG3_BRAKE_TEST_SPEED":
								brakeTestSpeed = double.Parse(s4bFormControl.fieldValue);
								break;
							case "FIELD_PG3_T_03_BREAK_TEST_MAIN":
								brakeTestMain = double.Parse(s4bFormControl.fieldValue);
								break;
							case "FIELD_PG3_T_03_BREAK_TEST_SECONDARY":
								brakeTestSecondary = double.Parse(s4bFormControl.fieldValue);
								break;
							case "FIELD_PG3_T_03_BREAK_TEST_PARKING":
								brakeTestParking = double.Parse(s4bFormControl.fieldValue);
								break;
							case "FIELD_PG3_SIGNATURE":
								signature = s4bFormControl.fieldValue;
								break;
							case "FIELD_PG3_SUBMIT":
								submit = s4bFormControl.fieldValue;
								break;

							case "FIELD_PG4_ROW01_DATE":
							case "FIELD_PG4_ROW02_DATE":
								try
								{
									row01Date = DateTime.ParseExact(s4bFormControl.fieldValue.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
								}
								catch (Exception ex)
								{ }
								break;
							case "FIELD_PG3_VEHICLE_LADEN":
								if (s4bFormControl.fieldValue.ToUpper() == "NOT SET")
								{
									ladenType = 0;
								}
								else if (s4bFormControl.fieldValue.ToUpper() == "LADEN")
								{
									ladenType = 1;
								}
								else if (s4bFormControl.fieldValue.ToUpper() == "UNLADEN")
								{
									ladenType = 2;
								}
								break;
							case "FIELD_PG4_MAKE":
								break;
							case "FIELD_PG4_MODEL":
								break;
							case "FIELD_PG4_ROW01_DECTIVE_ITEM":
							case "FIELD_PG4_ROW02_DECTIVE_ITEM":
							case "FIELD_PG4_ROW03_DECTIVE_ITEM":
							case "FIELD_PG4_ROW04_DECTIVE_ITEM":
							case "FIELD_PG4_ROW05_DECTIVE_ITEM":
							case "FIELD_PG4_ROW06_DECTIVE_ITEM":
							case "FIELD_PG4_ROW07_DECTIVE_ITEM":
							case "FIELD_PG4_ROW08_DECTIVE_ITEM":
							case "FIELD_PG4_ROW09_DECTIVE_ITEM":
							case "FIELD_PG4_ROW10_DECTIVE_ITEM":
								faultDesc.Add(s4bFormControl.fieldValue);
								break;
							case "FIELD_PG4_ROW01_ACTION_TAKEN":
							case "FIELD_PG4_ROW02_ACTION_TAKEN":
							case "FIELD_PG4_ROW03_ACTION_TAKEN":
							case "FIELD_PG4_ROW04_ACTION_TAKEN":
							case "FIELD_PG4_ROW05_ACTION_TAKEN":
							case "FIELD_PG4_ROW06_ACTION_TAKEN":
							case "FIELD_PG4_ROW07_ACTION_TAKEN":
							case "FIELD_PG4_ROW08_ACTION_TAKEN":
							case "FIELD_PG4_ROW09_ACTION_TAKEN":
							case "FIELD_PG4_ROW10_ACTION_TAKEN":
								rowId = -1;
								int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out rowId);
								if (rowId > 0)
								{
									string desc = faultDesc[rowId - 1];
									faultDesc[rowId - 1] = desc + "-" + s4bFormControl.fieldValue;
								}
								break;
							case "FIELD_PG4_ROW01_ITEM_NO":
							case "FIELD_PG4_ROW02_ITEM_NO":
							case "FIELD_PG4_ROW03_ITEM_NO":
							case "FIELD_PG4_ROW04_ITEM_NO":
							case "FIELD_PG4_ROW05_ITEM_NO":
							case "FIELD_PG4_ROW06_ITEM_NO":
							case "FIELD_PG4_ROW07_ITEM_NO":
							case "FIELD_PG4_ROW08_ITEM_NO":
							case "FIELD_PG4_ROW09_ITEM_NO":
							case "FIELD_PG4_ROW10_ITEM_NO":
								imNumber.Add(s4bFormControl.fieldValue);
								break;
							case "FIELD_PG4_ROW01_TIME_START":
								break;
							case "FIELD_PG4_ROW01_TIME_FINISH":
								break;
						}
					}
				}
				if (userId <= 0)
				{
					userId = 1;
				}
				ProjectSettings settings = Configs.settings[request.Headers["ProjectId"]];
				//Get Diary Resource
				//DiaryApps diaryApp = null;
				//DiaryAppsRepository diaryAppsRepository = new DiaryAppsRepository(null);
				//long entityId = diaryAppsRepository.GetDiaryResourceSequenceByUserId(diaryResourceId, request, null);

				AssetTestHRepository assetTestHRepos = new AssetTestHRepository();
				//------- Get Asset Id
				Utilities.WriteLog("Asset Sequence = :" + assetSequence);
				long assetId = assetTestHRepos.GetAssetId(assetSequence, request);
				Utilities.WriteLog("Find Asset Id=" + assetId);
				//---Lock All previous vehicle test records
				VehicleTestHRepository vchTestHRepos = new VehicleTestHRepository();
				vchTestHRepos.UpdateLocked(assetId, request);
				//----Insert into vehicleTestH
				VehicleTestH vchTestH = new VehicleTestH();
				vchTestH.FlgDeleted = false;
				vchTestH.AssetSequence = assetId;
				vchTestH.TypeSequence = 1;
				vchTestH.DateTest = testDate;
				vchTestH.FlgLocked = false;
				vchTestH.FlgComplete = true;
				vchTestH.TestPassOrFail = 0;
				vchTestH.CreatedBy = userId;
				Utilities.WriteLog("Insert Vehicle Test H rows");
				ResponseModel retVchTestH = vchTestHRepos.Insert(vchTestH, request);
				if (retVchTestH.IsSucessfull == true)
				{
					vchTestH = (VehicleTestH)retVchTestH.TheObject;
					long vchTestHSequence = vchTestH.Sequence ?? 0;
					int count = 0;
					DatabaseInfo dbInfo = Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId);
					VehicleTestIDB vchTestIDB = new VehicleTestIDB(dbInfo);
					//----Insert into VehicleTestI
					Utilities.WriteLog("Insert Vehicle Test I rows:" + rowIds.Count);
					foreach (int id in rowIds)
					{
						VehicleTestI vchTestI = new VehicleTestI();
						vchTestI.JoinSequence = vchTestHSequence;
						vchTestI.AssetSequence = assetId;
						vchTestI.RowId = id;
						vchTestI.SectionId = sectionIds[count];
						vchTestI.InputData = inputData[count];
						vchTestI.CreatedBy = userId;
						vchTestI.DateCreated = DateTime.Now;
						//-----------------------
						long testIsequence = -1;
						vchTestIDB.insert(out testIsequence, vchTestI);
						count += 1;
					}
					//---Insert Missing Rows of Vehicle Test I
					Utilities.WriteLog("Insert Missing Vehicle Test I rows");
					vchTestIDB.insertMissingRows(vchTestHSequence, assetId, userId);

					//----Insert into VehicleTestI2
					Utilities.WriteLog("Insert Vehicle Test I_2 rows:" + imNumber.Count);
					for (int idx = 0; idx < imNumber.Count; idx++)
					{
						VehicleTestI2 vchTestI2 = new VehicleTestI2();
						vchTestI2.JoinSequence = vchTestHSequence;
						vchTestI2.AssetSequence = assetId;
						vchTestI2.RowIndex = idx;
						vchTestI2.IMNumber = imNumber[idx];
						if (faultDesc.Count > idx)
						{
							vchTestI2.FaultDesc = faultDesc[idx].ToString();
						}
						vchTestI2.DoneBy = userName;
						vchTestI2.CreatedBy = userId;
						vchTestI2.DateCreated = DateTime.Now;
						VehicleTestI2DB vchTestI2DB = new VehicleTestI2DB(dbInfo);
						long testI2sequence = -1;
						vchTestI2DB.insert(out testI2sequence, vchTestI2);
					}
					//----Insert into vehicleTestI_3
					Utilities.WriteLog("Insert Vehicle Test I3 row");
					VehicleTestI3 vchTestI3 = new VehicleTestI3();
					vchTestI3.JoinSequence = vchTestHSequence;
					vchTestI3.AssetSequence = assetId;
					vchTestI3.BrakeTest = 1; //brakeTest;
					vchTestI3.LadenType = ladenType;
					vchTestI3.RoadCoditionType = roadConditionType;
					vchTestI3.BreakTestSpeed = brakeTestSpeed;
					vchTestI3.BreakTestSpeedType = brakeTestSpeedType;
					vchTestI3.FlgTapleyTest = flgTapleyTest;
					vchTestI3.FlgRollerBbrakeTest = flgRollerBrakeTest;
					vchTestI3.BreakTestMain = brakeTestMain;
					vchTestI3.BreakTestSecondary = brakeTestSecondary;
					vchTestI3.BreakTestParking = brakeTestParking;
					vchTestI3.TestMileage = mileage;
					vchTestI3.CreatedBy = userId;
					vchTestI3.DateCreated = DateTime.Now;
					VehicleTestI3DB vchTestI3DB = new VehicleTestI3DB(dbInfo);
					long testI3sequence = -1;
					vchTestI3DB.insert(out testI3sequence, vchTestI3);
				}
				else
				{
					Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "Unable to Insert Asset Test Header Row for S4BForm '" + s4bFormSubmission.RefNatForms.FormDesc + "' and Template id " + s4bFormSubmission.RefNatForms.FormId + " Reason:" + assetTestHRepos.Message, null);
				}
				returnValue = true;
			}
			catch (Exception ex)
			{
				Message = "Unable to Process Small Tools 13 Week Maintenance Check Sheet V01 for Capel Group - '" + s4bFormSubmission.RefNatForms.FormDesc + "'. Exception: " + ex.Message;
			}
			return returnValue;
		}

		public ResponseModel GetAppointmentNotesSetting(HttpRequest request)
      {
         ResponseModel response = new ResponseModel();
         CldSettingsRepository cldSettingsRepository = new CldSettingsRepository();
         response.TheObject = cldSettingsRepository.GetS4BFormShowDiaryJobNotesBoth(request);
         response.IsSucessfull = true;
         return response;
      }

      // CBS - Job Sheet and Risk Assessment V09 59626082
      // CBS - Job Sheet and Risk Assessment Blank V01 984204447
      private bool ProcessCBSJobSheetAndRiskAssessmentV009(HttpRequest request, S4BFormRequest s4bFormRequest, S4bFormSubmissions s4bFormSubmission,
                                                       RootObject root, Dictionary<string, S4BFormsControl> s4bFormsControls)
      {
         const string METHOD_NAME = "S4BFormsRepository.ProcessCBSJobSheetAndRiskAssessmentV009()";
         bool returnValue = false;
         int userId = Utilities.GetUserIdFromRequest(request);
         long appSequence = -1;
         string userName = "";
         DateTime datAttended = DateTime.Now;
         DateTime dateAssesment = DateTime.Now;
         string actionTaken = "", startTime = "", endTime = "";
         DateTime datEngineer = DateTime.Now;
         DateTime? timeStamp = s4bFormSubmission.DateCreated;
         Dictionary<string, string> imageValues = new Dictionary<string, string>();
         const int NO_ROWS_STOCK = 4;
         double[] stockQty = new double[NO_ROWS_STOCK];
         string[] stockDesc = new string[NO_ROWS_STOCK];
         string vehicleReg = "";
         int index = -1;
         try
         {
            foreach (KeyValuePair<string, S4BFormsControl> s4bFormControlEntry in s4bFormsControls)
            {
               S4BFormsControl s4bFormControl = s4bFormControlEntry.Value;
               if (s4bFormControl.fieldValue != null)
               {
                  switch (s4bFormControlEntry.Key)
                  {
                     case "VARIABLE_PG1_USER_ID":
                        if (int.TryParse(s4bFormControl.fieldValue, out userId))
                        {
                        };
                        break;

                     case "VAR_PG1_DIARY_RESOURCE":
                        userName = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG2_DATE_ATTENDED":
                        DateTime resultDate = DateTime.MinValue;
                        try
                        {
                           resultDate = DateTime.ParseExact(s4bFormControl.fieldValue.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                           datAttended = resultDate;
                        }
                        catch (Exception ex)
                        { }
                        break;

                     case "FIELD_PG2_ACTION_TAKEN":
                        actionTaken = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG1_DATE_ASSESMENT":
                        DateTime pag1Date = DateTime.MinValue;
                        try
                        {
                           pag1Date = DateTime.ParseExact(s4bFormControl.fieldValue.Trim(), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                           dateAssesment = pag1Date;
                           datAttended = pag1Date;     // This form has no FIELD_PG2_DATE_ATTENDED field Value will be taken from FIELD_PG1_DATE_ASSESMENT. 
                        }
                        catch (Exception ex)
                        { }
                        break;

                     case "VARIABLE_PG1_DIARY_ENTRY_ID":
                        long.TryParse(s4bFormControl.fieldValue, out appSequence);
                        break;

                     case "FIELD_PG2_TIME_START":
                        startTime = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG2_TIME_END":
                        endTime = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG2_VEHICLE_REGISTRATION":
                        vehicleReg = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG2_ROW01_DESCRIPTION":
                     case "FIELD_PG2_ROW02_DESCRIPTION":
                     case "FIELD_PG2_ROW03_DESCRIPTION":
                     case "FIELD_PG2_ROW04_DESCRIPTION":
                        index = -1;
                        int.TryParse(s4bFormControl.fieldName.Substring(13, 2), out index);
                        if (index > 0)
                        {
                           stockDesc[index - 1] = s4bFormControl.fieldValue;
                        }
                        break;

                     case "FIELD_PG2_ROW01_QTY":
                     case "FIELD_PG2_ROW02_QTY":
                     case "FIELD_PG2_ROW03_QTY":
                     case "FIELD_PG2_ROW04_QTY":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        if (index > 0)
                        {
                           double result = 0;
                           try
                           {
                              double.TryParse(s4bFormControl.fieldValue, out result);
                           }
                           catch (Exception) { }
                           stockQty[index - 1] = result;
                        }
                        break;
                  }
               }
            }
            if (userId <= 0)
            {
               userId = 1;
            }
            DiaryResources diaryResource = Utilities.GetDiaryResourceByUserId(request, userId);

            /// Action Taken - Code is comment out
            if (false)
            {
               string formattedActionTaken = "";
               if (formattedActionTaken == "")
               {
                  formattedActionTaken = actionTaken;
               }
               string header = "Action Taken / Required – " + dateAssesment.ToString("d/M/yyyy") + " – " + (diaryResource == null ? "" : diaryResource.ResourceName);
               OrderItemsRepository orderItemsRepository = new OrderItemsRepository();
               OrderItems orderItems = new OrderItems();
               orderItems.FlgRowIsText = true;
               orderItems.JobSequence = s4bFormSubmission.Orders.Sequence ?? 0;
               orderItems.RowIndex = 9999;
               orderItems.AssetSequence = -1;
               orderItems.ItemType = 0;
               orderItems.TransType = SimplicityConstants.ClientTransType;
               orderItems.ItemCode = "";
               orderItems.ItemDesc = header.Trim();
               orderItems.ItemUnits = "";
               orderItems.ItemQuantity = orderItems.AmountLabour = orderItems.AmountMaterials = orderItems.AmountPlant = orderItems.AmountValue = orderItems.AmountTotal = 0;
               orderItems.AssignedTo = diaryResource.JoinResource;
               orderItems.FlgCompleted = true;
               orderItems.FlgDocsRecd = true;
               orderItems.CreatedBy = orderItems.LastAmendedBy = userId;
               orderItems.DateCreated = orderItems.DateLastAmended = timeStamp;
               orderItemsRepository.CreateOrderItems(orderItems, request);
               orderItems = new OrderItems();
               orderItems.FlgRowIsText = true;
               orderItems.JobSequence = s4bFormSubmission.Orders.Sequence;
               orderItems.ItemType = 0;
               orderItems.ItemCode = "";
               orderItems.ItemDesc = formattedActionTaken;
               orderItems.ItemUnits = "";
               orderItems.ItemQuantity = orderItems.AmountLabour = orderItems.AmountMaterials = orderItems.AmountPlant = orderItems.AmountValue = orderItems.AmountTotal = 0;
               orderItems.AssignedTo = diaryResource.JoinResource;
               orderItems.FlgCompleted = true;
               orderItems.FlgDocsRecd = true;
               orderItems.CreatedBy = orderItems.LastAmendedBy = userId;
               orderItems.DateCreated = orderItems.DateLastAmended = timeStamp;
               orderItemsRepository.CreateOrderItems(orderItems, request);
            }
            /// New row for Engineer Visit Entry in Schedule
            string text = dateAssesment.ToString("d/M/yyyy") + " – " + (diaryResource == null ? "" : diaryResource.ResourceName);
            if (startTime != "")
            {
               text = text + " - Start Time(" + startTime + ")";
            }
            if (endTime != "")
            {
               text = text + " - Finish Time(" + endTime + ")";
            }
            double itemQuantity = 0;
            DateTime startAppTime = DateTime.MinValue;
            DateTime endAppTime = DateTime.MinValue;
            try
            {
               startAppTime = DateTime.ParseExact(startTime, "HH:mm", CultureInfo.InvariantCulture);
               endAppTime = DateTime.ParseExact(endTime, "HH:mm", CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            { }
            if (startAppTime != DateTime.MinValue && endAppTime != DateTime.MinValue)
            {
               TimeSpan span = endAppTime.Subtract(startAppTime);
               itemQuantity = Utilities.NearestOfQuarter(span.TotalHours);
               if (itemQuantity < 0)
               {
                  itemQuantity = itemQuantity + 24;
               }
               // Change request as per Tino's email dated 08/05/2017
               if (datAttended.DayOfWeek == DayOfWeek.Saturday)
               {
                  itemQuantity = itemQuantity * 1.5;
               }
               else if (datAttended.DayOfWeek == DayOfWeek.Sunday)
               {
                  itemQuantity = itemQuantity * 2;
               }
            }
            if (diaryResource.Sequence > 0)
            {
               DiaryResourcesRepository diaryResourcesRepository = new DiaryResourcesRepository();
               double vamRate = diaryResourcesRepository.GetResourceVAMCostRate(request, diaryResource.Sequence ?? 0);
               double total = itemQuantity * vamRate;
               OrderItems orderItems = new OrderItems();
               orderItems.FlgRowIsText = false;
               orderItems.JobSequence = s4bFormSubmission.Orders.Sequence;
               orderItems.RowIndex = 9999;
               orderItems.AssetSequence = -1;
               orderItems.TransType = SimplicityConstants.ClientTransType;
               orderItems.ItemType = 0;
               orderItems.ItemCode = "";
               orderItems.ItemDesc = text;
               orderItems.ItemUnits = "HR";
               orderItems.ItemQuantity = itemQuantity;
               orderItems.AmountLabour = vamRate;
               orderItems.AmountMaterials = orderItems.AmountPlant = 0;
               orderItems.AmountValue = vamRate;
               orderItems.AmountTotal = itemQuantity * vamRate;
               orderItems.AssignedTo = diaryResource.JoinResource;
               orderItems.FlgCompleted = true;
               // Need to put attended date on due date as per Tino's email dated 08/05/2017
               if (datAttended != null && datAttended != DateTime.MinValue)
               {
                  orderItems.ItemDueDate = datAttended;
               }
               orderItems.FlgDocsRecd = true;
               orderItems.CreatedBy = orderItems.LastAmendedBy = userId;
               orderItems.DateCreated = orderItems.DateLastAmended = timeStamp;
               OrderItemsRepository orderItemsRepository = new OrderItemsRepository();
               orderItemsRepository.CreateOrderItems(orderItems, request);

               // Changes related to Stock Requisition
               bool flgInsertStock = false;
               for (int rowIndex = 0; rowIndex < NO_ROWS_STOCK; rowIndex++)
               {
                  if (stockQty[rowIndex] > 0 && !string.IsNullOrWhiteSpace(stockDesc[rowIndex]))
                  {
                     flgInsertStock = true;
                  }
               }
               if (flgInsertStock)
               {
                  if (!string.IsNullOrWhiteSpace(vehicleReg))
                  {
                     StockRepository stockRepository = new StockRepository();
                     StockGroup stockGroup = stockRepository.GetStockGroupByStockCodeAndTreeViewLevel(request, vehicleReg, 1);
                     if (stockGroup != null)
                     {
                        StockJobReqHeader stockJobReqheader = new StockJobReqHeader();
                        stockJobReqheader.JobSequence = s4bFormSubmission.Orders.Sequence;
                        stockJobReqheader.FlgAuthorised = false;
                        stockJobReqheader.AuthorisedBy = -1;
                        stockJobReqheader.DateAuthorised = null;
                        stockJobReqheader.PoType = 0;
                        stockJobReqheader.FlgPoPlaced = false;
                        stockJobReqheader.PoSequence = -1;
                        stockJobReqheader.CreatedBy = userId;
                        stockJobReqheader.DateCreated = DateTime.Now;
                        stockJobReqheader.LastAmendedBy = userId;
                        stockJobReqheader.DateLastAmended = DateTime.Now;
                        stockJobReqheader = stockRepository.InsertStockJobReqHeader(request, stockJobReqheader);
                        if (stockJobReqheader != null)
                        {
                           for (int rowIndex = 0; rowIndex < NO_ROWS_STOCK; rowIndex++)
                           {
                              if (stockQty[rowIndex] > 0 && !string.IsNullOrWhiteSpace(stockDesc[rowIndex]))
                              {
                                 string[] splittedStockDesc = stockDesc[rowIndex].Split('~');
                                 StockList stockList = stockRepository.GetStockListByStockCodeAndEntityId(request, splittedStockDesc[0], stockGroup.EntityId ?? 0);
                                 if (stockList != null)
                                 {
                                    StockJobRequest stockJobRequest = new StockJobRequest();
                                    stockJobRequest.JobSequence = s4bFormSubmission.Orders.Sequence;
                                    stockJobRequest.JoinSequence = stockJobReqheader.Sequence;
                                    stockJobRequest.TransType = SimplicityConstants.StockTransType;
                                    stockJobRequest.EntityId = stockGroup.EntityId;
                                    stockJobRequest.StockCode = stockList.StockCode;
                                    stockJobRequest.StockUnit = "EA";
                                    stockJobRequest.StockDesc = string.IsNullOrWhiteSpace(splittedStockDesc[1]) ? "" : splittedStockDesc[1];
                                    stockJobRequest.StockQuantity = stockQty[rowIndex];
                                    stockJobRequest.StockAmountEst = stockQty[rowIndex] * stockList.StockCostPrice;
                                    stockJobRequest.StockRequestedDate = datAttended;
                                    stockJobRequest.DateStockRequired = datAttended;
                                    stockJobRequest.FlgStockOrdered = true;
                                    stockJobRequest.StockOrderedDate = datAttended;
                                    stockJobRequest.FlgStockReceived = true;
                                    stockJobRequest.StockReceivedDate = datAttended;
                                    stockJobRequest.FlgSorDrillDown = false;
                                    stockJobRequest.SorItemCode = "";
                                    stockJobRequest.ItemType = 0;
                                    stockJobRequest.ItemHours = 0;
                                    stockJobRequest.CreatedBy = userId;
                                    stockJobRequest.DateCreated = DateTime.Now;
                                    stockJobRequest.LastAmendedBy = userId;
                                    stockJobRequest.DateLastAmended = DateTime.Now;
                                    stockJobRequest = stockRepository.InsertStockJobRequest(request, stockJobRequest);
                                    if (stockJobRequest != null)
                                    {
                                       StockJobReceived stockJobReceived = new StockJobReceived();
                                       stockJobReceived.RequestSequence = stockJobRequest.Sequence;
                                       stockJobReceived.DeliveryRef = "EFORMS_" + s4bFormSubmission.S4bSubmitNo + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                                       stockJobReceived.TransType = SimplicityConstants.StockTransType;
                                       stockJobReceived.EntityId = stockGroup.EntityId;
                                       stockJobReceived.StockRecievedDate = datAttended;
                                       stockJobReceived.StockCode = stockList.StockCode;
                                       stockJobReceived.StockQuantity = stockQty[rowIndex];
                                       stockJobReceived.StockAmount = stockQty[rowIndex] * stockList.StockCostPrice;
                                       stockJobReceived.FlgFromStockroom = true;
                                       stockJobReceived.JobSequence = s4bFormSubmission.Orders.Sequence;
                                       stockJobReceived = stockRepository.InsertStockJobReceived(request, stockJobReceived);
                                       if (stockJobReceived != null)
                                       {
                                          StockDetails stockDetails = new StockDetails();
                                          stockDetails.StockCode = stockList.StockCode;
                                          stockDetails.EntityId = stockGroup.EntityId;
                                          stockDetails.StockQuantityAvail = stockQty[rowIndex];
                                          stockDetails.LastAmendedBy = userId;
                                          stockDetails.DateLastAmended = DateTime.Now;
                                          if (!stockRepository.UpdateStockDetailsIncrementQuantityAvail(request, stockDetails))
                                          {
                                             Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "Unable to Update Increment Stock Available Quantity for S4BForm '" + s4bFormSubmission.RefNatForms.FormDesc + "' and Template id " + s4bFormSubmission.RefNatForms.FormId + ". Reason: " + stockRepository.Message, null);
                                          }
                                       }
                                       else
                                       {
                                          Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "Unable to Insert Stock Job Received for S4BForm '" + s4bFormSubmission.RefNatForms.FormDesc + "' and Template id " + s4bFormSubmission.RefNatForms.FormId + ". Reason: " + stockRepository.Message, null);
                                       }
                                    }
                                    else
                                    {
                                       Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "Unable to Insert Stock Job Request for S4BForm '" + s4bFormSubmission.RefNatForms.FormDesc + "' and Template id " + s4bFormSubmission.RefNatForms.FormId + ". Reason: " + stockRepository.Message, null);
                                    }
                                 }
                              }
                           }
                        }
                        else
                        {
                           Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "Unable to Insert Stock Group Header Row for S4BForm '" + s4bFormSubmission.RefNatForms.FormDesc + "' and Template id " + s4bFormSubmission.RefNatForms.FormId + ". Reason: " + stockRepository.Message, null);
                        }

                     }
                     else
                     {
                        Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "Unable to get Stock Group Code for S4BForm '" + s4bFormSubmission.RefNatForms.FormDesc + "' and Template id " + s4bFormSubmission.RefNatForms.FormId + ". Reason: " + stockRepository.Message, null);
                     }
                  }
                  else
                  {
                     Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "Unable to get Group Code from Vehicle Reg for " + s4bFormSubmission.RefNatForms.FormDesc + " and Template id " + s4bFormSubmission.RefNatForms.FormId, null);
                  }
               }
            }
            returnValue = true;
         }
         catch (Exception ex)
         {
            Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "Unable to Process " + s4bFormSubmission.RefNatForms.FormDesc + " and Template id " + s4bFormSubmission.RefNatForms.FormId, ex);
         }
         return returnValue;
      }
      // CBS - Job Sheet and Risk Assessment V07 357000009
      // CBS - Job Sheet and Risk Assessment Blank V01 1587685603
      private bool ProcessCBSJobSheetAndRiskAssessment(HttpRequest request, S4BFormRequest s4bFormRequest, S4bFormSubmissions s4bFormSubmission,
                                                       RootObject root, Dictionary<string, S4BFormsControl> s4bFormsControls)
      {
         const string METHOD_NAME = "S4BFormsRepository.ProcessCBSJobSheetAndRiskAssessment()";
         bool returnValue = false;
         int userId = Utilities.GetUserIdFromRequest(request);
         long appSequence = -1;
         string userName = "";
         DateTime datAttended = DateTime.Now;
         string actionTaken = "", startTime = "", endTime = "";
         DateTime datEngineer = DateTime.Now;
         DateTime? timeStamp = s4bFormSubmission.DateCreated;
         Dictionary<string, string> imageValues = new Dictionary<string, string>();
         const int NO_ROWS_STOCK = 4;
         double[] stockQty = new double[NO_ROWS_STOCK];
         string[] stockDesc = new string[NO_ROWS_STOCK];
         string vehicleReg = "";
         int index = -1;
         try
         {
            foreach (KeyValuePair<string, S4BFormsControl> s4bFormControlEntry in s4bFormsControls)
            {
               S4BFormsControl s4bFormControl = s4bFormControlEntry.Value;
               if (s4bFormControl.fieldValue != null)
               {
                  switch (s4bFormControlEntry.Key)
                  {
                     case "VARIABLE_PG1_USER_ID":
                        if (int.TryParse(s4bFormControl.fieldValue, out userId))
                        {
                        };
                        break;

                     case "VARIABLE_PG1_DIARY_RESOURCE":
                        userName = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG2_DATE_ATTENDED":
                        DateTime resultDate = DateTime.MinValue;
                        try
                        {
                           resultDate = DateTime.ParseExact(s4bFormControl.fieldValue.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                           datAttended = resultDate;
                        }
                        catch (Exception ex)
                        { }
                        break;

                     case "FIELD_PG2_ACTION_TAKEN":
                        actionTaken = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG1_DATE":
                        DateTime pg1Date = DateTime.MinValue;
                        DateTime.TryParse(s4bFormControl.fieldValue, out pg1Date);
                        if (pg1Date != DateTime.MinValue)
                        {
                           timeStamp = pg1Date;
                        }
                        break;

                     case "VARIABLE_PG1_DIARY_ENTRY_ID":
                        long.TryParse(s4bFormControl.fieldValue, out appSequence);
                        break;

                     case "FIELD_PG2_START_TIME":
                        startTime = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG2_END_TIME":
                        endTime = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG2_VEHICLE_REGISTRATION":
                        vehicleReg = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG2_ROW01_DESCRIPTION":
                     case "FIELD_PG2_ROW02_DESCRIPTION":
                     case "FIELD_PG2_ROW03_DESCRIPTION":
                     case "FIELD_PG2_ROW04_DESCRIPTION":
                        index = -1;
                        int.TryParse(s4bFormControl.fieldName.Substring(13, 2), out index);
                        if (index > 0)
                        {
                           stockDesc[index - 1] = s4bFormControl.fieldValue;
                        }
                        break;

                     case "FIELD_PG2_ROW01_QTY":
                     case "FIELD_PG2_ROW02_QTY":
                     case "FIELD_PG2_ROW03_QTY":
                     case "FIELD_PG2_ROW04_QTY":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        if (index > 0)
                        {
                           double result = 0;
                           try
                           {
                              double.TryParse(s4bFormControl.fieldValue, out result);
                           }
                           catch (Exception) { }
                           stockQty[index - 1] = result;
                        }
                        break;
                  }
               }
            }
            if (userId <= 0)
            {
               userId = 1;
            }
            DiaryResources diaryResource = Utilities.GetDiaryResourceByUserId(request, userId);

            /// Action Taken - Code is comment out
            if (false)
            {
               string formattedActionTaken = "";
               if (formattedActionTaken == "")
               {
                  formattedActionTaken = actionTaken;
               }
               string header = "Action Taken / Required – " + datAttended.ToShortDateString() + " – " + (diaryResource == null ? "" : diaryResource.ResourceName);
               OrderItemsRepository orderItemsRepository = new OrderItemsRepository();
               OrderItems orderItems = new OrderItems();
               orderItems.FlgRowIsText = true;
               orderItems.JobSequence = s4bFormSubmission.Orders.Sequence;
               orderItems.RowIndex = 9999;
               orderItems.AssetSequence = -1;
               orderItems.ItemType = 0;
               orderItems.TransType = SimplicityConstants.ClientTransType;
               orderItems.ItemCode = "";
               orderItems.ItemDesc = header.Trim();
               orderItems.ItemUnits = "";
               orderItems.ItemQuantity = orderItems.AmountLabour = orderItems.AmountMaterials = orderItems.AmountPlant = orderItems.AmountValue = orderItems.AmountTotal = 0;
               orderItems.AssignedTo = diaryResource.JoinResource;
               orderItems.FlgCompleted = true;
               orderItems.FlgDocsRecd = true;
               orderItems.CreatedBy = orderItems.LastAmendedBy = userId;
               orderItems.DateCreated = orderItems.DateLastAmended = timeStamp;
               orderItemsRepository.CreateOrderItems(orderItems, request);
               orderItems = new OrderItems();
               orderItems.FlgRowIsText = true;
               orderItems.JobSequence = s4bFormSubmission.Orders.Sequence;
               orderItems.ItemType = 0;
               orderItems.ItemCode = "";
               orderItems.ItemDesc = formattedActionTaken;
               orderItems.ItemUnits = "";
               orderItems.ItemQuantity = orderItems.AmountLabour = orderItems.AmountMaterials = orderItems.AmountPlant = orderItems.AmountValue = orderItems.AmountTotal = 0;
               orderItems.AssignedTo = diaryResource.JoinResource;
               orderItems.FlgCompleted = true;
               orderItems.FlgDocsRecd = true;
               orderItems.CreatedBy = orderItems.LastAmendedBy = userId;
               orderItems.DateCreated = orderItems.DateLastAmended = timeStamp;
               orderItemsRepository.CreateOrderItems(orderItems, request);
            }
            /// New row for Engineer Visit Entry in Schedule
            string text = datAttended.ToString("dd/MM/yyyy") + " – " + (diaryResource == null ? "" : diaryResource.ResourceName);
            if (startTime != "")
            {
               text = text + " - Start Time(" + startTime + ")";
            }
            if (endTime != "")
            {
               text = text + " - Finish Time(" + endTime + ")";
            }
            double itemQuantity = 0;
            DateTime startAppTime = DateTime.MinValue;
            DateTime endAppTime = DateTime.MinValue;
            try
            {
               startAppTime = DateTime.ParseExact(startTime, "HH:mm", CultureInfo.InvariantCulture);
               endAppTime = DateTime.ParseExact(endTime, "HH:mm", CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            { }
            if (startAppTime != DateTime.MinValue && endAppTime != DateTime.MinValue)
            {
               TimeSpan span = endAppTime.Subtract(startAppTime);
               itemQuantity = Utilities.NearestOfQuarter(span.TotalHours);
               if (itemQuantity < 0)
               {
                  itemQuantity = itemQuantity + 24;
               }
               // Change request as per Tino's email dated 08/05/2017
               if (datAttended.DayOfWeek == DayOfWeek.Saturday)
               {
                  itemQuantity = itemQuantity * 1.5;
               }
               else if (datAttended.DayOfWeek == DayOfWeek.Sunday)
               {
                  itemQuantity = itemQuantity * 2;
               }
            }
            if (diaryResource.Sequence > 0)
            {
               DiaryResourcesRepository diaryResourcesRepository = new DiaryResourcesRepository();
               double vamRate = diaryResourcesRepository.GetResourceVAMCostRate(request, diaryResource.Sequence ?? 0);
               double total = itemQuantity * vamRate;
               OrderItems orderItems = new OrderItems();
               orderItems.FlgRowIsText = false;
               orderItems.JobSequence = s4bFormSubmission.Orders.Sequence;
               orderItems.RowIndex = 9999;
               orderItems.AssetSequence = -1;
               orderItems.TransType = SimplicityConstants.ClientTransType;
               orderItems.ItemType = 0;
               orderItems.ItemCode = "";
               orderItems.ItemDesc = text;
               orderItems.ItemUnits = "HR";
               orderItems.ItemQuantity = itemQuantity;
               orderItems.AmountLabour = vamRate;
               orderItems.AmountMaterials = orderItems.AmountPlant = 0;
               orderItems.AmountValue = vamRate;
               orderItems.AmountTotal = itemQuantity * vamRate;
               orderItems.AssignedTo = diaryResource.JoinResource;
               orderItems.FlgCompleted = true;
               // Need to put attended date on due date as per Tino's email dated 08/05/2017
               if (datAttended != null && datAttended != DateTime.MinValue)
               {
                  orderItems.ItemDueDate = datAttended;
               }
               orderItems.FlgDocsRecd = true;
               orderItems.CreatedBy = orderItems.LastAmendedBy = userId;
               orderItems.DateCreated = orderItems.DateLastAmended = timeStamp;
               OrderItemsRepository orderItemsRepository = new OrderItemsRepository();
               orderItemsRepository.CreateOrderItems(orderItems, request);

               // Changes related to Stock Requisition
               bool flgInsertStock = false;
               for (int rowIndex = 0; rowIndex < NO_ROWS_STOCK; rowIndex++)
               {
                  if (stockQty[rowIndex] > 0 && !string.IsNullOrWhiteSpace(stockDesc[rowIndex]))
                  {
                     flgInsertStock = true;
                  }
               }
               if (flgInsertStock)
               {
                  if (!string.IsNullOrWhiteSpace(vehicleReg))
                  {
                     StockRepository stockRepository = new StockRepository();
                     StockGroup stockGroup = stockRepository.GetStockGroupByStockCodeAndTreeViewLevel(request, vehicleReg, 1);
                     if (stockGroup != null)
                     {
                        StockJobReqHeader stockJobReqheader = new StockJobReqHeader();
                        stockJobReqheader.JobSequence = s4bFormSubmission.Orders.Sequence;
                        stockJobReqheader.FlgAuthorised = false;
                        stockJobReqheader.AuthorisedBy = -1;
                        stockJobReqheader.DateAuthorised = null;
                        stockJobReqheader.PoType = 0;
                        stockJobReqheader.FlgPoPlaced = false;
                        stockJobReqheader.PoSequence = -1;
                        stockJobReqheader.CreatedBy = userId;
                        stockJobReqheader.DateCreated = DateTime.Now;
                        stockJobReqheader.LastAmendedBy = userId;
                        stockJobReqheader.DateLastAmended = DateTime.Now;
                        stockJobReqheader = stockRepository.InsertStockJobReqHeader(request, stockJobReqheader);
                        if (stockJobReqheader != null)
                        {
                           for (int rowIndex = 0; rowIndex < NO_ROWS_STOCK; rowIndex++)
                           {
                              if (stockQty[rowIndex] > 0 && !string.IsNullOrWhiteSpace(stockDesc[rowIndex]))
                              {
                                 string[] splittedStockDesc = stockDesc[rowIndex].Split('~');
                                 StockList stockList = stockRepository.GetStockListByStockCodeAndEntityId(request, splittedStockDesc[0], stockGroup.EntityId ?? 0);
                                 if (stockList != null)
                                 {
                                    StockJobRequest stockJobRequest = new StockJobRequest();
                                    stockJobRequest.JobSequence = s4bFormSubmission.Orders.Sequence ?? 0;
                                    stockJobRequest.JoinSequence = stockJobReqheader.Sequence;
                                    stockJobRequest.TransType = SimplicityConstants.StockTransType;
                                    stockJobRequest.EntityId = stockGroup.EntityId;
                                    stockJobRequest.StockCode = stockList.StockCode;
                                    stockJobRequest.StockUnit = "EA";
                                    stockJobRequest.StockDesc = string.IsNullOrWhiteSpace(splittedStockDesc[1]) ? "" : splittedStockDesc[1];
                                    stockJobRequest.StockQuantity = stockQty[rowIndex];
                                    stockJobRequest.StockAmountEst = stockQty[rowIndex] * stockList.StockCostPrice;
                                    stockJobRequest.StockRequestedDate = datAttended;
                                    stockJobRequest.DateStockRequired = datAttended;
                                    stockJobRequest.FlgStockOrdered = true;
                                    stockJobRequest.StockOrderedDate = datAttended;
                                    stockJobRequest.FlgStockReceived = true;
                                    stockJobRequest.StockReceivedDate = datAttended;
                                    stockJobRequest.FlgSorDrillDown = false;
                                    stockJobRequest.SorItemCode = "";
                                    stockJobRequest.ItemType = 0;
                                    stockJobRequest.ItemHours = 0;
                                    stockJobRequest.CreatedBy = userId;
                                    stockJobRequest.DateCreated = DateTime.Now;
                                    stockJobRequest.LastAmendedBy = userId;
                                    stockJobRequest.DateLastAmended = DateTime.Now;
                                    stockJobRequest = stockRepository.InsertStockJobRequest(request, stockJobRequest);
                                    if (stockJobRequest != null)
                                    {
                                       StockJobReceived stockJobReceived = new StockJobReceived();
                                       stockJobReceived.RequestSequence = stockJobRequest.Sequence;
                                       stockJobReceived.DeliveryRef = "EFORMS_" + s4bFormSubmission.S4bSubmitNo + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                                       stockJobReceived.TransType = SimplicityConstants.StockTransType;
                                       stockJobReceived.EntityId = stockGroup.EntityId;
                                       stockJobReceived.StockRecievedDate = datAttended;
                                       stockJobReceived.StockCode = stockList.StockCode;
                                       stockJobReceived.StockQuantity = stockQty[rowIndex];
                                       stockJobReceived.StockAmount = stockQty[rowIndex] * stockList.StockCostPrice;
                                       stockJobReceived.FlgFromStockroom = true;
                                       stockJobReceived.JobSequence = s4bFormSubmission.Orders.Sequence;
                                       stockJobReceived = stockRepository.InsertStockJobReceived(request, stockJobReceived);
                                       if (stockJobReceived != null)
                                       {
                                          StockDetails stockDetails = new StockDetails();
                                          stockDetails.StockCode = stockList.StockCode;
                                          stockDetails.EntityId = stockGroup.EntityId;
                                          stockDetails.StockQuantityAvail = stockQty[rowIndex];
                                          stockDetails.LastAmendedBy = userId;
                                          stockDetails.DateLastAmended = DateTime.Now;
                                          if (!stockRepository.UpdateStockDetailsIncrementQuantityAvail(request, stockDetails))
                                          {
                                             Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "Unable to Update Increment Stock Available Quantity for S4BForm '" + s4bFormSubmission.RefNatForms.FormDesc + "' and Template id " + s4bFormSubmission.RefNatForms.FormId + ". Reason: " + stockRepository.Message, null);
                                          }
                                       }
                                       else
                                       {
                                          Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "Unable to Insert Stock Job Received for S4BForm '" + s4bFormSubmission.RefNatForms.FormDesc + "' and Template id " + s4bFormSubmission.RefNatForms.FormId + ". Reason: " + stockRepository.Message, null);
                                       }
                                    }
                                    else
                                    {
                                       Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "Unable to Insert Stock Job Request for S4BForm '" + s4bFormSubmission.RefNatForms.FormDesc + "' and Template id " + s4bFormSubmission.RefNatForms.FormId + ". Reason: " + stockRepository.Message, null);
                                    }
                                 }
                              }
                           }
                        }
                        else
                        {
                           Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "Unable to Insert Stock Group Header Row for S4BForm '" + s4bFormSubmission.RefNatForms.FormDesc + "' and Template id " + s4bFormSubmission.RefNatForms.FormId + ". Reason: " + stockRepository.Message, null);
                        }

                     }
                     else
                     {
                        Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "Unable to get Stock Group Code for S4BForm '" + s4bFormSubmission.RefNatForms.FormDesc + "' and Template id " + s4bFormSubmission.RefNatForms.FormId + ". Reason: " + stockRepository.Message, null);
                     }
                  }
                  else
                  {
                     Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "Unable to get Group Code from Vehicle Reg for " + s4bFormSubmission.RefNatForms.FormDesc + " and Template id " + s4bFormSubmission.RefNatForms.FormId, null);
                  }
               }
            }
            returnValue = true;
         }
         catch (Exception ex)
         {
            Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "Unable to Process " + s4bFormSubmission.RefNatForms.FormDesc + " and Template id " + s4bFormSubmission.RefNatForms.FormId, ex);
         }
         return returnValue;
      }

      // Lowry - Purchase Order Blank V02 2093570712
      private bool ProcessLowryPO(HttpRequest request, S4BFormRequest s4bFormRequest, S4bFormSubmissions s4bFormSubmission,
                                  RootObject root, Dictionary<string, S4BFormsControl> s4bFormsControls, int formType)
      {
         const string METHOD_NAME = "S4BFormsRepository.ProcessLowryPO()";
         bool returnValue = false;
         int userId = Utilities.GetUserIdFromRequest(request);
         long jobSequence = -1, jobAddressId = -1, jobClientId = -1;
         long appSequence = -1;
         DateTime datAttended = DateTime.Now;
         DateTime datEngineer = DateTime.Now;
         DateTime? timeStamp = s4bFormSubmission.DateCreated;
         string supplierName = "", requestedBy = "", orderedBy = "";
         const int TOTAL_NO_ROWS = 14;
         string jobRef = "", poNO = "", po1 = "", po2 = "", po3 = "", po4 = "", po5 = "", jobAddress = "", emailAddress = "";
         string attentionOf = "", poNotes = "", voRef = "", customerRef = "", supplierAddress = "", supplierTelephone = "";
         string addressInvoice = "", addressDelivery = "", additionalInfo = "eForms Import", diaryResourceName = "";
         int poVOTypeSequence = -1;
         int index = -1;
         bool flgDeliverToSite = false;
         string[] description = new string[TOTAL_NO_ROWS];
         string[] cOrd = new string[TOTAL_NO_ROWS];
         double[] qty = new double[TOTAL_NO_ROWS];
         double[] price = new double[TOTAL_NO_ROWS];
         string[] jobDesc = new string[TOTAL_NO_ROWS];
         string[] dn = new string[TOTAL_NO_ROWS];
         DateTime issueDate = new DateTime();
         DateTime dueDate = new DateTime();
         DateTime requiredByDate = new DateTime();
         Dictionary<string, string> imageValues = new Dictionary<string, string>();
         try
         {
            foreach (KeyValuePair<string, S4BFormsControl> s4bFormControlEntry in s4bFormsControls)
            {
               S4BFormsControl s4bFormControl = s4bFormControlEntry.Value;
               if (s4bFormControl.fieldValue != null)
               {
                  switch (s4bFormControlEntry.Key)
                  {
                     case "VARIABLE_PG1_JOB_SEQUENCE":
                        if (long.TryParse(s4bFormControl.fieldValue, out jobSequence))
                        {
                        };
                        break;
                     case "VARIABLE_PG1_JOB_CLIENT_ID":
                        if (long.TryParse(s4bFormControl.fieldValue, out jobClientId))
                        {
                        };
                        break;
                     case "VARIABLE_PG1_JOB_ADDRESS_ID":
                        if (long.TryParse(s4bFormControl.fieldValue, out jobAddressId))
                        {
                        };
                        break;
                     case "VARIABLE_PG1_USER_ID":
                        if (int.TryParse(s4bFormControl.fieldValue, out userId))
                        {
                        };
                        break;

                     case "VARIABLE_PG1_DIARY_ENTRY_ID":
                        long.TryParse(s4bFormControl.fieldValue, out appSequence);
                        break;

                     case "VAR_PG1_PO_NO":
                        poNO = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG1_SUPPLER":
                        supplierName = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG1_EMAIL_ADDRESS":
                        emailAddress = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG1_JOB_ADDRESS":
                        jobAddress = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG1_JOB_REF":
                        jobRef = s4bFormControl.fieldValue;
                        break;

                     case "VARIABLE_PG1_DIARY_RESOURCE":
                        diaryResourceName = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG1_PO_NO1":
                        po1 = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG1_PO_NO2":
                        po2 = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG1_PO_NO3":
                        po3 = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG1_PO_NO4":
                        po4 = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG1_PO_NO5":
                        po5 = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG1_REQUESTED_BY":
                        requestedBy = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG1_ORDERED_BY":
                        orderedBy = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG1_ISSUE_DATE":
                        try
                        {
                           issueDate = DateTime.ParseExact(s4bFormControl.fieldValue.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        }
                        catch (Exception ex)
                        { }
                        break;

                     case "FIELD_PG1_DUE_DATE":
                        try
                        {
                           dueDate = DateTime.ParseExact(s4bFormControl.fieldValue.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        }
                        catch (Exception ex)
                        { }
                        break;

                     case "FIELD_PG1_ROW01_QTY":
                     case "FIELD_PG1_ROW02_QTY":
                     case "FIELD_PG1_ROW03_QTY":
                     case "FIELD_PG1_ROW04_QTY":
                     case "FIELD_PG1_ROW05_QTY":
                     case "FIELD_PG1_ROW06_QTY":
                     case "FIELD_PG1_ROW07_QTY":
                     case "FIELD_PG2_ROW08_QTY":
                     case "FIELD_PG2_ROW09_QTY":
                     case "FIELD_PG2_ROW10_QTY":
                     case "FIELD_PG2_ROW11_QTY":
                     case "FIELD_PG2_ROW12_QTY":
                     case "FIELD_PG2_ROW13_QTY":
                     case "FIELD_PG2_ROW14_QTY":
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        if (index > 0)
                        {
                           double result = 0;
                           try
                           {
                              double.TryParse(s4bFormControl.fieldValue, out result);
                           }
                           catch (Exception) { }
                           qty[index - 1] = result;
                        }
                        break;

                     case "FIELD_PG1_ROW01_DESCRIPTION":
                     case "FIELD_PG1_ROW02_DESCRIPTION":
                     case "FIELD_PG1_ROW03_DESCRIPTION":
                     case "FIELD_PG1_ROW04_DESCRIPTION":
                     case "FIELD_PG1_ROW05_DESCRIPTION":
                     case "FIELD_PG1_ROW06_DESCRIPTION":
                     case "FIELD_PG1_ROW07_DESCRIPTION":
                     case "FIELD_PG1_ROW08_DESCRIPTION":
                     case "FIELD_PG2_ROW09_DESCRIPTION":
                     case "FIELD_PG2_ROW10_DESCRIPTION":
                     case "FIELD_PG2_ROW11_DESCRIPTION":
                     case "FIELD_PG2_ROW12_DESCRIPTION":
                     case "FIELD_PG2_ROW13_DESCRIPTION":
                     case "FIELD_PG2_ROW14_DESCRIPTION":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        if (index > 0)
                        {
                           description[index - 1] = s4bFormControl.fieldValue;
                        }
                        break;

                     case "FIELD_PG1_ROW01_PRICE":
                     case "FIELD_PG1_ROW02_PRICE":
                     case "FIELD_PG1_ROW03_PRICE":
                     case "FIELD_PG1_ROW04_PRICE":
                     case "FIELD_PG1_ROW05_PRICE":
                     case "FIELD_PG1_ROW06_PRICE":
                     case "FIELD_PG1_ROW07_PRICE":
                     case "FIELD_PG2_ROW08_PRICE":
                     case "FIELD_PG2_ROW09_PRICE":
                     case "FIELD_PG2_ROW10_PRICE":
                     case "FIELD_PG2_ROW11_PRICE":
                     case "FIELD_PG2_ROW12_PRICE":
                     case "FIELD_PG2_ROW13_PRICE":
                     case "FIELD_PG2_ROW14_PRICE":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        if (index > 0)
                        {
                           double result = 0;
                           try
                           {
                              double.TryParse(s4bFormControl.fieldValue, out result);
                           }
                           catch (Exception) { }
                           price[index - 1] = result;
                        }
                        break;

                     case "FIELD_PG1_ROW01_DN":
                     case "FIELD_PG1_ROW02_DN":
                     case "FIELD_PG1_ROW03_DN":
                     case "FIELD_PG1_ROW04_DN":
                     case "FIELD_PG1_ROW05_DN":
                     case "FIELD_PG1_ROW06_DN":
                     case "FIELD_PG1_ROW07_DN":
                     case "FIELD_PG1_ROW08_DN":
                     case "FIELD_PG2_ROW09_DN":
                     case "FIELD_PG2_ROW10_DN":
                     case "FIELD_PG2_ROW11_DN":
                     case "FIELD_PG2_ROW12_DN":
                     case "FIELD_PG2_ROW13_DN":
                     case "FIELD_PG2_ROW14_DN":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        if (index > 0)
                        {
                           dn[index - 1] = s4bFormControl.fieldValue;
                        }
                        break;

                     case "FIELD_PG1_ROW01_C_OR_D":
                     case "FIELD_PG1_ROW02_C_OR_D":
                     case "FIELD_PG1_ROW03_C_OR_D":
                     case "FIELD_PG1_ROW04_C_OR_D":
                     case "FIELD_PG1_ROW05_C_OR_D":
                     case "FIELD_PG1_ROW06_C_OR_D":
                     case "FIELD_PG1_ROW07_C_OR_D":
                     case "FIELD_PG1_ROW08_C_OR_D":
                     case "FIELD_PG2_ROW09_C_OR_D":
                     case "FIELD_PG2_ROW10_C_OR_D":
                     case "FIELD_PG2_ROW11_C_OR_D":
                     case "FIELD_PG2_ROW12_C_OR_D":
                     case "FIELD_PG2_ROW13_C_OR_D":
                     case "FIELD_PG2_ROW14_C_OR_D":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        if (index > 0)
                        {
                           cOrd[index - 1] = s4bFormControl.fieldValue;
                        }
                        break;
                  }
               }
            }
            if (userId <= 0)
            {
               userId = 1;
            }
            DiaryResources diaryResource = Utilities.GetDiaryResourceByUserId(request, userId);
            bool flgValidJobRef = false;
            Orders orderDetails = null;
            OrdersRepository ordersRepository = new OrdersRepository( null);
            if (jobSequence > 0)
            {
               orderDetails = ordersRepository.GetOrderDetailsBySequence(jobSequence, request);
               if (orderDetails != null)
               {
                  jobSequence = orderDetails.Sequence ?? 0;
                  jobAddress = orderDetails.JobAddress;
                  jobRef = orderDetails.JobRef;
                  flgValidJobRef = true;
               }
            }
            else
            {
               if (jobRef != null && jobRef != "")
               {
                  if (Information.IsNumeric(jobRef) && jobRef.Length < 8)
                  {
                     jobRef = jobRef.ToString().PadLeft(8, '0');
                  }
                  orderDetails = ordersRepository.GetOrderByJobRef(jobRef, request);
                  if (orderDetails != null)
                  {
                     jobSequence = orderDetails.Sequence ?? 0;
                     jobAddress = orderDetails.JobAddress;
                     jobRef = orderDetails.JobRef;
                     flgValidJobRef = true;
                  }
                  if (jobSequence > 0)
                  {
                     flgValidJobRef = true;
                  }
               }
            }
            if (jobRef == "")
            {
               jobRef = "N/A";
            }
            bool flgValidSupplier = false;
            long supplierMultiAddId = -1;
            long supplierId = -1;
            if (supplierName != null && supplierName != "")
            {
               string[] supplierNameWithEmail = supplierName.Split('~');
               supplierName = supplierNameWithEmail[0].Trim();
               EntityDetailsCoreRepository entityDetailsCoreRepository = new EntityDetailsCoreRepository();
               EntityDetailsCore supplier = entityDetailsCoreRepository.GetEntityByShortName(request, supplierName);
               if (supplier != null)
               {
                  flgValidSupplier = true;
                  supplierId = supplier.EntityId ?? 0;
                  supplierAddress = supplier.AddressFull;
                  supplierTelephone = supplier.Telephone;
               }

            }
            if (supplierName == "")
            {
               supplierName = "Not Set";
            }
            long requestedById = -1;
            bool flgValidRequestedBy = false;
            if (requestedBy != null && requestedBy != "")
            {
               EntityDetailsCore edcRequestedBy = new EntityDetailsCoreRepository().GetEntityByShortName(request, requestedBy);
               if (edcRequestedBy != null)
               {
                  requestedById = edcRequestedBy.EntityId ?? 0;
                  flgValidRequestedBy = (requestedById > 0);
               }
            }
            if (requestedBy == "")
            {
               requestedBy = "Not Set";
            }
            if (formType == 1)
            {
               orderedBy = diaryResourceName;
            }

            long orderedById = -1;
            bool flgValidOrderedBy = false;
            if (orderedBy != null && orderedBy != "")
            {
               EntityDetailsCore edcOrderedBy = new EntityDetailsCoreRepository().GetEntityByShortName(request, orderedBy);
               if (edcOrderedBy != null)
               {
                  orderedById = edcOrderedBy.EntityId ?? 0;
                  flgValidOrderedBy = (orderedById > 0);
               }
            }
            if (orderedBy == "")
            {
               orderedBy = "Not Set";
            }
            EntityDetailsCore owner = new EntityDetailsCoreRepository().GetEntityByEntityId(request, 1);
            if (owner != null)
            {
               addressInvoice = owner.AddressFull;
            }
            /// eForms PO Header
            ///
            string impRef = s4bFormSubmission.S4bSubmitNo + "-" + s4bFormSubmission.S4bSubmitTs;
            double orderAmount = 0, orderDiscountAmount = 0, orderShippingAmount = 0, orderSubtotalAmount = 0, orderVATAmount = 0;
            double orderTotalAmount = 0;
            bool poItemRowExists = false;
            bool flgOtherIssue = false;
            bool flgQtyIsOK = true;
            EformsPoHeader eformsPoHeader = new EformsPoHeader();
            eformsPoHeader.FlgDeleted = false;
            eformsPoHeader.FlgOtherIssue = false;
            eformsPoHeader.DataType = 0;
            eformsPoHeader.NfsSubmitNo = s4bFormSubmission.S4bSubmitNo;
            eformsPoHeader.NfsSubmitTimeStamp = s4bFormSubmission.S4bSubmitTs;
            eformsPoHeader.ImpRef = impRef;
            eformsPoHeader.FormType = formType;
            eformsPoHeader.JobRef = jobRef;
            eformsPoHeader.FlgValidJobRef = flgValidJobRef;
            eformsPoHeader.JobSequence = jobSequence;
            eformsPoHeader.SupplierShortName = Strings.Left(supplierName, 32);
            eformsPoHeader.FlgValidSupplierShortName = flgValidSupplier;
            eformsPoHeader.SupplierId = supplierId;
            eformsPoHeader.SupplierMultiAddId = supplierMultiAddId;
            eformsPoHeader.SupplierEmail = emailAddress;
            eformsPoHeader.AttentionOf = attentionOf;
            eformsPoHeader.NfPoRef = Strings.Left(poNO, 32);
            eformsPoHeader.DatePoDate = (issueDate == DateTime.MinValue) ? timeStamp : issueDate;
            eformsPoHeader.RequiredByDate = requiredByDate;
            eformsPoHeader.FlgDeliverToSite = flgDeliverToSite;
            eformsPoHeader.OrderedByShortName = orderedBy;
            eformsPoHeader.FlgValidOrderedByShortName = flgValidOrderedBy;
            eformsPoHeader.OrderedById = orderedById;
            eformsPoHeader.RequestedByShortName = requestedBy;
            eformsPoHeader.FlgValidRequestedByShortName = flgValidRequestedBy;
            eformsPoHeader.RequestedById = requestedById;
            eformsPoHeader.PoAddressInvoice = jobAddress;
            eformsPoHeader.PoNotes = poNotes;
            eformsPoHeader.PoVoTypeSequence = poVOTypeSequence;
            eformsPoHeader.VoRef = voRef;
            eformsPoHeader.OrderId = -1;
            eformsPoHeader.OrderAmount = orderAmount;
            eformsPoHeader.OrderDiscountAmount = orderDiscountAmount;
            eformsPoHeader.OrderShippingAmount = orderShippingAmount;
            eformsPoHeader.OrderSubtotalAmount = orderSubtotalAmount;
            eformsPoHeader.OrderVatAmount = orderVATAmount;
            eformsPoHeader.OrderTotalAmount = orderTotalAmount;
            eformsPoHeader.CreatedBy = userId;
            eformsPoHeader.DateCreated = DateTime.Now;
            EformsPOHeaderRepository eformsPOHeaderRepository = new EformsPOHeaderRepository();
            EformsPoHeader newEFormsPOHeader = eformsPOHeaderRepository.Insert(request, eformsPoHeader);
            if (newEFormsPOHeader == null)
            {
               Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "For PO Ref: " + s4bFormSubmission.S4bSubmitTs + ". Unable to Add Header for eForms PO " + eformsPOHeaderRepository.Message, null);
            }
            else
            {
               if (newEFormsPOHeader.Sequence > 0)
               {
                  for (int rowIndex = 0; rowIndex < TOTAL_NO_ROWS; rowIndex++)
                  {
                     if ((qty[rowIndex] > 0) ||
                         (description[rowIndex] != null && description[rowIndex].Trim() != "") ||
                         (price[rowIndex] > 0) ||
                         (dn[rowIndex] != null && dn[rowIndex].Trim() != ""))
                     {
                        double unitAmount = qty[rowIndex] * price[rowIndex];
                        poItemRowExists = true;
                        orderAmount = orderAmount + unitAmount;
                        if (qty[rowIndex] <= 0)
                        {
                           flgQtyIsOK = false;
                        }
                        EFormsPOItems eformsPoItems = new EFormsPOItems();
                        eformsPoItems.FlgDeleted = false;
                        eformsPoItems.DataType = 0;
                        eformsPoItems.ImpRef = impRef;
                        eformsPoItems.JoinSequence = newEFormsPOHeader.Sequence;
                        eformsPoItems.ItemType = 0;
                        eformsPoItems.ItemCode = SimplicityConstants.NotAvailable;
                        eformsPoItems.ItemDesc = description[rowIndex];
                        eformsPoItems.ItemUnit = SimplicityConstants.NotAvailable;
                        eformsPoItems.ItemQuantity = qty[rowIndex];
                        eformsPoItems.ItemAmtUnitPrice = price[rowIndex];
                        eformsPoItems.ItemAmtSubtotalBeforeDiscount = unitAmount;
                        eformsPoItems.FlgItemDiscount = false;
                        eformsPoItems.ItemDiscountPcent = 0;
                        eformsPoItems.ItemAmtDiscount = 0;
                        eformsPoItems.ItemAmtSubtotal = unitAmount;
                        eformsPoItems.FlgItemVat = false;
                        eformsPoItems.ItemVatPcent = 0;
                        eformsPoItems.ItemAmtVat = 0;
                        eformsPoItems.ItemAmtTotal = unitAmount;
                        eformsPoItems.DateItemDueDate = dueDate;
                        eformsPoItems.FlgDeliverToSite = false;
                        eformsPoItems.FlgDeliveryNote = (dn[rowIndex] != null && dn[rowIndex].Trim() != "");
                        eformsPoItems.DeliveryNoteRef = dn[rowIndex];
                        eformsPoItems.DeliveryNoteQty = 0;
                        eformsPoItems.DateDeliveryNote = issueDate;
                        eformsPoItems.CreatedBy = userId;
                        eformsPoItems.DateCreated = DateTime.Now;
                        EformsPOItemsRepository eformsPOItemsRepository = new EformsPOItemsRepository();
                        EFormsPOItems eformsPoItemsNew = eformsPOItemsRepository.Insert(request, eformsPoItems);
                        if (eformsPoItemsNew == null)
                        {
                           Message = eformsPOItemsRepository.Message;
                        }
                     }
                  }
               }
               else
               {
                  Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "Unable to Insert eForms PO Header " + eformsPOHeaderRepository.Message, null);
               }
               bool poPlaced = false;
               int orderStatus = 2;
               if (!poItemRowExists || !flgQtyIsOK || orderAmount <= 0)
               {
                  flgOtherIssue = true;
               }
               if (flgValidSupplier && flgValidOrderedBy && flgValidRequestedBy && poItemRowExists && flgValidJobRef && flgQtyIsOK && orderAmount > 0)
               {
                  poPlaced = true;
                  orderStatus = 6;
               }
               PurchaseOrdersRepository purchaseOrdersRepository = new PurchaseOrdersRepository();
               string poRef = purchaseOrdersRepository.GenerateNewPORef(request);
               if (!string.IsNullOrEmpty(poRef))
               {
                  addressDelivery = jobAddress; // As per Tino's Skype instructions, we are setting delivery address to Job address.
                  PurchaseOrders purchaseOrders = new PurchaseOrders();
                  purchaseOrders.FlgEformsImport = true;
                  purchaseOrders.EformsImportId = newEFormsPOHeader.Sequence;
                  purchaseOrders.FlgPoPlaced = poPlaced;
                  purchaseOrders.PoType = 0;
                  purchaseOrders.OrderRef = poRef;
                  purchaseOrders.CustomerRef = customerRef;
                  purchaseOrders.SupplierId = supplierId;
                  purchaseOrders.SupplierAddress = supplierAddress;
                  purchaseOrders.SupplierTelephone = supplierTelephone;
                  purchaseOrders.OrderDate = (issueDate == DateTime.MinValue) ? timeStamp : issueDate;
                  purchaseOrders.AddressInvoice = addressInvoice;
                  purchaseOrders.AddressDelivery = addressDelivery;
                  purchaseOrders.OrderAmount = orderAmount;
                  purchaseOrders.OrderDiscountAmount = 0;
                  purchaseOrders.OrderShippingAmount = 0;
                  purchaseOrders.OrderSubtotalAmount = orderAmount;
                  purchaseOrders.OrderVatAmount = 0;
                  purchaseOrders.OrderTotalAmount = orderAmount;
                  purchaseOrders.ContactId = requestedById;
                  purchaseOrders.VehicleReg = "";
                  purchaseOrders.AdditionInfo = additionalInfo;
                  purchaseOrders.RequiredByDate = dueDate;
                  purchaseOrders.FlgDispatchDate = false;
                  purchaseOrders.DateDespatchDate = null;
                  purchaseOrders.OrderedBy = orderedBy;
                  purchaseOrders.OrderStatus = orderStatus;
                  purchaseOrders.UserField01 = "";
                  purchaseOrders.UserField02 = "";
                  purchaseOrders.UserField03 = "";
                  purchaseOrders.UserField04 = "";
                  purchaseOrders.UserField05 = "";
                  purchaseOrders.UserField06 = "";
                  purchaseOrders.UserField07 = "";
                  purchaseOrders.UserField08 = "";
                  purchaseOrders.UserField09 = "";
                  purchaseOrders.UserField10 = "";
                  purchaseOrders.CreatedBy = userId;
                  purchaseOrders.DateCreated = timeStamp;
                  purchaseOrders.LastAmendedBy = userId;
                  purchaseOrders.DateLastAmended = timeStamp;
                  List<PurchaseOrderItems> poItemsList = null;
                  for (int rowIndex = 0; rowIndex < TOTAL_NO_ROWS; rowIndex++)
                  {
                     if ((qty[rowIndex] > 0) ||
                         (description[rowIndex] != null && description[rowIndex].Trim() != "") ||
                         (price[rowIndex] > 0) ||
                         (dn[rowIndex] != null && dn[rowIndex].Trim() != ""))
                     {
                        if (price[rowIndex] < 0.01)
                        {
                           price[rowIndex] = 0.01;
                        }
                        double unitAmount = qty[rowIndex] * price[rowIndex];
                        PurchaseOrderItems purchaseOrderItems = new PurchaseOrderItems();
                        purchaseOrderItems.OrderId = -1;
                        purchaseOrderItems.ItemImportType = 0;
                        purchaseOrderItems.RequestSequence = -1;
                        purchaseOrderItems.JobSequence = jobSequence;
                        purchaseOrderItems.TransType = SimplicityConstants.SupplierTransType;
                        purchaseOrderItems.EntityId = -1;
                        purchaseOrderItems.ItemType = 0;
                        purchaseOrderItems.ItemHours = 0;
                        purchaseOrderItems.ItemCode = "NO_CODE";
                        purchaseOrderItems.ItemDesc = description[rowIndex];
                        purchaseOrderItems.ItemUnit = "EA";
                        purchaseOrderItems.ItemQuantity = qty[rowIndex];
                        purchaseOrderItems.ItemAmount = unitAmount;
                        purchaseOrderItems.FlgItemDiscount = false;
                        purchaseOrderItems.ItemDiscountPcent = 0;
                        purchaseOrderItems.ItemDiscountAmount = 0;
                        purchaseOrderItems.ItemSubtotal = unitAmount;
                        purchaseOrderItems.FlgItemVat = false;
                        purchaseOrderItems.ItemVatPcent = 0;
                        purchaseOrderItems.ItemTotal = unitAmount;
                        if (poItemsList == null)
                        {
                           poItemsList = new List<PurchaseOrderItems>();
                        }
                        poItemsList.Add(purchaseOrderItems);
                     }
                  }
                  purchaseOrders.POItems = poItemsList;
                  PurchaseOrders purchaseOrdersNew = purchaseOrdersRepository.Insert(request, purchaseOrders);
                  if (purchaseOrdersNew == null)
                  {
                     Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "For PO Ref: " + poRef + ", Unable to Add Purchase order for eForms PO. " + purchaseOrdersRepository.Message, null);
                  }
                  EformsPoHeader eformsPoHeaderMin = new EformsPoHeader();
                  eformsPoHeaderMin.Sequence = newEFormsPOHeader.Sequence;
                  eformsPoHeaderMin.OrderId = purchaseOrdersNew.OrderId;
                  eformsPoHeaderMin.FlgOtherIssue = flgOtherIssue;
                  eformsPoHeaderMin.OrderAmount = orderAmount;
                  eformsPoHeaderMin.OrderSubtotalAmount = orderAmount;
                  eformsPoHeaderMin.OrderTotalAmount = orderAmount;
                  eformsPoHeaderMin.LastAmendedBy = userId;
                  eformsPoHeaderMin.DateLastAmended = timeStamp;
                  if (!eformsPOHeaderRepository.UpdateOrderIdAndAmounts(request, eformsPoHeaderMin))
                  {
                     Message = eformsPOHeaderRepository.Message;
                  }
               }
               else
               {
                  Message = purchaseOrdersRepository.Message;
               }
               returnValue = true;
            }
         }
         catch (Exception ex)
         {
            Message = "Unable to Process Lowry - PO. Exception: " + ex.Message;
         }
         return returnValue;
      }

      // Lowry - Purchase Order Blank Contractor V01 905560356
      private bool ProcessLowryPOContractor(HttpRequest request, S4BFormRequest s4bFormRequest, S4bFormSubmissions s4bFormSubmission,
                                            RootObject root, Dictionary<string, S4BFormsControl> s4bFormsControls, int formType)
      {
         const string METHOD_NAME = "S4BFormsRepository.ProcessLowryPOContractor()";
         bool returnValue = false;
         int userId = Utilities.GetUserIdFromRequest(request);
         long jobSequence = -1, jobAddressId = -1, jobClientId = -1;
         long appSequence = -1;
         DateTime datAttended = DateTime.Now;
         DateTime datEngineer = DateTime.Now;
         DateTime? timeStamp = s4bFormSubmission.DateCreated;
         string contractorName = "", requestedBy = "", orderedBy = "";
         const int TOTAL_NO_ROWS = 14;
         string jobRef = "", poNO = "", po1 = "", po2 = "", po3 = "", po4 = "", po5 = "", jobAddress = "", emailAddress = "";
         string customerRef = "", contractorAddress = "", contractorTelephone = "";
         string addressInvoice = "", addressDelivery = "", additionalInfo = "eForms Import", diaryResourceName = "";
         int index = -1;
         string[] description = new string[TOTAL_NO_ROWS];
         string[] cOrd = new string[TOTAL_NO_ROWS];
         double[] qty = new double[TOTAL_NO_ROWS];
         double[] price = new double[TOTAL_NO_ROWS];
         string[] jobDesc = new string[TOTAL_NO_ROWS];
         string[] dn = new string[TOTAL_NO_ROWS];
         DateTime issueDate = new DateTime();
         DateTime dueDate = new DateTime();
         Dictionary<string, string> imageValues = new Dictionary<string, string>();
         try
         {
            foreach (KeyValuePair<string, S4BFormsControl> s4bFormControlEntry in s4bFormsControls)
            {
               S4BFormsControl s4bFormControl = s4bFormControlEntry.Value;
               if (s4bFormControl.fieldValue != null)
               {
                  switch (s4bFormControlEntry.Key)
                  {
                     case "VARIABLE_PG1_JOB_SEQUENCE":
                        if (long.TryParse(s4bFormControl.fieldValue, out jobSequence))
                        {
                        };
                        break;
                     case "VARIABLE_PG1_JOB_CLIENT_ID":
                        if (long.TryParse(s4bFormControl.fieldValue, out jobClientId))
                        {
                        };
                        break;
                     case "VARIABLE_PG1_JOB_ADDRESS_ID":
                        if (long.TryParse(s4bFormControl.fieldValue, out jobAddressId))
                        {
                        };
                        break;
                     case "VARIABLE_PG1_USER_ID":
                        if (int.TryParse(s4bFormControl.fieldValue, out userId))
                        {
                        };
                        break;

                     case "VARIABLE_PG1_DIARY_ENTRY_ID":
                        long.TryParse(s4bFormControl.fieldValue, out appSequence);
                        break;

                     case "VAR_PG1_PO_NO":
                        poNO = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG1_CONTRACTOR":
                        contractorName = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG1_EMAIL_ADDRESS":
                        emailAddress = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG1_JOB_ADDRESS":
                        jobAddress = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG1_JOB_REF":
                        jobRef = s4bFormControl.fieldValue;
                        break;

                     case "VARIABLE_PG1_DIARY_RESOURCE":
                        diaryResourceName = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG1_PO_NO1":
                        po1 = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG1_PO_NO2":
                        po2 = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG1_PO_NO3":
                        po3 = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG1_PO_NO4":
                        po4 = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG1_PO_NO5":
                        po5 = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG1_REQUESTED_BY":
                        requestedBy = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG1_ORDERED_BY":
                        orderedBy = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG1_ISSUE_DATE":
                        try
                        {
                           issueDate = DateTime.ParseExact(s4bFormControl.fieldValue.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        }
                        catch (Exception ex)
                        { }
                        break;

                     case "FIELD_PG1_DUE_DATE":
                        try
                        {
                           dueDate = DateTime.ParseExact(s4bFormControl.fieldValue.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        }
                        catch (Exception ex)
                        { }
                        break;

                     case "FIELD_PG1_ROW01_QTY":
                     case "FIELD_PG1_ROW02_QTY":
                     case "FIELD_PG1_ROW03_QTY":
                     case "FIELD_PG1_ROW04_QTY":
                     case "FIELD_PG1_ROW05_QTY":
                     case "FIELD_PG1_ROW06_QTY":
                     case "FIELD_PG1_ROW07_QTY":
                     case "FIELD_PG2_ROW08_QTY":
                     case "FIELD_PG2_ROW09_QTY":
                     case "FIELD_PG2_ROW10_QTY":
                     case "FIELD_PG2_ROW11_QTY":
                     case "FIELD_PG2_ROW12_QTY":
                     case "FIELD_PG2_ROW13_QTY":
                     case "FIELD_PG2_ROW14_QTY":
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        if (index > 0)
                        {
                           double result = -1;
                           try
                           {
                              double.TryParse(s4bFormControl.fieldValue, out result);
                           }
                           catch (Exception) { }
                           qty[index - 1] = result;
                        }
                        break;

                     case "FIELD_PG1_ROW01_DESCRIPTION":
                     case "FIELD_PG1_ROW02_DESCRIPTION":
                     case "FIELD_PG1_ROW03_DESCRIPTION":
                     case "FIELD_PG1_ROW04_DESCRIPTION":
                     case "FIELD_PG1_ROW05_DESCRIPTION":
                     case "FIELD_PG1_ROW06_DESCRIPTION":
                     case "FIELD_PG1_ROW07_DESCRIPTION":
                     case "FIELD_PG1_ROW08_DESCRIPTION":
                     case "FIELD_PG2_ROW09_DESCRIPTION":
                     case "FIELD_PG2_ROW10_DESCRIPTION":
                     case "FIELD_PG2_ROW11_DESCRIPTION":
                     case "FIELD_PG2_ROW12_DESCRIPTION":
                     case "FIELD_PG2_ROW13_DESCRIPTION":
                     case "FIELD_PG2_ROW14_DESCRIPTION":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        if (index > 0)
                        {
                           description[index - 1] = s4bFormControl.fieldValue;
                        }
                        break;

                     case "FIELD_PG1_ROW01_PRICE":
                     case "FIELD_PG1_ROW02_PRICE":
                     case "FIELD_PG1_ROW03_PRICE":
                     case "FIELD_PG1_ROW04_PRICE":
                     case "FIELD_PG1_ROW05_PRICE":
                     case "FIELD_PG1_ROW06_PRICE":
                     case "FIELD_PG1_ROW07_PRICE":
                     case "FIELD_PG2_ROW08_PRICE":
                     case "FIELD_PG2_ROW09_PRICE":
                     case "FIELD_PG2_ROW10_PRICE":
                     case "FIELD_PG2_ROW11_PRICE":
                     case "FIELD_PG2_ROW12_PRICE":
                     case "FIELD_PG2_ROW13_PRICE":
                     case "FIELD_PG2_ROW14_PRICE":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        if (index > 0)
                        {
                           double result = -1;
                           try
                           {
                              double.TryParse(s4bFormControl.fieldValue, out result);
                           }
                           catch (Exception) { }
                           price[index - 1] = result;
                        }
                        break;

                     case "FIELD_PG1_ROW01_DN":
                     case "FIELD_PG1_ROW02_DN":
                     case "FIELD_PG1_ROW03_DN":
                     case "FIELD_PG1_ROW04_DN":
                     case "FIELD_PG1_ROW05_DN":
                     case "FIELD_PG1_ROW06_DN":
                     case "FIELD_PG1_ROW07_DN":
                     case "FIELD_PG1_ROW08_DN":
                     case "FIELD_PG2_ROW09_DN":
                     case "FIELD_PG2_ROW10_DN":
                     case "FIELD_PG2_ROW11_DN":
                     case "FIELD_PG2_ROW12_DN":
                     case "FIELD_PG2_ROW13_DN":
                     case "FIELD_PG2_ROW14_DN":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        if (index > 0)
                        {
                           dn[index - 1] = s4bFormControl.fieldValue;
                        }
                        break;

                     case "FIELD_PG1_ROW01_C_OR_D":
                     case "FIELD_PG1_ROW02_C_OR_D":
                     case "FIELD_PG1_ROW03_C_OR_D":
                     case "FIELD_PG1_ROW04_C_OR_D":
                     case "FIELD_PG1_ROW05_C_OR_D":
                     case "FIELD_PG1_ROW06_C_OR_D":
                     case "FIELD_PG1_ROW07_C_OR_D":
                     case "FIELD_PG1_ROW08_C_OR_D":
                     case "FIELD_PG2_ROW09_C_OR_D":
                     case "FIELD_PG2_ROW10_C_OR_D":
                     case "FIELD_PG2_ROW11_C_OR_D":
                     case "FIELD_PG2_ROW12_C_OR_D":
                     case "FIELD_PG2_ROW13_C_OR_D":
                     case "FIELD_PG2_ROW14_C_OR_D":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        if (index > 0)
                        {
                           cOrd[index - 1] = s4bFormControl.fieldValue;
                        }
                        break;
                  }
               }
            }
            if (userId <= 0)
            {
               userId = 1;
            }
            DiaryResources diaryResource = Utilities.GetDiaryResourceByUserId(request, userId);
            bool flgValidJobRef = false;
            Orders orderDetails = null;
            OrdersRepository ordersRepository = new OrdersRepository(null);
            if (jobSequence > 0)
            {
               orderDetails = ordersRepository.GetOrderDetailsBySequence(jobSequence, request);
               if (orderDetails != null)
               {
                  jobSequence = orderDetails.Sequence ?? 0;
                  jobAddress = orderDetails.JobAddress;
                  jobRef = orderDetails.JobRef;
                  flgValidJobRef = true;
               }
            }
            else
            {
               if (jobRef != null && jobRef != "")
               {
                  if (Information.IsNumeric(jobRef) && jobRef.Length < 8)
                  {
                     jobRef = jobRef.ToString().PadLeft(8, '0');
                  }
                  orderDetails = ordersRepository.GetOrderByJobRef(jobRef, request);
                  if (orderDetails != null)
                  {
                     jobSequence = orderDetails.Sequence ?? 0;
                     jobAddress = orderDetails.JobAddress;
                     jobRef = orderDetails.JobRef;
                     flgValidJobRef = true;
                  }
                  if (jobSequence > 0)
                  {
                     flgValidJobRef = true;
                  }
               }
            }
            if (jobRef == "")
            {
               jobRef = "N/A";
            }
            bool flgValidSupplier = false;
            long contractorId = -1;
            if (contractorName != null && contractorName != "")
            {
               string[] contractorNameWithEmail = contractorName.Split('~');
               contractorName = contractorNameWithEmail[0].Trim();
               EntityDetailsCoreRepository entityDetailsCoreRepository = new EntityDetailsCoreRepository();
               EntityDetailsCore contractor = entityDetailsCoreRepository.GetEntityByShortName(request, contractorName);
               if (contractor != null)
               {
                  flgValidSupplier = true;
                  contractorId = contractor.EntityId ?? 0;
                  contractorAddress = contractor.AddressFull;
                  contractorTelephone = contractor.Telephone;
               }

            }
            if (contractorName == "")
            {
               contractorName = "Not Set";
            }
            long requestedById = -1;
            bool flgValidRequestedBy = false;
            if (requestedBy != null && requestedBy != "")
            {
               EntityDetailsCore edcRequestedBy = new EntityDetailsCoreRepository().GetEntityByShortName(request, requestedBy);
               if (edcRequestedBy != null)
               {
                  requestedById = edcRequestedBy.EntityId ?? 0;
                  flgValidRequestedBy = (requestedById > 0);
               }
            }
            if (requestedBy == "")
            {
               requestedBy = "Not Set";
            }
            if (formType == 1)
            {
               orderedBy = diaryResourceName;
            }

            long orderedById = -1;
            bool flgValidOrderedBy = false;
            if (orderedBy != null && orderedBy != "")
            {
               EntityDetailsCore edcOrderedBy = new EntityDetailsCoreRepository().GetEntityByShortName(request, orderedBy);
               if (edcOrderedBy != null)
               {
                  orderedById = edcOrderedBy.EntityId ?? 0;
                  flgValidOrderedBy = (orderedById > 0);
               }
            }
            if (orderedBy == "")
            {
               orderedBy = "Not Set";
            }
            EntityDetailsCore owner = new EntityDetailsCoreRepository().GetEntityByEntityId(request, 1);
            if (owner != null)
            {
               addressInvoice = owner.AddressFull;
            }
            /// eForms PO Header
            ///
            PurchaseOrdersRepository purchaseOrdersRepository = new PurchaseOrdersRepository();
            string poRef = purchaseOrdersRepository.GenerateNewPORef(request);

            string impRef = s4bFormSubmission.S4bSubmitNo + "-" + s4bFormSubmission.S4bSubmitTs;
            double orderAmount = 0;
            bool poItemRowExists = false;
            bool flgQtyIsOK = true;
            SubConPoHeader subConPOHeader = new SubConPoHeader();
            subConPOHeader.FlgEformsImport = true;
            subConPOHeader.EformsImportId = s4bFormSubmission.Sequence;
            subConPOHeader.FlgPoPlaced = false;
            subConPOHeader.PoType = 0;
            subConPOHeader.PORef = poRef;
            subConPOHeader.CustomerRef = SimplicityConstants.NotSet;
            subConPOHeader.JobSequence = jobSequence;
            subConPOHeader.EntityId = contractorId;
            subConPOHeader.EntityAddress = contractorAddress;
            subConPOHeader.EntityTelephone = contractorTelephone;
            subConPOHeader.PODate = (issueDate == DateTime.MinValue) ? timeStamp : issueDate;
            subConPOHeader.AddressInvoice = SimplicityConstants.NotSet;
            subConPOHeader.AddressDelivery = SimplicityConstants.NotSet;
            subConPOHeader.RequestedId = requestedById;
            subConPOHeader.VehicleReg = SimplicityConstants.NotSet;
            subConPOHeader.RequiredByDate = (dueDate == DateTime.MinValue) ? timeStamp : dueDate;
            subConPOHeader.FlgDispatchDate = false;
            subConPOHeader.DateDespatchDate = null;
            subConPOHeader.OrderedBy = "";
            subConPOHeader.POStatus = 0;
            subConPOHeader.UserField01 = "";
            subConPOHeader.UserField02 = "";
            subConPOHeader.UserField03 = "";
            subConPOHeader.UserField04 = "";
            subConPOHeader.UserField05 = "";
            subConPOHeader.UserField06 = "";
            subConPOHeader.UserField07 = "";
            subConPOHeader.UserField08 = "";
            subConPOHeader.UserField09 = "";
            subConPOHeader.UserField10 = "";
            subConPOHeader.CreatedBy = userId;
            subConPOHeader.DateCreated = timeStamp;
            subConPOHeader.LastAmendedBy = userId;
            subConPOHeader.DateLastAmended = timeStamp;
            List<SubConPOItems> subConPOItemsList = null;
            for (int rowIndex = 0; rowIndex < TOTAL_NO_ROWS; rowIndex++)
            {
               if ((qty[rowIndex] > 0) ||
                   (description[rowIndex] != null && description[rowIndex].Trim() != "") ||
                   (price[rowIndex] > 0) ||
                   (dn[rowIndex] != null && dn[rowIndex].Trim() != ""))
               {
                  double unitAmount = qty[rowIndex] * price[rowIndex];
                  poItemRowExists = true;
                  orderAmount = orderAmount + unitAmount;
                  if (qty[rowIndex] <= 0)
                  {
                     flgQtyIsOK = false;
                  }
                  SubConPOItems subConPOItems = new SubConPOItems();
                  subConPOItems.POSequence = -1;
                  subConPOItems.EntityId = -1;
                  subConPOItems.ItemType = 0;
                  subConPOItems.ItemHours = 0;
                  subConPOItems.ItemCode = "NO_CODE";
                  subConPOItems.ItemDesc = description[rowIndex];
                  subConPOItems.ItemUnit = "EA";
                  subConPOItems.ItemQuantity = qty[rowIndex];
                  subConPOItems.ItemAmountMat = price[rowIndex];
                  subConPOItems.ItemAmountNet = unitAmount;
                  subConPOItems.FlgItemDiscount = false;
                  subConPOItems.ItemDiscountPcent = 0;
                  subConPOItems.ItemDiscountAmount = 0;
                  subConPOItems.ItemSubtotal = unitAmount;
                  subConPOItems.FlgItemVat = false;
                  subConPOItems.ItemVatPcent = 0;
                  subConPOItems.ItemTotal = unitAmount;
                  subConPOItems.CreatedBy = userId;
                  subConPOItems.DateCreated = timeStamp;

                  if (subConPOItemsList == null)
                  {
                     subConPOItemsList = new List<SubConPOItems>();
                  }
                  subConPOItemsList.Add(subConPOItems);
               }
            }
            subConPOHeader.SubConPOItems = subConPOItemsList;
            SubConPOHeaderRepository subConPOHeaderRepository = new SubConPOHeaderRepository();
            SubConPoHeader newSubConPoHeader = subConPOHeaderRepository.Insert(request, subConPOHeader);
            if (newSubConPoHeader == null)
            {
               Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "For PO Ref: " + s4bFormSubmission.S4bSubmitTs + ". Unable to Add Header for Sub Con PO " + subConPOHeaderRepository.Message, null);
            }
            bool poPlaced = false;
            int orderStatus = 2;
            if (flgValidSupplier && flgValidOrderedBy && flgValidRequestedBy && poItemRowExists && flgValidJobRef && flgQtyIsOK && orderAmount > 0)
            {
               poPlaced = true;
               orderStatus = 6;
            }
            //if (!string.IsNullOrEmpty(poRef))
            //{
            //    addressDelivery = jobAddress; // As per Tino's Skype instructions, we are setting delivery address to Job address.
            //    PurchaseOrders purchaseOrders = new PurchaseOrders();
            //    purchaseOrders.FlgEformsImport = true;
            //    purchaseOrders.EformsImportId = newSubConPoHeader.Sequence;
            //    purchaseOrders.FlgPoPlaced = poPlaced;
            //    purchaseOrders.PoType = 0;
            //    purchaseOrders.OrderRef = poRef;
            //    purchaseOrders.CustomerRef = customerRef;
            //    purchaseOrders.SupplierId = -1; // No Supplier set here.
            //    purchaseOrders.SupplierAddress = "";
            //    purchaseOrders.SupplierTelephone = "";
            //    purchaseOrders.OrderDate = (issueDate == DateTime.MinValue) ? timeStamp : issueDate;
            //    purchaseOrders.AddressInvoice = addressInvoice;
            //    purchaseOrders.AddressDelivery = addressDelivery;
            //    purchaseOrders.OrderAmount = orderAmount;
            //    purchaseOrders.OrderDiscountAmount = 0;
            //    purchaseOrders.OrderShippingAmount = 0;
            //    purchaseOrders.OrderSubtotalAmount = orderAmount;
            //    purchaseOrders.OrderVatAmount = 0;
            //    purchaseOrders.OrderTotalAmount = orderAmount;
            //    purchaseOrders.ContactId = requestedById;
            //    purchaseOrders.VehicleReg = "";
            //    purchaseOrders.AdditionInfo = additionalInfo;
            //    purchaseOrders.RequiredByDate = dueDate;
            //    purchaseOrders.FlgDispatchDate = false;
            //    purchaseOrders.DateDespatchDate = null;
            //    purchaseOrders.OrderedBy = orderedBy;
            //    purchaseOrders.OrderStatus = orderStatus;
            //    purchaseOrders.UserField01 = "";
            //    purchaseOrders.UserField02 = "";
            //    purchaseOrders.UserField03 = "";
            //    purchaseOrders.UserField04 = "";
            //    purchaseOrders.UserField05 = "";
            //    purchaseOrders.UserField06 = "";
            //    purchaseOrders.UserField07 = "";
            //    purchaseOrders.UserField08 = "";
            //    purchaseOrders.UserField09 = "";
            //    purchaseOrders.UserField10 = "";
            //    purchaseOrders.CreatedBy = userId;
            //    purchaseOrders.DateCreated = timeStamp;
            //    purchaseOrders.LastAmendedBy = userId;
            //    purchaseOrders.DateLastAmended = timeStamp;
            //    List<PurchaseOrderItems> poItemsList = null;
            //    //We do not need this for Sub Contractor PO's
            //    //for (int rowIndex = 0; rowIndex < TOTAL_NO_ROWS; rowIndex++)
            //    //{
            //    //    if ((qty[rowIndex] > 0) ||
            //    //        (description[rowIndex] != null && description[rowIndex].Trim() != "") ||
            //    //        (price[rowIndex] > 0) ||
            //    //        (dn[rowIndex] != null && dn[rowIndex].Trim() != ""))
            //    //    {
            //    //        if (price[rowIndex] < 0.01)
            //    //        {
            //    //            price[rowIndex] = 0.01;
            //    //        }
            //    //        double unitAmount = qty[rowIndex] * price[rowIndex];
            //    //        PurchaseOrderItems purchaseOrderItems = new PurchaseOrderItems();
            //    //        purchaseOrderItems.OrderId = -1;
            //    //        purchaseOrderItems.ItemImportType = 0;
            //    //        purchaseOrderItems.RequestSequence = -1;
            //    //        purchaseOrderItems.JobSequence = jobSequence;
            //    //        purchaseOrderItems.TransType = SimplicityConstants.ContractorTransType;
            //    //        purchaseOrderItems.EntityId = -1;
            //    //        purchaseOrderItems.ItemType = 0;
            //    //        purchaseOrderItems.ItemHours = 0;
            //    //        purchaseOrderItems.ItemCode = "NO_CODE";
            //    //        purchaseOrderItems.ItemDesc = description[rowIndex];
            //    //        purchaseOrderItems.ItemUnit = "EA";
            //    //        purchaseOrderItems.ItemQuantity = qty[rowIndex];
            //    //        purchaseOrderItems.ItemAmount = unitAmount;
            //    //        purchaseOrderItems.FlgItemDiscount = false;
            //    //        purchaseOrderItems.ItemDiscountPcent = 0;
            //    //        purchaseOrderItems.ItemDiscountAmount = 0;
            //    //        purchaseOrderItems.ItemSubtotal = unitAmount;
            //    //        purchaseOrderItems.FlgItemVat = false;
            //    //        purchaseOrderItems.ItemVatPcent = 0;
            //    //        purchaseOrderItems.ItemTotal = unitAmount;
            //    //        if (poItemsList == null)
            //    //        {
            //    //            poItemsList = new List<PurchaseOrderItems>();
            //    //        }
            //    //        poItemsList.Add(purchaseOrderItems);
            //    //    }
            //    //}
            //    purchaseOrders.POItems = poItemsList;
            //    PurchaseOrders purchaseOrdersNew = purchaseOrdersRepository.Insert(request, purchaseOrders);
            //    if (purchaseOrdersNew == null)
            //    {
            //        Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "For PO Ref: " + poRef + ", Unable to Add Purchase order for Contractor PO. " + purchaseOrdersRepository.Message, null);
            //    }

            SubConPoHeader subConPOHeaderMin = new SubConPoHeader();
            subConPOHeaderMin.Sequence = newSubConPoHeader.Sequence;
            subConPOHeaderMin.PORef = poRef;
            subConPOHeaderMin.PoAmtMat = orderAmount;
            subConPOHeaderMin.PoAmtSubtotal = orderAmount;
            subConPOHeaderMin.PoAmtTotal = orderAmount;
            subConPOHeaderMin.LastAmendedBy = userId;
            subConPOHeaderMin.DateLastAmended = timeStamp;
            if (!subConPOHeaderRepository.UpdatePORefAndAmounts(request, subConPOHeaderMin))
            {
               Message = subConPOHeaderRepository.Message;
            }
            //}
            //else
            //{
            //    Message = purchaseOrdersRepository.Message;
            //}
            returnValue = true;
         }
         catch (Exception ex)
         {
            Message = "Unable to Process Lowry - PO Contractor. Exception: " + ex.Message;
         }
         return returnValue;
      }

      // UNC - Template S4B New Client Sales Meeting V01 - 1788502524
      private bool ProcessUNCNewClientSalesMeeting(HttpRequest request, S4BFormRequest s4bFormRequest, S4bFormSubmissions s4bFormSubmission,
                                                   RootObject root, Dictionary<string, S4BFormsControl> s4bFormsControls)
      {
         bool returnValue = false;
         DateTime? datRegistered = DateTime.MinValue;
         int userId = Int32.Parse(request.Headers["UserId"].ToString());
         int jobAddressId = -1, jobClientId = -1, appSequence = -1;
         long jobSequence = -1;
         string userName = "";
         string submit = "";

         const int TOTAL_NO_ROWS_ATTENDEESFROM = 4;
         const int TOTAL_NO_ROWS_REPRESENTATIVES = 2;
         const int TOTAL_NO_ROWS_PURPOSEOFMEETING = 5;
         const int TOTAL_NO_ROWS_RECURRINGFEES = 9;
         const int TOTAL_NO_ROWS_NEXTSTEPS = 9;

         string email = "", quotationDate = "", customerPORef = "", meetingNotes = "", companyDetails = "";
         string clientDataTransfer = "", cloudAnalysingDay = "", cloudLicenses = "", cloudOther = "", cloudTrainingDays = "";
         string companyClients = "", companySystems = "", initialeFormsSetupFee = "", initialCloudSetupFee = "", issues = "", itemDescription = "";
         string sectionName = "", serviceStartDate = "", timeTrackLicenses = "", requirementsTimeTrack = "", meetingNotesOther = "", paymentDueDate = "";
         string noOfUsers = "", paymentTerms = "", incumbentSupplier = "", eFormTrainingDays = "", typeOfBusiness = "", eformsLicenses = "";
         string eformsTemplateConfig = "", eFormsAnalysingDay = "", clientName = "", clientRef = "", contactName = "";
         string contactTel = "", customerAddress = "";
         string[] attendeesFromName = new string[TOTAL_NO_ROWS_ATTENDEESFROM];
         string[] attendeesFromJobRole = new string[TOTAL_NO_ROWS_ATTENDEESFROM];
         string[] attendeesFromDepartment = new string[TOTAL_NO_ROWS_ATTENDEESFROM];
         string[] representative1 = new string[TOTAL_NO_ROWS_REPRESENTATIVES];
         string[] representative2 = new string[TOTAL_NO_ROWS_REPRESENTATIVES];
         string[] representative3 = new string[TOTAL_NO_ROWS_REPRESENTATIVES];
         string[] representative4 = new string[TOTAL_NO_ROWS_REPRESENTATIVES];
         string[] purposeOfMeeting = new string[TOTAL_NO_ROWS_PURPOSEOFMEETING];
         string[] purposeOfMeeting2 = new string[TOTAL_NO_ROWS_PURPOSEOFMEETING];
         string[] recurringFees = new string[TOTAL_NO_ROWS_RECURRINGFEES];
         string[] nextStepsTodo = new string[TOTAL_NO_ROWS_NEXTSTEPS];
         string[] nextStepsNotes = new string[TOTAL_NO_ROWS_NEXTSTEPS];
         bool[] nextStepsDone = new bool[TOTAL_NO_ROWS_NEXTSTEPS];



         DateTime? datUser2 = DateTime.MinValue;
         int index = -1;
         DateTime? timeStamp = s4bFormSubmission.DateCreated;
         Dictionary<string, string> imageValues = new Dictionary<string, string>();
         try
         {
            foreach (KeyValuePair<string, S4BFormsControl> s4bFormControlEntry in s4bFormsControls)
            {
               S4BFormsControl s4bFormControl = s4bFormControlEntry.Value;
               if (s4bFormControl.fieldValue != null)
               {
                  switch (s4bFormControlEntry.Key)
                  {
                     case "VARIABLE_PG1_JOB_SEQUENCE":
                        if (long.TryParse(s4bFormControl.fieldValue, out jobSequence))
                        {
                        };
                        break;
                     case "VARIABLE_PG1_JOB_CLIENT_ID":
                        if (int.TryParse(s4bFormControl.fieldValue, out jobClientId))
                        {
                        };
                        break;
                     case "VARIABLE_PG1_JOB_ADDRESS_ID":
                        if (int.TryParse(s4bFormControl.fieldValue, out jobAddressId))
                        {
                        };
                        break;
                     case "VARIABLE_PG1_USER_ID":
                        if (int.TryParse(s4bFormControl.fieldValue, out userId))
                        {
                        };
                        break;
                     case "VARIABLE_PG1_DIARY_RESOURCE":
                        userName = s4bFormControl.fieldValue;
                        break;
                     case "VARIABLE_PG1_DIARY_ENTRY_ID":
                        int.TryParse(s4bFormControl.fieldValue, out appSequence);
                        break;
                     case "VARIABLE_PG1_JOB_CLIENT_NAME":
                        clientName = s4bFormControl.fieldValue;
                        break;
                     case "VARIABLE_PG1_JOB_CLIENT_REF":
                        clientRef = s4bFormControl.fieldValue;
                        break;
                     case "VARIABLE_PG1_JOB_ADDRESS_V":
                        customerAddress = s4bFormControl.fieldValue;
                        break;
                     case "VARIABLE_PG1_JOB_OCCUPIER_NAME":
                        contactName = s4bFormControl.fieldValue;
                        break;
                     case "VARIABLE_PG1_JOB_OCCUPIER_TEL_HOME":
                        contactTel = s4bFormControl.fieldValue;
                        break;
                     case "FIELD_PG1_EMAIL":
                        email = s4bFormControl.fieldValue;
                        break;
                     case "FIELD_PG1_QUOTATION_DATE":
                        quotationDate = s4bFormControl.fieldValue;
                        break;
                     case "FIELD_PG1_CUSTOMER_PO_REF":
                        customerPORef = s4bFormControl.fieldValue;
                        break;
                     case "FIELD_PG1_ROW01_NAME":
                     case "FIELD_PG1_ROW02_NAME":
                     case "FIELD_PG1_ROW03_NAME":
                     case "FIELD_PG1_ROW04_NAME":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        if (index > 0)
                        {
                           attendeesFromName[index - 1] = s4bFormControl.fieldValue;
                        }
                        break;
                     case "FIELD_PG1_ROW01_JOB_ROLE":
                     case "FIELD_PG1_ROW02_JOB_ROLE":
                     case "FIELD_PG1_ROW03_JOB_ROLE":
                     case "FIELD_PG1_ROW04_JOB_ROLE":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        if (index > 0)
                        {
                           attendeesFromJobRole[index - 1] = s4bFormControl.fieldValue;
                        }
                        break;
                     case "FIELD_PG1_ROW01_DEPARTMENT":
                     case "FIELD_PG1_ROW02_DEPARTMENT":
                     case "FIELD_PG1_ROW03_DEPARTMENT":
                     case "FIELD_PG1_ROW04_DEPARTMENT":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        if (index > 0)
                        {
                           attendeesFromDepartment[index - 1] = s4bFormControl.fieldValue;
                        }
                        break;
                     case "FIELD_PG1_ROW05_REPRESENTATIVE01":
                     case "FIELD_PG1_ROW06_REPRESENTATIVE01":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        if (index > 0)
                        {
                           representative1[index - 5] = s4bFormControl.fieldValue;
                        }
                        break;
                     case "FIELD_PG1_ROW05_REPRESENTATIVE02":
                     case "FIELD_PG1_ROW06_REPRESENTATIVE02":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        if (index > 0)
                        {
                           representative2[index - 5] = s4bFormControl.fieldValue;
                        }
                        break;
                     case "FIELD_PG1_ROW05_REPRESENTATIVE03":
                     case "FIELD_PG1_ROW06_REPRESENTATIVE03":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        if (index > 0)
                        {
                           representative3[index - 5] = s4bFormControl.fieldValue;
                        }
                        break;
                     case "FIELD_PG1_ROW05_REPRESENTATIVE04":
                     case "FIELD_PG1_ROW06_REPRESENTATIVE04":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        if (index > 0)
                        {
                           representative4[index - 5] = s4bFormControl.fieldValue;
                        }
                        break;
                     case "FIELD_PG1_ROW07_COL01":
                     case "FIELD_PG1_ROW08_COL01":
                     case "FIELD_PG1_ROW09_COL01":
                     case "FIELD_PG1_ROW10_COL01":
                     case "FIELD_PG1_ROW11_COL01":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        if (index > 0)
                        {
                           purposeOfMeeting[index - 7] = s4bFormControl.fieldValue;
                        }
                        break;
                     case "FIELD_PG1_ROW07_COL02":
                     case "FIELD_PG1_ROW08_COL02":
                     case "FIELD_PG1_ROW09_COL02":
                     case "FIELD_PG1_ROW10_COL02":
                     case "FIELD_PG1_ROW11_COL02":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        if (index > 0)
                        {
                           purposeOfMeeting2[index - 7] = s4bFormControl.fieldValue;
                        }
                        break;

                     case "FIELD_PG1_MINUTES_NOTES":
                        meetingNotes = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG2_COMPANY_DETAILS":
                        companyDetails = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG2_COMPANY_CLIENTS":
                        companyClients = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG2_TYPE_OF_BUSINESS":
                        typeOfBusiness = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG2_INCUMBENT_SUPPLIER":
                        incumbentSupplier = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG2_COMPANY_SYSTEMS":
                        companySystems = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG2_ISSUES":
                        issues = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG2_SECTION_NAME":
                        sectionName = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG2_NO_OF_USERS":
                        noOfUsers = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG2_EFORMS_LICENSES":
                        eformsLicenses = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG2_CLOUD_LICENSES":
                        cloudLicenses = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG2_TIME_TRACK_LICENSES":
                        timeTrackLicenses = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG2_OTHER":
                        meetingNotesOther = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG3_INITIAL_SETUP_FEE":
                        initialeFormsSetupFee = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG3_EFORMS_TEMPLATE_CONFIG":
                        eformsTemplateConfig = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG3_ANALYSING_DAY":
                        eFormsAnalysingDay = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG3_TRAINING_DAYS":
                        eFormTrainingDays = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG3_TIME_TRACK":
                        requirementsTimeTrack = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG3_CLIENT_DATA_TRANSFER":
                        clientDataTransfer = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG3_INITIAL_CLOUD_SETUP_FEE":
                        initialCloudSetupFee = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG3_CLOUD_ANALYSING_DAY":
                        cloudAnalysingDay = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG3_CLOUND_TRAINING_DAYS":
                        cloudTrainingDays = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG3_OTHER":
                        cloudOther = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG3_ROW01_DESC":
                     case "FIELD_PG3_ROW02_DESC":
                     case "FIELD_PG3_ROW03_DESC":
                     case "FIELD_PG3_ROW04_DESC":
                     case "FIELD_PG3_ROW05_DESC":
                     case "FIELD_PG3_ROW06_DESC":
                     case "FIELD_PG3_ROW07_DESC":
                     case "FIELD_PG3_ROW08_DESC":
                     case "FIELD_PG3_ROW09_DESC":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        if (index > 0)
                        {
                           recurringFees[index - 1] = s4bFormControl.fieldValue;
                        }
                        break;

                     case "FIELD_PG3_ROW10_TO_DO":
                     case "FIELD_PG3_ROW11_TO_DO":
                     case "FIELD_PG3_ROW12_TO_DO":
                     case "FIELD_PG3_ROW13_TO_DO":
                     case "FIELD_PG3_ROW14_TO_DO":
                     case "FIELD_PG3_ROW15_TO_DO":
                     case "FIELD_PG3_ROW16_TO_DO":
                     case "FIELD_PG3_ROW17_TO_DO":
                     case "FIELD_PG3_ROW18_TO_DO":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        if (index > 0)
                        {
                           nextStepsTodo[index - 10] = s4bFormControl.fieldValue;
                        }
                        break;

                     case "FIELD_PG3_ROW10_NOTES":
                     case "FIELD_PG3_ROW11_NOTES":
                     case "FIELD_PG3_ROW12_NOTES":
                     case "FIELD_PG3_ROW13_NOTES":
                     case "FIELD_PG3_ROW14_NOTES":
                     case "FIELD_PG3_ROW15_NOTES":
                     case "FIELD_PG3_ROW16_NOTES":
                     case "FIELD_PG3_ROW17_NOTES":
                     case "FIELD_PG3_ROW18_NOTES":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        if (index > 0)
                        {
                           nextStepsNotes[index - 10] = s4bFormControl.fieldValue;
                        }
                        break;

                     case "FIELD_PG3_ROW10_DONE":
                     case "FIELD_PG3_ROW11_DONE":
                     case "FIELD_PG3_ROW12_DONE":
                     case "FIELD_PG3_ROW13_DONE":
                     case "FIELD_PG3_ROW14_DONE":
                     case "FIELD_PG3_ROW15_DONE":
                     case "FIELD_PG3_ROW16_DONE":
                     case "FIELD_PG3_ROW17_DONE":
                     case "FIELD_PG3_ROW18_DONE":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        if (index > 0)
                        {
                           try
                           {
                              bool value = Convert.ToBoolean(s4bFormControl.fieldValue);
                              nextStepsDone[index - 10] = value;
                           }
                           catch (Exception ex) { }
                        }
                        break;

                     case "FIELD_PG4_PAYMENT_TERMS":
                        paymentTerms = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG4_PAYMENT_DUE_DATE":
                        paymentDueDate = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG4_SERVICE_START_DATE":
                        serviceStartDate = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG4_ITEM_DESCRIPTION":
                        itemDescription = s4bFormControl.fieldValue;
                        break;
                     case "FIELD_PG4_SUBMIT":
                        submit = s4bFormControl.fieldValue;
                        break;
                  }
               }
            }
            /// Data Insert for Job Internal Notes
            /// 
            string formattedNotes = "SIMPLICITY FOR BUSINESS SALES MEETING\r\n\r\n";
            formattedNotes = formattedNotes + "Submission Time Stamp: " + ((DateTime)timeStamp).ToString("dd/MM/yyyy HH:mm") + "\r\n\r\n";
            formattedNotes = formattedNotes + "Customer Information\r\n";
            if (!string.IsNullOrEmpty(clientName))
            {
               formattedNotes = formattedNotes + "Customer Name: " + clientName + "\r\n";
            }
            if (!string.IsNullOrEmpty(clientRef))
            {
               formattedNotes = formattedNotes + "Simplicity Customer Ref: " + clientRef + "\r\n";
            }
            if (!string.IsNullOrEmpty(customerAddress))
            {
               formattedNotes = formattedNotes + "Customer Address: " + customerAddress + "\r\n";
            }
            if (!string.IsNullOrEmpty(contactName))
            {
               formattedNotes = formattedNotes + "Contact Name: " + contactName + "\r\n";
            }
            if (!string.IsNullOrEmpty(contactTel))
            {
               formattedNotes = formattedNotes + "Contact Tel: " + contactTel + "\r\n";
            }
            if (!string.IsNullOrEmpty(email))
            {
               formattedNotes = formattedNotes + "Contact Email: " + email + "\r\n";
            }
            if (!string.IsNullOrEmpty(quotationDate))
            {
               formattedNotes = formattedNotes + "Quotation Date: " + quotationDate + "\r\n";
            }
            if (!string.IsNullOrEmpty(customerPORef))
            {
               formattedNotes = formattedNotes + "Customer PO Ref: " + customerPORef + "\r\n";
            }
            if (!string.IsNullOrEmpty(userName))
            {
               formattedNotes = formattedNotes + "Simplicity Representative: " + userName + "\r\n";
            }
            formattedNotes = formattedNotes + "\r\nAttendees From\r\n";
            for (int counter = 0; counter < TOTAL_NO_ROWS_ATTENDEESFROM; counter++)
            {
               if (!string.IsNullOrEmpty(attendeesFromName[counter]) || !string.IsNullOrEmpty(attendeesFromDepartment[counter]) || !string.IsNullOrEmpty(attendeesFromJobRole[counter]))
               {
                  formattedNotes = formattedNotes + "Name: " + attendeesFromName[counter] + " - Job Role: " + attendeesFromJobRole[counter] + " - Department: " + attendeesFromDepartment[counter] + "\r\n";
               }
            }
            formattedNotes = formattedNotes + "\r\nSimplicity Representative(s)\r\n";
            for (int counter = 0; counter < TOTAL_NO_ROWS_REPRESENTATIVES; counter++)
            {
               if (!string.IsNullOrEmpty(representative1[counter]))
               {
                  formattedNotes = formattedNotes + "Name: " + representative1[counter] + "\r\n";
               }
               if (!string.IsNullOrEmpty(representative2[counter]))
               {
                  formattedNotes = formattedNotes + "Name: " + representative2[counter] + "\r\n";
               }
               if (!string.IsNullOrEmpty(representative3[counter]))
               {
                  formattedNotes = formattedNotes + "Name: " + representative3[counter] + "\r\n";
               }
               if (!string.IsNullOrEmpty(representative4[counter]))
               {
                  formattedNotes = formattedNotes + "Name: " + representative4[counter] + "\r\n";
               }
            }
            formattedNotes = formattedNotes + "\r\nPurpose of Meeting\r\n";
            for (int counter = 0; counter < TOTAL_NO_ROWS_PURPOSEOFMEETING; counter++)
            {
               if (!string.IsNullOrEmpty(purposeOfMeeting[counter]))
               {
                  formattedNotes = formattedNotes + purposeOfMeeting[counter] + "\r\n";
               }
               if (!string.IsNullOrEmpty(purposeOfMeeting2[counter]))
               {
                  formattedNotes = formattedNotes + purposeOfMeeting2[counter] + "\r\n";
               }
            }
            formattedNotes = formattedNotes + "\r\nMinutes/Notes\r\n";
            if (!string.IsNullOrEmpty(meetingNotes))
            {
               formattedNotes = formattedNotes + meetingNotes + "\r\n";
            }
            formattedNotes = formattedNotes + "\r\nMeeting Notes For Sales Meeting\r\n";
            if (!string.IsNullOrEmpty(companyDetails))
            {
               formattedNotes = formattedNotes + "Company Details:" + companyDetails + "\r\n";
            }
            if (!string.IsNullOrEmpty(companyClients))
            {
               formattedNotes = formattedNotes + "Company Clients:" + companyClients + "\r\n";
            }
            if (!string.IsNullOrEmpty(typeOfBusiness))
            {
               formattedNotes = formattedNotes + "Type of Business:" + typeOfBusiness + "\r\n";
            }
            if (!string.IsNullOrEmpty(incumbentSupplier))
            {
               formattedNotes = formattedNotes + "Incumbent Supplier:" + incumbentSupplier + "\r\n";
            }
            if (!string.IsNullOrEmpty(companySystems))
            {
               formattedNotes = formattedNotes + "Company Systems:" + companySystems + "\r\n";
            }
            if (!string.IsNullOrEmpty(issues))
            {
               formattedNotes = formattedNotes + "Issues:" + issues + "\r\n";
            }
            if (!string.IsNullOrEmpty(sectionName))
            {
               formattedNotes = formattedNotes + "Section Name:" + sectionName + "\r\n";
            }
            if (!string.IsNullOrEmpty(companyClients))
            {
               formattedNotes = formattedNotes + "No of Users:" + noOfUsers + "\r\n";
            }
            if (!string.IsNullOrEmpty(eformsLicenses))
            {
               formattedNotes = formattedNotes + "eForms Licenses:" + eformsLicenses + "\r\n";
            }
            if (!string.IsNullOrEmpty(cloudLicenses))
            {
               formattedNotes = formattedNotes + "Cloud Licenses:" + cloudLicenses + "\r\n";
            }
            if (!string.IsNullOrEmpty(timeTrackLicenses))
            {
               formattedNotes = formattedNotes + "Time-Track Licenses:" + timeTrackLicenses + "\r\n";
            }
            if (!string.IsNullOrEmpty(meetingNotesOther))
            {
               formattedNotes = formattedNotes + "Other:" + meetingNotesOther + "\r\n";
            }
            formattedNotes = formattedNotes + "\r\nRequirement Information\r\n";
            formattedNotes = formattedNotes + "Simplicity eForms\r\n";
            if (!string.IsNullOrEmpty(initialeFormsSetupFee))
            {
               formattedNotes = formattedNotes + "Initial Setup Fee:" + initialeFormsSetupFee + "\r\n";
            }
            if (!string.IsNullOrEmpty(eformsTemplateConfig))
            {
               formattedNotes = formattedNotes + "eForms Form Template Configuration:" + eformsTemplateConfig + "\r\n";
            }
            if (!string.IsNullOrEmpty(eFormsAnalysingDay))
            {
               formattedNotes = formattedNotes + "Analysing Day:" + eFormsAnalysingDay + "\r\n";
            }
            if (!string.IsNullOrEmpty(eFormTrainingDays))
            {
               formattedNotes = formattedNotes + "Training Day(s):" + eFormTrainingDays + "\r\n";
            }
            formattedNotes = formattedNotes + "Simplicity Time-Track\r\n";
            if (!string.IsNullOrEmpty(requirementsTimeTrack))
            {
               formattedNotes = formattedNotes + "Simplicity Time-Track:" + requirementsTimeTrack + "\r\n";
            }
            formattedNotes = formattedNotes + "Simplicity Cloud\r\n";
            if (!string.IsNullOrEmpty(clientDataTransfer))
            {
               formattedNotes = formattedNotes + "Client Date Transfer:" + clientDataTransfer + "\r\n";
            }
            if (!string.IsNullOrEmpty(initialCloudSetupFee))
            {
               formattedNotes = formattedNotes + "Initial Setup Fee:" + initialCloudSetupFee + "\r\n";
            }
            if (!string.IsNullOrEmpty(cloudAnalysingDay))
            {
               formattedNotes = formattedNotes + "Analysing Day:" + cloudAnalysingDay + "\r\n";
            }
            if (!string.IsNullOrEmpty(cloudTrainingDays))
            {
               formattedNotes = formattedNotes + "Training Day(s):" + cloudTrainingDays + "\r\n";
            }
            if (!string.IsNullOrEmpty(cloudOther))
            {
               formattedNotes = formattedNotes + "Other:" + cloudOther + "\r\n";
            }
            formattedNotes = formattedNotes + "\r\nRe-Occurring Fees\r\n";
            for (int counter = 0; counter < TOTAL_NO_ROWS_RECURRINGFEES; counter++)
            {
               if (!string.IsNullOrEmpty(recurringFees[counter]))
               {
                  formattedNotes = formattedNotes + GetRecurringFeesForUNCClientSalesMeetingTemplate(counter) + recurringFees[counter] + "\r\n";
               }
            }
            formattedNotes = formattedNotes + "\r\nSimplicity Next Steps\r\n";
            for (int counter = 0; counter < TOTAL_NO_ROWS_NEXTSTEPS; counter++)
            {
               if (!string.IsNullOrEmpty(nextStepsNotes[counter]) || !string.IsNullOrEmpty(nextStepsTodo[counter]) || nextStepsDone[counter])
               {
                  formattedNotes = formattedNotes + GetNextStepsDescForUNCClientSalesMeetingTemplate(counter) + "To Do: " + nextStepsTodo[counter] + " - Notes: " + nextStepsNotes[counter] + " - Done: " + (nextStepsDone[counter] ? "Yes" : "No") + "\r\n";
               }
            }
            formattedNotes = formattedNotes + "\r\nCONTRACT TERMS\r\n";
            if (!string.IsNullOrEmpty(paymentTerms))
            {
               formattedNotes = formattedNotes + "Payment Terms: " + paymentTerms + "\r\n";
            }
            if (!string.IsNullOrEmpty(paymentDueDate))
            {
               formattedNotes = formattedNotes + "Initial Payment Due Date: " + paymentDueDate + "\r\n";
            }
            if (!string.IsNullOrEmpty(serviceStartDate))
            {
               formattedNotes = formattedNotes + "Service Start Effective Date: " + serviceStartDate + "\r\n";
            }
            formattedNotes = formattedNotes + "\r\nSpecial Instructions or Extra Cost\r\n";
            if (!string.IsNullOrEmpty(itemDescription))
            {
               formattedNotes = formattedNotes + itemDescription + "\r\n";
            }
            formattedNotes = formattedNotes + "\r\nSubmit:" + submit;
            OrdersNotesRepository ordersNotesRepository = new OrdersNotesRepository();
            OrdersNotes ordersNote = new OrdersNotes();
            ordersNote.JobSequence = jobSequence;
            ordersNote.OrderNotes = Utilities.replaceSpecialCharsForInsert(formattedNotes);
            ordersNote.CreatedBy = userId;
            ordersNote.DateCreated = timeStamp;
            ordersNote.LastAmendedBy = userId;
            ordersNote.DateLastAmended = timeStamp;
            ordersNote = ordersNotesRepository.Insert(request, ordersNote);
            if (ordersNote == null)
            {
               //Utilities.ReportError("Unable to Add Order Internal Notes for 'Woodvale's Quality Control'" + Utilities.Message, method_name, true, system, edwFormInstance);
               //TODO: Report Error
            }

            //log.Info("Finished Processing Data for Woodvale Maintenance QC Report with Template Id '" + edwFormInstance.TemplateId + "' + and Imp Ref" + edwFormInstance.ImpRef);
            //TODO: Log Info
            returnValue = true;
         }
         catch (Exception ex)
         {
            Message = "Unable to Process UNC Client Sales Meeting Template. Exception: " + ex.Message;
         }
         return returnValue;
      }

      // Five Environmental - Template Job Ticket V01 - 898704720
      private bool ProcessFiveEnvJobTicket(HttpRequest request, S4BFormRequest s4bFormRequest, S4bFormSubmissions s4bFormSubmission,
                                           RootObject root, Dictionary<string, S4BFormsControl> s4bFormsControls)
      {
         bool returnValue = false;
         DateTime? datRegistered = DateTime.MinValue;
         int userId = Int32.Parse(request.Headers["UserId"].ToString());
         int jobAddressId = -1, jobClientId = -1, appSequence = -1;
         long jobSequence = -1;

         const int TOTAL_NO_ROWS_FURTHERWORK = 6;
         const int TOTAL_NO_ROWS_PARTSLIST = 5;
         const int TOTAL_NO_ROWS_PLANT_HIRE = 2;
         const int TOTAL_NO_ROWS_ENGINEERS_COMPLETED = 5;

         string userName = "";
         string descOfWorksCompleted = "";
         string contactTel = "", customerAddress = "";
         string[] furtherWorks = new string[TOTAL_NO_ROWS_FURTHERWORK];
         double[] furtherWorksHours = new double[TOTAL_NO_ROWS_FURTHERWORK];
         string[] partsListStatus = new string[TOTAL_NO_ROWS_PARTSLIST];
         string[] partsListSupplier = new string[TOTAL_NO_ROWS_PARTSLIST];
         string[] partsListCode = new string[TOTAL_NO_ROWS_PARTSLIST];
         string[] partsListDescription = new string[TOTAL_NO_ROWS_PARTSLIST];
         double[] partsListQty = new double[TOTAL_NO_ROWS_PARTSLIST];
         double[] partsListAmount = new double[TOTAL_NO_ROWS_PARTSLIST];
         string[] plantHireCompany = new string[TOTAL_NO_ROWS_PLANT_HIRE];
         string[] plantHireCode = new string[TOTAL_NO_ROWS_PLANT_HIRE];
         string[] plantHireDescription = new string[TOTAL_NO_ROWS_PLANT_HIRE];
         double[] plantHireQty = new double[TOTAL_NO_ROWS_PLANT_HIRE];
         double[] plantHireAmount = new double[TOTAL_NO_ROWS_PLANT_HIRE];
         bool isParkingFee = false, isParkingTicket = false, isCongestionCharge = false;
         string poNo = "", hirePONo = "";
         string[] engineersCompletedDesc = new string[TOTAL_NO_ROWS_ENGINEERS_COMPLETED];
         double[] engineersCompletedHours = new double[TOTAL_NO_ROWS_ENGINEERS_COMPLETED];
         string isSubmit = "";
         DateTime? datUser2 = DateTime.MinValue;
         string furtherWorksForUser2Field = "";
         int visitStatusId = -1;
         int index = -1;
         DateTime? timeStamp = s4bFormSubmission.DateCreated;
         Dictionary<string, string> imageValues = new Dictionary<string, string>();
         try
         {
            foreach (KeyValuePair<string, S4BFormsControl> s4bFormControlEntry in s4bFormsControls)
            {
               S4BFormsControl s4bFormControl = s4bFormControlEntry.Value;
               if (s4bFormControl.fieldValue != null)
               {
                  switch (s4bFormControlEntry.Key)
                  {
                     case "VARIABLE_PG1_JOB_SEQUENCE":
                        if (long.TryParse(s4bFormControl.fieldValue, out jobSequence))
                        {
                           //---get customer address
                           customerAddress = new OrdersRepository(null).GetClientAddressByJobSequence(request, jobSequence);
                        };
                        break;
                     case "VARIABLE_PG1_JOB_CLIENT_ID":
                        if (int.TryParse(s4bFormControl.fieldValue, out jobClientId))
                        {
                        };
                        break;
                     case "VARIABLE_PG1_JOB_ADDRESS_ID":
                        if (int.TryParse(s4bFormControl.fieldValue, out jobAddressId))
                        {
                        };
                        break;
                     case "VARIABLE_PG1_USER_ID":
                        if (int.TryParse(s4bFormControl.fieldValue, out userId))
                        {
                        };
                        break;
                     case "VAR_PG1_DIARY_RESOURCE":
                        userName = s4bFormControl.fieldValue;
                        break;
                     case "VARIABLE_PG1_DIARY_ENTRY_ID":
                        int.TryParse(s4bFormControl.fieldValue, out appSequence);
                        break;
                     case "VARIABLE_PG1_JOB_ADDRESS_V":
                        //customerAddress = s4bFormControl.fieldValue;
                        break;
                     case "VARIABLE_PG1_JOB_OCCUPIER_TEL_HOME":
                        contactTel = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG3_DESCRIPTION_OF_WORKS":
                        descOfWorksCompleted = s4bFormControl.fieldValue;
                        break;
                     case "FIELD_PG3_ROW01_FURTHER_WORKS":
                     case "FIELD_PG3_ROW02_FURTHER_WORKS":
                     case "FIELD_PG3_ROW03_FURTHER_WORKS":
                     case "FIELD_PG3_ROW04_FURTHER_WORKS":
                     case "FIELD_PG3_ROW05_FURTHER_WORKS":
                     case "FIELD_PG3_ROW06_FURTHER_WORKS":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        if (index > 0)
                        {
                           furtherWorks[index - 1] = s4bFormControl.fieldValue;
                        }
                        break;
                     case "FIELD_PG3_ROW01_FURTHER_WORKS_HOURS":
                     case "FIELD_PG3_ROW02_FURTHER_WORKS_HOURS":
                     case "FIELD_PG3_ROW03_FURTHER_WORKS_HOURS":
                     case "FIELD_PG3_ROW04_FURTHER_WORKS_HOURS":
                     case "FIELD_PG3_ROW05_FURTHER_WORKS_HOURS":
                     case "FIELD_PG3_ROW06_FURTHER_WORKS_HOURS":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        if (index > 0)
                        {
                           try
                           {
                              double value = Convert.ToDouble(s4bFormControl.fieldValue);
                              furtherWorksHours[index - 1] = value;
                           }
                           catch (Exception ex) { }
                        }
                        break;

                     case "FIELD_PG4_PO_NO":
                        poNo = s4bFormControl.fieldValue;
                        break;
                     case "FIELD_PG4_ROW01_STATUS":
                     case "FIELD_PG4_ROW02_STATUS":
                     case "FIELD_PG4_ROW03_STATUS":
                     case "FIELD_PG4_ROW04_STATUS":
                     case "FIELD_PG4_ROW05_STATUS":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        if (index > 0)
                        {
                           partsListStatus[index - 1] = s4bFormControl.fieldValue;
                        }
                        break;

                     case "FIELD_PG4_ROW01_CODE":
                     case "FIELD_PG4_ROW02_CODE":
                     case "FIELD_PG4_ROW03_CODE":
                     case "FIELD_PG4_ROW04_CODE":
                     case "FIELD_PG4_ROW05_CODE":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        if (index > 0)
                        {
                           partsListStatus[index - 1] = s4bFormControl.fieldValue;
                        }
                        break;

                     case "FIELD_PG4_ROW01_DESC":
                     case "FIELD_PG4_ROW02_DESC":
                     case "FIELD_PG4_ROW03_DESC":
                     case "FIELD_PG4_ROW04_DESC":
                     case "FIELD_PG4_ROW05_DESC":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        if (index > 0)
                        {
                           partsListDescription[index - 1] = s4bFormControl.fieldValue;
                        }
                        break;

                     case "FIELD_PG4_ROW01_SUPPLIER":
                     case "FIELD_PG4_ROW02_SUPPLIER":
                     case "FIELD_PG4_ROW03_SUPPLIER":
                     case "FIELD_PG4_ROW04_SUPPLIER":
                     case "FIELD_PG4_ROW05_SUPPLIER":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        if (index > 0)
                        {
                           partsListSupplier[index - 1] = s4bFormControl.fieldValue;
                        }
                        break;

                     case "FIELD_PG4_ROW01_QTY":
                     case "FIELD_PG4_ROW02_QTY":
                     case "FIELD_PG4_ROW03_QTY":
                     case "FIELD_PG4_ROW04_QTY":
                     case "FIELD_PG4_ROW05_QTY":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        if (index > 0)
                        {
                           try
                           {
                              double value = Convert.ToDouble(s4bFormControl.fieldValue);
                              partsListQty[index - 1] = value;
                           }
                           catch (Exception ex) { }
                        }
                        break;
                     case "FIELD_PG4_ROW01_AMOUNT":
                     case "FIELD_PG4_ROW02_AMOUNT":
                     case "FIELD_PG4_ROW03_AMOUNT":
                     case "FIELD_PG4_ROW04_AMOUNT":
                     case "FIELD_PG4_ROW05_AMOUNT":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        if (index > 0)
                        {
                           try
                           {
                              double value = Convert.ToDouble(s4bFormControl.fieldValue);
                              partsListAmount[index - 1] = value;
                           }
                           catch (Exception ex) { }
                        }
                        break;

                     case "FIELD_PG4_PARKING_TICKET":
                        try
                        {
                           isParkingTicket = Convert.ToBoolean(s4bFormControl.fieldValue);
                        }
                        catch (Exception ex) { }
                        break;
                     case "FIELD_PG4_PARKING":
                        try
                        {
                           isParkingFee = Convert.ToBoolean(s4bFormControl.fieldValue);
                        }
                        catch (Exception ex) { }
                        break;
                     case "CHECKBOX_168":
                        try
                        {
                           isCongestionCharge = Convert.ToBoolean(s4bFormControl.fieldValue);
                        }
                        catch (Exception ex) { }
                        break;

                     case "FIELD_PG4_ROW06_CODE":
                     case "FIELD_PG4_ROW07_CODE":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        if (index > 0)
                        {
                           plantHireCode[index - 6] = s4bFormControl.fieldValue;
                        }
                        break;

                     case "FIELD_PG4_ROW06_DESC":
                     case "FIELD_PG4_ROW07_DESC":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        if (index > 0)
                        {
                           plantHireDescription[index - 6] = s4bFormControl.fieldValue;
                        }
                        break;

                     case "FIELD_PG4_ROW06_HIRE_COMPANY":
                     case "FIELD_PG4_ROW07_HIRE_COMPANY":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        if (index > 0)
                        {
                           plantHireCompany[index - 6] = s4bFormControl.fieldValue;
                        }
                        break;

                     case "FIELD_PG4_ROW06_QTY":
                     case "FIELD_PG4_ROW07_QTY":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        if (index > 0)
                        {
                           try
                           {
                              double value = Convert.ToDouble(s4bFormControl.fieldValue);
                              plantHireQty[index - 6] = value;
                           }
                           catch (Exception ex) { }
                        }
                        break;
                     case "FIELD_PG4_ROW06_AMOUNT":
                     case "FIELD_PG4_ROW07_AMOUNT":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        if (index > 0)
                        {
                           try
                           {
                              double value = Convert.ToDouble(s4bFormControl.fieldValue);
                              plantHireAmount[index - 6] = value;
                           }
                           catch (Exception ex) { }
                        }
                        break;
                     case "FIELD_PG4_HIRE_PO_NO":
                        hirePONo = s4bFormControl.fieldValue;
                        break;

                     case "FIELD_PG5_ROW01_DESC":
                     case "FIELD_PG5_ROW02_DESC":
                     case "FIELD_PG5_ROW03_DESC":
                     case "FIELD_PG5_ROW04_DESC":
                     case "FIELD_PG5_ROW05_DESC":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        if (index > 0)
                        {
                           engineersCompletedDesc[index - 1] = s4bFormControl.fieldValue;
                        }
                        break;
                     case "FIELD_PG5_ROW01_HOURS":
                     case "FIELD_PG5_ROW02_HOURS":
                     case "FIELD_PG5_ROW03_HOURS":
                     case "FIELD_PG5_ROW04_HOURS":
                     case "FIELD_PG5_ROW05_HOURS":
                        index = -1;
                        int.TryParse(s4bFormControlEntry.Key.Substring(13, 2), out index);
                        if (index > 0)
                        {
                           try
                           {
                              double value = Convert.ToDouble(s4bFormControl.fieldValue);
                              furtherWorksHours[index - 1] = value;
                           }
                           catch (Exception ex) { }
                        }
                        break;

                     case "FIELD_PG5_SUBMIT":
                        try
                        {
                           isSubmit = s4bFormControl.fieldValue;
                        }
                        catch (Exception ex) { }
                        break;

                     case "FIELD_PG3_VISIT_STATUS":
                        visitStatusId = Utilities.GetVisitStatusIdByStatusDesc(request, s4bFormControl.fieldValue);
                        break;

                     case "FIELD_PG3_FURTHER_WORKS":
                        furtherWorksForUser2Field = s4bFormControl.fieldValue;
                        break;

                  }
               }
            }
            if (userId <= 0)
            {
               userId = 1;
            }
            DiaryResources diaryResource = Utilities.GetDiaryResourceByUserId(request, userId);
            double vamRate = new DiaryResourcesRepository().GetResourceVAMCostRate(request, diaryResource.Sequence ?? 0);
            /// Data Insert for Page 3 - Job Internal Notes & Schedule Items
            /// 
            string formattedNotes = "Point of Works Risk Assessment (POWRA) Submission" + "\r\n";
            formattedNotes = "Engineer: " + userName + ". Submission Time Stamp: " + ((DateTime)timeStamp).ToString("dd/MM/yyyy HH:mm") + "\r\n\r\n";
            formattedNotes = formattedNotes + "Description of Works Complete and any extra information:\r\n\r\n";
            OrderItemsRepository orderItemsRepository = new OrderItemsRepository();
            if (!string.IsNullOrEmpty(descOfWorksCompleted))
            {
               formattedNotes = formattedNotes + "Engineers Report: " + descOfWorksCompleted + "\r\n";
               string itemDesc = "Engineers Report";
               OrderItems oi = new OrderItems();
               oi.JobSequence = jobSequence;
               oi.FlgRowIsText = true;
               oi.RowIndex = 9999;
               oi.TransType = SimplicityConstants.ClientTransType;
               oi.ItemType = 0;
               oi.ItemDesc = itemDesc;
               oi.ItemUnits = "";
               oi.ItemQuantity = 0;
               oi.AmountLabour = 0;
               oi.AmountMaterials = 0;
               oi.AmountPlant = 0;
               oi.AmountValue = 0;
               oi.AmountTotal = 0;
               oi.AssignedTo = -1;
               oi.FlgCompleted = true;
               oi.FlgDocsRecd = true;
               oi.CreatedBy = userId;
               oi.DateCreated = timeStamp;
               oi.LastAmendedBy = userId;
               oi.DateLastAmended = timeStamp;
               OrderItems oiUpdated = orderItemsRepository.CreateOrderItems(oi, request);
               itemDesc = descOfWorksCompleted;
               oi = new OrderItems();
               oi.JobSequence = jobSequence;
               oi.FlgRowIsText = false;
               oi.RowIndex = 9999;
               oi.TransType = SimplicityConstants.ClientTransType;
               oi.ItemType = 0;
               oi.ItemDesc = itemDesc;
               oi.ItemUnits = "NR";
               oi.ItemQuantity = 1;
               oi.AmountLabour = 0;
               oi.AmountMaterials = 0;
               oi.AmountPlant = 0;
               oi.AmountValue = 0;
               oi.AmountTotal = 0;
               oi.AssignedTo = -1;
               oi.FlgCompleted = true;
               oi.FlgDocsRecd = true;
               oi.CreatedBy = userId;
               oi.DateCreated = timeStamp;
               oi.LastAmendedBy = userId;
               oi.DateLastAmended = timeStamp;
               oiUpdated = orderItemsRepository.CreateOrderItems(oi, request);
            }
            bool isFurtherWorks = false;
            formattedNotes = formattedNotes + "\r\nRECOMMENDATION FOR FURTHER WORK:\r\n";
            for (int counter = 0; counter < TOTAL_NO_ROWS_FURTHERWORK; counter++)
            {
               if (!string.IsNullOrEmpty(furtherWorks[counter]) || !string.IsNullOrEmpty(furtherWorks[counter]) || furtherWorksHours[counter] > 0)
               {
                  isFurtherWorks = true;
                  formattedNotes = formattedNotes + furtherWorks[counter] + " - Hours Required: " + furtherWorksHours[counter] + "\r\n";
               }
            }
            if (isFurtherWorks)
            {
               string itemDesc = "RECOMMENDATION FOR FURTHER WORK";
               OrderItems oi = new OrderItems();
               oi.JobSequence = jobSequence;
               oi.FlgRowIsText = true;
               oi.RowIndex = 9999;
               oi.ItemType = 0;
               oi.ItemDesc = itemDesc;
               oi.ItemUnits = "";
               oi.TransType = SimplicityConstants.ClientTransType;
               oi.ItemQuantity = 0;
               oi.AmountLabour = 0;
               oi.AmountMaterials = 0;
               oi.AmountPlant = 0;
               oi.AmountValue = 0;
               oi.AmountTotal = 0;
               oi.AssignedTo = -1;
               oi.FlgCompleted = true;
               oi.FlgDocsRecd = true;
               oi.CreatedBy = userId;
               oi.DateCreated = timeStamp;
               oi.LastAmendedBy = userId;
               oi.DateLastAmended = timeStamp;
               OrderItems oiUpdated = orderItemsRepository.CreateOrderItems(oi, request);
               for (int counter = 0; counter < TOTAL_NO_ROWS_FURTHERWORK; counter++)
               {
                  if (!string.IsNullOrEmpty(furtherWorks[counter]) || !string.IsNullOrEmpty(furtherWorks[counter]) || furtherWorksHours[counter] > 0)
                  {
                     itemDesc = furtherWorks[counter];
                     oi = new OrderItems();
                     oi.JobSequence = jobSequence;
                     oi.FlgRowIsText = false;
                     oi.RowIndex = 9999;
                     oi.TransType = SimplicityConstants.ClientTransType;
                     oi.ItemType = 0;
                     oi.ItemDesc = itemDesc;
                     oi.ItemUnits = "HRS";
                     oi.ItemQuantity = furtherWorksHours[counter];
                     oi.AmountLabour = vamRate;
                     oi.AmountMaterials = 0;
                     oi.AmountPlant = 0;
                     oi.AmountValue = 0;
                     oi.AmountTotal = 0;
                     oi.AssignedTo = -1;
                     oi.FlgCompleted = false;
                     oi.FlgDocsRecd = false;
                     oi.CreatedBy = userId;
                     oi.DateCreated = timeStamp;
                     oi.LastAmendedBy = userId;
                     oi.DateLastAmended = timeStamp;
                     oiUpdated = orderItemsRepository.CreateOrderItems(oi, request);
                  }
               }
            }
            //Page 4 - Parts List
            bool isPartsListRequired = false;
            for (int counter = 0; counter < TOTAL_NO_ROWS_PARTSLIST; counter++)
            {
               if (!string.IsNullOrEmpty(partsListSupplier[counter]) || !string.IsNullOrEmpty(partsListCode[counter]) || !string.IsNullOrEmpty(partsListDescription[counter]) || partsListQty[counter] > 0 || partsListAmount[counter] > 0)
               {
                  isPartsListRequired = true;
               }
            }
            if (isPartsListRequired)
            {
               string itemDesc = "Parts List";
               OrderItems oi = new OrderItems();
               oi.JobSequence = jobSequence;
               oi.FlgRowIsText = true;
               oi.RowIndex = 9999;
               oi.ItemType = 0;
               oi.ItemDesc = itemDesc;
               oi.ItemUnits = "";
               oi.TransType = SimplicityConstants.ClientTransType;
               oi.ItemQuantity = 0;
               oi.AmountLabour = 0;
               oi.AmountMaterials = 0;
               oi.AmountPlant = 0;
               oi.AmountValue = 0;
               oi.AmountTotal = 0;
               oi.AssignedTo = -1;
               oi.FlgCompleted = true;
               oi.FlgDocsRecd = true;
               oi.CreatedBy = userId;
               oi.DateCreated = timeStamp;
               oi.LastAmendedBy = userId;
               oi.DateLastAmended = timeStamp;
               OrderItems oiUpdated = orderItemsRepository.CreateOrderItems(oi, request);
               double partsListPercent = 0.2;
               for (int counter = 0; counter < TOTAL_NO_ROWS_PARTSLIST; counter++)
               {
                  if (!string.IsNullOrEmpty(partsListSupplier[counter]) || !string.IsNullOrEmpty(partsListCode[counter]) || !string.IsNullOrEmpty(partsListDescription[counter]) || partsListQty[counter] > 0 || partsListAmount[counter] > 0)
                  {
                     itemDesc = (string.IsNullOrEmpty(partsListStatus[counter]) ? "" : partsListStatus[counter] + " ") +
                                (string.IsNullOrEmpty(partsListSupplier[counter]) ? "" : partsListSupplier[counter] + " ") +
                                (string.IsNullOrEmpty(partsListDescription[counter]) ? "" : partsListDescription[counter]);
                     itemDesc = itemDesc.Trim();
                     oi = new OrderItems();
                     oi.JobSequence = jobSequence;
                     oi.FlgRowIsText = false;
                     oi.RowIndex = 9999;
                     oi.TransType = SimplicityConstants.ClientTransType;
                     oi.ItemType = 0;
                     oi.ItemCode = partsListCode[counter];
                     oi.ItemDesc = itemDesc;
                     oi.ItemUnits = "EA";
                     oi.ItemQuantity = partsListQty[counter];
                     oi.AmountLabour = 0;
                     oi.AmountMaterials = partsListAmount[counter];
                     oi.AmountPlant = 0;
                     oi.AdjCode = "A020";
                     oi.ChgPcentAdj = partsListPercent;
                     oi.AmountValue = partsListAmount[counter] + (oi.AmountMaterials * oi.ChgPcentAdj);
                     oi.AmountTotal = partsListQty[counter] * oi.AmountValue;
                     oi.AssignedTo = -1;
                     oi.FlgCompleted = false;
                     oi.FlgDocsRecd = false;
                     oi.CreatedBy = userId;
                     oi.DateCreated = timeStamp;
                     oi.LastAmendedBy = userId;
                     oi.DateLastAmended = timeStamp;
                     oiUpdated = orderItemsRepository.CreateOrderItems(oi, request);
                  }
               }
            }
            if (isParkingFee || isParkingTicket || isCongestionCharge)
            {
               string itemDesc = "Expenses";
               OrderItems oi = new OrderItems();
               oi.JobSequence = jobSequence;
               oi.FlgRowIsText = true;
               oi.RowIndex = 9999;
               oi.ItemType = 0;
               oi.ItemDesc = itemDesc;
               oi.ItemUnits = "";
               oi.TransType = SimplicityConstants.ClientTransType;
               oi.ItemQuantity = 0;
               oi.AmountLabour = 0;
               oi.AmountMaterials = 0;
               oi.AmountPlant = 0;
               oi.AmountValue = 0;
               oi.AmountTotal = 0;
               oi.AssignedTo = -1;
               oi.FlgCompleted = true;
               oi.FlgDocsRecd = true;
               oi.CreatedBy = userId;
               oi.DateCreated = timeStamp;
               oi.LastAmendedBy = userId;
               oi.DateLastAmended = timeStamp;
               OrderItems oiUpdated = orderItemsRepository.CreateOrderItems(oi, request);
               if (isCongestionCharge)
               {
                  itemDesc = "Congestion Charges";
                  oi = new OrderItems();
                  oi.JobSequence = jobSequence;
                  oi.FlgRowIsText = false;
                  oi.RowIndex = 9999;
                  oi.TransType = SimplicityConstants.ClientTransType;
                  oi.ItemType = 0;
                  oi.ItemCode = "";
                  oi.ItemDesc = itemDesc;
                  oi.ItemUnits = "EA";
                  oi.ItemQuantity = 1;
                  oi.AmountLabour = 0;
                  oi.AmountMaterials = 0;
                  oi.AmountPlant = 0;
                  oi.AmountValue = 0;
                  oi.AmountTotal = 0;
                  oi.AssignedTo = -1;
                  oi.FlgCompleted = false;
                  oi.FlgDocsRecd = false;
                  oi.CreatedBy = userId;
                  oi.DateCreated = timeStamp;
                  oi.LastAmendedBy = userId;
                  oi.DateLastAmended = timeStamp;
                  oiUpdated = orderItemsRepository.CreateOrderItems(oi, request);
               }
               if (isParkingFee)
               {
                  itemDesc = "Parking";
                  oi = new OrderItems();
                  oi.JobSequence = jobSequence;
                  oi.FlgRowIsText = false;
                  oi.RowIndex = 9999;
                  oi.TransType = SimplicityConstants.ClientTransType;
                  oi.ItemType = 0;
                  oi.ItemCode = "";
                  oi.ItemDesc = itemDesc;
                  oi.ItemUnits = "EA";
                  oi.ItemQuantity = 1;
                  oi.AmountLabour = 0;
                  oi.AmountMaterials = 0;
                  oi.AmountPlant = 0;
                  oi.AmountValue = 0;
                  oi.AmountTotal = 0;
                  oi.AssignedTo = -1;
                  oi.FlgCompleted = false;
                  oi.FlgDocsRecd = false;
                  oi.CreatedBy = userId;
                  oi.DateCreated = timeStamp;
                  oi.LastAmendedBy = userId;
                  oi.DateLastAmended = timeStamp;
                  oiUpdated = orderItemsRepository.CreateOrderItems(oi, request);
               }
               if (isParkingTicket)
               {
                  itemDesc = "Parking Ticket";
                  oi = new OrderItems();
                  oi.JobSequence = jobSequence;
                  oi.FlgRowIsText = false;
                  oi.RowIndex = 9999;
                  oi.TransType = SimplicityConstants.ClientTransType;
                  oi.ItemType = 0;
                  oi.ItemCode = "";
                  oi.ItemDesc = itemDesc;
                  oi.ItemUnits = "EA";
                  oi.ItemQuantity = 1;
                  oi.AmountLabour = 0;
                  oi.AmountMaterials = 0;
                  oi.AmountPlant = 0;
                  oi.AmountValue = 0;
                  oi.AmountTotal = 0;
                  oi.AssignedTo = -1;
                  oi.FlgCompleted = false;
                  oi.FlgDocsRecd = false;
                  oi.CreatedBy = userId;
                  oi.DateCreated = timeStamp;
                  oi.LastAmendedBy = userId;
                  oi.DateLastAmended = timeStamp;
                  oiUpdated = orderItemsRepository.CreateOrderItems(oi, request);
               }
            }
            //Page 4 - Plant Hire
            //Page 4 - Parts List
            bool isPlantHireRequired = false;
            for (int counter = 0; counter < TOTAL_NO_ROWS_PLANT_HIRE; counter++)
            {
               if (!string.IsNullOrEmpty(plantHireCompany[counter]) || !string.IsNullOrEmpty(plantHireCode[counter]) || !string.IsNullOrEmpty(plantHireDescription[counter]) || plantHireQty[counter] > 0 || plantHireAmount[counter] > 0)
               {
                  isPartsListRequired = true;
               }
            }
            if (isPlantHireRequired)
            {
               string itemDesc = "Plant Hire";
               OrderItems oi = new OrderItems();
               oi.JobSequence = jobSequence;
               oi.FlgRowIsText = true;
               oi.RowIndex = 9999;
               oi.ItemType = 0;
               oi.ItemDesc = itemDesc;
               oi.ItemUnits = "";
               oi.TransType = SimplicityConstants.ClientTransType;
               oi.ItemQuantity = 0;
               oi.AmountLabour = 0;
               oi.AmountMaterials = 0;
               oi.AmountPlant = 0;
               oi.AmountValue = 0;
               oi.AmountTotal = 0;
               oi.AssignedTo = -1;
               oi.FlgCompleted = true;
               oi.FlgDocsRecd = true;
               oi.CreatedBy = userId;
               oi.DateCreated = timeStamp;
               oi.LastAmendedBy = userId;
               oi.DateLastAmended = timeStamp;
               OrderItems oiUpdated = orderItemsRepository.CreateOrderItems(oi, request);
               for (int counter = 0; counter < TOTAL_NO_ROWS_PLANT_HIRE; counter++)
               {
                  if (!string.IsNullOrEmpty(plantHireCompany[counter]) || !string.IsNullOrEmpty(plantHireCode[counter]) || !string.IsNullOrEmpty(plantHireDescription[counter]) || plantHireQty[counter] > 0 || plantHireAmount[counter] > 0)
                  {
                     itemDesc = (string.IsNullOrEmpty(plantHireCompany[counter]) ? "" : plantHireCompany[counter] + " ") +
                                (string.IsNullOrEmpty(plantHireDescription[counter]) ? "" : plantHireDescription[counter]);
                     itemDesc = itemDesc.Trim();
                     oi = new OrderItems();
                     oi.JobSequence = jobSequence;
                     oi.FlgRowIsText = false;
                     oi.RowIndex = 9999;
                     oi.TransType = SimplicityConstants.ClientTransType;
                     oi.ItemType = 0;
                     oi.ItemCode = plantHireCode[counter];
                     oi.ItemDesc = itemDesc;
                     oi.ItemUnits = "EA";
                     oi.ItemQuantity = plantHireQty[counter];
                     oi.AmountLabour = 0;
                     oi.AmountMaterials = plantHireAmount[counter];
                     oi.AmountPlant = 0;
                     oi.AmountValue = plantHireAmount[counter];
                     oi.AmountTotal = plantHireQty[counter] * oi.AmountValue;
                     oi.AssignedTo = -1;
                     oi.FlgCompleted = false;
                     oi.FlgDocsRecd = false;
                     oi.CreatedBy = userId;
                     oi.DateCreated = timeStamp;
                     oi.LastAmendedBy = userId;
                     oi.DateLastAmended = timeStamp;
                     oiUpdated = orderItemsRepository.CreateOrderItems(oi, request);
                  }
               }
            }
            //Page 5 - Engineers Work Complete
            bool isEngineersComplete = false;
            formattedNotes = formattedNotes + "\r\nEngineers Completed Report:\r\n";
            for (int counter = 0; counter < TOTAL_NO_ROWS_ENGINEERS_COMPLETED; counter++)
            {
               if (!string.IsNullOrEmpty(engineersCompletedDesc[counter]) || engineersCompletedHours[counter] > 0)
               {
                  isEngineersComplete = true;
                  formattedNotes = formattedNotes + engineersCompletedDesc[counter] + " - Hours: " + engineersCompletedHours[counter] + "\r\n";
               }
            }
            if (isEngineersComplete)
            {
               string itemDesc = "Engineers Completed Report";
               OrderItems oi = new OrderItems();
               oi.JobSequence = jobSequence;
               oi.FlgRowIsText = true;
               oi.RowIndex = 9999;
               oi.ItemType = 0;
               oi.ItemDesc = itemDesc;
               oi.ItemUnits = "";
               oi.TransType = SimplicityConstants.ClientTransType;
               oi.ItemQuantity = 0;
               oi.AmountLabour = 0;
               oi.AmountMaterials = 0;
               oi.AmountPlant = 0;
               oi.AmountValue = 0;
               oi.AmountTotal = 0;
               oi.AssignedTo = -1;
               oi.FlgCompleted = true;
               oi.FlgDocsRecd = true;
               oi.CreatedBy = userId;
               oi.DateCreated = timeStamp;
               oi.LastAmendedBy = userId;
               oi.DateLastAmended = timeStamp;
               OrderItems oiUpdated = orderItemsRepository.CreateOrderItems(oi, request);
               for (int counter = 0; counter < TOTAL_NO_ROWS_ENGINEERS_COMPLETED; counter++)
               {
                  if (!string.IsNullOrEmpty(engineersCompletedDesc[counter]) || engineersCompletedHours[counter] > 0)
                  {
                     itemDesc = engineersCompletedDesc[counter];
                     oi = new OrderItems();
                     oi.JobSequence = jobSequence;
                     oi.FlgRowIsText = false;
                     oi.RowIndex = 9999;
                     oi.TransType = SimplicityConstants.ClientTransType;
                     oi.ItemType = 0;
                     oi.ItemDesc = itemDesc;
                     oi.ItemUnits = "HRS";
                     oi.ItemQuantity = engineersCompletedHours[counter];
                     oi.AmountLabour = vamRate;
                     oi.AmountMaterials = 0;
                     oi.AmountPlant = 0;
                     oi.AmountValue = 0;
                     oi.AmountTotal = 0;
                     oi.AssignedTo = -1;
                     oi.FlgCompleted = false;
                     oi.FlgDocsRecd = false;
                     oi.CreatedBy = userId;
                     oi.DateCreated = timeStamp;
                     oi.LastAmendedBy = userId;
                     oi.DateLastAmended = timeStamp;
                     oiUpdated = orderItemsRepository.CreateOrderItems(oi, request);
                  }
               }
            }
            OrdersNotesRepository ordersNotesRepository = new OrdersNotesRepository();
            OrdersNotes ordersNote = new OrdersNotes();
            ordersNote.JobSequence = jobSequence;
            ordersNote.OrderNotes = Utilities.replaceSpecialCharsForInsert(formattedNotes);
            ordersNote.CreatedBy = userId;
            ordersNote.DateCreated = timeStamp;
            ordersNote.LastAmendedBy = userId;
            ordersNote.DateLastAmended = timeStamp;
            ordersNote = ordersNotesRepository.Insert(request, ordersNote);
            if (ordersNote == null)
            {
               //Utilities.ReportError("Unable to Add Order Internal Notes for 'Woodvale's Quality Control'" + Utilities.Message, method_name, true, system, edwFormInstance);
               //TODO: Report Error
            }
            OrdersRepository ordersRepository = new OrdersRepository( null);
            int jobStatusId = -1;
            if (isSubmit.Trim().Equals("Completed", StringComparison.InvariantCultureIgnoreCase))
            {
               jobStatusId = 2;
               OrdersBillsRepository ordersBillsRepository = new OrdersBillsRepository();
               if (!ordersBillsRepository.CreateApplicationForPayment(request, jobSequence, customerAddress, timeStamp))
               {
               }
               ordersRepository.updateFlgJobFinishAndJobDateFinishByJobSequence(request, jobSequence, true, DateTime.Now);
               if (!ordersRepository.UpdateFlgJobSLATimerStopAndDateJobSLATimerStopByJobSequence(request, jobSequence, true, DateTime.Now))
               {
               }
               new DiaryAppsRepository().updateVisitStatusAndFlgCompletedBySequence(request, appSequence, -1, true, userId, DateTime.Now);
               if (!ordersRepository.updateFlgJobCompletedByJobSequence(request, jobSequence, true))
               {
               }
            }
            else if (isSubmit.Trim().Equals("Awaiting Parts", StringComparison.InvariantCultureIgnoreCase))
            {
               jobStatusId = 7;
            }
            else if (isSubmit.Trim().Equals("Job Not Completed", StringComparison.InvariantCultureIgnoreCase))
            {
               jobStatusId = 8;
            }
            if (jobStatusId > 0)
            {
               ordersRepository.boolUpdateOrderStatusSucceeded(request, true, jobSequence, jobStatusId, DateTime.Now, isSubmit, userId, DateTime.Now);
            }
            if (furtherWorksForUser2Field.Equals("Yes", StringComparison.InvariantCultureIgnoreCase))
            {
               if (!ordersRepository.UpdateUserFlag2AndUserDate2ByJobSequence(request, jobSequence, true, DateTime.Now))
               {
               }
            }
            //log.Info("Finished Processing Data for Woodvale Maintenance QC Report with Template Id '" + edwFormInstance.TemplateId + "' + and Imp Ref" + edwFormInstance.ImpRef);
            //TODO: Log Info
            returnValue = true;
         }
         catch (Exception ex)
         {
            Message = "Unable to Process UNC Client Sales Meeting Template. Exception: " + ex.Message;
         }
         return returnValue;
      }

      private string GetRecurringFeesForUNCClientSalesMeetingTemplate(int counter)
      {
         string returnValue = "";
         switch (counter + 1)
         {
            case 1:
               returnValue = "Simplicity eForms User Licenses";
               break;
            case 2:
               returnValue = "eForms Annual Support & Maintenance Contract";
               break;
            case 3:
               returnValue = "Simplicity Cloud User License`s";
               break;
            case 4:
               returnValue = "Cloud Annual Support & Maintenance Contract";
               break;
            case 5:
               returnValue = "Remote Access – Hosting Date Files";
               break;
            case 6:
               returnValue = "Simplicity Time-Track";
               break;
            case 7:
               returnValue = "Time-Track Annual Support & Maintenance Contract";
               break;
            case 8:
               returnValue = "Misc.";
               break;
            case 9:
               returnValue = "Misc.";
               break;
         }
         return returnValue + ": ";
      }

      private string GetNextStepsDescForUNCClientSalesMeetingTemplate(int counter)
      {
         string returnValue = "";
         switch (counter + 1)
         {
            case 1:
               returnValue = "Send a quote";
               break;
            case 2:
               returnValue = "Call back required";
               break;
            case 3:
               returnValue = "Meet the decision makers";
               break;
            case 4:
               returnValue = "Meet with Accountants or Outside Team";
               break;
            case 5:
               returnValue = "Book an Analysing Day";
               break;
            case 6:
               returnValue = "Book Site Visit";
               break;
            case 7:
               returnValue = "Send a Contract";
               break;
            case 8:
               returnValue = "Book a Contract signing meeting";
               break;
            case 9:
               returnValue = "Set up a Free Trial";
               break;
         }
         return returnValue + ": ";
      }



      public ResponseModel GetFilesAndUpdate(HttpRequest request, string filePath)
      {
         ResponseModel returnValue = new ResponseModel();
         ProjectSettings settings = Configs.settings[request.Headers["ProjectId"]];
         string logFilePath = "";
         try
         {
            //---create a Log file
            String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            logFilePath = Path.Combine(filePath, "MyLog " + timeStamp + ".txt");
            if (File.Exists(logFilePath))
            {
               File.Delete(logFilePath);
            }
            TextWriter swLog = new StreamWriter(logFilePath, true);
            // Create a file to write to.
            using (swLog)
            {
               swLog.WriteLine("******************************************");
               swLog.WriteLine("Date" + DateTime.Now);
               swLog.WriteLine("******************************************");
            }
            string s4bSubmitNo = "";

            foreach (string item in Directory.GetDirectories(filePath))
            {
               FileInfo folder = new FileInfo(item);
               Console.WriteLine(folder.Name);
               s4bSubmitNo = folder.Name;
               this.writeTxt(logFilePath, "Processing Submit no : " + s4bSubmitNo);
               S4bFormSubmissionsDB SubObject = new S4bFormSubmissionsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
               S4bFormSubmissions s4bFormSubmission = SubObject.getFormSubmissionBySubmitNo(s4bSubmitNo);
               this.writeTxt(logFilePath, "Submission Job Sequence:" + s4bFormSubmission.JobSequence);
               //---Read Zip file Name from folder
               this.writeTxt(logFilePath, "Read Zip File");
               string folderPath = filePath + "\\" + folder.Name;
               string[] allfiles = Directory.GetFiles(folderPath, "*.zip");
               string[] allPDFFiles = Directory.GetFiles(folderPath, "*.pdf");
               foreach (var fileItem in allfiles)
               {
                  FileInfo file = new FileInfo(fileItem);
                  Console.WriteLine(file.Name);
                  //---Upload Zip File on Google drive
                  string newZipFileName = file.Name; // Utilities.GenerateS4BeFormZipFileName(s4bFormSubmission, 1);
                  this.writeTxt(logFilePath, "Find Zip file:" + newZipFileName);
                  //string[] excludeFiles = { SimplicityConstants.S4BFormSubmittedTemplateName, SimplicityConstants.S4BFormJsonFileName };
                  string zipFileExtractedPath = folderPath;

                  s4bFormSubmission.ZipFilePath = Path.Combine(zipFileExtractedPath, newZipFileName); //Utilities.zipFiles(zipFileExtractedPath, zipFileExtractedPath, newZipFileName, excludeFiles,
                                                                                                      //Path.Combine(settings.TempUploadFolderPath, request.Headers["ProjectId"], s4bFormSubmission.S4bSubmitNo));
                  this.writeTxt(logFilePath, "Zip file path:" + s4bFormSubmission.ZipFilePath);
                  this.writeTxt(logFilePath, "Uploading Zip file");
                  string zipFileCabId = UploadToDrive(request, s4bFormSubmission.ZipFilePath, s4bFormSubmission.Orders.JobRef + "," + settings.FilingCabinetS4BFormsFolder, newZipFileName);
                  this.writeTxt(logFilePath, "Zip File Uploaded:" + zipFileCabId);
                  //---Upload PDf File on Google Drive

                  /*    string pdfFileName = Utilities.GenerateS4BeFormPdfFileName(s4bFormSubmission, 1);
                      this.writeTxt(logFilePath, "Uploading PDF file : " + pdfFileName);
                      s4bFormSubmission.PdfFilePath = Path.Combine(zipFileExtractedPath, SimplicityConstants.S4BFormSubmittedTemplateName);
                      s4bFormSubmission.FileCabId = UploadToDrive(request, s4bFormSubmission.PdfFilePath, s4bFormSubmission.Orders.JobRef + "," + settings.FilingCabinetS4BFormsFolder, pdfFileName);
                      this.writeTxt(logFilePath, "PDF file Cabinet Id:" + s4bFormSubmission.FileCabId);
                      //---Update FileCabinet Id in Database
                      s4bFormSubmission.LastAmendedBy = long.Parse(request.Headers["UserId"]);
                      s4bFormSubmission.DateLastAmended = DateTime.Now;
                      this.writeTxt(logFilePath, "Update Record in Submission");
                      if (!new S4bFormSubmissionRepository().UpdateFormSubmission(s4bFormSubmission, request))
                      {
                          Utilities.GenerateAndLogMessage("SubmissionAndUpdate", "Unable to Update Submission.", null);
                          this.writeTxt(logFilePath, "Error in updating Submission");
                          returnValue.IsSucessfull = false;
                      } */
               }
               foreach (var fileItem in allPDFFiles)
               {
                  FileInfo file = new FileInfo(fileItem);
                  Console.WriteLine(file.Name);
                  string pdfFileName = file.Name;
                  this.writeTxt(logFilePath, "Uploading PDF file : " + pdfFileName);
                  s4bFormSubmission.PdfFilePath = Path.Combine(folderPath, pdfFileName);
                  s4bFormSubmission.FileCabId = UploadToDrive(request, s4bFormSubmission.PdfFilePath, s4bFormSubmission.Orders.JobRef + "," + settings.FilingCabinetS4BFormsFolder, pdfFileName);
                  this.writeTxt(logFilePath, "PDF file Cabinet Id:" + s4bFormSubmission.FileCabId);
                  //---Update FileCabinet Id in Database
                  s4bFormSubmission.LastAmendedBy = long.Parse(request.Headers["UserId"]);
                  s4bFormSubmission.DateLastAmended = DateTime.Now;
                  this.writeTxt(logFilePath, "Update Record in Submission");
                  if (!new S4bFormSubmissionRepository().UpdateFormSubmission(s4bFormSubmission, request))
                  {
                     Utilities.GenerateAndLogMessage("SubmissionAndUpdate", "Unable to Update Submission.", null);
                     this.writeTxt(logFilePath, "Error in updating Submission");
                     returnValue.IsSucessfull = false;
                  }


               }
               this.writeTxt(logFilePath, "************** Next Record *****************");
               returnValue.IsSucessfull = true;

            }
         }
         catch (Exception ex)
         {
            returnValue.IsSucessfull = false;
            returnValue.Message = "Error in uploading file" + ex.Message;
            this.writeTxt(logFilePath, "ERROR Occured:" + ex.Message);
            //throw ex;
         }
         return returnValue;
      }

      public void writeTxt(string path, string text)
      {
         try
         {
            using (var tw = new StreamWriter(path, true))
            {
               tw.WriteLine(text);
               tw.Close();
            }
         }
         catch (Exception ex)
         {

         }
      }
   }
}
