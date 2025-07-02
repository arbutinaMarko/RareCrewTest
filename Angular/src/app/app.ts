import { Component } from '@angular/core';
import { EmployeeTableComponent } from './components/employee-table/employee-table.component';
import { EmployeeChartComponent } from './components/employee-chart/employee-chart.component';

@Component({
  selector: 'app-root',
  standalone: true,
  templateUrl: './app.html',
  styleUrls: ['./app.css'],
  imports: [
    EmployeeTableComponent,
    EmployeeChartComponent
  ]
})
export class App {}
