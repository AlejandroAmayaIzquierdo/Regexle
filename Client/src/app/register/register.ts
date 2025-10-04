import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../services/auth-service';
import { toast } from 'ngx-sonner';

@Component({
  selector: 'app-register',
  imports: [FormsModule, RouterLink],
  templateUrl: './register.html',
  styleUrl: './register.css',
})
export class Register {
  constructor(private authService: AuthService, private router: Router) {}

  public userName: string = '';
  public password: string = '';
  public email: string = '';

  public async onSubmit(event: Event) {
    event.preventDefault();
    const { success, message } = await this.authService.register(
      this.email,
      this.userName,
      this.password
    );

    if (success) {
      this.router.navigate(['/login']);
      toast.success('Registration successful! Please log in.');
    } else {
      toast.error(message ?? 'Registration failed. Please try again.');
    }
  }
}
