using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace smartFunds.Common
{
    public enum ActiveStatus
    {
        None = 0,
        Active = 1,
        Inactive = 2
    }    

    public enum BuildCacheStatusType
    {
        [Description("INPROGRESS")]
        InProgress = 0,
        [Description("FINISHED")]
        Finished = 1,
        [Description("ERROR")]
        Error = 2,
        [Description("QUEUED")]
        Queued = 3,
    }

    public enum AutoCompleteType
    {
        Setting,
        Member,
        ContactBaseLocality,
        ContactBaseCountry
    }

    public enum LoginStatus
    {
        Succeeded,
        RequiresTwoFactor,
        IsLockedOut,
        NotVerify,
        Error,
        None
    }

    public enum RegisterStatus
    {
        Succeeded,
        ExistUser,
        NotVerify,
        None
    }

    public enum ResendCodeType
    {
        NotVerify,
        Expired
    }

    public enum FAQCategory
    {
        [Display(Name = "All", ResourceType = typeof(Resources.Enum))]
        All = 0,
        [Display(Name = "StartWithSaveNow", ResourceType = typeof(Resources.Enum))]
        StartWithSaveNow = 1,
        [Display(Name = "KVRR", ResourceType = typeof(Resources.Enum))]
        KVRR = 2,
        [Display(Name = "PortfolioFund", ResourceType = typeof(Resources.Enum))]
        PortfolioFund = 3,
        [Display(Name = "InvestAndWithdraw", ResourceType = typeof(Resources.Enum))]
        InvestAndWithdraw = 4
    }

    public enum TransactionStatus
    {
        [Display(Name = "All", ResourceType = typeof(Resources.Enum))]
        None = 0,
        [Display(Name = "TransactionStatusSuccess", ResourceType = typeof(Resources.Enum))]
        Success = 1,
        [Display(Name = "TransactionStatusProcessing", ResourceType = typeof(Resources.Enum))]
        Processing = 2
    }

    public enum TaskStatus
    {
        [Display(Name = "TaskStatusNew", ResourceType = typeof(Resources.Enum))]
        New = 0,
        [Display(Name = "TaskStatusWaiting", ResourceType = typeof(Resources.Enum))]
        Waiting = 1,
        [Display(Name = "TaskStatusProcessing", ResourceType = typeof(Resources.Enum))]
        Processing = 2,
        [Display(Name = "TaskStatusSuccess", ResourceType = typeof(Resources.Enum))]
        Success = 3
    }

    public enum EditStatus
    {
        Updating = 0,
        Success = 1
    }

    public enum TransactionType
    {
        [Display(Name = "All", ResourceType = typeof(Resources.Enum))]
        None = 0,
        [Display(Name = "TransactionTypeInvestment", ResourceType = typeof(Resources.Enum))]
        Investment = 1,
        [Display(Name = "TransactionTypeWithdrawal", ResourceType = typeof(Resources.Enum))]
        Withdrawal = 2,
        [Display(Name = "TransactionTypeAccountFee", ResourceType = typeof(Resources.Enum))]
        AccountFee = 3,
        [Display(Name = "WithdrawalFee", ResourceType = typeof(Resources.Enum))]
        WithdrawalFee = 4,
        [Display(Name = "FundPurchaseFee", ResourceType = typeof(Resources.Enum))]
        FundPurchaseFee = 5,
        [Display(Name = "FundSellFee", ResourceType = typeof(Resources.Enum))]
        FundSellFee = 6
    }

    public enum Duration
    {
        [Display(Name = "Duration3Month", ResourceType = typeof(Resources.Enum))]
        ThreeMonth = 3,
        [Display(Name = "Duration6Month", ResourceType = typeof(Resources.Enum))]
        SixMonth = 6,
        [Display(Name = "Duration12Month", ResourceType = typeof(Resources.Enum))]
        TwelfthMonth = 12,
        [Display(Name = "Duration2Year", ResourceType = typeof(Resources.Enum))]
        TwoYear = 24,
        [Display(Name = "Duration5Year", ResourceType = typeof(Resources.Enum))]
        FiveYear = 60
    }

    public enum Frequency
    {
        [Display(Name = "Frequency1Week", ResourceType = typeof(Resources.Enum))]
        OneWeek = 4,
        [Display(Name = "Frequency2Week", ResourceType = typeof(Resources.Enum))]
        TwoWeek = 2,
        [Display(Name = "Frequency1Month", ResourceType = typeof(Resources.Enum))]
        OneMonth = 1
    }

    public enum InvestmentMethod
    {
        [Display(Name = "InvestmentMethodManually", ResourceType = typeof(Resources.Enum))]
        Manually = 0,
        [Display(Name = "InvestmentMethodAuto", ResourceType = typeof(Resources.Enum))]
        Auto = 1
    }

    public enum RemittanceStatus
    {
        None = 0,
        Success = 1
    }

    public enum WithdrawalType
    {
        [Display(Name = "WithdrawalTypeQuick", ResourceType = typeof(Resources.Enum))]
        Quick = 0,
        [Display(Name = "WithdrawalTypeManually", ResourceType = typeof(Resources.Enum))]
        Manually = 1
    }

    public enum TransactionTypeAdmin
    {        
        [Display(Name = "TransactionTypeAdminInvestment", ResourceType = typeof(Resources.Enum))]
        Investment = 1,
        [Display(Name = "TransactionTypeAdminWithdrawal", ResourceType = typeof(Resources.Enum))]
        Withdrawal = 2
    }

    public enum TaskTypeAccountant
    {
        [Display(Name = "All", ResourceType = typeof(Resources.Enum))]
        None = 0,
        [Display(Name = "Buy", ResourceType = typeof(Resources.Enum))]
        Buy = 1,
        [Display(Name = "Sell", ResourceType = typeof(Resources.Enum))]
        Sell = 2,
        [Display(Name = "DealCustomer", ResourceType = typeof(Resources.Enum))]
        DealCustomer = 3
    }

    public enum TaskApproveAdmin
    {
        [Display(Name = "Portfolio", ResourceType = typeof(Resources.Enum))]
        Portfolio = 0,
        [Display(Name = "Nav", ResourceType = typeof(Resources.Enum))]
        Nav = 1
    }

    public enum TaskStatusAdmin
    {
        [Display(Name = "Rejected", ResourceType = typeof(Resources.Enum))]
        Rejected = 0,
        [Display(Name = "Approved", ResourceType = typeof(Resources.Enum))]
        Approved = 1
    }

    public enum DecimalLabel
    {
        [Display(Name = "Million", ResourceType = typeof(Resources.Enum))]
        Million = 0,
        [Display(Name = "Billion", ResourceType = typeof(Resources.Enum))]
        Billion = 1
    }

    public enum DateLabel
    {
        [Display(Name = "Day", ResourceType = typeof(Resources.Enum))]
        Day = 0,
        [Display(Name = "Month", ResourceType = typeof(Resources.Enum))]
        Month = 1,
        [Display(Name = "Year", ResourceType = typeof(Resources.Enum))]
        Year = 2
    }

    public enum FromLabel
    {
        [Display(Name = "From", ResourceType = typeof(Resources.Enum))]
        From = 0,
        [Display(Name = "Over", ResourceType = typeof(Resources.Enum))]
        Over = 1
    }
    public enum ToLabel
    {
        [Display(Name = "To", ResourceType = typeof(Resources.Enum))]
        To = 0,
        [Display(Name = "Under", ResourceType = typeof(Resources.Enum))]
        Under = 1
    }

    public static class EnumHelpers
    {
        public static T ToEnum<T>(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return default(T);
            return (T)Enum.Parse(typeof(T), value);
        }       
    }

    public enum FormState
    {
        Edit,
        Add,
        View
    }
    public enum KVRRQuestionCategories
    {
        [Display(Name = "BigExpenses", ResourceType = typeof(Resources.Enum))]
        BigExpenses = 0,
        [Display(Name = "Inflationary", ResourceType = typeof(Resources.Enum))]
        Inflationary = 1,
        [Display(Name = "Investment", ResourceType = typeof(Resources.Enum))]
        Investment = 2,
        [Display(Name = "PriceFluctuations", ResourceType = typeof(Resources.Enum))]
        PriceFluctuations = 3,
        [Display(Name = "RiskVersusProfit", ResourceType = typeof(Resources.Enum))]
        RiskVersusProfit = 4,
        [Display(Name = "Discount", ResourceType = typeof(Resources.Enum))]
        Discount = 5,
        [Display(Name = "UnderstandingTheRisks", ResourceType = typeof(Resources.Enum))]
        UnderstandingTheRisks = 6,
        [Display(Name = "PersonalTime", ResourceType = typeof(Resources.Enum))]
        PersonalTime = 7,
        [Display(Name = "LongTermInvestment", ResourceType = typeof(Resources.Enum))]
        LongTermInvestment = 8,
    }
}
