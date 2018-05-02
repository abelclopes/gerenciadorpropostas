import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FornecedoresEditarComponent } from './fornecedores-editar.component';

describe('FornecedoresEditarComponent', () => {
  let component: FornecedoresEditarComponent;
  let fixture: ComponentFixture<FornecedoresEditarComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FornecedoresEditarComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FornecedoresEditarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
