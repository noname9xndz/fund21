import validator from 'validator';

const Resgister = {
  toogleViewPass: () => {
    const elems = document.querySelectorAll('.icon-view');
    elems.forEach((elem) => {
      // eslint-disable-next-line func-names
      elem.addEventListener('click', function () {
        if (this.parentNode.parentNode.querySelector('.password.form-group').type === 'password') {
          this.parentNode.parentNode.querySelector('.password.form-group').type = 'text';
        } else {
          this.parentNode.parentNode.querySelector('.password.form-group').type = 'password';
        }
      });
    });
  },

  toogleClickInput: () => {
    $('.resgister .text-placeholder').click((e) => {
      $(e.currentTarget).parent().children('input.form-group').focus();
    });
  },

  validateEmail: (current, value) => {
    if (validator.isEmail(value)) {
      current.parentNode.classList.add('active');
      current.parentNode.classList.remove('warning');
    }
    if (validator.isMobilePhone(value)) {
      current.parentNode.classList.add('active');
      current.parentNode.classList.remove('warning');
    }
    if (validator.isEmail(value) === false && validator.isMobilePhone(value) === false) {
      current.parentNode.classList.remove('active');
      current.parentNode.classList.add('warning');
    }
  },

  validate: () => {
    const elems = document.querySelectorAll('.form-group');

    elems.forEach((elem) => {
      // eslint-disable-next-line func-names
      elem.addEventListener('focus', function () {
        if (this.value === 0) {
          elem.parentNode.classList.remove('active');
          // eslint-disable-next-line no-console
        }
      });
      if (elem.id === 'email') {
        // eslint-disable-next-line func-names
        elem.addEventListener('change', function () {
          Resgister.validateEmail(this, this.value);
        });
        // eslint-disable-next-line func-names
        elem.addEventListener('keyup', function () {
          Resgister.validateEmail(this, this.value);

          // eslint-disable-next-line no-restricted-globals
          if (event.key === 'Delete' || event.key === 'Backspace') {
            Resgister.validateEmail(this, this.value);
          }
        });
      }
    });
    // eslint-disable-next-line func-names
    if (document.getElementById('accept') !== null) {
      // eslint-disable-next-line func-names
      document.getElementById('accept').addEventListener('change', function () {
        if (this.checked) {
          this.parentNode.classList.remove('warning');
        }
      });
    }
  },

  submitForm: () => {
    if (document.getElementById('resgister-form') !== null) {
      document.getElementById('resgister-form').addEventListener('submit', (event) => {
        if (document.getElementById('accept').checked === false) {
          document.querySelector('.accept-condition').classList.add('warning');
          Resgister.validate();
          event.preventDefault();
        }
      });
    }
  }
};

export default Resgister;
