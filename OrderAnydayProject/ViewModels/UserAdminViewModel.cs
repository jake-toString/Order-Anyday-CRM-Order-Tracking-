(function()
{
    'use strict';

    angular
        .module('app')
        .controller('UserAdminViewModel', UserAdminViewModel);

    UserAdminViewModel.$inject = ['$location'];

    function UserAdminViewModel($location) {
        /* jshint validthis:true */
        var vm = this;
        vm.title = 'UserAdminViewModel';

        activate();

        function activate() { }
    }
})();
