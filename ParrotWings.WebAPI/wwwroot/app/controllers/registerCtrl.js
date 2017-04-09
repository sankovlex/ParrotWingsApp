(function () {
    'use strict';

    angular
        .module('ParrotWingsApp')
        .controller('registerCtrl', registerCtrl);

    registerCtrl.$inject = ['$location', '$rootScope', 'authService'];

    function registerCtrl($location, $rootScope, authService) {
        /* jshint validthis:true */
        var vm = this;
        vm.title = 'registerCtrl';

        vm.user = {};

        vm.error = '';

        vm.register = function () {
            authService.register(vm.user, completedRegister);
        }

        function completedRegister(result) {
            if (result.status === 200) {
                authService.login(vm.user, complitedLogin);
            }
            else
                vm.error = result.data.message;
        }

        function complitedLogin(result) {
            if (result.status === 200) {
                authService.saveIdentity(result.data);

                $location.path('/profile');
            }
        }

        activate();

        function activate() { }
    }
})();
