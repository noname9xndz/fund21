const SlideAboutUs = {
  showSlide: () => {
    if (window.innerWidth < 768) {
      $('.content-human .content-human__slide').not('.slick-initialized').slick({
        centerMode: true,
        centerPadding: '0px',
        slidesToShow: 1,
        variableWidth: true,
        dots: false,
        arrows: false
      });
    } else if ($('.content-human .content-human__slide').hasClass('slick-initialized')) {
      $('.content-human .content-human__slide').slick('unslick');
    }
  }
};
export default SlideAboutUs;
