using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using smartFunds.Common.Exceptions;
using smartFunds.Data.Models;
using smartFunds.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace smartFunds.Business.Common
{
    public interface IHomepageCMSManager
    {
        List<HomepageCMS> GetAll();
        Task UpdateHomepageConfiguration(HomepageCMS homepageCMS,int category = 0, string typeUpload = "", int Id = 0);
        Task AddImageConfiguration(HomepageCMS homepageCMS, string typeUpload = "");
        Task Delete(int Id);

    }
    public class HomepageCMSManager : IHomepageCMSManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHostingEnvironment _hostingEnvironment;

        public HomepageCMSManager(IUnitOfWork unitOfWork, IHostingEnvironment hostingEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostingEnvironment = hostingEnvironment;
        }

        public List<HomepageCMS> GetAll()
        {
            return _unitOfWork.HomepageCMSRepository.GetAllHomepageConfiguration();
        }

        public async Task UpdateHomepageConfiguration(HomepageCMS homepageCMS, int category = 0, string typeUpload = "", int Id = 0)
        {
            try
            {
                if(homepageCMS == null)
                {
                    throw new InvalidParameterException();
                }
                if(homepageCMS.Banner?.Length > 0)
                {
                    homepageCMS = await UploadBanner(homepageCMS, typeUpload);
                }
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
            catch(Exception ex)
            {
                throw ex;
            }
        }
        private async Task<HomepageCMS> UploadBanner(HomepageCMS homepageCMS, string typeUpload = "")
        {
            if (homepageCMS.Banner?.Length > 0)
            {
                string suffix = "";
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

        
        public async Task Delete(int id)
        {
            try
            {
                var banner = await _unitOfWork.HomepageCMSRepository.GetAsync(q => q.Id == id);
                if (banner == null) throw new NotFoundException();
                _unitOfWork.HomepageCMSRepository.Delete(banner);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task AddImageConfiguration(HomepageCMS homepageCMS, string typeUpload = "")
        {
            try
            {
                if (homepageCMS == null) throw new NotFoundException();
                if (homepageCMS.Banner?.Length > 0)
                {
                    homepageCMS = await UploadBanner(homepageCMS, typeUpload);
                }
                _unitOfWork.HomepageCMSRepository.Add(homepageCMS);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
