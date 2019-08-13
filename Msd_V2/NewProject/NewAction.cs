using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using AskForm.Application;
using AskForm.CenterApplication;
using Msd_V2.NewProject;

namespace Msd_V2.NewProject
{
    public class NewAction
    {
        /// <summary>
        /// 配置文件
        /// </summary>
        private static NewSetting setting = new NewSetting();

        public static void Exec()
        {
            var ids = GetIDList();
            //Console.WriteLine($"社招执行了ID为{x}的数据");

            ids.ForEach(ExecForID);

        }

        private static List<long> GetIDList()
        {
            var statetime = DateTime.Now.AddDays(-3).ToString("yyyyMMddhhmmss");
            //var statetime = DateTime.Now.AddDays(-3).ToString("20190720000127");
            var endtime = DateTime.Now.AddDays(1).ToString("yyyyMMddhhmmss");
            string url = $@"http://api.beisenapp.com/RecruitV2/{setting.Tenant_ID}/applicant/GetApplicantIdsByDateAndStatus?start_time={statetime}&end_time={endtime}&phase_id=S35&status_id=U03";

            var result = HttpHelper.HttpGet(url,setting.AccessToken);
            List<long> ids = JsonConvert.DeserializeObject<List<long>>(result);
            ids.Reverse();

            return ids;
        }

        public static void ExecForID(long id)
        {
            //string url = $@"http://api.beisenapp.com/RecruitV2/{setting.Tenant_ID}/Applicant/ById/{id}?language={1}&photo_base64={0}&has_Long={1}";
            string url = $@"http://api.beisenapp.com/RecruitV2/{setting.Tenant_ID}/Applicant/GetApplicantProfileById?personids={id}";
            var result = HttpHelper.HttpGet(url, setting.AccessToken);
            if (string.IsNullOrEmpty(result) || result == "[]")
                return;

            NewApplicant app = JsonConvert.DeserializeObject<NewApplicant>(result);
            //var app = applicants[0];
            var applicant = app.applicants[0];
            var items = applicant.ApplicantProfile;
            var item = items.Find(x => x.PropertyName == "Name");
            if (item == null)
            {
                Console.WriteLine("未获取到姓名");
                return;
            }
            var applicantId = items.Find(x => x.PropertyName == "personId").Value; 

            string JobID = items.Find(x => x.PropertyName == "R_LastRelationJob").Code; //GetJobID(applicantId);
            //获取用户名和邮箱
            var firstName = item.Value.Substring(0, 1);
            var lastName = item.Value.Substring(1);
            var emailAddress = items.Find(x => x.PropertyName == "email");
            if (emailAddress == null)
            {
                Console.WriteLine(item.Value + "未填写邮箱");
                return;
            }

            var email = emailAddress.Value;

            //判断邮箱是否已发送过五次以上   临时改成发过一次就不发邮件 为了解决接口更新成测评中不成功的问题
            var emailSendCount = SelectCountByEmail(email);
            if (emailSendCount >= 1)
            {
                Console.WriteLine("邮箱已发送过五次以上");
                return;
            }

            //获取JobID(根据JOBID获取岗位序列)
            //var jobId = app.ApplyJobSummaries.LastOrDefault()?.JobCustomFields.Find(x => x.PropertyName == "extJobSequence_102854_300134805")?.Value;
            //jobId = GetJobID(jobId);
            var cadidateId = items.Find(x => x.PropertyName == "CadidateId").Value;
            //if (string.IsNullOrEmpty(jobId) || jobId == "0")
            //{
            //    Console.WriteLine("JobId为空");
            //    return;
            //}

            //根据该ID 在测评做完之后 更新测评状态
            //var jobid2 = app.ApplyJobSummaries.LastOrDefault()?.JobId;
            //var jobid2 = jobId;
            //获取NormsetId
            //string normsetId = app.ApplyJobSummaries.LastOrDefault()?.JobCustomFields.Find(x => x.PropertyName == "extszcpcmlx_102854_1709877407")?.Code;
            string normsetId = "1060";
            //if (string.IsNullOrEmpty(normsetId))
            //{
            //    Console.WriteLine("NormsetId为空");
            //    return;
            //}

            //normsetId = string.IsNullOrEmpty(normsetId) ? "1100" : normsetId;

            //normsetId = NorID.Nors.ContainsKey(normsetId) ? NorID.Nors[normsetId] : "1060";

            //app.ApplyJobSummaries.ForEach(x =>
            //{
            //    SignJobToU15(app.ApplicantId, x.JobId);
            //    jobid2 += string.IsNullOrEmpty(jobid2) ? x.JobId:string.Empty;
            //    jobId += string.IsNullOrEmpty(jobId) ? x.JobCustomFields[0].Code : string.Empty;
            //});

            //如果已经投递过岗位并且已完成测评 则按新职位(需要更新该CadidateId的JobID)模拟请求Callback
            //if (IsTested(app.CadidateId, out var entryId))
            //{
            //    //更新该用户的岗位编号
            //    UpdateJobID(entryId, Convert.ToInt64(jobId));
            //    //模拟请求
            //    SimulationExec(app.CadidateId);

            //    return;
            //}

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            var responese = new MaptqCN.ws().runWSobj(new MaptqCN.wsRequestObj()
            {
                CandidateId = cadidateId,
                LastName = firstName,
                FirstName = lastName,
                ClientId = setting.ClientId,
                ProjectId = setting.ProjectId,
                SecureCode = setting.SecureCode,
                ReturnUrl = setting.ReturnUrl,
                EmailAddress = email,
                RequestType = "page_candhub_autoregister",
                LanguageId = 19,
                NormsetId = normsetId
            });

            //只有接口请求成功的时候才修改状态
            if (responese.ErrorCode == 0)
            {
                //添加Entry
                var entryId = AskForm_Entry_Insert();

                AddValueForFieldName(entryId, setting.ApplicantIdFieldID, applicantId);
                AddValueForFieldName(entryId, setting.NameFieldID, item.Value);
                AddValueForFieldName(entryId, setting.CadidateIdFieldId, cadidateId);
                AddValueForFieldName(entryId, setting.EmailFieldID, email);
                AddValueForFieldName(entryId, setting.JobIdFieldId, JobID);
                AddValueForFieldName(entryId, setting.JobId2FieldID, JobID);
                AddValueForFieldName(entryId, setting.NormsetIdFieldID, normsetId);
                //加入邮件消息队列
                AddEmailSendList(entryId, email, responese.RedirectUrl, item.Value);

                Console.WriteLine("编号:" + id + " ,姓名:" + item.Value + " ,邮箱:" + emailAddress.Value);
            }

            SignJobToU15(applicantId, JobID);
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        private static void AddEmailSendList(long EntryID, string Email, string TestHref, string UserName)
        {
            var ec = setting.EmailContent.Replace("{LinkHref}", TestHref).Replace("{UserName}", UserName).Replace("'", "\"");
            string InsertSQL = $@"Insert into AskForm_EmailSendList(Title,Content,Recipient,CC,BCC,DisplayName,SMTPID,
                    Attachment,SentCount,AllowedCount,Status,Result,entryId,EmailServerUrlID,FormID,FormApplicationID,
                    UserID,CompanyID,IsDeleted,StartTime,CreatedDate,ModifiedDate,SendPriority,IsIdleSend)
                    Values('汇顶科技-测评通知','{ec}','{Email}','','','汇顶科技',211,'',1,2,1,'',{EntryID},53,{setting.FormID},{setting.FormApplicationID},0,{setting.CompanyID},0,GETDATE(),GETDATE(),GETDATE(),0,0)";

            DB.ExecuteNonQuery(InsertSQL, true);
        }


        /// <summary>
        /// 重新发送邮件
        /// </summary>
        /// <param name="Email"></param>
        private static void UpdateEmailToSending(string Email)
        {
            var sql = $@"update AskForm_EmailSendList set Status=1 where Recipient='{Email}' and IsDeleted=0 and FormID={setting.FormID}";
            DB.ExecuteNonQuery(sql, true);
        }

        private static int SelectCountByEmail(string Email)
        {
            var sql = $@"select count(*) from AskForm_EmailSendList where Recipient='{Email}' and IsDeleted=0 and FormID={setting.FormID}";
            return Convert.ToInt32(DB.ExecuteScalar(sql, true));
        }


        /// <summary>
        /// 为AskForm_Entry 主表添加数据
        /// </summary>
        /// <returns></returns>
        private static long AskForm_Entry_Insert()
        {
            //return new FormEntry()
            //{
            //    CompanyID = setting.CompanyID,
            //    FormApplicationID = setting.FormApplicationID,
            //    FormID = setting.FormID,
            //    FormPageID = 0,
            //    Version = 0,
            //    UserID = 0,
            //    TotalPoint = 0,
            //    Status = EntryStatus.Added,
            //    ModifiedDate = DateTime.Now,
            //    CreatedDate = DateTime.Now,
            //    CreatedUserID = 0,
            //}.InsertUpdate();

            string sql = $@"Insert Into AskForm_Entry
            (FormID,CityID,TotalPoint,AllowedVersion,Version,Duration,BigIntIP,FormPageID,SourceType,Status,Reason
            ,IsArchived,CreatedUserID,ModifiedUserID,FormApplicationID,CompanyID,IsDeleted,CreatedDate,ModifiedDate,SendStatus,SendDate) 
            Values
            ({setting.FormID},0,0,0,0,0,0,{setting.FormPageID},0,0,'',0,0,0,{setting.FormApplicationID},{setting.CompanyID},0,GETDATE(),GETDATE(),1,GETDATE())
            SELECT @@IDENTITY AS ID";
            var result = Convert.ToInt64(DB.ExecuteScalar(sql));

            return result;
        }

        /// <summary>
        /// 为EntryText添加数据  根据EntryID、FieldID、Value
        /// </summary>
        /// <param name="EntryID"></param>
        /// <param name="FieldID"></param>
        /// <param name="Value"></param>
        private static void AddValueForFieldName(long EntryID, long FieldID, string Value)
        {
            //var entryTextID = new EntryText()
            //{
            //    entryId = entryId,
            //    FieldID = FieldID,
            //    Value = Value,
            //    CompanyID = setting.CompanyID,
            //    FormApplicationID = setting.FormApplicationID,
            //    FormID = setting.FormID,
            //    CreatedDate = DateTime.Now,
            //    CreatedUserID = 0,
            //    ModifiedUserID = 0
            //}.InsertUpdate();

            DB.ExecuteNonQuery($@"Insert Into AskForm_EntryText(entryId, FieldID, Value, Point, FormID, FormApplicationID, CompanyID, IsDeleted, CreatedDate, ModifiedDate, CreatedUserID, ModifiedUserID)
                Values({EntryID},{FieldID},'{Value}',0,{setting.FormID},{setting.FormApplicationID},{setting.CompanyID},0,getdate(),getdate(),0,0)");
        }


        /// <summary>
        /// 查找是否已做过测评 (使用AppID查询到EntryID.使用EntryID查询测评分数)
        /// </summary>
        public static bool IsTested(string cand, out long entryId)
        {
            entryId = 0;
            var sql = $@"select entryId,Version from AskForm_Entry where IsDeleted=0 and entryId=(select top 1 entryId from AskForm_EntryText 
            where IsDeleted=0 and FieldID={setting.CadidateIdFieldId} and Value='{cand}' Order By CreatedDate Desc)";

            var dt = DB.ExecuteQuery(sql);
            if (dt.Rows.Count != 0)
            {
                entryId = Convert.ToInt64(dt.Rows[0]["entryId"]);
                //当大于等于4的时候  需要根据candid重新生成对应的报告
                return Convert.ToInt32(dt.Rows[0]["Version"]) >= 2;
            }

            return false;
        }

        /// <summary>
        /// 如果已经测评的AppID  那么直接模拟Post  Callback
        /// </summary>
        public static void SimulationExec(string cadidateId)
        {
            var url = "https://app.askform.cn/Custom/MSDReport/SHDTCallBack-v3.ashx";
            var parms = $"instr=1&cand={cadidateId}&valid=1";
            HttpHelper.HttpPost(url, parms);
        }

        /// <summary>
        /// 更新用户的JOBID
        /// </summary>
        private static void UpdateJobID(long entryId, long jobId)
        {
            var Sql = $@"update AskForm_EntryText set Value='{jobId}' where entryId={entryId} and FormID={setting.FormID} and 
            IsDeleted=0 and FieldID={setting.JobIdFieldId}";
            DB.ExecuteNonQuery(Sql);
        }

        /// <summary>
        /// 将简历标记为测评进行中
        /// </summary>
        private static void SignJobToU15(string applicantId, string jobId)
        {
            var url = $"http://api.beisenapp.com/recruitv2/{setting.Tenant_ID}/applicant/contojoborstorsDb?format=json";

            Dictionary<string, string> dicParameter = new Dictionary<string, string>
            {
                    {"tenant_id",setting.Tenant_ID},
                    {"applicant_ids",applicantId},
                    {"job_id",jobId},
                    {"phase_id","S035" },
                    {"status_id","U028" }
            };
            string resultjson = HttpHelper.HttpPost(url, dicParameter, setting.AccessToken);

            Result result = JsonConvert.DeserializeObject<Result>(resultjson);
        }

        /// <summary>
        /// 获取JobID
        /// </summary>
        private static string GetJobID(string Applicantid)
        {
            var Url = "http://api.beisenapp.com/RecruitV2/" + setting.Tenant_ID + "/applicant/GetApplyListById?personids=" + Applicantid;
            //Dictionary<string, string> dicParameter = new Dictionary<string, string>() {
            //            {"applicant_ids",Applicantid}
            //    };

            string resultjson = HttpHelper.HttpGet(Url,setting.AccessToken);

            if (string.IsNullOrEmpty(resultjson) || resultjson == "[]")
                return "";

            Apply app = JsonConvert.DeserializeObject<Apply>(resultjson);

            return app.allApplys.First().Applys.First().jobId;
        }
    }
}
