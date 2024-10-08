import { Component, Inject, OnInit } from '@angular/core';
import { AdminService } from '../services/admin.service';
import { NoteChangeLog } from '../models/note-change-log';
import { Observable } from 'rxjs';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-note-change-log',
  templateUrl: './note-change-log.component.html',
  styleUrl: './note-change-log.component.css',
})
export class NoteChangeLogComponent implements OnInit {
  changeLog$?: Observable<NoteChangeLog[]>;
  changelog: NoteChangeLog[] = [];
  constructor(
    private adminService: AdminService,
    @Inject(MAT_DIALOG_DATA) public email: string
  ) {}

  ngOnInit(): void {
    this.changeLog$ = this.adminService.getChangeLogs(this.email);
    this.changeLog$.subscribe({
      next: (data) => {
        this.changelog = data;
        console.log(this.changelog);
      },
    });
  }
}
