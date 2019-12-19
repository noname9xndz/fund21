import MicroModal from 'micromodal'; // es6 module
import PerfectScrollbar from 'perfect-scrollbar';

let isSubmitted = false;
const checkbtn = document.querySelector('#btnAnswer');
const subInvesment = document.querySelectorAll('.sub-invesment') || '';
const subInvesmentMain = document.querySelectorAll('.sub-innvesment-cheked');
const investSuggestion = document.querySelector('.inputuser-invesment-suggetestion');
const suggestitonBlock = document.querySelectorAll('.suggest-block');
const subInvestmentContent = document.querySelectorAll('.sub-invesment-content');
const subInvesmentDetail = document.querySelectorAll('.name-sub-invesment');

const userInvestmentPortfolio = {
  sliderInvsement: () => {
    // eslint-disable-next-line no-restricted-globals
    if (screen.width < 992) {
      $('.list-invesment-main').not('.slick-initialized').slick({
        slidesToShow: 1,
        // centerMode: true,
        variableWidth: true,
        infinite: false,
        arrows: false,
        mobileFirst: true,
        responsive: [
          {
            breakpoint: 992,
            settings: 'unslick'
          }
        ]
      });
    }
  },
  invesmentChecked: () => {
    suggestitonBlock.forEach((eachBlock) => {
      const btn = eachBlock.querySelector('.btn-select');
      if (btn) {
        btn.addEventListener('click', () => {
          subInvesmentMain.forEach((subitem) => {
            subitem.parentNode.parentNode.classList.remove('is-active');
            subitem.checked = false;
          });
          subInvestmentContent.forEach((item) => {
            if (item.classList.contains('is-active')) {
              item.classList.remove('is-active');
            }
          });
          // subInvestmentContent.classList.remove('is-active');
          const inputElm = eachBlock.querySelector('input');
          if (inputElm && inputElm.checked) {
            eachBlock.classList.add('is-active');
            checkbtn.removeAttribute('disabled');
            checkbtn.setAttribute('style', 'background-color: #F77314;');
          }
        });
      }
    });
    // subInvesmentMain.forEach((subitem) => {
    //   subitem.parentNode.parentNode.classList.remove('is-active');
    //   subitem.checked = false;
    // });
    // eslint-disable-next-line func-names
    subInvesment.forEach((item) => {
      item.addEventListener('click', () => {
        if (item) {
          // debugger;
          subInvesmentMain.forEach((subitem) => {
            if (subitem.checked) {
              subitem.parentNode.parentNode.parentNode.classList.add('is-active');
              checkbtn.removeAttribute('disabled');
              checkbtn.setAttribute('style', 'background-color: #F77314;');
              if (investSuggestion) {
                document.querySelector('.user-invesment-suggetestion-main').classList.remove('is-active');
              }
            }
            if (!subitem.checked) {
              subitem.parentNode.parentNode.parentNode.classList.remove('is-active');
            }
          });
        }
      });
      if (investSuggestion) {
        investSuggestion.addEventListener('click', () => {
          if (investSuggestion.checked) {
            subInvestmentContent.forEach((items) => {
              items.classList.remove('is-active');
            });
            document.querySelector('.user-invesment-suggetestion-main').classList.add('is-active');
          }
        });
      }
    });
  },
  modalDetailInvesment: () => {
    const container = document.querySelector('.detail__subInvesment__main__inside');
    if (container) {
      const ps = new PerfectScrollbar(container);
      const detailInvesment = document.querySelectorAll('.name-sub-invesment');
      detailInvesment.forEach((linkDetail) => {
        linkDetail.addEventListener('click', () => {
          MicroModal.show('modal-1', {
            onClose: () => {
              if (document.querySelector('.modal.micromodal-slide')) {
                if (document.querySelector('.modal.micromodal-slide').getAttribute('aria-hidden') === 'true') {
                  document.getElementsByTagName('body')[0].style.overflow = 'visible';
                }
              }
              container.scrollTop = 0;
              ps.update(container);
            },
            onShow: () => {
              if (document.querySelector('.modal.micromodal-slide')) {
                if (document.querySelector('.modal.micromodal-slide').getAttribute('aria-hidden') === 'false') {
                  document.getElementsByTagName('body')[0].style.overflow = 'hidden';
                }
              }
            }
          });
        });
      });
    }

    subInvesmentDetail.forEach((viewDetail) => {
      viewDetail.addEventListener('click', (e) => {
        // eslint-disable-next-line no-unused-vars
        if (document.querySelector('.detail__subInvesment__main__title')) {
          const titleInvesment = e.currentTarget.innerHTML;
          document.querySelector('.detail__subInvesment__main__title').innerHTML = titleInvesment;
        }
        const descriptionSubInvesments = document.querySelectorAll('.sub-invesment-sort-description');
        const allLinkImages = document.querySelectorAll('.sub-invesment-avatar-cover');
        for (let i = 0; i < e.currentTarget.parentNode.children.length; i++) {
          const child = e.currentTarget.parentNode.children[i];
          if (child.nodeType === 1 && child !== e.currentTarget) {
            descriptionSubInvesments.forEach((descriptionSubInvesment) => {
              if (child === descriptionSubInvesment) {
                if (document.querySelector('.detail__subInvesment__main__content')) {
                  const contentInvesmnet = child.innerHTML;
                  document.querySelector('.detail__subInvesment__main__content').innerHTML = contentInvesmnet;
                }
              }
            });
            // eslint-disable-next-line no-shadow
            allLinkImages.forEach((linkActive) => {
              if (linkActive.parentNode.parentNode.parentNode === e.currentTarget.parentNode.parentNode) {
                const contentLink = linkActive.getAttribute('src');
                document.querySelector('.detail__subInvesment_images_banner').src = contentLink;
              }
            });
          }
        }
      });
    });
  },
  loadingSubmitKVRR: () => {
    const submitForm = $('.user-invesment-portfolio form');
    const imgLoad = $('#pageloader');
    const btnSubmit = $('.user-invesment-portfolio button.btn-submit');
    if (submitForm.length && imgLoad.length && btnSubmit.length) {
      submitForm.on('submit', () => {
        if (!isSubmitted) {
          imgLoad.css('display', 'block');
          isSubmitted = true;
        }
      });
    }
  }
};
export default userInvestmentPortfolio;
