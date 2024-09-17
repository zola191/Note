import { Component, OnInit } from '@angular/core';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { NotebookService } from '../services/notebook.service';
import { Notebook } from '../models/notebook.model';
import { MatDialog } from '@angular/material/dialog';
import { AddNotebookComponent } from '../add-notebook/add-notebook.component';
import { HttpEventType, HttpResponse } from '@angular/common/http';
import { MatSnackBar } from '@angular/material/snack-bar';
import { utils, writeFile } from 'xlsx';
import { AlertModalComponent } from '../alert-modal/alert-modal.component';
import { ErrorModel } from '../models/notebook-error.model';
import { ChangeDetectorRef } from '@angular/core';
@Component({
  selector: 'app-notebook-list',
  templateUrl: './notebook-list.component.html',
  styleUrl: './notebook-list.component.css',
})
export class NotebookListComponent implements OnInit {
  isLoading: boolean = false;

  notebooks$?: Observable<Notebook[]>;
  currentFile?: File;
  message = '';
  fileName = 'Select File';
  fileInfos?: Observable<any>;

  notebooks: Notebook[] = [];

  constructor(
    private notebookService: NotebookService,
    public dialog: MatDialog,
    private snackBar: MatSnackBar,
    private cd: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.notebooks$ = this.notebookService.getAll();

    this.notebooks$.subscribe({
      next: (data: Notebook[]) => {
        this.notebooks = data;
      },
      error: (err) => {
        console.error('Ошибка закгрузки note', err);
      },
    });
  }

  openCreateNoteDialog(): void {
    const dialogRef = this.dialog.open(AddNotebookComponent, {
      height: '800px',
      width: '600px',
    });

    dialogRef.afterClosed().subscribe((result) => {
      console.log('The dialog was closed');
    });
  }

  openImportFromXlDialog(erorrs: ErrorModel[]): void {
    const dialogRef = this.dialog.open(AlertModalComponent, {
      height: '800px',
      width: '600px',
      data: erorrs,
    });

    dialogRef.afterClosed().subscribe((result) => {
      console.log('The dialog was closed');
    });
  }

  selectFile(event: any): void {
    this.message = '';

    if (event.target.files && event.target.files[0]) {
      const file: File = event.target.files[0];
      this.currentFile = file;
      this.fileName = this.currentFile.name;
    } else {
      this.fileName = 'Select File';
    }
  }

  upload(): void {
    if (this.currentFile) {
      this.isLoading = true;
      this.notebookService.upload(this.currentFile).subscribe({
        next: (newNotebooks: Notebook[]) => {
          console.log(event);
          if (newNotebooks && newNotebooks.length > 0) {
            console.log('Новые записи пришли', newNotebooks);
            console.log('старые записи ', this.notebooks);
            const oldNotebooks = this.notebooks$?.subscribe({
              next: (data: Notebook[]) => {
                this.notebooks = data;
              },
            });
            const updateNotebooks = [...this.notebooks, ...newNotebooks];
            this.notebooks$ = of(updateNotebooks);
            this.cd.detectChanges;
            this.fileInfos = this.notebookService.getFiles();
          }
          this.snackBar.open('Файл успешно загружен', 'close', {
            duration: 3000,
            panelClass: ['snackbar-1'],
          });
        },
        error: (err: any) => {
          if (err.status === 402) {
            console.log('FileNotFoundException');
          } else if (err.status === 403) {
            console.log('FormatException');
            this.snackBar.open('Можно загрузить только excel', 'close', {
              duration: 3000,
              panelClass: ['snackbar-1'],
            });
          } else if (err.status === 400) {
            console.log(err);
            const errors = this.extractErrors(err);
            console.log('Errors:', errors);
            this.openImportFromXlDialog(errors);
          } else {
            this.snackBar.open('something going wrong', 'close', {
              duration: 3000,
              panelClass: ['snackbar-1'],
            });
          }
          this.isLoading = false;
        },
        complete: () => {
          this.isLoading = false;
          this.currentFile = undefined;
        },
      });
    }
  }

  extractErrors(err: any): ErrorModel[] {
    const errorModels: ErrorModel[] = [];

    // Проверяем, существует ли поле `error` и является ли оно массивом
    if (err.error && Array.isArray(err.error)) {
      err.error.forEach((errorItem: any) => {
        // Добавляем каждый элемент в массив `ErrorModel`
        if (errorItem.message) {
          errorModels.push({ message: errorItem.message });
        }
      });
    } else {
      // Если ошибка не является массивом, добавляем общее сообщение
      errorModels.push({ message: 'Произошла неизвестная ошибка' });
    }

    return errorModels;
  }

  exportToExcel() {
    const heading = [
      [
        'FirstName',
        'MiddleName',
        'LastName',
        'PhoneNumber',
        'Country',
        'BirthDay',
        'Organization',
        'Position',
        'Other',
      ],
    ];
    const wb = utils.book_new();
    const ws = utils.json_to_sheet([]);
    utils.sheet_add_aoa(ws, heading);
    utils.sheet_add_json(
      ws,
      this.notebooks.map(({ id, ...rest }) => rest)
    );
    utils.book_append_sheet(wb, ws, 'notes');
    writeFile(wb, 'notes.xlsx');
  }
}
