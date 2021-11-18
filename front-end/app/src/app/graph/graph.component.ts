import { Component, HostListener, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Edge, Node } from '@swimlane/ngx-graph';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { DomainService } from '../services/domain.services';
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
  domains: any[] = [];
  edgeForm: FormGroup;
  addEdgeForm: boolean = false;
  edges: Edge[] = [];
  domainId: number = 0;
  private routeSub: Subscription;
  selectedLevel: number = 0;

  constructor(private fb: FormBuilder,
    private domainService: DomainService,
    private route: ActivatedRoute,
    private graphService: GraphService,
    private toastr: ToastrService) { }

  ngOnInit(): void {
    this.routeSub = this.route.params.subscribe(params => {
      this.domainId = params['d_id']; 
      if (this.domainId == undefined) {
        this.domainId = 0;
      }
      else {
        this.selectedLevel = this.domainId;
        this.getNodes(this.domainId);
      }
    });
    this.nodeForm = this.fb.group({
      id: ['', Validators.required],
      label: ['', Validators.required],
    });
    this.edgeForm = this.fb.group({
      id: ['', Validators.required],
      label: ['', Validators.required],
    });
    
    this.getDomains();
  }

  changeDomain(event: any) {
    this.domainId = <number>event.target.value;
    this.getNodes(this.domainId);
  }

  private getDomains() {
    this.domains = [];
    this.domainService.getDomains().subscribe(result => {
      this.domains = result as any[];
      // console.log(this.domains);
      if (this.domains.length > 0 && this.domainId == 0) {
        this.domainId = this.domains[0].id;
        this.selectedLevel = this.domains[0].id;
        this.getNodes(this.domains[0].id);
      }
    }, error => {
        console.error(error);
    });
  }

  private getNodes(domainId: number) {
    this.nodes = [];
    this.sourceNodes = [];
    this.targetNodes = [];
    let body = {
      DomainId: domainId
    };
    this.graphService.getNodes(body).subscribe(result => {
      (result as any[]).forEach(x => {
        let node: Node = {
          id: x.id,
          label: x.label
        };
        this.nodes.push(node);
        this.sourceNodes.push(node);
        this.targetNodes.push(node);
      });
      this.nodes = [...this.nodes];
      this.getEdges(domainId);
    }, error => {
        this.toastr.error(error.error);
        console.error(error);
    });
  }

  private getEdges(domainId: number) {
    this.edges = [];
    let body = {
      DomainId: domainId
    };
    this.graphService.getEdges(body).subscribe(result => {
      (result as any[]).forEach(x => {
        let edge: Edge = {
          id: x.id,
          label: x.label,
          source: x.sourceId,
          target: x.targetId
        };
        this.edges.push(edge);
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
        DomainId: this.domainId,
        NodeJson: JSON.stringify(node)
      };
      this.graphService.createNode(body).subscribe(result => {
        this.toastr.success("Node created");
        this.getNodes(this.domainId);
      }, error => {
          this.toastr.error(error.error);
          console.error(error);
      });
    }
  }

  
  onSearchSourceNode(input: any) {
    this.sourceNodes = [];
    this.nodes.forEach(node => {
      if (this.selectedSourceNodeId != '') {
        let elem = <HTMLElement>document.getElementById("sn_" + this.selectedSourceNodeId);
        if (elem != null)
          elem.style.background = 'green';
      }
      
      if (node.label?.toLowerCase().includes(input.target.value)) {
        this.sourceNodes.push(node);
      }
    });
  }

  
  onSearchTargetNode(input: any) {
    this.targetNodes = [];
    this.nodes.forEach(node => {
      if (this.selectedTargetNodeId != '') {
        let elem = <HTMLElement>document.getElementById("tn_" + this.selectedTargetNodeId);
        if (elem != null)
          elem.style.background = 'green';
      }

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
        DomainId: this.domainId,
        EdgeJson: JSON.stringify(edge)
      };
      this.graphService.createEdge(body).subscribe(result => {
        this.toastr.success("Edge created");
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
        this.getNodes(this.domainId);
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
      this.getNodes(this.domainId);
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
