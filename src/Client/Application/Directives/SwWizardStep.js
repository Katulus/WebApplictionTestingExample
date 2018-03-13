app.directive('swWizardStep', function ($compile, $templateRequest, $http, $controller, $rootScope) {
    var renderStep = function (scope, element, attrs) {
        var controlName = scope.controlName;
        if (!controlName)
            return;
        var controllerName = controlName + 'Controller';
        $http.get('/Application/Controls/' + controlName + '/' + controllerName + '.js').success(function (script) {
            jQuery.globalEval(script);
            app.controllerProvider.register(controllerName, function () {
                return new AddNodeWizard[controllerName]();
            });
            $templateRequest('/Application/Controls/' + controlName + '/' + controlName + '.html').then(function (template) {
                element.html(template).show();
                scope.vm = $controller(controllerName);
                scope.registerStep.call(scope.$parent.vm, scope.vm);
                $compile(element.contents())(scope);
            });
        })
            .error(function () {
            alert('Unable to load wizard step control ' + controlName);
        });
    };
    var linker = function (scope, element, attrs) {
        scope.$watch('controlName', function () {
            renderStep(scope, element, attrs);
        });
    };
    return {
        restrict: "E",
        link: linker,
        scope: {
            controlName: '=',
            node: '=',
            registerStep: '='
        }
    };
});
//# sourceMappingURL=SwWizardStep.js.map