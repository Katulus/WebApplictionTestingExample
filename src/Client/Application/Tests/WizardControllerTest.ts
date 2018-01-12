module AddNodeWizard {
    describe("WizardController", () => {
        var wizardServiceMock,
            createController;

        beforeEach(() => {
            angular.mock.module('AddNodeWizard');
            // create mocks for dependency injection
            angular.mock.module(($provide: ng.IModule) => {
                $provide.factory('wizardService', (): IWizardService => {
                    return {
                        loadSteps: jasmine.createSpy("loadSteps").and.callFake((callback: (nodes: WizardStepDefinition[]) => void, failCallback) => {
                            callback([
                                { Id: 'Step1', ControlName: 'Step1Control', Title: 'Step 1' },
                                { Id: 'Step2', ControlName: 'Step2Control', Title: 'Step 2' }
                            ]);
                        }),
                        back: jasmine.createSpy("back"),
                        next: jasmine.createSpy("next"),
                        cancel: jasmine.createSpy("cancel").and.callFake((callback) => {callback()}),
                        addNode: jasmine.createSpy("addNode")
                    }
                });
            });

            inject(($controller, _wizardService_) => {
                wizardServiceMock = _wizardService_;

                // here we pass custom $scope to controller and let injector inject mocked service
                createController = () => $controller('WizardController', { '$scope': {} });
            });
        });

        it("loads steps from web service", () => {
            var controller = createController();

            expect(controller.stepDefinitions.length).toBe(2, 'Wrong number of steps returned.');
            expect(controller.stepDefinitions[0].Id).toBe('Step1');
            expect(controller.stepDefinitions[1].Id).toBe('Step2');
        });

        it("moves to next step on next() if server and step allows", () => {
            wizardServiceMock.next.and.callFake((node, callback: (result: StepTransitionResult) => void, failCallback) => {
                callback({ CanTransition: true, ErrorMessage: '' });
            });

            var controller = createController();

            var testStep = {
                onNext: () => { return true; },
                setNode: (node: Node) => {}
            };
            controller.registerStep(testStep);

            controller.next();

            expect(controller.currentStepIndex).toBe(1, "Controller hasn't moved to next step");
        });

        it("does not move to next step on next() if server denies", () => {
            wizardServiceMock.next.and.callFake((node, callback: (result: StepTransitionResult) => void, failCallback) => {
                callback({ CanTransition: false, ErrorMessage: 'test error' });
            });

            var controller = createController();
            controller.next();

            expect(controller.currentStepIndex).toBe(0, "Controller has moved to next step when it should not.");
        });

        it("does not move to next step on next() if current step denies", () => {
            wizardServiceMock.next.and.callFake((node, callback: (result: StepTransitionResult) => void, failCallback) => {
                callback({ CanTransition: true, ErrorMessage: '' });
            });

            var controller = createController();

            var testStep = {
                onNext: () => { return false; },
                setNode: (node: Node) => {}
            };
            controller.registerStep(testStep);

            controller.next();

            expect(controller.currentStepIndex).toBe(0, "Controller has moved to next step when it should not.");
        });

        it("sets error if server responds with error", () => {
            wizardServiceMock.next.and.callFake((node, callback: (result: StepTransitionResult) => void, failCallback) => {
                callback({ CanTransition: false, ErrorMessage: 'test error' });
            });

            var controller = createController();
            controller.next();

            expect(controller.showError).toBe(true);
            expect(controller.errorMessage).toBe('test error');
        });

        it("does not allow to move back from first step", () => {
            var controller = createController();

            expect(controller.cantGoBack).toBe(true);
        });

        it("does not allow to move forward from last step", () => {
            wizardServiceMock.next.and.callFake((node, callback: (result: StepTransitionResult) => void, failCallback) => {
                callback({ CanTransition: true, ErrorMessage: '' });
            });

            var controller = createController();

            controller.next();

            expect(controller.cantGoForward).toBe(true);
        });

        it("adds node when addNode() is called", () => {
            var controller = createController();

            controller.addNode();

            expect(wizardServiceMock.addNode).toHaveBeenCalledWith(controller.node, jasmine.any(Function), jasmine.any(Function));
        });

        it("sets error when addNode() fails", () => {
            wizardServiceMock.addNode.and.callFake((node, callback, failCallback: (error) => void) => {
                failCallback('test error');
            });

            var controller = createController();
            controller.addNode();

            expect(controller.showError).toBe(true);
            expect(controller.errorMessage).toBe('test error');
        });
    });
}