using AutoMapper;
using smartFunds.Business.Common;
using smartFunds.Common;
using smartFunds.Data.Models;
using smartFunds.Model.Common;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace smartFunds.Service.Services
{
    public interface ITaskService
    {
        Task<List<TaskApproveModel>> GetTasksForAdmin(int pageSize, int pageIndex);
        Task<TaskApproveModel> GetTaskById(int idTask, TaskApproveAdmin typeTask);
        Task<TasksModel> GetTasks(int pageSize, int pageIndex, SearchTask searchModel = null);
        Task UpdateTask(int taskId);
        Task<byte[]> ExportPendingTasks(SearchTask searchModel = null);
        Task<byte[]> ExportCompletedTasks(SearchTask searchModel = null);
    }

    public class TaskService : ITaskService
    {
        private readonly IMapper _mapper;
        private readonly IFundService _fund;
        private readonly IPortfolioService _portfolio;
        private readonly ITaskManager _taskManager;
        public TaskService(IMapper mapper, ITaskManager taskManager, IFundService fund, IPortfolioService portfolio)
        {
            _mapper = mapper;
            _taskManager = taskManager;
            _fund = fund;
            _portfolio = portfolio;
        }

        public async Task<List<TaskApproveModel>> GetTasksForAdmin(int pageSize, int pageIndex)
        {
            var listTasksModel = new List<TaskApproveModel>();
            var taskFunds = await _fund.GetFundByStatus(EditStatus.Updating);
            var taskPortfolios = await _portfolio.GetPortfolioByStatus(EditStatus.Updating);
            var infoFund = taskFunds.Find(x => x.LastUpdatedBy != null);
            if (taskFunds != null && taskFunds.Count > 0)
            {
                var task = new TaskApproveModel();
                task.IdTask = infoFund != null ? infoFund.Id: 0;
                task.NameTask = Model.Resources.Common.ApprovedNAVName;
                task.TaskType = TaskApproveAdmin.Nav;
                task.DateLastUpdated = infoFund != null ? infoFund.DateLastUpdated : DateTime.MinValue;
                task.LastUpdatedBy = infoFund?.LastUpdatedBy;
                listTasksModel.Add(task);
            }
            if (taskPortfolios != null && taskPortfolios.Count > 0)
                foreach (var portfolio in taskPortfolios)
                {
                    var portfolioFund = _portfolio.GetPortfolioFundByPortfolioId(portfolio.Id, EditStatus.Updating).Result;
                    var infoPortfolio = portfolioFund.Find(x => x.LastUpdatedBy != null);
                    if (portfolioFund.Count > 0)
                    {
                        var task = new TaskApproveModel();
                        task.IdTask = portfolio.Id;
                        task.NameTask = portfolio.Title;
                        task.TaskType = TaskApproveAdmin.Portfolio;
                        task.DateLastUpdated = infoPortfolio != null ? infoPortfolio.DateLastUpdated : DateTime.MinValue;
                        task.LastUpdatedBy = infoPortfolio?.LastUpdatedBy;
                        listTasksModel.Add(task);
                    }
                }
            listTasksModel = listTasksModel.OrderBy(x => x.DateLastUpdated).ToList();
            return listTasksModel;
        }

        public async Task<TasksModel> GetTasks(int pageSize, int pageIndex, SearchTask searchModel = null)
        {
            var tasksModel = new TasksModel();
            var tasks = await _taskManager.GetTasks(pageSize, pageIndex, searchModel);
            tasksModel.Tasks = _mapper.Map<List<AdminTask>, List<TaskModel>>(tasks);
            return tasksModel;
        }

        public async Task<TaskApproveModel> GetTaskById(int idTask, TaskApproveAdmin typeTask)
        {
            var tasksModel = new TaskApproveModel();
            if (typeTask == TaskApproveAdmin.Nav)
            {
                var taskFunds = await _fund.GetFundByStatus(EditStatus.Updating);
                var infoFund = taskFunds.Find(x => x.LastUpdatedBy != null);

                if (taskFunds != null && taskFunds.Count > 0)
                {
                    var taskFund = taskFunds.FirstOrDefault(m => m.Id == idTask);
                    if (taskFund != null)
                    {
                        tasksModel.IdTask = taskFund.Id;
                        tasksModel.NameTask = taskFund.Title;
                        tasksModel.TaskType = typeTask;
                        tasksModel.DateLastUpdated = infoFund != null ? infoFund.DateLastUpdated : DateTime.MinValue;
                        tasksModel.LastUpdatedBy = infoFund?.LastUpdatedBy;
                    }
                }
            }
            else if (typeTask == TaskApproveAdmin.Portfolio)
            {
                var taskPortfolios = await _portfolio.GetPortfolioByStatus(EditStatus.Updating);
                if (taskPortfolios != null && taskPortfolios.Count > 0)
                {
                    var taskPortfolio = taskPortfolios.FirstOrDefault(m => m.Id == idTask);
                    if (taskPortfolio != null)
                    {
                        var portfolioFund = _portfolio.GetPortfolioFundByPortfolioId(taskPortfolio.Id, EditStatus.Updating).Result;
                        var infoPortfolio = portfolioFund.Find(x => x.LastUpdatedBy != null);

                        if (portfolioFund.Count > 0)
                        {
                            tasksModel.IdTask = taskPortfolio.Id;
                            tasksModel.NameTask = taskPortfolio.Title;
                            tasksModel.TaskType = TaskApproveAdmin.Portfolio;
                            tasksModel.DateLastUpdated = infoPortfolio != null ? infoPortfolio.DateLastUpdated : DateTime.MinValue;
                            tasksModel.LastUpdatedBy = infoPortfolio?.LastUpdatedBy;
                        }
                    }
                }
            }

            return tasksModel;
        }

        public async Task UpdateTask(int taskId)
        {
            try
            {
                await _taskManager.UpdateTask(taskId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<byte[]> ExportPendingTasks(SearchTask searchModel = null)
        {
            return await _taskManager.ExportPendingTasks(searchModel);
        }

        public async Task<byte[]> ExportCompletedTasks(SearchTask searchModel = null)
        {
            return await _taskManager.ExportCompletedTasks(searchModel);
        }
    }
}
