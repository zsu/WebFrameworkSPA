(function () {
    /**
     * @param $http
     * @param $q
     * @param $angularCacheFactory
     */
    $app.LogDetailApi = function ($http, $q, $angularCacheFactory) {
        this.getData = function (id) {
            var item = {};
            var deferred = $q.defer();
            var url = ttTools.baseUrl + "api/log"
            var p = {
                id: id
            };
            $http({
                method: "GET",
                url: url,
                params: p
            }).success(function (response) {
                deferred.resolve(response);
            }).error(function (response) {
                deferred.reject(response);
            });

            return deferred.promise;
        };
    };

    app.service("logDetailApi", ["$http", "$q", "$angularCacheFactory", $app.LogDetailApi]);
})();
