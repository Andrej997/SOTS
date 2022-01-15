import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthGuard } from 'src/app/guards/auth.guard';
import { TestsService } from 'src/app/services/tests.service';

@Component({
  selector: 'app-results',
  templateUrl: './results.component.html',
  styleUrls: ['./results.component.css']
})
export class ResultsComponent implements OnInit {

  tests: any[] = [];
  seeTestResults: boolean = false;
  studentTestId: number = 0;
  test: any;

  constructor(private testsService: TestsService,
    private router: Router,
    private toastr: ToastrService,
    public authGuard: AuthGuard) { }

  ngOnInit(): void {
    this.getTests();
  }

  seeTestResult(studentTestId: number) {
    this.test = this.tests.find(t => t.id == studentTestId);
  }

  seeRealKnowlageGraph(studentTestId: number) {
    this.router.navigate([`/user-knowledge-graph/${studentTestId}`]);
  }

  private getTests() {
    this.testsService.getTestOfStudent(this.authGuard.getId()).subscribe(result => {
      this.tests = (result as any);
      // console.log(this.tests);
      
    }, error => {
        console.error(error);
    });
  }

}
