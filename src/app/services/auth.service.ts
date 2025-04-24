import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

interface LoginDto {
  userName: string;
  password: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl: string = 'http://localhost:5000/api/auth';

  constructor(private http: HttpClient) { }

  login(req: LoginDto) {
    return this.http.post(`${this.apiUrl}/login`, req);
  }

  logout() {
    return this.http.post(`${this.apiUrl}/logout`, {});
  }

  getCurrentUser(): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/me`, { withCredentials: true });
  }
}
