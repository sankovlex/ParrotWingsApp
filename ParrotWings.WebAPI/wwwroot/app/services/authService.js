(function () {
    'use strict';

    angular
        .module('ParrotWingsApp')
        .factory('authService', authService);

    authService.$inject = ['$http', '$rootScope', '$cookieStore', '$q', 'apiService'];

    function authService($http, $rootScope, $cookieStore, $q, apiService) {
        var service = {
            login: login,
            register: register,
            saveIdentity: saveIdentity,
            removeIdentity: removeIdentity,
            isLoggedIn: isLoggedIn
        };

        function login(user, completed) {
            apiService.post('/api/v1.00/account/token', user,
                completed,
                loginFailed);
        }

        function register(user, completed) {
            apiService.post('/api/v1.00/account/register', user,
                completed,
                registrationFailed);
        }

        function saveIdentity(user) {
            $rootScope.identity = {
                token: user.access_token,
                email: user.user_email
            };

            $cookieStore.put('identity', $rootScope.identity);
        }

        function removeIdentity() {
            $rootScope.identity = null;
            $cookieStore.remove('identity');

        };

        function loginFailed(response) {
            alert(response.data);
        }

        function registrationFailed(response) {

            alert('Registration failed. Try again.');
        }

        function isLoggedIn() {
            return $rootScope.identity != null;
        }
        
        return service;

        function getData() { }
    }
})();