import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { HttpServiceBase } from './http-service-base.service';
import { Node, PollingMethod } from './models';

export interface INodesService {
  loadNodes(callback: (result: Node[]) => void, errorCallback?: (error: string) => void): void;
  deleteAll(callback: () => void, errorCallback?: (error: string) => void): void;
}

@Injectable()
export class NodesService extends HttpServiceBase implements INodesService {
  static Create(http: HttpClient): INodesService {
    return new NodesService(http);
  }

  constructor(http: HttpClient) {
    super(http);
  }

  public loadNodes(callback: (result: Node[]) => void, errorCallback?: (error: string) => void): void {
    //this.get('/server/nodes', callback, errorCallback);
    callback([
      new Node("node1"),
      new Node("node2")
    ]);
  }

  public deleteAll(callback: () => void, errorCallback?: (error: string) => void): void {
    //this.post('/server/deleteAll', null, callback, errorCallback);
  }
}