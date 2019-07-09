using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Msd_V2
{
    public class NewSetting
    {
        #region Email
        public static string EmailContent = @"<p>
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

        public static string ConstString { get { return "Bearer"; } }
        public static string Tenant_ID { get { return "102854"; } }
        public static string AccessToken { get { return "22645fc78af0436f85b15c56e90ee655"; } }

        public static int IntervalSecond { get { return 300000; } }
        public static string ConnectionString { get { return ConfigurationManager.ConnectionStrings["AskForm1"].ConnectionString; } }

        public static long FormID { get { return 8445370001; } }
        public static long FormApplicationID { get { return 7331550001; } }
        public static int CompanyID { get { return 270829; } }

        public static long FormPageID { get { return 0; } }

        #region 字段编号
        public static long ApplicantIdFieldID { get { return 74424670001; } }
        public static long NameFieldID { get { return 74424860001; } }
        public static long CadidateIdFieldID { get { return 74424870001; } }
        public static long EmailFieldID { get { return 74424880001; } }
        public static long ReportFieldID { get { return 74424890001; } }
        public static long ScoreFieldID { get { return 74424900001; } }
        public static long JobIdFieldID { get { return 74424910001; } }
        public static long JobId2FieldID { get { return 74424930001; } }

        public static long NormsetIdFieldID { get { return 74424940001; } }

        #endregion
    }
}
