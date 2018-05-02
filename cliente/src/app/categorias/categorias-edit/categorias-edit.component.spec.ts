import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CategoriasEditComponent } from './categorias-edit.component';

describe('CategoriasEditComponent', () => {
  let component: CategoriasEditComponent;
  let fixture: ComponentFixture<CategoriasEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CategoriasEditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CategoriasEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
