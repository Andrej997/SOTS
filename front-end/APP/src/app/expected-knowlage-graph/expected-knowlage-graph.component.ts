import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { GraphService } from '../services/graph.service';
import { Edge, Node } from '@swimlane/ngx-graph';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-expected-knowlage-graph',
  templateUrl: './expected-knowlage-graph.component.html',
  styleUrls: ['./expected-knowlage-graph.component.css']
})
export class ExpectedKnowlageGraphComponent implements OnInit {

  nodes: Node[] = [];
  edges: Edge[] = [];
  private testId: number = 0;
  private routeSub: Subscription;

  constructor(private graphService: GraphService,
    private route: ActivatedRoute,
    private toastr: ToastrService) { }

  ngOnInit(): void {
    this.routeSub = this.route.params.subscribe(params => {
      this.testId = params['t_id']; 
      this.getGraph(this.testId);
    });
  }

  private getGraph(testId: number) {
    this.graphService.expectedKnowlageGraphForTest(testId).subscribe(result => {
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
