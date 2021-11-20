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

  getTest(testId: number) {
    let params = new HttpParams().set('testId', testId);
    return this.http.get(environment.api + `Tests/test`, {params: params});
  }

  getTestOfStudent(userId: number) {
    let params = new HttpParams().set('userId', userId);
    return this.http.get(environment.api + `Users/tests`, {params: params});
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

  questionStartTime(body: any) {
    return this.http.post(environment.api + `QA/question/starttime`, body);
  }

  questionEndTime(body: any) {
    return this.http.post(environment.api + `QA/question/endtime`, body);
  }

  createAnswer(body: any) {
    return this.http.post(environment.api + `QA/create/answer`, body);
  }

  choosenAnswers(body: any) {
    return this.http.post(environment.api + `Users/choosenanswers`, body);
  }

  startTest(body: any) {
    return this.http.post(environment.api + `Users/start/test`, body);
  }

  finishTest(body: any) {
    return this.http.post(environment.api + `Users/finish/test`, body);
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
    return this.http.put(environment.api + `QA/update/answer`, body);
  }

  editQuestion(body: any) {
    return this.http.put(environment.api + `QA/update/question`, body);
  }

  editTest(body: any) {
    return this.http.put(environment.api + `Tests/update`, body);
  }

  publishTest(testId: number) {
    return this.http.get(environment.api + `Tests/publish/` + testId);
  }

  deleteAnswer(answerId: number) {
    return this.http.delete(environment.api + `QA/detele/answer/${answerId}`);
  }

  deleteQuestion(questionId: number) {
    return this.http.delete(environment.api + `QA/detele/${questionId}`);
  }

  deleteTest(testId: number) {
    return this.http.delete(environment.api + `Tests/detele/${testId}`);
  }
}