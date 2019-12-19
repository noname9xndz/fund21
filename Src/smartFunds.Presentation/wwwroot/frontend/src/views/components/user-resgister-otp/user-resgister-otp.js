// eslint-disable-next-line camelcase
const userResgisterOTP = {
  setHeightOTP: () => {
    const headerHeight = $('.headerUser').outerHeight();
    const footerHeight = $('.footerUser').outerHeight();

    if ($(window).width() < 768) {
      // eslint-disable-next-line camelcase
      const min_height = $(window).height() - headerHeight - footerHeight;
      $('.resgiste-user').css('min-height', min_height);
    }
  }
};
// eslint-disable-next-line camelcase
export default userResgisterOTP;
