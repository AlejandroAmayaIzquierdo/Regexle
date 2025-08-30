import { Injectable, signal } from '@angular/core';
import { AuthState } from '../models/Auth';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import { environment } from '../../environments/environment.development';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly apiUrl = `${environment.apiUrl}/Auth`;

  constructor(private http: HttpClient) {}

  public authState = signal<AuthState>({
    isLoggedIn: false,
    isGuest: false,
  });

  get isAuthenticated() {
    return this.authState().isLoggedIn;
  }
  get currentUser() {
    return this.authState().user;
  }

  async login(username: string, password: string): Promise<boolean> {
    try {
      const a = await firstValueFrom(
        this.http.post<{ accessToken: string; refreshToken: string }>(
          '/login',
          {
            username,
            password,
          }
        )
      );

      return true;
    } catch (error) {
      console.error('Login failed', error);
      return false;
    }
  }

  async loginGuest(): Promise<boolean> {
    try {
      const response = await firstValueFrom(
        this.http.post<{ accessToken: string }>(
          `${this.apiUrl}/login/guest`,
          {}
        )
      );

      this.authState.set({
        isLoggedIn: true,
        isGuest: true,
        accessToken: response.accessToken,
      });

      return true;
    } catch (error) {
      console.error('Guest login failed', error);
      this.authState.set({
        isLoggedIn: false,
        isGuest: false,
        accessToken: undefined,
      });
      return false;
    }
  }
}
