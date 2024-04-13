import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NotebookListComponent } from './notebook-list.component';

describe('NotebookListComponent', () => {
  let component: NotebookListComponent;
  let fixture: ComponentFixture<NotebookListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [NotebookListComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(NotebookListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
