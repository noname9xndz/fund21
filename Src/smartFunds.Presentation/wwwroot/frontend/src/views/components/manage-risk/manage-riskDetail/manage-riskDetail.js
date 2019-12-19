
const manageRiskDetail = {
  Edit: () => {
    const myImg = document.getElementById('myImgPopup');
    if (myImg) {
      document.querySelector('input[type="file"]').addEventListener('change', function x() {
        if (this.files && this.files[0]) {
          const img = document.querySelector('img:nth-child(3)');
          img.src = URL.createObjectURL(this.files[0]);
        }
      });
      myImg.onclick = function c() {
        $('input[type="file"]').trigger('click');
      };
    }
  }
};

export default manageRiskDetail;
