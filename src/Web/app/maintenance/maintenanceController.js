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
    function Controller($scope, $location, signalRSubscribe, toast, dialog, $translate, common, personalization, maintenanceApi) {
        $scope.refreshCache = function () {
            return maintenanceApi.refreshCache()
            .then(function (data) {
                toast.pop({
                    title: $translate("POPUP_SUCCESS"),
                    body: JSON.parse(data),
                    type: "success"
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
        $scope.restartApp = function () {
            return maintenanceApi.restartApp()
            .then(function (data) {
                toast.pop({
                    title: $translate("POPUP_SUCCESS"),
                    body: JSON.parse(data),
                    type: "success"
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

    app.controller("MaintenanceController",
        ["$scope", "$location", "signalRSubscribe", "toast", "dialog", "$translate", "common", "personalization","maintenanceApi", Controller]);
})();
