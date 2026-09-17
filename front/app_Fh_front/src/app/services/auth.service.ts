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
  private static readonly nameClaim = 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name';
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

  getDisplayName(): string | null {
    const token = this.getToken();
    if (!token) return null;

    try {
      const payload = token.split('.')[1];
      if (!payload) return null;
      const base64 = payload.replace(/-/g, '+').replace(/_/g, '/');
      const bytes = Uint8Array.from(atob(base64), (character) => character.charCodeAt(0));
      const value = JSON.parse(new TextDecoder().decode(bytes))[AuthService.nameClaim];
      return typeof value === 'string' && value.trim() ? value : null;
    } catch {
      return null;
    }
  }

  logout(): void {
    this.clearToken();
  }

  private saveToken(token: string): void {
    sessionStorage.setItem(AuthService.tokenKey, token);
  }
}
