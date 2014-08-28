var ttTools = ttTools || {};

ttTools.cloudUrl = "WebFrameworkService/";

ttTools.isInApp = function () {
    var local=ttTools.cloudUrl.indexOf("http://") === -1 &&
        ttTools.cloudUrl.indexOf("https://") === -1;
    //var local = document.URL.indexOf("http://") === -1 &&
    //    document.URL.indexOf("https://") === -1;

    return local;
};

ttTools.getBaseUrl = function () {
    if (!ttTools.isInApp()) {
        return ttTools.cloudUrl;
    }
    else {
        var p = window.location.pathname.split("/");
        var u = window.location.protocol + "//" + window.location.host + "/";

        //if (p.length > 2) {
        //    u = u + p[1] + "/";
        //}
        u = u + ttTools.cloudUrl;
        return u;
    }
};

ttTools.baseUrl = ttTools.getBaseUrl();

ttTools.iOS = function () {
    return (navigator.userAgent.match(/(iPad|iPhone|iPod)/g) ? true : false);
};

ttTools.lowercaseFirstLetter = function (string) {
    return string.charAt(0).toLowerCase() + string.slice(1);
};

ttTools.startHub = function (hub) {
    if (ttTools.iOS()) {
        return hub.start({ transport: "longPolling" });
    } else {
        return hub.start();
    }
};

ttTools.stopHub = function (hub) {
    hub.stop();
};

ttTools.initLogger = function (url) {
    ttTools.JsonAppender.prototype = new log4javascript.Appender();
    ttTools.JsonAppender.prototype.toString = function () {
        return 'JsonAppender';
    };
    log4javascript.JsonAppender = ttTools.JsonAppender;

    ttTools.logger = log4javascript.getLogger();

    var ajaxAppender = new log4javascript.JsonAppender(url);
    ajaxAppender.setThreshold(log4javascript.Level.DEBUG);
    ttTools.logger.addAppender(ajaxAppender);

    var consoleAppender = new log4javascript.BrowserConsoleAppender();
    var patternLayout = new log4javascript.PatternLayout("%d{HH:mm:ss,SSS} %-5p - %m{1}%n");
    consoleAppender.setLayout(patternLayout);
    ttTools.logger.addAppender(consoleAppender);
};

ttTools.JsonAppender = function (url) {
    var isSupported = true;
    var successCallback = function (data, textStatus, jqXHR) { return; };

    if (!url) {
        isSupported = false;
    }

    this.setSuccessCallback = function (successCallbackParam) {
        successCallback = successCallbackParam;
    };

    this.append = function (loggingEvent) {
        if (!isSupported) {
            return;
        }

        $.ajax({
            url: url,
            type: "POST",
            dataType: "JSON",
            contentType: "application/json",
            data: JSON.stringify({
                'logger': loggingEvent.logger.name,
                'timestamp': loggingEvent.timeStampInMilliseconds,
                'level': loggingEvent.level.name,
                'url': window.location.href,
                'message': loggingEvent.getCombinedMessages()+loggingEvent.getThrowableStrRep()
            }),
            beforeSend: function (xhr) {
                var injector = angular.injector(['Thinktecture.Authentication']);

                if (injector) {
                    var tt = injector.get("tokenAuthentication");
                    //tt.getToken().then(function (tokenData) {
                    //    if (!tokenData) {
                    //    } else {
                    //        xhr.setRequestHeader("Authorization", "Bearer " + tokenData.access_token);
                    //        xhr.setRequestHeader('x-my-custom-header', 'some value');
                    //    }
                    //});
                    var tokenData = tt.getToken();
                    if(tokenData)
                    {
                        xhr.setRequestHeader("Authorization", "Bearer " + tokenData.access_token);
                    }
                }
            }
        });
    };
};

ttTools.getSampleData = function () {
    var injector = angular.element(document.body).injector();
    var articlesApiService = injector.get("articlesApi");

    articlesApiService.getArticlesPaged(10, 1).then(function(resultData) {
        window.cefCallback.sampleDataResult(resultData);
    });
}