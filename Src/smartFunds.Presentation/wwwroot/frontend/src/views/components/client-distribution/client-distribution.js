
import Global from '../global/global';
import DatatableLanguages from '../datatables/datatables-language';

const tableElm = document.querySelector('#clientDistributionTable');

const clientDistribution = {
  initFunction: () => {
    clientDistribution.initTableClientDistributionTier();
  },

  initTableClientDistributionTier: () => {
    if (tableElm) {
      Global.initDataTable('#clientDistributionTable', {
        searching: false,
        info: false,
        paging: false,
        language: DatatableLanguages,
        columnDefs: [{
          targets: 1,
          orderable: false
        }],
        drawCallback: () => {
          Global.initAllCheckBox(tableElm);

          // Global.hidePagingIfOnepage(tableElm);
        }
      });
    }
    // clientDistribution.TestPaging();
  },
  TestPaging: () => {
    // const totalPage = 2;

    // const pagingFake = document.querySelectorAll('.pagingFake--Button');
    // const pagingReal = document.querySelectorAll('.paginate_button');
    // const pagingRealArray = Array.from(pagingReal);
    // const a = pagingRealArray.indexOf('2');
    // console.log('pagingRealArray', pagingRealArray);

    // pagingFake.forEach((fakeButton) => {
    //   fakeButton.addEventListener('click', () => {
    //     pagingReal.forEach((realButton) => {
    //       if (realButton.innerHTML === fakeButton.innerHTML) {
    //         $(realButton).trigger('click');
    //       }
    //     });
    //   });
    // });
  }
};

export default clientDistribution;
