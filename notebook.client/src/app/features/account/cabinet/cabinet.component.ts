import { Component, OnInit } from '@angular/core';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-cabinet',
  templateUrl: './cabinet.component.html',
  styleUrl: './cabinet.component.css',
})
export class CabinetComponent implements OnInit {
  constructor(private readonly userService: UserService) {}

  ngOnInit(): void {
    throw new Error('Method not implemented.');
  }
}
