(function () {
    'use strict';

    angular
        .module('ParrotWingsApp')
        .controller('transactCtrl', transactCtrl);

    transactCtrl.$inject = ['$location', 'apiService', '$routeParams'];

    function transactCtrl($location, apiService, $routeParams) {
        /* jshint validthis:true */
        var vm = this;
        vm.title = 'transactCtrl';

        var id = $routeParams.id;

        vm.transaction = {};
        vm.error = '';

        vm.user = {};
        vm.show = '';

        vm.transact = function () {
            apiService.post('api/v1.00/transactions', vm.transaction,
                function (result) {
                    if (result.status > 200)
                        vm.error = result.data.message;
                    else
                        $location.path('/profile');
                },
                function (error) {
                    alert(error.data);
                }
            );
        }

        function getTransactionById(id) {
            apiService.get('api/v1.00/transactions/' + id,
                function (result) {
                    vm.transaction = result.data;
                },
                function (result) {
                    alert(result.data);
                }
            )
        }

        vm.getUsers = function (username) {
            vm.user = {};
            apiService.get('api/v1.00/user/search?filter=' + username,
                function (result) {
                    vm.users = result.data;
                    vm.show = vm.users ? 'show' : '';
                },
                function (result) {
                    alert(result.data);
                }
            )
        }

        vm.choiceUser = function (user) {
            vm.user = user;
            vm.transaction.recipient.name = user.name;
            vm.transaction.recipient.email = user.email;
            vm.show = '';
        }

        activate();

        function activate() {
            if (id) {
                getTransactionById(id);
            }
        }
    }
})();
