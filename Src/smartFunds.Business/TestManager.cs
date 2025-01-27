﻿using smartFunds.Data.Models;
using smartFunds.Data.UnitOfWork;
using smartFunds.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace smartFunds.Business
{
    public interface ITestManager
    {
        Task<string> GetTest(int? testId);
        Task<Test> SaveTest(Test test);
    }
    public class TestManager : ITestManager
    {
        private readonly IUnitOfWork _unitOfWork;

        public TestManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<string> GetTest(int? testId)
        {
            if (testId == null)
            {
                return string.Empty;
            }
            var test = await _unitOfWork.TestRepository.GetAsync(m => m.Id == testId);
            if (test != null)
            {
                return test.Name;
            }
            return string.Empty;
        }

        public async Task<Test> SaveTest(Test test)
        {
            var savedTest = _unitOfWork.TestRepository.Add(test);
            await _unitOfWork.SaveChangesAsync();

            return savedTest;
        }
    }
}
