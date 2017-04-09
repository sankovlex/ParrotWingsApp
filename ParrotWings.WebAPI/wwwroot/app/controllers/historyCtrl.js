(function () {
    'use strict';

    angular
        .module('ParrotWingsApp')
        .controller('historyCtrl', historyCtrl);

    historyCtrl.$inject = ['$location', 'apiService'];

    function historyCtrl($location, apiService) {
        /* jshint validthis:true */
        var vm = this;
        vm.title = 'historyCtrl';

        vm.transactions = [];

        var config = {};

        vm.filterString = '';


        vm.orders = [
            { name: 'Sort by date', value: '' },
            { name: 'Sort by amount', value: 'amount' },
            { name: 'Sort by name', value: 'name' }
        ]

        vm.sort = vm.orders[0];

        function getTransactions() {
            apiService.get('api/v1.00/transactions?orderby=' + vm.sort.value + '&search=' + vm.filterString,
                function (result) {
                    vm.transactions = result.data;
                },
                function (error) {
                    alert(error.data);
                }
            );
        }

        vm.onTextChange = function (filterString) {
            getTransactions();
        }

        vm.onSelectChange = function () {
            getTransactions();
        }

        vm.balance = 0;

        function getBalance() {
            apiService.get('api/v1.00/transactions/balance',
                function (result) {
                    vm.balance = result.data;
                },
                function (error) {
                    alert(error.data);
                }
            )
        }

        activate();

        function activate() {
            getTransactions();
            getBalance();
        }
    }
})();
