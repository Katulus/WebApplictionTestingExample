app.directive('swWizard', () => {
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