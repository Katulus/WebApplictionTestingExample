module AddNodeWizard {
    describe("IndexController", () => {
        var nodesServiceMock,
            createController;

        beforeEach(() => {
            angular.mock.module('AddNodeWizard');
            // create mocks for dependency injection
            angular.mock.module(($provide: ng.IModule) => {
                $provide.factory('nodesService', (): INodesService => {
                    return {
                        loadNodes: jasmine.createSpy("loadNodes"),
                        deleteAll: jasmine.createSpy("deleteAll")
                    }
                });
            });

            inject(($controller, _nodesService_) => {
                nodesServiceMock = _nodesService_;
                createController = () => $controller('IndexController', { '$scope': {} });
            });
        });

        it("loads nodes from web service", () => {
            var testNodes = [
                new Node('1.1.1.1'),
                new Node('2.2.2.2')
            ]
            nodesServiceMock.loadNodes.and.callFake((callback: (result: Node[]) => void, failCallback) => {
                callback(testNodes);
            });
            var controller = createController();

            expect(controller.nodes.length).toBe(2, 'Wrong number of nodes returned.');
            expect(controller.nodes[0].IpOrHostname).toBe('1.1.1.1');
            expect(controller.nodes[1].IpOrHostname).toBe('2.2.2.2');
        });
    });
}
