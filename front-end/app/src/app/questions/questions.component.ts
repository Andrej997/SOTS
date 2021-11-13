import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
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
    private toastr: ToastrService,
    private router: Router,
    private testsService: TestsService) { }

  ngOnInit(): void {
    this.routeSub = this.route.params.subscribe(params => {
      this.testId = params['id']; 
      this.getQustions(this.testId);
    });
  }

  private getQustions(testId: number) {
    this.testsService.getQustions(testId).subscribe(result => {
      this.questions = result as any[];
      if (this.questions.length > 1) {
        this.testText = this.questions[0].testText;
      } else {
        this.testText = 'Missing answers';
        this.toastr.warning('Missing answers');
      }
      this.data = this.questions;
      console.log(this.questions);
    }, error => {
        console.error(error);
    });
  }

  save(data: any) {
    let canCreate: boolean = true;

    if (data.text == '') {
      this.toastr.error("Question can't be empty");
      canCreate = false;
      return;
    }

    let points = Number.parseInt(data.points);
    if (points <= 0) {
      this.toastr.error("Points can't be that low");
      canCreate = false;
      return;
    }

    if (canCreate) {
      let body = {
        TestId: this.testId,
        QuestionText: data.text,
        Points: points
      };
      this.testsService.createQuestion(body).subscribe(result => {
        this.getQustions(this.testId);
      }, error => {
          console.error(error);
      });
    }
  }

  edit(data: any) {
    if (data.text == '') {
      this.toastr.error("Question can't be empty");
      return;
    }

    let points = Number.parseInt(data.points);
    if (points <= 0) {
      this.toastr.error("Points can't be that low");
      return;
    }

    let body = {
      QuestionId: data.id,
      QuestionText: data.text,
      Points: points
    };
    this.testsService.editQuestion(body).toPromise()
      .then(result => {
        this.getQustions(this.testId);
      })
      .catch(
        error => {
          console.error(error);
        });
  }

  delete(questionId: number) {
    this.testsService.deleteQuestion(questionId).toPromise()
    .then(result => {
      this.getQustions(this.testId);
    })
    .catch(
      error => {
        console.error(error);
      });
  }

  ngOnDestroy() {
    this.routeSub.unsubscribe();
  }

  onUserRowSelect(event: any) {
    this.router.navigate([`/test/${event.data.testId}/question/${event.data.id}/answers`]);
  }

  settings = {
    rowClassFunction: (row: any) => {
      if (row.data.answersCount == 0) {
          return 'text-danger';
      } else {
          return 'text';
      }
    },
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
        title: 'Number of answers',
        editable:false,
        addable: false,
      },
      points: {
        title: 'Points'
      }
    }
  };
}
