'use strict';
var app = angular.module('AddNodeWizard', [
    'ngRoute'
]);
app.config(function ($controllerProvider, $routeProvider) {
    app.controllerProvider = $controllerProvider;
    $routeProvider
        .when('/add', {
        templateUrl: '/Application/Views/add.html'
    })
        .otherwise({
        templateUrl: '/Application/Views/index.html',
        controller: 'IndexController',
        controllerAs: 'vm'
    });
    ;
})
    .factory('wizardService', ['$http', AddNodeWizard.WizardService.Create])
    .factory('nodesService', ['$http', AddNodeWizard.NodesService.Create])
    .controller("IndexController", ['$scope', 'nodesService', AddNodeWizard.IndexController])
    .controller("WizardController", ['$scope', '$location', 'wizardService', AddNodeWizard.WizardController]);
//# sourceMappingURL=App.js.map