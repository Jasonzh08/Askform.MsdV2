using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;

namespace Msd_V2
{
    public class ShezhaoAction
    {
        static Setting setting = new Setting();

        public static void Exec()
        {
            var list = GetList();

            list.ForEach(x =>
            {
                _exec(x);
                //Console.WriteLine($"社招执行了ID为{x}的数据");
            });
        }

        private static List<long> GetList()
        {
            var statetime = DateTime.Now.AddDays(-1).ToString("yyyyMMddhhmmss");
            var endtime = DateTime.Now.AddDays(1).ToString("yyyyMMddhhmmss");
            string Url = $@"http://api.beisenapp.com/RecruitV2/{setting.Tenant_ID}/applicant/GetApplicantIdsByDateAndStatus?start_time={statetime}&end_time={endtime}&phase_id=S02&status_id=U03";

            var result = HttpHelper.HttpGet(Url);
            List<long> ids = JsonConvert.DeserializeObject<List<long>>(result);
            ids.Reverse();
            return ids;
        }


        public static void _exec(long id)
        {
            string Url = $@"http://api.beisenapp.com/RecruitV2/{setting.Tenant_ID}/Applicant/ById/{id}?language={1}&photo_base64={0}&has_Long={1}";
            var Result = HttpHelper.HttpGet(Url);
            if (string.IsNullOrEmpty(Result) || Result == "[]") { return; }
             
            List<Applicant> AppList = JsonConvert.DeserializeObject<List<Applicant>>(Result);
            var app = AppList[0];
            var ItemObj = app.Profile.Items.Find(x => x.PropertyName == "Name");
            if (ItemObj == null)
            {
                //ErrorLog.WriteLog(new Msd_V2.ErrorObj()
                //{
                //    ErrorMsg ="用户名为空",
                //    ID = id,
                //    UserName =""
                //});
                return;
            }


            //获取用户名和邮箱
            var LastName = ItemObj.Value.Substring(0, 1);
            var FirstName = ItemObj.Value.Substring(1);
            var EmailObj = app.Profile.Items.Find(x => x.PropertyName == "email");
            if (EmailObj == null)
            {
                //ErrorLog.WriteLog(new Msd_V2.ErrorObj()
                //{
                //    ErrorMsg ="未填写邮箱",
                //    ID = id,
                //    UserName = ItemObj.Value
                //});
                Console.WriteLine(ItemObj.Value + "未填写邮箱");
                return;
            }

            var Email = EmailObj.Value;
            //if (Email.IndexOf("@qq.com") > 0 || Email.IndexOf("@163.com") > 0) {
            //    return;
            //}

            //判断邮箱是否已发送过五次以上
            var emailSendCount = SelectCountByEmail(Email);
            if (emailSendCount > 5)
            {
                //ErrorLog.WriteLog(new Msd_V2.ErrorObj()
                //{
                //    ErrorMsg ="已连续发送超过5次,可能是标记不成功,已跳过",
                //    ID = id,
                //    UserName = ItemObj.Value
                //}, "RepeatEmail.txt");
                return;
            }

            //重复发送测评
            //if (Result.IndexOf("extshfxychffscp_102854_2003095649") > 0)
            //{
            //    UpdateEmailToSending(Email);
            //    return;
            //}

            //Email = "442485840@qq.com";

            //获取JobID(根据JOBID获取岗位序列)
            string jobid = app.ApplyJobSummaries[app.ApplyJobSummaries.Count-1].JobCustomFields.Find(x => x.PropertyName == "extJobSequence_102854_300134805")?.Value;
            jobid = GetJobID(jobid);

            if (string.IsNullOrEmpty(jobid)|| jobid=="0")
            {
                //ErrorLog.WriteLog(new Msd_V2.ErrorObj()
                //{
                //    ErrorMsg = "JobID为空",
                //    ID = id,
                //    UserName = ItemObj.Value
                //});
                return;
            }

            //根据该ID 在测评做完之后 更新测评状态
            string jobid2 = app.ApplyJobSummaries[app.ApplyJobSummaries.Count-1].JobId;
            //获取NormsetId
            string NormsetId = app.ApplyJobSummaries[app.ApplyJobSummaries.Count-1].JobCustomFields.Find(x => x.PropertyName == "extszcpcmlx_102854_1709877407")?.Code;

            if (string.IsNullOrEmpty(NormsetId))
            {
                //ErrorLog.WriteLog(new Msd_V2.ErrorObj()
                //{
                //    ErrorMsg = "NormsetId为空",
                //    ID = id,
                //    UserName = ItemObj.Value
                //});
                return;
            }

            NormsetId = string.IsNullOrEmpty(NormsetId) ? "1100" : NormsetId;
            
            NormsetId = NorID.Nors.ContainsKey(NormsetId) ? NorID.Nors[NormsetId] : "1060";
             
            //app.ApplyJobSummaries.ForEach(x =>
            //{
            //    SignJobToU15(app.ApplicantId, x.JobId);
            //    jobid2 += string.IsNullOrEmpty(jobid2) ? x.JobId:string.Empty;
            //    jobid += string.IsNullOrEmpty(jobid) ? x.JobCustomFields[0].Code : string.Empty;
            //});

            long EntryID = 0;
            //如果已经投递过岗位并且已完成测评 则按新职位(需要更新该CadidateId的JobID)模拟请求Callback
            if (IsTested(app.CadidateId, out EntryID))
            {
                //更新该用户的岗位编号
                UpdateJobID(EntryID, Convert.ToInt64(jobid));
                //模拟请求
                SimulationExec(app.CadidateId);
                return;
            }


            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            var responese = new MaptqCN.ws().runWSobj(new MaptqCN.wsRequestObj()
            {
                CandidateId = app.CadidateId,
                LastName = FirstName,
                FirstName = LastName,
                ClientId = "10000010",
                ProjectId = "10000383",
                SecureCode = "f97c3ded78e44adf18bc35780916b6df",
                ReturnUrl = "https://app.askform.cn/Custom/MSDReport/SHDTCallBack-v2.ashx",
                EmailAddress = Email,
                RequestType = "page_candhub_autoregister",
                LanguageId = 19,
                NormsetId = NormsetId
            });

            //只有接口请求成功的时候才修改状态
            if (responese.ErrorCode == 0)
            {
                //添加Entry
                EntryID = AskForm_Entry_Insert();

                AddValueForFieldName(EntryID, setting.ApplicantIdFieldID, app.ApplicantId.ToString());
                AddValueForFieldName(EntryID, setting.NameFieldID, ItemObj.Value);
                AddValueForFieldName(EntryID, setting.CadidateIdFieldID, app.CadidateId);
                AddValueForFieldName(EntryID, setting.EmailFieldID, Email);
                AddValueForFieldName(EntryID, setting.JobIdFieldID, jobid);
                AddValueForFieldName(EntryID, setting.JobIdsFieldID, jobid2);
                AddValueForFieldName(EntryID, setting.NormsetIdFieldID, NormsetId);
                //加入邮件消息队列
                AddEmailSendList(EntryID, Email, responese.RedirectUrl, ItemObj.Value);

                Console.WriteLine("编号:" + id + " ,姓名:" + ItemObj.Value + " ,邮箱:" + EmailObj.Value);
            }

            SignJobToU15(app.ApplicantId, jobid2);
        }

        #region 邮件内容
        /// <summary>
        /// 邮件内容
        /// </summary>
        private static string EmailContent = @"<p>
        <a><span style='font-family:宋体'>亲爱的</span> </a><span style='font-size:13px;line-height:107%;font-family:&#39;Verdana&#39;,sans-serif'>{UserName}</span><span style='font-family:宋体'>：</span>
        </p>

        <p>
            <span style='font-family:宋体'>您好！</span>
        </p>
        <p>
            <span style='font-family:宋体'>恭喜您通过上汽大通社会招聘的简历筛选环节。现邀请您进行上汽大通网上评估环节。</span>
        </p>
        <p>
            1. <span style='font-family:宋体'>在线测试：测试前，请确保您的网络稳定，并处在一个相对安静的环境以避免中途被打扰。您的测评任务如下</span>:
        </p>
        <p class='MsoListParagraph' style='margin-left:28px'>
            <span style='font-family:Wingdings'>l<span style='font:9px &#39;Times New Roman&#39;'>&nbsp; </span></span><span style='font-family:宋体'>职业行为问卷</span> – <span style='font-family:宋体'>约</span> 15<span style='font-family:宋体'>分鈡</span>
        </p>
        <p class='MsoListParagraph' style='margin-left:28px'>
            <span style='font-family:Wingdings'>l<span style='font:9px &#39;Times New Roman&#39;'>&nbsp; </span></span><span style='font-family:宋体'>工作动机问卷</span> – <span style='font-family:宋体'>约</span> 15<span style='font-family:宋体'>分鈡</span>
        </p>
        <p>
            <a><span style='font-family:宋体'>当您一切准备就绪，要进入测试。请点击</span> </a><a></a><a>&nbsp;</a><a></a><a></a><a href='{LinkHref}'>{LinkHref}</a>
        </p>
        <p>
            &nbsp;
        </p>
        <p>
            2<span style='font-family:宋体'>、注意事项：</span>
        </p>
        <p>
            <span style='font-family:宋体'>（</span>1<span style='font-family:宋体'>）请安装好</span>Flash<span style='font-family:宋体'>插件，建议使用火狐或</span>chrome<span style='font-family:宋体'>浏览器，并在稳定的网络环境中完成测试。若网速不稳定，此可能造成测试过程中断；</span>
        </p>
        <p>
            <span style='font-family:宋体'>（</span>2<span style='font-family:宋体'>）请独立完成全部测试；</span>
        </p>
        <p>
            <span style='font-family:宋体'>（</span>3<span style='font-family:宋体'>）登录后请仔细阅读说明，熟悉说明将帮助您更好的完成测试；</span>
        </p>
        <p>
            <a></a><a><span style='font-family: 宋体'>（</span>4</a><span style='font-family:宋体'>）请安排好时间，一次性完成所有测试</span>;
        </p>
        <p>
            <span style='font-family:宋体'>（</span>5<span style='font-family:宋体'>）测评截止时间：请在参加综合评估</span>/<span style='font-family:宋体'>面试前完成测试；</span>
        </p>
        <p>
            <span style='font-family:宋体'>（</span>6<span style='font-family:宋体'>）如果测试中途中断，请不要紧张，重新点击测试链接即可再次进入测评。</span>
        </p>

        <p>
            <span style='font-family:宋体'>如您未能在指定时间内完成全部测试，我们将无法获取您的任何测试结果。测试过程中如果遇到任何技术问题，请发邮件至</span> <a href='mailto:info.china@cut-e.com'>info.china@cut-e.com</a> <span style='font-family:宋体'>咨询。技术小组将在</span>2<span style='font-family:宋体'>小时内做出答复。</span>
        </p>
        <p>
            <span style='font-family:宋体'>感谢您关注上汽大通招聘。</span>
        </p>


        <p style='color:#7100E9;'>
            <span style='font-family:宋体;'>上汽大通</span><span style='font-family:宋体'>人力资源部</span>
        </p>
        <p>
            <br />
        </p>";
        #endregion

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="EntryID"></param>
        /// <param name="Email"></param>
        /// <param name="TestHref"></param>
        /// <param name="UserName"></param>
        /// <param name="SendAgn"></param>
        private static void AddEmailSendList(long EntryID, string Email, string TestHref, string UserName)
        {
            var ec = EmailContent.Replace("{LinkHref}", TestHref).Replace("{UserName}", UserName).Replace("'", "\"");
            string InsertSQL = $@"Insert into AskForm_EmailSendList(Title,Content,Recipient,CC,BCC,DisplayName,SMTPID,
                    Attachment,SentCount,AllowedCount,Status,Result,EntryID,EmailServerUrlID,FormID,FormApplicationID,
                    UserID,CompanyID,IsDeleted,StartTime,CreatedDate,ModifiedDate,SendPriority,IsIdleSend)
                    Values('上汽大通-测评通知','{ec}','{Email}','','','上汽大通',211,'',1,2,1,'',{EntryID},53,{setting.FormID},{setting.FormApplicationID},0,{setting.CompanyID},0,GETDATE(),GETDATE(),GETDATE(),0,0)";

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
            string SQL = $@"Insert Into AskForm_Entry
            (FormID,CityID,TotalPoint,AllowedVersion,Version,Duration,BigIntIP,FormPageID,SourceType,Status,Reason
            ,IsArchived,CreatedUserID,ModifiedUserID,FormApplicationID,CompanyID,IsDeleted,CreatedDate,ModifiedDate,SendStatus,SendDate) 
            Values
            ({setting.FormID},0,0,0,0,0,0,{setting.FormPageID},0,0,'',0,0,0,{setting.FormApplicationID},{setting.CompanyID},0,GETDATE(),GETDATE(),1,GETDATE())
            SELECT @@IDENTITY AS ID";
            var Result = Convert.ToInt64(DB.ExecuteScalar(SQL));
            return Result;
        }

        /// <summary>
        /// 为EntryText添加数据  根据EntryID、FieldID、Value
        /// </summary>
        /// <param name="EntryID"></param>
        /// <param name="FieldID"></param>
        /// <param name="Value"></param>
        private static void AddValueForFieldName(long EntryID, long FieldID, string Value)
        {
            DB.ExecuteNonQuery($@"Insert Into AskForm_EntryText(EntryID, FieldID, Value, Point, FormID, FormApplicationID, CompanyID, IsDeleted, CreatedDate, ModifiedDate, CreatedUserID, ModifiedUserID)
                Values({EntryID},{FieldID},'{Value}',0,{setting.FormID},{setting.FormApplicationID},{setting.CompanyID},0,getdate(),getdate(),0,0)");
        }


        /// <summary>
        /// 查找是否已做过测评 (使用AppID查询到EntryID.使用EntryID查询测评分数)
        /// </summary>
        public static bool IsTested(string CandID, out long EntryID)
        {
            EntryID = 0;
            var Sql = $@"select EntryID,Version from AskForm_Entry where IsDeleted=0 and EntryID=(select top 1 EntryID from AskForm_EntryText 
            where IsDeleted=0 and FieldID={setting.CadidateIdFieldID} and Value='{CandID}' Order By CreatedDate Desc)";

            var dt = DB.ExecuteQuery(Sql);
            if (dt.Rows.Count != 0)
            {
                EntryID = Convert.ToInt64(dt.Rows[0]["EntryID"]);
                //当大于等于4的时候  需要根据candid重新生成对应的报告
                return Convert.ToInt32(dt.Rows[0]["Version"]) >= 2;
            }
            return false;
        }

        /// <summary>
        /// 如果已经测评的AppID  那么直接模拟Post  Callback
        /// </summary>
        public static void SimulationExec(string CadidateId)
        {
            var url = "https://app.askform.cn/Custom/MSDReport/SHDTCallBack-v2.ashx";
            var parms = $"instr=1&cand={CadidateId}&valid=1";
            HttpHelper.HttpPost(url, parms);
        }

        /// <summary>
        /// 更新用户的JOBID
        /// </summary>
        private static void UpdateJobID(long EntryID, long JobID)
        {
            var Sql = $@"update AskForm_EntryText set Value='{JobID}' where EntryID={EntryID} and FormID={setting.FormID} and 
            IsDeleted=0 and FieldID={setting.JobIdFieldID}";
            DB.ExecuteNonQuery(Sql);
        }

        /// <summary>
        /// 将简历标记为测评进行中
        /// </summary>
        private static void SignJobToU15(string Applicantid, string Jobid)
        {
            var Url = $"http://api.beisenapp.com/recruitv2/{setting.Tenant_ID}/applicant/contojoborstorsDb?format=json";

            Dictionary<string, string> dicParameter = new Dictionary<string, string>() {
                    {"applicant_ids",Applicantid},
                    {"job_id",Jobid},
                    {"phase_id","S02" },
                    {"status_id","U015" }
            };
            string resultjson = HttpHelper.HttpPost(Url, dicParameter);
            Result result = JsonConvert.DeserializeObject<Result>(resultjson);
        }





        private static string GetJobID(string str)
        {
            if (string.IsNullOrEmpty(str)) return "0";

            if (str.IndexOf("研发序列") >= 0)
            {
                return "18110205";
            }
            else if (str.IndexOf("供应链序列") >= 0)
            {
                return "18110201";
            }
            else if (str.IndexOf("IT序列") >= 0)
            {
                return "18110202";
            }
            else if (str.IndexOf("职能序列") >= 0)
            {
                return "18110203";
            }
            else if (str.IndexOf("制造序列") >= 0)
            {
                return "18110204";
            }
            else if (str.IndexOf("营销序列") >= 0) {
                return "18110206";
            }
            return "0";
        }
    }
}
