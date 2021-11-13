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

  allLoaded: boolean = false;

  nodeForm: FormGroup;
  addNodeForm: boolean = false;
  addFormBtn: boolean = true;
  nodes: Node[] = [];
  sourceNodes: Node[] = [];
  targetNodes: Node[] = [];

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
        this.nodes.push(x.nodeJson);
        this.sourceNodes.push(x.nodeJson);
        this.targetNodes.push(x.nodeJson);
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
        this.edges.push(x.edgeJson);
      });
      this.edges = [...this.edges];
      this.allLoaded = true;
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

  
  onSearchSourceNode(input: any) {
    this.sourceNodes = [];
    this.nodes.forEach(node => {
      if (node.label?.toLowerCase().includes(input.target.value)) {
        this.sourceNodes.push(node);
      }
    });
  }

  
  onSearchTargetNode(input: any) {
    this.targetNodes = [];
    this.nodes.forEach(node => {
      if (node.label?.toLowerCase().includes(input.target.value)) {
        this.targetNodes.push(node);
      }
    });
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

  clickedNode: Node = {
    id: '',
    label: ''
  }
  clickOnNode(node: Node) {
    this.clickedNode = node;
  }

  deleteNode() {
    let edgeIds: string[] = [];
    this.edges.forEach(edge => {
      if (edge.source == this.clickedNode.id || edge.target == this.clickedNode.id) {
        edgeIds.push(edge.id as string);
      }
    });
    let body = {
      EdgeIds: edgeIds
    }
    this.graphService.deleteEdge(body).subscribe(result => {
      if (edgeIds.length >0)
        this.toastr.success("Deteled edges from node "+ this.clickedNode.label);
      this.graphService.deleteNode(this.clickedNode.id as string).subscribe(result => {
        this.toastr.success("Deteled node " + this.clickedNode.label);
        this.clickedNode = {
          id: '',
          label: ''
        }
        this.getNodes();
      }, error => {
          this.toastr.error(error.error);
          console.error(error);
      });
    }, error => {
        this.toastr.error(error.error);
        console.error(error);
    });
  }

  clickedEdge: Edge = {
    id: '',
    label: '',
    source: '',
    target: ''
  }
  clickOnEdge(edge: Edge){
    this.clickedEdge = edge;
  }

  deleteEdge() {
    let body = {
      EdgeIds: [this.clickedEdge.id]
    }
    this.graphService.deleteEdge(body).subscribe(result => {
      this.toastr.success("Deteled edge " + this.clickedEdge.label);
      this.getNodes();
      this.clickedEdge = {
        id: '',
        label: '',
        source: '',
        target: ''
      }
    }, error => {
        this.toastr.error(error.error);
        console.error(error);
    });
  }
}
