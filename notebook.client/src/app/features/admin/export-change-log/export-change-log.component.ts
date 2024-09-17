import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Observable } from 'rxjs';
import { NoteChangeLog } from '../models/note-change-log';
import { AdminService } from '../services/admin.service';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { LogFileByPeriod } from '../models/log-file-by-period';
import {
  MAT_MOMENT_DATE_ADAPTER_OPTIONS,
  MomentDateAdapter,
} from '@angular/material-moment-adapter';
import { DateAdapter } from '@angular/material/core';
import moment from 'moment';

@Component({
  selector: 'app-export-change-log',
  templateUrl: './export-change-log.component.html',
  styleUrl: './export-change-log.component.css',
  providers: [
    { provide: MAT_MOMENT_DATE_ADAPTER_OPTIONS, useValue: { useUtc: true } },
  ],
})
export class ExportChangeLogComponent implements OnInit {
  dateRangeForm!: FormGroup;
  changeLog$?: Observable<NoteChangeLog[]>;
  changeLog: NoteChangeLog[] = [];

  constructor(
    private snackBar: MatSnackBar,
    private fb: FormBuilder,
    private adminService: AdminService,
    @Inject(MAT_DIALOG_DATA) public email: string,
    private dateAdapter: DateAdapter<any>
  ) {}

  ngOnInit() {
    this.dateRangeForm = this.fb.group({
      dateRange: this.fb.group({
        start: [null, Validators.required],
        end: [null, Validators.required],
      }),
    });
  }

  async export() {
    const { start, end } = this.dateRangeForm.value.dateRange;

    if (start && end) {
      // Создаем локальные даты без преобразования в UTC
      const startDateUtc = moment(start)
        .startOf('day')
        .format('YYYY-MM-DDTHH:mm:ss.SSS[Z]');
      const endDateUtc = moment(end)
        .endOf('day')
        .format('YYYY-MM-DDTHH:mm:ss.SSS[Z]');

      console.log('Start Date UTC:', startDateUtc);
      console.log('End Date UTC:', endDateUtc);

      // Формируем объект для отправки на backend
      const logFileByPeriod: LogFileByPeriod = {
        email: this.email,
        startDate: startDateUtc,
        endDate: endDateUtc,
      };

      try {
        const blob = await this.adminService
          .getExcelFileLogs(logFileByPeriod)
          .toPromise();

        if (!blob) {
          this.snackBar.open(
            'Не удалось загрузить файл, пустой ответ от сервера',
            'close',
            {
              duration: 3000,
              panelClass: ['snackbar-1'],
            }
          );
          return;
        }

        const startDateForFileName = moment(start).format('YYYY-MM-DD');
        const endDateForFileName = moment(end).format('YYYY-MM-DD');

        const fileName = window.prompt(
          'Введите имя файла для экспорта',
          `changeLog_${startDateForFileName}_to_${endDateForFileName}.xlsx`
        );

        if (!fileName) {
          this.snackBar.open(
            'Имя файла не введено, операция отменена',
            'close',
            {
              duration: 3000,
              panelClass: ['snackbar-1'],
            }
          );
          return;
        }

        this.downloadExcelFile(blob, fileName);
      } catch (error) {
        console.error('Ошибка при получении файла:', error);
        this.snackBar.open('Не удалось загрузить файл', 'close', {
          duration: 3000,
          panelClass: ['snackbar-1'],
        });
      }
    }
  }

  private downloadExcelFile(blob: Blob, fileName: string): void {
    const tempExcel = document.createElement('a');
    const objectUrl = URL.createObjectURL(blob);
    tempExcel.href = objectUrl;
    tempExcel.download = `${fileName}.xlsx`;
    tempExcel.click();
    URL.revokeObjectURL(objectUrl);
  }
}
