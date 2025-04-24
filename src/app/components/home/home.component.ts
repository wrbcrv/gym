import { Component, OnInit } from '@angular/core';
import { GroupService } from '../../services/group.service';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-home',
  imports: [
    CommonModule,
    RouterModule
  ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit {
  groups: any[] = [];

  constructor(
    private groupService: GroupService
  ) { }

  ngOnInit(): void {
    this.getAll();
  }
   
  getAll(): void {
    this.groupService.getAll().subscribe(
      (res) => {
        this.groups = res;
      },
      (err) => {
        console.log(err);
      }
    )
  }
}
