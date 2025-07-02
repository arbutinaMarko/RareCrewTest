import { Component, OnInit } from '@angular/core';
import { TimeEntryService, TimeEntry } from '../../services/time-entry.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-employee-table',
  imports: [CommonModule],
  templateUrl: './employee-table.component.html',
  styleUrl: './employee-table.component.css'
})
export class EmployeeTableComponent implements OnInit {
  employees: TimeEntry[] = [];

  constructor(private timeEntryService: TimeEntryService) {}

  ngOnInit(): void {
  this.timeEntryService.getTimeEntries().subscribe(data => {
    this.employees = data;
  });
}
}
