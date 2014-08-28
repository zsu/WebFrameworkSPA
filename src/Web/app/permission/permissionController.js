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
    function Controller($scope, $location, signalRSubscribe, toast, dialog, $translate, common, personalization, permissionApi) {
    };

    app.controller("PermissionController",
        ["$scope", "$location", "signalRSubscribe", "toast", "dialog", "$translate", "common", "personalization", "permissionApi", Controller]);
})();
