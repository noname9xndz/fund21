using smartFunds.Model.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace smartFunds.Presentation.Models
{
    public class SettingCMSModel
    {
        public HomepageCMSModel HomepageCMSModel { get; set; }
        public GenericIntroducingSettingModel GenericIntroducingSettingModel { get; set; }
        public ContactCMSModel ContactCMSModel { get; set; }
        public ListInvestmentTargetSettingModel InvestmentTargetSettingModel { get; set; }
        public ListWithdrawFee ListWithdrawFee { get; set; }
        public ListMaintainingFee ListMaintainingFee { get; set; }
        public bool HasError { get; set; }
        

        public SettingCMSModel()
        {
            HomepageCMSModel = new HomepageCMSModel();
            GenericIntroducingSettingModel = new GenericIntroducingSettingModel();
            ContactCMSModel = new ContactCMSModel();
            InvestmentTargetSettingModel = new ListInvestmentTargetSettingModel();
            ListWithdrawFee = new ListWithdrawFee();
            ListMaintainingFee = new ListMaintainingFee();
            HasError = false;
        }
    }
}
