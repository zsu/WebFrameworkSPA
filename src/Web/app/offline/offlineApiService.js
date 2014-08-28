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

            if (cache.get(cacheKey)) {
                deferred.resolve(cache.get(cacheKey));
            } else {
                deferred.resolve("The site is under maintenance.");
            }

            return deferred.promise;
        };
    };

    app.service("offlineApi", ["$http", "$q", "$angularCacheFactory", $app.OfflineApi]);
})();
