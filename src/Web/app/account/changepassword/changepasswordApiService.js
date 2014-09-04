(function () {
    /**
     * @param $http
     * @param $q
     * @param $angularCacheFactory
     */
    $app.ChangePasswordApi = function ($http, $q, $angularCacheFactory) {
        this.changePassword = function (params) {
            var deferred = $q.defer();
            var url = ttTools.baseUrl + "api/account/changepassword";
            var p = {
                OldPassword: params.oldPassword,
                NewPassword: params.newPassword,
                NewPasswordConfirm: params.newPasswordConfirm
            };
            $http({
                method: "PUT",
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

    app.service("changepasswordApi", ["$http", "$q", "$angularCacheFactory", $app.ChangePasswordApi]);
})();
