const Wallet = {
  setHeight: () => {
    const headerUser = $('.headerUser').outerHeight();
    const footerUser = $('.footerUser').outerHeight();
    // eslint-disable-next-line camelcase
    const min_heigt = $(window).height() - headerUser - footerUser;
    $('.body-content-endUser').css('min-height', min_heigt);
  }
};
export default Wallet;
