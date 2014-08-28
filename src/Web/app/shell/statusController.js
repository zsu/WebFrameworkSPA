(function() {
    /**
     * @param $scope
     * @param {$app.NetworkStatus} networkStatus
     * @param {$app.Personalization} personalization
     * @param tokenAuthentication
     */
    function Controller($scope,$http,$q, $angularCacheFactory, networkStatus) {
        var cache = $angularCacheFactory.get("statusCache");
        if (!cache)
            cache = $angularCacheFactory("statusCache", {});
        $scope.status = {};
        $scope.status.isOnline = networkStatus.isOnline();
        getServerVersion();
        function getServerVersion() {
            var cacheKey = "serverVersion";
            var deferred = $q.defer();
            if (cache.get(cacheKey)) {
                deferred.resolve(cache.get(cacheKey));
            } else {
                var url = ttTools.baseUrl + "api/version"
                $http({
                    method: "GET",
                    url: url,
                }).success(function (response) {
                    $scope.status.serverVersion = response.Version;
                    cache.put(cacheKey, response.Version);
                    deferred.resolve(response);
                }).error(function (response) {
                    $scope.status.serverVersion = {}
                    deferred.reject(response);
                });
            }
        }
        $scope.$on(tt.networkstatus.onlineChanged, function (evt, isOnline) {
            $scope.status.isOnline = isOnline;
        });
    }

    app.controller("StatusController", ["$scope", "$http","$q", "$angularCacheFactory","networkStatus", Controller]);
})();
