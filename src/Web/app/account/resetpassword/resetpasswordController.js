(function () {
    /**
     * @param $scope
     * @param $location
     * @param {$app.entitiesApi} entitiesApi
     * @param signalRSubscribe
     * @param {$app.Toast} toast
     * @param {$app.Dialog} dialog
     * @param $translate
     * @param {$app.Personalization} personalization
     */
    function Controller($scope, $location, signalRSubscribe, toast, dialog, $translate, $stateParams,common, personalization, resetpasswordApi) {
        $scope.resetPasswordModel = {};
        $scope.resetPasswordModel.Email = "";
        $scope.resetPassword = function () {
            return resetpasswordApi.resetPassword($scope.resetPasswordModel)
            .then(function (data) {
                var success = "POPUP_ERROR";
                var type = "error";
                if (data.Success ==true) {
                    success = "POPUP_SUCCESS";
                    type = "success";
                }
                toast.pop({
                    title: $translate(success),
                    body: data.Message,
                    type: type
                });
                return data;
            }, function (data) {
                if (data.httpStatus) {
                    switch (data.httpStatus) {
                        case 401:
                        case 400:
                            return;
                    }
                }
                showError(data);
            });
        };
        function showError(data) {
            dialog.showModalDialog({}, {
                headerText: $translate("COMMON_ERROR"),
                bodyText: "Error occurred while requesting data",
                closeButtonText: $translate("COMMON_CLOSE"),
                actionButtonText: $translate("COMMON_OK"),
                detailsText: JSON.stringify(data)
            });
        };
    };

    app.controller("ResetPasswordController",
        ["$scope", "$location", "signalRSubscribe", "toast", "dialog", "$translate", "$stateParams", "common", "personalization", "resetpasswordApi", Controller]);
})();
