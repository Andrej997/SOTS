import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthGuard } from '../guards/auth.guard';
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

  data: any[] = [];

  constructor(private testsService: TestsService,
    private router: Router,
    private toastr: ToastrService,
    public authGuard: AuthGuard) { }

  ngOnInit(): void {
    this.getTests();
  }

  private getTests() {
    this.testsService.getTests(this.authGuard.getId()).subscribe(result => {
      this.tests = result as any[];
      this.data = this.tests;
      console.log(this.tests);
    }, error => {
        console.error(error);
    });
  }

  edit(data: any) {
    let canEdit: boolean = true;
    if (data.name == '') {
      this.toastr.error("Test name can't be empty");
      canEdit = false;
      return;
    }

    if (data.name == '') {
      this.toastr.error("Test name can't be empty");
      canEdit = false;
      return;
    }

    let points = Number.parseInt(data.maxPoints);
    if (points <= 0) {
      this.toastr.error("Max points can't be that low");
      canEdit = false;
      return;
    }
    else if (points > 100) {
      this.toastr.error("Max points can't be that big");
      canEdit = false;
      return;
    }

    if (canEdit) {
      let body = {
        TestId: data.id,
        TestText: data.name,
        MaxPoints: points
      };
      this.testsService.editTest(body).toPromise()
        .then(result => {
          this.getTests();
        })
        .catch(
          error => {
            console.error(error);
          });
    }
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

  onUserRowSelect(event: any) {
    if (this.authGuard.isStudent())
      this.router.navigate([`/take-test/${event.data.id}`]);
    else
      this.router.navigate([`/test/${event.data.id}/questions`]);
  }

  onCustomAction(event: any) {
    console.log(event);
    if (event.action === 'publish') {
      this.testsService.publishTest(event.data.id).subscribe(result => {
        this.getTests();
      }, error => {
          this.toastr.error(error.error);
          console.error(error);
      });
    }
    else if (event.action === 'expextedKnowlage') {
      this.router.navigate([`/expected-knowlage-graph/${event.data.id}`]);
    }
    else if (event.action === 'realKnowlage') {
      this.router.navigate([`/real-knowlage-graph/${event.data.id}`]);
    }
    else if (event.action === 'qti') {
      this.testsService.dowloadQTI(event.data.id).subscribe(result => {
        console.log(result);
        let type = 'application/zip';
        let name = 'file.zip';
        let byteCharacters = null;
        let byteArray = null;
        let fileName = null;
        if ((result as any).fileContents || (result as any).fileBytes) {
            let content = (result as any).fileContents ? (result as any).fileContents : (result as any).fileBytes;
              byteCharacters =  atob(content);
              const byteNumbers = new Array(byteCharacters.length);
              for (let i = 0; i < byteCharacters.length; i++) {
                  byteNumbers[i] = byteCharacters.charCodeAt(i);
              }
            byteArray = new Uint8Array(byteNumbers);
            fileName = name ? name : (result as any).fileDownloadName;
        } else {
            fileName = name ? name : (result as any).name;
            byteArray = (result as any);
        }

        const blob = new Blob([byteArray], { type });

        const url = window.URL.createObjectURL(blob);
        var anchor = document.createElement("a");
        anchor.download = fileName;
        anchor.href = url;
        anchor.click();
        anchor.remove();
      }, error => {
          this.toastr.error(error.error);
          console.error(error);
      });
    }
    else if (event.action === 'comporeKnowledge') {
      this.router.navigate([`/compore-knowledge/${event.data.id}`]);
    }
  }

  settings = {
    actions: !this.authGuard.isStudent() ? {
        add: false,
        custom: [
          { name: 'publish', title: 'Publish '},
          { name: 'expextedKnowlage', title: 'Expexted knowledge '},
          { name: 'realKnowlage', title: 'Real knowledge '},
          { name: 'qti', title: 'Download qti '},
          { name: 'comporeKnowledge', title: 'Compore knowledge '}
        ],
        position: 'right'
      } : false,
    rowClassFunction: (row: any) => {
      if (row.data.questionCount == 0) {
          return 'text-danger';
      } else {
          return 'text';
      }
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
      name: {
        title: 'Name'
      },
      subjectName: {
        title: 'Subject',
        editable:false
      },
      domainName: {
        title: 'Domain',
        editable:false
      },
      questionCount: {
        title: 'Number of questions',
        editable:false
      },
      start: {
        title: 'Start of test',
        editable:false
      },
      end: {
        title: 'End of test',
        editable:false
      },
      creatorName: {
        title: 'Test creator',
        editable:false
      },
      maxPoints: {
        title: 'Max points'
      },
      published: {
        title: 'Published',
        editable:false
      }
    }
  };
}
