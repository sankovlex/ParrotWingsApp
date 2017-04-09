(function () {
    'use strict';

    angular
        .module('ParrotWingsApp')
        .directive('passwordValidator', passwordValidator);

    passwordValidator.$inject = [];

    function passwordValidator() {
        // Usage:
        //     <passwordValidator></passwordValidator>
        // Creates:
        // 
        var directive = {
            require: '?ngModel',
            link: link
        };
        return directive;

        function link(scope, element, attrs, ctrl) {
            var PASS_REGEXP = /^(?=.*[A - Za - z])(?=.*\d)[A - Za - z\d]{8,}$/;
            if (ctrl && ctrl.$validators.password) {
                ctrl.$validators.password = function (modelValue) {
                    return ctrl.$isEmpty(modelValue) || PASS_REGEXP.test(modelValue);
                };
            }
        }
    }

})();