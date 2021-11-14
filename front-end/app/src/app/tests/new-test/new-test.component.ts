import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Answer, Question } from '../../models/question';
import { TestsService } from '../../services/tests.service';

@Component({
  selector: 'app-new-test',
  templateUrl: './new-test.component.html',
  styleUrls: ['./new-test.component.css']
})
export class NewTestComponent implements OnInit {

  testForm: FormGroup;

  subjects: any[] = [];
  questions: Question[] = [];
  answers: Answer[] = [];

  constructor(private testsService: TestsService,
    private toastr: ToastrService,
    private fb: FormBuilder) { }

  ngOnInit(): void {
    this.testForm = this.fb.group({
      name: ['', Validators.required],
      subject: [0, Validators.required],
      maxPoints: [100, Validators.required],
      start: ['', Validators.required],
      end: ['', Validators.required],
    });
    this.getSubjects();
    this.addQuestion();
  }

  private getSubjects() {
    this.testsService.getSubjects().subscribe(result => {
      this.subjects = result as any[];
      console.log(this.subjects);
    }, error => {
        console.error(error);
    });
  }

  addQuestion() {
    let id = this.questions.length + 1;
    this.questions.push(new Question(id));
  }

  deleteQuestion(question_id: number) {
    let index = this.questions.findIndex(x => x.question_id == question_id);
    this.questions.splice(index, 1);
  }

  addQuestionText(event: any, questionId: number) {
    let question = <Question>this.questions.find(x => x.question_id == questionId);
    question.TextQuestion = event.srcElement.value;
  }

  addQuestionPoints(event: any, questionId: number) {
    let question = <Question>this.questions.find(x => x.question_id == questionId);
    question.Points = event.srcElement.value;
  }

  addAnswer(questionId: number) {
    let question = <Question>this.questions.find(x => x.question_id == questionId);
    let id = question.Answers.length + 1;
    question.Answers.push(new Answer(id));
  }

  deleteAnswer(question_id: number, answer_id: number) {
    let q_index = this.questions.findIndex(x => x.question_id == question_id);
    let a_index = this.questions[q_index].Answers.findIndex(x => x.answer_id == answer_id);
    this.questions[q_index].Answers.splice(a_index, 1);
  }

  addAnswerText(event: any, questionId: number, answerId: number) {
    let question = <Question>this.questions.find(x => x.question_id == questionId);
    let answer = <Answer>question.Answers.find(x => x.answer_id == answerId);
    answer.TextAnswer = event.srcElement.value;
  }

  setCorrectAnswer(questionId: number, answerId: number) {
    let question = <Question>this.questions.find(x => x.question_id == questionId);
    let answer = <Answer>question.Answers.find(x => x.answer_id == answerId);
    answer.IsCorrect = !answer.IsCorrect;
  }

  private createTest() {
    let canCreate: boolean = true;
    
    if (this.testForm.value.name == '') {
      this.toastr.error("Missing name");
      canCreate = false;
      return;
    }

    if (this.testForm.value.subject === 0) {
      this.toastr.error("Missing subject");
      canCreate = false;
      return;
    }

    if (this.testForm.value.start == '') {
      this.toastr.error("Missing start date");
      canCreate = false;
      return;
    }

    if (this.testForm.value.end == '') {
      this.toastr.error("Missing end date");
      canCreate = false;
      return;
    }

    let dStart = new Date(this.testForm.value.start);
    console.log(dStart);

    let dEnd = new Date(this.testForm.value.end);
    console.log(dEnd);
    
    if (dStart.getTime() > dEnd.getTime()) {
      this.toastr.error("End of test can't be before start of test");
      canCreate = false;
      return;
    }
    else if (dStart.getTime() == dEnd.getTime()) {
      this.toastr.error("Start and end time can't be the same");
      canCreate = false;
      return;
    }
    
    if (this.questions.length == 0) {
      this.toastr.error("There must be at least one question");
      canCreate = false;
      return;
    }
    else {
      let maxQuestionPointsSum: number = 0;
      this.questions.forEach(x => {
        let questionPoint: number = x.Points;
        if (x.TextQuestion == '') {
          this.toastr.error("Missing question text");
          canCreate = false;
          return;
        }

        if (questionPoint == 0) {
          this.toastr.error("Points of questions must be greater than 0");
          canCreate = false;
          return;
        }
        
        if (x.Answers.length < 2) {
          this.toastr.error("There must be at least two answers for each question");
          canCreate = false;
          return;
        }
        else {
          x.Answers.forEach(a => {
            if (a.TextAnswer == '') {
              this.toastr.error("Missing answer text");
              canCreate = false;
              return;
            }
          });
        }
        maxQuestionPointsSum = +<number>maxQuestionPointsSum + +<number>questionPoint;
      });

      if (maxQuestionPointsSum != this.testForm.value.maxPoints) {
        this.toastr.error("Sum of question points does not equal to max test points");
        canCreate = false;
        return;
      }
    }
    if (canCreate) {
      let body = {
        Name: this.testForm.value.name,
        SubjectId: this.testForm.value.subject,
        Questions: this.questions,
        CreatorId: 1,
        MaxPoints: this.testForm.value.maxPoints,
        Start: this.testForm.value.start,
        End: this.testForm.value.end
      };
      this.testsService.createTest(body).subscribe(result => {
        this.toastr.success("Test created")
      }, error => {
          console.error(error);
      });
    }
  }

  onFirstSubmit() {
    this.createTest();
  }

}
