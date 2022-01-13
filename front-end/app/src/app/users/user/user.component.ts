import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { TestsService } from 'src/app/services/tests.service';
import { UsersService } from 'src/app/services/users.services';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent implements OnInit {

  userForm: FormGroup;

  private userId: number;
  subjects: any[] = [];
  roles: any[] = [];
  user: any;
  private routeSub: Subscription;

  constructor(private usersService: UsersService,
    private testsService: TestsService,
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private router: Router,
    private toastr: ToastrService) { }

  ngOnInit(): void {
    this.userForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required],
      name: ['', Validators.required],
      surname: ['', Validators.required],
      role: [0, Validators.required],
      subjects: [[], Validators.required],
    });
    this.routeSub = this.route.params.subscribe(params => {
      this.userId = params['id']; 
      if (this.userId != 0) {
        this.getUser(this.userId);
      }
    });
    this.getSubjects();
    this.getRoles();
  }

  private getUser(userId: number) {
    this.usersService.getUser(userId).subscribe(result => {
      this.user = result as any;
      console.log(this.user);
      
      this.userForm = this.fb.group({
        username: [this.user.username, Validators.required],
        password: ['', Validators.required],
        name: [this.user.name, Validators.required],
        surname: [this.user.surname, Validators.required],
        role: [this.user.roleId, Validators.required],
        subjects: [this.user.subjectIds, Validators.required],
      });
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

  private getRoles() {
    this.usersService.getRoles().subscribe(result => {
      this.roles = result as any[];
    }, error => {
        console.error(error);
    });
  }

  private createUser() {
    let body = {
        Username: this.userForm.value.username,
        Password: this.userForm.value.password,
        Name: this.userForm.value.name,
        Surname: this.userForm.value.surname,
        RoleId: this.userForm.value.role,
        SubjectIds: this.userForm.value.subjects,
    }
    this.usersService.createUser(body).subscribe(result => {
      this.toastr.success("User created");
      this.router.navigate([`/users`]);
    }, error => {
        console.error(error);
    });
  }

  private editUser() {
    let body = {
      User: {
        Id: this.userId,
        Username: this.userForm.value.username,
        Password: this.userForm.value.password,
        Name: this.userForm.value.name,
        Surname: this.userForm.value.surname,
        RoleId: this.userForm.value.role,
        SubjectIds: this.userForm.value.subjects,
      }
  }
    this.usersService.editUser(body).toPromise()
    .then(result => {
      this.toastr.success("User updated");
    })
    .catch(
      error => {
        this.toastr.error();
        console.error(error);
      });
  }

  deleteUser() {
    this.usersService.deleteUser(this.userId).toPromise()
    .then(result => {
      this.toastr.success("User deleted");
      this.router.navigate([`/users`]);
    })
    .catch(
      error => {
        this.toastr.error();
        console.error(error);
      });
  }

  onFirstSubmit() {
    if (this.userId == 0) {
      this.createUser();
    }
    else {
      this.editUser();
    }
  }
}
