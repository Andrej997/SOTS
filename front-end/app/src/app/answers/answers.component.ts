import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs/internal/Subscription';
import { TestsService } from '../services/tests.service';

@Component({
  selector: 'app-answers',
  templateUrl: './answers.component.html',
  styleUrls: ['./answers.component.css']
})
export class AnswersComponent implements OnInit {

  editRow: boolean = false;
  editAnswer: number = 0;
  testId: number;
  private questionId: number;
  private routeSub: Subscription;
  answers: any[] = [];

  constructor(private route: ActivatedRoute,
    private testsService: TestsService) { }

  ngOnInit(): void {
    this.routeSub = this.route.params.subscribe(params => {
      this.testId = params['t_id']; 
      this.questionId = params['q_id']; 
      this.getAnswers(this.testId, this.questionId);
    });
  }

  edit(answerId: number) {
    this.editRow = true;
    this.editAnswer = answerId;
  }

  cancel(answerId: number) {
    this.editRow = false;
    let index = this.answers.findIndex(x => x.id == answerId);
    if (answerId === 0)
      this.answers.splice(index, 1);
  }

  save(answerId: number) {
    this.editRow = false;
    let index = this.answers.findIndex(x => x.id == answerId);
    if (answerId === 0)
      this.saveNewAnswer();
    else 
      this.updateAnswer();
  }

  addAnswer() {
    this.editRow = true;
    this.editAnswer = 0;
    this.answers.push({
      id: 0,
      text: '',
      isCorrect: false
    })
  }

  private saveNewAnswer() {
    let body = {
      TestId: this.testId,
      QuestionId: this.questionId,
      AnswerText: (<HTMLInputElement>document.getElementById("i_0")).value,
      IsCorrect: ((<HTMLSelectElement>document.getElementById("s_0")).value == 'true')? true : false
    };
    this.testsService.createAnswer(body).subscribe(result => {
      this.getAnswers(this.testId, this.questionId);
    }, error => {
        console.error(error);
    });
  }

  private updateAnswer() {

  }

  private getAnswers(testId: number, questionId: number) {
    this.answers = [];
    this.testsService.getAnswers(testId, questionId).subscribe(result => {
      this.answers = result as any[];
      console.log(this.answers);
    }, error => {
        console.error(error);
    });
  }

  ngOnDestroy() {
    this.routeSub.unsubscribe();
  }
}
