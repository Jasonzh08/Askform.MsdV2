using Msd_V2.Zoho;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Msd_V2
{
    class Program
    {

        static Setting setting = new Setting();
        static Timer timer = new Timer()
        {
            Enabled = true,
            Interval = setting.IntervalSecond
        };


        static void Main(string[] args)
        {
            //NewAction._exec(159434107);
   
            //return;

            Console.WriteLine("启动成功");
            int Step = 1;

            timer.Elapsed += (n, m) =>
            {
                bool success = true;
                string errmsg = string.Empty;
                try
                {
                    //上海大通校招
                    if (Step == 1 || Step == 4)
                        XiaozhaoAction.GetApplicantIdsByDateAndStatus();
                    //上海大通社招
                    else if (Step == 2 || Step == 5)
                        ShezhaoAction.Exec();
                    //Zoho联系人读取
                    else if (Step == 3 || Step == 6)
                    {
                        NewAction.Exec();
                        //code
                        //GetZohoInfoAction.Exec();
                    }
                    //系统数据监测
                    else
                    {
                        //ToZohoAction.Exec();
                    }
                }
                catch (Exception ex) { success = false; errmsg = ex.Message; }
                Console.WriteLine(DateTime.Now + "执行步骤?:" + Step + "、通过?:" + success + "、错误消息?:" + errmsg);
                Step = Step == 6 ? 0 : Step;
                Step++;
            };
            Console.ReadKey();
        }


    }
}
