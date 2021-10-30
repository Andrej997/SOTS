import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
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
      console.log("Created");
    }, error => {
        console.error(error);
    });
  }

  onFirstSubmit() {
    this.createTest();
  }

}
