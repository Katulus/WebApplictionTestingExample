app.directive('swWizard', function () {
    return {
        restrict: 'E',
        scope: {
            wizardTitle: '='
        },
        controller: 'WizardController',
        controllerAs: 'vm',
        templateUrl: '/Application/Controls/Wizard/wizard.html'
    };
});
//# sourceMappingURL=SwWizard.js.map