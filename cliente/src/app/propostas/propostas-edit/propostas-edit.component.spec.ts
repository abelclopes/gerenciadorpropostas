import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PropostasEditComponent } from './propostas-edit.component';

describe('PropostasEditComponent', () => {
  let component: PropostasEditComponent;
  let fixture: ComponentFixture<PropostasEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PropostasEditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PropostasEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
