import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AnswersComponent } from './answers/answers.component';
import { AppComponent } from './app.component';
import { AuthGuard } from './guards/auth.guard';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { QuestionsComponent } from './questions/questions.component';
import { NewTestComponent } from './tests/new-test/new-test.component';
import { TakeTestComponent } from './tests/take-test/take-test.component';
import { TestsComponent } from './tests/tests.component';

const routes: Routes = [
  {
    path: '',
    component: HomeComponent,
    pathMatch: 'full',
  },
  {
    path: 'login',
    component: LoginComponent,
    pathMatch: 'full',
  },
  {
    path: 'tests',
    component: TestsComponent,
    pathMatch: 'full',
    canActivate: [AuthGuard]
  },
  {
    path: 'new-test',
    component: NewTestComponent,
    pathMatch: 'full',
    canActivate: [AuthGuard]
  },
  {
    path: 'take-test/:t_id',
    component: TakeTestComponent,
    pathMatch: 'full',
    canActivate: [AuthGuard]
  },
  {
    path: 'test/:id/questions',
    component: QuestionsComponent,
    pathMatch: 'full',
    canActivate: [AuthGuard]
  }
  ,
  {
    path: 'test/:t_id/question/:q_id/answers',
    component: AnswersComponent,
    pathMatch: 'full',
    canActivate: [AuthGuard]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
