import MicroModal from 'micromodal';
import PerfectScrollbar from 'perfect-scrollbar';
import Global from '../global/global';

const UserProfileInfo = {
  modalEditInfo: () => {
    const container = document.querySelector('.user-info-section .modal__container');
    if (container) {
      const ps = new PerfectScrollbar(container);
      const editInfo = document.querySelector('.user-info-section .link-show');
      editInfo.addEventListener('click', () => {
        MicroModal.show('modal-change-info', {
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
    }
  },
  validateEditInfo: () => {
    $.validator.setDefaults({
      debug: true,
      success: (label) => {
        label.attr('id', 'valid');
      }
    });
    $('.user-info-section #form_login').validate({
      rules: {
        your_name: {
          required: true
        }
      },
      messages: {
        your_name: {
          required: 'Dữ liệu trường Họ và Tên không được để trống'
        }
      }
    });
  },
  editInfo: () => {
    UserProfileInfo.initButtonUpdateInfo();
  },
  getDataProfile: () => {
    const editProfileDataApiUrl = document.querySelector('#editProfileDataApiUrl');
    if (editProfileDataApiUrl) {
      const fullName = document.querySelector('.user-info-section input[name="your_name"]');
      const phoneNumber = document.querySelector('.user-info-section input[name="your_phone"]');
      const userName = document.querySelector('.user-info-section .username');
      const userNameHeaderDesktop = document.querySelector('.headerUser_login .show-desktop-header .username');
      const userNameHeaderMobile = document.querySelector('.headerUser_login .show-mobile-header .username');
      const apiGetPropertyFluctuations = `${editProfileDataApiUrl.value}?FullName=${fullName ? fullName.value : ''}&PhoneNumber=${phoneNumber ? phoneNumber.value : ''}`;
      // console.log(apiGetPropertyFluctuations);
      const model = {
        FullName: fullName ? fullName.value : '',
        PhoneNumber: phoneNumber ? phoneNumber.value : ''
      };
      // debugger;
      Global.getDataFromUrlPost(apiGetPropertyFluctuations, model).then((data) => {
        if (fullName && phoneNumber && userName && userNameHeaderDesktop && userNameHeaderMobile && fullName.value !== '' && phoneNumber.value !== '') {
          const fullNameValue = fullName.value;
          userName.textContent = fullNameValue;
          userNameHeaderDesktop.textContent = fullNameValue;
          userNameHeaderMobile.textContent = fullNameValue;
          $('.user-info-section .modal__close').trigger('click');
        }
      });
    }
  },
  initButtonUpdateInfo: () => {
    const btnUpdate = document.querySelector('.user-info-section .user-input--update');
    if (btnUpdate) {
      btnUpdate.addEventListener('click', () => {
        const fullName = document.querySelector('.user-info-section input[name="your_name"]');
        const phoneNumber = document.querySelector('.user-info-section input[name="your_phone"]');
        if (fullName && phoneNumber && fullName.value !== '' && phoneNumber.value !== '') {
          UserProfileInfo.getDataProfile();
        }
      });
    }
  },
  showPopup: () => {

    const container = document.querySelector('.popup-modal .modal__container');
    if (container) {
      const ps = new PerfectScrollbar(container);
      const linkDetail = document.querySelectorAll('.link-show-popup');
      linkDetail.forEach((linkDetailItems) => {
        linkDetailItems.addEventListener('click', () => {
          MicroModal.show('modal-general', {
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
  },
};
export default UserProfileInfo;
