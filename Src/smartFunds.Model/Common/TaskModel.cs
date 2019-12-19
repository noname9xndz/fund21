using smartFunds.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace smartFunds.Model.Common
{
    public class TaskApproveModel
    {
        public int IdTask { get; set; }
        public string NameTask { get; set; }
        public TaskApproveAdmin TaskType { get; set; }
        public TaskStatusAdmin TaskStatus { get; set; }
        [Display(Name = "ContentMessage", ResourceType = typeof(Model.Resources.Common))]
        [Required(ErrorMessageResourceName = "FieldEmpty", ErrorMessageResourceType = typeof(Model.Resources.ValidationMessages))]
        public string ContentMessage { get; set; }
        public DateTime DateLastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }

    }
    public class TasksApproveModel : PagingModel
    {
        public List<TaskApproveModel> Tasks { get; set; }

        public TasksApproveModel()
        {
            Tasks = new List<TaskApproveModel>();
        }
    }
    public class TaskCompletedModel
    {
        public int Id { get; set; }

        [Display(Name = "Type", ResourceType = typeof(Resources.Common))]
        public TaskTypeAccountant TaskType { get; set; }

        public int ObjectID { get; set; }

        public string ObjectName { get; set; }

        [Display(Name = "Amount", ResourceType = typeof(Resources.Common))]
        public decimal TransactionAmount { get; set; }

        [Display(Name = "LastUpdatedDate", ResourceType = typeof(Resources.Common))]
        public DateTime LastUpdatedDate { get; set; }

        public string LastUpdatedBy { get; set; }
    }

    public class TasksCompletedModel : PagingModel
    {
        public List<TaskCompletedModel> TasksCompleted { get; set; }

        public int TotalCount { get; set; }

        public TasksCompletedModel()
        {
            TasksCompleted = new List<TaskCompletedModel>();
            TotalCount = 0;
        }
    }

    public class TaskModel
    {
        public int Id { get; set; }

        [Display(Name = "Type", ResourceType = typeof(Resources.Common))]
        public TransactionTypeAdmin TransactionType { get; set; }

        public FundModel Fund { get; set; }

        [Display(Name = "Amount", ResourceType = typeof(Resources.Common))]
        public decimal TransactionAmount { get; set; }

        [Display(Name = "Status", ResourceType = typeof(Resources.Common))]
        public TransactionStatus Status { get; set; }

        [Display(Name = "LastUpdatedDate", ResourceType = typeof(Resources.Common))]
        public DateTime LastUpdatedDate { get; set; }
    }

    public class TasksModel : PagingModel
    {
        public List<TaskModel> Tasks { get; set; }

        public TasksModel()
        {
            Tasks = new List<TaskModel>();
        }
    }
}
