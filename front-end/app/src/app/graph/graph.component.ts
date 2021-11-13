import { Component, HostListener, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Edge, Node } from '@swimlane/ngx-graph';
import { ToastrService } from 'ngx-toastr';
import { GraphService } from '../services/graph.service';

@Component({
  selector: 'app-graph',
  templateUrl: './graph.component.html',
  styleUrls: ['./graph.component.css']
})
export class GraphComponent implements OnInit {

  nodeForm: FormGroup;
  addNodeForm: boolean = false;
  addFormBtn: boolean = true;
  nodes: Node[] = [];

  edgeForm: FormGroup;
  addEdgeForm: boolean = false;
  edges: Edge[] = [];

  constructor(private fb: FormBuilder,
    private graphService: GraphService,
    private toastr: ToastrService) { }

  ngOnInit(): void {
    this.nodeForm = this.fb.group({
      id: ['', Validators.required],
      label: ['', Validators.required],
    });
    this.edgeForm = this.fb.group({
      id: ['', Validators.required],
      label: ['', Validators.required],
    });

    this.getNodes();

  }

  private getNodes() {
    this.nodes = [];
    let body = {};
    this.graphService.getNodes(body).subscribe(result => {
      console.log(result);
      (result as any[]).forEach(x => {
        this.nodes.push(JSON.parse(x.nodeJson));
      });
      this.nodes = [...this.nodes];
      this.getEdges();
    }, error => {
        this.toastr.error(error.error);
        console.error(error);
    });
  }

  private getEdges() {
    this.edges = [];
    let body = {};
    this.graphService.getEdges(body).subscribe(result => {
      console.log(result);
      (result as any[]).forEach(x => {
        this.edges.push(JSON.parse(x.edgeJson));
      });
      this.edges = [...this.edges];
    }, error => {
        this.toastr.error(error.error);
        console.error(error);
    });
  }

  createNode() {
    let canCreate: boolean = true;

    if (this.nodeForm.value.id == '') {
      this.toastr.error("Node id is empty");
      canCreate = false;
      return;
    }

    if (this.nodeForm.value.label == '') {
      this.toastr.error("Node label is empty");
      canCreate = false;
      return;
    }

    this.nodes.forEach(node => {
      if (node.id == this.nodeForm.value.id) {
        this.toastr.error("Node with that id exists");
        canCreate = false;
        return;
      }
    });

    if (canCreate) {
      let node: Node = {
        id: this.nodeForm.value.id,
        label: this.nodeForm.value.label
      };
      this.nodes.push(node);
      this.nodes = [...this.nodes];

      let body = {
        NodeJson: JSON.stringify(node)
      };
      this.graphService.createNode(body).subscribe(result => {
        this.getNodes();
      }, error => {
          this.toastr.error(error.error);
          console.error(error);
      });
    }
  }

  

  showAddForm(form: string) {
    if (form == 'node') {
      this.addNodeForm = true;
      this.addEdgeForm = false;
      this.addFormBtn = false;
    }
    else if (form == 'edge') {
      this.addNodeForm = false;
      this.addEdgeForm = true;
      this.addFormBtn = false;
    }
    else {
      this.addNodeForm = false;
      this.addEdgeForm = false;
      this.addFormBtn = true;
    }
  }

  private selectedSourceNodeId: string = '';
  selectSourceNode(nodeId: string) {
    this.nodes.forEach((node => {
      let elemTemp = <HTMLElement>document.getElementById("sn_" + node.id);
      elemTemp.style.background = 'white';
    }))
    let elem = <HTMLElement>document.getElementById("sn_" + nodeId);
    elem.style.background = 'green';
    this.selectedSourceNodeId = nodeId;
  }

  private selectedTargetNodeId: string = '';
  selectTargetNode(nodeId: string) {
    this.nodes.forEach((node => {
      let elemTemp = <HTMLElement>document.getElementById("tn_" + node.id);
      elemTemp.style.background = 'white';
    }))
    let elem = <HTMLElement>document.getElementById("tn_" + nodeId);
    elem.style.background = 'green';
    this.selectedTargetNodeId = nodeId;
  }

  createEdge() {
    let canCreate: boolean = true;

    if (this.edgeForm.value.id == '') {
      this.toastr.error("Edge id is empty");
      canCreate = false;
      return;
    }

    if (this.edgeForm.value.label == '') {
      this.toastr.error("Edge label is empty");
      canCreate = false;
      return;
    }

    if (this.selectedSourceNodeId == '') {
      this.toastr.error("Select source node");
      canCreate = false;
      return;
    }

    if (this.selectedTargetNodeId == '') {
      this.toastr.error("Select target node");
      canCreate = false;
      return;
    }

    this.edges.forEach(edge => {
      if (edge.id == this.edgeForm.value.id) {
        this.toastr.error("Edge with that id exists");
        canCreate = false;
        return;
      }
      if (edge.source == this.selectedSourceNodeId && edge.target == this.selectedTargetNodeId && edge.label == this.edgeForm.value.label) {
        this.toastr.error("Edge with same source, target and label exists");
        canCreate = false;
        return;
      }
    });

    if (canCreate) {
      let edge: Edge = {
        id: this.edgeForm.value.id,
        label: this.edgeForm.value.label,
        source: this.selectedSourceNodeId,
        target: this.selectedTargetNodeId
      };
      this.edges.push(edge);
      this.edges = [...this.edges];

      let body = {
        EdgeJson: JSON.stringify(edge)
      };
      this.graphService.createEdge(body).subscribe(result => {
            
      }, error => {
          this.toastr.error(error.error);
          console.error(error);
      });
    }
  }
}
