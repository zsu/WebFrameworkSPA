(function () {
    /**
     * @param $http
     * @param $q
     * @param $angularCacheFactory
     */
    $app.MaintenanceApi = function ($http, $q, $angularCacheFactory) {
        this.refreshCache = function () {
            var deferred = $q.defer();
            var url = ttTools.baseUrl + "api/cache"
            $http({
                method: "DELETE",
                url: url
            }).success(function (response) {
                deferred.resolve(response);
            }).error(function (response) {
                deferred.reject(response);
            });

            return deferred.promise;
        };
        this.restartApp = function () {
            var deferred = $q.defer();
            var url = ttTools.baseUrl + "api/app";
            $http({
                method: "GET",
                url: url
            }).success(function (response) {
                deferred.resolve(response);
            }).error(function (response) {
                deferred.reject(response);
            });

            return deferred.promise;
        };
    };

    app.service("maintenanceApi", ["$http", "$q", "$angularCacheFactory", $app.MaintenanceApi]);
})();
