import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-cabinet',
  templateUrl: './cabinet.component.html',
  styleUrl: './cabinet.component.css',
})
export class CabinetComponent implements OnInit {
  constructor(private readonly autthService: AuthService) {}

  ngOnInit(): void {
    throw new Error('Method not implemented.');
  }
}
