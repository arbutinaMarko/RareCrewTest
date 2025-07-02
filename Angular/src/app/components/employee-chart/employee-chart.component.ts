import { Component } from '@angular/core';
import { BaseChartDirective } from 'ng2-charts';
import { ChartOptions } from 'chart.js';
import ChartDataLabels from 'chartjs-plugin-datalabels';
import { TimeEntryService, TimeEntry } from '../../services/time-entry.service';

@Component({
  selector: 'app-employee-chart',
  imports: [BaseChartDirective],
  templateUrl: './employee-chart.component.html',
  styleUrl: './employee-chart.component.css',
})
export class EmployeeChartComponent {
  public pieChartOptions: ChartOptions<'pie'> = {
    responsive: true,
    layout: {
      padding: {
        top: 20,
        bottom: 20,
      },
    },
    plugins: {
      legend: {
        position: 'bottom'
      },
      tooltip: {
        callbacks: {
          label: (context) => {
            const label = context.label || '';
            const value = context.raw as number;
            const dataset = context.chart.data.datasets[0];
            const total = (dataset.data as number[]).reduce(
              (sum, val) => sum + val,
              0
            );
            const percent = ((value / total) * 100).toFixed(1);
            return `${label}: ${value} (${percent}%)`;
          },
        },
      },
      datalabels: {
        formatter: (value, context) => {
          const dataset = context.chart.data.datasets[0];
          const total = (dataset.data as number[]).reduce(
            (sum, val) => sum + val,
            0
          );
          const percent = ((value / total) * 100).toFixed(1);
          return `${percent}%`;
        },
        color: '#fff',
        font: {
          weight: 'bold',
          size: 14,
        },
      },
    },
  };

  public pieChartLabels: string[] = [];
  public pieChartDatasets: { data: number[]; label?: string }[] = [];
  public pieChartLegend = true;
  public pieChartPlugins = [ChartDataLabels];

  constructor(private timeEntryService: TimeEntryService) {}

  ngOnInit(): void {
    this.timeEntryService.getTimeEntries().subscribe((data) => {
      const employeeHours = data.reduce(
        (acc: { [key: string]: number }, entry: TimeEntry) => {
          acc[entry.name] = (acc[entry.name] || 0) + entry.totalTimeInMonth;
          return acc;
        },
        {} as { [key: string]: number }
      );

      const employeeNames = Object.keys(employeeHours);

      this.pieChartLabels = employeeNames;

      this.pieChartDatasets = [{ data: Object.values(employeeHours) }];
    });
  }
}
