using smartFunds.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace smartFunds.Data.Models
{
    public class TaskCompleted
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public TaskTypeAccountant TaskType { get; set; }

        public int ObjectID { get; set; }

        public string ObjectName { get; set; }

        public decimal TransactionAmount { get; set; }

        public DateTime LastUpdatedDate { get; set; }

        public string LastUpdatedBy { get; set; }
    }
}
