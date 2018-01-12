app.directive('swWizardStep', ($compile, $templateRequest, $http, $controller, $rootScope) => {
    var renderStep = (scope, element, attrs) => {
        var controlName = scope.controlName;
        if (!controlName)
            return;

        var controllerName = controlName + 'Controller';

        // load and execute script with controller
        $http.get('/Application/Controls/' + controlName + '/' + controllerName + '.js').success((script) => {
                // This is naive way of dynamically loading and executing script. But it works for the purpose of this example.
                $.globalEval(script);

                app.controllerProvider.register(controllerName, function() {
                    return new AddNodeWizard[controllerName]();
                });

                // load step template and show it
                $templateRequest('/Application/Controls/' + controlName + '/' + controlName + '.html').then((template) => {
                    element.html(template).show();
                    scope.vm = $controller(controllerName);
                    scope.registerStep.call(scope.$parent.vm, scope.vm);
                    $compile(element.contents())(scope);
                });
            })
            .error(() => {
                alert('Unable to load wizard step control ' + controlName);
            });
    }

    var linker = (scope, element, attrs) => {
        // allow dynamic change of controlName to render new step
        scope.$watch('controlName', () => {
            renderStep(scope, element, attrs);
        });
    }

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