import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
@Injectable({
  providedIn: 'root'
})
export class TestsService {

  constructor(private http: HttpClient) { }

  getSubjects() {
    return this.http.get(environment.api + `Tests/subjects`);
  }

  createTest(body: any) {
    return this.http.post(environment.api + `Tests/create`, body);
  }
}