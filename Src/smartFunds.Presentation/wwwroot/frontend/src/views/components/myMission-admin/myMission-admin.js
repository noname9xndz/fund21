import MicroModal from 'micromodal';
import Global from '../global/global';
import DatatableLanguages from '../datatables/datatables-language';


const myMissionAdmin = {
  initFuction: () => {
    myMissionAdmin.initTableMyMissionAdmin();
  },
  initTableMyMissionAdmin: () => {
    const tableElm = document.querySelector('#myMissionAdminTable');
    if (tableElm) {
      const $table = $(tableElm).DataTable({
        info: false,
        searching: false,
        lengthChange: false,
        language: DatatableLanguages,
        sort: true,
        drawCallback: () => {
          Global.hidePagingIfOnepage(tableElm);
        }
      });
      Global.drawTableWithLength(tableElm);
      Global.initPageLengthChange(tableElm);
      myMissionAdmin.onupdate(tableElm);
      myMissionAdmin.onclickPaging(tableElm);
    }
  },
  onupdate: (tableElm) => {
    const updateButton = tableElm.querySelectorAll('.myMission__Admin--accept');
    const myMissionAdminLink = document.querySelector('#missionAdminLink');
    if (updateButton.length > 0) {
      updateButton.forEach((button) => {
        button.addEventListener('click', () => {
          MicroModal.show('my-mission__Modal--admin');
          const ModalAcceptButton = document.querySelector('.modal__btn');
          if (ModalAcceptButton) {
            ModalAcceptButton.addEventListener('click', () => {
              const updateLink = button.nextElementSibling;
              Global.getDataFromAjaxCall(updateLink.value).then((data) => {
                if (data.success) {
                  window.open(myMissionAdminLink.value, '_self');
                }
              });
            });
          }
        });
      });
    }
  },
  onclickPaging: (tableElm) => {
    const pagingButtons = document.querySelectorAll('.paging_simple_numbers .paginate_button');
    if (pagingButtons.length > 0) {
      pagingButtons.forEach((items) => {
        items.addEventListener('click', () => {
          myMissionAdmin.onupdate(tableElm);
        });
      });
    }
  }
};

export default myMissionAdmin;
