(function () {
    /**
     * @param $http
     * @param $scope
     * @param $translate
     * @param {$app.Personalization} personalization
     */
    function Controller($http, $rootScope, $scope, $translate, dialog, personalization, tokenAuthentication, common, appAuth) {
        $scope.navigation = {};
        $scope.navigation.isCollapsed = true;

        $scope.navigation.currentLanguage = $translate.preferredLanguage() || $translate.proposedLanguage();

        $scope.navigation.toggleMenu = function () {
            $scope.sideMenuController.toggleLeft();
        };

        $scope.$on(tt.personalization.dataLoaded, function () {
            $scope.navigation.navigationItems = personalization.data.Features;
        });

        $scope.$on(tt.authentication.logoutConfirmed, function () {
            $scope.navigation.navigationItems = null;
        });

        $scope.navigation.changeLanguage = function (langKey) {
            $scope.navigation.currentLanguage = langKey;
            $translate.uses(langKey);
        };

        $scope.navigation.logout = function() {
            tokenAuthentication.logout();
        };
        $scope.getToken = function () {
            return tokenAuthentication.getToken();
        }
        $scope.requireLogin=function()
        {
            $rootScope.$broadcast(tt.authentication.authenticationRequired);
        }
        $scope.showError = function (data) {
            var status = data.status;
            if (status) {
                switch (status) {
                    case 401:
                    case 400:
                        appAuth.saveAttemptUrl();
                        appAuth.gotoLogin();
                        return;
                }
            }
            showError(data); dialog.showModalDialog({}, {
                headerText: $translate("COMMON_ERROR"),
                bodyText: "Error occurred while requesting data",
                closeButtonText: $translate("COMMON_CLOSE"),
                actionButtonText: $translate("COMMON_OK"),
                detailsText: JSON.stringify(data)
            });
        };
        $scope.menuVisible = true;
        $scope.$on(common.events.hideMenu, function () {
            $scope.menuVisible = false;
        });
    };

    app.controller("NavigationController", ["$http", "$rootScope","$scope", "$translate","dialog", "personalization", "tokenAuthentication","common","appAuth", Controller]);
})();
