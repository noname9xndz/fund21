import { Validation } from '../bunnyjs/src/Validation';

const subscribeForm = document.querySelector('form[name="subscribeForm"]');
const Footer = {
  initSubscribeForm: () => {
    Validation.validateSection(subscribeForm).then((result) => {
      if (result === true) {
        // validation success
      } else {
        // section invalid, result is array of invalid inputs
        result[0].focus();
      }
    });
  },
  submitFormSubscribe: () => {
    if (subscribeForm) {
      const submitBtn = document.querySelector('#subscribeForm .btn.btn--transparent');
      submitBtn.addEventListener('click', (e) => {
        e.preventDefault();
        Footer.initSubscribeForm(e);
      });
    }
  }
};

export default Footer;
