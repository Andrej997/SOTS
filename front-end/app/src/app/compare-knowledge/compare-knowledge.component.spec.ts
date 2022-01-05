import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CompareKnowledgeComponent } from './compare-knowledge.component';

describe('CompareKnowledgeComponent', () => {
  let component: CompareKnowledgeComponent;
  let fixture: ComponentFixture<CompareKnowledgeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CompareKnowledgeComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CompareKnowledgeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
