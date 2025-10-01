import { Injectable, signal } from '@angular/core';
import { AuthState } from '../models/Auth';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import { environment } from '../../environments/environment.development';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly apiUrl = `${environment.apiUrl}/Auth`;

  constructor(private http: HttpClient, private router: Router) {}

  public authState = signal<AuthState>({
    isLoggedIn: false,
  });

  get isAuthenticated() {
    return this.authState().isLoggedIn;
  }
  get isGuest() {
    const alreadyGuestGenerated = localStorage.getItem('GUEST_GENERATED');
    return alreadyGuestGenerated === 'true' && this.isAuthenticated;
  }
  get currentUser() {
    return this.authState().user;
  }

  async login(username: string, password: string): Promise<boolean> {
    try {
      const response = await firstValueFrom(
        this.http.post<{ accessToken: string; refreshToken: string }>(
          `${this.apiUrl}/login`,
          {
            username,
            password,
          }
        )
      );

      this.authState.set({
        isLoggedIn: true,
        accessToken: response.accessToken,
        refreshToken: response.refreshToken,
      });

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
        accessToken: response.accessToken,
      });

      return true;
    } catch (error) {
      console.error('Guest login failed', error);
      this.authState.set({
        isLoggedIn: false,
        accessToken: undefined,
      });
      return false;
    }
  }

  async register(username: string, password: string): Promise<boolean> {
    try {
      const response = await firstValueFrom(
        this.http.post<{ accessToken: string; refreshToken: string }>(
          '/register',
          {
            username,
            password,
          }
        )
      );
    } catch (error) {
      console.error('Registration failed', error);
    }
    return false;
  }

  get accessToken() {
    return this.authState().accessToken;
  }
  get refreshToken() {
    return this.authState().refreshToken;
  }

  setToken(
    tokens: { accessToken?: string; refreshToken?: string },
    isGuest?: boolean
  ) {
    if (tokens.accessToken) {
      localStorage.setItem('ACCESS_TOKEN', tokens.accessToken);
      if (isGuest) localStorage.setItem('GUEST_GENERATED', 'true');
    } else if (tokens.refreshToken) {
      localStorage.setItem('ACCESS_TOKEN', tokens.refreshToken);
    } else {
      localStorage.removeItem('ACCESS_TOKEN');
    }
    this.authState.set({
      isLoggedIn: !!tokens.accessToken,
      accessToken: tokens.accessToken,
      refreshToken: tokens.refreshToken,
    });
  }

  setUnauthorized() {
    const prev = this.authState();
    this.authState.set({
      ...prev,
      isLoggedIn: false,
      accessToken: undefined,
      user: undefined,
    });
    this.router.navigate(['/login']);
  }
}
