import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs/internal/Subscription';
import { TestsService } from '../services/tests.service';

@Component({
  selector: 'app-questions',
  templateUrl: './questions.component.html',
  styleUrls: ['./questions.component.css']
})
export class QuestionsComponent implements OnInit, OnDestroy {

  private testId: number;
  private routeSub: Subscription;
  questions: any[] = [];

  constructor(private route: ActivatedRoute,
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
      console.log(this.questions);
    }, error => {
        console.error(error);
    });
  }

  ngOnDestroy() {
    this.routeSub.unsubscribe();
  }
}
