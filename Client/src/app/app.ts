import { Component, effect, OnInit, signal } from '@angular/core';
import { RouterOutlet, Router, RouterLink } from '@angular/router';
import { toast, NgxSonnerToaster } from 'ngx-sonner';
import { AuthService } from './services/auth-service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, NgxSonnerToaster, RouterLink],
  standalone: true,
  templateUrl: './app.html',
})
export class App implements OnInit {
  protected readonly title = signal('daily-regex');
  protected readonly toast = toast;

  private loading = signal(true);

  constructor(public authService: AuthService, public router: Router) {}

  async ngOnInit() {
    const token = localStorage.getItem('ACCESS_TOKEN');
    const refreshToken = localStorage.getItem('REFRESH_TOKEN');
    console.log('Token found on init:', token);
    console.log('Refresh token found on init:', refreshToken);
    const alreadyGuestGenerated = localStorage.getItem('GUEST_GENERATED');

    // si tiene token se lo asignas al servicio
    if (token) {
      this.authService.setToken({
        accessToken: token,
        refreshToken: refreshToken ?? undefined,
      });
      const payloadJWT = this.authService.parseToken(token);
      this.authService.setUser({
        id: payloadJWT[
          'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'
        ],
        email:
          payloadJWT[
            'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'
          ],
        userName:
          payloadJWT[
            'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'
          ],
      });
    } else if (!alreadyGuestGenerated) {
      // Primera vez → guest
      const ok = await this.authService.loginGuest();
      if (ok)
        this.authService.setToken(
          { accessToken: this.authService.accessToken! },
          true
        );
    } else {
      // Ya fue guest → forzar login
      this.authService.setToken({});
      toast.info('Please log in or register to continue');
      this.router.navigate(['/login']);
    }
    this.loading.set(false);
  }
}
