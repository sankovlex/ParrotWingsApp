(function () {
    'use strict';

    angular
        .module('ParrotWingsApp')
        .directive('emailValidator', emailValidator);

    emailValidator.$inject = [];

    function emailValidator() {
        // Usage:
        //     <email-validator></email-validator>
        // Creates:
        // 
        var directive = {
            require: '?ngModel',
            link: link
        };
        return directive;

        function link(scope, element, attrs, ctrl) {
            var EMAIL_REGEXP = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
            if (ctrl && ctrl.$validators.email) {
                ctrl.$validators.email = function (modelValue) {
                    return ctrl.$isEmpty(modelValue) || EMAIL_REGEXP.test(modelValue);
                };
            }
        }
    }

})();