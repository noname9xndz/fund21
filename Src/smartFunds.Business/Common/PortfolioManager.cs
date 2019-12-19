using smartFunds.Data.UnitOfWork;
using smartFunds.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqKit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using OfficeOpenXml;
using smartFunds.Business.Common;
using smartFunds.Common;
using smartFunds.Common.Exceptions;
using smartFunds.Data.Models;
using smartFunds.Model.Common;

namespace smartFunds.Business
{
    public interface IPortfolioManager
    {
        IEnumerable<Portfolio> GetAllPortfolios(int pageSize, int pageIndex);
        Portfolio GetPortfolioById(int id);
        Task<IEnumerable<Portfolio>> GetAllPortfolio();
        Task<ICollection<Portfolio>> GetPortfolioByStatus(EditStatus status);
        Task<ICollection<PortfolioFund>> GetPortfolioFundByPortfolioId(int portfolioId, EditStatus status);
        Task<ICollection<PortfolioFund>> GetPortfolioFundByPortfolioId(int portfolioId);
        Task<IEnumerable<Portfolio>> GetPortfoliosUnUse();
        Task<Portfolio> Save(Portfolio portfolio);
        Task<Portfolio> Update(Portfolio portfolio);
        Task<byte[]> ExportPortfolio(SearchPortfolio searchPortfolio = null);
        Task SaveFunds(Portfolio portfolio);
        Task<bool> IsPortfolioNameExists(string Title, string initTitle);
        Task UpdatePortfolioImage(HomepageCMS homepageCMS, string typeUpload = "", int Id = 0);
        Task RejectedPortfolio(int idPortfolio);
    }
    public class PortfolioManager : IPortfolioManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserManager _userManager;
        private readonly IHostingEnvironment _hostingEnvironment;

        public PortfolioManager(IUnitOfWork unitOfWork, IUserManager userManager, IHostingEnvironment hostingEnvironment)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
        }

        public IEnumerable<Portfolio> GetAllPortfolios(int pageSize, int pageIndex)
        {
            if (pageSize < 0 || pageIndex < 0) throw new InvalidParameterException();
            if (pageSize == 0 && pageIndex == 0) return _unitOfWork.PortfolioRepository.GetAllPortfolio()?.ToList();

            return _unitOfWork.PortfolioRepository.GetAllPortfolio()?.Take(pageSize).Skip((pageIndex - 1) * pageSize).ToList();
        }

        public async Task<ICollection<Portfolio>> GetPortfolioByStatus(EditStatus status)
        {
            try
            {
                var listPortfolioFund = await _unitOfWork.PortfolioFundRepository.FindByAsync(m => m.EditStatus == status);
                if (listPortfolioFund == null) throw new NotFoundException();
                var currentPortfolioIds = listPortfolioFund.Select(x => x.PortfolioId).ToList();

                return await _unitOfWork.PortfolioRepository.FindByAsync(m => currentPortfolioIds.Contains(m.Id) && m.IsDeleted == false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ICollection<PortfolioFund>> GetPortfolioFundByPortfolioId(int portfolioId, EditStatus status)
        {
            try
            {
                var listPortfolioFund = await _unitOfWork.PortfolioFundRepository.FindByAsync(m => m.EditStatus == status && m.PortfolioId == portfolioId);
                if (listPortfolioFund == null) throw new NotFoundException();

                return listPortfolioFund;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ICollection<PortfolioFund>> GetPortfolioFundByPortfolioId(int portfolioId)
        {
            try
            {
                var listPortfolioFund = await _unitOfWork.PortfolioFundRepository.FindByAsync(m => m.PortfolioId == portfolioId);
                if (listPortfolioFund == null) throw new NotFoundException();

                return listPortfolioFund;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Portfolio GetPortfolioById(int id)
        {
            var result = _unitOfWork.PortfolioRepository.GetPortfolioById(id);

            return result;
        }

        public async Task<IEnumerable<Portfolio>> GetAllPortfolio()
        {
            return await _unitOfWork.PortfolioRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Portfolio>> GetPortfoliosUnUse()
        {
            var kvrrPortfolios = await _unitOfWork.KVRRPortfolioRepository.GetAllAsync();
            if (kvrrPortfolios == null || !kvrrPortfolios.Any())
                return await _unitOfWork.PortfolioRepository.GetAllAsync();

            var portfoliosUsing = kvrrPortfolios?.Select(x => x.PortfolioId)?.ToList();
            return await _unitOfWork.PortfolioRepository.FindByAsync(x => !portfoliosUsing.Contains(x.Id));
        }

        public async Task<Portfolio> Save(Portfolio portfolio)
        {
            try
            {
                if (portfolio == null) throw new InvalidParameterException();

                portfolio.DateLastUpdated = DateTime.Now;
                portfolio.LastUpdatedBy = _userManager.CurrentUser();

                portfolio = _unitOfWork.PortfolioRepository.Add(portfolio);

                await _unitOfWork.SaveChangesAsync();
                return portfolio;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Portfolio> Update(Portfolio portfolio)
        {
            try
            {
                if (portfolio == null) throw new InvalidParameterException();

                portfolio.DateLastUpdated = DateTime.Now;
                portfolio.LastUpdatedBy = _userManager.CurrentUser();

                _unitOfWork.PortfolioRepository.Update(portfolio);
                await _unitOfWork.SaveChangesAsync();

                return portfolio;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task RejectedPortfolio(int idPortfolio)
        {
            try
            {
                ICollection<PortfolioFund> _listPortfolioFundUpdate = _unitOfWork.PortfolioFundRepository.FindByAsync(m => m.PortfolioId == idPortfolio && m.EditStatus == EditStatus.Updating).Result;
                if (_listPortfolioFundUpdate == null) throw new NotFoundException();

                if (_listPortfolioFundUpdate.Count > 0)
                {
                    foreach (var item in _listPortfolioFundUpdate)
                    {
                        if (item.FundPercent == null || item.FundPercent <= 0)
                        {
                            _unitOfWork.PortfolioFundRepository.Delete(item);
                            await _unitOfWork.SaveChangesAsync();
                        }
                        else
                        {
                            item.EditStatus = EditStatus.Success;
                            item.FundPercentNew = null;
                            item.LastUpdatedBy = _userManager.CurrentUser();
                            item.DateLastUpdated = DateTime.Now;
                            _unitOfWork.PortfolioFundRepository.Update(item);
                            await _unitOfWork.SaveChangesAsync();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdatePortfolioImage(HomepageCMS homepageCMS, string typeUpload = "", int Id = 0)
        {
            try
            {
                if (homepageCMS == null)
                {
                    throw new InvalidParameterException();
                }
                if (homepageCMS.Banner?.Length > 0)
                {
                    homepageCMS = await UploadBanner(homepageCMS, typeUpload);
                }
                //var a = !_unitOfWork.HomepageCMSRepository.GetAllAsync().Result.ToList().Any(x => int.Parse(x.Category) == 2);
                if (Id == 0)
                {
                    _unitOfWork.HomepageCMSRepository.Add(homepageCMS);
                }
                else
                {
                    homepageCMS.Id = Id;
                    _unitOfWork.HomepageCMSRepository.Update(homepageCMS);
                }
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<HomepageCMS> UploadBanner(HomepageCMS homepageCMS, string typeUpload = "")
        {
            if (homepageCMS.Banner?.Length > 0)
            {
                string suffix = "" + DateTime.Now.ToFileTimeUtc();
                if (typeUpload != "")
                {
                    suffix = "mobile_";
                }
                var filePath = $"{_hostingEnvironment.WebRootPath}{smartFunds.Common.Constants.BannerHomepageFolder.Path}{suffix + homepageCMS.Banner.FileName}";

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await homepageCMS.Banner.CopyToAsync(stream);
                }
                homepageCMS.ImageName = suffix + homepageCMS.Banner.FileName;
            }
            return homepageCMS;
        }

        public async Task<byte[]> ExportPortfolio(SearchPortfolio searchPortfolio = null)
        {
            try
            {
                var comlumHeadrs = new string[]
                {
                    Model.Resources.Common.PortfolioName,
                    Model.Resources.Common.PortfolioContent,
                    Model.Resources.Common.KVRRName
                };

                var predicate = PredicateBuilder.New<Portfolio>(true);

                if (searchPortfolio != null)
                {
                    if (!string.IsNullOrWhiteSpace(searchPortfolio.PortfolioName))
                    {
                        predicate = predicate.And(u => !string.IsNullOrWhiteSpace(u.Title) && u.Title.Contains(searchPortfolio.PortfolioName));
                    }
                    if (!string.IsNullOrWhiteSpace(searchPortfolio.PortfolioContent))
                    {
                        predicate = predicate.And(u => !string.IsNullOrWhiteSpace(u.Content) && u.Content.Contains(searchPortfolio.PortfolioContent));
                    }
                    if (!string.IsNullOrWhiteSpace(searchPortfolio.KVRRName))
                    {
                        predicate = predicate.And(u => u.KVRRPortfolios != null
                                                       && u.KVRRPortfolios.Any()
                                                       && u.KVRRPortfolios.First().KVRR != null
                                                       && !string.IsNullOrWhiteSpace(u.KVRRPortfolios.First().KVRR.Name)
                                                       && u.KVRRPortfolios.First().KVRR.Name.Contains(searchPortfolio.KVRRName));
                    }
                }

                var listPortfolio = _unitOfWork.PortfolioRepository.GetAllPortfolio().Where(predicate).ToList();

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Portfolios");
                    using (var cells = worksheet.Cells[1, 1, 1, 3])
                    {
                        cells.Style.Font.Bold = true;
                    }

                    for (var i = 0; i < comlumHeadrs.Count(); i++)
                    {
                        worksheet.Cells[1, i + 1].Value = comlumHeadrs[i];
                    }

                    var j = 2;
                    foreach (var portfolio in listPortfolio)
                    {
                        worksheet.Cells["A" + j].Value = portfolio.Title;
                        worksheet.Cells["B" + j].Value = portfolio.Content;
                        worksheet.Cells["C" + j].Value = portfolio.KVRRPortfolios != null && portfolio.KVRRPortfolios.Any() ? portfolio.KVRRPortfolios.First().KVRR.Name : string.Empty;
                        j++;
                    }

                    return package.GetAsByteArray();
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Export Portfolio: " + ex.Message);
            }
        }

        public async Task SaveFunds(Portfolio portfolio)
        {
            try
            {
                if (portfolio == null) throw new InvalidParameterException();

                var old = await _unitOfWork.PortfolioFundRepository.FindByAsync(m => m.PortfolioId == portfolio.Id);
                if (old == null) throw new NotFoundException();

                if (portfolio.PortfolioFunds != null && portfolio.PortfolioFunds.Any())
                {
                    var percentList = portfolio.PortfolioFunds.ToList();
                    var lstPortfolioAdds = new List<PortfolioFund>();
                    var lstPortfolioUpdates = new List<PortfolioFund>();
                    var lstPortfolioDeletes = new List<PortfolioFund>();
                    foreach (var percent in percentList)
                    {
                        var _fundUpdate = _unitOfWork.PortfolioFundRepository.GetAsync(m => m.FundId == percent.FundId && m.PortfolioId == percent.PortfolioId).Result;

                        if (_fundUpdate == null)
                        {
                            var addPercent = new PortfolioFund()
                            {
                                FundId = percent.FundId,
                                DateLastUpdated = DateTime.Now,
                                EditStatus = EditStatus.Updating,
                                FundPercent = percent.FundPercent == null ? 0 : percent.FundPercent,
                                FundPercentNew = percent.FundPercentNew == null ? 0 : percent.FundPercentNew,
                                LastUpdatedBy = _userManager.CurrentUser(),
                                PortfolioId = percent.PortfolioId
                            };
                            if (!((percent.FundPercent == 0 && percent.FundPercentNew == 0) || (percent.FundPercent == null && percent.FundPercentNew == null) || (percent.FundPercent == 0 && percent.FundPercentNew == null) || (percent.FundPercent == null && percent.FundPercentNew == 0)))
                            {
                                lstPortfolioAdds.Add(addPercent);
                            }
                        }
                        else
                        {
                            if ((percent.FundPercent == 0 && percent.FundPercentNew == 0) || (percent.FundPercent == null && percent.FundPercentNew == null) || (percent.FundPercent == 0 && percent.FundPercentNew == null) || (percent.FundPercent == null && percent.FundPercentNew == 0))
                            {
                                lstPortfolioDeletes.Add(percent);
                            }
                            else
                            {
                                var newpercent = new PortfolioFund();
                                newpercent.FundId = percent.FundId;
                                newpercent.PortfolioId = percent.PortfolioId;
                                newpercent.EditStatus = EditStatus.Updating;
                                newpercent.DateLastUpdated = DateTime.Now;
                                newpercent.LastUpdatedBy = _userManager.CurrentUser();
                                newpercent.FundPercent = percent.FundPercent == null ? 0 : percent.FundPercent;
                                newpercent.FundPercentNew = percent.FundPercentNew == null ? 0 : percent.FundPercentNew;
                                lstPortfolioUpdates.Add(newpercent);
                            }
                        }
                    }

                    await _unitOfWork.SaveChangesAsync();

                    if (lstPortfolioUpdates.Count > 0) _unitOfWork.PortfolioFundRepository.BulkUpdate(lstPortfolioUpdates);
                    if (lstPortfolioAdds.Count > 0) _unitOfWork.PortfolioFundRepository.BulkInsert(lstPortfolioAdds);
                    if (lstPortfolioDeletes.Count > 0) _unitOfWork.PortfolioFundRepository.BulkDelete(lstPortfolioDeletes);
                    await _unitOfWork.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> IsPortfolioNameExists(string newName, string initTitle)
        {
            if (!string.IsNullOrEmpty(initTitle) && newName.Trim().Equals(initTitle))
                return true;
            var existPortfolioName = await _unitOfWork.PortfolioRepository.FindByAsync(x => x.Title.ToLower().Equals(newName.ToLower()));
            if (existPortfolioName != null && existPortfolioName.Any())
                return false;

            return true;
        }
    }
}
