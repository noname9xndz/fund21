(function ($) {
    const inputUserName = $('#modalcanhan .username-input');
    const userName = $('.user-info-content .username');
    const buttonChangeName = $('#modalcanhan .button-change-user-name')
    const nameInHeader = $('.u-header--static .name-header');

    // INFO: search hight chart
    const startDate = $('.user-profile .user-profile-start-date');
    const endDate = $('.user-profile .user-profile-end-date');
    const buttonSearchChart = $('.user-profile .button-search-chart');
    const messageChart = $('.user-profile .message-chart');
    const hightChart = $('.user-profile #container');
    let startDateValue;
    let endDateValue
    let xCategories = [];
    let yValue = [];
    'use strict';                                                 
    $.UserProfile = {
        init: function () {
            if ($('#modalcanhan').length) {
                onChangeUserName();
            }
            if (startDate.length) {
                setInitialDate();
                callApiSearchChart();
                onChangeDate();
                onSearchChart();
                setInitialChart();
            }
        }
    };

    function onChangeUserName() {
        buttonChangeName.on('click', (e) => {
            e.preventDefault();
            callApiChangeUserName();
        })
    }

    function callApiChangeUserName() {
        const userNameValue = inputUserName.val();
        $.ajax({
            url: `/profile/edit?FullName=${userNameValue}`,
            contentType: "application/json; charset=utf-8",
            method: 'POST'
        }).done(res => {
            nameInHeader.empty();
            if (res.success) {
                userName.html(`${userNameValue}`);
                let arrowDown = '<i class="hs-admin-angle-down g-pos-rel g-top-2 g-ml-10"></i>';
                let splitUserName = userNameValue.split(" ");
                if (splitUserName.length) {
                    nameInHeader.append(`
                    ${splitUserName[0]}
                       ${arrowDown}
                `)
                }
                Custombox.modal.close();
            }
        })
    }


    function setInitialChart() {
        Highcharts.setOptions({
            chart: {
                style: {
                    fontFamily: 'Quicksand',
                    fontSize: '16px'
                }
            }
        });

        Highcharts.chart('container', {
            chart: {
                zoomType: 'x'
            },
            title: {
                text: ''
            },
            subtitle: {
                text: document.ontouchstart === undefined ?
                    '' : ''
            },
            xAxis: {
                categories: xCategories
            },
            yAxis: {
                title: {
                    text: 'VNĐ'
                },
                
                min: 0,
                labels: {
                    //formatter: function () {
                    //    let value = this.axis.defaultlabelformatter.call(this).tostring();
                    //    console.log(value);
                    //    let unitmoney = '';
                    //    //if (value.indexof('m') != -1) {
                    //    //    unitmoney = 'million'
                    //    //}
                    //    //if (value.indexof('k') != -1) {
                    //    //    unitmoney = 'thounsand'
                    //    //}
                    //    let valuenumber = value.replace("k", "");
                    //    valuenumber = value.replace("m", "");
                    //    valuenumber = number(valuenumber.replace(" ", ""));
                    //    return unitmoney == 'million' ? valuenumber : valuenumber / 1000;
                    //}
                }
                 
            },
            legend: {
                enabled: false
            },
            plotOptions: {
                area: {
                    fillColor: {
                        linearGradient: {
                            x1: 0,
                            y1: 0,
                            x2: 0,
                            y2: 1
                        },
                        stops: [
                            [0, Highcharts.getOptions().colors[0]],
                            [1, Highcharts.Color(Highcharts.getOptions().colors[0]).setOpacity(0).get('rgba')]
                        ]
                    },
                    marker: {
                        radius: 2
                    },
                    lineWidth: 1,
                    states: {
                        hover: {
                            lineWidth: 1
                        }
                    },
                    threshold: null
                }
            },
            //tooltip: {
            //    enabled: true,
            //    formatter: function () {
            //        console.log('time : ', this.x, ' value : ', this.y);
            //    }
            //},
            series: [{
                type: 'area',
                name: 'Tài khoản',
                data: yValue
            }],
            exporting:
            {
                enabled: false
            }
        });
    }
    function setInitialDate() {
        //let endDateTimeStamp = Date.now();
        //let subStractDate = endDateTimeStamp - (30 * 86400 * 1000);
        //startdate.val(new date(substractdate).tolocaledatestring('en-sg'));
        //enddate.val(new date(enddatetimestamp).tolocaledatestring('en-sg'));
        startDateValue = '01/01/0001';
        //enddatevalue = new date(enddatetimestamp).tolocaledatestring('en-sg');
        endDateValue = new Date().toLocaleDateString('en-sg');
    }

    function onChangeDate() {
        startDate.datepicker({
            dateFormat: 'dd/mm/yy',
            maxDate: 'today',
            onSelect: function () {
                let fullDate = $(this).datepicker('getDate').toLocaleDateString('en-SG');
                startDate.val(fullDate);
                startDateValue = fullDate;
            }
        });
        endDate.datepicker({
            dateFormat: 'dd/mm/yy',
            maxDate: 'today',
            onSelect: function () {
                let fullDate = $(this).datepicker('getDate').toLocaleDateString('en-SG');
                endDate.val(fullDate);
                endDateValue = fullDate;
            }
        });
    }
    function onSearchChart() {
        buttonSearchChart.on('click', () => {
            callApiSearchChart();
        })
    }
    function callApiSearchChart() {
        $.ajax({
            method: 'POST',
            url: `/profile/PropertyFluctuations?dateFrom=${startDateValue}&dateTo=${endDateValue}`
        }).done(res => {
            xCategories = [];
            yValue = [];
            if (res.listProperty && res.listProperty.length) {
                res.listProperty.forEach(item => {
                    let timeStamp = Date.parse(item.date);
                    let dateToString = new Date(timeStamp);
                    let date = dateToString.getDate();
                    let month = dateToString.getMonth() + 1;
                    let year = dateToString.getFullYear();
                    xCategories.push(date + "/" + month + "/" + year)
                    yValue.push(item.amount);
                })
                setInitialChart();
                hightChart.css("display", "block");
                messageChart.css("display", "none");
            } else {
                setInitialChart();
                hightChart.css("display", "none");
                messageChart.css("display", "block");
            }
        })
    }
    $.UserProfile.init();
})(jQuery);