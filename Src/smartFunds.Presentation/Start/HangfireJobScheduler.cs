using Hangfire;
using smartFunds.Business;
using smartFunds.Presentation.Controllers.Admin;
using smartFunds.Presentation.Job;

namespace smartFunds.Presentation.Start
{
    public class HangfireJobScheduler
    {
        public static void ScheduleRecurringJobs()
        {
            RecurringJob.AddOrUpdate<FundTransactionHistoryManager>("UpdateAccountAmountByFee", s => s.UpdateAccountAmountByFee(), Cron.Monthly(1,0));
            RecurringJob.AddOrUpdate<TaskController>("BatchUpdateDealFund", s => s.BatchUpdateDealFund(), Cron.Daily(19));
            RecurringJob.AddOrUpdate<HangfireJob>("CheckOrderRequestPendingTask", s => s.CheckOrderRequestPendingTask(), Cron.MinuteInterval(20));
        }
    }
}
