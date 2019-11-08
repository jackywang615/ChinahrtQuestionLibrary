// ==UserScript==
// @name         四川省专业技术人员继续教育网辅助答题及数据采集
// @namespace    http://tampermonkey.net/
// @version      0.1
// @description  try to take over the world!
// @author       jackywang
// @require      https://cdn.bootcss.com/jquery/3.3.1/jquery.min.js
// @match        https://videoadmin.chinahrt.com.cn/videoPlay/play*
// @match        https://videoadmin.chinahrt.com/videoPlay/play*
// @match        https://web.chinahrt.com/exam/view_exam_result*
// @match        https://web.chinahrt.com/exam/go_exam*
// @grant        none
// ==/UserScript==
(function () {
    'use strict';

    function uploadResult() {
        $('.question-y').each(function (i, e) {
            var title = $(e).find('h3').text();
            var value = $(e).find('span').text();
            var result = { 'title': title, 'value': value };
            $.post('http://127.0.0.1:8080/api/questions', result);
        });
    }

    function queryResult() {
        var q = $("<a href='javascript:void(0)'>查询</a>");
        q.click(function () {
            var q = $(this).parent().children('h3').text();
            $.get('http://127.0.0.1:8080/api/questions?title=' + q).done(function (data) {
                console.clear();
                if (data.length > 0) {
                    for (let i in data) {
                        console.group(data[i].title);
                        var answers = data[i].answers;
                        for (let r in answers) {
                            console.log('选项：' + answers[r].result + '  结果：' + (answers[r].isRight ? '正确' : '错误'));
                        }
                        console.groupEnd();
                    }
                }
                else {
                    console.log('查无数据!');
                }
            });
        });

        $('.question-y>h3').wrap("<div />").parent().append(q);
    }

    $(function () {
        var url = window.location.href;
        if (url.startsWith("https://web.chinahrt.com/exam/view_exam_result")) {
            uploadResult();
        }

        if (url.startsWith("https://web.chinahrt.com/exam/go_exam")) {
            queryResult();
        }
    });
})();