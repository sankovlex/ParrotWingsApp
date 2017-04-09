(function () {
    'use strict';

    angular
        .module('ParrotWingsApp')
        .directive('accountPanel', accountPanel);

    accountPanel.$inject = ['$window'];

    function accountPanel($window) {
        // Usage:
        //     <account-panel></account-panel>
        // Creates:
        // 
        var directive = {
            controller: 'loginCtrl',
            controllerAs: 'vm',
            templateUrl: '/app/views/directives/accountPanel.html',
            transclude: true,
            restrict: 'EA'
        };
        return directive;
    }

})();