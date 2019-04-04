/***
** 功能：  字符串格式化替换操作
** Author: Allen Zhang
** RTX：   14002
***/
String.prototype.format = function () {
    var args = arguments;
    return this.replace(/\{(\d+)\}/g,
        function (m, i) {
            return args[i];
        });
}

/***
** 功能：  去掉字符左右空格
** Author: Allen Zhang
** RTX：   14002
***/
String.prototype.trim = function () {
    return this.replace(/^\s\s*/, '').replace(/\s\s*$/, '');
}

/***
** 功能：  格式化时间字符串，支持多种时间格式化类型
** 参数：  format 日期对象 
** 示例：  new Date().format("yyyy年MM月dd日 hh:mm:ss");
** Author: Allen Zhang
** RTX：   14002
***/
Date.prototype.format = function (format) {
    var o = {
        "M+": this.getMonth() + 1, //month
        "d+": this.getDate(), //day
        "h+": this.getHours(), //hour
        "m+": this.getMinutes(), //minute
        "s+": this.getSeconds(), //second
        "q+": Math.floor((this.getMonth() + 3) / 3), //quarter
        "S": this.getMilliseconds() //millisecond
    }
    if (/(y+)/.test(format)) {
        format = format.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    }
    for (var k in o) {
        if (new RegExp("(" + k + ")").test(format)) {
            format = format.replace(RegExp.$1, RegExp.$1.length == 1 ? o[k] : ("00" + o[k]).substr(("" + o[k]).length));
        }
    }
    return format;
}


// Production steps of ECMA-262, Edition 5, 15.4.4.14
// Reference: http://es5.github.io/#x15.4.4.14
if (!Array.prototype.indexOf) {
    Array.prototype.indexOf = function (searchElement, fromIndex) {

        var k;

        // 1. Let O be the result of calling ToObject passing
        //    the this value as the argument.
        if (this == null) {
            throw new TypeError('"this" is null or not defined');
        }

        var O = Object(this);

        // 2. Let lenValue be the result of calling the Get
        //    internal method of O with the argument "length".
        // 3. Let len be ToUint32(lenValue).
        var len = O.length >>> 0;

        // 4. If len is 0, return -1.
        if (len === 0) {
            return -1;
        }

        // 5. If argument fromIndex was passed let n be
        //    ToInteger(fromIndex); else let n be 0.
        var n = +fromIndex || 0;

        if (Math.abs(n) === Infinity) {
            n = 0;
        }

        // 6. If n >= len, return -1.
        if (n >= len) {
            return -1;
        }

        // 7. If n >= 0, then Let k be n.
        // 8. Else, n<0, Let k be len - abs(n).
        //    If k is less than 0, then let k be 0.
        k = Math.max(n >= 0 ? n : len - Math.abs(n), 0);

        // 9. Repeat, while k < len
        while (k < len) {
            // a. Let Pk be ToString(k).
            //   This is implicit for LHS operands of the in operator
            // b. Let kPresent be the result of calling the
            //    HasProperty internal method of O with argument Pk.
            //   This step can be combined with c
            // c. If kPresent is true, then
            //    i.  Let elementK be the result of calling the Get
            //        internal method of O with the argument ToString(k).
            //   ii.  Let same be the result of applying the
            //        Strict Equality Comparison Algorithm to
            //        searchElement and elementK.
            //  iii.  If same is true, return k.
            if (k in O && O[k] === searchElement) {
                return k;
            }
            k++;
        }
        return -1;
    };
}

/***
** 功能：  加载外部JS文件，加载完成后执行回调函数callback
** Author: Allen Zhang
** RTX：   14002
***/
var utools = {
    config: {
        id: "",
        url: "",
        charset: "gb2312",
        callback: function () { }
    },
    merge: function (a, c) {
        for (var b in c) a[b] = c[b];
        return a
    },
    getScript: function (a) {
        var r = Math.floor(Math.random() * 10000);
        this.config = this.merge(this.config, a);
        var callback = this.config.callback;
        var scriptNode = document.createElement("script");
        scriptNode.setAttribute("id", this.config.id);
        scriptNode.setAttribute('charset', this.config.charset);
        scriptNode.setAttribute('type', 'text/javascript');
        scriptNode.setAttribute('src', this.config.url + "?r=" + r);
        var head = document.getElementsByTagName("head")[0];
        head.appendChild(scriptNode);
        scriptNode[document.all ? "onreadystatechange" : "onload"] = function () {
            if (!this.readyState || this.readyState == "loaded" || this.readyState == "complete") {
                if (callback) callback();
                scriptNode.onreadystatechange = scriptNode.onload = null;
                scriptNode.parentNode.removeChild(scriptNode)
            }
        };
    },
    loadScript: function (a) {
        var c, r = Math.floor(Math.random() * 10000);
        this.config = this.merge(this.config, a);
        a = document.getElementById(this.config.id)
        a = document.createElement("script");
        a.setAttribute("id", this.config.id);
        a.setAttribute("type", "text/javascript");
        a.setAttribute("charset", this.config.charset);
        a.setAttribute("src", this.config.url + "?r" + r);
        var b = document.getElementsByTagName("script")[0];
        b.parentNode.insertBefore(a, b);
        c = this.config.callback;
        a.onload = a.onreadystatechange = function () {
            ("undefined" == typeof this.readyState || "loaded" == this.readyState || "complete" == this.readyState) && c()
        }
    },
    loadAjax: function (url, data, fn, beforeSend, complete) {
        var isLoading = isLoading || false;
        jQuery.ajax({
            url: url,
            type: "POST",
            data: data || {},
            beforeSend: function () {
                beforeSend && beforeSend();
            },
            success: function (result) {
                fn && fn(result);
            },
            complete: function () {
                complete && complete();
            },
            error: function (result) {
                if (console && console.log)
                    console && console.log(result);
            }
        });
    },
    parseQueryString: function (url) {
        var reg_url = /^[^\?]+\?([\w\W]+)$/,
            reg_para = /([^&=]+)=([\w\W]*?)(&|$|#)/g,
            arr_url = reg_url.exec(url),
            ret = {};
        if (arr_url && arr_url[1]) {
            var str_para = arr_url[1], result;
            while ((result = reg_para.exec(str_para)) != null) {
                ret[result[1]] = result[2];
            }
        }
        return ret;
    },
    getQueryString: function (name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
        var r = decodeURI(window.location.search).substr(1).match(reg);
        if (r != null) return unescape(r[2]);
        return null;
    },
    stopEventBubble: function (event) {
        var e = event || window.event;
        if (e && e.stopPropagation) {
            e.stopPropagation();
        }
        else {
            e.cancelBubble = true;
        }
        return false;
    },
    parsePercentToDecimal: function (percent) {
        return Number(percent.replace('%', '')) / 100;
    }
}

/***
** 功能：  cookie操作对象
** Author: Allen Zhang
** RTX：   14002
***/
var cookies = {
    /***
    ** 功能：  写入cookie操作
    ** 参数：  name cookie名称
    **         value cookie值 
    **         expires 过期时间
    **         path  路径
    **         domain 域
    ***/
    set: function (name, value, expires, path, domain) {
        expires = new Date(new Date().getTime() + (((typeof expires == "undefined") ? 12 * 7200 : expires)) * 1000);
        var tempcookie = name + "=" + escape(value) +
            ((expires) ? "; expires=" + expires.toGMTString() : "") +
            ((path) ? "; path=" + path : "; path=/") +
            ((domain) ? "; domain=" + domain : "");
        (tempcookie.length < 4096) ? document.cookie = tempcookie : alert("The cookie is bigger than cookie lagrest");
    },

    /***
    ** 功能：  获取cookie操作
    ** 参数：  name cookie名称
    ***/
    get: function (name) {
        var xarr = document.cookie.match(new RegExp("(^| )" + name + "=([^;]*)(;|$)"));
        if (xarr != null)
            return unescape(xarr[2]);
        return null;
    },

    /***
    ** 功能：  删除cookie操作
    ** 参数：  name cookie名称
    **         path  路径
    **         domain 域
    ***/
    del: function (name, path, domain) {
        if (this.get(name))
            document.cookie = name + "=" +
                ((path) ? "; path=" + path : "; path=/") +
                ((domain) ? "; domain=" + domain : "") +
                ";expires=Thu, 01-Jan-1970 00:00:01 GMT";
    },
    day: function (xd) {
        return xd * 24 * 3600;
    },
    hour: function (xh) {
        return xh * 3600;
    }
}
/***
** 功能：  选项卡切换
** Author: Allen Zhang
** RTX：   14002
***/
jQuery.fn.Tabs = function (options) {
    var defaults = {
        tabSelector: ".tabs li",
        conSelector: ".tabcontent",
        focusClass: "c",
        moreTrigger: ".tabTitle .more .link",
        events: "mouseover",
        selected: 0,
        delay: 0.2
    };
    var events = ["mouseover", "click"];
    var settings = jQuery.extend({}, defaults, options);
    var that = this;
    var _tabs = jQuery(settings.tabSelector, that);
    var _cons = jQuery(settings.conSelector, that);
    var _more = jQuery(settings.moreTrigger, that);
    var _isDelay = settings.events == events[0] ? !0 : !1;

    void function () {
        var tab = _tabs.eq(settings.selected);
        if (tab && tab.length == 0) {
            tab = _tabs.eq(0);
        }
        tab.addClass(settings.focusClass);
        tab.siblings(settings.tabSelector).removeClass(settings.focusClass);

        var cons = _cons.eq(settings.selected);
        if (cons && cons.length == 0) {
            cons = _cons.eq(0);
        }
        cons.show();
        cons.siblings(settings.conSelector).hide();

        var more = _more.eq(settings.selected);
        if (more && more.length == 0) {
            more = _more.eq(0);
        }
        more.show();
        more.siblings().hide();
    }();

    _tabs.each(function (i, v) {
        jQuery(v).on(settings.events, function () {
            var _this = this;
            delay.apply(this, [settings.delay, function () {
                jQuery(_this).addClass(settings.focusClass);
                jQuery(_this).siblings(settings.tabSelector).removeClass(settings.focusClass);
                jQuery(_cons[i]).show();
                jQuery(_cons[i]).siblings(settings.conSelector).hide();
                jQuery(_more[i]).show();
                jQuery(_more[i]).siblings().hide();
            }, _isDelay])
        });
    });
    //接收两个参数 t延迟时间秒为单位，fn要执行的函数,m是否执行延迟取决于事件的类型
    var delay = function (t, fn, m) {
        if (m) {
            var _this = this,
                d = setInterval(function () {
                    fn.apply(_this);
                }, t * 1000);
            _this.onmouseout = function () {
                clearInterval(d);
            };
        }
        else fn.apply(this);
    }
}

var setMenuStatus = function () {
    //var href = window.location.href.toLowerCase();
    var $menu = jQuery("a[data-page]", ".top_ul");
    var count = 0;
    var flag = false;
    $menu.each(function (i, ele) {
        var selectedMenu = jQuery(ele).attr("data-page");
        //var index = href.indexOf(selectedMenu);
        //if (index > 0) {

        if (page_type && page_type == selectedMenu) {
            flag = true;
            count++;
            if (count > 1) {
                return;
            }
            var current_a = jQuery("a[data-page='" + selectedMenu + "']");

            if (current_a.length > 0) {
                current_a = current_a.eq(current_a.length - 1)
            }

            var current_mid_li = current_a.parents("li.mid_li");
            var current_top_li = current_a.parents("li.top_li");


            if (current_mid_li && current_mid_li.hasClass("cos")) {
                current_mid_li.removeClass("cos").addClass("exd");
            } else if (current_mid_li && current_mid_li.hasClass("exd")) {
                current_mid_li.removeClass("exd").addClass("cos");
            }
            if (current_top_li && current_top_li.hasClass("cos")) {
                current_top_li.removeClass("cos").addClass("exd");
            } else if (current_top_li && current_top_li.hasClass("exd")) {
                current_top_li.removeClass("exd").addClass("cos");
            }

            //ie6兼容性调整
            if (is_ie6() && !current_mid_li.hasClass("curr")) {
                jQuery(".mid_li.exd h5").css("background-color", "#e5f3ff").css("border", "none");
                jQuery(".mid_li.cos h5").css("background-color", "#e5f3ff").css("border", "none");
                jQuery(".top_li ul").css("border", "none");
            }

            var current_sub_li = current_a.parents("li.sub_li");

            if (current_sub_li.length > 0) {
                current_sub_li.addClass("curr");
            }
            else if (current_mid_li.length > 0) {
                current_mid_li.addClass("curr");

                //ie6兼容性调整
                if (is_ie6() && current_mid_li.hasClass("curr")) {
                    current_mid_li.css("background-color", "#fff");
                    current_mid_li.find("h5").css("background-color", "#fff");
                    //current_mid_li.css("border", "none");
                    if (current_mid_li.hasClass("exd") || current_mid_li.hasClass("cos")) {
                        current_mid_li.find("h5").css("margin-left", "-7px");
                    } else {
                        current_mid_li.css("padding-left", "17px");
                    }
                }
            }

        }

    })
    //ie6兼容性调整
    if (is_ie6() && !flag) {
        jQuery(".mid_li.exd h5").css("background-color", "#e5f3ff").css("border", "none");
        jQuery(".mid_li.cos h5").css("background-color", "#e5f3ff").css("border", "none");
        jQuery(".top_li ul").css("border", "none");
    }

    (function ($) {

        try {

            if (typeof Cookies == undefined) return;

            var records = Cookies.Get('RECORDS');

            //if (!!records) {
            //    records = records.split(',');
            //    var html = ['<div class="record-wrap"><div class="record-btn"><i class="closeBtn" tracker-eventcode="sjpd_sjzxsy_sjzxggsj_gbzjfw"></i></div><div class="record-con"><div class="right"></div> <ul class="center" id="record-list">'];
            //    for (var i = records.length - 1; i >= 0; i--) {
            //        if (records[i] == '') {
            //            records.splice(i, 1)//删除空白元素
            //            continue;
            //        }
            //        var item = records[i].split('|');
            //        if (item[0] && item[1]) {
            //            html.push('<li><a href="' + item[1] + '" target="_blank" tracker-eventcode="sjpd_sjzxsy_sjzxggsj_djzjfw">' + item[0] + '</a></li>')
            //        }

            //    }
            //    html.push('</ul><div class="left"></div></div></div>')
            //    html = html.join('');
            //    $('body').append(html)

            //}

            //var seter;
            //var isDeep = false;
            //$(window).scroll(function () {
            //    if (!isDeep) {
            //        $('.record-wrap .record-btn').addClass('deep');
            //    }
            //    window.clearTimeout(seter)
            //    seter = setTimeout(function () {
            //        $('.record-wrap .record-btn').removeClass('deep')
            //        isDeep = false;
            //    }, 300)

            //    isDeep = true;
            //})

            //$('.record-wrap').hover(function () {
            //    $('.record-wrap').toggleClass('active')
            //    $('.record-wrap .record-btn').toggleClass('icons')
            //    try {
            //        window.sendTrackLog('div', 'MouseOver', 'sjpd_sjzxsy_sjzxggsj_xfzjfw', '');
            //    } catch (error) {

            //    }

            //})

            //$('.record-wrap .closeBtn').click(function () {
            //    $('.record-wrap').remove();
            //})


            if (typeof page_type == 'undefined') return;
            if (page_type === '' || page_type === null || page_type === 'undefined') {
                return;
            }

            records = !!records ? records.split(',') : [];
            var $menu = $('.top_ul');
            var item = $('a[data-page="' + page_type + '"]', $menu);
            if (item.length > 0) {
                item = item.eq(0)
            }
            var href = item.attr("href") || '';
            var name = item.text() || '';
            if (href == '' || name == '' || name.length > 10) {
                return;
            }

            var val = name + "|" + href;

            if ($.inArray(val, records) == -1) {
                if (records.length >= 5) {
                    records.shift();
                }
                records.push(val)

                Cookies.Set("RECORDS", records.join(","))

            }
        } catch (error) {

        }

    })(jQuery)



    // News right
    var fixedBodyWidth = window.emRightBodyWidth || 1000;
    var fixedwidth = window.emRightAdFixedWidth || 138;
    var adwidth = window.emRightAdWidth || 135;
    var adheight = window.emRightAdHeight || 1841;
    //var adurl = window.emRightAdUrl || 'http://fundact.eastmoney.com/banner/Hot_Em_DC.html?spm=100015.ra';
    var adtop = window.emRightAdTop || '43px';
    var adzindex = window.rightAdZindex || 99;
    var addefault = window.emRightadDataType || "default";

    // 1显示返回顶部；0不显示返回顶部

    var switchbacktop = (window.switchBackTop == undefined && true) || (window.switchBackTop == 1 && true) || false;
    var backtopheight = window.backTopHeight || 1000;

    var browser = {
        isie6: function () {
            return document.all && !window.XMLHttpRequest;
        }
    };

    var isie6 = browser.isie6();

    function addEvent(obj, type, fn) {
        if (obj.attachEvent) {
            obj['e' + type + fn] = fn;
            obj[type + fn] = function () { obj['e' + type + fn](window.event); }
            obj.attachEvent('on' + type, obj[type + fn]);
        } else
            obj.addEventListener(type, fn, false);
    }

    var adurl = function (type) {
        var e = "";
        switch (type) {
            case "quote":
                e = "http://fundact.eastmoney.com/banner/Hot_Em.html?spm=100002.rw";
                break;
            case "data":
                e = " http://fundact.eastmoney.com/banner/Hot_Em.html?spm=100004.rw";
                break;
            case "guba":
                e = "http://fundact.eastmoney.com/banner/Hot_Em.html?spm=100005001.rw";
                break;
            default:
                e = " http://fundact.eastmoney.com/banner/Hot_Em.html?spm=001004.rw"
        }
        var n = new Date;
        return e + "&rmd=" + n.getFullYear().toString().substring(2) + (n.getMonth() + 1) + n.getDate() + n.getHours() + Math.round(n.getMinutes() / 5);
    }

    var togo = function () {
        if (!document.getElementById("rightAD")) {
            var html = '<iframe width="' + adwidth + '" height="' + adheight + '" frameborder="0" scrolling="no" marginwidth="0" marginheight="0" src="' + adurl(addefault) + '"></iframe>';
            html += '<div id="quoteQRcode" style="position: fixed; bottom: 30px; z-index: 999999; display: block; margin-left:-12px;"><img src="http://hqres.eastmoney.com/emag14/images/quoteCode.png"></div>';
            var divObj = document.createElement("div");
            divObj.className = "rbadbox";
            divObj.id = "rightAD";
            divObj.innerHTML = html;
            divObj.style.height = adheight;
            divObj.style.width = adwidth;
            divObj.style.position = 'absolute';
            divObj.style.top = adtop;
            divObj.style.zIndex = 99;
            document.body.appendChild(divObj);
            //setInterval(reset_fixedbound, 200);
            addEvent(window, 'resize', function () {
                reset_fixedbound();
            });
            reset_fixedbound();

            document.getElementById("quoteQRcode").onclick = function () {
                document.getElementById("quoteQRcode").style.display = "none";
            }
        }
    }

    // 获取浏览器的宽度和高度
    var getBrowserWH = function () {
        var de = document.documentElement;
        var _width = (de && de.clientWidth) || document.body.clientWidth || window.innerWidth;
        var _height = (de && de.clientHeight) || document.body.clientHeight || window.innerHeight;
        return { w: _width, h: _height };
    };

    var reset_fixedbound = function () {
        var flowfixed_ad = document.getElementById("rightAD");
        var _wh = getBrowserWH();
        var _fr = ((document.documentElement.clientWidth - fixedBodyWidth) / 2 - fixedwidth - 5);
        if (parseFloat(flowfixed_ad.style.right) != _fr)
            flowfixed_ad.style.right = _fr + 'px';
        if (_wh.w <= fixedBodyWidth + adwidth * 2 + 20) {
            flowfixed_ad.style.display = "none";
        } else {
            flowfixed_ad.style.display = "block";
        }
    };
    //togo();

    // 返回顶部
    var backtop = {
        init: function () {
            this.create();
            this.bind();
        },
        create: function () {
            var fixWidth = fixedBodyWidth / 2 + 8;
            var backtopcss = '#embacktop {width: 18px;position: fixed;bottom: 250px;left: 50%;margin-left:' + fixWidth + 'px;text-align: center;_position: absolute;z-index: 9999;display: none;_position: absolute;}#embacktop #backtophq, #embacktop #backtopyj, #embacktop #gotop {display: block;width: 50px;height: 51px;background: url(http://g1.dfcfw.com/g2/201607/20160728143011.png) no-repeat;font-size: 12px;}#embacktop #backtophq {background-position: 0 0;color: #fff;text-decoration: none;padding-top: 35px;height: 16px;} #embacktop #backtophq:hover {background-position: -50px 0;}#embacktop #backtophq.on {background-position: -102px -2px;background-color: #3A5E95;}  #embacktop #backtopyj {background-position: 0 -115px;color: #fff;text-decoration: none;height: 51px;margin-top: 4px;}#embacktop #backtopyj:hover {background-position: -60px -115px;}#embacktop #gotop {background-position: 0 -50px;margin-top: 4px;display: block;}#embacktop #gotop:hover {background-position: -50px -50px;}#embacktop #backtopsearch {position: absolute;left: -237px;top: 0;background-color: #3A5E95;height: 36px;width: 238px;display: none;padding-top: 15px;}#embacktop #backtopsearch form {margin: 0;padding: 0;display: inline;position: relative;}#embacktop #backtopsearch input {width: 123px;padding: 4px;font-size: 12px;font-family: simsun;border: 0;height: 16px;vertical-align: middle;}#embacktop #backtopsearch #backtopsearchsbm {border-style: none;border-color: inherit;border-width: 0;width: 60px;color: #315895;height: 24px;background: url(http://g1.dfcfw.com/g2/201607/20160728143011.png) 5px -174px no-repeat #BBD4E8;text-align: right;padding-right: 7px;vertical-align: middle;cursor: pointer;}';
            this.backtop = '<a href="javascript:;" target="_self" id="backtophq">行情</a><a href="http://corp.eastmoney.com/Lianxi_liuyan.asp" target="_blank" id="backtopyj" title="意见反馈"></a><a href="javascript:;" target="_self" id="gotop" title="回到顶部" onclick="window.scroll(0,0);return false;"></a><div id="backtopsearch"><input type="text" value="" id="backtopsearchinput" /><button id="backtopsearchsbm">查询</button></div>';
            var _style = document.createElement("style");
            _style.type = "text/css";
            if (_style.styleSheet) {         //ie下  
                _style.styleSheet.cssText = backtopcss;
            } else {
                _style.innerHTML = backtopcss;
            }
            var _backtop = document.createElement("div");
            _backtop.id = "embacktop";
            _backtop.innerHTML = this.backtop;
            document.body.appendChild(_style);
            document.body.appendChild(_backtop);
        },
        open: false,
        bind: function () {
            var _this = this;
            // 行情查询
            this.btnsearch = document.getElementById("backtopsearchsbm");
            this.btnsearch.onclick = function () {
                var backtopsearchinput = document.getElementById("backtopsearchinput");
                var _value = backtopsearchinput.value;
                if (_value != "" && _value != "代码/名称/拼音") {
                    _value = encodeURI(_value);
                    window.open('http://quote.eastmoney.com/search.html?stockcode=' + _value);
                }
                else {
                    backtopsearchinput.focus();
                }
            };

            // 隐藏、显示行情
            this.backtophq = document.getElementById('backtophq');
            this.backtopsearch = document.getElementById('backtopsearch');
            this.backtophq.onclick = function () {
                if (_this.open) {
                    this.className = "";
                    _this.backtopsearch.style.display = "none";
                    _this.open = false;
                }
                else {
                    this.className = "on";
                    _this.backtopsearch.style.display = "block";
                    _this.open = true;
                }

            };

            if (typeof StockSuggest != "undefined") {
                var arg = {
                    text: "代码/名称/拼音",
                    autoSubmit: false,
                    width: 235,
                    type: "CNSTOCK",
                    header: ["选项", "代码", "名称", "类型"],
                    body: [-1, 1, 4, -2],
                    callback: function (ag) {
                        window.open('http://quote.eastmoney.com/' + ag.code + '.html');
                        return false;
                    }
                };
                s1001 = new StockSuggest("backtopsearchinput", arg);
            }

            setInterval(function () {
                if (isie6) {
                    document.getElementById("embacktop").style.bottom = "auto";
                    document.getElementById("embacktop").style.top = document.documentElement.scrollTop + 500;
                }
                if ((document.documentElement.scrollTop + document.body.scrollTop) > backtopheight) {
                    document.getElementById("embacktop").style.display = "block";
                    //_this.backtop.fadeIn();
                }
                else {
                    document.getElementById("embacktop").style.display = "none";
                    //_this.backtop.fadeOut();
                }

                return true;
            }, 500);
        }
    }

    switchbacktop && backtop.init();
}







var setOpen = function (opend) {
    if (opend < 0) {
        jQuery(".top_li.cos", ".vnav").removeClass("cos").addClass("exd");
    }
}

jQuery.fn.navigator = function () {
    var opend = 100;//全部展开
    var $header = jQuery(".top_li", ".vnav");
    var $content = jQuery(".mid_ul", ".vnav");
    var $trigger = jQuery(".top_li h4, .mid_li h5", ".vnav");
    var config = [];

    void function ($) {
        setOpen(opend);
        //setMenuStatus();
        $trigger.on("click", function (event) {

            if ($(this).parent().hasClass("hotcos")) {
                return;
            }
            else if ($(this).parent().hasClass("cos")) {
                if ($(this).parent().hasClass("top_li")) {
                    $('.top_ul .top_li').addClass('cos').first().removeClass("cos");
                }
                $(this).parent().removeClass("cos").addClass("exd");
                $('html,body').animate({ scrollTop: '330px' }, 300);
                //ie6兼容性调整
                if (is_ie6() && $(this).parent().hasClass("curr") && $(this).parent().hasClass("mid_li")) {
                    var current_mid_li = $(this).parent();
                    current_mid_li.find("h5").css("background", "url('/images/data-icon.gif') no-repeat scroll 17px -207px #fff");
                }

            } else if ($(this).parent().hasClass("exd")) {
                $(this).parent().removeClass("exd").addClass("cos");
                $('html,body').animate({ scrollTop: '330px' }, 300);
                //ie6兼容性调整
                if (is_ie6() && $(this).parent().hasClass("curr") && $(this).parent().hasClass("mid_li")) {
                    $(this).parent().find("h5").css("background", "url('/images/data-icon.gif') no-repeat scroll 17px -249px #e5f3ff");
                }
            }
            utools.stopEventBubble(event);
        });

        //隐藏左侧导航第一个二级菜单上边框
        var top_lis = $(".navbody>.top_ul>.top_li");
        top_lis.each(function (index, ele) {
            $(ele).find("ul.mid_ul>li.mid_li").first().addClass("remove_top_border");
        })
    }(jQuery);
}


/***
注册事件
***/
function addEvent(obj, type, fn) {
    if (obj.addEventListener) {
        obj.addEventListener(type, fn, false);
    } else if (obj.attachEvent) {
        obj.attachEvent('on' + type, fn);
    }
}

//通用函数
var Tools = {
    /*
    *获取数据
    */
    getJsonData: function (url, callback, fail) {
        jQuery.ajax({
            url: url,
            method: 'get',
            dataType: 'jsonp',
            jsonp: 'jsonp_callback',
            success: function (data) {
                try {
                    if (data.code == 0) {
                        callback && callback(data)
                    } else {
                        fail && fail();
                    }
                } catch (err) {
                    fail && fail(err);
                }
            },
            error: function (err) {
                fail && fail(err);
            }
        })
    },
    getJsonData2: function (url, callback, fail) {
        jQuery.ajax({
            url: url,
            method: 'get',
            dataType: 'jsonp',
            success: function (data) {
                try {
                    if (data) {
                        callback && callback(data)
                    } else {
                        fail && fail();
                    }
                } catch (err) {
                    fail && fail(err);
                }
            },
            error: function (err) {
                fail && fail(err);
            }
        })
    },
    /*
    *判断值是否非空
    */
    getTextValOrEmpty: function (value) {
        if (value != '' && value != undefined && value != null) {
            return value
        } else {
            return '-'
        }
    },
    /*
    *单位自动换算
    */
    FixAmt: function (str, num, fix, ride) {
        try {
            if (str === '' || str === undefined || str === null || str === '-' || isNaN(str)) {
                return '-';
            }
            var result;
            fix = !!fix ? fix : '';
            num = isNaN(parseFloat(num)) ? 2 : parseFloat(num);
            ride = !!ride ? ride : 1;
            str = parseFloat(str) * parseInt(ride);
            var intStr = Math.abs(parseInt(str));
            if (intStr.toString().length > 12) {
                result = (parseFloat(str) / 1000000000000).toFixed(num) + '万亿' + fix;
            } else if (intStr.toString().length > 8) {
                result = (parseFloat(str) / 100000000).toFixed(num) + '亿' + fix;
            } else if (intStr.toString().length > 4) {
                result = (parseFloat(str) / 10000).toFixed(num) + '万' + fix;
            } else {
                result = parseFloat(str).toFixed(num) + fix;
            }
            // console.log(result)
            return result;
        } catch (err) {
            return '-'
        }
    },
    /*
    *换算成百分比
    */
    toPercent: function (data) {
        if (data === '' || data === undefined || data === null || data === '-' || isNaN(data)) {
            return '-';
        }
        return !isNaN(parseFloat(data)) ? parseFloat(data).toFixed(2) + '%' : '-';
    },
    toFixed: function (data) {
        if (data === '' || data === undefined || data === null || data === '-' || isNaN(data)) {
            return '-';
        }

        return parseFloat(data).toFixed(2);
    },
    /**
    * @param {data}字符串 
    * @param {num}保留小数点几位
    * @param {fix}单位
    * @param {divide} 除以多少，eg:10000 ,如果没有则自动填充
    */
    getColorByDate: function (data, num, fix, divide) {
        try {
            if (data === '' || data === undefined || data === null || data === '-' || isNaN(data)) {
                return '<span>-</span>'
            }
            data = parseFloat(data);
            var retult = '';

            if (!divide) {
                retult = this.FixAmt(data, num, fix);
            } else {
                num = !!parseFloat(num) ? parseFloat(num) : 2;
                retult = (data / parseInt(divide)).toFixed(num) + fix;
            }

            var color = '';
            color = !!data ? (data == 0 ? '' : data > 0 ? 'red' : 'green') : '';
            retult = '<span class="' + color + '">' + retult + '</span>'

            return retult;
        } catch (err) {
            return '-'
        }
    },

    getDescribe: function (data, num, fix, divide, abs, arr) {
        try {
            if (data === '' || data === undefined || data === null || data === '-' || isNaN(data)) {
                return '<span>-</span>'
            }
            data = parseFloat(data);
            if (data == 0) {
                return "无变化"
            }
            var retult = '';
            if (!arr) {
                arr = ['上升', '下降']
            }
            if (!divide) {
                retult = this.FixAmt(data, num, fix);
            } else {
                num = !!parseFloat(num) ? parseFloat(num) : 2;

                if (abs) {
                    retult = Math.abs((data / parseInt(divide)).toFixed(num)) + fix;
                } else {
                    retult = (data / parseInt(divide)).toFixed(num) + fix;
                }
            }


            var color = '';
            var desc = '';
            color = !!data ? (data == 0 ? '' : data > 0 ? 'red' : 'green') : '';
            desc = !!data ? (data >= 0 ? arr[0] : arr[1]) : '';
            retult = desc + '<span class="' + color + '">' + retult + '</span>'

            return retult;
        } catch (err) {
            return '-'
        }
    },
    isExist: function (data) {
        if (data !== undefined && data !== null && data !== '-' && data !== '') {
            return true;
        }
        return false;
    },
    getColor: function (data) {
        data = parseFloat(data);
        return !!data ? (data == 0 ? '' : data > 0 ? 'red' : 'green') : '';
    },
    //时间戳转化格式
    dateFormat: function (str, type) {

        if (str === '' || str === undefined || str === null || str === '-') {
            return '-';
        }

        try {
            type = !!type ? type : 'yyyy-MM-dd';
            var retDate = new Date(str);
            if (isNaN(retDate))
                retDate = this.parseISO8601(str);
            return retDate.format(type);
        } catch (err) {
            return '-';
        }
    },
    //生成日期parseISO8601("2016-9-5")
    parseISO8601: function (dateStringInRange) {
        var isoExp = /^\s*(\d{4})-(\d\d)-(\d\d)\s*/,
            date = new Date(NaN), month,
            parts = isoExp.exec(dateStringInRange);

        if (parts) {
            month = +parts[2];
            date.setFullYear(parts[1], month - 1, parts[3]);
            if (month != date.getMonth() + 1) {
                date.setTime(NaN);
            }
        }
        return date;
    },
    //2018-09-27T00:00:00格式转化
    dateFormat2: function (dateS, part) {
        if (dateS == "-" || typeof dateS == "undefined") {
            return '-';
        }
        if (dateS.length > 10) {
            dateS = dateS.split('T')[0].replace(/-/g, "/");
        }
        var date = new Date(dateS);
        var datecopy;
        var redate = "";
        part = (part == null) ? "yyyy-MM-dd HH:mm:ss" : part;
        var y = date.getFullYear();
        var M = date.getMonth() + 1;
        var d = date.getDate();
        var H = date.getHours();
        var m = date.getMinutes();
        var s = date.getSeconds();
        var MM = (M > 9) ? M : "0" + M;
        var dd = (d > 9) ? d : "0" + d;
        var HH = (H > 9) ? H : "0" + H;
        var mm = (m > 9) ? m : "0" + m;
        var ss = (s > 9) ? s : "0" + s;
        redate = part.replace("yyyy", y).replace("MM", MM).replace("dd", dd).replace("HH", HH).replace("mm", mm).replace("ss", ss).replace("M", M).replace("d", d).replace("H", H).replace("m", m).replace("s", s);
        return redate;
    },
    cutText: function (str, num) {
        if (str === '' || str === undefined || str === null || str === '-') {
            return '-';
        }
        if (!num) {
            return str;
        }
        str = str.toString().replace(/(&#x0D;)/g, ',');
        num = parseInt(num);
        if (str.length > num) {
            return str.substring(0, num) + '...'
        } else {
            return str;
        }
    },
    //默认取25宽度的长度
    cutTitle: function (str) {
        if (str === '' || str === undefined || str === null || str === '-') {
            return '-';
        }
        str = str.replace(/(.{25})/g, "$1&#10;")
        return str
    },
    /**
    *计算行情中心数据
    * @param {data}数据 
    * @param {zoom}缩放大小
    */
    computQuoteDate: function (data, zoom) {
        try {
            if (data === '' || data === undefined || data === null || data === '-') {
                return '-';
            }

            zoom = zoom || 1;
            //console.log(parseFloat(data) / Math.pow(10, parseInt(zoom)))
            return parseFloat(data) / Math.pow(10, parseInt(zoom)) || '-';
        } catch (err) {
            return "-"
        }
    },
    //币种链接
    getBiLink: function (type, name) {
        if (!name) {
            return '-';
        }
        if (!type) {
            return name;
        }
        type = type.toUpperCase();
        var result = '';
        switch (type) {
            case 'ZAR':
                result = '<a href="http://quote.eastmoney.com/cnyrate/CNYZARC.html">' + name + '</a>';
                break;
            case 'GBP':
                result = '<a href="http://quote.eastmoney.com/cnyrate/GBPCNYC.html">' + name + '</a>';
                break;
            case 'EUR':
                result = '<a href="http://quote.eastmoney.com/cnyrate/EURCNYC.html">' + name + '</a>';
                break;
            case 'KRW':
                result = '<a href="http://quote.eastmoney.com/cnyrate/CNYKRWC.html">' + name + '</a>';
                break;
            case 'RUB':
                result = '<a href="http://quote.eastmoney.com/cnyrate/CNYRUBC.html">' + name + '</a>';
                break;
            case 'MYR':
                result = '<a href="http://quote.eastmoney.com/cnyrate/CNYMYRC.html">' + name + '</a>';
                break;
            case 'CHF':
                result = '<a href="http://quote.eastmoney.com/cnyrate/CHFCNYC.html">' + name + '</a>';
                break;
            case 'NOK':
                result = '<a href=" http://quote.eastmoney.com/cnyrate/CNYNOKC.html">' + name + '</a>';
                break;
            case 'AED':
                result = '<a href="http://quote.eastmoney.com/cnyrate/CNYAEDC.html">' + name + '</a>';
                break;
            case 'SAR':
                result = '<a href="http://quote.eastmoney.com/cnyrate/CNYSARC.html">' + name + '</a>';
                break;
            case 'AUD':
                result = '<a href="http://quote.eastmoney.com/cnyrate/AUDCNYC.html">' + name + '</a>';
                break;
            case 'THB':
                result = '<a href="http://quote.eastmoney.com/cnyrate/CNYTHBC.html">' + name + '</a>';
                break;
            case 'SGD':
                result = '<a href="http://quote.eastmoney.com/cnyrate/SGDCNYC.html">' + name + '</a>';
                break;
            case 'CAD':
                result = '<a href="http://quote.eastmoney.com/cnyrate/CADCNYC.html">' + name + '</a>';
                break;
            case 'HKD':
                result = '<a href="http://quote.eastmoney.com/cnyrate/HKDCNYC.html">' + name + '</a>';
                break;
            case 'USD':
                result = '<a href="http://quote.eastmoney.com/cnyrate/USDCNYC.html">' + name + '</a>';
                break;
            case 'JPY':
                result = '<a href="http://quote.eastmoney.com/cnyrate/JPYCNYC.html">' + name + '</a>';
                break;
            case 'MXN':
                result = '<a href="http://quote.eastmoney.com/cnyrate/CNYMXNC.html">' + name + '</a>';
                break;
            case 'SEK':
                result = '<a href="http://quote.eastmoney.com/cnyrate/CNYSEKC.html">' + name + '</a>';
                break;
            case 'DKK':
                result = '<a href="http://quote.eastmoney.com/cnyrate/CNYDKKC.html">' + name + '</a>';
                break;
            case 'TRY':
                result = '<a href="http://quote.eastmoney.com/cnyrate/CNYTRYC.html">' + name + '</a>';
                break;
            default:
                result = name;
        }
        return result;
    },
    getCutNum: function (str) {
        if (str === '' || str === undefined || str === null || str === '-') {
            return '-';
        }
        var result = str.toString();
        var i = str.toString().indexOf(".");
        var num = parseFloat(str);
        if (num < 0) {
            i = i - 1;
        }
        if (i >= 4) {
            num = num.toFixed(0);
            result = num.toString();
        }
        else if (i == 3) {
            num = num.toFixed(1);
            result = num.toString();
        }
        return result;

    }
}