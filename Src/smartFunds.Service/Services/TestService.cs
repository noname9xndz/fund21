using AutoMapper;
using smartFunds.Business;
using smartFunds.Data.Models;
using smartFunds.Model.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace smartFunds.Service.Services
{
    public interface ITestService
    {
        Task<string> GetTest(int? testId);
        Task<TestModel> SaveTest(TestModel testModel);
    }
    public class TestService : ITestService
    {
        private readonly IMapper _mapper;
        private readonly ITestManager _testManager;
        public TestService(IMapper mapper, ITestManager testManager)
        {
            _mapper = mapper;
            _testManager = testManager;
        }
        public async Task<string> GetTest(int? testId)
        {
            if (testId == null)
            {
                return string.Empty;
            }
            string test = await _testManager.GetTest(testId);
            return test;
        }

        public async Task<TestModel> SaveTest(TestModel testModel)
        {
            Test test = _mapper.Map<Test>(testModel);
            Test savedTest = await _testManager.SaveTest(test);
            TestModel savedTestModel = _mapper.Map<TestModel>(savedTest);
            return savedTestModel;
        }
    }
}
