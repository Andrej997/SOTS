import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http: HttpClient) { }

  login(username: string, password: string) {
    let body = {
        username: username,
        password: password
    };
    return this.http.post(environment.api + `Users/login`, body).subscribe(result => {
        this.setData(result);
      }, error => {
          console.error(error);
      });
  }

  logout() {
    this.removeData('user');
  }

  setData(data: any) {
    const jsonData = JSON.stringify(data)
    localStorage.setItem('user', jsonData)
  }
  
  getData() {
      return localStorage.getItem('user')
  }
  
  private removeData(key: any) {
      localStorage.removeItem(key)
  }
}