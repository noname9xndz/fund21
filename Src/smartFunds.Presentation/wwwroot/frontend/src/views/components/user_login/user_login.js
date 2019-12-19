const Validate = {
  validateFormLogin: () => {
    $.validator.setDefaults({
      debug: true,
      success: (label) => {
        label.attr('id', 'valid');
      }
    });
    $('#form_login').validate({
      rules: {
        password: 'required',
        your_email: {
          required: true,
          email: true
        }
      },
      messages: {
        your_email: {
          required: ' ',
          email: ' '
        },
        password: {
          required: ' '
        }
      }
    });
  },
  showPasswordFormLogin: () => {
    $('.user_login .form-detail .show-pw').on('click', () => {
      const passWord = document.getElementById('Password');
      if (passWord.type === 'password') {
        passWord.type = 'text';
      } else {
        passWord.type = 'password';
      }
    });
  },
  hideHeaderFooterOnDesktop: () => {
    if ($(window).width() > 767) {
      $('.user-login-page .headerUser').css('display', 'none');
      $('.user-login-page .footerUser').css('display', 'none');
    } else {
      $('.user-login-page .headerUser').css('display', 'block');
      $('.user-login-page .footerUser').css('display', 'block');
    }
  },
  checkLengthInputToHidePlaceholder: () => {
    let inputs = document.querySelectorAll('#form_login .user-input');
    if (inputs) {
      inputs = Array.from(inputs);

      inputs.forEach((inputItem) => {
        if (inputItem) {
          const self = inputItem;
          inputItem.addEventListener('keyup', (e) => {
            if (self && self.value !== '') {
              self.classList.add('input-has-value');
            } else {
              self.classList.remove('input-has-value');
            }
          });
        }
      });
    }
  }
};
export default Validate;
