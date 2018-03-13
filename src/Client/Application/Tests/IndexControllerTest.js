var AddNodeWizard;
(function (AddNodeWizard) {
    describe("IndexController", function () {
        var nodesServiceMock, createController;
        beforeEach(function () {
            angular.mock.module('AddNodeWizard');
            angular.mock.module(function ($provide) {
                $provide.factory('nodesService', function () {
                    return {
                        loadNodes: jasmine.createSpy("loadNodes"),
                        deleteAll: jasmine.createSpy("deleteAll")
                    };
                });
            });
            inject(function ($controller, _nodesService_) {
                nodesServiceMock = _nodesService_;
                createController = function () { return $controller('IndexController', { '$scope': {} }); };
            });
        });
        it("loads nodes from web service", function () {
            var testNodes = [
                new AddNodeWizard.Node('1.1.1.1'),
                new AddNodeWizard.Node('2.2.2.2')
            ];
            nodesServiceMock.loadNodes.and.callFake(function (callback, failCallback) {
                callback(testNodes);
            });
            var controller = createController();
            expect(controller.nodes.length).toBe(2, 'Wrong number of nodes returned.');
            expect(controller.nodes[0].IpOrHostname).toBe('1.1.1.1');
            expect(controller.nodes[1].IpOrHostname).toBe('2.2.2.2');
        });
    });
})(AddNodeWizard || (AddNodeWizard = {}));
//# sourceMappingURL=IndexControllerTest.js.map