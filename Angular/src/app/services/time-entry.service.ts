import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map, shareReplay } from 'rxjs/operators';

export interface RawTimeEntry {
  EmployeeName: string | null;
  StarTimeUtc: string;
  EndTimeUtc: string;
  DeletedOn: string | null;
}

export interface TimeEntry {
  name: string;
  totalTimeInMonth: number;
}

@Injectable({
  providedIn: 'root'
})
export class TimeEntryService {
  private apiUrl = 'https://rc-vault-fap-live-1.azurewebsites.net/api/gettimeentries?code=vO17RnE8vuzXzPJo5eaLLjXjmRW07law99QTD90zat9FfOQJKKUcgQ==';

  constructor(private http: HttpClient) {}

  getTimeEntries(): Observable<TimeEntry[]> {
  return this.http.get<RawTimeEntry[]>(this.apiUrl).pipe(
    map(entries => {
      const grouped = new Map<string, number>();

      for (const entry of entries) {
        const start = new Date(entry.StarTimeUtc);
        const end = new Date(entry.EndTimeUtc);

        const durationInHours = (end.getTime() - start.getTime()) / (1000 * 60 * 60);

        if(entry.EmployeeName === null) {
          entry.EmployeeName = "Unnamed Employees";
        }

        if (!grouped.has(entry.EmployeeName)) {
          grouped.set(entry.EmployeeName, 0);
        }

        grouped.set(entry.EmployeeName, grouped.get(entry.EmployeeName)! + durationInHours);
      }

      const result: TimeEntry[] = [];

      grouped.forEach((hours, name) => {
        result.push({
          name,
          totalTimeInMonth: Math.round(hours)
        });
      });
      return result.sort((a, b) => b.totalTimeInMonth - a.totalTimeInMonth);
    }),
    shareReplay(1) 
  );
}

}
