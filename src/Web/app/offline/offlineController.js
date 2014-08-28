(function () {
    function Controller($rootScope, $scope, $location, signalRSubscribe, toast, dialog, $translate, common, offlineApi) {
        $scope.offlineMessage = {};
        $rootScope.$broadcast(common.events.hideMenu);
        getOfflineMessage();
        function getOfflineMessage() {
            return offlineApi.getOfflineMessage()
                .then(function (data) {
                $scope.offlineMessage = data;
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

    app.controller("OfflineController",
        ["$rootScope", "$scope", "$location", "signalRSubscribe", "toast", "dialog", "$translate", "common", "offlineApi", Controller]);
})();
