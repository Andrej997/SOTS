import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { GraphService } from '../services/graph.service';
import { Edge, Node } from '@swimlane/ngx-graph';

@Component({
  selector: 'app-expected-knowlage-graph',
  templateUrl: './expected-knowlage-graph.component.html',
  styleUrls: ['./expected-knowlage-graph.component.css']
})
export class ExpectedKnowlageGraphComponent implements OnInit {

  nodes: Node[] = [];
  edges: Edge[] = [];

  constructor(private graphService: GraphService,
    private toastr: ToastrService) { }

  ngOnInit(): void {
    this.getGraph();
  }

  private getGraph() {
    this.graphService.expectedKnowlageGraphForTest(26).subscribe(result => {
      ((result as any).item1 as any[]).forEach(x => {
        let node: Node = {
          id: x.id,
          label: x.label,
          data: {
            customColor: x.customColor
          }
        };
        this.nodes.push(node);
      });
      ((result as any).item2 as any[]).forEach(x => {
        let edge: Edge = {
          id: x.id,
          label: x.label,
          source: x.sourceId,
          target: x.targetId
        };
        this.edges.push(edge);
      });
      this.nodes = [...this.nodes];
      this.edges = [...this.edges];
    }, error => {
        this.toastr.error(error.error);
        console.error(error);
    });
  }

}
