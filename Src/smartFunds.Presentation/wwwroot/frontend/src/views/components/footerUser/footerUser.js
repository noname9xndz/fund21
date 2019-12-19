const FooterUser = {
  showItemFooter: () => {
    const dropBtn = document.querySelectorAll('.drop');
    dropBtn.forEach((item) => {
      item.addEventListener('click', () => {
        if (window.innerWidth < 768) {
          if (item.classList.contains('active')) {
            item.classList.remove('active');
            $(item.children[2]).slideUp();
          } else {
            dropBtn.forEach((x) => {
              x.classList.remove('active');
              $(x.children[2]).slideUp();
            });
            item.classList.add('active');
            $(item.children[2]).slideDown();
          }
        }
      });
    });
  }
};
export default FooterUser;
