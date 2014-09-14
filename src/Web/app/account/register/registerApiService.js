(function () {
    /**
     * @param $http
     * @param $q
     * @param $angularCacheFactory
     */
    $app.RegisterApi = function ($http, $q, $angularCacheFactory) {
        this.register = function (params) {
            var deferred = $q.defer();
            var url = ttTools.baseUrl + "api/account/register";
            var p = {
                Username: params.Username,
                Email:params.Email,
                Password: params.Password,
                ConfirmPassword: params.ConfirmPassword,
                FirstName: params.FirstName,
                LastName:params.LastName
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

    app.service("registerApi", ["$http", "$q", "$angularCacheFactory", $app.RegisterApi]);
})();
