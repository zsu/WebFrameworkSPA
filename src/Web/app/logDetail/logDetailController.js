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
    function Controller($scope, $location, signalRSubscribe, toast, dialog, $translate, common, personalization, logDetailApi) {
        $scope.item = {};
        var id=$location.search()["id"];
        getData(id);
        $scope.goBack=function()
        {
            $location.path("logs");
        };
        function getData(id) {
            return logDetailApi.getData(id)
                .then(function (data) {
                    $scope.item = data;

                    return data;
                }, function (data) {
                    if (data.httpStatus)
                    {
                        switch(data.httpStatus)
                        {
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

    app.controller("LogDetailController",
        ["$scope", "$location", "signalRSubscribe", "toast", "dialog", "$translate", "common","personalization", "logDetailApi", Controller]);
})();
