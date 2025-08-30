import { Component, effect, OnInit, signal } from '@angular/core';
import { ChallengeCardComponent } from '../shared/components/challenge-card/challenge-card';
import { Challenges } from '../services/challenges';
import { ChallengeCard } from '../models/challenge';
import { toast } from 'ngx-sonner';
import { AuthService } from '../services/auth-service';

@Component({
  selector: 'app-home',
  imports: [ChallengeCardComponent],
  templateUrl: './home.html',
  styleUrl: './home.css',
})
export class Home implements OnInit {
  constructor(public challenges: Challenges, public authService: AuthService) {}

  loadingChallenge = signal(false);
  loadingSubmit = signal(false);

  challenge = signal<ChallengeCard | undefined>(undefined);

  async ngOnInit() {
    const accessToken = localStorage.getItem('ACCESS_TOKEN');

    console.log('Access Token from storage:', accessToken);

    if (!accessToken) {
      toast.error('No access token found. Logging in with guest access.');
      const loggingSuccessful = await this.authService.loginGuest();
      if (loggingSuccessful) {
        localStorage.setItem(
          'ACCESS_TOKEN',
          this.authService.authState().accessToken!
        );
      }
    } else {
      this.authService.authState.set({
        isLoggedIn: true,
        isGuest: true,
        accessToken: accessToken,
      });
    }

    this.getDailyChallenge();
  }

  public async getDailyChallenge() {
    this.loadingChallenge.set(true);
    const challenge = await this.challenges.getDailyChallenge();
    this.challenge.set(challenge);
    this.loadingChallenge.set(false);
  }

  public async submitUserInput(input: string) {
    this.loadingSubmit.set(true);
    const result = await this.challenges.submitUserInput(input);

    if (result) {
      toast.success('Input submitted successfully!');
    } else {
      toast.error('Failed to submit input.');
    }
    this.loadingSubmit.set(false);
  }
}
