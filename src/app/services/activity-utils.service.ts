import { Injectable } from '@angular/core';

export interface Activity {
  id: number;
  title: string;
  description: string;
  date: string;
  startTime: string;
  endTime: string;
  userFullName: string;
  [key: string]: any;
}

export interface ActivityGroup {
  date: string;
  displayDate: string;
  activities: Activity[];
}

@Injectable({
  providedIn: 'root'
})
export class ActivityUtilsService {

  groupActivitiesByDate(activities: Activity[]): ActivityGroup[] {
    const groupedMap = new Map<string, Activity[]>();

    activities.forEach(activity => {
      const groupKey = this.parseDate(activity.date).toISOString().split('T')[0];
      if (!groupedMap.has(groupKey)) {
        groupedMap.set(groupKey, []);
      }
      groupedMap.get(groupKey)?.push(activity);
    });

    const groups: ActivityGroup[] = [];

    groupedMap.forEach((groupActivities, date) => {
      groups.push({
        date: date,
        displayDate: this.formatDisplayDate(date),
        activities: groupActivities
      });
    });

    return groups;
  }

  parseDate(dateStr: string, timeStr?: string): Date {
    if (dateStr.includes('/')) {
      const [day, month, year] = dateStr.split('/').map(Number);
      const date = new Date(year, month - 1, day);
      if (timeStr) {
        const [hours, minutes] = timeStr.split(':').map(Number);
        date.setHours(hours, minutes, 0, 0);
      }
      return date;
    } else if (dateStr.includes('-')) {
      const [year, month, day] = dateStr.split('-').map(Number);
      const date = new Date(year, month - 1, day);
      if (timeStr) {
        const [hours, minutes] = timeStr.split(':').map(Number);
        date.setHours(hours, minutes, 0, 0);
      }
      return date;
    }

    return new Date(dateStr);
  }

  formatDisplayDate(dateStr: string): string {
    const today = new Date();
    today.setHours(0, 0, 0, 0);
  
    const targetDate = this.parseDate(dateStr);
    targetDate.setHours(0, 0, 0, 0);
  
    const diffInDays = Math.round(
      (targetDate.getTime() - today.getTime()) / (1000 * 60 * 60 * 24)
    );
  
    if (Math.abs(diffInDays) <= 1) {
      const rtf = new Intl.RelativeTimeFormat(navigator.language, { numeric: 'auto' });
      return rtf.format(diffInDays, 'day');
    }
  
    const formatter = new Intl.DateTimeFormat(navigator.language, {
      weekday: 'short',
      day: 'numeric',
      month: 'long'
    });
  
    return formatter.format(targetDate);
  }  
}
