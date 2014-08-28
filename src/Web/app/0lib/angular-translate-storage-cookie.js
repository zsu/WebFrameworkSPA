angular.module('pascalprecht.translate').factory('$translateCookieStorage', [
  'ipCookie',
  function ($cookieStore) {
    var $translateCookieStorage = {
        get: function (name) {
            return ipCookie(name);
        },
        set: function (name, value) {
            ipCookie(name, value);
        }
      };
    return $translateCookieStorage;
  }
]);