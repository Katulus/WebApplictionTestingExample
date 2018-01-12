'use strict';

module AddNodeWizard {
    export class IndexController {
        public nodes: Node[] = [];

        constructor(private $scope: ng.IScope, private nodesService: INodesService) {
            this.loadNodes();
        }

        private loadNodes() {
            this.nodesService.loadNodes((result: Node[]) => {
                this.nodes = result;
            });
        }

        public deleteAll() {
            this.nodesService.deleteAll(() => {
                this.nodes = [];
            });
        }
    }
}