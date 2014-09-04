(function () {
    /**
     * @param $scope
     * @param tokenAuthentication
     * @param {$app.Toast} toast
     */
    function Controller($scope, $location, tokenAuthentication, dialog, $translate) {
        $scope.login = {};

        $scope.login.username = "";
        $scope.login.password = "";

        $scope.login.submit = function () {
            tokenAuthentication.login($scope.login.username, $scope.login.password)
                .error(function (data, status, headers, config) {
                    if (status === 400) {
                        if (data.error == 'password_expired')
                        {
                            $location.path('/changepassword/'+$scope.login.username);
                            return;
                        }
                        dialog.showModalDialog({}, {
                            headerText: $translate("COMMON_ERROR"),
                            bodyText: $translate("LOGIN_FAILED"),
                            closeButtonText: $translate("COMMON_CLOSE"),
                            actionButtonText: $translate("COMMON_OK")
                        });
                    };
                });
        };
    };

    app.controller("LoginController", ["$scope","$location", "tokenAuthentication", "dialog", "$translate", Controller]);
})();
