window.$app = window.$app || {};
var app;

if (ttMobile) {
    app = angular.module("myApp", ["ui.router", "ngTouch", "ngAnimate", "ngSanitize", "Thinktecture.Dialog", "Thinktecture.Toast", "Thinktecture.SignalR", "Thinktecture.Authentication", "ipCookie", "pascalprecht.translate", "routeResolverServices", "ngStorage", "nvd3ChartDirectives", "jmdobry.angular-cache", "ionic", "angular-loading-bar", "btford.phonegap.ready", "btford.phonegap.geolocation", "common"]);
} else {
    app = angular.module("myApp", ["ui.router", "ngTouch", "ngAnimate", "ngSanitize", "Thinktecture.Dialog", "Thinktecture.Toast", "$strap.directives", "ui.bootstrap", "ui.utils", "Thinktecture.SignalR", "Thinktecture.Authentication", "ipCookie", "pascalprecht.translate", "routeResolverServices", "angular-carousel", "frapontillo.bootstrap-switch", "ngStorage", "imageupload", "nvd3ChartDirectives", "jmdobry.angular-cache", "angular-loading-bar", "btford.phonegap.ready", "btford.phonegap.geolocation", "common"]);
}
var keyCodes = {
    backspace: 8,
    tab: 9,
    enter: 13,
    esc: 27,
    space: 32,
    pageup: 33,
    pagedown: 34,
    end: 35,
    home: 36,
    left: 37,
    up: 38,
    right: 39,
    down: 40,
    insert: 45,
    del: 46
};
toastr.options.positionClass = 'toast-bottom-right';
app.value('redirectToUrlAfterLogin', { url: '/' });
app.factory('appAuth', function ($location, redirectToUrlAfterLogin) {
    return {
        saveAttemptUrl: function () {
            if ($location.path().toLowerCase() != '/login') {
                redirectToUrlAfterLogin.url = $location.path();
            }
            //else
            //    redirectToUrlAfterLogin.url = '/';
        },
        redirectToAttemptedUrl: function () {
            $location.path(redirectToUrlAfterLogin.url);
        },
        gotoLogin: function () {
            $location.path('/login');
        }
    };
});
app.factory('CommonHttpInterceptor', function ($q, $injector) {
    return {
        // On request success
        request: function (config) {
            var tt = $injector.get("tokenAuthentication");
            //tt.getToken().then(function (tokenData) {
            //    if (!tokenData) {
            //    } else {
            //        xhr.setRequestHeader("Authorization", "Bearer " + tokenData.access_token);
            //        xhr.setRequestHeader('x-my-custom-header', 'some value');
            //    }
            //});
            var tokenData = tt.getToken();
            if (tokenData) {
                config.headers["Authorization"] = "Bearer " + tokenData.access_token;
            }
            // Return the config or wrap it in a promise if blank.
            return config || $q.when(config);
        },

        // On request failure
        requestError: function (rejection) {
            // console.log(rejection); // Contains the data about the error on the request.

            // Return the promise rejection.
            return $q.reject(rejection);
        },

        // On response success
        response: function (response) {
            // console.log(response); // Contains the data from the response.
            // Return the response or promise.
            return response || $q.when(response);
        },

        // On response failture
        responseError: function (rejection) {
            // console.log(rejection); // Contains the data about the error.
            if (rejection.status === 401) {

                var appAuth = $injector.get('appAuth');
                appAuth.saveAttemptUrl();
                appAuth.gotoLogin();
            }
            rejection.data.httpStatus = rejection.status;
            // Return the promise rejection.
            return $q.reject(rejection);
        }
    };
});
app.config(["$urlRouterProvider", "$stateProvider", "$locationProvider", "$translateProvider", "$httpProvider", "routeResolverProvider", "$controllerProvider", "$compileProvider", "$filterProvider", "$provide", "cfpLoadingBarProvider", "tokenAuthenticationProvider",
    function ($urlRouterProvider, $stateProvider, $locationProvider, $translateProvider, $httpProvider, routeResolverProvider, $controllerProvider, $compileProvider, $filterProvider, $provide, cfpLoadingBarProvider, tokenAuthenticationProvider) {
        $compileProvider.aHrefSanitizationWhitelist(/^\s*(https?|ftp|mailto|file|ghttps?|ms-appx|x-wmapp0):/);

        cfpLoadingBarProvider.includeSpinner = false;

        tokenAuthenticationProvider.setUrl(ttTools.baseUrl + "token");

        ttTools.initLogger(ttTools.baseUrl + "api/log");
        ttTools.logger.debug("Configuring myApp...");
        app.lazy =
        {
            controller: $controllerProvider.register,
            directive: $compileProvider.directive,
            filter: $filterProvider.register,
            factory: $provide.factory,
            service: $provide.service
        };

        var viewBaseUrl = "";

        if (ttMobile) {
            viewBaseUrl = "mobile/";
        }

        routeResolverProvider.routeConfig.setBaseDirectories(viewBaseUrl, "app/");

        $urlRouterProvider.otherwise("/");

        $stateProvider
           .state('admin', {
               templateUrl: viewBaseUrl + 'admin.html',
               controller:"NavigationController",
               abstract: true,
           })
           .state("login", {
                url: "/login",
                parent: 'admin',
                templateUrl: viewBaseUrl + "login/login.html",
                controller: "LoginController"
            })
          .state("start", {
              url: "/",
              parent: 'admin',
              templateUrl: viewBaseUrl + "start/start.html",
              controller: "StartController"
          }).state("about", {
              url: "/about",
              parent: 'admin',
              templateUrl: viewBaseUrl + "about/about.html",
              controller: "AboutController"
          }).state("logDetail", {
              url: "/logDetail",
              parent: 'admin',
              templateUrl: viewBaseUrl + "logDetail/logDetail.html",
              controller: "LogDetailController"
          }).state("logs", {
              url: "/logs",
              parent: 'admin',
              templateUrl: viewBaseUrl + "logs/logs.html",
              controller: "LogsController"
          }).state("maintenance", {
              url: "/maintenance",
              parent: 'admin',
              templateUrl: viewBaseUrl + "maintenance/maintenance.html",
              controller: "MaintenanceController"
          }).state("offline", {
              url: "/offline",
              parent: 'admin',
              templateUrl: viewBaseUrl + "offline/offline.html",
              controller: "OfflineController"
          }).state("setting", {
              url: "/setting",
              parent: 'admin',
              templateUrl: viewBaseUrl + "setting/setting.html",
              controller: "SettingController"
          }).state("activitylog", {
              url: "/activitylog",
              parent: 'admin',
              templateUrl: viewBaseUrl + "activitylog/activitylog.html",
              controller: "ActivityLogController"
          }).state("authenticationaudit", {
              url: "/authenticationaudit",
              parent: 'admin',
              templateUrl: viewBaseUrl + "authenticationaudit/authenticationaudit.html",
              controller: "AuthenticationAuditController"
          }).state("messagetemplate", {
              url: "/messagetemplate",
              parent: 'admin',
              templateUrl: viewBaseUrl + "messagetemplate/messagetemplate.html",
              controller: "MessageTemplateController"
          }).state("user", {
              url: "/user",
              parent: 'admin',
              templateUrl: viewBaseUrl + "user/user.html",
              controller: "UserController"
          }).state("userrole", {
              url: "/userrole",
              parent: 'admin',
              templateUrl: viewBaseUrl + "userrole/userrole.html",
              controller: "UserRoleController"
          }).state("role", {
              url: "/role",
              parent: 'admin',
              templateUrl: viewBaseUrl + "role/role.html",
              controller: "RoleController"
          }).state("rolepermission", {
              url: "/rolepermission",
              parent: 'admin',
              templateUrl: viewBaseUrl + "rolepermission/rolepermission.html",
              controller: "RolePermissionController"
          }).state("permission", {
              url: "/permission",
              parent: 'admin',
              templateUrl: viewBaseUrl + "permission/permission.html",
              controller: "PermissionController"
          }).state("roleuserlist", {
              url: "/roleuserlist",
              parent: 'admin',
              templateUrl: viewBaseUrl + "roleuserlist/roleuserlist.html",
              controller: "RoleUserListController"
          }).state("account", {
              url: "/account",
              parent: 'admin',
              templateUrl: viewBaseUrl + "account/account.html",
              controller: "AccountController"
          }).state("changepassword", {
              url: "/changepassword/:name",
              params: {
                  name: {
                      value: null
                  }
              },
              parent: 'admin',
              templateUrl: viewBaseUrl + "account/changepassword/changepassword.html",
              controller: "ChangePasswordController"
          }).state("confirmemail", {
              url: "/confirmemail/:id",
              parent: 'admin',
              templateUrl: viewBaseUrl + "account/confirmemail/confirmemail.html",
              controller: "ConfirmEmailController"
          }).state("resetpassword", {
              url: "/resetpassword",
              parent: 'admin',
              templateUrl: viewBaseUrl + "account/resetpassword/resetpassword.html",
              controller: "ResetPasswordController"
          }).state("confirmpasswordreset", {
              url: "/confirmpasswordreset/:key",
              parent: 'admin',
              templateUrl: viewBaseUrl + "account/confirmpasswordreset/confirmpasswordreset.html",
              controller: "ConfirmPasswordResetController"
          }).state("cancelverificationrequest", {
              url: "/cancelverificationrequest/:id",
              parent: 'admin',
              templateUrl: viewBaseUrl + "account/cancelverificationrequest/cancelverificationrequest.html",
              controller: "CancelVerificationRequestController"
          }).state("register", {
              url: "/register",
              parent: 'admin',
              templateUrl: viewBaseUrl + "account/register/register.html",
              controller: "RegisterController"
          });
        $provide.factory("$stateProviderService", function () {
            return $stateProvider;
        });

        $translateProvider.translations("de", tt.translations.de);
        $translateProvider.useStaticFilesLoader({
            prefix: "translations/locale-",
            suffix: ".js"
        });
        $translateProvider.preferredLanguage("en");
        $translateProvider.useLocalStorage();

        $provide.decorator('$exceptionHandler',
    ['$delegate', function ($delegate) {
        return function (exception, cause) {
            $delegate(exception, cause);
            log4javascript.getLogger().error(exception.stack || exception.message || exception || '', cause || '');
            toastr.error(exception.message);
        };
    }]);

        // Add the interceptor to the $httpProvider.
        $httpProvider.interceptors.push('CommonHttpInterceptor');

    }]);

app.run(["$localStorage", "$stateProviderService", "$state", "$http", "$templateCache", "$rootScope", "$location", "$translate", "$angularCacheFactory", "toast", "dialog", "routeResolver", "personalization", "geoLocationTracker", "appAuth", "common",
    function ($localStorage, $stateProviderService, $state, $http, $templateCache, $rootScope, $location, $translate,$angularCacheFactory, toast, dialog, routeResolver, personalization, geoLocationTracker, appAuth, common) {
        geoLocationTracker.startSendPosition(10000, function (pos) { });
        common.setTimezoneCookie();

        window.addEventListener("online", function () {
            $rootScope.$apply($rootScope.$broadcast(tt.networkstatus.onlineChanged, true));
        }, true);
        window.addEventListener("offline", function () {
            $rootScope.$apply($rootScope.$broadcast(tt.networkstatus.onlineChanged, false));
        }, true);

        $http.defaults.headers.common["Accept-Language"] = $translate.uses();
        $rootScope.$on("$translateChangeSuccess", function () {
            $http.defaults.headers.common["Accept-Language"] = $translate.uses();
        });

        var currentPath = $location.path();
        var viewsDir = routeResolver.routeConfig.getViewsDirectory();

        $http({ method: "GET", url: ttTools.baseUrl + "api/personalization" })
        .success(function (data) {
            personalization.data = data;
            var route = routeResolver.route;

            angular.forEach(data.Features, function (value, key) {
                // TODO: check how to add states only when not yet in $state - this is too dirty
                try {
                    $stateProviderService.state(value.Module.toLowerCase(), route.resolve(value));
                    $http.get(viewsDir + value.Module.toLowerCase() + "/" + value.Module.toLowerCase() + ".html", { cache: $templateCache });
                } catch (e) {
                }
            });

            $rootScope.$broadcast(tt.personalization.dataLoaded);
            $location.path(currentPath);
        });

        $rootScope.$on(tt.authentication.loggedIn, function () {
            $http({ method: "GET", url: ttTools.baseUrl + "api/personalization" })
            .success(function (data) {
                personalization.data = data;
                var route = routeResolver.route;

                angular.forEach(data.Features, function (value, key) {
                    // TODO: check how to add states only when not yet in $state - this is too dirty
                    try {
                        $stateProviderService.state(value.Module.toLowerCase(), route.resolve(value));
                        $http.get(viewsDir + value.Module.toLowerCase() + "/" + value.Module.toLowerCase() + ".html", { cache: $templateCache });
                    } catch (e) {
                    }
                });
                $rootScope.$broadcast(tt.personalization.dataLoaded);
                //$location.path(currentPath);
                appAuth.redirectToAttemptedUrl();
            });
        });

        // TODO: what about unloading!?

        $rootScope.$on(tt.authentication.authenticationRequired, function () {
            appAuth.saveAttemptUrl();
            appAuth.gotoLogin();
        });
        $rootScope.$on(tt.authentication.loginConfirmed, function () {
            var maintenanceCache = $angularCacheFactory.get("offlineCache");
            var offlineCacheKey = "offlineMessage";
            if (maintenanceCache)
            { maintenanceCache.remove(offlineCacheKey); }
            //$location.path("/");
            appAuth.redirectToAttemptedUrl();

            toast.pop({
                title: "Login",
                body: $translate("LOGIN_SUCCESS"),
                type: "success"
            });
        });
        $rootScope.$on(tt.authentication.loginFailed, function () {
            $location.path("/login");
            toast.pop({
                title: "Login",
                body: $translate("LOGIN_FAILED"),
                type: "error"
            });
        });
        $rootScope.$on(tt.authentication.logoutConfirmed, function () {
            $localStorage.$reset();
            $location.path("/login");
        });

        //$rootScope.$on("$stateChangeStart", function (event, toState, toParams, fromState, fromParams) {
        //    if (!$rootScope.tt.authentication.userLoggedIn) {
        //        $rootScope.$broadcast(tt.authentication.authenticationRequired);
        //    }
        //});

        $rootScope.ttAppLoaded = true;
    }]);

app.animation(".reveal-animation", function () {
    return {
        enter: function (element, done) {
            element.css("display", "none");
            element.fadeIn(500, done);
            return function () {
                element.stop();
            };
        },
        leave: function (element, done) {
            element.fadeOut(500, done);
            return function () {
                element.stop();
            };
        }
    };
});
