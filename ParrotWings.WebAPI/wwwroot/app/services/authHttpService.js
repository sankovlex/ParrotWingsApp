(function () {
    'use strict';

    angular
        .module('ParrotWingsApp')
        .factory('authHttpService', ['$cookieStore', '$location', '$q', '$rootScope', function ($cookieStore, $location, $q, $rootScope) {
            return {

                'request': function (config) {

                    config.headers = config.headers || {};

                    var authData = $cookieStore.get('identity');
                    if (authData) {
                        config.headers.authorization = 'Bearer ' + authData.token;
                    }

                    return config;
                },
                'responseError': function (response) {
                    if (response.status === 401) {
                        $cookieStore.remove('identity');
                        $rootScope.identity = null;

                        $location.path('/login');
                    }
                    return response || $q.when(response);
                }
            }
        }])
})();