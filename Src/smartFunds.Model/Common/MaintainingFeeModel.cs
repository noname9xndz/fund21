using smartFunds.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace smartFunds.Model.Common
{
    public class MaintainingFeeModel
    {
        public int Id { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.ValidationMessages), ErrorMessageResourceName = "MarkFromIsNotEmpty")]
        public decimal AmountFrom { get; set; }
        public decimal AmountTo { get; set; }
        public decimal Percentage { get; set; }
        public FormState EntityState { get; set; }
    }

    public class ListFeesModel
    {
        public List<MaintainingFeeModel> ListFees { get; set; }
        public List<WithdrawFeeModel> ListWithdrawFees { get; set; }
        public ListFeesModel()
        {
            ListFees = new List<MaintainingFeeModel>();
            ListWithdrawFees = new List<WithdrawFeeModel>();
        }
    }

    public class ListMaintainingFee
    {
        public List<MaintainingFeeModel> ListFees { get; set; }
        public ListMaintainingFee()
        {
            ListFees = new List<MaintainingFeeModel>();
        }
        [NotMapped]
        public int ValidAmount { get; set; }
    }
    public class ListWithdrawFee
    {
        public List<WithdrawFeeModel> ListFees { get; set; }
        public WithdrawFeeModel QuickWithdrawFee { get; set; }
        public ListWithdrawFee()
        {
            ListFees = new List<WithdrawFeeModel>();
        }
    }

    //public class ListItem
    //{
    //    public List<int> ListIds { get; set; }

    //    public ListItem()
    //    {
    //        ListIds = new List<int>();
    //    }
    //}



}
