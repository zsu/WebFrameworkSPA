(function () {
    /**
     * @param $http
     * @param $q
     * @param $angularCacheFactory
     */
    $app.CancelVerificationRequestApi = function ($http, $q, $angularCacheFactory) {
        this.cancelVerificationRequest = function (params) {
            var deferred = $q.defer();
            var url = ttTools.baseUrl + "api/account/cancelverificationrequest";
            var p = {
                id: params.id
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

    app.service("cancelverificationrequestApi", ["$http", "$q", "$angularCacheFactory", $app.CancelVerificationRequestApi]);
})();
