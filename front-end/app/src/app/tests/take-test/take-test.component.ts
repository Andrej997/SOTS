import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CountdownComponent, CountdownConfig } from 'ngx-countdown';
import { Subscription } from 'rxjs/internal/Subscription';
import { AuthService } from 'src/app/services/auth.service';
import { TestsService } from 'src/app/services/tests.service';

@Component({
  selector: 'app-take-test',
  templateUrl: './take-test.component.html',
  styleUrls: ['./take-test.component.css']
})
export class TakeTestComponent implements OnInit {

  @ViewChild('cd', { static: false }) private countdown: CountdownComponent;
  config: CountdownConfig = {
    leftTime: 0
  };

  private testId: number;
  private routeSub: Subscription;
  test: any;
  testLoaded: boolean = false;
  testStarted: boolean = false;
  questionCount: number = 0;
  currentQuestionCounter: number = 0;
  currentQuestion: any;

  constructor(private route: ActivatedRoute,
    private testsService: TestsService,
    private authService: AuthService) { }

  ngOnInit(): void {
    this.routeSub = this.route.params.subscribe(params => {
      this.testId = params['t_id']; 
      this.getTakeTest(this.testId);
    });
  }

  private getTakeTest(testId: number) {
    this.testsService.getTakeTest(testId).subscribe(result => {
      this.test = result as any;
      this.questionCount = this.test.questions.length;
      if (this.questionCount > 0) {
        this.currentQuestion = this.test.questions[0];
        console.log(this.currentQuestion);
        
      }
      this.testLoaded = true;
      console.log(result);
    }, error => {
        console.error(error);
    });
  }

  startTest() {
    let body = {
      TestId: this.testId,
      UserId: this.authService.getUserId()
    }
    this.testsService.startTest(body).subscribe(result => {
      this.testLoaded = false;
      this.testStarted = true;
      let dstart = new Date(this.test.start);
      let dend = new Date(this.test.end);
      let startNum = (dstart.getHours() * 3600) + (dstart.getMinutes() * 60) + dstart.getSeconds();
      let endNum = (dend.getHours() * 3600) + (dend.getMinutes() * 60) + dend.getSeconds();
      this.config = {
        leftTime: endNum - startNum
      }; 
      this.countdown.restart();
    }, error => {
        console.error(error);
    });
  }

  showNextBtn: boolean = true;
  showFinishBtn: boolean = false;
  nextQuestion() {
    ++this.currentQuestionCounter;
    if (this.currentQuestionCounter == this.questionCount) {
      this.showNextBtn = false;
      this.showFinishBtn = true;
    }
    else {
      this.currentQuestion = this.test.questions[this.currentQuestionCounter];
      if (this.currentQuestionCounter + 1 == this.questionCount) {
        this.showNextBtn = false;
        this.showFinishBtn = true;
      }
    }
  }

  finishQuestion() {

  }
}
