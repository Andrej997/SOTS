import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
@Injectable({
  providedIn: 'root'
})
export class UsersService {

  constructor(private http: HttpClient) { }

  getUsers() {
    return this.http.get(environment.api + `Users`);
  }

  getUser(userId: number) {
    return this.http.get(environment.api + `Users/` + userId);
  }

  getRoles() {
    return this.http.get(environment.api + `Users/roles`);
  }

  createUser(body: any) {
    return this.http.post(environment.api + `Users/create`, body);
  }

  editUser(body: any) {
    return this.http.put(environment.api + `Users/edit`, body);
  }

  deleteUser(userId: number) {
    return this.http.delete(environment.api + `Users/delete/${userId}`);
  }
}