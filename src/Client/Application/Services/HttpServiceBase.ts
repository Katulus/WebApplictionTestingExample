'use strict';

module AddNodeWizard {
    export class HttpServiceBase {
        private _baseAddress: string = 'http://localhost:63598';

        constructor(private $http: ng.IHttpService) {
        }

        protected get<T>(path: string, callback: (result: T) => void, errorCallback?: (error: string) => void): void {
            this.$http.get(this._baseAddress + path).success((result: T) => {
                callback(result);
            })
                .error((error) => {
                    if (errorCallback) {
                        errorCallback(error);
                    }
                });
        }

        protected post<T>(path: string, data: any, callback: (result: T) => void, errorCallback?: (error: string) => void): void {
            this.$http.post(this._baseAddress + path, data).success((result: T) => {
                callback(result);
            })
                .error((error) => {
                    if (errorCallback) {
                        errorCallback(error);
                    }
                });
        }
    }
}