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

  getTakeTest(testId: number) {
    return this.http.get(environment.api + `Tests/take-test/` + testId);
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

  startTest(body: any) {
    return this.http.post(environment.api + `Users/start/test`, body);
  }

  finishTest(body: any) {
    return this.http.put(environment.api + `Users/finish/test`, body);
  }

  saveUserAnswer(body: any) {
    return this.http.post(environment.api + `QA/save/user/answer`, body);
  }

  createQuestion(body: any) {
    return this.http.post(environment.api + `QA/create/question`, body);
  }

  createTest(body: any) {
    return this.http.post(environment.api + `Tests/create`, body);
  }

  editAnswer(body: any) {
    return this.http.put(environment.api + `QA/update`, body);
  }

  deleteAnswer(answerId: number) {
    return this.http.delete(environment.api + `QA/detele/answer/${answerId}`);
  }

  deleteTest(testId: number) {
    return this.http.delete(environment.api + `Tests/detele/${testId}`);
  }
}