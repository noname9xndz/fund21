using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using smartFunds.Common.Exceptions;
using smartFunds.Data;
using smartFunds.Data.Models;
using smartFunds.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace smartFunds.Business.Common
{
    public interface IIntroducingSettingManager
    {
        GenericIntroducingSetting GetSetting();

        GenericIntroducingSetting UpdateIntroducingSetting(GenericIntroducingSetting GenericIntroducingSetting, int isDeleteMobile, int isDeleteDesktop);

        Task AddDefault(GenericIntroducingSetting GenericIntroducingSetting);
    }

    public class IntroducingPageCMSManager : IIntroducingSettingManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly smartFundsDbContext _smartFundsDbContext;

        public IntroducingPageCMSManager(IUnitOfWork unitOfWork, IHostingEnvironment hostingEnvironment, smartFundsDbContext smartFundsDbContext)
        {
            _unitOfWork = unitOfWork;
            _hostingEnvironment = hostingEnvironment;
            _smartFundsDbContext = smartFundsDbContext;
        }

        public async Task AddDefault(GenericIntroducingSetting modelSetting)
        {
            try
            {
                UploadBanners(modelSetting);
                var addedModel = _unitOfWork.IntroducingPageCMSRepository.Add(modelSetting);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public GenericIntroducingSetting UpdateIntroducingSetting(GenericIntroducingSetting GenericIntroducingSetting, int isDeleteMobile, int isDeleteDesktop)
        {
            try
            {
                if (GenericIntroducingSetting == null) throw new InvalidParameterException();
                GenericIntroducingSetting data = new GenericIntroducingSetting();

                ////Check if image was existed then delete it.
                if(GenericIntroducingSetting.BannerFile != null || GenericIntroducingSetting.MobileBannerFile != null)
                    DeleteOldImage(GenericIntroducingSetting);
                data = UploadBanners(GenericIntroducingSetting, isDeleteMobile, isDeleteDesktop);

                data.Id = GenericIntroducingSetting.Id;
                data.Description = GenericIntroducingSetting.Description;

                _smartFundsDbContext.GenericIntroducingSettings.Update(data);
                _smartFundsDbContext.SaveChanges();
                //_unitOfWork.IntroducingPageCMSRepository.Update(data);
                //await _unitOfWork.SaveChangesAsync();
                return GenericIntroducingSetting;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private string GetFilePath(IFormFile image)
        {
            return $"{_hostingEnvironment.WebRootPath}{smartFunds.Common.Constants.IntroducingPageFolder.Path}{image.FileName}";
        }
        private GenericIntroducingSetting UploadBanners(GenericIntroducingSetting modelSetting, int isDeleteMobile = 0, int isDeleteDesktop = 0)
        {
            string orginalPath = $"{_hostingEnvironment.WebRootPath}{smartFunds.Common.Constants.IntroducingPageFolder.Path}";
            try
            {
                //Desktop image
                if(modelSetting.BannerFile != null)
                {
                    string newFilePath = orginalPath + modelSetting.BannerFile.FileName;
                    using (var stream = new FileStream(newFilePath, FileMode.Create))
                    {
                        modelSetting.BannerFile.CopyTo(stream);
                    }
                    modelSetting.Banner = modelSetting.BannerFile.FileName;
                }
                else
                {
                    //if isDeleteDesktop == 0 then do not delete that image from database
                    //else the image will be removed since Banner now is Null
                    //the same with mobile image below
                    if (isDeleteDesktop == 0)
                    {
                        modelSetting.Banner = GetSetting().Banner;
                    }
                    else
                    {
                        modelSetting.Banner = modelSetting.Banner;
                    }
                }


                //Mobile image
                if (modelSetting.MobileBannerFile != null)
                {
                    string newFilePath = orginalPath + modelSetting.MobileBannerFile.FileName;
                    using (var stream = new FileStream(newFilePath, FileMode.Create))
                    {
                        modelSetting.MobileBannerFile.CopyTo(stream);
                    }
                    modelSetting.MobileBanner = modelSetting.MobileBannerFile.FileName;
                }
                else
                {
                    if (isDeleteMobile == 0)
                    {
                        modelSetting.MobileBanner = GetSetting().MobileBanner;
                    }
                    else
                    {
                        modelSetting.MobileBanner = modelSetting.MobileBanner;
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return modelSetting;
        }

        public GenericIntroducingSetting GetSetting()
        {
            return _unitOfWork.IntroducingPageCMSRepository.GetSetting();
        }

        
        private void DeleteOldImage(GenericIntroducingSetting modelSetting)
        {
            var oldDesktopImagePath = $"{_hostingEnvironment.WebRootPath}{smartFunds.Common.Constants.IntroducingPageFolder.Path}{modelSetting.BannerFile}";
            var oldMobileImagePath = $"{_hostingEnvironment.WebRootPath}{smartFunds.Common.Constants.IntroducingPageFolder.Path}{modelSetting.MobileBannerFile}";
            try
            {
                if(oldDesktopImagePath != null && System.IO.File.Exists(oldDesktopImagePath))
                {
                    System.IO.File.Delete(oldDesktopImagePath);
                }
                if (oldMobileImagePath != null && System.IO.File.Exists(oldMobileImagePath))
                {
                    System.IO.File.Delete(oldMobileImagePath);
                }
                    
            }
            catch (IOException ioExp)
            {
                throw ioExp;
            }
        }
    }
}
