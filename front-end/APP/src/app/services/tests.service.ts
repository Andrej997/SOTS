import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
@Injectable({
  providedIn: 'root'
})
export class TestsService {

  constructor(private http: HttpClient) { }

  getTests(userId: number) {
    let params = new HttpParams().set('userId', userId);
    return this.http.get(environment.api + `Tests`, {params: params});
  }

  getSubjects() {
    return this.http.get(environment.api + `Tests/subjects`);
  }

  getQustions(testId: number) {
    return this.http.get(environment.api + `QA/test/${testId}`);
  }

  getAnswers(testId: number, questionId: number) {
    return this.http.get(environment.api + `QA/test/${testId}/question/${questionId}`);
  }

  createAnswer(body: any) {
    return this.http.post(environment.api + `QA/create/answer`, body);
  }

  createTest(body: any) {
    return this.http.post(environment.api + `Tests/create`, body);
  }
}