using Hangfire;
using smartFunds.Business;

namespace smartFunds.Presentation.Start
{
    public class HangfireJobScheduler
    {    
        public static void ScheduleRecurringJobs()
        {
            RecurringJob.AddOrUpdate<FundTransactionHistoryManager>("UpdateAccountAmountByFee", s => s.UpdateAccountAmountByFee(), Cron.Monthly(1));
        }        
    }
}
