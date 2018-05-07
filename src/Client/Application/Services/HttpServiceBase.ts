'use strict';
import IHttpPromiseCallbackArg = angular.IHttpPromiseCallbackArg;

module AddNodeWizard {
    export class HttpServiceBase {
        private _baseAddress: string = 'http://localhost:8081';

        constructor(private $http: ng.IHttpService) {
        }

        protected get<T>(path: string, callback: (result: T) => void, errorCallback?: (error: string) => void): void {
            this.$http.get(this._baseAddress + path).then((response: IHttpPromiseCallbackArg<T>) => {
                callback(response.data);
            }, (error) => {
                if (errorCallback) {
                    errorCallback(error.data);
                }
            });
        }

        protected post<T>(path: string, data: any, callback: (result: T) => void, errorCallback?: (error: string) => void): void {
            this.$http.post(this._baseAddress + path, data).then((response: IHttpPromiseCallbackArg<T>) => {
                callback(response.data);
            }, (error) => {
                if (errorCallback) {
                    errorCallback(error.data);
                }
            });
        }
    }
}