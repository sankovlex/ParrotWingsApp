(function () {
    'use strict';

    angular
        .module('ParrotWingsApp')
        .factory('apiService', apiService);

    apiService.$inject = ['$http', '$rootScope', '$location'];

    function apiService($http, $rootScope, $location) {
        var service = {
            get: get,
            post: post
        };

        function get(url, success, failed, config) {
            return $http.get(url, config)
                .then(
                function (result) {
                    success(result);
                },
                function (error) {
                    if (error.status === '401') {
                        $location.path('/login');
                    }
                    else if (failed !== null) {
                        failed(error);
                    }
                });
        }

        function post(url, data, success, failed) {
            return $http.post(url, data)
                .then(
                function (result) {
                    success(result);
                },
                function (error) {
                    if (error.status === '401') {
                        $location.path('/login');
                    }
                    else if (failed !== null) {
                        failed(error);
                    }
                });
        }

        return service;
    }
})();