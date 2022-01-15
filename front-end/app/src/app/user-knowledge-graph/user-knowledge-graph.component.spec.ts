import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserKnowledgeGraphComponent } from './user-knowledge-graph.component';

describe('UserKnowledgeGraphComponent', () => {
  let component: UserKnowledgeGraphComponent;
  let fixture: ComponentFixture<UserKnowledgeGraphComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UserKnowledgeGraphComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UserKnowledgeGraphComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
