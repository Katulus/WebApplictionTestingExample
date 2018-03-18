var AddNodeWizard;
(function (AddNodeWizard) {
    describe("WizardController", function () {
        var wizardServiceMock, createController;
        beforeEach(function () {
            angular.mock.module('AddNodeWizard');
            // create mocks for dependency injection
            angular.mock.module(function ($provide) {
                $provide.factory('wizardService', function () {
                    return {
                        loadSteps: jasmine.createSpy("loadSteps").and.callFake(function (callback, failCallback) {
                            callback([
                                { Id: 'Step1', ControlName: 'Step1Control', Title: 'Step 1' },
                                { Id: 'Step2', ControlName: 'Step2Control', Title: 'Step 2' }
                            ]);
                        }),
                        back: jasmine.createSpy("back"),
                        next: jasmine.createSpy("next"),
                        cancel: jasmine.createSpy("cancel").and.callFake(function (callback) { callback(); }),
                        addNode: jasmine.createSpy("addNode")
                    };
                });
            });
            inject(function ($controller, _wizardService_) {
                wizardServiceMock = _wizardService_;
                // here we pass custom $scope to controller and let injector inject mocked service
                createController = function () { return $controller('WizardController', { '$scope': {} }); };
            });
        });
        it("loads steps from web service", function () {
            var controller = createController();
            expect(controller.stepDefinitions.length).toBe(2, 'Wrong number of steps returned.');
            expect(controller.stepDefinitions[0].Id).toBe('Step1');
            expect(controller.stepDefinitions[1].Id).toBe('Step2');
        });
        it("moves to next step on next() if server and step allows", function () {
            wizardServiceMock.next.and.callFake(function (node, callback, failCallback) {
                callback({ CanTransition: true, ErrorMessage: '' });
            });
            var controller = createController();
            var testStep = {
                onNext: function () { return true; },
                setNode: function (node) { }
            };
            controller.registerStep(testStep);
            controller.next();
            expect(controller.currentStepIndex).toBe(1, "Controller hasn't moved to next step");
        });
        it("does not move to next step on next() if server denies", function () {
            wizardServiceMock.next.and.callFake(function (node, callback, failCallback) {
                callback({ CanTransition: false, ErrorMessage: 'test error' });
            });
            var controller = createController();
            controller.next();
            expect(controller.currentStepIndex).toBe(0, "Controller has moved to next step when it should not.");
        });
        it("does not move to next step on next() if current step denies", function () {
            wizardServiceMock.next.and.callFake(function (node, callback, failCallback) {
                callback({ CanTransition: true, ErrorMessage: '' });
            });
            var controller = createController();
            var testStep = {
                onNext: function () { return false; },
                setNode: function (node) { }
            };
            controller.registerStep(testStep);
            controller.next();
            expect(controller.currentStepIndex).toBe(0, "Controller has moved to next step when it should not.");
        });
        it("sets error if server responds with error", function () {
            wizardServiceMock.next.and.callFake(function (node, callback, failCallback) {
                callback({ CanTransition: false, ErrorMessage: 'test error' });
            });
            var controller = createController();
            controller.next();
            expect(controller.showError).toBe(true);
            expect(controller.errorMessage).toBe('test error');
        });
        it("does not allow to move back from first step", function () {
            var controller = createController();
            expect(controller.cantGoBack).toBe(true);
        });
        it("does not allow to move forward from last step", function () {
            wizardServiceMock.next.and.callFake(function (node, callback, failCallback) {
                callback({ CanTransition: true, ErrorMessage: '' });
            });
            var controller = createController();
            controller.next();
            expect(controller.cantGoForward).toBe(true);
        });
        it("adds node when addNode() is called", function () {
            var controller = createController();
            controller.addNode();
            expect(wizardServiceMock.addNode).toHaveBeenCalledWith(controller.node, jasmine.any(Function), jasmine.any(Function));
        });
        it("sets error when addNode() fails", function () {
            wizardServiceMock.addNode.and.callFake(function (node, callback, failCallback) {
                failCallback('test error');
            });
            var controller = createController();
            controller.addNode();
            expect(controller.showError).toBe(true);
            expect(controller.errorMessage).toBe('test error');
        });
    });
})(AddNodeWizard || (AddNodeWizard = {}));
//# sourceMappingURL=WizardControllerTest.js.map