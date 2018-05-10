import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PropostasListComponent } from './propostas-list.component';

describe('PropostasListComponent', () => {
  let component: PropostasListComponent;
  let fixture: ComponentFixture<PropostasListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PropostasListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PropostasListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
