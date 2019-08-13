using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Msd_V2.NewProject
{
    public class NewSetting
    {
        #region Email
        public string EmailContent = @"<p>
        <a><span style='font-family:宋体'>亲爱的</span> </a><span style='font-size:13px;line-height:107%;font-family:&#39;Verdana&#39;,sans-serif'>{UserName}</span><span style='font-family:宋体'>：</span>
        </p>
        
        <p>
            <span style='font-family:宋体'>您好！</span>
        </p>
        <p>
            <span style='font-family:宋体'>恭喜您通过汇顶科技招聘的简历筛选环节。现邀请您进行汇顶科技网上评估环节。</span>
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
        <p class='MsoListParagraph' style='margin-left:28px'>
            <span style='font-family:Wingdings'>l<span style='font:9px &#39;Times New Roman&#39;'>&nbsp; </span></span><span style='font-family:宋体'>认知能力问卷（共三项）</span> – <span style='font-family:宋体'>合共约</span> 20<span style='font-family:宋体'>分鈡</span>
        </p>
        <p>
            <a><span style='font-family:宋体'>当您一切准备就绪，要进入测试。请点击</span> &nbsp;<a href='{LinkHref}'>{LinkHref}</a>
        </p>
        <p>
            &nbsp;
        </p>
        <p>
            2<span style='font-family:宋体'>、注意事项：</span>
        </p>
        <p>
            <span style='font-family:宋体'>（</span>1<span style='font-family:宋体'>）建议使用火狐或</span>chrome<span style='font-family:宋体'>浏览器，并在稳定的网络环境中完成测试。若网速不稳定，此可能造成测试过程中断；</span>
        </p>
        <p>
            <span style='font-family:宋体'>（</span>2<span style='font-family:宋体'>）请独立完成全部测试；</span>
        </p>
        <p>
            <span style='font-family:宋体'>（</span>3<span style='font-family:宋体'>）登录后请仔细阅读说明，熟悉说明将帮助您更好的完成测试；</span>
        </p>
        <p>
            <span style='font-family: 宋体'>（</span>4</a><span style='font-family:宋体'>）测评截止时间：请在收到这封邮件后的48小时内完成测试；</span>;
        </p>
        <p>
            <span style='font-family: 宋体'>（</span>4</a><span style='font-family:宋体'>）请安排好时间，一次性完成所有测试</span>;
        </p>
        <p>
            <span style='font-family:宋体'>（</span>5<span style='font-family:宋体'>）测评截止时间：请在参加综合评估</span>/<span style='font-family:宋体'>面试前完成测试；</span>
        </p>
        <p>
            <span style='font-family:宋体'>（</span>6<span style='font-family:宋体'>）如无特殊情况，请勿半途退出测评，否则会导致测评被锁定。如果因为特殊情况导致某项测评被中断。请重新登陆您的测评，继续完成其他测试任务，XX小时后被锁定的那项测评会解锁，可重新点击进入完成测试。</span>
        </p>
        <p>
            <span style='font-family:宋体'>（</span>6<span style='font-family:宋体'>）如您未能在指定时间内完成全部测试，我们将无法获取您的任何测试结果。测试过程中如果遇到任何技术问题，请发邮件至cut-e.china.info@aon.com 咨询。</span>
        </p>
        <p>
            <span style='font-family:宋体'>感谢您关注汇顶科技招聘。</span>
        </p>
        <p style='color:#7100E9;'>
            <span style='font-family:宋体;'>汇顶科技</span><span style='font-family:宋体'>人力资源部</span>
        </p>
        <p>
            <br />
        </p>";

        #endregion

        public string Tenant_ID => "107964";

        public string Job_ID => "150100430";

        public string ClientId => "10000174";

        public string ProjectId => "10001337";

        public string SecureCode => "6016ecc7ddba7a90c697c49bfb32e806";

        public string ReturnUrl => "https://app.askform.cn/Custom/MSDReport/SHDTCallBack-v3.ashx";
        #region MyRegion
        //public   string ConstString => "Bearer";
        public string AccessToken => "6d6a4dff08cf4de68053ec175f30c157";
        //public   int IntervalSecond => 300000;
        //public   string ConnectionString => ConfigurationManager.ConnectionStrings["AskForm1"].ConnectionString;
        #endregion


        public long FormID => 8456720001;
        public long FormApplicationID => 8058030001;
        public int CompanyID => 280851;

        public long FormPageID => 0;

        #region 字段编号
        public long ApplicantIdFieldID => 74681490001;

        public long NameFieldID => 74680980001;

        public long CadidateIdFieldId => 74681500001;

        public long EmailFieldID => 74680970001;

        public long reportFieldID => 74681510001;

        public long scoreFieldID => 74681570001;

        public long ErrorMsg => 74681530001;

        public long JobIdFieldId => 74681520001;

        public long JobId2FieldID => 74681540001;

        public long NormsetIdFieldID => 74681550001;

        #endregion
    }
}
