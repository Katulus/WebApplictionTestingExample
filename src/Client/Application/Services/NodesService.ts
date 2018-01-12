'use strict';

module AddNodeWizard {
    export interface INodesService {
        loadNodes(callback: (result: Node[]) => void, errorCallback?: (error: string) => void): void;
        deleteAll(callback: () => void, errorCallback?: (error: string) => void): void;
    }

    export class NodesService extends HttpServiceBase implements INodesService {
        static Create($http: ng.IHttpService): INodesService {
            return new NodesService($http);
        }

        constructor($http: ng.IHttpService) {
            super($http);
        }

        public loadNodes(callback: (result: Node[]) => void, errorCallback?: (error: string) => void): void {
            this.get('/server/nodes', callback, errorCallback);
        }

        public deleteAll(callback: () => void, errorCallback?: (error: string) => void): void {
            this.post('/server/deleteAll', null, callback, errorCallback);
        }
    }
}