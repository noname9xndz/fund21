(function ($) {
    const questionGroupKVRR1 = $('[question-group="1"]');
    const questionGroupKVRR2 = $('[question-group="2"]');
    const questionKVRRItem2 = $('.defind-kvrr-main .define-kvrr-step-2');
    const buttonAccept = $('.defind-kvrr-main .button-accept');
    const buttonNext = $('.defind-kvrr-main .button-next');
    const buttonPrev = $('.defind-kvrr-main .button-prev');
    const questionKVRRItem = $('.defind-kvrr-main .define-kvrr-question-item');
    let step = 1;
    let questionGroup;
    let answerList = [];
    'use strict';
    $.DefindKVRR = {
        init: function () {
            if ($('.defind-kvrr-main').length) {
                hideQuestionFirstTime();
                selectAnswer();
                buttonNextClick();
                buttonPrevClick();
            }
        }

    };
    function hideQuestionFirstTime() {
        questionGroupKVRR1.css('display', 'none');
        questionGroupKVRR2.css('display', 'none');
        questionKVRRItem2.css('display', 'none');
        // INFO : none event button accept
        noneEvent(buttonAccept);
        noneEvent(buttonPrev);
        noneEvent(buttonNext);
    }

    function selectAnswer() {
        questionKVRRItem.find('input[type = "radio"]').change(function () {
            // INFO : use trick, not use radio button, use background color
            questionKVRRItem.find('span.u-btn-bluegray').css('background-color', '#585f69');
            let score = Number($(this).attr('score'));
            let dataQuestionIndex = Number($(this).parents('.define-kvrr-question-item').attr('data-question-index'));
            step = dataQuestionIndex;
            updateListAnswerScore(dataQuestionIndex, score);
            updateLinkButtonAccept();
            updateStateButtonNav();
        })
    }

    function buttonNextClick() {
        buttonNext.on('click', () => {
            questionKVRRItem.css('display', 'none');
            showQuestion('next');
            checkStepByCss();
        })
    }

    function buttonPrevClick() {
        buttonPrev.on('click', () => {
            questionKVRRItem.css('display', 'none');
            showQuestion('prev');
            checkStepByCss();
        })
    }

    function checkStepByCss() {
        $('.define-kvrr-question-item').each((index, item) => {
            if ($(item).css('display') === 'block') {
                let dataQuestionIndex = Number($(item).attr('data-question-index'));
                step = dataQuestionIndex;
            }
        })
    }

    function showQuestion(type) {
        switch (type) {
            case 'next': {
                if (step + 1 == 2) {
                    $(`[data-question-index=${step + 1}]`).css('display', 'block');
                }
                if (step + 1 == 3 && getSumScore() <= 3) {
                    questionGroup = 1;
                    $(`[data-question-index=${step + 1}][question-group="${questionGroup}"]`).css('display', 'block');
                    // INFO: if questionIndex <=2 .user changed radio, remove score questionIndex > 2
                    if (answerList.length <= 2) {
                        $('.define-kvrr-step-3, .define-kvrr-step-4, .define-kvrr-step-5').find('span.u-btn-bluegray').css('background-color', '#585f69');
                    }
                }
                if (step + 1 == 3 && getSumScore() > 3) {
                    questionGroup = 2;
                    $(`[data-question-index=${step + 1}][question-group="${questionGroup}"]`).css('display', 'block');
                    // INFO: if questionIndex <=2 .user changed radio, remove score questionIndex > 2
                    if (answerList.length <= 2) {
                        $('.define-kvrr-step-3, .define-kvrr-step-4, .define-kvrr-step-5').find('span.u-btn-bluegray').css('background-color', '#585f69');
                    }                   
                }
                if (step + 1 > 3) {
                    $(`[data-question-index=${step + 1}][question-group="${questionGroup}"]`).css('display', 'block');
                }
                checkStateRadioSelected();
                setTimeout(() => {
                    updateStateButtonNav();
                }, 200);
                break;
            }
            case 'prev': {
                if (step - 1 == 1 || step - 1 == 2) {
                    $(`[data-question-index=${step - 1}]`).css('display', 'block');
                }
                if (step - 1 == 3 || step - 1 == 4) {
                    $(`[data-question-index=${step - 1}][question-group="${questionGroup}"]`).css('display', 'block');
                }
                checkStateRadioSelected();
                setTimeout(() => {
                    updateStateButtonNav();
                }, 200);
                break
            }
            default: {
                break;
            }
               
        }
    }
    function checkStateRadioSelected() {
        questionKVRRItem.find('input[type="radio"]').prop('checked', false);
        answerList.forEach(e => {
            if (e.dataQuestionIndex <= 2) {
                $(`[data-question-index="${e.dataQuestionIndex}"] [score="${e.score}"]`).siblings('span').css('background-color', '#fe541e')
            } else {
                $(`[data-question-index="${e.dataQuestionIndex}"][question-group="${questionGroup}"] [score="${e.score}"]`).siblings('span').css('background-color', '#fe541e')
            }
        })
    }

    function updateListAnswerScore(dataQuestionIndex, score) {
        let checkExists = false;
        answerList.forEach(item => {
            if (item.dataQuestionIndex == dataQuestionIndex) {
                checkExists = true;
                item.score = score;
            }
        });

        if (!checkExists) {
            answerList.push({
                dataQuestionIndex,
                score
            })
        }
        if (dataQuestionIndex <= 2) {
            answerList = answerList.filter(x => x.dataQuestionIndex <= 2);
        }
    }

    function updateLinkButtonAccept() {
        buttonAccept.attr('href', `/Account/KVRRRecommendation?questionGroup=${questionGroup}&totalScore=${getSumScore()}`);
    }

    function updateStateButtonNav() {
        switch (step) {
            case 1: {
                noneEvent(buttonPrev);
                if (checkStepChosenAnswer()) {
                    blockEvent(buttonNext);
                } 
                break;
            }
            case 5: {
                blockEvent(buttonPrev);
                noneEvent(buttonNext);
                if (checkStepChosenAnswer()) {
                    blockEvent(buttonAccept);
                }
                break;
            }
            default: {
                blockEvent(buttonPrev);
                if (checkStepChosenAnswer()) {
                    console.log("vao day....")
                    blockEvent(buttonNext);
                } else {
                    noneEvent(buttonNext);
                }
                break;
            }
        }
    }

    function checkStepChosenAnswer() {
        console.log(step);
        let check = false;
        answerList.forEach(e => {
            if (e.dataQuestionIndex == step) {
                check = true;
            }
        })
        return check;
    }

    function getSumScore() {
        let totalScore = 0;
        answerList.forEach(item => {
            totalScore += item.score
        })
        return totalScore;
    }

    function noneEvent(event) {
        event.css({
            'pointer-events': 'none',
            'opacity': '0.5'
        })
    }
    function blockEvent(event) {
        event.css({
            'pointer-events': 'initial',
            'opacity': 'initial'
        })
    }

    $.DefindKVRR.init();
})(jQuery);