import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
@Injectable({
  providedIn: 'root'
})
export class DomainService {

  constructor(private http: HttpClient) { }

  getDomains() {
    return this.http.get(environment.api + `Domain`);
  }

  createDomain(body: any) {
    return this.http.post(environment.api + `Domain/create/domain`, body);
  }
}