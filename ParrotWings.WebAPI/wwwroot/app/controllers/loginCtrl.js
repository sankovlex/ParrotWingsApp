(function () {
    'use strict';

    angular
        .module('ParrotWingsApp')
        .controller('loginCtrl', loginCtrl);

    loginCtrl.$inject = ['$scope', '$location', '$rootScope', 'authService'];

    function loginCtrl($scope, $location, $rootScope, authService) {
        /* jshint validthis:true */
        var vm = this;
        vm.title = 'loginCtrl';

        vm.user = {};
        vm.error = '';

        

        vm.login = function () {
            authService.login(vm.user, completed);
            vm.identity = $rootScope.identity;

        }

        function completed(result) {
            if (result.data) {
                if (result.status === 200) {
                    authService.saveIdentity(result.data);
                    
                    $location.path('/profile');
                }
                else {
                    vm.error = result.data.message;
                }
            }
        }

        vm.logout = function () {
            authService.removeIdentity();
            $location.path('/');
        }

        vm.auth = authService;

        activate();

        function activate() {
        }
    }
})();
