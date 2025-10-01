import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth-service';
import { Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { toast } from 'ngx-sonner';

@Component({
  selector: 'app-login',
  imports: [FormsModule, RouterLink],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login implements OnInit {
  constructor(private authService: AuthService, private router: Router) {}

  public userName: string = '';
  public password: string = '';

  ngOnInit(): void {
    if (this.authService.isAuthenticated) {
      this.router.navigate(['/']);
    }
  }

  async onSubmit(e: Event) {
    e.preventDefault();
    const success = await this.authService.login(this.userName, this.password);
    this.authService.setToken(
      {
        accessToken: this.authService.accessToken,
        refreshToken: this.authService.refreshToken,
      },
      false
    );

    if (success) {
      this.router.navigate(['/']);
    } else {
      toast.error('Login failed. Please check your credentials.');
    }
  }
}
