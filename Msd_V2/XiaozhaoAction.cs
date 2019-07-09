using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Msd_V2
{
    public class XiaozhaoAction
    {

        private static string EmailContent = @"<div style='width:90%; margin:3rem auto;'>              <p>                  <a><span style='font-family: 宋体'>亲爱的</span> </a><span style='font-size: 13px; line-height: 107%; font-family: &#39; verdana&#39; ,sans-serif; background: yellow; background: yellow'>{UserName}</span><span style='font-family: 宋体'>：</span>              </p>              <p>                  &nbsp;              </p>              <p>                  <span style='font-family: 宋体'>你好！</span>              </p>              <p>                  <span style='font-family: 宋体'>恭喜你通过上汽大通校招的简历筛选环节。现邀请你进行上汽大通网上评估环节。</span>              </p>              <p>                  1. <span style='font-family: 宋体'>在线测试：测试前，请确保你的网络稳定，并处在一个相对安静的环境以避免中途被打扰。你的测评任务如下</span>:              </p>              <p>                  <a></a><a>a.</a><span style='font-family: 宋体'>逻辑推理</span> <a></a><a>–&nbsp; 6</a><span style='font-family: 宋体'>分钟</span>              </p>              <p>                  b.<span style='font-family: 宋体'>数字推理</span>–&nbsp; 6<span style='font-family: 宋体'>分钟</span>              </p>              <p>                  c.<span style='font-family: 宋体'>文字推理</span>–&nbsp; 6<span style='font-family: 宋体'>分钟</span>              </p>              <p>                  d.<span style='font-family: 宋体'>职业行为问卷</span> – <span style='font-family: 宋体'>约</span> 15<span style='font-family: 宋体'>分鈡</span>              </p>              <p>                  <a><span style='font-family: 宋体'>当你一切准备就绪，要进入测试。请点击</span>&nbsp; </a><a></a><a></a><a href='{LinkHref}'>{LinkHref}</a>              </p>              <p>                  &nbsp;              </p>              <p>                  2<span style='font-family: 宋体'>、注意事项：</span>              </p>              <p>                  <span style='font-family: 宋体'>（</span>1<span style='font-family: 宋体'>）请安装好</span>Flash<span style='font-family: 宋体'>插件，建议使用</span>chrome<span style='font-family: 宋体'>浏览器，并在稳定的网络环境中完成测试。若网速不稳定，此可能造成测试过程中断；</span>              </p>              <p>                  <span style='font-family: 宋体'>（</span>2<span style='font-family: 宋体'>）请独立完成全部测试；</span>              </p>              <p>                  <span style='font-family: 宋体'>（</span>3<span style='font-family: 宋体'>）登录后请仔细阅读说明，熟悉说明将帮助你更好的完成测试；</span>              </p>              <p>                  <span style='font-family: 宋体'>（</span>4<span style='font-family: 宋体'>）请安排好时间，一次性完成所有测试。</span>              </p>             <p style='background-color:yellow; font-weight:bold;'> <span style='font-family: 宋体'>（</span>5<span style='font-family: 宋体'>）测评截止时间：在宣讲会前完成测试。如果是宣讲会当场报名的同学，请在报名当天完成测试。</span>    </p>      <p style='background-color:yellow; font-weight:bold;'> <span style='font-family: 宋体'>（</span>6<span style='font-family: 宋体'>）如果测试中途中断，请不要紧张，重新点击测试链接即可再次进入测评。</span>              </p>               <p>                  &nbsp;              </p>              <p>                  <span style='font-family: 宋体'>如你未能在指定时间内完成全部测试，我们将无法获取你的任何测试结果。测试过程中如果遇到任何技术问题，请发邮件至</span> <a href='mailto:info.china@cut-e.com'>info.china@cut-e.com</a> <span style='font-family: 宋体'>咨询。技术小组将在</span>2<span style='font-family: 宋体'>小时内做出答复。</span>              </p>              <p>                  <span style='font-family: 宋体'>感谢你关注</span>2019<span style='font-family: 宋体'>上汽大通校园招聘。</span>              </p>              <p>                  &nbsp;              </p>              <p>                  <span style='font-size: 15px; line-height: 107%; font-family: 宋体'>上汽大通校招小组</span>              </p>              <p>                  <br />              </p>          </div>";
        /// <summary>
        /// 配置文件
        /// </summary>
        private static Setting setting = new Setting();

        /// <summary>
        /// 获取往前一个小时内所有没有处理的数据
        /// </summary>
        public static void GetApplicantIdsByDateAndStatus()
        {
            var stateTime = DateTime.Now.AddDays(-7).ToString("yyyyMMddhhmmss");
            var endtime = DateTime.Now.AddDays(1).ToString("yyyyMMddhhmmss");
            string Url = $@"http://api.beisenapp.com/RecruitV2/{setting.Tenant_ID}/applicant/GetApplicantIdsByDateAndStatus?start_time={stateTime}&end_time={endtime}&phase_id=S03&status_id=U01";

            var result = HttpHelper.HttpGet(Url);
            List<long> ids = JsonConvert.DeserializeObject<List<long>>(result);
            if (!ids.Any())
                return;

            ids.ForEach(x =>
            {
                GetApplicantsById(x.ToString());
                Console.WriteLine($"校招执行了ID为{x}的数据");
            });
        }

        /// <summary>
        /// 根据用户ID获取用户详细信息
        /// </summary>
        private static void GetApplicantsById(string IdsStr)
        {
            try
            {
                string Url = $@"http://api.beisenapp.com/RecruitV2/{setting.Tenant_ID}/Applicant/ById/{IdsStr}?language={1}&photo_base64={0}&has_Long={1}";
                var Result = HttpHelper.HttpGet(Url);
                if (string.IsNullOrEmpty(Result) || Result == "[]") { return; }

                bool SendAgn = false;
                SendAgn = Result.IndexOf("extshfxychffscp_102854_2003095649") > 0;

                Dictionary<string, string> ApplicantJobs = new Dictionary<string, string>();

                List<Applicant> AppList = JsonConvert.DeserializeObject<List<Applicant>>(Result);
                foreach (var app in AppList)
                {
                    var ItemObj = app.Profile.Items.Find(x => x.PropertyName == "Name");
                    if (ItemObj == null)
                    {
                        return;
                    }

                    var FirstName = ItemObj.Value.Substring(0, 1);
                    var LastName = ItemObj.Value.Substring(1);
                    var EmailObj = app.Profile.Items.Find(x => x.PropertyName == "email");
                    if (EmailObj == null)
                    {
                        Console.WriteLine(FirstName + LastName + "未填写邮箱");
                        return;
                    }
                    var Email = EmailObj.Value;

                    //如果有重新发送字段 则不新添加数据  更改邮件队列的状态
                    if (SendAgn)
                    {
                        AddEmailSendList(0, Email, string.Empty, string.Empty, true);
                        continue;
                    }

                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    var responese = new MaptqCN.ws().runWSobj(new MaptqCN.wsRequestObj()
                    {
                        CandidateId = app.CadidateId,
                        LastName = FirstName,
                        FirstName = LastName,
                        ClientId = "10000010",
                        ProjectId = "10000382",
                        SecureCode = "f97c3ded78e44adf18bc35780916b6df",
                        ReturnUrl = "https://app.askform.cn/Custom/MSDReport/SHDTCallBack.ashx",
                        EmailAddress = Email,
                        RequestType = "page_candhub_autoregister",
                        LanguageId = 19,
                        NormsetId = "1060"
                        //GenderId = Genderid,
                        //InstrumentId = Instrumentid
                    });

                    //只有接口请求成功的时候才修改状态
                    if (responese.ErrorCode == 0)
                    {
                        app.ApplyJobSummaries.ForEach(x =>
                        {
                            ApplicantJobs.Add(app.ApplicantId, x.JobId);
                            ConToJobOrStoreDB(ApplicantJobs);
                            ApplicantJobs.Clear();
                        });

                        string jobid = app.ApplyJobSummaries[0].JobCustomFields.Find(x => x.PropertyName == "extJobSequence_102854_300134805").Code;

                        //添加Entry
                        var EntryID = AskForm_Entry_Insert();

                        AddValueForFieldName(EntryID, "ApplicantId", app.ApplicantId.ToString());
                        AddValueForFieldName(EntryID, "Name", ItemObj.Value);
                        AddValueForFieldName(EntryID, "CadidateId", app.CadidateId);
                        AddValueForFieldName(EntryID, "Email", Email);
                        AddValueForFieldName(EntryID, "JobId", jobid);

                        //加入邮件消息队列
                        AddEmailSendList(EntryID, Email, responese.RedirectUrl, ItemObj.Value, SendAgn);
                    }
                }
                //UpdateApplicantProfile(ApplicantJobs);
                //ConToJobOrStoreDB(ApplicantJobs);
            }
            catch (Exception)
            {
                return;
            }
        }

        /// <summary>
        /// 公用的更改状态
        /// </summary>
        static List<StringValueContainer> StringValueContainers = new List<StringValueContainer>()
        {
            new StringValueContainer() { PropertyName="extcpbg_102854_1037800854", Code="", Value="vone", PropertyShortName="cpbg" }
        };

        /// <summary>
        /// 更新简历状态  替换下列方法ConToJobOrStoreDB
        /// </summary>
        private static void UpdateApplicantProfile(Dictionary<string, string> ApplicantJobs)
        {
            var Url = $"http://api.beisenapp.com/RecruitV2/{setting.Tenant_ID}/applicant/updateapplicantprofile";
            foreach (var key in ApplicantJobs.Keys)
            {
                Dictionary<string, string> dicParameter = new Dictionary<string, string>() {
                    {"Tenant_id",setting.Tenant_ID},
                    {"applicant_id",key},
                    //{"job_id",ApplicantJobs[key]},
                    //{"phase_id","S03" },
                    //{"status_id","U02" }
                    {"user_id","0"},
                    {"field_values",JsonConvert.SerializeObject(StringValueContainers)}
                };
                string resultjson = HttpHelper.HttpPost(Url, dicParameter);
                Result result = JsonConvert.DeserializeObject<Result>(resultjson);
            }
        }

        private static void ConToJobOrStoreDB(Dictionary<string, string> ApplicantJobs)
        {
            var Url = $"http://api.beisenapp.com/recruitv2/{setting.Tenant_ID}/applicant/contojoborstorsDb?format=json";
            foreach (var key in ApplicantJobs.Keys)
            {
                Dictionary<string, string> dicParameter = new Dictionary<string, string>() {
                    //{"Tenant_id",setting.Tenant_ID},
                    {"applicant_ids",key},
                    {"job_id",ApplicantJobs[key]},
                    {"phase_id","S03" },
                    {"status_id","U02" }
                    //{"field_values",JsonConvert.SerializeObject(StringValueContainers)}
                };
                string resultjson = HttpHelper.HttpPost(Url, dicParameter);
                Result result = JsonConvert.DeserializeObject<Result>(resultjson);
            }
        }

        /// <summary>
        /// 将对象信息加入邮件消息队列
        /// </summary>
        /// <param name="a"></param>
        private static void AddEmailSendList(long EntryID, string Email, string TestHref, string UserName, bool SendAgn = false)
        {
            if (SendAgn)
            {
                var UpdateSQL = $"update AskForm_EmailSendList set Status=1 where Recipient='{Email}' and Isdeleted=0";
                DB.ExecuteNonQuery(UpdateSQL, true);
                return;
            }
            else
            {
                var ec = EmailContent.Replace("{LinkHref}", TestHref).Replace("{UserName}", UserName).Replace("'", "\"");
                string InsertSQL = $@"if not exists(select * from AskForm_EmailSendList where FormID={FormID} and CompanyID={CompanyID} and Recipient='{Email}')
                Begin
                   Insert into AskForm_EmailSendList(Title,Content,Recipient,CC,BCC,DisplayName,SMTPID,Attachment,SentCount,AllowedCount,Status,Result,EntryID,EmailServerUrlID,FormID,FormApplicationID,UserID,CompanyID,IsDeleted,StartTime,CreatedDate,ModifiedDate,SendPriority,IsIdleSend)
                    Values('测评通知','{ec}','{Email}','','','上汽大通',211,'',1,2,1,'',{EntryID},53,{FormID},{FormApplicationID},0,{CompanyID},0,GETDATE(),GETDATE(),GETDATE(),0,0)
                End";
                //string InsertSQL = $@" Insert into AskForm_EmailSendList(Title,Content,Recipient,CC,BCC,DisplayName,SMTPID,Attachment,SentCount,AllowedCount,Status,Result,EntryID,EmailServerUrlID,FormID,FormApplicationID,UserID,CompanyID,IsDeleted,StartTime,CreatedDate,ModifiedDate,SendPriority,IsIdleSend)
                //    Values('测评通知','恭喜您的简历已通过筛选.请点击链接进行进一步测评<a href=''{TestHref}''>{TestHref}<a>','{Email}','','','上汽大通',55,'',1,2,1,'',{EntryID},53,{FormID},{FormApplicationID},0,{CompanyID},0,GETDATE(),GETDATE(),GETDATE(),0,0)";
                DB.ExecuteNonQuery(InsertSQL, true);
            }
        }


        #region AskForm操作相关
        private static long FormID = 7645470001, FormApplicationID = 7331550001, CompanyID = 270829, FormPageID = 7661020001;

        /// <summary>
        /// 给Entry表添加数据
        /// </summary>
        private static long AskForm_Entry_Insert()
        {
            string SQL = $@"Insert Into AskForm_Entry
            (FormID,CityID,TotalPoint,AllowedVersion,Version,Duration,BigIntIP,FormPageID,SourceType,Status,Reason
            ,IsArchived,CreatedUserID,ModifiedUserID,FormApplicationID,CompanyID,IsDeleted,CreatedDate,ModifiedDate,SendStatus,SendDate) 
            Values
            ({FormID},0,0,0,0,0,0,{FormPageID},0,0,'',0,0,0,{FormApplicationID},{CompanyID},0,GETDATE(),GETDATE(),1,GETDATE())
            SELECT @@IDENTITY AS ID";
            var Result = Convert.ToInt64(DB.ExecuteScalar(SQL));
            return Result;
        }

        /// <summary>
        /// 为字段添加指定值
        /// </summary>
        /// <param name="FieldName">字段名称</param>
        /// <param name="Value">值</param>
        private static void AddValueForFieldName(long EntryID, string FieldName, string Value)
        {
            var dt = DB.ExecuteQuery($@"select * from AskForm_Field where  CompanyID={CompanyID} and Name='{FieldName}' and FormID={FormID}");
            var FieldID = dt.Rows[0]["FieldID"];

            DB.ExecuteNonQuery($@"if not exists(select * from AskForm_EntryText where CompanyID ={CompanyID}
            and FormID = {FormID} and EntryID = {EntryID} and IsDeleted = 0 and FieldID = { FieldID }) 
            Begin
                Insert Into AskForm_EntryText(EntryID, FieldID, Value, Point, FormID, FormApplicationID, CompanyID, IsDeleted, CreatedDate, ModifiedDate, CreatedUserID, ModifiedUserID)
                Values({EntryID},{FieldID},'{Value}',0,{FormID},{FormApplicationID},{CompanyID},0,getdate(),getdate(),0,0)
            End");
        }
        #endregion
    }
}
