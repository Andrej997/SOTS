import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
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

  constructor(private testsService: TestsService) { }

  ngOnInit(): void {
    this.getTests();
  }

  private getTests() {
    this.testsService.getTests(1).subscribe(result => {
      this.tests = result as any[];
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
}
