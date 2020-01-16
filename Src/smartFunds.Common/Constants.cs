
namespace smartFunds.Common
{
    public static class Constants
    {
        public static class Database
        {
            public const string smartFundsConnectionStringName = "smartFundsDatabase";
            public const string ContactBaseConnectionStringName = "ContactBaseDatabase";
        }
        public static readonly string ESC = "\u001B";
        public static class CorsPolicy
        {
            public const string AllowAll = "AllowAll";
        }

        public static class RedisAutocomplete
        {
            public const string SetNameFormat = "AUTOC: {0}";
        }

        public static class Cache {
            public static class BuildKeys
            {
                public const string Status = "Build.Status";
                public const string Heartbeat = "Build.Heartbeat";
                public const string StartTime = "Build.StartTime";
                public const string EndTime = "Build.EndTime";
                public const string Error = "Build.Error";
            }

            public static class HeartbeatMessage
            {
                public const string StartsmartFunds = "Starting build smartFunds cache";
                public const string FinishsmartFunds = "Finished build smartFunds cache";
                public const string StartContactBase = "Starting build contactbase cache";
                public const string FinishContactBase = "Finished build smartFunds cache.EndTime";
            }
        }

        public static class DateTimeFormats
        {
            public const string IsoLongDateTime = "yyyy-MM-dd HH:mm:ss.ffffff";
        }

        public static class EventMemberRole
        {
            public const string AlreadyHost = "Already mark as host";
            public const string AlreadyGuest = "Already a guest";
            public const string Away = "Away";
            public const string PersonTba = "Person to be assigned";
            public const string AlreadyAway = "Already mark as away";
            public const string AlreadyTba = "Already mark as to be assigned";
            public const string AlreadyInOtherEvent = "Already Host / Guest on this date";
        }

        public static class Message
        {
            public const string UpdateTbaToHostError = "One or more Household member is assigned as Guest.";
            public const string UpdateGuestToHostWarnming = "Entire Household will be assigned as Host. Do you wish to Continue?";
            public const string SCPMustBiggerThanCP = "Host SCP must always bigger then Host CP";

            public const string FieldEmpty = "Dữ liệu trường {0} không được để trống";
            public const string DuplicateData = "Trùng dữ liệu";
            public const string CreateError = "Có lỗi trong quá trình tạo mới";
            public const string PasswordInvalid = "Mật khẩu phải có ít nhất 1 chữ số, 1 ký tự đặc biệt và ít nhất 9 ký tự";
        }

        public static class RoleName
        {
            public const string Admin = "admin";
            public const string CustomerManager = "customer manager";
            public const string InvestmentManager = "investment manager";
            public const string Accountant = "accountant";
            public const string Customer = "customer";
        }

        public static class KVRRImageFolder
        {
            public const string Path = "\\images\\kvrr\\";
        }
        public static class BannerHomepageFolder
        {
            public const string Path = "\\images\\BannerHomepageCMS\\";
        }
        public static class IntroducingPageFolder
        {
            public const string Path = "\\images\\IntroducingPageCMS\\";
        }
        public static class GeneralImageFolder
        {
            public const string Path = "\\images\\";
        }
        public static class KVRRQuestionAnswerFolder
        {
            public const string Path = "\\images\\KVRRQuestionAnswer\\";
        }
        public static class KVRRQuestionAnswerImageUrl
        {
            public const string Path = "/images/KVRRQuestionAnswer/";
        }
        public static class NewsImageFolder
        {
            public const string Path = "\\images\\news\\";
        }
        public static class FAQsCategory
        {
            public const string All = "Tất cả";
            public const string StartWithSaveNow = "Bắt đầu với SaveNow";
            public const string KVRR = "Khẩu vị rủi ro";
            public const string PortfolioFund = "Danh mục đầu tư";
            public const string InvestAndWithdraw = "Đầu tư và rút tiền";
        }
        public static class KVRRQuestionsCategory
        {
            public const string BigExpenses = "Các khoản chi phí lớn";
            public const string Discount = "Giảm giá";
            public const string Inflationary = "Lạm phát";
            public const string Investment = "Đầu tư";
            public const string LongTermInvestment = "Đầu tư dài hạn";
            public const string PersonalTime = "Thời gian cá nhân của bạn";
            public const string PriceFluctuations = "Biến động giá";
            public const string RiskVersusProfit = "Rủi ro so với lợi nhuận";
            public const string UnderstandingTheRisks = "Hiểu biết về các rủi ro";
        }

        public static class Configuration
        {
            public const string ProgramLocked = "ProgramLocked";
            public const string IsAdminApproving = "IsAdminApproving";
        }

        public static class FastWithdrawFee
        {
            public const double Fee = 0.0375;
        }

        public static class MaxDecimal
        {
            public const decimal MaxMoney = 999999999999;
        }
    }
}
