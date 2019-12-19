﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace smartFunds.Model.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class ValidationMessages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ValidationMessages() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("smartFunds.Model.Resources.ValidationMessages", typeof(ValidationMessages).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Bạn đang cố gắng đăng nhập quá nhiều lần. Vui lòng thử lại trong {0} giây..
        /// </summary>
        public static string AccountLockedOut {
            get {
                return ResourceManager.GetString("AccountLockedOut", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Tài khoản chưa đăng ký. Vui lòng nhập các thông tin bên dưới để đăng ký.
        /// </summary>
        public static string AccountNotRegister {
            get {
                return ResourceManager.GetString("AccountNotRegister", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Giá trị tài sản từ chưa điền.
        /// </summary>
        public static string AmountFromIsNotEmpty {
            get {
                return ResourceManager.GetString("AmountFromIsNotEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Giá trị tài sản đến chưa điền.
        /// </summary>
        public static string AmountToIsNotEmpty {
            get {
                return ResourceManager.GetString("AmountToIsNotEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Nội dung câu trả lời đang để trống.
        /// </summary>
        public static string AnswerContentIsEmpty {
            get {
                return ResourceManager.GetString("AnswerContentIsEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Phê duyệt thất bại !.
        /// </summary>
        public static string ApprovedError {
            get {
                return ResourceManager.GetString("ApprovedError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Đã phê duyệt thành công !.
        /// </summary>
        public static string ApprovedSuccess {
            get {
                return ResourceManager.GetString("ApprovedSuccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Danh mục đang để trống.
        /// </summary>
        public static string CategoryIsEmpty {
            get {
                return ResourceManager.GetString("CategoryIsEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Danh mục {0} không tồn tại.
        /// </summary>
        public static string CategoryIsNotExisted {
            get {
                return ResourceManager.GetString("CategoryIsNotExisted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Giá trị Tổng Tài Sản Từ phải nhỏ hơn giá trị Tổng Tài Sản Đến.
        /// </summary>
        public static string CompareAmountFromAmountTo {
            get {
                return ResourceManager.GetString("CompareAmountFromAmountTo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Trọng số từ phải nhỏ hơn trọng số đến.
        /// </summary>
        public static string CompareMarkFromMarkTo {
            get {
                return ResourceManager.GetString("CompareMarkFromMarkTo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Tháng Bắt Đầu phải nhỏ hơn Tháng Kết Thúc.
        /// </summary>
        public static string CompareMonthFromMonthTo {
            get {
                return ResourceManager.GetString("CompareMonthFromMonthTo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Mật khẩu nhập lại không khớp.
        /// </summary>
        public static string ConfirmPasswordInvalid {
            get {
                return ResourceManager.GetString("ConfirmPasswordInvalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Hiện tại không thể kết nối đến cổng thanh toán của ViettelPay, xin vui lòng thử lại sau.
        /// </summary>
        public static string ConnectVTPError {
            get {
                return ResourceManager.GetString("ConnectVTPError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Có lỗi trong quá trình tạo mới.
        /// </summary>
        public static string CreateError {
            get {
                return ResourceManager.GetString("CreateError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Mật khẩu hiện tại không đúng.
        /// </summary>
        public static string CurrentPasswordError {
            get {
                return ResourceManager.GetString("CurrentPasswordError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Không có khách hàng nào được chọn.
        /// </summary>
        public static string CustomerNotSelect {
            get {
                return ResourceManager.GetString("CustomerNotSelect", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Dữ liệu đang để trống.
        /// </summary>
        public static string DataIsEmpty {
            get {
                return ResourceManager.GetString("DataIsEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Có lỗi trong quá trình xóa khách hàng.
        /// </summary>
        public static string DeleteCustomerError {
            get {
                return ResourceManager.GetString("DeleteCustomerError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Xóa khách hàng thành công.
        /// </summary>
        public static string DeleteCustomerSuccess {
            get {
                return ResourceManager.GetString("DeleteCustomerSuccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Có lỗi trong quá trình xóa tài khoản.
        /// </summary>
        public static string DeleteUserError {
            get {
                return ResourceManager.GetString("DeleteUserError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Xóa tài khoản thành công.
        /// </summary>
        public static string DeleteUserSuccess {
            get {
                return ResourceManager.GetString("DeleteUserSuccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Trùng dữ liệu.
        /// </summary>
        public static string DuplicateData {
            get {
                return ResourceManager.GetString("DuplicateData", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Câu hỏi trong danh sách import trùng nhau.
        /// </summary>
        public static string DuplicatedQuestionImport {
            get {
                return ResourceManager.GetString("DuplicatedQuestionImport", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Có lỗi trong quá trình chỉnh sửa khách hàng.
        /// </summary>
        public static string EditCustomerError {
            get {
                return ResourceManager.GetString("EditCustomerError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Địa chỉ email đã được sử dụng.
        /// </summary>
        public static string EmailUsed {
            get {
                return ResourceManager.GetString("EmailUsed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Tài khoản này đã tồn tại.
        /// </summary>
        public static string ExistedAccount {
            get {
                return ResourceManager.GetString("ExistedAccount", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Khẩu vị rủi ro này đã được danh mục đầu tư khác sử dụng.
        /// </summary>
        public static string ExistedKVRRPortfolio {
            get {
                return ResourceManager.GetString("ExistedKVRRPortfolio", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Email đã được đăng ký.
        /// </summary>
        public static string ExistEmail {
            get {
                return ResourceManager.GetString("ExistEmail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Dữ liệu trường {0} không được để trống.
        /// </summary>
        public static string FieldEmpty {
            get {
                return ResourceManager.GetString("FieldEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Dữ liệu trường {0} không đúng định dạng.
        /// </summary>
        public static string FieldFormat {
            get {
                return ResourceManager.GetString("FieldFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to File được chọn đang không đúng định dạng.
        /// </summary>
        public static string FileIsWrongType {
            get {
                return ResourceManager.GetString("FileIsWrongType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Chúng tôi đã gửi cho bạn một email để thay đổi lại mật khẩu. Xin vui lòng kiểm tra email của bạn..
        /// </summary>
        public static string ForgotPasswordConfirm {
            get {
                return ResourceManager.GetString("ForgotPasswordConfirm", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Số bắt đầu không được nhỏ hơn 0.
        /// </summary>
        public static string FromInvalid {
            get {
                return ResourceManager.GetString("FromInvalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Mã quỹ không được để trống.
        /// </summary>
        public static string FundCodeIsNotEmpty {
            get {
                return ResourceManager.GetString("FundCodeIsNotEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Tên quỹ không được để trống.
        /// </summary>
        public static string FundNameIsNotEmpty {
            get {
                return ResourceManager.GetString("FundNameIsNotEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Tổng % phân bổ phải bằng 100.
        /// </summary>
        public static string FundPercent100 {
            get {
                return ResourceManager.GetString("FundPercent100", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to % phân bổ phải là kiểu số và lớn hơn 0.
        /// </summary>
        public static string FundPercentMustBeNumber {
            get {
                return ResourceManager.GetString("FundPercentMustBeNumber", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Import dữ liệu thành công.
        /// </summary>
        public static string ImportSuccessful {
            get {
                return ResourceManager.GetString("ImportSuccessful", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Số tiền đầu tư phải lớn hơn 0.
        /// </summary>
        public static string InvestInvalid {
            get {
                return ResourceManager.GetString("InvestInvalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Số tiền kì vọng không được thấp hơn {0}.
        /// </summary>
        public static string InvestmentTargetAmountInvalid {
            get {
                return ResourceManager.GetString("InvestmentTargetAmountInvalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Số tiền kì vọng không được thấp hơn số tiền đang có trong tài khoản.
        /// </summary>
        public static string InvestmentTargetAmountInvalid2 {
            get {
                return ResourceManager.GetString("InvestmentTargetAmountInvalid2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Số tiền kì vọng quá thấp so với số tiền đang có trong tài khoản.
        /// </summary>
        public static string InvestmentTargetAmountInvalid3 {
            get {
                return ResourceManager.GetString("InvestmentTargetAmountInvalid3", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Tên phân cấp khách hàng không được trùng.
        /// </summary>
        public static string IsCustomerLevelNameExists {
            get {
                return ResourceManager.GetString("IsCustomerLevelNameExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Mã quỹ không được trùng.
        /// </summary>
        public static string IsFundCodeExists {
            get {
                return ResourceManager.GetString("IsFundCodeExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Tên quỹ không được trùng.
        /// </summary>
        public static string IsFundNameExists {
            get {
                return ResourceManager.GetString("IsFundNameExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Tên khẩu vị rủi ro không được trùng.
        /// </summary>
        public static string IsKVRRNameExists {
            get {
                return ResourceManager.GetString("IsKVRRNameExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Tên danh mục đầu tư không được trùng.
        /// </summary>
        public static string IsPortfolioNameExists {
            get {
                return ResourceManager.GetString("IsPortfolioNameExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Câu hỏi không được trùng với câu hỏi đã có.
        /// </summary>
        public static string IsQuestionExisted {
            get {
                return ResourceManager.GetString("IsQuestionExisted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Nội dung khẩu vị rủi ro không được để trống.
        /// </summary>
        public static string KVRRDetailIsNotEmpty {
            get {
                return ResourceManager.GetString("KVRRDetailIsNotEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Khẩu vị rủi ro này đã có trọng số.
        /// </summary>
        public static string KVRRExistMark {
            get {
                return ResourceManager.GetString("KVRRExistMark", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Phải chọn khẩu vị rủi ro.
        /// </summary>
        public static string KVRRIsNotEmpty {
            get {
                return ResourceManager.GetString("KVRRIsNotEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Tên khẩu vị rủi ro không được để trống.
        /// </summary>
        public static string KVRRNameIsNotEmpty {
            get {
                return ResourceManager.GetString("KVRRNameIsNotEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Câu hỏi ít hơn 2 câu trả lời.
        /// </summary>
        public static string LessThan2Answers {
            get {
                return ResourceManager.GetString("LessThan2Answers", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Có lỗi trong quá trình đăng nhập. Vui lòng thử lại sau..
        /// </summary>
        public static string LoginError {
            get {
                return ResourceManager.GetString("LoginError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Sai tài khoản hoặc mật khẩu.
        /// </summary>
        public static string LoginFailed {
            get {
                return ResourceManager.GetString("LoginFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Giá trị trọng số không được nằm trong khoảng giá trị của các cấp trọng số khác.
        /// </summary>
        public static string MarkExisted {
            get {
                return ResourceManager.GetString("MarkExisted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Trọng số từ không được để trống.
        /// </summary>
        public static string MarkFromIsNotEmpty {
            get {
                return ResourceManager.GetString("MarkFromIsNotEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Giá trị trọng số từ phải là số nguyên dương lớn hơn hoặc bằng {1}.
        /// </summary>
        public static string MarkFromRange {
            get {
                return ResourceManager.GetString("MarkFromRange", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Trọng số phải là số nguyên dương.
        /// </summary>
        public static string MarkMustBeNumber {
            get {
                return ResourceManager.GetString("MarkMustBeNumber", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Trọng số hoặc câu trả lời chưa có dữ liệu.
        /// </summary>
        public static string MarkOrAnswerIsNull {
            get {
                return ResourceManager.GetString("MarkOrAnswerIsNull", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Trọng số đến không được để trống.
        /// </summary>
        public static string MarkToIsNotEmpty {
            get {
                return ResourceManager.GetString("MarkToIsNotEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Giá trị trọng số đến phải là số nguyên dương lớn hơn hoặc bằng {1}.
        /// </summary>
        public static string MarkToRange {
            get {
                return ResourceManager.GetString("MarkToRange", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Giá trị NAV phải &gt; 0.
        /// </summary>
        public static string MinNAV {
            get {
                return ResourceManager.GetString("MinNAV", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Có lỗi trong quá trình tạo mới mục tiêu đầu tư. Xin vui lòng thử lại sau.
        /// </summary>
        public static string NewInvestmentTargetError {
            get {
                return ResourceManager.GetString("NewInvestmentTargetError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Mật khẩu mới không được giống mật khẩu cũ.
        /// </summary>
        public static string NewPasswordError {
            get {
                return ResourceManager.GetString("NewPasswordError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to File không có dữ liệu.
        /// </summary>
        public static string NoData {
            get {
                return ResourceManager.GetString("NoData", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Chưa có File.
        /// </summary>
        public static string NoFile {
            get {
                return ResourceManager.GetString("NoFile", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Không tìm thấy khách hàng.
        /// </summary>
        public static string NotFoundCustomer {
            get {
                return ResourceManager.GetString("NotFoundCustomer", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Chỉ chấp nhận số nguyên dương.
        /// </summary>
        public static string OnlyPoitiveNumber {
            get {
                return ResourceManager.GetString("OnlyPoitiveNumber", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Sai mã OTP hoặc mã đã hết hạn.
        /// </summary>
        public static string OTPFailed {
            get {
                return ResourceManager.GetString("OTPFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Mật khẩu phải có ít nhất 1 chữ số, 1 ký tự đặc biệt và ít nhất 8 ký tự.
        /// </summary>
        public static string PasswordInvalid {
            get {
                return ResourceManager.GetString("PasswordInvalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Khoảng thời gian không hợp lệ..
        /// </summary>
        public static string PeriodInvalid {
            get {
                return ResourceManager.GetString("PeriodInvalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Số điện thoại đã được sử dụng.
        /// </summary>
        public static string PhoneNumberUsed {
            get {
                return ResourceManager.GetString("PhoneNumberUsed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Không có danh mục đầu tư khả dụng.
        /// </summary>
        public static string PortfolioIsNotAvailable {
            get {
                return ResourceManager.GetString("PortfolioIsNotAvailable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Nội dung câu hỏi đang để trống.
        /// </summary>
        public static string QuestionTitleIsEmpty {
            get {
                return ResourceManager.GetString("QuestionTitleIsEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Phí rút tiền nhanh không để trống.
        /// </summary>
        public static string QuickWithdrawalIsEmpty {
            get {
                return ResourceManager.GetString("QuickWithdrawalIsEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Khoảng giá trị không được nằm trong khoảng giá trị khác.
        /// </summary>
        public static string RangeAmountExisted {
            get {
                return ResourceManager.GetString("RangeAmountExisted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Khoảng thời gian không được nằm trong khoảng thời gian trước đó.
        /// </summary>
        public static string RangeMonthExisted {
            get {
                return ResourceManager.GetString("RangeMonthExisted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Quá trình đăng ký đã xảy ra lỗi. Xin vui lòng thử lại sau..
        /// </summary>
        public static string RegisterError {
            get {
                return ResourceManager.GetString("RegisterError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Có lỗi trong quá trình tạo lại mật khẩu. Xin vui lòng thử lại sau.
        /// </summary>
        public static string ResetPasswordError {
            get {
                return ResourceManager.GetString("ResetPasswordError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Câu hỏi {0} chưa chọn câu trả lời.
        /// </summary>
        public static string SelectAnswer {
            get {
                return ResourceManager.GetString("SelectAnswer", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Chưa chọn quỹ.
        /// </summary>
        public static string SelectFundEmpty {
            get {
                return ResourceManager.GetString("SelectFundEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Phải chọn 1 danh mục đầu tư.
        /// </summary>
        public static string SelectKVRR {
            get {
                return ResourceManager.GetString("SelectKVRR", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Hiện tại chúng tôi không thể gửi email ngay lúc này, vui lòng thử lại sau..
        /// </summary>
        public static string SendMailError {
            get {
                return ResourceManager.GetString("SendMailError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Thông tin của bạn đã được gửi đi. Chúng tôi sẽ liên hệ với bạn trong thời gian sớm nhất..
        /// </summary>
        public static string SendMailSucess {
            get {
                return ResourceManager.GetString("SendMailSucess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Hiện tại chúng tôi không thể gửi SMS ngay lúc này, vui lòng thử lại sau..
        /// </summary>
        public static string SendSMSError {
            get {
                return ResourceManager.GetString("SendSMSError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Trọng số đến phải lớn hơn trọng số từ.
        /// </summary>
        public static string String1 {
            get {
                return ResourceManager.GetString("String1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Thời gian thực hiện giao dịch không hợp lệ..
        /// </summary>
        public static string TimeActionInvalid {
            get {
                return ResourceManager.GetString("TimeActionInvalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Tiêu đề phải được nhập.
        /// </summary>
        public static string Title {
            get {
                return ResourceManager.GetString("Title", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Số kết thúc phải lớn hơn 0 hoặc bằng -1.
        /// </summary>
        public static string ToInvalid {
            get {
                return ResourceManager.GetString("ToInvalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Không có user nào được chọn.
        /// </summary>
        public static string UserNotSelect {
            get {
                return ResourceManager.GetString("UserNotSelect", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Có lỗi trong quá trình kết nối đến tài khoản ViettelPay, hãy thử lại sau.
        /// </summary>
        public static string VTPError {
            get {
                return ResourceManager.GetString("VTPError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Số điện thoại của bạn chưa đăng ký ViettelPay hoặc tên đầy đủ không đúng với tài khoản ViettelPay..
        /// </summary>
        public static string VTPInvalidAccount {
            get {
                return ResourceManager.GetString("VTPInvalidAccount", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Số điện thoại chưa đăng ký ViettelPay hoặc tên đầy đủ không đúng với tài khoản ViettelPay..
        /// </summary>
        public static string VTPInvalidAccount2 {
            get {
                return ResourceManager.GetString("VTPInvalidAccount2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Tài khoản của bạn đang trong quá trình rút tiền. Xin vui lòng đợi khi giao dịch hoàn tất..
        /// </summary>
        public static string WithdrawalError {
            get {
                return ResourceManager.GetString("WithdrawalError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Số tiền muốn rút không được nhỏ hơn {0}.
        /// </summary>
        public static string WithdrawalInvalid {
            get {
                return ResourceManager.GetString("WithdrawalInvalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Số tiền muốn rút (tính cả phí) không được lớn hơn số tiền trong tài khoản.
        /// </summary>
        public static string WithdrawalInvalid2 {
            get {
                return ResourceManager.GetString("WithdrawalInvalid2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Số tiền muốn rút không được vượt quá 90% số tiền trong tài khoản.
        /// </summary>
        public static string WithdrawalInvalid3 {
            get {
                return ResourceManager.GetString("WithdrawalInvalid3", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Chỉ được chọn file có đuôi .xlsx.
        /// </summary>
        public static string WrongExcelFile {
            get {
                return ResourceManager.GetString("WrongExcelFile", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Chỉ được chọn các file ảnh có đuôi sau đây: jpg,jpeg,png,gif.
        /// </summary>
        public static string WrongFileType {
            get {
                return ResourceManager.GetString("WrongFileType", resourceCulture);
            }
        }
    }
}
