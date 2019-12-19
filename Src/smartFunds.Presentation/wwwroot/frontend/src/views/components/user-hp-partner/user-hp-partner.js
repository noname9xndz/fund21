const SlidePartner = {
  showSlidePartnerHomepageUser: () => {
    $('.user-hp-partner .slide-contents').slick({
      mobileFirst: true,
      arrows: false,
      dots: false,
      infinite: false,
      speed: 300,
      slidesToShow: 2,
      slidesToScroll: 1,
      responsive: [
        {
          breakpoint: 768,
          settings: {
            slidesToShow: 3,
            slidesToScroll: 1
          }
        },
        {
          breakpoint: 769,
          settings: {
            arrows: true,
            slidesToShow: 4,
            slidesToScroll: 1
          }
        }
      ]
    });
  }
};
export default SlidePartner;
