import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
@Injectable({
  providedIn: 'root'
})
export class GraphService {

  constructor(private http: HttpClient) { }

  getNodes(body: any) {
    return this.http.post(environment.api + `Graph/get/nodes`, body);
  }

  getEdges(body: any) {
    return this.http.post(environment.api + `Graph/get/edges`, body);
  }

  expectedKnowlageGraphForTest(testId: any) {
    return this.http.get(environment.api + `Graph/expected-knowlage-domain/test/` + testId);
  }

  realKnowlageGraphForTest(testId: any) {
    return this.http.get(environment.api + `Graph/real-knowlage-domain/test/` + testId);
  }

  createNode(body: any) {
    return this.http.post(environment.api + `Graph/create/node`, body);
  }

  createEdge(body: any) {
    return this.http.post(environment.api + `Graph/create/edge`, body);
  }

  deleteNode(nodeId: string) {
    return this.http.delete(environment.api + `Graph/node/${nodeId}`);
  }

  deleteEdge(body: any) {
    return this.http.post(environment.api + `Graph/delete/edges`, body);
  }
}