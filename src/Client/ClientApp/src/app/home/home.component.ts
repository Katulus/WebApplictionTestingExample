import { Component } from '@angular/core';
import { NodesService } from '../nodes.service';
import { Node } from '../models';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  public nodes: Node[] = [];

  constructor(private nodesService: NodesService) {
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
