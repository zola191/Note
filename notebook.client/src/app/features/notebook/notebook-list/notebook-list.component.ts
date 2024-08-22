import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { NotebookService } from '../services/notebook.service';
import { Notebook } from '../models/notebook.model';
import { MatDialog } from '@angular/material/dialog';
import { AddNotebookComponent } from '../add-notebook/add-notebook.component';
import { HttpEventType, HttpResponse } from '@angular/common/http';
import { MatSnackBar } from '@angular/material/snack-bar';
import { utils, writeFile } from 'xlsx';
import { AlertModalComponent } from '../alert-modal/alert-modal.component';
import { ErrorModel } from '../models/notebook-error.model';
@Component({
  selector: 'app-notebook-list',
  templateUrl: './notebook-list.component.html',
  styleUrl: './notebook-list.component.css',
})
export class NotebookListComponent implements OnInit {
  notebooks$?: Observable<Notebook[]>;
  currentFile?: File;
  progress = 0;
  message = '';

  fileName = 'Select File';
  fileInfos?: Observable<any>;

  notebooks: Notebook[] = [];

  constructor(
    private notebookService: NotebookService,
    public dialog: MatDialog,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.notebooks$ = this.notebookService.getAll();

    this.notebooks$.subscribe({
      next: (data: Notebook[]) => {
        this.notebooks = data;
      },
      error: (err) => {
        console.error('Failed to load notebooks', err);
      },
    });
  }

  openCreteNoteDialog(): void {
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

    // dialogRef.componentInstance.errors = erorrs;
    // [
    //   {
    //     message: '1',
    //   },
    //   {
    //     message: '2',
    //   },
    //   {
    //     message: '3',
    //   },
    // ];

    dialogRef.afterClosed().subscribe((result) => {
      console.log('The dialog was closed');
    });
  }

  selectFile(event: any): void {
    this.progress = 0;
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
      this.notebookService.upload(this.currentFile).subscribe({
        // next: (res) => {
        //   this.snackBar.open('Файл успешно загружен', 'close', {
        //     duration: 3000,
        //     panelClass: ['snackbar-1'],
        //   });
        // },

        next: (event: any) => {
          console.log(event);
          if (event.type === HttpEventType.UploadProgress) {
            this.progress = Math.round((100 * event.loaded) / event.total);
          } else if (event instanceof HttpResponse) {
            this.message = event.body.message;
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
            let errors: ErrorModel[] = [
              {
                message: 'Можно загрузить только excel',
              },
            ];
            console.log('FormatException');
            this.openImportFromXlDialog(errors);
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

            this.progress = 0;
          }

          // console.log('Errors:', err);
          // this.openImportFromXlDialog(err);

          // if (err.error && err.error.message) {
          //   this.message = err.error.message;
          // } else {
          //   this.message = 'Could not upload the file!';
          // }
        },
        complete: () => {
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
