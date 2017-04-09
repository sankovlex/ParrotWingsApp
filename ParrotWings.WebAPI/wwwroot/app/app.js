(function () {
    'use strict';

    angular.module('ParrotWingsApp', [
        // Angular modules 
        'ngRoute',
        'ngCookies',
        'ngMessages',

        // Custom modules 

        // 3rd Party Modules
    ])
        .config(['$routeProvider', '$locationProvider', '$httpProvider', function ($routeProvider, $locationProvider, $httpProvider) {
            //$locationProvider.hashPrefix('!');
            $locationProvider.html5Mode(true);

            $routeProvider
                .when('/', {
                    controller: 'homeCtrl',
                    templateUrl: 'app/views/home/home.html',
                    controllerAs: 'vm'
                })
                .when('/login', {
                    controller: 'loginCtrl',
                    templateUrl: 'app/views/account/login.html',
                    controllerAs: 'vm'
                })
                .when('/register', {
                    controller: 'registerCtrl',
                    templateUrl: 'app/views/account/register.html',
                    controllerAs: 'vm'
                })
                .when('/profile', {
                    controller: 'profileCtrl',
                    templateUrl: 'app/views/profile/profile.html',
                    controllerAs: 'vm',
                    resolve: { isAuthenticated: isAuthenticated }
                })
                .when('/history', {
                    controller: 'historyCtrl',
                    templateUrl: 'app/views/profile/history.html',
                    controllerAs: 'vm',
                    resolve: { isAuthenticated: isAuthenticated }
                })
                .when('/send/:id?', {
                    controller: 'transactCtrl',
                    templateUrl: 'app/views/profile/transact.html',
                    controllerAs: 'vm',
                    resolve: { isAuthenticated: isAuthenticated }
                })
                .otherwise({ redirectTo: '/' });

            //add api interceptor
            $httpProvider.interceptors.push('authHttpService');
        }])
        .run(['$rootScope', '$location', '$cookieStore', '$http', 'authService', function ($rootScope, $location, $cookieStore, $http, authService) {
            //$rootScope.$on('balanceChange', function () {
            //    if ($rootScope.identity === null) {
            //        $location.path("/login");
            //    }
            //    authService.isLoggedIn();
            //});
            $rootScope.identity = $cookieStore.get('identity') || null;
        }]);

    isAuthenticated.$inject = ['authService', '$rootScope', '$location'];

    function isAuthenticated(authService, $rootScope, $location) {
        if (!authService.isLoggedIn()) {
            $location.path('/login');
        }
    }
})();