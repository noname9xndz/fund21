using System;
using System.Collections.Generic;
using smartFunds.Common.Exceptions;
using smartFunds.Data.Models;
using smartFunds.Data.UnitOfWork;
using System.Threading.Tasks;
using System.Linq;
using smartFunds.Model.Common;
using LinqKit;
using OfficeOpenXml;
using smartFunds.Common.Helpers;

namespace smartFunds.Business.Common
{
    public interface ITaskCompletedManager
    {
        Task<List<TaskCompleted>> GetTasksCompleted(int pageSize, int pageIndex, SearchTask searchModel = null);
        Task<List<TaskCompleted>> GetTasksCompleted(SearchTask searchModel = null);

        Task<TaskCompleted> SaveTaskCompleted(TaskCompleted taskCompleted);

        Task<byte[]> ExportTasksCompleted(SearchTask searchModel = null);
    }

    public class TaskCompletedManager : ITaskCompletedManager
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserManager _userManager;

        public TaskCompletedManager(IUnitOfWork unitOfWork, IUserManager userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<TaskCompleted> SaveTaskCompleted(TaskCompleted taskCompleted)
        {
            try
            {
                taskCompleted.LastUpdatedDate = DateTime.Now;
                taskCompleted.LastUpdatedBy = _userManager.CurrentUser();

                var savedTaskCompleted = _unitOfWork.TaskCompeletedRepository.Add(taskCompleted);
                await _unitOfWork.SaveChangesAsync();
                return savedTaskCompleted;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<byte[]> ExportTasksCompleted(SearchTask searchModel = null)
        {
            try
            {
                var comlumHeadrs = new string[]
                {
                    Model.Resources.Common.STT,
                    Model.Resources.Common.Deal,
                    Model.Resources.Common.Object,
                    Model.Resources.Common.DealMoney,
                   Model.Resources.Common.LastUpdatedDate
                };

                var predicate = SetPredicate(searchModel);

                var listTaskCompleted = (await _unitOfWork.TaskCompeletedRepository.GetAllAsync())
                                .Where(predicate).OrderByDescending(i => i.LastUpdatedDate).ToList();

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Tasks Completed");
                    using (var cells = worksheet.Cells[1, 1, 1, 5])
                    {
                        cells.Style.Font.Bold = true;
                    }

                    for (var i = 0; i < comlumHeadrs.Count(); i++)
                    {
                        worksheet.Cells[1, i + 1].Value = comlumHeadrs[i];
                    }

                    var j = 2;
                    foreach (var taskCompleted in listTaskCompleted)
                    {
                        worksheet.Cells["A" + j].Value = j - 1;
                        worksheet.Cells["B" + j].Value = taskCompleted.TaskType.GetDisplayName();
                        worksheet.Cells["C" + j].Value = taskCompleted.ObjectName;
                        worksheet.Cells["D" + j].Style.Numberformat.Format = "#,##0";
                        worksheet.Cells["D" + j].Value = taskCompleted.TransactionAmount;
                        worksheet.Cells["E" + j].Value = taskCompleted.LastUpdatedDate.ToString("dd/MM/yyyy HH:mm:ss");
                        j++;
                    }

                    return package.GetAsByteArray();
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Export Transaction History: " + ex.Message);
            }

        }

        public async Task<List<TaskCompleted>> GetTasksCompleted(int pageSize, int pageIndex, SearchTask searchModel = null)
        {   // for accountant
            if (pageSize < 1 || pageIndex < 1)
            {
                throw new InvalidParameterException();
            }
            try
            {
                var predicate = SetPredicate(searchModel);
                var listTaskCompleted = new List<TaskCompleted>();

                listTaskCompleted = (await _unitOfWork.TaskCompeletedRepository.GetAllAsync())
                                .Where(predicate).OrderBy(i => i.LastUpdatedDate).OrderByDescending(i => i.LastUpdatedDate)
                                .Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                return listTaskCompleted;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<TaskCompleted>> GetTasksCompleted(SearchTask searchModel = null)
        {   // for accountant
            try
            {
                var predicate = SetPredicate(searchModel);
                var listTaskCompleted = new List<TaskCompleted>();

                listTaskCompleted = (await _unitOfWork.TaskCompeletedRepository.GetAllAsync())
                                .Where(predicate).OrderByDescending(i => i.LastUpdatedDate).ToList();

                return listTaskCompleted;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private ExpressionStarter<TaskCompleted> SetPredicate(SearchTask searchTask)
        {
            var predicate = PredicateBuilder.New<TaskCompleted>(true);

            if (searchTask != null)
            {
                if (searchTask.CustomerName != 0)
                {
                    predicate = predicate.And(i => i.TaskType == searchTask.CustomerName);
                }
                if (!string.IsNullOrWhiteSpace(searchTask.AmountFrom) && Decimal.TryParse(searchTask.AmountFrom, out decimal amountFrom))
                {
                    predicate = predicate.And(i => Decimal.Round(i.TransactionAmount) >= Decimal.Round(amountFrom));
                }
                if (!string.IsNullOrWhiteSpace(searchTask.AmountTo) && Decimal.TryParse(searchTask.AmountTo, out decimal amountTo))
                {
                    predicate = predicate.And(i => Decimal.Round(i.TransactionAmount) <= Decimal.Round(amountTo));
                }
            }

            return predicate;
        }
    }
}
