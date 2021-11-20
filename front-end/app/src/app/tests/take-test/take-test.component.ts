import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CountdownComponent, CountdownConfig } from 'ngx-countdown';
import { ToastrService } from 'ngx-toastr';
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
  private startTestId: number = 0;

  constructor(private route: ActivatedRoute,
    private toastr: ToastrService,
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
    let date = new Date(Date.now());
    if ((new Date(this.test.start).getTime() < date.getTime()) === false) {
      this.toastr.error("Can't start test yet");
    }
    else {
      this.testsService.startTest(body).subscribe(result => {
        this.toastr.success('Test started');
        this.startTestId = result as number;
        console.log(this.startTestId);
        
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
        this.startQuestionTime(this.currentQuestion.id, this.startTestId);
      }, error => {
          console.error(error);
      });
    }
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
      let body = {
        AnswersId: this.userAnswers,
        QuestionId: this.currentQuestion.id,
        StudentTestId: this.startTestId
      };
      this.testsService.saveUserAnswer(body).subscribe(result => {
        this.userAnswers = [];
        
        this.currentQuestion = this.test.questions[this.currentQuestionCounter];
        this.startQuestionTime(this.currentQuestion.id, this.startTestId);
      }, error => {
          console.error(error);
      });
      
      if (this.currentQuestionCounter + 1 == this.questionCount) {
        this.showNextBtn = false;
        this.showFinishBtn = true;
      }
    }
  }

  userAnswers: number[] = [];
  storeAnswer(answerId: number) {
    let elem = <HTMLElement>document.getElementById('o_'+answerId);
    
    if (!this.userAnswers.includes(answerId)) {
      elem.style.background = 'green';
      this.userAnswers.push(answerId);
    }
    else {
      let index = this.userAnswers.findIndex(x => x == answerId);
      this.userAnswers.splice(index, 1);
      elem.style.background = 'white';
    }
  }

  startQuestionTime(questionId: number, startTestId: number) {
    let body = {
      StudentTestsId: startTestId,
      QuestionId: questionId,
    }
    this.testsService.questionStartTime(body).subscribe(result => {
    }, error => {
        console.error(error);
    });
  }

  finishTest: any;
  finishQuestion() {
    let body = {
      AnswersId: this.userAnswers,
      QuestionId: this.currentQuestion.id,
      StudentTestId: this.startTestId
    };
    this.testsService.saveUserAnswer(body).subscribe(result => {
      this.userAnswers = [];
      let body = {
        StudentTestId: this.startTestId,
        TestId: this.testId,
        UserId: this.authService.getUserId()
      };
      this.testsService.finishTest(body).subscribe(result => {
        this.testStarted = false;
        this.toastr.success('Test finished');

        console.log((result as any));
        

        this.finishTest = (result as any);
        this.getChoosenAnswer();
      }, error => {
          console.error(error);
      });
    }, error => {
        console.error(error);
    });
  }

  showChoosenAnswer: boolean = false;
  choosenAnswerDto: any;
  getChoosenAnswer() {
    let body = {
      StudentTestId: this.startTestId,
      UserId: this.authService.getUserId()
    };
    this.testsService.choosenAnswers(body).subscribe(result => {
      this.choosenAnswerDto = result;
      this.showChoosenAnswer = true;
      console.log(this.choosenAnswerDto);
      
    }, error => {
        console.error(error);
    });
  }
}
