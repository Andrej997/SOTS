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

  testForm: FormGroup;

  subjects: any[] = [];
  questions: Question[] = [];
  answers: Answer[] = [];

  constructor(private testsService: TestsService,
    private fb: FormBuilder) { }

  ngOnInit(): void {
    this.testForm = this.fb.group({
      name: ['', Validators.required],
      subject: [0, Validators.required],
    });
    this.getSubjects();
    
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

  addQuestionText(event: any, questionId: number) {
    let question = <Question>this.questions.find(x => x.question_id == questionId);
    question.TextQuestion = event.srcElement.value;
  }

  addAnswer(questionId: number) {
    let question = <Question>this.questions.find(x => x.question_id == questionId);
    let id = question.Answers.length + 1;
    question.Answers.push(new Answer(id));
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
    console.log(this.questions);
    
    let body = {
      Name: this.testForm.value.name,
      SubjectId: this.testForm.value.subject,
      Questions: this.questions,
      CreatorId: 1
    };
    this.testsService.createTest(body).subscribe(result => {
      console.log("Created");
    }, error => {
        console.error(error);
    });
  }

  onFirstSubmit() {
    console.log(this.testForm.value.name);
    this.createTest();
  }
}
