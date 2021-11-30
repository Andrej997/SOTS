import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ExpectedKnowlageGraphComponent } from './expected-knowlage-graph.component';

describe('ExpectedKnowlageGraphComponent', () => {
  let component: ExpectedKnowlageGraphComponent;
  let fixture: ComponentFixture<ExpectedKnowlageGraphComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ExpectedKnowlageGraphComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ExpectedKnowlageGraphComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
