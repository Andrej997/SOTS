import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private authService: AuthService) {

  }
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
      if (this.authService.getData() == null){
        return false;
      }
      else {
        return true;
      }
  }

  isStudent(): boolean {
    let user = this.authService.getData();
    if (user.roleId == 3) 
      return true;
    return false;
  }

  getId(): number {
    let user = this.authService.getData();
    return user.id;
  }
  
  isLoggedIn() {
    if (this.authService.getData() == null){
      return false;
    }
    else {
      return true;
    }
  }
}
