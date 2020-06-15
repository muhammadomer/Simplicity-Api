using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using SimplicityOnlineWebApi.DAL;

namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class AttFOrderDocsMasterRepository : IAttFOrderDocsMasterRepository
    {
        public string Message { get; set; }
        //private ILogger<AttFOrderDocsMasterRepository> LOGGER;
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }
        public AttFOrderDocsMasterRepository()
        {
        }

        public List<AttfOrdDocsMasters> GetAttfOrderDocsMasterByJobSequence(HttpRequest request, long jobSequence)
        {
            List<AttfOrdDocsMasters> returnValue = null;
            string projectId = request.Headers["ProjectId"];
            if (!string.IsNullOrWhiteSpace(projectId))
            {
                ProjectSettings settings = Configs.settings[projectId];
                if (settings != null)
                {
                    AttfOrdDocsMastersDB attFOrderDocsMasterDB = new AttfOrdDocsMastersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    returnValue = attFOrderDocsMasterDB.selectAllAttfOrdDocsMastersByJobSequence(jobSequence);
                }
                if (returnValue != null)
                {
                    //LOGGER.LogInformation("AttfOrdDocsMasters Capacity: " + returnValue.Capacity);
                }
                else
                {
                    //LOGGER.LogInformation("GetAttfOrderDocsMasterByJobSequence - AttfOrdDocsMasters Query returned no result");
                }
            }
            return returnValue;
        }

        public SimplicityFile GetAttfOrderDocsMasterByJobSequenceAndSequence(HttpRequest request, long jobSequence, long sequence)
        {
            ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
            if (settings != null)
            {
                AttfOrdDocsMastersDB attFOrderDocsMasterDB = new AttfOrdDocsMastersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                AttfOrdDocsMasters attfOrdDocsMasters = attFOrderDocsMasterDB.selectAllAttfOrdDocsMastersByJobSequenceAndSequence(jobSequence, sequence);
                if (attfOrdDocsMasters != null)
                {
                    string templatePath = Utilities.getOrderTemplatePath(settings, attfOrdDocsMasters.FilePathAndName);
                    //LOGGER.LogInformation("Reading PDF Template at Path: " + templatePath);
                    if (File.Exists(templatePath))
                    {
                        byte[] templateArray = File.ReadAllBytes(templatePath);
                        return new SimplicityFile()
                        {
                            FileName = Path.GetFileName(templatePath),
                            Isbase64 = false,
                            MemStream = new MemoryStream(templateArray),
                            ContentType = MimeKit.MimeTypes.GetMimeType(Path.GetFileName(templatePath))
                        };
                    }
                    else
                    {
                        //LOGGER.LogInformation("File  " + templatePath + " doestn't exist.");
                    }
                }
                else
                {
                    //LOGGER.LogInformation("AttfOrdDocsMasters query return no result. [Job Sequence =" + jobSequence + ", Sequence = " + sequence + "]");
                }
            }
            return null;
        }

        public bool PutAttFOrdDocsMaster(HttpRequest request, AttfOrdDocsMastersFile attfOrdDocsMastersFile)
        {
            bool returnValue = false;
            this.Message = "";
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        if (attfOrdDocsMastersFile != null && attfOrdDocsMastersFile.OrderDocFile != null && attfOrdDocsMastersFile.OrderDocFile.Base64String != "")
                        {
                            string fileName = "";
                            long fileNumber = 1;
                            string folderName = "ORD_DOC-" + attfOrdDocsMastersFile.JobSequence;
                            string filePath = Path.Combine(settings.AttFileOrderDocsPath, folderName);
                            if (!Directory.Exists(filePath))
                            {
                                Directory.CreateDirectory(filePath);
                            }
                            string dbFilePath = Path.Combine(settings.AttFileOrderDocsLocationDB, folderName);
                            AttFOrderDocsFileCounterRepository attFOrderDocsFileCounterRepository = new AttFOrderDocsFileCounterRepository();
                            long fileNo = attFOrderDocsFileCounterRepository.GetAndUpdateAttfOrderDocsFileCounterNextFileNoByJobSequenceAndFlgMasterFile(request, attfOrdDocsMastersFile.JobSequence ?? 0, false);
                            var numJobSeq = attfOrdDocsMastersFile.JobSequence ?? 0;
                            fileName = "F-" + numJobSeq.ToString("000000") + "-" + fileNumber.ToString("0000") + ".pdf";
                            filePath = Path.Combine(filePath, fileName);
                            dbFilePath = Path.Combine(dbFilePath, fileName);
                            if (Utilities.SaveBase64File(attfOrdDocsMastersFile.OrderDocFile.Base64String, filePath))
                            {
                                long sequence = -1;
                                long userId = Utilities.GetUserIdFromRequest(request);
                                returnValue = new AttfOrdDocsFilesRepository().insertAttfOrdDocsFiles(request, out sequence, false, attfOrdDocsMastersFile.JobSequence ?? 0, attfOrdDocsMastersFile.Sequence ?? 0, "-1",
                                                                                                      "File Uploaded", "", dbFilePath, userId, DateTime.Now, userId, DateTime.Now);
                            }
                            else
                            {
                                //log.Error("Unable to write file to the Server.");
                            }
                        }
                        else
                        {
                            Message = "Invalid Request";
                        }
                    }
                    else
                    {
                        //log.Info("Config is null. In Method " + METHOD_NAME);
                    }
                }
            }
            catch (Exception ex)
            {
                //log.Error("Exception occurred while Inserting Record in AttfOrdDocFiles. " + ex + ". In Method " + METHOD_NAME);
            }
            return returnValue;
        }
    }
}
