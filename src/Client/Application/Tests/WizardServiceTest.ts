module AddNodeWizard {
    describe("WizardService", () => {
        var $httpBackend: ng.IHttpBackendService,
            service: IWizardService;

        beforeEach(angular.mock.module('AddNodeWizard'));

        // let Angular give us what we need using dependency injection
        beforeEach(inject((wizardService, _$httpBackend_) => {
            service = wizardService;
            $httpBackend = _$httpBackend_;
        }));

        it("loads steps from web service", inject(($http) => {
            $httpBackend.whenGET(/.*\/steps/).respond([
                { Id: 'Step1', ControlName: 'Step1Control', Title: 'Step 1' },
                { Id: 'Step2', ControlName: 'Step2Control', Title: 'Step 2' }
            ]);

            var loadedSteps;
            service.loadSteps((steps) => { loadedSteps = steps; }, () => {});
            $httpBackend.flush();

            expect(loadedSteps.length).toBe(2, 'Wrong number of steps returned.');
            expect(loadedSteps[0].Id).toBe('Step1');
            expect(loadedSteps[1].Id).toBe('Step2');
        }));

        it("calls to web service on next()", () => {
            $httpBackend.whenPOST(/.*\/next/).respond({ CanTransition: true });

            service.next(null, () => {}, () => {});

            expect($httpBackend.flush).not.toThrow();
        });

        it("sends node to web service on next()", () => {
            var node = new Node();
            $httpBackend.whenPOST(/.*\/next/, node).respond({ CanTransition: true });
            
            service.next(node, () => { }, () => { });

            expect($httpBackend.flush).not.toThrow();
        });

        it("returns server response on next()", () => {
            $httpBackend.whenPOST(/.*\/next/).respond({ CanTransition: true });

            var returnedResult;
            service.next(null, (response) => {
                returnedResult = response;
            }, () => { });

            expect($httpBackend.flush).not.toThrow();
            expect(returnedResult.CanTransition).toBeTruthy("expected response should allow transition");
        });

        it("calls error handler on error in next()", () => {
            $httpBackend.whenPOST(/.*\/next/).respond(500, 'test error');

            var returnedError;
            service.next(null, () => {}, (error) => {
                returnedError = error;
            });

            expect($httpBackend.flush).not.toThrow();
            expect(returnedError).toBe("test error");
        });

        it("calls to web service on back()", () => {
            $httpBackend.whenPOST(/.*\/back/).respond({ CanTransition: true });

            service.back(() => { }, () => { });

            expect($httpBackend.flush).not.toThrow();
        });

        it("returns server response on back()", () => {
            $httpBackend.whenPOST(/.*\/back/).respond({ CanTransition: true });

            var returnedResult;
            service.back((response) => {
                returnedResult = response;
            }, () => { });

            expect($httpBackend.flush).not.toThrow();
            expect(returnedResult.CanTransition).toBeTruthy("expected response should allow transition");
        });

        it("calls error handler on error in back()", () => {
            $httpBackend.whenPOST(/.*\/back/).respond(500, 'test error');

            var returnedError;
            service.back(() => { }, (error) => {
                returnedError = error;
            });

            expect($httpBackend.flush).not.toThrow();
            expect(returnedError).toBe("test error");
        });

        it("calls web service on addNode()", () => {
            $httpBackend.whenPOST(/.*\/add/).respond(200, '');

            service.addNode(null, () => { }, () => {});

            expect($httpBackend.flush).not.toThrow();
        });

        it("sends node to web service on addNode()", () => {
            var node = new Node();
            $httpBackend.whenPOST(/.*\/add/, node).respond(200, '');

            service.addNode(node, () => { }, () => { });

            expect($httpBackend.flush).not.toThrow();
        });

        // ... more tests for rest of the methods ...
    });
}

