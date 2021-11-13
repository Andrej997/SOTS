import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs/internal/Subscription';
import { TestsService } from '../services/tests.service';

@Component({
  selector: 'app-questions',
  templateUrl: './questions.component.html',
  styleUrls: ['./questions.component.css']
})
export class QuestionsComponent implements OnInit, OnDestroy {

  editRow: boolean = false;
  editQuestion: number = 0;
  private testId: number;
  private routeSub: Subscription;
  questions: any[] = [];
  testText: string = '';
  data: any[] = [];

  constructor(private route: ActivatedRoute,
    private router: Router,
    private testsService: TestsService) { }

  ngOnInit(): void {
    this.routeSub = this.route.params.subscribe(params => {
      this.testId = params['id']; 
      this.getQustions(this.testId);
    });
  }

  addQuestion() {
    this.editRow = true;
    this.editQuestion = 0;
    this.questions.push({
      id: 0,
      text: '',
      testId: this.testId
    })
  }

  private getQustions(testId: number) {
    this.testsService.getQustions(testId).subscribe(result => {
      this.questions = result as any[];
      this.testText = this.questions[0].testText;
      this.data = this.questions;
      console.log(this.questions);
    }, error => {
        console.error(error);
    });
  }

  save(questionId: number) {
    this.editRow = false;
    let index = this.questions.findIndex(x => x.id == questionId);
    if (questionId === 0)
      this.saveNewAnswer();
    else 
      this.updateAnswer(questionId);
  }

  private saveNewAnswer() {
    let body = {
      TestId: this.testId,
      QuestionText: (<HTMLInputElement>document.getElementById("i_0")).value,
      Points: (<HTMLSelectElement>document.getElementById("in_0")).value
    };
    this.testsService.createQuestion(body).subscribe(result => {
      this.getQustions(this.testId);
    }, error => {
        console.error(error);
    });
  }

  private updateAnswer(answerId: number) {
    let body = {
      AnswerId: answerId,
      AnswerText: (<HTMLInputElement>document.getElementById("i_" + answerId)).value,
      IsCorrect: ((<HTMLSelectElement>document.getElementById("s_" + answerId)).value == 'true')? true : false
    };
    this.testsService.editAnswer(body).toPromise()
      .then(result => {
        this.getQustions(this.testId);
      })
      .catch(
        error => {
          console.error(error);
        });
  }

  cancel(questionId: number) {

  }

  delete(questionId: number) {

  }

  ngOnDestroy() {
    this.routeSub.unsubscribe();
  }

  onUserRowSelect(event: any) {
    // console.log(event);
    this.router.navigate([`/test/${event.data.testId}/question/${event.data.id}/answers`]);
  }

  settings = {
    delete: {
      confirmDelete: true,
    },
    add: {
      confirmCreate: true,
    },
    edit: {
      confirmSave: true,
    },
    columns: {
      text: {
        title: 'Question'
      },
      answersCount: {
        title: 'Number of answers'
      },
      points: {
        title: 'Points'
      }
    }
  };
}
