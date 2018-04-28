import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PropostaformComponent } from './propostaform.component';

describe('PropostaformComponent', () => {
  let component: PropostaformComponent;
  let fixture: ComponentFixture<PropostaformComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PropostaformComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PropostaformComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
