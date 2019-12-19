using AutoMapper;
using smartFunds.Business.Common;
using smartFunds.Data.Models;
using smartFunds.Model.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace smartFunds.Service.Services
{
    public interface ITaskCompletedService
    {
        Task<TasksCompletedModel> GetTasksCompleted(int pageSize, int pageIndex, SearchTask searchModel = null);

        Task<TaskCompletedModel> SaveTaskCompleted(TaskCompletedModel taskCompleted);

        Task<byte[]> ExportTasksCompleted(SearchTask searchModel = null);
    }

    public class TaskCompletedService : ITaskCompletedService
    {
        private readonly IMapper _mapper;
        private readonly ITaskCompletedManager _task;

        public TaskCompletedService(IMapper mapper, ITaskCompletedManager task)
        {
            _mapper = mapper;
            _task = task;

        }

        public async Task<TaskCompletedModel> SaveTaskCompleted(TaskCompletedModel taskCompleted)
        {
            try
            {
                var _taskCompleted = _mapper.Map<TaskCompleted>(taskCompleted);
                var savedTaskCompleted = await _task.SaveTaskCompleted(_taskCompleted);
                var outTaskCompleted = _mapper.Map<TaskCompletedModel>(savedTaskCompleted);

                return outTaskCompleted;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<TasksCompletedModel> GetTasksCompleted(int pageSize, int pageIndex, SearchTask searchModel = null)
        {   // for accountant
            var listTasksCompletedModel = new TasksCompletedModel();
            var listTaskCompletedModel = await _task.GetTasksCompleted(pageSize, pageIndex, searchModel);

            listTasksCompletedModel.TasksCompleted = _mapper.Map<List<TaskCompleted>, List<TaskCompletedModel>>(listTaskCompletedModel);
            listTasksCompletedModel.TotalCount = _task.GetTasksCompleted(searchModel).Result.Count;
            return listTasksCompletedModel;
        }

        public async Task<byte[]> ExportTasksCompleted(SearchTask searchModel = null)
        {
            return await _task.ExportTasksCompleted(searchModel);
        }
    }
}
