var AddNodeWizard;
(function (AddNodeWizard) {
    describe("WizardService", function () {
        var $httpBackend, service;
        beforeEach(angular.mock.module('AddNodeWizard'));
        beforeEach(inject(function (wizardService, _$httpBackend_) {
            service = wizardService;
            $httpBackend = _$httpBackend_;
        }));
        it("loads steps from web service", inject(function ($http) {
            $httpBackend.whenGET(/.*\/steps/).respond([
                { Id: 'Step1', ControlName: 'Step1Control', Title: 'Step 1' },
                { Id: 'Step2', ControlName: 'Step2Control', Title: 'Step 2' }
            ]);
            var loadedSteps;
            service.loadSteps(function (steps) { loadedSteps = steps; }, function () { });
            $httpBackend.flush();
            expect(loadedSteps.length).toBe(2, 'Wrong number of steps returned.');
            expect(loadedSteps[0].Id).toBe('Step1');
            expect(loadedSteps[1].Id).toBe('Step2');
        }));
        it("calls to web service on next()", function () {
            $httpBackend.whenPOST(/.*\/next/).respond({ CanTransition: true });
            service.next(null, function () { }, function () { });
            expect($httpBackend.flush).not.toThrow();
        });
        it("sends node to web service on next()", function () {
            var node = new AddNodeWizard.Node();
            $httpBackend.whenPOST(/.*\/next/, node).respond({ CanTransition: true });
            service.next(node, function () { }, function () { });
            expect($httpBackend.flush).not.toThrow();
        });
        it("returns server response on next()", function () {
            $httpBackend.whenPOST(/.*\/next/).respond({ CanTransition: true });
            var returnedResult;
            service.next(null, function (response) {
                returnedResult = response;
            }, function () { });
            expect($httpBackend.flush).not.toThrow();
            expect(returnedResult.CanTransition).toBeTruthy("expected response should allow transition");
        });
        it("calls error handler on error in next()", function () {
            $httpBackend.whenPOST(/.*\/next/).respond(500, 'test error');
            var returnedError;
            service.next(null, function () { }, function (error) {
                returnedError = error;
            });
            expect($httpBackend.flush).not.toThrow();
            expect(returnedError).toBe("test error");
        });
        it("calls to web service on back()", function () {
            $httpBackend.whenPOST(/.*\/back/).respond({ CanTransition: true });
            service.back(function () { }, function () { });
            expect($httpBackend.flush).not.toThrow();
        });
        it("returns server response on back()", function () {
            $httpBackend.whenPOST(/.*\/back/).respond({ CanTransition: true });
            var returnedResult;
            service.back(function (response) {
                returnedResult = response;
            }, function () { });
            expect($httpBackend.flush).not.toThrow();
            expect(returnedResult.CanTransition).toBeTruthy("expected response should allow transition");
        });
        it("calls error handler on error in back()", function () {
            $httpBackend.whenPOST(/.*\/back/).respond(500, 'test error');
            var returnedError;
            service.back(function () { }, function (error) {
                returnedError = error;
            });
            expect($httpBackend.flush).not.toThrow();
            expect(returnedError).toBe("test error");
        });
        it("calls web service on addNode()", function () {
            $httpBackend.whenPOST(/.*\/add/).respond(200, '');
            service.addNode(null, function () { }, function () { });
            expect($httpBackend.flush).not.toThrow();
        });
        it("sends node to web service on addNode()", function () {
            var node = new AddNodeWizard.Node();
            $httpBackend.whenPOST(/.*\/add/, node).respond(200, '');
            service.addNode(node, function () { }, function () { });
            expect($httpBackend.flush).not.toThrow();
        });
    });
})(AddNodeWizard || (AddNodeWizard = {}));
//# sourceMappingURL=WizardServiceTest.js.map