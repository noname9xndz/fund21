const requiredMess = document.querySelector('#required-mess');
const phonePattern = /^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$/im;
const phoneValidate = document.querySelector('#phone-validate-mess');

const ManagerSettingIntroduce = {
  initFunction: () => {
    ManagerSettingIntroduce.validateFormIntroduce();
    ManagerSettingIntroduce.clickValidateImage();
  },
  validateFormIntroduce: () => {
    if ($('#form-introduce').length) {
      if (requiredMess) {
        $.extend($.validator.messages, {
          required: requiredMess.value
        });
        $.validator.addMethod(
          'validatePhoneNumber',
          value => phonePattern.test(value),
          phoneValidate.value,
        );
        $('#form-introduce').validate({
          rules: {
            phoneIntroduce: {
              required: true,
              validatePhoneNumber: true
            }
          },
          submitHandler: () => {

          }
        });
      }
    }
  },
  clickValidateImage: () => {
    const errorImageIntroduce = document.querySelector('#imageIntroduceError');
    const btnUpload = document.querySelector('.image-upload__item__btn-upload');
    const btnAddIntroduce = document.querySelector('#add-introduce');
    if (btnAddIntroduce) {
      btnAddIntroduce.addEventListener('click', () => {
        if (btnUpload.style.display !== 'none') {
          errorImageIntroduce.style.display = 'block';
        } else {
          $('#form-introduce').submit();
        }
      });
    }
  }
};

export default ManagerSettingIntroduce;
