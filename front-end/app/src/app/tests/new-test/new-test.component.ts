import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { DomainService } from 'src/app/services/domain.services';
import { Answer, Question } from '../../models/question';
import { TestsService } from '../../services/tests.service';
import { Edge, Node } from '@swimlane/ngx-graph';
import { GraphService } from 'src/app/services/graph.service';
import { AuthService } from 'src/app/services/auth.service';

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
  domains: any[] = [];
  serviceDomains: any[] = [];

  allLoaded: boolean = false;
  nodes: Node[] = [];
  edges: Edge[] = [];
  domainId: number = 0;
  sourceNodes: Node[] = [];

  constructor(private testsService: TestsService,
    private domainService: DomainService,
    private toastr: ToastrService,
    private graphService: GraphService,
    private auth: AuthService,
    private fb: FormBuilder) { }

  ngOnInit(): void {
    this.testForm = this.fb.group({
      name: ['', Validators.required],
      subject: [0, Validators.required],
      domain: [0, Validators.required],
      maxPoints: [100, Validators.required],
      start: ['', Validators.required],
      end: ['', Validators.required],
    });
    this.getSubjects();
    // this.addQuestion();
    this.getDomains();
  }

  private getSubjects() {
    this.testsService.getSubjects().subscribe(result => {
      this.subjects = result as any[];
      // console.log(this.subjects);
    }, error => {
        console.error(error);
    });
  }

  private getDomains() {
    this.domains = [];
    this.domainService.getDomains().subscribe(result => {
      this.domains = result as any[];
      // console.log(this.domains);
    }, error => {
        console.error(error);
    });
  }

  changeDomain(event: any) {
    if(this.domainId == 0 || this.questions.length == 0) {
      this.domainId = <number>event.target.value;
      this.getNodes(this.domainId);
    }
    else if (this.questions.length > 0) {
      this.toastr.warning("Delete first all questions before changing domain");
      event.target.value = this.domainId;
    }
  }

  private getNodes(domainId: number) {
    this.nodes = [];
    this.sourceNodes = [];
    let body = {
      DomainId: domainId
    };
    this.graphService.getNodes(body).subscribe(result => {
      (result as any[]).forEach(x => {
        let node: Node = {
          id: x.id,
          label: x.label,
          data: {
            customColor: '#807977'
          }
        };
        this.nodes.push(node);
        this.sourceNodes.push(node);
      });
      this.nodes = [...this.nodes];
      this.getEdges(domainId);
    }, error => {
        this.toastr.error(error.error);
        console.error(error);
    });
  }

  private getEdges(domainId: number) {
    this.edges = [];
    let body = {
      DomainId: domainId
    };
    this.graphService.getEdges(body).subscribe(result => {
      (result as any[]).forEach(x => {
        let edge: Edge = {
          id: x.id,
          label: x.label,
          source: x.sourceId,
          target: x.targetId
        };
        this.edges.push(edge);
      });
      this.edges = [...this.edges];
      this.allLoaded = true;
    }, error => {
        this.toastr.error(error.error);
        console.error(error);
    });
  }

  private choosenSubject: number = 0;
  changeSubject(event: any) {
    if (this.questions.length > 0) {
      this.toastr.warning("Delete first all questions before changing subject");
      event.target.value = this.choosenSubject;
    }
    else {
      this.choosenSubject = event.target.value;
      this.serviceDomains = [];
      // console.log(event.target.value);
      this.domains.forEach(domain => {
        if (domain.subjectId == event.target.value)
          this.serviceDomains.push(domain);
      });
    }
  }

  addQuestion(problemNodeId: string) {
    let node = this.nodes.find(x => x.id == problemNodeId);
    if (node?.id.includes('new_node_question_id_')) {
      this.toastr.error('That node represents a question');
    }
    else {
      if (node && node.data && node.data.customColor)
        node.data.customColor = '#15C232';
      let newQuestion = new Question(this.questions.length + 1)
      newQuestion.ProblemNodeId = problemNodeId;
      this.questions.push(newQuestion);
      let newNode: Node = {
        id: 'new_node_question_id_' + (newQuestion.question_id),
        label: '',
        data: {
          customColor: '#3A9FB4'
        }
      };
      this.nodes.push(newNode);
      this.nodes = [... this.nodes];
      let newEdge: Edge = {
        id: 'new_edge_question_id_' + (newQuestion.question_id),
        label: 'containsQuestion',
        source: problemNodeId,
        target: newNode.id
      };
      this.edges.push(newEdge);
      this.edges = [... this.edges];
    }
  }

  deleteQuestion(question_id: number) {
    let index = this.questions.findIndex(x => x.question_id == question_id);
    let question = this.questions.find(x => x.question_id == question_id);
    let nIndex = this.nodes.findIndex(x => x.id == 'new_node_question_id_' + (question?.question_id));
    let eIndex = this.edges.findIndex(x => x.id == 'new_edge_question_id_' + (question?.question_id));
    this.edges.splice(eIndex, 1);
    this.edges = [... this.edges];
    this.nodes.splice(nIndex, 1);
    this.nodes = [... this.nodes];
    this.questions.splice(index, 1);
  }

  addQuestionText(event: any, questionId: number) {
    let question = <Question>this.questions.find(x => x.question_id == questionId);
    question.TextQuestion = event.srcElement.value;
    let node = this.nodes.find(x => x.id == 'new_node_question_id_' + question.question_id);
    if(node != undefined)
      node.label = event.srcElement.value
    this.nodes = [... this.nodes];
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

  clickedNode: Node = {
    id: '',
    label: ''
  }
  clickOnNode(node: Node) {
    this.clickedNode = node;
    console.log(this.clickedNode);
    this.addQuestion(this.clickedNode.id);
  }

  setProblemNodeToQuestion(questionId: number, event: any) {
    let question = this.questions.find(q => q.question_id == questionId);
    if (question != undefined)
      question.ProblemNodeId = event.target.value;
  }

  private createTest() {
    let canCreate: boolean = true;
    
    if (this.testForm.value.name == '') {
      this.toastr.error("Missing name");
      canCreate = false;
      return;
    }

    if (this.testForm.value.domain === 0) {
      this.toastr.error("Missing domain");
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
    
    if (this.questions.length < 2) {
      this.toastr.error("There must be at least two question");
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

        if (x.ProblemNodeId == '') {
          this.toastr.error("Question is missing domain problem");
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
        DomainId: this.testForm.value.domain,
        Questions: this.questions,
        CreatorId: this.auth.getUserId(),
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
