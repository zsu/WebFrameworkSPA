(function () {
    'use strict';

    // Define the common module 
    // Contains services:
    //  - common
    //  - logger
    //  - spinner
    var commonModule = angular.module('common', []);

    // Must configure the common service and set its 
    // events via the commonConfigProvider
    commonModule.provider('commonConfig', function () {
        this.config = {
            // These are the properties we need to set
        };

        this.$get = function () {
            return {
                config: this.config
            };
        };
    });

    commonModule.factory('common',
        ['$q', '$rootScope', '$timeout', 'ipCookie', 'commonConfig', common]);

    function common($q, $rootScope, $timeout, ipCookie, commonConfig) {
        var throttles = {};
        var events = {
            hideMenu: "hideMenu"
        };
        var service = {
            // common angular dependencies
            $broadcast: $broadcast,
            $q: $q,
            $timeout: $timeout,
            // generic
            activateController: activateController,
            createSearchThrottle: createSearchThrottle,
            debouncedThrottle: debouncedThrottle,
            isNumber: isNumber,
            textContains: textContains,
            filterBarPlugin: filterBarPlugin,
            setTimezoneCookie: setTimezoneCookie,
            events:events
        };

        return service;

        function activateController(promises, controllerId) {
            return $q.all(promises).then(function (eventArgs) {
                var data = { controllerId: controllerId };
                $broadcast(commonConfig.config.controllerActivateSuccessEvent, data);
            });
        }

        function $broadcast() {
            return $rootScope.$broadcast.apply($rootScope, arguments);
        }

        function createSearchThrottle(filterFnIn, delay) {
            // custom delay or use default
            delay = +delay || 300;
            // create the filtering function we will call from here
            var filterFn = function () {
                // translates to ...
                // vm.filteredSessions 
                //      = vm.sessions.filter(function(item( { returns vm.sessionFilter (item) } );
                //viewmodel[filteredList] = viewmodel[list].filter(function (item) {
                //    return viewmodel[filter](item);
                return filterFnIn(true);
            };
            return (function () {
                // Wrapped in outer IFFE so we can use closure 
                // over filterInputTimeout which references the timeout
                var filterInputTimeout;

                // return what becomes the 'applyFilter' function in the controller
                return function (searchNow) {
                    if (filterInputTimeout) {
                        $timeout.cancel(filterInputTimeout);
                        filterInputTimeout = null;
                    }
                    if (searchNow || !delay) {
                        filterFn();
                    } else {
                        filterInputTimeout = $timeout(filterFn, delay);
                    }
                };
            })();
        }

        function debouncedThrottle(key, callback, delay, immediate) {
            var defaultDelay = 1000;
            delay = delay || defaultDelay;
            if (throttles[key]) {
                $timeout.cancel(throttles[key]);
                throttles[key] = undefined;
            }
            if (immediate) {
                callback();
            } else {
                throttles[key] = $timeout(callback, delay);
            }
        }

        function isNumber(val) {
            // negative or positive
            return /^[-]?\d+$/.test(val);
        }

        function textContains(text, searchText) {
            return text && -1 !== text.toLowerCase().indexOf(searchText.toLowerCase());
        }
        function filterBarPlugin(opts) {
            var self = this;
            self.grid = null;
            self.scope = null;
            var setFilter=function (filter)
            {
                //filterBarPlugin.scope.$parent.filterText = filter;
                filterBarPlugin.scope.gridOptions.filterOptions.filterText = filter;
            }
            self.init = function (scope, grid) {
                filterBarPlugin.scope = scope;
                filterBarPlugin.grid = grid;
                scope.$watch(function () {
                    var searchQuery = {};
                    angular.forEach(filterBarPlugin.scope.columns, function (col) {
                        if (col.visible && col.filterText) {
                            var filterText = (col.filterText.indexOf('*') == 0 ? col.filterText.replace('*', '') : col.filterText);
                            searchQuery[col.field] = filterText;
                        }
                    });
                    return searchQuery?JSON.stringify(searchQuery):'';
                }, function (searchQuery) {
                    var filterInputTimeout;
                    filterBarPlugin.scope.$parent.filterText = searchQuery;//searchQuery?'{'+searchQuery+'}':null;
                    filterBarPlugin.scope.gridOptions.filterOptions.filterText = searchQuery;//searchQuery? '{' + searchQuery + '}' : null;
                    //if (filterInputTimeout) {
                    //    $timeout.cancel(filterInputTimeout);
                    //    filterInputTimeout = null;
                    //}
                    //filterInputTimeout = $timeout(setFilter.bind(null, searchQuery), 3000);
                });
            }
        }

        function setTimezoneCookie() {

            var timezone_cookie = "timezoneid";

            // if the timezone cookie not exists create one.
            if (!ipCookie(timezone_cookie)) {

                //// check if the browser supports cookie
                //var test_cookie = 'test cookie';
                //$.cookie(test_cookie, true);

                //// browser supports cookie
                //if ($.cookie(test_cookie)) {

                // delete the test cookie
                //$.cookie(test_cookie, null);

                // create a new cookie
                ipCookie(timezone_cookie, jstz.determine().name(), { path: '/' });

                // re-load the page
                //location.reload();
                //}
            }
                // if the current timezone and the one stored in cookie are different
                // then store the new timezone in the cookie and refresh the page.
            else {

                var storedTimezone = ipCookie(timezone_cookie);
                var currentTimezone = jstz.determine().name();

                // user may have changed the timezone
                if (storedTimezone !== currentTimezone) {
                    ipCookie(timezone_cookie, currentTimezone, { path: '/' });
                    location.reload();
                }
            }
        }
    }
})();




var filterBarPlugin = {
    init: function (scope, grid) {
        filterBarPlugin.scope[grid.gridId] = scope;
        filterBarPlugin.grid[grid.gridId] = grid;
        $scope.$watch(function () {
            var searchQuery = "";
            var gridId = grid.gridId;
            angular.forEach(filterBarPlugin.scope[gridId].columns, function (col) {
                if (col.visible && col.filterText) {
                    var filterText = (col.filterText.indexOf('*') == 0 ? col.filterText.replace('*', '') : "^" + col.filterText) + ";";
                    searchQuery += col.displayName + ": " + filterText;
                }
            });
            return searchQuery;
        }, function (searchQuery) {
            var gridId = grid.gridId;
            filterBarPlugin.scope[gridId].$parent.filterText = searchQuery;
            filterBarPlugin.grid[gridId].searchProvider.evalFilter();
        });
    },
    scope: {},
    grid: {}
};