import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AnswersComponent } from './answers/answers.component';
import { AppComponent } from './app.component';
import { CompareKnowledgeComponent } from './compare-knowledge/compare-knowledge.component';
import { DomainsComponent } from './domains/domains.component';
import { ExpectedKnowlageGraphComponent } from './expected-knowlage-graph/expected-knowlage-graph.component';
import { GraphComponent } from './graph/graph.component';
import { AuthGuard } from './guards/auth.guard';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { QuestionsComponent } from './questions/questions.component';
import { RealKnowlageGraphComponent } from './real-knowlage-graph/real-knowlage-graph.component';
import { NewTestComponent } from './tests/new-test/new-test.component';
import { ResultsComponent } from './tests/results/results.component';
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
    path: 'graph',
    component: GraphComponent,
    pathMatch: 'full',
    canActivate: [AuthGuard]
  },
  {
    path: 'graph/domain/:d_id',
    component: GraphComponent,
    pathMatch: 'full',
    canActivate: [AuthGuard]
  },
  {
    path: 'expected-knowlage-graph/:t_id',
    component: ExpectedKnowlageGraphComponent,
    pathMatch: 'full',
    canActivate: [AuthGuard]
  },
  {
    path: 'real-knowlage-graph/:t_id',
    component: RealKnowlageGraphComponent,
    pathMatch: 'full',
    canActivate: [AuthGuard]
  },
  {
    path: 'compore-knowledge/:t_id',
    component: CompareKnowledgeComponent,
    pathMatch: 'full',
    canActivate: [AuthGuard]
  },
  {
    path: 'domains',
    component: DomainsComponent,
    pathMatch: 'full',
    canActivate: [AuthGuard]
  },
  {
    path: 'results',
    component: ResultsComponent,
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
  },
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
