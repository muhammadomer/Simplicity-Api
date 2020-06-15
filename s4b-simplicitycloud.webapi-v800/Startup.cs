using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.ClientInvoice.Models.Interfaces;
using SimplicityOnlineWebApi.ClientInvoice.Models.Repositories;
using SimplicityOnlineWebApi.Models.Interfaces;
using SimplicityOnlineWebApi.Models.Repositories;
using SimplicityOnlineWebApi.Commons;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PdfSharpCore.Internal;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using FluentScheduler;
//using SimplicityOnlineWebApi.BLL.CustomBinders;

namespace SimplicityOnlineWebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment hostingEnv)
        {
            var builder = new ConfigurationBuilder().SetBasePath(hostingEnv.ContentRootPath).AddJsonFile("config.json").AddEnvironmentVariables();
            Configuration = builder.Build();
            LoadConfigs(hostingEnv);
            hostingEnv.ConfigureLog4Net("log4net.xml");

            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //services.AddCors(options =>
            //{
            //    options.AddPolicy("CorsPolicy", builder =>
            //        builder.SetIsOriginAllowed(_ => true)
            //        .AllowAnyMethod()
            //        .AllowAnyHeader()
            //        .AllowCredentials());
            //});
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                    builder.AllowAnyOrigin();
                    //builder.WithOrigins("https://demomssql.test-simplicity4business.co.uk:9000");
                    //builder.AllowCredentials();
                });
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc(option => option.EnableEndpointRouting = false)
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddJsonOptions(options =>
                    {
                        options.JsonSerializerOptions.IgnoreNullValues = true;
                        options.JsonSerializerOptions.PropertyNamingPolicy = null;
                    });
            //services.AddMvc(config =>
            //    config.ModelBinderProviders.Insert(0, new LongToZeroModelBinderProvider())
            //);

            //services.AddControllers().AddNewtonsoftJson();
            //----Add Logging
            services.AddLogging(builder => builder
                 .AddConsole()
                 .AddDebug()
                 .AddLog4Net("log4net.xml")
                 );  // Rule for all providers

            services.AddLogging(ConfigLogging);
            //-----aDD SCOPE
            services.AddScoped<IApplicationWebPagesRepository, ApplicationWebPagesRepository>();
            services.AddScoped<IAttachmentFilesFolderRepository, SimplicityOnlineBLL.Entities.AttachmentFilesFolderRepository>();
            services.AddScoped<IAddInfoRepository, AddInfoRepository>();
            services.AddScoped<IUserDetailsRepository, UserDetailsRepository>();
            services.AddScoped<IRefGenericLabelsRepository, RefGenericLabelsRepository>();
            services.AddScoped<IOrdersTagsRepository, OrdersTagsRepository>();
            services.AddScoped<IOrdersTagsImagesRepository, OrdersTagsImagesRepository>();
            services.AddScoped<IWebThirdPartiesRepository, WebThirdPartiesRepository>();
            services.AddScoped<IDiaryAppsRepository, DiaryAppsRepository>();
            services.AddScoped<IDiaryAppsWebAssignRepository, DiaryAppsWebAssignRepository>();
            services.AddScoped<INaturalFormsRepository, NaturalFormsRepository>();
            services.AddScoped<IRefVisitStatusTypesRepository, RefVisitStatusTypesRepository>();
            services.AddScoped<IDiaryAppsReturnedRepository, DiaryAppsReturnedRepository>();
            services.AddScoped<IOrdersRepository, OrdersRepository>();
            services.AddScoped<IOrderItemsRepository, OrderItemsRepository>();
            services.AddScoped<IPassthroughRepository, PassthroughRepository>();
            services.AddScoped<IDiaryResourcesRepository, DiaryResourcesRepository>();
            services.AddScoped<IEntityDetailsCoreRepository, EntityDetailsCoreRepository>();
            services.AddScoped<ITradeCodeRepository, TradeCodeRepository>();
            services.AddScoped<IS4BFormsRepository, S4BFormsRepository>();
            services.AddScoped<ISiteInspectionRepository, SiteInspectionRepository>();
            services.AddScoped<IS4bFormSubmissionRepository, S4bFormSubmissionRepository>();
            services.AddScoped<IRefS4bFormsRepository, RefS4bFormsRepository>();
            services.AddScoped<IS4bFormsAssignRepository, S4bFormsAssignRepository>();
            services.AddScoped<IAssetRegisterRepository, AssetRegisterRepository>();
            services.AddScoped<IRefPropertyTypesRepository, RefPropertyTypesRespository>();
            services.AddScoped<IEntityDetailsSupplementaryRepository, EntityDetailsSupplementaryRepository>();
            services.AddScoped<IEntityDetailsJoinRepository, EntityDetailsJoinRepository>();
            services.AddScoped<IOrdersMeHeaderRepository, OrdersMeHeaderRepository>();
            services.AddScoped<IOrdersTendersRepository, OrdersTendersRepository>();
            services.AddScoped<IWebViewerRepository, WebViewerRepository>();
            services.AddScoped<IRefDiaryTypesRepository, RefDiaryAppTypesRepository>();
            services.AddScoped<IRefTradeCodeTypeRepository, RefTradeCodeTypeRepository>();
            services.AddScoped<IRefDiaryAppRatesRepository, RefDiaryAppRatesRepository>();
            services.AddScoped<IAttFOrderDocsMasterRepository, AttFOrderDocsMasterRepository>();
            services.AddScoped<IRefOrderTypeRepository, RefOrderTypeRepository>();
            services.AddScoped<IRefJobStatusTypeRepository, RefJobStatusTypeRepository>();
            services.AddScoped<IOrderCheckListRepository, OrderCheckListRepository>();
            services.AddScoped<IMailMergeRepository, MailMergeRepository>();
            services.AddScoped<ICldSettingsRepository, CldSettingsRepository>();
            services.AddScoped<ITimeAndAttendanceRepository, TimeAndAttendanceRepository>();
            services.AddScoped<IDashboardRepository, DashboardRepository>();
            services.AddScoped<IRefEntityPaymentTypeRepository, RefEntityPaymentTypeRepository>();
            services.AddScoped<IRefItemTypeRepository, RefItemTypesRespository>();
            services.AddScoped<IRefProductUnitRepository, RefProductUnitRespository>();
            services.AddScoped<IPercentageRateRepository, PercentageRateRespository>();
            services.AddScoped<IScheduleItemsRepository, ScheduleItemsRepository>();
            services.AddScoped<IS4BFormTemplateRepository, S4BFormTemplateRepository>();
            services.AddScoped<IDiaryAppsNaturalFormsRepository, DiaryAppsNaturalFormsRepository>();
            services.AddScoped<IRefNaturalFormsRepository, RefNaturalFormsRepository>();
            services.AddScoped<IOrdersBillsRepository, OrdersBillsRepository>();
            services.AddScoped<IAppSettingRepository, AppSettingRepository>();
            services.AddScoped<IEdcGdprRepository, EdcGdprRepository>();
            services.AddScoped<IUserNotificationsRepository, UserNotificationsRepository>();
            services.AddScoped<IClientInvoiceRepository, ClientInvoiceRepository>();
            services.AddScoped<ITmpTimesheetRepository, TmpTimesheetRepository>();
            services.AddScoped<IReportRepository, ReportRepository>();
            services.AddScoped<IRefOrderHireDamageTypeRepository, RefOrderHireDamageTypeRepository>();
            services.AddScoped<IOrderHireRepository, OrderHireRepository>();
            services.AddScoped<IPurchaseOrderItemsRepository, PurchaseOrderItemsRepository>();
            services.AddScoped<IRossumRepository, RossumRepository>();
            services.AddScoped<ISupplierInvoiceRepository, SupplierInvoiceRepository>();
            services.AddScoped<ICloudStorageRepository, CloudStorageRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IRossumRepository rossumRepository)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            //loggerFactory. = LogLevel.Information;
            //loggerFactory.AddConsole();
            //loggerFactory.AddDebug();
            //loggerFactory.AddLog4Net();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            //app.UseIISPlatformHandler();
            app.UseStaticFiles();
            app.UseCors("CorsPolicy");

            JobManager.Initialize(new RossumScheduler(rossumRepository));

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });


        }
        private void ConfigLogging(ILoggingBuilder builder)
        {
            builder.SetMinimumLevel(LogLevel.Trace);
            //... additional configuration...
        }
        private void LoadConfigs(IWebHostEnvironment hostingEnv)
        {
            //BLOCK: Project Settings
            IEnumerable<IConfigurationSection> configSectionsForKey = Configuration.GetSection("AppSettings:ProjectIds").GetChildren();
            Configs.settings = new Dictionary<string, SimplicityOnlineBLL.Entities.ProjectSettings>();
            foreach (var value in configSectionsForKey)
            {
                string configFileName = value.Value;
                string configFilePath = Path.Combine(hostingEnv.ContentRootPath, configFileName);
                ProjectSettings settings = Utilities.LoadConfigSettings(configFilePath);
                Configs.settings.Add(value.Key, settings);
            }

            //BLOCK: Email Settings
            configSectionsForKey = Configuration.GetSection("AppSettings:EmailSettings").GetChildren();
            EmailSettings emailSettings = new EmailSettings();
            foreach (var value in configSectionsForKey)
            {
                switch (value.Key.ToLower())
                {
                    case "host":
                        emailSettings.HostName = value.Value;
                        break;

                    case "user":
                        emailSettings.HostUser = value.Value;
                        break;

                    case "password":
                        emailSettings.HostPassword = value.Value;
                        break;

                    case "port":
                        emailSettings.HostPort = Int32.Parse(value.Value);
                        break;

                    case "enablessl":
                        emailSettings.IsEnableSsl = Convert.ToBoolean(value.Value);
                        break;
                }
            }
            Configs.EmailSettings = emailSettings;

            //BLOCK: Email Doc Logos for s4B Forms
            Configs.DocumentLogos = new List<RefDocsLogos>();
            configSectionsForKey = Configuration.GetSection("AppSettings:DocsLogos").GetChildren();
            foreach (var value in configSectionsForKey)
            {
                IEnumerable<IConfigurationSection> docLogosSections = Configuration.GetSection(value.Path).GetChildren();
                RefDocsLogos item = new RefDocsLogos();
                foreach (var docLogoEntry in docLogosSections)
                {
                    switch (docLogoEntry.Key.ToLower())
                    {
                        case "ext":
                            item.Ext = docLogoEntry.Value;
                            break;

                        case "logourl":
                            item.LogoURL = docLogoEntry.Value;
                            break;

                        case "isimgext":
                            item.IsImgExt = Boolean.Parse(docLogoEntry.Value);
                            break;
                    }
                }
                Configs.DocumentLogos.Add(item);
            }

            //BLOCK: Rossum Settings

             

            configSectionsForKey = Configuration.GetSection("AppSettings:RossumSettings").GetChildren();
            Configs.RossumSettings = new List<RossumSetting>();

            foreach (var value in configSectionsForKey)
            {
                IEnumerable<IConfigurationSection> rossumSections = Configuration.GetSection(value.Path).GetChildren();
                RossumSetting rossSetting = new RossumSetting();
                var temp = rossumSections.ToList().Find(x => x.Key.ToLower() == "project_id");
                rossSetting.ProjectId = temp == null ? "" : temp.Value;
                temp = rossumSections.ToList().Find(x => x.Key.ToLower() == "user_id");
                rossSetting.UserId = temp==null? 0 : Convert.ToInt32(temp.Value);
                temp = rossumSections.ToList().Find(x => x.Key.ToLower() == "invoice_queue_id");
                rossSetting.InvoicesQueueID= temp == null ? 0 : Convert.ToInt32(temp.Value);
                temp = rossumSections.ToList().Find(x => x.Key.ToLower() == "receipt_queue_id");
                rossSetting.ReceiptsQueueID= temp == null ? 0 : Convert.ToInt32(temp.Value);
                temp = rossumSections.ToList().Find(x => x.Key.ToLower() == "purchase_order_queue_id");
                rossSetting.PurchaseOrdersQueueID = temp == null ? 0 : Convert.ToInt32(temp.Value);
                temp = rossumSections.ToList().Find(x => x.Key.ToLower() == "delivery_note_queue_id");
                rossSetting.DeliveryNotesQueueID = temp == null ? 0 : Convert.ToInt32(temp.Value);
                temp = rossumSections.ToList().Find(x => x.Key.ToLower() == "is_run_scheduler");
                rossSetting.IsRunScheduler = temp == null ? false : Convert.ToBoolean(temp.Value);
                Configs.RossumSettings.Add (rossSetting);
            }
        }

    }
}
