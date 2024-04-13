import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditNotebookComponent } from './edit-notebook.component';

describe('EditNotebookComponent', () => {
  let component: EditNotebookComponent;
  let fixture: ComponentFixture<EditNotebookComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [EditNotebookComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(EditNotebookComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
