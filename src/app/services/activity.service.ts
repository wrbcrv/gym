import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class ActivityService {
  private apiUrl: string = 'http://localhost:5000/api/groups';

  constructor(private http: HttpClient) {}

  getById(groupId: number, activityId: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${groupId}/activities/${activityId}`);
  }

  getAll(groupId: number, pageNumber: number, pageSize: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/${groupId}/activities?pageNumber=${pageNumber}&pageSize=${pageSize}`);
  }  
  
  create(groupId: number, activity: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/${groupId}/activities`, activity);
  }

  getRanking(groupId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/${groupId}/activities/ranking`);
  }
}
