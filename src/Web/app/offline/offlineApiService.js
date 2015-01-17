(function () {
    /**
     * @param $http
     * @param $q
     * @param $angularCacheFactory
     */
    $app.OfflineApi = function ($http, $q, $angularCacheFactory) {
        var cache = $angularCacheFactory.get("offlineCache");
        if (!cache)
            cache = $angularCacheFactory("offlineCache", {});
        this.getOfflineMessage = function () {
            var deferred = $q.defer();
            var cacheKey = "offlineMessage";
            var url = ttTools.baseUrl + "api/maintenancewarning";

            if (cache.get(cacheKey)) {
                deferred.resolve(cache.get(cacheKey));
            } else
                $http({
                    method: "GET",
                    url: url
                }).success(function (response) {
                    if (response.Message) {
                        cache.put(cacheKey, response.Message);
                    }
                    else {
                        cache.put(cacheKey, "The site is under maintenance.")
                    }
                    deferred.resolve(response.Message);
                }).error(function (response) {
                    deferred.reject(response);
                });
            return deferred.promise;
        };
    };

    app.service("offlineApi", ["$http", "$q", "$angularCacheFactory", $app.OfflineApi]);
})();
