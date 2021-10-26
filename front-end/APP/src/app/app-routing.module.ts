import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { TestsComponent } from './tests/tests.component';

const routes: Routes = [
  {
    path: '',
    component: HomeComponent,
    pathMatch: 'full',
    // children: [
    //   {
    //     path: 'tests',
    //     component: TestsComponent,
    //     // canActivate: [],
    //   },
    // ]
  },
  {
    path: 'tests',
    component: TestsComponent,
    pathMatch: 'full',
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
