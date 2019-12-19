/* eslint-disable func-names */
const HideLongText = {
  hiddenLongText: () => {
    if ($(window).width() < 768) {
      const minimizedElements = $('.userfund-kvrr p.minimize');
      if (minimizedElements) {
        minimizedElements.each(function () {
          const minimizedElementsLength = $(this).text();
          if (minimizedElementsLength.length < 190) return;
          $(this).html(
            `${minimizedElementsLength.slice(0, 190)}<span>... </span><a href="#" class="more">Xem thêm</a>`
              + `<span style="display:none;">${minimizedElementsLength.slice(190, minimizedElementsLength.length)} <a href="#" class="less">Thu gọn</a></span>`
          );
        });
        $('a.more', minimizedElements).click(function (event) {
          event.preventDefault();
          $(this).hide().prev().hide();
          $(this).next().show();
        });
        $('a.less', minimizedElements).click(function (event) {
          event.preventDefault();
          $(this).parent().hide().prev()
            .show()
            .prev()
            .show();
        });
      }
    }
  }
};
export default HideLongText;
