import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UsersService } from '../services/users.services';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit {

  data: any[] = [];
  users: any[] = [];
  roles: any[] = [];

  constructor(private usersService: UsersService, private router: Router,) { }

  ngOnInit(): void {
    this.getUsers();
  }

  private getUsers() {
    this.usersService.getUsers().subscribe(result => {
      this.users = result as any[];
      this.data = this.users;
      console.log(this.users);
    }, error => {
        console.error(error);
    });
  }

  onUserRowSelect(event: any) {
    this.router.navigate([`/users/${event.data.id}`]);
  }

  settings = {
    actions: false,
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
      username: {
        title: 'Username'
      },
      name: {
        title: 'Name'
      },
      surname: {
        title: 'Surname'
      },
      role: {
        title: 'Role'
      }
    }
  };

}
