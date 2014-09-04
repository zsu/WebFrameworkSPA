(function () {
    /**
     * @param $http
     * @param $q
     * @param $angularCacheFactory
     */
    $app.ConfirmPasswordResetApi = function ($http, $q, $angularCacheFactory) {
        this.changePassword = function (params) {
            var deferred = $q.defer();
            var url = ttTools.baseUrl + "api/account/confirmpasswordreset";
            var p = {
                Key:params.key,
                Password: params.newPassword,
                ConfirmPassword: params.newPasswordConfirm
            };
            $http({
                method: "POST",
                url: url,
                params:p
            }).success(function (response) {
                deferred.resolve(response);
            }).error(function (response) {
                deferred.reject(response);
            });

            return deferred.promise;
        };
    };

    app.service("confirmpasswordresetApi", ["$http", "$q", "$angularCacheFactory", $app.ConfirmPasswordResetApi]);
})();
