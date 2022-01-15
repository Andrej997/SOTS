import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { TestsService } from '../services/tests.service';

@Component({
  selector: 'app-subjects',
  templateUrl: './subjects.component.html',
  styleUrls: ['./subjects.component.css']
})
export class SubjectsComponent implements OnInit {

  data: any[] = [];
  subjects: any[] = [];

  constructor(private testsService: TestsService,
    private toastr: ToastrService) { }

  ngOnInit(): void {
    this.getSubjects();
  }

  add(data: any) {
    let body = {
      Name: data.newData.name,
      Description: data.newData.description,
    };
    this.testsService.addSubject(body).subscribe(result => {
      this.getSubjects();
      this.toastr.success("Created");
    }, error => {
        console.error(error);
        this.toastr.error(error.error);
    });
  }

  edit(data: any) {
    let body = {
      Id: data.id,
      Name: data.name,
      Description: data.description,
    };
    this.testsService.editSubject(body).toPromise()
      .then(result => {
        this.getSubjects();
        this.toastr.success("Updated");
      })
      .catch(
        error => {
          console.error(error);
          this.toastr.error(error.error);
        });
  }

  private getSubjects() {
    this.testsService.getSubjects().subscribe(result => {
      this.subjects = result as any[];
      this.data = this.subjects;
    }, error => {
        console.error(error);
        this.toastr.error(error.error);
    });
  }

  settings = {
    actions: {
      position: 'right',
      delete: false
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
      description: {
        title: 'Description'
      }
    }
  };

}
