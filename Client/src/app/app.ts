import { Component, effect, OnInit, signal } from '@angular/core';
import { RouterOutlet, Router } from '@angular/router';
import { toast, NgxSonnerToaster } from 'ngx-sonner';
import { AuthService } from './services/auth-service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, NgxSonnerToaster],
  standalone: true,
  templateUrl: './app.html',
})
export class App implements OnInit {
  protected readonly title = signal('daily-regex');
  protected readonly toast = toast;

  private loading = signal(true);

  constructor(public authService: AuthService, private router: Router) {}

  async ngOnInit() {
    const token = localStorage.getItem('ACCESS_TOKEN');
    const alreadyGuestGenerated = localStorage.getItem('GUEST_GENERATED');

    // si tiene token se lo asignas al servicio
    if (token) {
      this.authService.setToken(token);
    } else if (!alreadyGuestGenerated) {
      // Primera vez → guest
      const ok = await this.authService.loginGuest();
      if (ok) this.authService.setToken(this.authService.accessToken!, true);
    } else {
      // Ya fue guest → forzar login
      this.authService.setToken(undefined);
      toast.info('Please log in or register to continue');
      this.router.navigate(['/login']);
    }
    this.loading.set(false);
  }
}
