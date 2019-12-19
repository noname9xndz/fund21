using smartFunds.Common.Exceptions;
using smartFunds.Data.Models;
using smartFunds.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using smartFunds.Common;
using smartFunds.Model.Common;
using LinqKit;
using OfficeOpenXml;
using smartFunds.Common.Helpers;

namespace smartFunds.Business.Common
{
    public interface ITaskManager
    {
        Task<List<AdminTask>> GetTasks(int pageSize, int pageIndex, SearchTask searchModel = null);
        Task UpdateTask(int taskId);
        Task<byte[]> ExportPendingTasks(SearchTask searchModel = null);
        Task<byte[]> ExportCompletedTasks(SearchTask searchModel = null);

    }
    public class TaskManager : ITaskManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserManager _userManager;

        public TaskManager(IUnitOfWork unitOfWork, IUserManager userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }        

        public async Task<List<AdminTask>> GetTasks(int pageSize, int pageIndex, SearchTask searchModel = null)
        {
            if (pageSize < 1 || pageIndex < 1)
            {
                throw new InvalidParameterException();
            }
            try
            {               
                IEnumerable<AdminTask> allTasks = (await _unitOfWork.TaskRepository.GetAllAsync("Fund"));
                List<AdminTask> tasks = allTasks.Take(pageSize).Skip((pageIndex - 1) * pageSize).ToList();
                return tasks;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdateTask(int taskId)
        {
            try
            {               
                var task = _unitOfWork.TaskRepository.GetAsync(m => m.Id == taskId).Result;
                if (task == null)
                {
                    throw new NotFoundException();
                }
                task.LastUpdatedDate = DateTime.Now;
                task.LastUpdatedBy = _userManager.CurrentUser();
                task.Status = TransactionStatus.Success;
                _unitOfWork.TaskRepository.Update(task);
                await _unitOfWork.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<byte[]> ExportPendingTasks(SearchTask searchModel = null)
        {
            try
            {
                var columnHeaders = new string[]
                {
                    Model.Resources.Common.Type,
                    Model.Resources.Common.Fund,                    
                    Model.Resources.Common.Amount                    
                };
             
                List<AdminTask> tasks = (await _unitOfWork.TaskRepository.GetAllAsync("Fund")).ToList();
                
                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add(Model.Resources.Common.PendingTasks);
                    using (var cells = worksheet.Cells[1, 1, 1, 3])
                    {
                        cells.Style.Font.Bold = true;
                    }

                    for (var i = 0; i < columnHeaders.Count(); i++)
                    {
                        worksheet.Cells[1, i + 1].Value = columnHeaders[i];
                    }

                    var j = 2;
                    foreach (var task in tasks)
                    {
                        worksheet.Cells["A" + j].Value = task.TransactionType.GetDisplayName();
                        worksheet.Cells["B" + j].Value = task.Fund.Title;
                        worksheet.Cells["C" + j].Value = task.TransactionAmount;                        
                        j++;
                    }

                    return package.GetAsByteArray();
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Export Tasks: " + ex.Message);
            }
        }

        public async Task<byte[]> ExportCompletedTasks(SearchTask searchModel = null)
        {
            try
            {
                var columnHeaders = new string[]
                {
                    Model.Resources.Common.Type,
                    Model.Resources.Common.Fund,
                    Model.Resources.Common.Amount,
                    Model.Resources.Common.LastUpdatedDate
                };

                List<AdminTask> tasks = (await _unitOfWork.TaskRepository.GetAllAsync("Fund")).ToList();

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add(Model.Resources.Common.CompletedTasks);
                    using (var cells = worksheet.Cells[1, 1, 1, 4])
                    {
                        cells.Style.Font.Bold = true;
                    }

                    for (var i = 0; i < columnHeaders.Count(); i++)
                    {
                        worksheet.Cells[1, i + 1].Value = columnHeaders[i];
                    }

                    var j = 2;
                    foreach (var task in tasks)
                    {
                        worksheet.Cells["A" + j].Value = task.TransactionType.GetDisplayName();
                        worksheet.Cells["B" + j].Value = task.Fund.Title;
                        worksheet.Cells["C" + j].Value = task.TransactionAmount;
                        worksheet.Cells["D" + j].Value = task.LastUpdatedDate.ToString("dd/MM/yyyy HH:mm:ss");
                        j++;
                    }

                    return package.GetAsByteArray();
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Export Tasks: " + ex.Message);
            }
        }
    }
}
