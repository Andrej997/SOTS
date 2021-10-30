import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AnswersComponent } from './answers/answers.component';
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { QuestionsComponent } from './questions/questions.component';
import { NewTestComponent } from './tests/new-test/new-test.component';
import { TestsComponent } from './tests/tests.component';

const routes: Routes = [
  {
    path: '',
    component: HomeComponent,
    pathMatch: 'full',
  },
  {
    path: 'tests',
    component: TestsComponent,
    pathMatch: 'full',
  },
  {
    path: 'new-test',
    component: NewTestComponent,
    pathMatch: 'full',
  },
  {
    path: 'test/:id/questions',
    component: QuestionsComponent,
    pathMatch: 'full',
  }
  ,
  {
    path: 'test/:t_id/question/:q_id/answers',
    component: AnswersComponent,
    pathMatch: 'full',
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
