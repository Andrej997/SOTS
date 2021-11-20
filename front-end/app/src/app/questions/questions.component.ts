import { AfterViewInit, Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DefaultEditor } from 'ng2-smart-table';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs/internal/Subscription';
import { DomainService } from '../services/domain.services';
import { GraphService } from '../services/graph.service';
import { TestsService } from '../services/tests.service';

@Component({
  selector: 'app-questions',
  templateUrl: './questions.component.html',
  styleUrls: ['./questions.component.css']
})
export class QuestionsComponent implements OnInit, OnDestroy {

  nodes: any[] = [];
  editRow: boolean = false;
  editQuestion: number = 0;
  private testId: number;
  private routeSub: Subscription;
  questions: any[] = [];
  testText: string = '';
  data: any[] = [];
  nodesForTableSelect: any[] = [];

  settings: any = {};

  constructor(private route: ActivatedRoute,
    private toastr: ToastrService,
    private router: Router,
    private graphService: GraphService,
    private domainService: DomainService,
    private testsService: TestsService) { 
      this.settings = this.setSetings();
    }

  ngOnInit(): void {
    this.routeSub = this.route.params.subscribe(params => {
      this.testId = params['id']; 
      this.getQustions(this.testId);
      this.getTest(this.testId);
    });
  }

  private getTest(testId: number) {
    this.testsService.getTest(testId).subscribe(result => {
      this.testText = (result as any).name;
      this.getNodes((result as any).domainId);
    }, error => {
        this.toastr.error(error.error);
        console.error(error);
    });
  }


  private getNodes(domainId: number) {
    this.nodes = [];
    let body = {
      DomainId: domainId
    };
    this.graphService.getNodes(body).subscribe(result => {
      (result as any[]).forEach(x => {
        this.nodesForTableSelect.push({
          value: x.id,
          title: x.label
        });
      });
      this.settings = this.setSetings();
    }, error => {
        this.toastr.error(error.error);
        console.error(error);
    });
  }

  private getQustions(testId: number) {
    this.testsService.getQustions(testId).subscribe(result => {
      this.questions = result as any[];
      this.data = this.questions;
      console.log(this.questions);
    }, error => {
        console.error(error);
    });
  }

  save(data: any) {
    let canCreate: boolean = true;

    if (data.text == '') {
      this.toastr.error("Question can't be empty");
      canCreate = false;
      return;
    }

    let points = Number.parseInt(data.points);
    if (points <= 0) {
      this.toastr.error("Points can't be that low");
      canCreate = false;
      return;
    }

    if (canCreate) {
      let body = {
        TestId: this.testId,
        QuestionText: data.text,
        Points: points,
        ProblemNodeId: data.problemNodeLabel
      };
      this.testsService.createQuestion(body).subscribe(result => {
        this.getQustions(this.testId);
      }, error => {
          console.error(error);
      });
    }
  }

  edit(data: any) {
    console.log(data);
    
    if (data.text == '') {
      this.toastr.error("Question can't be empty");
      return;
    }

    let points = Number.parseInt(data.points);
    if (points <= 0) {
      this.toastr.error("Points can't be that low");
      return;
    }

    let body = {
      QuestionId: data.id,
      QuestionText: data.text,
      Points: points,
      ProblemNodeId: data.problemNodeLabel
    };
    this.testsService.editQuestion(body).toPromise()
      .then(result => {
        this.getQustions(this.testId);
      })
      .catch(
        error => {
          console.error(error);
        });
  }

  delete(questionId: number) {
    this.testsService.deleteQuestion(questionId).toPromise()
    .then(result => {
      this.getQustions(this.testId);
    })
    .catch(
      error => {
        console.error(error);
      });
  }

  ngOnDestroy() {
    this.routeSub.unsubscribe();
  }

  onUserRowSelect(event: any) {
    this.router.navigate([`/test/${event.data.testId}/question/${event.data.id}/answers`]);
  }

 setSetings() {
  return {
    rowClassFunction: (row: any) => {
      if (row.data.answersCount == 0) {
          return 'text-danger';
      } else {
          return 'text';
      }
    },
    actions: {
      position: 'right'
    },
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
      text: {
        title: 'Question'
      },
      problemNodeLabel: {
        title: 'Domain problem',
        type: 'html',
        valuePrepareFunction: (cell: any, row: any) => { return row.problemNodeLabel },
        editor: {
          type: 'list',
          config: {
            list: this.nodesForTableSelect
          }
        }
        // editor: {
        //   type: 'custom',
        //   component: CustomEditorComponent,
        // },
      },
      answersCount: {
        title: 'Number of answers',
        editable:false,
        addable: false,
      },
      points: {
        title: 'Points'
      }
    }
  };
 }
  
}

// @Component({
//   template: `
//   <select class="form-control" placeholder="Domain problem" (change)="setProblemNodeToQuestion($event)">
//     <option hidden disabled selected value>Select domain problem</option>
//     <option *ngFor="let domain of domains" [value]="domain.id">{{ domain.name }}</option>
//   </select>
//   `,
// })
// export class CustomEditorComponent extends DefaultEditor implements AfterViewInit {

//   domains: any[] = [];

//   constructor(private domainService: DomainService) {
//     super();
//     this.getDomains();
//   }


//   private getDomains() {
//     this.domains = [];
//     this.domainService.getDomains().subscribe(result => {
//       this.domains = result as any[];
//       console.log(this.domains);
//     }, error => {
//         console.error(error);
//     });
//   }

//   ngAfterViewInit() {
//     if (this.cell.newValue !== '') {
//       // this.name.nativeElement.value = this.getUrlName();
//       // this.url.nativeElement.value = this.getUrlHref();
//     }
//   }

//   setProblemNodeToQuestion(event: any) {

//   }

// }