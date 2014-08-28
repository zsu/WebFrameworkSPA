app.directive('maintenanceWarning', ["$http", "$location","$q", "$angularCacheFactory", "$translate", "toast", function ($http, $location,$q, $angularCacheFactory,$translate, toast) {
    return {
        restrict: 'E',
        link: function (scope, element, attrs) {
            var maintenanceCache = $angularCacheFactory.get("offlineCache");
            if(!maintenanceCache)
                maintenanceCache = $angularCacheFactory("offlineCache", {});
            var deferred = $q.defer();
            var cacheKey = "offlineWarningMessage", offlineCacheKey = "offlineMessage";
            var url = ttTools.baseUrl + "api/maintenancewarning";
            if (maintenanceCache.get(offlineCacheKey)) {
                scope.message = maintenanceCache.get(offlineCacheKey);
                if (scope.message)
                    $location.path('/offline');
                deferred.resolve(maintenanceCache.get(offlineCacheKey));
            }
            else if (maintenanceCache.get(cacheKey)) {
                scope.message = maintenanceCache.get(cacheKey);
                if (scope.message)
                    toast.pop({
                        title: "",//$translate("POPUP_SUCCESS"),
                        body: scope.message,//.parse(scope.message),
                        type: "warning"
                    });
                deferred.resolve(maintenanceCache.get(cacheKey));
            }
            else
                $http({
                    method: "GET",
                    url: url
                }).success(function (response) {
                    if (response.Redirect) {
                        maintenanceCache.put(offlineCacheKey, response.Message);
                        deferred.resolve(response);
                        $location.path('/offline');
                        //window.location = 'offline/offline.html';
                        //if (!scope.$$phase) scope.$apply();
                    }
                    else {
                        if (response.Message) {
                            maintenanceCache.put(cacheKey, response.Message);
                            scope.message = maintenanceCache.get(cacheKey);
                            toast.pop({
                                title: "",//$translate("POPUP_SUCCESS"),
                                body: scope.message,//.parse(scope.message),
                                type: "warning"
                            });
                        }
                        deferred.resolve(response);
                    }
                }).error(function (response) {
                    deferred.reject(response);
                });
            return deferred.promise;
        },
    };
}]);
