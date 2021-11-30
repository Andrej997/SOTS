import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RealKnowlageGraphComponent } from './real-knowlage-graph.component';

describe('RealKnowlageGraphComponent', () => {
  let component: RealKnowlageGraphComponent;
  let fixture: ComponentFixture<RealKnowlageGraphComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RealKnowlageGraphComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RealKnowlageGraphComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
