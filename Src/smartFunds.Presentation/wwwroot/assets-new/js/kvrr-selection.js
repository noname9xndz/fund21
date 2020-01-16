
(function ($) {
    const kvrrSelectionMain = $('.kvrr-select-main');
    const kvrrSelectionItem = $('.kvrr-select-main .kvrr-selection-item');
    const kvrrImageChosen = $('.kvrr-select-main .kvrr-image-chosen');
    const kvrrTextChosen = $('.kvrr-select-main .kvrr-text-chosen');
    const kvrrSelectionSlider = $('.kvrr-select-main .js-carousel');

    'use strict';

    $.KVRRSelection = {
        init: function () {
            if (kvrrSelectionMain.length) {
                clickCheckKVRRItem();	
                setTimeout(() => {
                    if (kvrrSelectionSlider.hasClass('slick-initialized')) {
                        $('.kvrr-select-main .js-carousel').slick('destroy');
                        setTimeout(() => {
                            kvrrSelectionSlider.not('.slick-initialized').slick({
                                slidesToShow: 4,
                                slidesToScroll: 1,
                                infinite: false,
                                arrows: false,
                                dots: false,
                                responsive: [
                                    {
                                        breakpoint: 1025,
                                        settings: {
                                            slidesToShow: 3,
                                            slidesToScroll: 1,
                                            dots: true,
                                        },
                                    },
                                    {
                                        breakpoint: 769,
                                        settings: {
                                            slidesToShow: 2,
                                            slidesToScroll: 1,
                                            dots: true,
                                        },
                                    },
                                    {
                                        breakpoint: 480,
                                        settings: {
                                            slidesToShow: 1,
                                            slidesToScroll: 1,
                                            dots: true,
                                        },
                                    },
                                ],
                            });
                        },500)
                
                    }
                },500)
                checkedFirstSelectionItem();
            }
        }
    };

    function checkedFirstSelectionItem() {
        let getFirstItem = $(kvrrSelectionItem[0]);
        getFirstItem.find('input[type="radio"]').prop("checked", true);
        changeInfomationKVRRChosen(getFirstItem);
    }

    function changeInfomationKVRRChosen(itemChosen) {
        let imageChosen = itemChosen.find('img').attr('src');
        let textChosen = itemChosen.find('.kvrr-text-item').text();
        $(kvrrImageChosen).attr('src', imageChosen);
        $(kvrrTextChosen).text(textChosen);
    }

    //check riêng nút radio
    //function clickCheckKVRRItem() {
    //    $(kvrrSelectionItem).find('input[type="radio"]').change(function () {
    //        let itemSelect = $(this).parents('.kvrr-selection-item');
    //        changeInfomationKVRRChosen(itemSelect);
    //    })
    //}

    //check cả items
    function clickCheckKVRRItem() {
        $(kvrrSelectionItem).on('click', (e) => {
            let $this = $(e.currentTarget);
            $this.find('input[type="radio"]').prop("checked", true);
            changeInfomationKVRRChosen($this);
        })
    }

    $.KVRRSelection.init();
})(jQuery);





