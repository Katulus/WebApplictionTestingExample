import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { HttpServiceBase } from './http-service-base.service';
import { Node } from './models';

export interface INodesService {
  loadNodes(callback: (result: Node[]) => void, errorCallback?: (error: string) => void): void;
  deleteAll(callback: () => void, errorCallback?: (error: string) => void): void;
}

@Injectable()
export class NodesService extends HttpServiceBase implements INodesService {

  constructor(http: HttpClient, @Inject('BASE_API_URL') baseUrl: string) {
    super(http, baseUrl);
  }

  public loadNodes(callback: (result: Node[]) => void, errorCallback?: (error: string) => void): void {
    this.get('/nodes', callback, errorCallback);
  }

  public deleteAll(callback: () => void, errorCallback?: (error: string) => void): void {
    this.post('/deleteAll', null, callback, errorCallback);
  }
}
