(function () {
    'use strict';

    angular
        .module('ParrotWingsApp')
        .controller('profileCtrl', profileCtrl);

    profileCtrl.$inject = ['$location', '$rootScope', 'apiService'];

    function profileCtrl($location, $rootScope, apiService) {
        /* jshint validthis:true */
        var vm = this;
        vm.title = 'profileCtrl';

        vm.user = {};
        vm.sentTransactions = [];
        vm.balance = 0;

        //user
        function getUser() {
            apiService.get('api/v1.00/user/', getUserCompleted, getUserFailed)
        }

        function getUserCompleted(result) {
            vm.user = result.data;
        }

        function getUserFailed(result) {
            alert(result.data);
        }

        //sent transactions
        function getLastTransactions() {
            apiService.get('api/v1.00/transactions?take=5',
                function (result) {
                    vm.sentTransactions = result.data;
                },
                function (result) {
                    vm.sentTransactions = [];
                }
            )
        }

        function getBalance() {
            apiService.get('api/v1.00/transactions/balance',
                function (result) {
                    vm.balance = result.data;
                    $rootScope.identity.balance = result.data;
                },
                function (error) {
                    alert(error.data);
                }
            )
        }

        activate();

        function activate() {
            getUser();
            getBalance();
            getLastTransactions();
        }
    }
})();
