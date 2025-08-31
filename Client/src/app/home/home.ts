import { Component, effect, signal } from '@angular/core';
import { ChallengeCardComponent } from '../shared/components/challenge-card/challenge-card';
import { Challenges } from '../services/challenges';
import { ChallengeCard } from '../models/challenge';
import { toast } from 'ngx-sonner';
import { AuthService } from '../services/auth-service';
import { SessionExpired } from '../shared/Errors';

@Component({
  selector: 'app-home',
  imports: [ChallengeCardComponent],
  templateUrl: './home.html',
  styleUrl: './home.css',
})
export class Home {
  constructor(public challenges: Challenges, public authService: AuthService) {
    effect(() => {
      if (this.authService.isAuthenticated) {
        this.getDailyChallenge();
      }
    });
  }

  loadingChallenge = signal(true);
  loadingSubmit = signal(false);

  challenge = signal<ChallengeCard | undefined>(undefined);

  public async getDailyChallenge() {
    this.loadingChallenge.set(true);
    const result = await this.challenges.getDailyChallenge();
    if (result.success) {
      this.challenge.set(result.data);
    } else {
      const error = result.error.code;

      switch (error) {
        case SessionExpired.code:
          toast.error('Your session has expired. Please log in.');
          this.authService.setUnauthorized();
          break;
        default:
          toast.error('Failed to load challenge');
          break;
      }
    }
    this.loadingChallenge.set(false);
  }

  public async submitUserInput(input: string) {
    this.loadingSubmit.set(true);
    const result = await this.challenges.submitUserInput(input);

    console.log(result);

    if (result.success) {
      toast.success('Input submitted successfully!');
    } else {
      const error = result.error.code;

      switch (error) {
        case SessionExpired.code:
          toast.error('Your session has expired. Please log in.');
          this.authService.setUnauthorized();
          break;
        default:
          toast.error('Failed to submit input.');
          break;
      }
    }
    this.loadingSubmit.set(false);
  }
}
