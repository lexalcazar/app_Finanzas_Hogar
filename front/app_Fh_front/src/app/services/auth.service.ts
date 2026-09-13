import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { tap } from 'rxjs';
import { environment } from '../../environments/environment';
import { LoginRequest } from '../models/login-request';
import { LoginResponse } from '../models/login-response';
import { RegisterRequest } from '../models/register-request';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private static readonly tokenKey = 'fh.auth-token';
  private readonly http = inject(HttpClient);

  login(credentials: LoginRequest) {
    return this.http.post<LoginResponse>(`${environment.apiUrl}/Auth/login`, credentials).pipe(
      tap(({ token }) => this.saveToken(token)),
    );
  }

  register(request: RegisterRequest) {
    return this.http.post(`${environment.apiUrl}/Auth/register`, request);
  }

  getToken(): string | null {
    return sessionStorage.getItem(AuthService.tokenKey);
  }

  clearToken(): void {
    sessionStorage.removeItem(AuthService.tokenKey);
  }

  private saveToken(token: string): void {
    sessionStorage.setItem(AuthService.tokenKey, token);
  }
}
