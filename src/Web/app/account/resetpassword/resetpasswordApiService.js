(function () {
    /**
     * @param $http
     * @param $q
     * @param $angularCacheFactory
     */
    $app.ResetPasswordApi = function ($http, $q, $angularCacheFactory) {
        this.resetPassword = function (params) {
            var deferred = $q.defer();
            var url = ttTools.baseUrl + "api/account/resetpassword";
            var p = {
                Email: params.Email,
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

    app.service("resetpasswordApi", ["$http", "$q", "$angularCacheFactory", $app.ResetPasswordApi]);
})();
