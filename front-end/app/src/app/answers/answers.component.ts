import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
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
  questionText: string = '';
  data: any[] = [];

  constructor(private route: ActivatedRoute,
    private toastr: ToastrService,
    private router: Router,
    private testsService: TestsService) { }

  ngOnInit(): void {
    this.routeSub = this.route.params.subscribe(params => {
      this.testId = params['t_id']; 
      this.questionId = params['q_id']; 
      this.getAnswers(this.testId, this.questionId);
    });
  }

  save(data: any) {
    let canSave: boolean = true;

    if (data.text == '') {
      this.toastr.error("Answer can't be empty");
      canSave = false;
      return;
    }
    else {
      this.answers.forEach(result => {
        if (result.text == data.text) {
          this.toastr.error("Answer already exists");
          canSave = false;
          return;
        }
      });
    }

    let isCorrectStr = data.isCorrect.toString();

    if (isCorrectStr != 'true' && isCorrectStr != 'false') {
      this.toastr.error("Not a boolean value");
      canSave = false;
      return;
    }

    if (canSave) {
      let body = {
        TestId: this.testId,
        QuestionId: this.questionId,
        AnswerText: data.text,
        IsCorrect: (isCorrectStr == 'true')? true : false
      };
      this.testsService.createAnswer(body).subscribe(result => {
        this.getAnswers(this.testId, this.questionId);
      }, error => {
          console.error(error);
      });
    }
  }


  edit(data: any) {
    if (data.text == '') {
      this.toastr.error("Answer can't be empty");
      return;
    }

    let isCorrectStr = data.isCorrect.toString();

    if (isCorrectStr != 'true' && isCorrectStr != 'false') {
      this.toastr.error("Not a boolean value");
      return;
    }
    
    let body = {
      AnswerId: data.id,
      AnswerText: data.text,
      IsCorrect: (data.isCorrect == 'true')? true : false
    };
    this.testsService.editAnswer(body).toPromise()
      .then(result => {
        this.getAnswers(this.testId, this.questionId);
      })
      .catch(
        error => {
          console.error(error);
        });
  }

  delete(answerId: number) {
    this.testsService.deleteAnswer(answerId).toPromise()
      .then(result => {
        this.getAnswers(this.testId, this.questionId);
      })
      .catch(
        error => {
          console.error(error);
        });
  }

  private getAnswers(testId: number, questionId: number) {
    this.answers = [];
    this.testsService.getAnswers(testId, questionId).subscribe(result => {
      this.answers = result as any[];
      if (this.answers.length > 1) {
        this.questionText = this.answers[0].questionText;
      } else {
        this.questionText = 'Missing answers';
        this.toastr.warning('Missing answers');
      }
      
      this.data = this.answers;
      console.log(this.answers);
    }, error => {
        console.error(error);
    });
  }

  ngOnDestroy() {
    this.routeSub.unsubscribe();
  }

  onUserRowSelect(event: any) {
    // console.log(event);
  }

  settings = {
    actions: {
      position: 'right'
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
      isCorrect: {
        title: 'Is correct'
      }
    }
  };
}
