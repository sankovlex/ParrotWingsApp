(function () {
    'use strict';

    angular
        .module('ParrotWingsApp')
        .controller('homeCtrl', homeCtrl);

    homeCtrl.$inject = ['$location'];

    function homeCtrl($location) {
        /* jshint validthis:true */
        var vm = this;
        vm.title = 'homeCtrl';

        activate();

        function activate() { }
    }
})();
