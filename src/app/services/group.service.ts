import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class GroupService {
  private apiUrl: string = 'http://localhost:5000/api/groups';

  constructor(private http: HttpClient) { }

  getAll(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }

  getById(id: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${id}`);
  } 

  getActivityById(groupId: number, activityId: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${groupId}/activities/${activityId}`);
  }  

  createActivity(groupId: number, activity: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/${groupId}/activities`, activity);
  }  
}
