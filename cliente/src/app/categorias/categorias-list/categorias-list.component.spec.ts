import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CategoriasListComponent } from './categorias-list.component';

describe('CategoriasListComponent', () => {
  let component: CategoriasListComponent;
  let fixture: ComponentFixture<CategoriasListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CategoriasListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CategoriasListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
