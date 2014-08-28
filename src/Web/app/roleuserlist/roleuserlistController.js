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
    function Controller($scope, $location, signalRSubscribe, toast, dialog, $translate, common, personalization, roleUserListApi) {
    };

    app.controller("RoleUserListController",
        ["$scope", "$location", "signalRSubscribe", "toast", "dialog", "$translate", "common", "personalization", "roleUserListApi", Controller]);
})();
