import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthGuard } from '../guards/auth.guard';
import { DomainService } from '../services/domain.services';
import { TestsService } from '../services/tests.service';

@Component({
  selector: 'app-domains',
  templateUrl: './domains.component.html',
  styleUrls: ['./domains.component.css']
})
export class DomainsComponent implements OnInit {

  domainForm: FormGroup;
  subjects: any[] = [];
  domains: any[] = [];

  constructor(private testsService: TestsService,
    private domainService: DomainService,
    private router: Router,
    private fb: FormBuilder,
    private toastr: ToastrService,
    public authGuard: AuthGuard) { }

  ngOnInit(): void {
    this.domainForm = this.fb.group({
      name: ['', Validators.required],
      subject: [0, Validators.required],
      description: ['', Validators.required]
    });
    this.getSubjects();
    this.getDomains();
  }

  private getDomains() {
    this.domains = [];
    this.domainService.getDomains().subscribe(result => {
      this.domains = result as any[];
      console.log(this.domains);
      
    }, error => {
        console.error(error);
    });
  }

  private getSubjects() {
    this.testsService.getSubjects().subscribe(result => {
      this.subjects = result as any[];
    }, error => {
        console.error(error);
    });
  }

  private createDomain() {
    let body = {
      Name: this.domainForm.value.name,
      Description: this.domainForm.value.description,
      SubjectId: this.domainForm.value.subject
    };
    this.domainService.createDomain(body).subscribe(result => {
      this.toastr.success('Domain created');
      this.getDomains();
    }, error => {
        console.error(error);
    });
  }

  onFirstSubmit() {
    this.createDomain();
  }
}
