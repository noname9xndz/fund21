using AutoMapper;
using smartFunds.Data.Migrations;
using smartFunds.Model.Common;
using smartFunds.Service.Models;

namespace smartFunds.Service.Mapper
{
    public class ServiceProfile : Profile
    {
        public ServiceProfile()
        {            
            CreateMap<Data.Models.HangFire.Job, Models.HangFire.Job>();           
            CreateMap<Data.Models.Test, Model.Common.TestModel>();
            CreateMap<Model.Common.TestModel, Data.Models.Test>();
            CreateMap<Data.Models.FundPurchaseFee, Model.Common.FundPurchaseFeeModel>();
            CreateMap<Model.Common.FundPurchaseFeeModel, Data.Models.FundPurchaseFee>();
            CreateMap<Data.Models.FundSellFee, Model.Common.FundSellFeeModel>();
            CreateMap<Model.Common.FundSellFeeModel, Data.Models.FundSellFee>();
            CreateMap<Data.Models.Order, Model.Common.OrderModel>();
            CreateMap<Model.Common.OrderModel, Data.Models.Order>();
            CreateMap<Data.Models.OrderRequest, Model.Common.OrderRequestModel>();
            CreateMap<Model.Common.OrderRequestModel, Data.Models.OrderRequest>();
            CreateMap<Data.Models.User, Model.Common.UserModel>();
            CreateMap<Model.Common.UserModel, Data.Models.User>();
            CreateMap<Data.Models.FAQ, Model.Common.FAQModel>();
            CreateMap<Model.Common.FAQModel, Data.Models.FAQ>();
            CreateMap<Data.Models.KVRRQuestion, Model.Common.KVRRQuestion>();
            CreateMap<Model.Common.KVRRQuestion, Data.Models.KVRRQuestion>();
            CreateMap<Data.Models.KVRRAnswer, Model.Common.KVRRAnswer>();
            CreateMap<Model.Common.KVRRAnswer, Data.Models.KVRRAnswer>();
            CreateMap<Data.Models.KVRR, Model.Common.KVRRModel>();
            CreateMap<Model.Common.KVRRModel, Data.Models.KVRR>();
            CreateMap<Data.Models.KVRRMark, Model.Common.KVRRMarkModel>();
            CreateMap<Model.Common.KVRRMarkModel, Data.Models.KVRRMark>();
            CreateMap<Data.Models.Portfolio, Model.Common.PortfolioModel>();
            CreateMap<Model.Common.PortfolioModel, Data.Models.Portfolio>();
            CreateMap<Data.Models.KVRRPortfolio, Model.Common.KVRRPortfolioModel>();
            CreateMap<Model.Common.KVRRPortfolioModel, Data.Models.KVRRPortfolio>();
            CreateMap<Data.Models.TransactionHistory, Model.Common.TransactionHistoryModel>();
            CreateMap<Model.Common.TransactionHistoryModel, Data.Models.TransactionHistory>();
            CreateMap<Data.Models.InvestmentTarget, Model.Common.InvestmentTargetModel>();
            CreateMap<Model.Common.InvestmentTargetModel, Data.Models.InvestmentTarget>();
            CreateMap<Data.Models.AdminTask, Model.Common.TaskModel>();
            CreateMap<Model.Common.TaskModel, Data.Models.AdminTask>();
            CreateMap<Data.Models.InvestmentTargetSetting, Model.Common.InvestmentTargetSettingModel>();
            CreateMap<Model.Common.InvestmentTargetSettingModel, Data.Models.InvestmentTargetSetting>();
            CreateMap<Data.Models.Fund, Model.Common.FundModel>();
            CreateMap<Model.Common.FundModel, Data.Models.Fund>()
                .ForMember(x => x.UserFunds, opt => opt.Ignore());
            CreateMap<Data.Models.ContactCMS, Model.Common.ContactCMSModel>();
            CreateMap<Model.Common.ContactCMSModel, Data.Models.ContactCMS>();
            CreateMap<Data.Models.Investment, Model.Common.InvestmentModel>();
            CreateMap<Model.Common.InvestmentModel, Data.Models.Investment>();
            CreateMap<Data.Models.WithdrawalFee, Model.Common.WithdrawFeeModel>();
            CreateMap<Model.Common.WithdrawFeeModel, Data.Models.WithdrawalFee>();

            CreateMap<Data.Models.CustomerLevel, Model.Common.CustomerLevelModel>();// phuongnc
            CreateMap<Model.Common.CustomerLevelModel, Data.Models.CustomerLevel>();
            CreateMap<Data.Models.TaskCompleted, Model.Common.TaskCompletedModel>();// phuongnc
            CreateMap<Model.Common.TaskCompletedModel, Data.Models.TaskCompleted>();
        }
    }
}
