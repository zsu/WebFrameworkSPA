//$script.path("app/");

$script(
    [
        "translations/translations-de.js",
        "constants.js",
        "app.js"
    ], "app");

$script.ready("app", function () {
    $script(
    [
        "infrastructure/tools.js",
        "infrastructure/thinktecture.ng.Authentication.js",
        "infrastructure/thinktecture.ng.SignalR.js",
        "infrastructure/baseUrlFilter.js",
        "infrastructure/serverValidationDirective.js",
        "infrastructure/routeResolverService.js",
        "infrastructure/thinktecture.ng.Toast.js",
        "infrastructure/thinktecture.ng.Dialog.js",
        "infrastructure/geoLocationTrackerService.js",
        "infrastructure/common.js",

        "login/loginController.js",

        "shell/navigationController.js",
        "shell/networkStatusService.js",
        "shell/personalizationService.js",
        "shell/statusController.js",

        "start/startController.js",

         "infrastructure/maintenanceWarningDirective.js",
         "about/AboutController.js",
         "logs/logsApiService.js",
         "logs/logsController.js",
         "logDetail/logDetailApiService.js",
         "logDetail/logDetailController.js",
         "maintenance/maintenanceApiService.js",
         "maintenance/maintenanceController.js",
         "offline/offlineApiService.js",
         "offline/offlineController.js",
         "setting/settingApiService.js",
         "setting/settingController.js",
         "activitylog/activitylogApiService.js",
         "activitylog/activitylogController.js",
         "authenticationaudit/authenticationauditApiService.js",
         "authenticationaudit/authenticationauditController.js",
         "messagetemplate/messagetemplateApiService.js",
         "messagetemplate/messagetemplateController.js",
         "user/userApiService.js",
         "user/userController.js",
         "userrole/userroleApiService.js",
         "userrole/userroleController.js",
         "role/roleApiService.js",
         "role/roleController.js",
         "rolepermission/rolepermissionApiService.js",
         "rolepermission/rolepermissionController.js",
         "permission/permissionApiService.js",
         "permission/permissionController.js",
         "roleuserlist/roleuserlistApiService.js",
         "roleuserlist/roleuserlistController.js",
         "account/accountApiService.js",
         "account/accountController.js",
         "account/changepassword/changepasswordApiService.js",
         "account/changepassword/changepasswordController.js",
         "account/confirmemail/confirmemailApiService.js",
         "account/confirmemail/confirmemailController.js"
    ], "bundle");

    $script.ready("bundle", function () {
        $script.path("");
        angular.bootstrap(document, ["myApp"]);
    });
});
