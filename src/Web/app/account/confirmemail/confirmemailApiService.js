(function () {
    /**
     * @param $http
     * @param $q
     * @param $angularCacheFactory
     */
    $app.ConfirmEmailApi = function ($http, $q, $angularCacheFactory) {
        this.confirmEmail = function (params) {
            var deferred = $q.defer();
            var url = ttTools.baseUrl + "api/account/confirmemail";
            var p = {
                Password: params.Password,
                Key: params.Key
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

    app.service("confirmemailApi", ["$http", "$q", "$angularCacheFactory", $app.ConfirmEmailApi]);
})();
