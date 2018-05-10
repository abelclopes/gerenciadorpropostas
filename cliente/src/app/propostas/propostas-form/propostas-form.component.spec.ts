import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PropostasFormComponent } from './propostas-form.component';

describe('PropostasFormComponent', () => {
  let component: PropostasFormComponent;
  let fixture: ComponentFixture<PropostasFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PropostasFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PropostasFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
