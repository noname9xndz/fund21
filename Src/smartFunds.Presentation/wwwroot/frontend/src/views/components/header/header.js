/* eslint-disable no-plusplus */
const Header = {
  toogleMenu: () => {
    $('#menuToogle').click(() => {
      if (!localStorage.getItem('toggle')) {
        // Check if theres anything in localstorage already
        localStorage.setItem('toggle', 'true');
      } else if (localStorage.getItem('toggle') === 'true') {
        localStorage.setItem('toggle', 'false');
      } else if (localStorage.getItem('toggle') === 'false') {
        localStorage.setItem('toggle', 'true');
      }
      Header.activeToogle();
    });
  },
  checkToogle: () => {
    if (localStorage.getItem('toggle') === 'true') {
      Header.activeToogle();
    }
  },
  activeToogle: () => {
    $('.sidebar').toggleClass('is-active');
    $('.sidebar__links').toggleClass('displayNone');
    $('.sidebar__items').toggleClass('text-center');
    $('.sidebar__items--Name').toggleClass('displayNone');
    $('#headerLeftArea').toggleClass('headerLeftCollapse');
    $('#companyFullName').toggleClass('displayNone');
    $('#companyReduceName').toggleClass('displayBlock');
    $('.header__Right').toggleClass('headerRightCollapse');
  },
  activeCMSDropDown: () => {
    $('.sidebar__Dropdown').click(() => {
      $('.sidebar__Dropdown').toggleClass('is-active');
    });
  }
};
export default Header;
