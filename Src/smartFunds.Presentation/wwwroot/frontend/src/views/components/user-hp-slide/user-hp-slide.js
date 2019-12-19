const Slide = {
  showSlideHomepageUser: () => {
    $('.user-hp-slide .slide-contents').slick({
      dots: true,
      arrows: false,
      infinite: false,
      speed: 300,
      slidesToShow: 1
    });
  }
};
export default Slide;
