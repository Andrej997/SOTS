import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthGuard } from '../guards/auth.guard';
import { Answer, Question } from '../models/question';
import { TestsService } from '../services/tests.service';

@Component({
  selector: 'app-tests',
  templateUrl: './tests.component.html',
  styleUrls: ['./tests.component.css']
})
export class TestsComponent implements OnInit {

  editRow: boolean = false;
  editTest: number = 0;
  tests: any[] = [];

  data: any[] = [];

  constructor(private testsService: TestsService,
    private router: Router,
    public authGuard: AuthGuard) { }

  ngOnInit(): void {
    this.getTests();
  }

  private getTests() {
    this.testsService.getTests(1).subscribe(result => {
      this.tests = result as any[];
      this.data = this.tests;
      console.log(this.tests);
    }, error => {
        console.error(error);
    });
  }

  delete(testId: number) {
    this.testsService.deleteTest(testId).toPromise()
    .then(result => {
      this.getTests();
    })
    .catch(
      error => {
        console.error(error);
      });
  }

  onUserRowSelect(event: any) {
    if (this.authGuard.isStudent())
      this.router.navigate([`/take-test/${event.data.id}`]);
    else
      this.router.navigate([`/test/${event.data.id}/questions`]);
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
      name: {
        title: 'Name'
      },
      questionCount: {
        title: 'Number of questions'
      },
      start: {
        title: 'Start of test'
      },
      end: {
        title: 'End of test'
      },
      created: {
        title: 'Test created'
      },
      maxPoints: {
        title: 'Max points'
      }
    }
  };
}
